using System;
using System.Runtime.CompilerServices;
using GraduateRecruitment.ConsoleApp.Data;
using System.Collections.Generic;

[assembly: InternalsVisibleTo("GraduateRecruitment.UnitTests")]

namespace GraduateRecruitment.ConsoleApp
{
    //TODO remove
    //TODO var names?
    class GreatestInfo {
                public int[] quantities;
                public int greatestIndex = 0;
                public String greatestName = "";

                public GreatestInfo(int length){
                    quantities = new int[length];

                    for(int i=0;i<length;i++){
                    quantities[i] = 0;
                    }
                }
    }

    internal class Program
    {
        internal static void Main(string[] args)
        {
            var repo = new OpenBarRepository();

            Question1(repo);
            Console.WriteLine(Environment.NewLine);
            Question2(repo);
            Console.WriteLine(Environment.NewLine);
            Question3(repo);
            Console.WriteLine(Environment.NewLine);
            Question4(repo);
            Console.WriteLine(Environment.NewLine);
            Question5(repo);
            Console.WriteLine(Environment.NewLine);
            Question6(repo);
            Console.WriteLine(Environment.NewLine);
            Question7(repo);
            Console.WriteLine(Environment.NewLine);
        }

        private static void Question1(OpenBarRepository repo)
        {
            Console.WriteLine("Question 1: What is the most popular drink, including the quantity, on a Wednesday?");
            
            // Write your answer to the console here.
            // Format e.g.  {inventory name}: {quantity}

            //Declarations
            int amountOfInventory = repo.AllInventory.Count;
            int[] quantitiesTaken = new int[amountOfInventory];

            var greatestIndex = new List<int>();        //Incase of ties
            var greatestName = new List<String>();      //Incase of ties

            //Initialisations
            for(int i=0;i<amountOfInventory;i++){
                quantitiesTaken[i] = 0;
            }

            greatestIndex.Add(0);
            greatestName.Add("");

            foreach( var item in repo.AllOpenBarRecords){

                if(item.DayOfWeek == DayOfWeek.Wednesday)
                {

                    foreach(var stock in item.FridgeStockTakeList){
                        int quantityIndex = stock.Inventory.Id-1;

                        if(stock.Quantity.Taken > 0){ //if quantitiesTaken dont't change then greatest evaluation can be skipped
                            quantitiesTaken[quantityIndex] += stock.Quantity.Taken;

                                if( greatestIndex.Count > 1 && greatestIndex.Contains(quantityIndex)) //if item part of a tie and it's quantity changed then item has to be re evaluated
                                    greatestIndex.Remove(quantityIndex);   

                                if(!greatestIndex.Contains(quantityIndex) && quantitiesTaken[quantityIndex] == quantitiesTaken[greatestIndex[0]]){ //a tie

                                    greatestIndex.Add(quantityIndex);
                                    greatestName.Add(stock.Inventory.Name);

                                } else if(quantitiesTaken[quantityIndex] > quantitiesTaken[greatestIndex[0]]){ //greater

                                    greatestIndex.Clear();
                                    greatestName.Clear();

                                    greatestIndex.Add(quantityIndex);
                                    greatestName.Add(stock.Inventory.Name);

                                }
                        }

                    }

                }   

            }

            for( int i=0; i<greatestIndex.Count; i++){ 
                Console.WriteLine(greatestName[i] + ": " + quantitiesTaken[greatestIndex[i]]);
            }

        }

        private static void Question2(OpenBarRepository repo)
        {
            Console.WriteLine("Question 2: What is the most popular drink, including the quantities, per day?");

            // Write your answer to the console here.
            // Format e.g.  {day of week}
            //              {inventory name}: {quantity}

        }

