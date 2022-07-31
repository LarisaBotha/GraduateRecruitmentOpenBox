using System.Collections.Generic;
using System;
using GraduateRecruitment.ConsoleApp.Data;
using GraduateRecruitment.ConsoleApp.Data.Models;

namespace GraduateRecruitment.ConsoleApp.Classes
{
    /*
        Single Responsibility: Track drink popularity
    */
    internal class DOWTrendTracker: TrendTracker<DayOfWeek>
    {
        
        public DOWTrendTracker(OpenBarRepository repo):base(repo){}

        //Complexity: O(n) where n is the amount of inventory
        protected override void initialise(){

            foreach(var inventory in repository.AllInventory){

                inventoryData[inventory.Id-1].Add(DayOfWeek.Sunday,0);
                inventoryData[inventory.Id-1].Add(DayOfWeek.Monday,0);
                inventoryData[inventory.Id-1].Add(DayOfWeek.Tuesday,0);
                inventoryData[inventory.Id-1].Add(DayOfWeek.Wednesday,0);
                inventoryData[inventory.Id-1].Add(DayOfWeek.Thursday,0);
                inventoryData[inventory.Id-1].Add(DayOfWeek.Friday,0);
                inventoryData[inventory.Id-1].Add(DayOfWeek.Saturday,0);

            }

            tCount.Add(DayOfWeek.Sunday,0);
            tCount.Add(DayOfWeek.Monday,0);
            tCount.Add(DayOfWeek.Tuesday,0);
            tCount.Add(DayOfWeek.Wednesday,0);
            tCount.Add(DayOfWeek.Thursday,0);
            tCount.Add(DayOfWeek.Friday,0);
            tCount.Add(DayOfWeek.Saturday,0);

        }

        //Complexity: O(1)
        public decimal getAvgInventoryUsedPerDOW(DayOfWeek day, int inventoryId){
            if(tCount[day] != 0){

                return Decimal.Divide(inventoryData[inventoryId-1][day],tCount[day]);

            }else{

                return 0;

            }

        }

        //Complexity: O(1)
        protected override DayOfWeek getT(OpenBarRecord record){
            return record.DayOfWeek;
        }

    }

}

