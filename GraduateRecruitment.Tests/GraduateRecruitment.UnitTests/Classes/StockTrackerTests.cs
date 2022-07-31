using GraduateRecruitment.ConsoleApp.Data;
using GraduateRecruitment.ConsoleApp.Classes;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using System;

namespace GraduateRecruitment.UnitTests.Classes
{
    [TestFixture]
    internal class StockTrackerTests
    {
        
        [Test]
        public void getInventoryCountByDate(){

            var repo = new OpenBarRepository(); 
          
            int amount = 0;
            int amountOfInventory = repo.AllInventory.Count;
            int randomId = (new Random()).Next(1,amountOfInventory);
            DateTime lastDateRecorded = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;
            StockTracker stockTracker = new StockTracker(repo);

            foreach(var record in repo.AllOpenBarRecords){

                foreach(var item in record.FridgeStockTakeList){

                    if(item.Inventory.Id == randomId){ //one inventory test is sufficient as they are all calculated over the same code
                       
                        amount += item.Quantity.Added;
                        amount -= item.Quantity.Taken;
                    
                    }

                }

            }

            stockTracker.getInventoryCountByDate(randomId,lastDateRecorded).Should().Be(amount);

        }

        [Test]
        public void getDatesByInventoryCount(){

            var repo = new OpenBarRepository(); 

            DateTime lastDateRecorded = repo.AllOpenBarRecords[repo.AllOpenBarRecords.Count-1].Date;
            DateTime firstDateRecorded = repo.AllOpenBarRecords[0].Date;

            double dayCount = (lastDateRecorded-firstDateRecorded).TotalDays;
            DateTime halfwayDate = firstDateRecorded.AddDays(dayCount/2);

            int amount = 0;
            int amountOfInventory = repo.AllInventory.Count;
            int randomId = (new Random()).Next(1,amountOfInventory);

            var stockTracker = new StockTracker(repo);

            foreach(var record in repo.AllOpenBarRecords){

                foreach(var item in record.FridgeStockTakeList){

                    if(item.Inventory.Id == randomId){ //one inventory test is sufficient as they are all calculated over the same code
                        
                        amount += item.Quantity.Added;
                        amount -= item.Quantity.Taken;

                    }

                }

                if(record.Date == halfwayDate){

                    break;

                }

            }
     
            List<DateTime> tempList = stockTracker.getDatesByInventoryCount(randomId,amount);

            tempList.Contains(halfwayDate).Should().Be(true);
            
        }

        [Test]
        public void KeysByValue(){

            Dictionary<string,int> tempDict = new Dictionary<string, int>();
            tempDict.Add("Larisa",21);
            tempDict.Add("Melissa",25);
            tempDict.Add("Ewan",17);
            tempDict.Add("Johan",17);
            tempDict.Add("Cornette",52);
            tempDict.Add("Anette",74);
            tempDict.Add("Petual",58);
            tempDict.Add("Arnold",52);

            var repo = new OpenBarRepository(); 
            List<string> tempList = StockTracker.KeysByValue(tempDict,17);

            using (new AssertionScope()){
                tempList.FindIndex(a => a == "Ewan").Should().Be(0);
                tempList.FindIndex(a => a == "Johan").Should().Be(1);
                tempList.Count.Should().Be(2);
            }

        }

    }
   
}