        /* Assumptions:
            -last recorded does not refer to the las fully recorded month
            -the AllOpenBarRecords-list (thus also assuming the data) is given in ascending date order
            -the stock is taken accurately/correctly/consistently
        */
        private static void Question3(OpenBarRepository repo)
        {
            Console.WriteLine("Question 3: Which dates did we run out of Savanna Dry for the last recorded month?");

            // Write your answer to the console here.
            // Format e.g.  {year}/{month}/{day}

            //Declarations and Initialisations
            int amountOfSD = 0;         //SD := Savanna Dry
            var lastDateRecorded = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;

            foreach( var item in repo.AllOpenBarRecords ){

                foreach(var stock in item.FridgeStockTakeList){

                    if(stock.Inventory.Name.CompareTo("Savanna Dry")==0)
                    {
                        var date = item.Date;
                        
                        amountOfSD += stock.Quantity.Added;
                        amountOfSD -= stock.Quantity.Taken;

                        /*if(amountOfSD < 0)
                        {
                            Console.WriteLine("Error: Stock recorded incorrectly!");
                            return;
                        }*/

                        if(amountOfSD == 0 && date.Month == lastDateRecorded.Month && date.Year == lastDateRecorded.Year){
                            Console.WriteLine(date.Year + "/" + date.Month + "/" + date.Day);
                        }

                    }
                }
            }
        }

        /* Assumptions:
            -the AllOpenBarRecords-list (thus also assuming the data) is given in ascending date order
            -the stock is taken accurately/correctly/consistently
        */
        private static void Question4(OpenBarRepository repo)
        {
            Console.WriteLine("Question 4: How many Fanta Oranges do we need to order next week?");

            // Write your answer to the console here.
            // Format e.g.  {quanity}

            //Declarations and Initialisations
            int amountOfFO = 0;
            int amountOfFOTaken = 0;       
            int weekCount = 0;
            int daysTillSaterday = 0;
            int avgFOUsedPerWeek = 0;
            int amountFOToOrder = 0;
            var currDay = repo.AllOpenBarRecords[0].DayOfWeek;
            
            switch(currDay){
                case DayOfWeek.Monday: 
                    daysTillSaterday = 5;
                break;
                case DayOfWeek.Tuesday: 
                    daysTillSaterday = 4;
                break;
                case DayOfWeek.Wednesday: 
                    daysTillSaterday = 3;
                break;
                case DayOfWeek.Thursday: 
                    daysTillSaterday = 2;
                break;
                case DayOfWeek.Friday: 
                    daysTillSaterday = 1;
                break;
                case DayOfWeek.Saturday: 
                    daysTillSaterday = 0;
                break;
                case DayOfWeek.Sunday: 
                    daysTillSaterday = 6;
                break;
            }

            var currEndOfWeekDate = repo.AllOpenBarRecords[0].Date.AddDays(daysTillSaterday);
            
            foreach( var item in repo.AllOpenBarRecords){

                    if(item.Date>=currEndOfWeekDate){
                        weekCount++;
                        
                        while(currEndOfWeekDate<=item.Date){                    //Incase stock haven't changed or been recorded for a few days/weeks (#Covid)
                            currEndOfWeekDate = currEndOfWeekDate.AddDays(7);
                        }
                    }

                    foreach(var stock in item.FridgeStockTakeList){

                        if(stock.Inventory.Name.CompareTo("Fanta Orange") == 0)
                        {
                            amountOfFOTaken += stock.Quantity.Taken;
                            amountOfFO += stock.Quantity.Added;
                            amountOfFO -= stock.Quantity.Taken;

                            /*if(amountOfFO < 0)
                            {
                                Console.WriteLine("Error: Stock recorded incorrectly!");
                                return;
                            }*/
                        }

                    }   

            }

            avgFOUsedPerWeek = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt(amountOfFOTaken/weekCount); 
            amountFOToOrder = avgFOUsedPerWeek - amountOfFO;

            if(amountFOToOrder<0)
            amountFOToOrder = 0;

            Console.WriteLine(amountFOToOrder);
        }

