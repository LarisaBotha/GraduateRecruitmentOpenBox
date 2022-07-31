using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using GraduateRecruitment.ConsoleApp;
using GraduateRecruitment.ConsoleApp.Data;
using GraduateRecruitment.ConsoleApp.Classes;
using System.IO;
using System.Collections;

namespace GraduateRecruitment.UnitTests
{
    [TestFixture]
    public class ConsoleAppTests
    {
        public class ConsoleOutput : IDisposable
        {
            private StringWriter stringWriter;
            private TextWriter originalOutput;

            public ConsoleOutput()
            {
                stringWriter = new StringWriter();
                originalOutput = Console.Out;
                Console.SetOut(stringWriter);
            }

            public string[] GetOuput()
            {
                string[] consoleArray = stringWriter.ToString().Split(new string[]{Environment.NewLine},StringSplitOptions.None);
                return consoleArray;
            }

            public void Dispose()
            {
                Console.SetOut(originalOutput);
                stringWriter.Dispose();
            }
        }

        [Test]
        public void Question1(){

            using( var consoleOutput = new ConsoleOutput()){

                OpenBarRepository repo = new OpenBarRepository();
                InventoryLibrary library = new InventoryLibrary(repo);
                StockTracker stockTracker = new StockTracker(repo);
                GraduateRecruitment.ConsoleApp.Program.Question1(repo,library,stockTracker);

                string[] output = consoleOutput.GetOuput();
                int consoleIndex = 1;   //the first line printed is the question

                /*
                Declarations
                */
                decimal wednesdayCount = 0;
                Dictionary<string,int> quantitiesTaken = new Dictionary<string,int>();

                foreach(var inventory in repo.AllInventory){
                    quantitiesTaken.Add(inventory.Name,0);
                }

                foreach( var record in repo.AllOpenBarRecords){

                    if(record.DayOfWeek == DayOfWeek.Wednesday){  //filtering: only interested in wednesdays
                        
                        wednesdayCount++;

                        foreach(var item in record.FridgeStockTakeList){
                            string stockName = item.Inventory.Name;
                            quantitiesTaken[stockName] += item.Quantity.Taken;
                        }

                    }   

                }

                Dictionary<string,decimal> greatestAvgs = new Dictionary<string, decimal>();
                greatestAvgs.Add("",0);

                foreach(var item in quantitiesTaken){
                    
                    decimal tempAvg = item.Value/wednesdayCount;
                    
                    if(greatestAvgs.Count > 1 && greatestAvgs.ContainsKey(item.Key)){   //if item part of a tie and it's quantity changed then item has to be re-evaluated
                        greatestAvgs.Remove(item.Key);
                    }

                    if(!greatestAvgs.ContainsKey(item.Key) && greatestAvgs[Program.getFirst(quantitiesTaken).key] == tempAvg){  //a tie identified
                        
                        greatestAvgs.Add(item.Key,tempAvg);

                    } else if(tempAvg > greatestAvgs[Program.getFirst(quantitiesTaken).key]){  //more popular item identified
                                        
                        greatestAvgs.Clear();
                        greatestAvgs.Add(item.Key,tempAvg);
                    }
                }

                    ICollection greatestNames = greatestAvgs.Keys;

                    foreach(string name in greatestNames){
                        output[consoleIndex].Should().Be(name + ": " + greatestAvgs[name]);
                        consoleIndex++;
                    }
            }
        }

        [Test]
        public void Question2(){
                
        }

        [Test]
        public void Question3(){

            using( var consoleOutput = new ConsoleOutput()){
                
                OpenBarRepository repo = new OpenBarRepository();
                InventoryLibrary library = new InventoryLibrary(repo);
                StockTracker stockTracker = new StockTracker(repo);
                GraduateRecruitment.ConsoleApp.Program.Question3(repo,library,stockTracker);
                
                string[] output = consoleOutput.GetOuput();
                int consoleIndex = 1;   //the first line printed is the question

                int amountOfSD = 0; 
                DateTime lastDateRecorded = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;
                
                foreach( var item in repo.AllOpenBarRecords ){

                    foreach(var stock in item.FridgeStockTakeList){

                        if(stock.Inventory.Name.CompareTo("Savanna Dry")==0){ //filtering: only interested in savanna dry
                            DateTime date = item.Date;
                            
                            amountOfSD += stock.Quantity.Added;
                            amountOfSD -= stock.Quantity.Taken;

                            if(date.Year == lastDateRecorded.Year && date.Month == lastDateRecorded.Month && amountOfSD == 0){
                                
                                consoleIndex.Should().BeLessThan(output.Length);
                                output[consoleIndex].Should().Be(date.Year + "/" + date.Month + "/" + date.Day);
                                
                                /*Assert.Greater(output.Length,consoleIndex);
                                Assert.AreEqual(date.Year + "/" + date.Month + "/" + date.Day,output[consoleIndex]);*/

                                consoleIndex++;
                            }

                        }
                    }
                }
            }
        }

        [Test]
        public void Question4(){

            OpenBarRepository repo = new OpenBarRepository();
            InventoryLibrary library = new InventoryLibrary(repo);
            StockTracker stockTracker = new StockTracker(repo);
            GraduateRecruitment.ConsoleApp.Program.Question4(repo,library,stockTracker);

        }

        [Test]
        public void Question5(){

            OpenBarRepository repo = new OpenBarRepository();
            InventoryLibrary library = new InventoryLibrary(repo);
            StockTracker stockTracker = new StockTracker(repo);
            GraduateRecruitment.ConsoleApp.Program.Question5(repo,library,stockTracker);

        }

        [Test]
        public void Question6(){

            OpenBarRepository repo = new OpenBarRepository();
            InventoryLibrary library = new InventoryLibrary(repo);
            StockTracker stockTracker = new StockTracker(repo);
            GraduateRecruitment.ConsoleApp.Program.Question6(repo,library,stockTracker);

        }

        [Test]
        public void Question7(){

            OpenBarRepository repo = new OpenBarRepository();
            InventoryLibrary library = new InventoryLibrary(repo);
            StockTracker stockTracker = new StockTracker(repo);
            GraduateRecruitment.ConsoleApp.Program.Question7(repo,library,stockTracker);

            /*int amountOfInventory = repo.AllInventory.Count;
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
                Console.WriteLine(i+" "+totalQuantityTakenPerPersonPerDrink[i]);
                Console.WriteLine(i+" "+nrOfDays);
                averageQuantityTakenPerPersonPerDrinkPerDay[i] = totalQuantityTakenPerPersonPerDrink[i]/nrOfDays;
                Console.WriteLine(repo.AllInventory[i].Name+ ": " + GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt(averageQuantityTakenPerPersonPerDrinkPerDay[i]*100));
            }
            */

        }
    }
}