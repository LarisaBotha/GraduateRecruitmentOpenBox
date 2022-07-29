using System;
using System.Runtime.CompilerServices;
using GraduateRecruitment.ConsoleApp.Data;
using GraduateRecruitment.ConsoleApp.Classes;
using System.Collections.Generic;
using System.Collections;

[assembly: InternalsVisibleTo("GraduateRecruitment.UnitTests")]

namespace GraduateRecruitment.ConsoleApp
{
    class GreatestInfo {
                public int[] quantitiesTaken;
                public List<int> greatestIndex = new List<int>();        //Incase of ties
                public List<String> greatestName = new List<String>();   //Incase of ties

                public GreatestInfo(int length){
                    quantitiesTaken = new int[length];

                    for(int i=0;i<length;i++){
                    quantitiesTaken[i] = 0;
                    }

                     greatestIndex.Add(0);
                     greatestName.Add("");
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

        private static int getFirstIndex(Hashtable table){
            ICollection keys = table.Keys;
            int first = 0;
            foreach(string key in keys)
                first = (int) table[key];
            return first;
        }

        private static int getInventoryId(OpenBarRepository repo, String inventoryName){
            foreach( var item in repo.AllInventory){
                if(item.Name.CompareTo(inventoryName) == 0)
                    return item.Id;
            }

            return -1;
        }

        protected internal static void Question1(OpenBarRepository repo)
        {
            Console.WriteLine("Question 1: What is the most popular drink, including the quantity, on a Wednesday?");
            
            // Write your answer to the console here.
            // Format e.g.  {inventory name}: {quantity}

            /*
                getquantitiesTakenOnWednesday
                getTotalWdnesdays
            */

        }

        protected internal static void Question2(OpenBarRepository repo)
        {
            Console.WriteLine("Question 2: What is the most popular drink, including the quantities, per day?");

            // Write your answer to the console here.
            // Format e.g.  {day of week}
            //              {inventory name}: {quantity}
            
            int amountOfInventory = repo.AllInventory.Count;
            List<GreatestInfo> greatestPerDay = new List<GreatestInfo>();
            
            for(int i=0; i<7; i++){
                greatestPerDay.Add(new GreatestInfo(amountOfInventory));
            }

            /*Hashtable dayOfWeekIndex = new Hashtable();
            dayOfWeekIndex.Add(DayOfWeek.Monday,1);
            dayOfWeekIndex.Add(DayOfWeek.Tuesday,2);
            dayOfWeekIndex.Add(DayOfWeek.Wednesday,3);
            dayOfWeekIndex.Add(DayOfWeek.Thursday,4);
            dayOfWeekIndex.Add(DayOfWeek.Friday,5);
            dayOfWeekIndex.Add(DayOfWeek.Saturday,6);
            dayOfWeekIndex.Add(DayOfWeek.Sunday,0);*/

            foreach( var item in repo.AllOpenBarRecords){

                int dayOfWeekIndex=0;
                switch(item.DayOfWeek){
                case DayOfWeek.Monday: 
                    dayOfWeekIndex = 1;
                break;
                case DayOfWeek.Tuesday: 
                    dayOfWeekIndex = 2;
                break;
                case DayOfWeek.Wednesday: 
                    dayOfWeekIndex = 3;
                break;
                case DayOfWeek.Thursday: 
                    dayOfWeekIndex = 4;
                break;
                case DayOfWeek.Friday: 
                    dayOfWeekIndex = 5;
                break;
                case DayOfWeek.Saturday: 
                    dayOfWeekIndex = 6;
                break;
                case DayOfWeek.Sunday: 
                    dayOfWeekIndex = 0;
                break;
                }
                    foreach(var stock in item.FridgeStockTakeList){
                        int quantityIndex = stock.Inventory.Id-1;

                        if(stock.Quantity.Taken > 0){ //if quantitiesTaken dont't change then greatest evaluation can be skipped
                            greatestPerDay[dayOfWeekIndex].quantitiesTaken[quantityIndex] += stock.Quantity.Taken;

                                if( greatestPerDay[dayOfWeekIndex].greatestIndex.Count > 1 && greatestPerDay[dayOfWeekIndex].greatestIndex.Contains(quantityIndex)) //if item part of a tie and it's quantity changed then item has to be re evaluated
                                    greatestPerDay[dayOfWeekIndex].greatestIndex.Remove(quantityIndex);   

                                if(!greatestPerDay[dayOfWeekIndex].greatestIndex.Contains(quantityIndex) && greatestPerDay[dayOfWeekIndex].quantitiesTaken[quantityIndex] == greatestPerDay[dayOfWeekIndex].quantitiesTaken[greatestPerDay[dayOfWeekIndex].greatestIndex[0]]){ //a tie

                                    greatestPerDay[dayOfWeekIndex].greatestIndex.Add(quantityIndex);
                                    greatestPerDay[dayOfWeekIndex].greatestName.Add(stock.Inventory.Name);

                                } else if(greatestPerDay[dayOfWeekIndex].quantitiesTaken[quantityIndex] > greatestPerDay[dayOfWeekIndex].quantitiesTaken[greatestPerDay[dayOfWeekIndex].greatestIndex[0]]){ //greater

                                    greatestPerDay[dayOfWeekIndex].greatestIndex.Clear();
                                    greatestPerDay[dayOfWeekIndex].greatestName.Clear();

                                    greatestPerDay[dayOfWeekIndex].greatestIndex.Add(quantityIndex);
                                    greatestPerDay[dayOfWeekIndex].greatestName.Add(stock.Inventory.Name);

                                }
                        }

                    }
            }

            for(int j=0;j<7;j++){

                DayOfWeek day = DayOfWeek.Sunday;
                switch(j){
                case 1: 
                    day = DayOfWeek.Monday;
                break;
                case 2: 
                    day = DayOfWeek.Tuesday;
                break;
                case 3: 
                    day = DayOfWeek.Wednesday;
                break;
                case 4: 
                    day = DayOfWeek.Thursday;;
                break;
                case 5: 
                    day = DayOfWeek.Friday;
                break;
                case 6: 
                    day = DayOfWeek.Saturday;
                break;
                case 0: 
                    day = DayOfWeek.Sunday;;
                break;
                }
                Console.WriteLine(day);

                for( int i=0; i<greatestPerDay[j].greatestIndex.Count; i++){ 
                    Console.WriteLine(greatestPerDay[j].greatestName[i] + ": " + greatestPerDay[j].quantitiesTaken[greatestPerDay[j].greatestIndex[i]]);
                }

                Console.WriteLine();
            }

        }

        /* 
            Assumptions:
                -last recorded does not refer to the last fully recorded month
                -the AllOpenBarRecords-list (thus also assuming the data) is given in ascending date order
        */
        protected internal static void Question3(OpenBarRepository repo)
        {
            Console.WriteLine("Question 3: Which dates did we run out of Savanna Dry for the last recorded month?");

            // Write your answer to the console here.
            // Format e.g.  {year}/{month}/{day}

            StockTracker tracker = new StockTracker(repo);
            List<DateTime> dates = tracker.getDatesByInventoryCount(getInventoryId(repo,"Savanna Dry"),0);

            DateTime lastDateRecorded = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;

            foreach(var date in dates){

                if(date.Year == lastDateRecorded.Year && date.Month == lastDateRecorded.Month){
                    Console.WriteLine(date.Year+"/"+date.Month+"/"+date.Day);
                }
            }
        }

        /*  
            Assumptions:
                -the AllOpenBarRecords-list (thus also assuming the data) is given in ascending date order
                -I am not expected to identify half weeks in order to take their data out of the averaging calculation
        */
        protected internal static void Question4(OpenBarRepository repo)
        {
            Console.WriteLine("Question 4: How many Fanta Oranges do we need to order next week?");

            // Write your answer to the console here.
            // Format e.g.  {quanity}

            AvgInventoryUsageCalculator avgCalculator = new AvgInventoryUsePerWeekCalculator(repo); //these could be passed in to question for efficiency
            InventoryLibrary library = new InventoryLibrary(repo);
            StockTracker stockTracker = new StockTracker(repo);

            int fantaID = library.getInventoryIdByName("Fanta Orange");
            DateTime lastDate = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;

            int avgFOUsedPerWeek = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt((decimal) avgCalculator.Calculate(fantaID));
            int amountFOToOrder = avgFOUsedPerWeek -  stockTracker.getInventoryCountByDate(fantaID,lastDate);

            if(amountFOToOrder<0) //when there is more than enough drinks
            amountFOToOrder = 0;

            Console.WriteLine(amountFOToOrder);
        }

        /* 
            Assumptions:
                -the AllOpenBarRecords-list (thus also assuming the data) is given in ascending date order
        */
        protected internal static void Question5(OpenBarRepository repo)
        {
            Console.WriteLine("Question 5: How much do we need to budget next month for Ceres Orange Juice?");

            // Write your answer to the console here.
            // Format e.g.  R{amount}

            AvgInventoryUsageCalculator avgCalculator = new AvgInventoryUsePerMonthCalculator(repo); //these could be passed in to question for efficiency
            InventoryLibrary library = new InventoryLibrary(repo);
            StockTracker stockTracker = new StockTracker(repo);

            DateTime lastDate = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;

            int cojID = library.getInventoryIdByName("Ceres Orange Juice");
        
            int avgCOJUsedPerMonth = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt((decimal) avgCalculator.Calculate(cojID));
            int amountCOJToOrder = avgCOJUsedPerMonth -  stockTracker.getInventoryCountByDate(cojID,lastDate);

            if(amountCOJToOrder<0) //when there is more than enough drinks
            amountCOJToOrder = 0;

            decimal COJCost = amountCOJToOrder*library.getInventoryPriceById(cojID);

            Console.WriteLine("R"+COJCost);
        }

        /* Assumptions:
            -the AllOpenBarRecords-list (thus also assuming the data) is given in ascending date order
            -the stock is taken accurately/correctly/consistently
            -one does not have to take time of year into consideration when making the estimate
        */
        protected internal static void Question6(OpenBarRepository repo)
        {
            Console.WriteLine("Question 6: How much do we need to budget for next month to restock the fridge?");

            // Write your answer to the console here.
            // Format e.g.  R{amount}

            AvgInventoryUsageCalculator avgCalculator = new AvgInventoryUsePerMonthCalculator(repo); //these could be passed in to question for efficiency
            InventoryLibrary library = new InventoryLibrary(repo);
            StockTracker stockTracker = new StockTracker(repo);

            DateTime lastDate = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;
            decimal totalCost = 0;

            foreach(var inventory in repo.AllInventory){
                int Id = inventory.Id;

                int avgUsedPerMonth = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt((decimal) avgCalculator.Calculate(Id));
                int amountToOrder = avgUsedPerMonth - stockTracker.getInventoryCountByDate(Id,lastDate);

                if(amountToOrder<0) //when there is more than enough drinks
                amountToOrder = 0;

                totalCost += amountToOrder*library.getInventoryPriceById(Id);
            }
            
            Console.WriteLine("R"+totalCost);
        }

        /* Assumptions:
            -I am not expected to design a unique way of estimating the quanitities but should rather rely on basic averaging calculations
        */
        protected internal static void Question7(OpenBarRepository repo)
        {
            Console.WriteLine("Question 7: We're planning a braai and expecting 100 people, how many of each drink should we order based on historical popularity of drinks?");

            // Write your answer to the console here.
            // Format e.g.  {inventory name}: {quantity}

            int amountOfInventory = repo.AllInventory.Count;
            int nrOfDays = repo.AllOpenBarRecords.Count;   
            decimal[] totalQuantityTakenPerPersonPerDrink = new decimal[amountOfInventory];

            for(var i=0;i<amountOfInventory;i++)
                totalQuantityTakenPerPersonPerDrink[i] = 0;

            foreach( var item in repo.AllOpenBarRecords){

                int nrOfPeople = item.NumberOfPeopleInBar;
                if(nrOfPeople>0){
                    int[] quantitiesPerDrink = new int[amountOfInventory];

                    foreach(var stock in item.FridgeStockTakeList){
                        quantitiesPerDrink[stock.Inventory.Id-1] += stock.Quantity.Taken;
                    }

                    for(var i=0;i<amountOfInventory;i++){
                        totalQuantityTakenPerPersonPerDrink[i] += quantitiesPerDrink[i]/nrOfPeople;
                    }
                }
            }

            decimal[] averageQuantityTakenPerPersonPerDrinkPerDay = new decimal[amountOfInventory];
            for(var i=0;i<amountOfInventory;i++)
            {
                averageQuantityTakenPerPersonPerDrinkPerDay[i] = totalQuantityTakenPerPersonPerDrink[i]/nrOfDays;
                Console.WriteLine(repo.AllInventory[i].Name+ ": " + GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt(averageQuantityTakenPerPersonPerDrinkPerDay[i]*100));
            }


        }
    }
}