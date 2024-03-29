using System.Collections.Generic;
using System;
using GraduateRecruitment.ConsoleApp.Data;

namespace GraduateRecruitment.ConsoleApp.Classes
{
    /*
        Single Responsibility: Track the fridge's stock count by day
    */
    internal class StockTracker
    {
        private Dictionary<DateTime,int>[] inventoryData;   //Stores amount of each inventory in the fridge at the end of each day

        // Complexity: O(n*m(n)) where n is the amount of OpenBarRecords and m(n) the amount of StockTake entries for n
        public StockTracker(OpenBarRepository repo){

            int amountOfInventory = repo.AllInventory.Count;
            inventoryData = new Dictionary<DateTime,int>[amountOfInventory];

            for(int i=0;i<amountOfInventory;i++){

                inventoryData[i] = new Dictionary<DateTime, int>();

            }

            int[] tempStockCount = new int[amountOfInventory];

            foreach(var record in repo.AllOpenBarRecords){

                foreach(var item in record.FridgeStockTakeList){

                    int inventoryIndex = item.Inventory.Id-1;

                    tempStockCount[inventoryIndex] += item.Quantity.Added;
                    tempStockCount[inventoryIndex] -= item.Quantity.Taken;

                    if(tempStockCount[inventoryIndex] < 0){

                        Console.WriteLine("Error: Stock recorded incorrectly!");
                        Environment.Exit(0);
                        
                    }

                    inventoryData[inventoryIndex].Add(record.Date,tempStockCount[inventoryIndex]); //TODO (Larisa): What if the same inventory item has two stock-take entries on the same day?
                
                }

            }
            
        }

        //Complexity: O(1)
        public int getInventoryCountByDate(int inventoryId, DateTime date){

            return (int) inventoryData[inventoryId-1][date];

        }

        //Complexity: O(n) where n is the number of days for which stock was recorded
        public List<DateTime> getDatesByInventoryCount(int inventoryId, int inventoryCount){

            return KeysByValue(inventoryData[inventoryId-1],inventoryCount);

        }

        public static List<T> KeysByValue<T,V>( Dictionary<T,V> dict, V value){

            List<T> keys = new List<T>();
            foreach( var pair in dict){

                if (EqualityComparer<V>.Default.Equals(pair.Value, value)){

                    keys.Add(pair.Key);

                }

            }
            return keys;

        }

    }

}

