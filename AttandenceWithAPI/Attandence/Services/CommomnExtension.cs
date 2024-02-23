using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace Attandence.Services
{
    public static class CommomnExtension
    {
        #region Extension Methods

        /// <summary>
        /// used to get string value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetString(this object value)
        {
            if (value != null)
            {
                return value.ToString();
            }
            return string.Empty;
        }
        /// <summary>
        /// Method to get current date
        /// </summary>
        /// <returns></returns>
        public static string GetTodayDate()
        {

            return System.DateTime.Now.Day + "_" + System.DateTime.Now.Month + "_" + System.DateTime.Now.Year;

        }
        /// <summary>
        /// Method to get current date
        /// </summary>
        /// <returns></returns>
        public static string GetTodayDateYYYY_MM_DD()
        {

            return System.DateTime.Now.Year + "_" + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day;

        }
        /// <summary>
        /// Get Last Characters
        /// </summary>
        /// <param name="source"></param>
        /// <param name="numberOfChars"></param>
        /// <returns></returns>
        public static string GetLast(this string source, int numberOfChars)
        {
            if (numberOfChars >= source.Length)
                return source;
            return source.Substring(source.Length - numberOfChars);
        }
        /// <summary>
        /// used to get integer value - (like sort order)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal GetTwoDecimal(this object value)
        {
            if (value != null)
            {
                try
                {

                    return Decimal.Round(Convert.ToDecimal(value), 2);
                }
                catch
                {
                    // TODO : Need to comment when 
                    //throw new Exception("Unable to convert in integer");
                }
            }
            return Decimal.Round(0, 2);
        }
        /// <summary>
        /// Mask String
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Mask"></param>
        /// <returns></returns>
        public static string MaskString(this object value, int Mask)
        {
            if (value != null)
            {
                try
                {
                    if (value.GetString().Length > Mask)
                        return new String('X', value.GetString().Length - Mask) + value.GetString().Substring(value.GetString().Length - Mask);
                    else
                        return new String('X', value.GetString().Length);
                }
                catch { }
            }
            return string.Empty;
        }

        /// <summary>
        /// Method to get current date
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTime(this object value)
        {
            DateTime dt = new DateTime();
            try
            {
                dt = DateTime.Parse(value.GetString());
            }
            catch
            {
            }
            return dt;

        }
        /// <summary>
        /// Method to get current date
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTimeParseyyyyMMMdHHmm(this object value)
        {
            DateTime dt = new DateTime();
            try
            {
                dt = (DateTime.ParseExact(value.ToString(), "yyyy-MMM-d HH:mm", CultureInfo.InvariantCulture));
            }
            catch
            {
            }
            return dt;

        }
        /// <summary>
        /// Method to get current date
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTimeParseddMMyyy(this object value)
        {
            DateTime dt = new DateTime();
            try
            {
                dt = (DateTime.ParseExact(value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture));
            }
            catch
            {
            }
            return dt;

        }

        /// <summary>
        /// used to get integer value - (like sort order)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetInteger(this object value)
        {
            if (value != null)
            {
                try
                {
                    return Convert.ToInt32(value);
                }
                catch
                {
                    // TODO : Need to comment when 
                    throw new Exception("Unable to convert in integer");
                }
            }
            return 0;
        }

        /// <summary>
        /// used to get Double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double GetDouble(this object value)
        {
            if (value != null)
            {
                try
                {

                    return Convert.ToDouble(value);
                }
                catch
                {
                    // TODO : Need to comment when 
                    //throw new Exception("Unable to convert in integer");
                }
            }
            return 0.0;
        }

        /// <summary>
        /// used to get integer value - (like sort order)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDateYYYYMMdd(this object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                try
                {
                    return DateTime.ParseExact(value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                }
                catch
                {
                    // TODO : Need to comment when 
                    //throw new Exception("Unable to convert in integer");
                }
            }
            return "";
        }
        /// <summary>
        /// used to get integer value - (like sort order)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDateddMMYYYY(this object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                try
                {
                    return DateTime.ParseExact(value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                }
                catch
                {
                    // TODO : Need to comment when 
                    //throw new Exception("Unable to convert in integer");
                }
            }
            return "";
        }
        /// <summary>
        /// used to get boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetBoolean(this object value)
        {

            if (value != null)
            {
                try
                {
                    return Convert.ToBoolean(value);
                }
                catch
                {
                    // TODO : Need to comment when 
                    throw new Exception("Unable to convert in boolean");
                }
            }
            return false;
        }

        /// <summary>
        /// Return Response
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static WebResponse GetResponseWithoutException(this WebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            try
            {
                return request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    throw;
                }

                return e.Response;
            }
        }
        #endregion
    }
}
