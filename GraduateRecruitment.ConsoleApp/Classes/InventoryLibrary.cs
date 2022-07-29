using System.Collections.Generic;
using System;
using GraduateRecruitment.ConsoleApp.Data;

namespace GraduateRecruitment.ConsoleApp.Classes
{
    /*
        Single Responsibility: Provide easy access to inventory attributes
    */
    internal class InventoryLibrary{

        Dictionary<int,decimal> priceLibrary = new Dictionary<int,decimal>();
        Dictionary<int,string> nameLibrary = new Dictionary<int,string>();

        public InventoryLibrary(OpenBarRepository repo){
            foreach(var item in repo.AllInventory){
                priceLibrary.Add(item.Id,item.Price);
                nameLibrary.Add(item.Id,item.Name);
            }
        }

        public string getInventoryNameById(int id){
            return nameLibrary[id];
        }

        public int getInventoryIdByName(string name){
            return StockTracker.KeysByValue(nameLibrary,name)[0];
        }

        public decimal getInventoryPriceByName(string name){
            return priceLibrary[getInventoryIdByName(name)];
        }

        public decimal getInventoryPriceById(int id){
            return priceLibrary[id];
        }
    }
}