        /* Assumptions:
            -the AllOpenBarRecords-list (thus also assuming the data) is given in ascending date order
            -the stock is taken accurately/correctly/consistently
        */
        private static void Question5(OpenBarRepository repo)
        {
            Console.WriteLine("Question 5: How much do we need to budget next month for Ceres Orange Juice?");

            // Write your answer to the console here.
            // Format e.g.  R{amount}

            //Declarations and Initialisations
            int avgFOUsedPerMonth = 0;
            int amountCOJToOrder = 0;
            int amountOfCOJ = 0;
            int amountOfCOJTaken = 0;       
            int monthCount = 0;   
            decimal COJCost = 0;
            int currDay = repo.AllOpenBarRecords[0].Date.Day;
            var currEndOfMonthDate = repo.AllOpenBarRecords[0].Date.AddDays(-currDay).AddMonths(1);

            decimal COJPrice = 0;
            foreach (var beverage in repo.AllInventory){
                if(beverage.Name.CompareTo("Ceres Orange Juice")==0)
                    COJPrice = beverage.Price;
            }

            foreach( var item in repo.AllOpenBarRecords){

                    if(item.Date>=currEndOfMonthDate){
                        monthCount++;
                        
                        while(currEndOfMonthDate<=item.Date){                       //Incase stock haven't changed or been recorded for a few days/weeks/months (#Covid)
                            currEndOfMonthDate = currEndOfMonthDate.AddMonths(1);
                        }
                    }

                    foreach(var stock in item.FridgeStockTakeList){

                        if(stock.Inventory.Name.CompareTo("Ceres Orange Juice") == 0)
                        {
                            amountOfCOJTaken += stock.Quantity.Taken;
                            amountOfCOJ += stock.Quantity.Added;
                            amountOfCOJ -= stock.Quantity.Taken;

                            /*if(amountOfCOJ < 0)
                            {
                                Console.WriteLine("Error: Stock recorded incorrectly!");
                                return;
                            }*/
                        }

                    }            
            }

            avgFOUsedPerMonth = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt(amountOfCOJTaken/monthCount); 
            amountCOJToOrder = avgFOUsedPerMonth - amountOfCOJ;

            if(amountCOJToOrder<0)
            amountCOJToOrder = 0;

            COJCost = amountCOJToOrder*COJPrice;

            Console.WriteLine("R" +COJCost);
        }

        /* Assumptions:
            -the AllOpenBarRecords-list (thus also assuming the data) is given in ascending date order
            -the stock is taken accurately/correctly/consistently
            -one does not have to take time of year into consideration when making the estimate
        */
        private static void Question6(OpenBarRepository repo)
        {
            Console.WriteLine("Question 6: How much do we need to budget for next month to restock the fridge?");

            // Write your answer to the console here.
            // Format e.g.  R{amount}

            int amountOfInventory = repo.AllInventory.Count;
            int[] quantities = new int[amountOfInventory];
            int[] quantitiesTaken = new int[amountOfInventory];       
            
            for(int i=0;i<amountOfInventory;i++){
                quantitiesTaken[i] = 0;
            }

            for(int i=0;i<amountOfInventory;i++){
                quantities[i] = 0;
            }

            decimal totalCost = 0;
            int monthCount = 0;
            int avgUsedPerMonth = 0;
            int quanityToOrder = 0;
            int currDay = repo.AllOpenBarRecords[0].Date.Day;
            var currEndOfMonthDate = repo.AllOpenBarRecords[0].Date.AddDays(-currDay).AddMonths(1);
            
            foreach( var item in repo.AllOpenBarRecords){

                    if(item.Date>=currEndOfMonthDate){
                        monthCount++;
                        
                        while(currEndOfMonthDate<=item.Date){                       //Incase stock haven't changed or been recorded for a few days/weeks/months (#Covid)
                            currEndOfMonthDate = currEndOfMonthDate.AddMonths(1);
                        }
                    }

                    foreach(var stock in item.FridgeStockTakeList){
                        int quanitiesIndex = stock.Inventory.Id-1;

                        quantitiesTaken[quanitiesIndex] += stock.Quantity.Taken;
                        quantities[quanitiesIndex] += stock.Quantity.Added;
                        quantities[quanitiesIndex] -= stock.Quantity.Taken;

                        /*if(quantities[quanitiesIndex]<0) {
                            Console.WriteLine("Error: Stock recorded incorrectly!");
                            return;
                        }*/

                    }            
            }

            foreach (var beverage in repo.AllInventory){
                int quanitiesIndex = beverage.Id-1;

                avgUsedPerMonth = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt(quantitiesTaken[quanitiesIndex]/monthCount);
                quanityToOrder = avgUsedPerMonth - quantities[quanitiesIndex];

                if(quanityToOrder<0)
                quanityToOrder = 0;

                totalCost += beverage.Price* quanityToOrder;
            }

            Console.WriteLine("R" +totalCost);
        }

        private static void Question7(OpenBarRepository repo)
        {
            Console.WriteLine("Question 7: We're planning a braai and expecting 100 people, how many of each drink should we order based on historical popularity of drinks?");

            // Write your answer to the console here.
            // Format e.g.  {inventory name}: {quantity}
        }
    }
}