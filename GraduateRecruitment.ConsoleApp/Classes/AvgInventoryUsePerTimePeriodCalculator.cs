using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GraduateRecruitment.ConsoleApp.Data;

[assembly: InternalsVisibleTo("GraduateRecruitment.UnitTests")]

namespace GraduateRecruitment.ConsoleApp.Classes
{
    /*
        Single Responsibility: Calculate average inventory usage per time chunk
    */
    abstract class AvgInventoryUsePerTimePeriodCalculator{

        protected OpenBarRepository repository;
        private Dictionary<int,Tuple<decimal,decimal>> inventoryQs = new Dictionary<int,Tuple<decimal,decimal>>();  //stores each inventory's quartile values
        private Dictionary<int,List<decimal>> inventoryUsed = new Dictionary<int,List<decimal>>();  //stores inventory used per "time chunk" (determined by derived class eg. weeks,months,years)

        // Complexity: Dependent on derived classes' details
        public AvgInventoryUsePerTimePeriodCalculator(OpenBarRepository repo){

            repository = repo;

            /*
                Initialisations
            */
            initialise();

            DateTime firstDate = repository.AllOpenBarRecords[0].Date;
            DateTime currEndDate = findEndDate(firstDate);

            Dictionary<int,decimal> tempInventoryCount = new Dictionary<int,decimal>();

            foreach(var inventory in repository.AllInventory){

                int id = inventory.Id;
                tempInventoryCount.Add(id,0);

                inventoryUsed.Add(id,new List<decimal>()); 
                inventoryQs.Add(id,new Tuple<decimal, decimal>(0,0));   

            }

            /*
                Count inventory used
            */
            foreach(var record in repository.AllOpenBarRecords){

                foreach(var item in record.FridgeStockTakeList){ //count all in "time chunk"

                    tempInventoryCount[item.Inventory.Id] += item.Quantity.Taken;
                }   

                if(record.Date>=currEndDate){ //new "time chunk"

                    foreach(var inventory in repository.AllInventory){

                        int id = inventory.Id;

                        tempInventoryCount[id] = manipulateInventoryCount(record.NumberOfPeopleInBar,tempInventoryCount[id]);
                        inventoryUsed[id].Add(tempInventoryCount[id]);

                        tempInventoryCount[id] = 0.0m; //Reset for new "time chunk"

                    }
            
                    while(currEndDate<=record.Date){    //Incase stock haven't changed or been recorded for a few days/weeks/months/years (#Covid)
                        
                        currEndDate = updateEndDate(currEndDate);

                    }

                }

            }

            calculateQuartiles();
            
        }

        //Complexity: O(n) where n is the amount of "time chunks" (eg weeks,months,years) in the time frame of the repository data
        public decimal Calculate(int inventoryId){

            /* 
                Sum inventory usage excluding outliers
            */
            decimal sum = 0;

            foreach( var item in inventoryUsed[inventoryId].ToArray()){

                if(isOutlier(inventoryId,item)){    //abnormal time chunks will affect the normal usage rate of inventory and is therefore excluded
                    
                    inventoryUsed[inventoryId].Remove(item);

                } else{

                    sum += item;

                }
                
            }

            /*  
                Average each inventory item per "time chunk"
            */
            decimal avg = Decimal.Divide(sum,inventoryUsed[inventoryId].Count); 
            return avg;

        }

        protected internal void calculateQuartiles(){

           foreach(var inventory in repository.AllInventory){

                inventoryUsed[inventory.Id].Sort();
                int listLength = inventoryUsed[inventory.Id].Count;

                decimal[] inventoryUsedArray = inventoryUsed[inventory.Id].ToArray();

                /*
                    Calculate quartile index
                */
                double q1Index = 0.25*(listLength+1);
                double q3Index = 0.75*(listLength+1);

                /*
                    Get quartile values
                */
                decimal q1;
                decimal q3;

                if(q1Index - Math.Floor(q1Index) == 0.5){

                    q1 = Decimal.Divide((inventoryUsedArray[(int) Math.Floor(q1Index)] + inventoryUsedArray[(int) Math.Ceiling(q1Index)]),2);
                    
                }else{

                    q1Index = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt((decimal) q1Index);
                    q1 = inventoryUsedArray[(int) q1Index];

                }

                if(q3Index - Math.Floor(q3Index) == 0.5){

                    q3 = Decimal.Divide((inventoryUsedArray[(int) Math.Floor(q3Index)] + inventoryUsedArray[(int) Math.Ceiling(q3Index)]),2);
                
                }else{

                    q3Index = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt((decimal) q3Index);
                    q3 = inventoryUsedArray[(int) q3Index];

                }

                /*
                    Store quartiles per inventory item
                */
                inventoryQs[inventory.Id] = new Tuple<decimal,decimal>(q1,q3);

           }

        }

        protected internal bool isOutlier(int inventoryId, decimal item){

            /*
                Determine whether the "time chunk's" inventor usage is
                between q1-1.5(q3-q1) and q3+1.5(q3-q1) or not
            */
            decimal q1 = inventoryQs[inventoryId].Item1;
            decimal q3 = inventoryQs[inventoryId].Item2;
            decimal iqr15 = (decimal) 1.5*(q3-q1);

            if(item < (q1 - iqr15) || item > (q3 + iqr15)){

                return true;

            } else {

                return false;

            }


        }

        protected abstract DateTime findEndDate(DateTime startDate);

        protected abstract DateTime updateEndDate(DateTime currEndDate);

        protected virtual void initialise(){} //incase a derived class has to be initialised before the base class's constructor is executed

        protected virtual decimal manipulateInventoryCount(int nrPeople,decimal tempInventoryCount){
            return tempInventoryCount;
        }

    }
   
}