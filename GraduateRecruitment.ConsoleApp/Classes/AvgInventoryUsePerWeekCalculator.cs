using System;
using System.Collections.Generic;
using GraduateRecruitment.ConsoleApp.Data;

namespace GraduateRecruitment.ConsoleApp.Classes
{
    /*
        Single Responsibility: Calculate average drinks used per week for each inventory item
    */
    class AvgInventoryUsePerWeekCalculator : AvgInventoryUsageCalculator{

        Dictionary<DayOfWeek,int> daysTillSaturday = new Dictionary<DayOfWeek,int>();

        public AvgInventoryUsePerWeekCalculator(OpenBarRepository repo): base(repo) {}

        protected override void initialise(){ //such that this code can be called before base class constructor
           
            daysTillSaturday.Add(DayOfWeek.Saturday,0);
            daysTillSaturday.Add(DayOfWeek.Friday,1);
            daysTillSaturday.Add(DayOfWeek.Thursday,2);
            daysTillSaturday.Add(DayOfWeek.Wednesday,3);
            daysTillSaturday.Add(DayOfWeek.Tuesday,4);
            daysTillSaturday.Add(DayOfWeek.Monday,5);
            daysTillSaturday.Add(DayOfWeek.Sunday,6);

        }

        protected override DateTime findEndDate(DateTime startDate){
        
            return startDate.AddDays(daysTillSaturday[startDate.DayOfWeek]);

        }

        protected override DateTime updateEndDate(DateTime currEndDate){

            return currEndDate.AddDays(7);

        }

    }
    
}