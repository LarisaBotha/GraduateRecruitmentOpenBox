using System;
using System.Runtime.CompilerServices;
using GraduateRecruitment.ConsoleApp.Data;
using GraduateRecruitment.ConsoleApp.Classes;
using System.Collections.Generic;
using System.Collections;
using static GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions;

[assembly: InternalsVisibleTo("GraduateRecruitment.UnitTests")]

namespace GraduateRecruitment.ConsoleApp
{
    internal class Program
    {
        public static (K key,V value) getFirst<K,V>(Dictionary<K,V> dict){
            ICollection keys = dict.Keys;
            K firstKey = default;
            V firstVal = default;
            foreach(K key in keys){
                firstKey = key;
                firstVal = dict[key];
            }
            return (firstKey,firstVal);
        }


        internal static void Main(string[] args)
        {
            var repo = new OpenBarRepository(); 
            InventoryLibrary library = new InventoryLibrary(repo);
            StockTracker stockTracker = new StockTracker(repo);

            Question1(repo,library,stockTracker);
            Console.WriteLine(Environment.NewLine);
            Question2(repo,library,stockTracker);
            Console.WriteLine(Environment.NewLine);
            Question3(repo,library,stockTracker);
            Console.WriteLine(Environment.NewLine);
            Question4(repo,library,stockTracker);
            Console.WriteLine(Environment.NewLine);
            Question5(repo,library,stockTracker);
            Console.WriteLine(Environment.NewLine);
            Question6(repo,library,stockTracker);
            Console.WriteLine(Environment.NewLine);
            Question7(repo,library,stockTracker);
            Console.WriteLine(Environment.NewLine);
        }

        protected internal static void Question1(OpenBarRepository repo, InventoryLibrary library,StockTracker stockTracker)
        {
            Console.WriteLine("Question 1: What is the most popular drink, including the quantity, on a Wednesday?");
            
            // Write your answer to the console here.
            // Format e.g.  {inventory name}: {quantity}

            DOWTrendTracker trendTracker = new DOWTrendTracker(repo); 

            Dictionary<string,decimal> greatestAvgs = new Dictionary<string,decimal>();
            foreach(var inventory in repo.AllInventory){

                string name = inventory.Name;
                decimal currAvg = trendTracker.getAvgInventoryUsedPerDOW(DayOfWeek.Wednesday,inventory.Id);

                if(greatestAvgs.Count > 1 && greatestAvgs.ContainsKey(name)){   //if item part of a tie and it's quantity changed then item has to be re-evaluated
                    greatestAvgs.Remove(name);
                }

                if(!greatestAvgs.ContainsKey(name) && getFirst(greatestAvgs).value == currAvg){  //a tie identified
                    
                    greatestAvgs.Add(name,currAvg);

                } else if(currAvg > getFirst(greatestAvgs).value){  //more popular item identified
                                    
                    greatestAvgs.Clear();
                    greatestAvgs.Add(name,currAvg);
                }

            }

            foreach(var greatest in greatestAvgs){
                int avgQuantity = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt(greatest.Value);
                Console.WriteLine(greatest.Key+": "+avgQuantity);
            }
           

        }

        protected internal static void Question2(OpenBarRepository repo, InventoryLibrary library,StockTracker stockTracker)
        {
            Console.WriteLine("Question 2: What is the most popular drink, including the quantities, per day?");

            // Write your answer to the console here.
            // Format e.g.  {day of week}
            //              {inventory name}: {quantity}
            
            DOWTrendTracker trendTracker = new DOWTrendTracker(repo); 

            foreach( var day in Enum.GetValues(typeof(DayOfWeek))){

                Console.WriteLine(day);

                Dictionary<string,decimal> greatestAvgs = new Dictionary<string,decimal>();
                foreach(var inventory in repo.AllInventory){

                    string name = inventory.Name;
                    decimal currAvg = trendTracker.getAvgInventoryUsedPerDOW((DayOfWeek) day,inventory.Id);

                    if(greatestAvgs.Count > 1 && greatestAvgs.ContainsKey(name)){   //if item part of a tie and it's quantity changed then item has to be re-evaluated
                        greatestAvgs.Remove(name);
                    }

                    if(!greatestAvgs.ContainsKey(name) && getFirst(greatestAvgs).value == currAvg){  //a tie identified
                        
                        greatestAvgs.Add(name,currAvg);

                    } else if(currAvg > getFirst(greatestAvgs).value){  //more popular item identified
                                        
                        greatestAvgs.Clear();
                        greatestAvgs.Add(name,currAvg);
                    }

                }

                foreach(var greatest in greatestAvgs){
                    int avgQuantity = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt(greatest.Value);
                    Console.WriteLine(greatest.Key+": "+avgQuantity);
                }

                Console.WriteLine();

            }

        }

