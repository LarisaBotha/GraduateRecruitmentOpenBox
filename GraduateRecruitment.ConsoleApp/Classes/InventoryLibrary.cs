using System.Collections.Generic;
using GraduateRecruitment.ConsoleApp.Data;

namespace GraduateRecruitment.ConsoleApp.Classes
{
    /*
        Single Responsibility: Provide easy access to inventory attributes
    */
    internal class InventoryLibrary{

        Dictionary<int,decimal> priceLibrary = new Dictionary<int,decimal>(); //stores the price of each inventory item
        Dictionary<int,string> nameLibrary = new Dictionary<int,string>();  //stores the name of each inventory item

        //Complexity: O(n) where n is the amount of inventory
        public InventoryLibrary(OpenBarRepository repo){

            foreach(var item in repo.AllInventory){

                priceLibrary.Add(item.Id,item.Price);
                nameLibrary.Add(item.Id,item.Name);

            }

        }

        //Complexity: O(1)
        public string getInventoryNameById(int id){

            return nameLibrary[id];

        }

        //Complexity: O(n) where n is the amount of inventory
        public int getInventoryIdByName(string name){

            return StockTracker.KeysByValue(nameLibrary,name)[0];

        }

        //Complexity: O(1)
        public decimal getInventoryPriceByName(string name){

            return priceLibrary[getInventoryIdByName(name)];

        }

        //Complexity: O(1)
        public decimal getInventoryPriceById(int id){

            return priceLibrary[id];

        }

    }

}
