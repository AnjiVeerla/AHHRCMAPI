using System;

namespace RCMAPI.AgeCalculator
{
    public static class NxGAgeCalculator
    {
        public static string CalculateAgeFromDob(string dobOfPatient)
        {
            int intAge = 0;
            DateTime dob;
            string calculatedAge = string.Empty;
            if (!string.IsNullOrEmpty(dobOfPatient.Trim()))
            {
                try
                {
                    dob = Convert.ToDateTime(dobOfPatient);
                    string strDate = string.Empty;
                    if (!string.IsNullOrEmpty(dobOfPatient.Trim()))
                    {
                        DateTime dt = DateTime.Now;
                        TimeSpan tsAgeDiff = Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(dob);
                        if (tsAgeDiff.Negate().Hours <= 0)
                        {
                            intAge = getNoOfYears(dob);
                            int NoOfMonths = getNoOfMonths(dob);
                            int daysCount = getNoOfDays(dob);
                            int hourCount = getNoOfHours(dob);
                            int minuteCount = getNoOfMins(dob);
                            int secondsCount = getNoOfSecs(dob);
                            if (intAge > 150)
                            {
                                calculatedAge = "0-0-Enter Valid DOB < 150";
                            }
                            else if (intAge >= 1)
                            {
                                calculatedAge = intAge.ToString() + "-" + "1";
                            }
                            else if (NoOfMonths >= 1)
                            {
                                calculatedAge = NoOfMonths.ToString() + "-" + "2";
                            }
                            else if (daysCount > 0)
                            {
                                calculatedAge = daysCount.ToString() + "-" + "3";
                            }
                            else if (hourCount > 0)
                            {
                                calculatedAge = hourCount.ToString() + "-" + "4";
                            }
                            else if (minuteCount > 0)
                            {
                                calculatedAge = minuteCount.ToString() + "-" + "5";
                            }
                            else if (secondsCount >= 0)
                            {
                                calculatedAge = secondsCount.ToString() + "-" + "6";
                            }
                        }
                        else
                        {
                            calculatedAge = "0-0-Future dates cannot be selected as Date Of Birth";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return calculatedAge;
        }

        public static string CalculateAgeByDOB(this DateTime dtDOB)
        {
            return GetAgeByDOB(dtDOB);
        }

        private static int getNoOfSecs(DateTime dob)
        {
            DateTime Now = DateTime.Now;
            //Get Years  
            int Years = new DateTime(DateTime.Now.Subtract(dob).Ticks).Year - 1;
            //Get Past Year to calculate months, days and time  
            DateTime dtPastYearDate = dob.AddYears(Years);
            int Seconds = Now.Subtract(dtPastYearDate).Seconds;
            return Seconds;
        }
        private static int getNoOfMins(DateTime dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(dob).Ticks).Year - 1;
            //Get Past Year to calculate months, days and time  
            DateTime dtPastYearDate = dob.AddYears(Years);
            int Minutes = Now.Subtract(dtPastYearDate).Minutes;
            return Minutes;
        }
        private static int getNoOfHours(DateTime dob)
        {
            DateTime Now = DateTime.Now;
            //Get Years  
            int Years = new DateTime(DateTime.Now.Subtract(dob).Ticks).Year - 1;
            //Get Past Year to calculate months, days and time  
            DateTime dtPastYearDate = dob.AddYears(Years);
            int Hours = Now.Subtract(dtPastYearDate).Hours;
            return Hours;
        }
        private static int getNoOfDays(DateTime dob)
        {
            DateTime current = DateTime.Now;
            if (dob.Day > current.Day)
            {
                if ((dob.Month < current.Month) || (dob.Year < current.Year))
                {
                    int numberOfDays = 0;
                    if (DateTime.Now.Month != 1)
                        numberOfDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month - 1);
                    else
                        numberOfDays = DateTime.DaysInMonth(DateTime.Now.Year, 12);
                    return (current.Day + (numberOfDays - dob.Day));
                }
                else
                    return -1;
            }
            else if (dob.Day == current.Day)
            {
                return 0;
            }
            else
            {
                return (current.Day - dob.Day);
            }
        }
        private static int getNoOfMonths(DateTime dob)
        {
            int NoOfMonths = 0;
            DateTime current = DateTime.Now;
            if (dob.Month < current.Month)
            {
                if (dob.Day > current.Day)
                {
                    NoOfMonths = (current.Month - dob.Month - 1);
                }
                else
                {
                    NoOfMonths = (current.Month - dob.Month);
                }
            }
            else if (dob.Month == current.Month)
            {
                if (dob.Day > current.Day)
                {
                    NoOfMonths = -1;
                }
                else
                {
                    NoOfMonths = 0;
                }
            }
            else
            {
                NoOfMonths = -1;
            }
            if (dob.Year < current.Year)
            {
                if (NoOfMonths < 0)
                {
                    if (dob.Day <= current.Day)
                        NoOfMonths = (12 - dob.Month) + current.Month;
                    else
                        NoOfMonths = (12 - dob.Month) + (current.Month - 1);
                }
            }
            return NoOfMonths;
        }
        private static int getNoOfYears(DateTime dob)
        {
            DateTime current = DateTime.Now;
            if (dob.Year > current.Year)
            {
                return -1;
            }
            else if (dob.Year == current.Year)
            {
                if (dob.Month < current.Month)
                {
                    return 0;
                }
                else if (dob.Month == current.Month)
                {
                    if (dob.Day > current.Day)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (dob.Month < current.Month)
                {
                    return current.Year - dob.Year;
                }
                else if (dob.Month == current.Month)
                {
                    if (dob.Day > current.Day)
                    {
                        return (current.Year - dob.Year - 1);
                    }
                    else
                    {
                        return (current.Year - dob.Year);
                    }
                }
                else
                {
                    return (current.Year - dob.Year - 1);
                }
            }
        }

        public static string GetAgeByDOB(DateTime dtDOB)
        {
            string age = string.Empty;
            int Years = getNoOfYears(dtDOB);
            int Months = getNoOfMonths(dtDOB);
            int Days = getNoOfDays(dtDOB);
            int Hours = getNoOfHours(dtDOB);
            if (Years + Months + Days + Hours == 0)
            {
                age = "< 1 Hour";
            }
            else if(Years+Months==0 && Days<=1)
            {
                age=(Hours + (Days * 24)).ToString() + " Hour(s)";
            }
            else if(Years+Months==0)//if age is in days
            {
               age= Days.ToString() + " Day(s)";
            }
            else if (Years == 0 && Months != 0 && Days<30)//if age is in days
            {
                age = Months.ToString() + " Mth(s) " + Days.ToString() + " Day(s)";
            }
            else if (Years == 0 && Days <= 61)//if age lessthen 2 months
            {
                age = Days.ToString()+" Day(s)";
            }
            else if (Years == 0)
            {
                age=Months.ToString() + " Month(s)";
            }
            else if (Years <= 18)
            {
                if (Months != 0)
                    age = Years.ToString() + " Yr(s)" + Months.ToString() + " Mth(s)";
                else
                    age = Years.ToString() + " Year(s)";
            }
            else
            {
                age = Years.ToString() + " Year(s)";
            }
            return age;
        }
    }
}