        /* 
            Assumptions:
                -last recorded does not refer to the last fully recorded month
        */
        protected internal static void Question3(OpenBarRepository repo, InventoryLibrary library,StockTracker stockTracker)
        {
            Console.WriteLine("Question 3: Which dates did we run out of Savanna Dry for the last recorded month?");

            // Write your answer to the console here.
            // Format e.g.  {year}/{month}/{day}

            List<DateTime> dates = stockTracker.getDatesByInventoryCount(library.getInventoryIdByName("Savanna Dry"),0);

            DateTime lastDateRecorded = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;

            foreach(var date in dates){

                if(date.Year == lastDateRecorded.Year && date.Month == lastDateRecorded.Month){
                    Console.WriteLine(date.Year+"/"+date.Month+"/"+date.Day);
                }
            }
        }

        protected internal static void Question4(OpenBarRepository repo, InventoryLibrary library,StockTracker stockTracker)
        {
            Console.WriteLine("Question 4: How many Fanta Oranges do we need to order next week?");

            // Write your answer to the console here.
            // Format e.g.  {quanity}

            AvgInventoryUsePerTimePeriodCalculator avgCalculator = new AvgInventoryUsePerWeekCalculator(repo); //these could be passed in to question for efficiency


            int fantaID = library.getInventoryIdByName("Fanta Orange");
            DateTime lastDate = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;

            int avgFOUsedPerWeek = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt((decimal) avgCalculator.Calculate(fantaID));
            int amountFOToOrder = avgFOUsedPerWeek -  stockTracker.getInventoryCountByDate(fantaID,lastDate);

            if(amountFOToOrder<0) //when there is more than enough drinks
            amountFOToOrder = 0;

            Console.WriteLine(amountFOToOrder);
        }

        protected internal static void Question5(OpenBarRepository repo, InventoryLibrary library,StockTracker stockTracker)
        {
            Console.WriteLine("Question 5: How much do we need to budget next month for Ceres Orange Juice?");

            // Write your answer to the console here.
            // Format e.g.  R{amount}

            AvgInventoryUsePerTimePeriodCalculator avgCalculator = new AvgInventoryUsePerMonthCalculator(repo); //these could be passed in to question for efficiency

            DateTime lastDate = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;

            int cojID = library.getInventoryIdByName("Ceres Orange Juice");
        
            int avgCOJUsedPerMonth = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt((decimal) avgCalculator.Calculate(cojID));
            int amountCOJToOrder = avgCOJUsedPerMonth -  stockTracker.getInventoryCountByDate(cojID,lastDate);

            if(amountCOJToOrder<0) //when there is more than enough drinks
            amountCOJToOrder = 0;

            decimal COJCost = amountCOJToOrder*library.getInventoryPriceById(cojID);

            Console.WriteLine("R"+COJCost);
        }

        protected internal static void Question6(OpenBarRepository repo, InventoryLibrary library,StockTracker stockTracker)
        {
            Console.WriteLine("Question 6: How much do we need to budget for next month to restock the fridge?");

            // Write your answer to the console here.
            // Format e.g.  R{amount}

            AvgInventoryUsePerTimePeriodCalculator avgCalculator = new AvgInventoryUsePerMonthCalculator(repo); 

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

        protected internal static void Question7(OpenBarRepository repo, InventoryLibrary library,StockTracker stockTracker)
        {
            Console.WriteLine("Question 7: We're planning a braai and expecting 100 people, how many of each drink should we order based on historical popularity of drinks?");

            // Write your answer to the console here.
            // Format e.g.  {inventory name}: {quantity}

            AvgInventoryUsePerTimePeriodCalculator avgCalculator = new AvgInventoryUsePerDayPerPesonCalculator(repo);

            DateTime lastDate = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;

            foreach(var inventory in repo.AllInventory){
                int Id = inventory.Id;

                decimal avgUsedPerDayPerPerson = avgCalculator.Calculate(Id);
                int avgUsedPerDayPer100Persons = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt(100*avgUsedPerDayPerPerson);
                int amountToOrder = avgUsedPerDayPer100Persons - stockTracker.getInventoryCountByDate(Id,lastDate);

                if(amountToOrder<0) //when there is more than enough drinks
                amountToOrder = 0;

                Console.WriteLine(library.getInventoryNameById(Id)+": "+amountToOrder);
            }


        }
    }
}