using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseKeyGeneratorWPF.Constants
{
    public static class AppConstants
    {
        public const string InvalidInputType = "Invalid Input";
        public const string InvalidDateType = "Invalid Date";

        public const int MinimumNumberOfLicenses = 1;
        public const int LengthOfDate = 8;
        public const int IndexOfDays = 2;
        public const int IndexAfterMonth = 5;
        public const int MaxNumberOfDays = 31;
        public const int MaxNumberOfMonth = 12;
        public const int MinimumDaysAndMonth = 1;
        public const int MonthStartOrder = 3;




        public const string EncryptionKey = "HardCodedEncryptionKey123";

        //public const string DateFormatPattern = @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/([0-9]{4})$";
        public const string NumberPattern = @"[^0-9]";
    }
}
