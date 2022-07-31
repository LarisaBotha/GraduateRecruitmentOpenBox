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
        Single Responsibility: Calculate average drinks used per day per person for each inventory item
    */
    class AvgInventoryUsePerDayPerPesonCalculator : AvgInventoryUsePerDayCalculator{

        public AvgInventoryUsePerDayPerPesonCalculator(OpenBarRepository repo): base(repo) {}

        protected override decimal manipulateInventoryCount(int nrPeople,decimal tempInventoryCount){
            
            if(nrPeople != 0)
                return Decimal.Divide(tempInventoryCount,nrPeople);
            else
                return 0;

        }

    }
    
}