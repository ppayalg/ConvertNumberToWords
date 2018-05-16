using System;

namespace TestAkQA.Models
{
    public static class ConvertToWords
    {
        #region Public Method
        public static String ConvertNumber(String number)
        {
            String wholeNo = number, points = "", pointStr = "";
            String andDollarStr = "DOLLARS";
            String endStr = "";
            try
            {
                int decimalPlace = number.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = number.Substring(0, decimalPlace);                    
                    points = number.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andDollarStr = andDollarStr + " AND"; // just to separate whole numbers from points/cents 
                        endStr = "CENTS";  
                        pointStr = ConvertToDecimals(points);
                    }
                }
                if (wholeNo.Length >= 13)
                {
                    //not supported currency above billions
                    return Constants.ErrorNotSupported;
                }
                var wordFormat = ConvertWholeNumber(wholeNo).Trim();
                if (wordFormat == Constants.ErrorNotSupported)
                {
                    return Constants.ErrorNotSupported;
                }
                if (endStr != "")
                {   //append cents
                    return String.Format("{0} {1}{2} {3}", wordFormat, andDollarStr, pointStr, endStr);
                }
                else
                {
                    return String.Format("{0} {1}{2}", wordFormat, andDollarStr, pointStr);
                }
            }
            catch
            {
                //do the logging here
            }
            return "";
        }

        #endregion

        #region Private Methods
        private static String ConvertWholeNumber(String input)
        {
            string word = "";
            try
            {
                bool isDone = false;
                double currency = (Convert.ToDouble(input));
                if (currency > 0)
                {
                    int numDigits = input.Length;
                    int pos = 0;//store digit grouping
                    String place = "";//digit grouping name:hundres,thousand,etc...
                    switch (numDigits)
                    {
                        case 1://ones' range

                            word = Ones(input);
                            isDone = true;
                            break;
                        case 2://tens' range
                            word = Tens(input);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = " HUNDRED ";
                            break;
                        case 4://thousands' range
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " THOUSAND ";
                            break;
                        case 7://millions' range
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " MILLION ";
                            break;
                        case 10://Billions's range
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " BILLION ";
                            break;
                        default:
                            //not supported above billions
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {
                        //verify current position in recursion
                        if (input.Substring(0, pos) != "0" && input.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(input.Substring(0, pos)) + place + ConvertWholeNumber(input.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(input.Substring(0, pos)) + ConvertWholeNumber(input.Substring(pos));
                        }
                    }
                    //trim word 
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch
            {
                //do the logging here
            }
            return word.Trim();
        }

        private static String Tens(String number)
        {
            var _number = Convert.ToInt32(number);
            String name = null;
            switch (_number)
            {
                case 10:
                    name = "TEN";
                    break;
                case 11:
                    name = "ELEVEN";
                    break;
                case 12:
                    name = "TWELVE";
                    break;
                case 13:
                    name = "THIRTEEN";
                    break;
                case 14:
                    name = "FOURTEEN";
                    break;
                case 15:
                    name = "FIFTEEN";
                    break;
                case 16:
                    name = "SIXTEEN";
                    break;
                case 17:
                    name = "SEVENTEEN";
                    break;
                case 18:
                    name = "EIGHTEEN";
                    break;
                case 19:
                    name = "NINETEEN";
                    break;
                case 20:
                    name = "TWENTY-";
                    break;
                case 30:
                    name = "THIRTY-";
                    break;
                case 40:
                    name = "FORTY-";
                    break;
                case 50:
                    name = "FIFTY-";
                    break;
                case 60:
                    name = "SIXTY-";
                    break;
                case 70:
                    name = "SEVENTY-";
                    break;
                case 80:
                    name = "EIGHTY-";
                    break;
                case 90:
                    name = "NINETY-";
                    break;
                default:
                    if (_number > 0)
                    {
                        name = Tens(number.Substring(0, 1) + "0") + "" + Ones(number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static String Ones(String number)
        {
            int _number = Convert.ToInt32(number);
            String englishNum = "";
            switch (_number)
            {

                case 1:
                    englishNum = "ONE";
                    break;
                case 2:
                    englishNum = "TWO";
                    break;
                case 3:
                    englishNum = "THREE";
                    break;
                case 4:
                    englishNum = "FOUR";
                    break;
                case 5:
                    englishNum = "FIVE";
                    break;
                case 6:
                    englishNum = "SIX";
                    break;
                case 7:
                    englishNum = "SEVEN";
                    break;
                case 8:
                    englishNum = "EIGHT";
                    break;
                case 9:
                    englishNum = "NINE";
                    break;
            }
            return englishNum;
        }

        private static String ConvertToDecimals(String number)
        {
            if (number.StartsWith("0") || number.Length == 1 || number.Length > 2)
            {
                // print as one two three
                String sb = "", digit = "", engOne = "";
                for (int i = 0; i < number.Length; i++)
                {
                    digit = number[i].ToString();
                    if (digit.Equals("0"))
                    {
                        engOne = "ZERO";
                    }
                    else
                    {
                        engOne = Ones(digit);
                    }
                    sb += " " + engOne;
                }
                return sb;
            }
            //print as Forty
            var strTens = Tens(number);
            if (strTens.EndsWith("-"))
            {
                strTens = strTens.Replace("-", "");
            }
            return " " + strTens;
        }

        #endregion
    }
}