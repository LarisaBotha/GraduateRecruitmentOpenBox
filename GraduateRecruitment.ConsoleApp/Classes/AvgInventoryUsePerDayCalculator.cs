using System;
using System.Collections.Generic;
using GraduateRecruitment.ConsoleApp.Data;

/*
    Note: 
    Calculating the average of each inventory used per day through the AvgInventoryUsePerTimePeriodCalculator 
    is more inefficient than it could be but for the sake of SOLID principles, this is more consistent.
*/

namespace GraduateRecruitment.ConsoleApp.Classes
{
    /*
        Single Responsibility: Calculate average drinks used per day for each inventory item
    */
    class AvgInventoryUsePerDayCalculator : AvgInventoryUsePerTimePeriodCalculator{

        public AvgInventoryUsePerDayCalculator(OpenBarRepository repo): base(repo) {}

        protected override DateTime findEndDate(DateTime startDate){
        
            return startDate;

        }

        protected override DateTime updateEndDate(DateTime currEndDate){

            return currEndDate.AddDays(1);

        }

    }
    
}