using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using GraduateRecruitment.ConsoleApp.Data;
using System.IO;

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

        }

        [Test]
        public void Question2(){

        }

        [Test]
        public void Question3(){

            using( var consoleOutput = new ConsoleOutput()){
                var repo = new OpenBarRepository();
                GraduateRecruitment.ConsoleApp.Program.Question3(repo);
                
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
                                
                                Assert.Greater(output.Length,consoleIndex);
                                Assert.AreEqual(date.Year + "/" + date.Month + "/" + date.Day,output[consoleIndex]);
                                consoleIndex++;
                            }

                        }
                    }
                }
            }
        }

        [Test]
        public void Question4(){

        }

        [Test]
        public void Question5(){

        }

        [Test]
        public void Question6(){

        }

        [Test]
        public void Question7(){

        }
    }
}