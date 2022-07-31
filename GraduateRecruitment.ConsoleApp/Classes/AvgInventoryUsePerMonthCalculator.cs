using System;
using GraduateRecruitment.ConsoleApp.Data;

namespace GraduateRecruitment.ConsoleApp.Classes
{
    /*
        Single Responsibility: Calculate average drinks used per month for each inventory item
    */
    class AvgInventoryUsePerMonthCalculator : AvgInventoryUsePerTimePeriodCalculator{

        public AvgInventoryUsePerMonthCalculator(OpenBarRepository repo): base(repo) {}

        protected override DateTime findEndDate(DateTime startDate){   

            return startDate.AddDays(-startDate.Day).AddMonths(1);

        }

        protected override DateTime updateEndDate(DateTime currEndDate){

            return currEndDate.AddMonths(1);

        }

    }
    
}