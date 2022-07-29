using System;
using System.Collections.Generic;
using GraduateRecruitment.ConsoleApp.Data;

namespace GraduateRecruitment.ConsoleApp.Classes
{
    abstract class AvgInventoryUsageCalculator{

        private OpenBarRepository repository;
        private Dictionary<int,Tuple<decimal,decimal>> inventoryQs = new Dictionary<int,Tuple<decimal,decimal>>();  //stores each inventory's quartile values
        private Dictionary<int,List<int>> inventoryUsed = new Dictionary<int,List<int>>();  //stores inventory used per "time chunk" (determined by derived class eg. weeks,months,years)

        // Complexity: Dependent on derived classes' details
        public AvgInventoryUsageCalculator(OpenBarRepository repo){

            repository = repo;

            /*
                Initialisations
            */

            initialise();

            DateTime firstDate = repository.AllOpenBarRecords[0].Date;
            DateTime currEndDate = findEndDate(firstDate);

            Dictionary<int,int> tempInventoryCount = new Dictionary<int,int>();

            foreach(var inventory in repository.AllInventory){
                int id = inventory.Id;
                tempInventoryCount.Add(id,0);

                inventoryUsed.Add(id,new List<int>()); 
                inventoryQs.Add(id,new Tuple<decimal, decimal>(0,0));   
            }

            /*
                Count inventory used
            */
            foreach(var record in repository.AllOpenBarRecords){

                if(record.Date>=currEndDate){

                    foreach(var inventory in repository.AllInventory){

                        int id = inventory.Id;
                        inventoryUsed[id].Add(tempInventoryCount[id]);

                        tempInventoryCount[id] = 0; //Reset for new "time chunk"
                    }
            
                    while(currEndDate<=record.Date){    //Incase stock haven't changed or been recorded for a few days/weeks/months/years (#Covid)
                        currEndDate = updateEndDate(currEndDate);
                    }
                }

                foreach(var item in record.FridgeStockTakeList){
                    tempInventoryCount[item.Inventory.Id] += item.Quantity.Taken;
                }   

            }

            calculateQuartiles();
        }

        //Complexity: O(n) where n is the amount of "time chunks" (eg weeks,months,years) in the time frame of the repository data
        public decimal Calculate(int inventoryId){

            int sum = 0;
            foreach( var item in inventoryUsed[inventoryId].ToArray()){

                if(isOutlier(inventoryId,item)){    //weeks with a few public holidays will affect the normal usage rate of inventory and is therefore excluded
                    inventoryUsed[inventoryId].Remove(item);
                } else{
                    sum += item;
                }
            }
            decimal avg = sum/inventoryUsed[inventoryId].Count; 
            return avg;
        }

        private void calculateQuartiles(){

           foreach(var inventory in repository.AllInventory){

                inventoryUsed[inventory.Id].Sort();
                int listLength = inventoryUsed[inventory.Id].Count;

                int[] inventoryUsedArray = inventoryUsed[inventory.Id].ToArray();

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
                    q1 = (inventoryUsedArray[(int) Math.Floor(q1Index)] + inventoryUsedArray[(int) Math.Ceiling(q1Index)])/2;
                }else{
                    q1Index = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt((decimal) q1Index);
                    q1 = inventoryUsedArray[(int) q1Index];
                }

                if(q3Index - Math.Floor(q3Index) == 0.5){
                    q3 = (inventoryUsedArray[(int) Math.Floor(q3Index)] + inventoryUsedArray[(int) Math.Ceiling(q3Index)])/2;
                }else{
                    q3Index = GraduateRecruitment.ConsoleApp.Extensions.DecimalExtensions.RoundToInt((decimal) q3Index);
                    q3 = inventoryUsedArray[(int) q1Index];
                }

                /*
                    Store quartiles per inventory item
                */
                inventoryQs[inventory.Id] = new Tuple<decimal,decimal>(q1,q3);
           }
        }

        private bool isOutlier(int inventoryId, int item){

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

    protected virtual void initialise(){}

    }

    
}