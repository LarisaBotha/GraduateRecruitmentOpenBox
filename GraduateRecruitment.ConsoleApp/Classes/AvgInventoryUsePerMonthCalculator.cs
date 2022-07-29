using System;
using System.Collections.Generic;
using GraduateRecruitment.ConsoleApp.Data;

namespace GraduateRecruitment.ConsoleApp.Classes
{
    class AvgInventoryUsePerMonthCalculator : AvgInventoryUsageCalculator{

        Dictionary<DayOfWeek,int> daysTillSaturday = new Dictionary<DayOfWeek,int>();
        public AvgInventoryUsePerMonthCalculator(OpenBarRepository repo): base(repo) {
            
        }

        protected override DateTime findEndDate(DateTime startDate){    
            return startDate.AddDays(-startDate.Day).AddMonths(1);
        }

        protected override DateTime updateEndDate(DateTime currEndDate){
            return currEndDate.AddMonths(1);
        }

    }
}