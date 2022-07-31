using System.Collections.Generic;
using GraduateRecruitment.ConsoleApp.Data;
using GraduateRecruitment.ConsoleApp.Data.Models;

namespace GraduateRecruitment.ConsoleApp.Classes
{
    /*
        Single Responsibility: Track drink popularity
    */
    internal abstract class TrendTracker<T>
    {
        protected OpenBarRepository repository;
        protected Dictionary<T,int>[] inventoryData;   //Stores amount of each inventory used per T
        protected Dictionary<T,int> tCount; //Stores the amount of occurances per T

        // Complexity: O(n*m(n)) where n is the amount of OpenBarRecords and m(n) the amount of StockTake entries for n
        public TrendTracker(OpenBarRepository repo){

            repository = repo;

            int amountOfInventory = repository.AllInventory.Count;
            inventoryData = new Dictionary<T, int>[amountOfInventory];

            foreach(var inventory in repository.AllInventory){

                inventoryData[inventory.Id-1] = new Dictionary<T, int>();
                tCount = new Dictionary<T, int>();

            }

            initialise();

            foreach( var record in repository.AllOpenBarRecords){

                T curr = getT(record);
                tCount[curr]+=1;

                foreach(var item in record.FridgeStockTakeList){

                    inventoryData[item.Inventory.Id-1][curr] += item.Quantity.Taken;
                
                }

            }
            
        }

        protected virtual void initialise(){}

        protected abstract T getT(OpenBarRecord record);

    }

}

