using System;
using System.Runtime.CompilerServices;
using GraduateRecruitment.ConsoleApp.Data;
using System.Collections.Generic;

[assembly: InternalsVisibleTo("GraduateRecruitment.UnitTests")]

namespace GraduateRecruitment.ConsoleApp
{
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

            int amountOfInventory = repo.AllInventory.Count;
            int[] quantitiesTaken = new int[amountOfInventory];
            int greatestIndex = 0;
            String greatestName = "";

            for(int i=0;i<amountOfInventory;i++){
                quantitiesTaken[i] = 0;
            }

            foreach( var item in repo.AllOpenBarRecords){
                if(item.DayOfWeek == DayOfWeek.Wednesday)
                {
                    foreach(var stock in item.FridgeStockTakeList){
                        quantitiesTaken[stock.Inventory.Id-1] += stock.Quantity.Taken;

                        if(quantitiesTaken[stock.Inventory.Id-1] > quantitiesTaken[greatestIndex]){
                            greatestIndex = stock.Inventory.Id-1;
                            greatestName = stock.Inventory.Name;
                        }
                    }
                }        
            }

            Console.WriteLine(greatestName+ ": " + quantitiesTaken[greatestIndex]);
        }

        private static void Question2(OpenBarRepository repo)
        {
            Console.WriteLine("Question 2: What is the most popular drink, including the quantities, per day?");

            // Write your answer to the console here.
            // Format e.g.  {day of week}
            //              {inventory name}: {quantity}

        }

        private static void Question3(OpenBarRepository repo)
        {
            Console.WriteLine("Question 3: Which dates did we run out of Savanna Dry for the last recorded month?");

            // Write your answer to the console here.
            // Format e.g.  {year}/{month}/{day}

            int amountOfSD = 0;

            //Assuming it does not have to be a fully recorded month
            //Assuming Openbar records in ascending date order 
            var lastDate = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;

            foreach( var item in repo.AllOpenBarRecords ){

                foreach(var stock in item.FridgeStockTakeList){

                    if(stock.Inventory.Name.CompareTo("Savanna Dry")==0)
                    {
                        amountOfSD += stock.Quantity.Added;
                        amountOfSD -= stock.Quantity.Taken;

                        /*if(amountOfSD < 0)
                        {
                            Console.WriteLine("Error: Stock recorded incorrectly!");
                            return;
                        }*/

                        if(amountOfSD == 0 && item.Date.Month == lastDate.Month && item.Date.Year == lastDate.Year){
                            Console.WriteLine(item.Date.Year + "/" + item.Date.Month + "/" + item.Date.Day);
                        }
                    }
                }
            }
        }

        private static void Question4(OpenBarRepository repo)
        {
            Console.WriteLine("Question 4: How many Fanta Oranges do we need to order next week?");

            // Write your answer to the console here.
            // Format e.g.  {quanity}
        }

        private static void Question5(OpenBarRepository repo)
        {
            Console.WriteLine("Question 5: How much do we need to budget next month for Ceres Orange Juice?");

            // Write your answer to the console here.
            // Format e.g.  R{amount}
        }

        private static void Question6(OpenBarRepository repo)
        {
            Console.WriteLine("Question 6: How much do we need to budget for next month to restock the fridge?");

            // Write your answer to the console here.
            // Format e.g.  R{amount}
        }

        private static void Question7(OpenBarRepository repo)
        {
            Console.WriteLine("Question 7: We're planning a braai and expecting 100 people, how many of each drink should we order based on historical popularity of drinks?");

            // Write your answer to the console here.
            // Format e.g.  {inventory name}: {quantity}
        }
    }
}