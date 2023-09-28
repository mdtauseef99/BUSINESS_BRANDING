using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net.Mail;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace BizzBranding.CommonUtility
{
   public class CommonClassUtility
    {

       public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
       {
           DataTable dtReturn = new DataTable();

           // column names
           PropertyInfo[] oProps = null;

           if (varlist == null) return dtReturn;

           foreach (T rec in varlist)
           {
               // Use reflection to get property names, to create table, Only first time, others

               if (oProps == null)
               {
                   oProps = ((Type)rec.GetType()).GetProperties();
                   foreach (PropertyInfo pi in oProps)
                   {
                       Type colType = pi.PropertyType;

                       if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                       == typeof(Nullable<>)))
                       {
                           colType = colType.GetGenericArguments()[0];
                       }

                       if (colType.FullName != "System.Data.EntityState")
                       {
                           if ((colType.IsClass && colType.FullName == "System.String") || (!colType.IsClass))
                           {
                               dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                           }
                       }
                   }
               }

               DataRow dr = dtReturn.NewRow();

               foreach (PropertyInfo pi in oProps)
               {
                   if (dtReturn.Columns.Contains(pi.Name))
                   {
                       dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                       (rec, null);
                   }
               }

               dtReturn.Rows.Add(dr);
           }
           oProps = null;
           varlist = null;
           return dtReturn;
       }


        #region EmailTemplates

        public static string SearchAndReplace(string strEmailTemp, DataTable dtUser)
        {
            return Regex.Replace(strEmailTemp, @"([\[]\w*[\]]){1,5}", delegate(Match match)
            {
                string st = String.Empty;

                if (match.Success)
                {
                    st = GetReplaceWithValues(match.Value, dtUser);
                }
                return st;
            }, RegexOptions.Multiline);
        }

        public static string FindAndReplace(string input)
        {
            string[] URLEndocing = new string[] { "%2f", "%3d" };

            string result = null;

            for (int i = 0; i < URLEndocing.Length; i++)
            {
                int count = i;

                result = input.Replace(URLEndocing[i].ToString(), URLEncodingList(URLEndocing[i].ToString()));
                input = null;
                input = result;

            }

            return result;
        }

        public static string URLEncodingList(string found)
        {
            string str = String.Empty;

            switch (found)
            {
                case "%2e":
                    str = ".";
                    break;

                case "%2f":
                    str = "/";
                    break;

                case "%3d":
                    str = "=";
                    break;

                //case "%30":
                //    str = "0";
                //    break;

                //case "%31":
                //    str = "1";
                //    break;

                //case "%32":
                //    str = "2";
                //    break;

                //case "%33":
                //    str = "3";
                //    break;

                //case "%34":
                //    str = "4";
                //    break;

                //case "%35":
                //    str = "5";
                //    break;

                //case "%36":
                //    str = "6";
                //    break;

                //case "%37":
                //    str = "7";
                //    break;

                //case "%38":
                //    str = "8";
                //    break;

                //case "%39":
                //    str = "9";
                //    break;

                //case "%40":
                //    str = "@";
                //    break;

                //case "%41":
                //    str = "A";
                //    break;

                //case "%42":
                //    str = "B";
                //    break;

                //case "%43":
                //    str = "C";
                //    break;

                //case "%44":
                //    str = "D";
                //    break;

                //case "%45":
                //    str = "E";
                //    break;

                //case "%46":
                //    str = "F";
                //    break;

                //case "%47":
                //    str = "G";
                //    break;

                //case "%48":
                //    str = "H";
                //    break;

                //case "%49":
                //    str = "I";
                //    break;

                //case "%4A":
                //    str = "J";
                //    break;

                //case "%4B":
                //    str = "K";
                //    break;

                //case "%4C":
                //    str = "L";
                //    break;

                //case "%4D":
                //    str = "M";
                //    break;

                //case "%4E":
                //    str = "N";
                //    break;

                //case "%4F":
                //    str = "O";
                //    break;

                //case "%50":
                //    str = "P";
                //    break;

                //case "%51":
                //    str = "Q";
                //    break;

                //case "%52":
                //    str = "R";
                //    break;

                //case "%53":
                //    str = "S";
                //    break;

                //case "%54":
                //    str = "T";
                //    break;

                //case "%55":
                //    str = "U";
                //    break;

                //case "%56":
                //    str = "V";
                //    break;

                //case "%57":
                //    str = "W";
                //    break;

                //case "%58":
                //    str = "X";
                //    break;

                //case "%59":
                //    str = "Y";
                //    break;

                //case "%5A":
                //    str = "Z";
                //    break;

                //case "%61":
                //    str = "a";
                //    break;

                //case "%62":
                //    str = "b";
                //    break;

                //case "%63":
                //    str = "c";
                //    break;

                //case "%64":
                //    str = "d";
                //    break;

                //case "%65":
                //    str = "e";
                //    break;

                //case "%66":
                //    str = "f";
                //    break;

                //case "%67":
                //    str = "g";
                //    break;

                //case "%68":
                //    str = "h";
                //    break;

                //case "%69":
                //    str = "i";
                //    break;

                //case "%6A":
                //    str = "j";
                //    break;

                //case "%6B":
                //    str = "k";
                //    break;

                //case "%6C":
                //    str = "l";
                //    break;

                //case "%6D":
                //    str = "m";
                //    break;

                //case "%6E":
                //    str = "n";
                //    break;

                //case "%6F":
                //    str = "o";
                //    break;
                //case "%70":
                //    str = "p";
                //    break;
                //case "%71":
                //    str = "q";
                //    break;
                //case "%72":
                //    str = "r";
                //    break;
                //case "%73":
                //    str = "s";
                //    break;
                //case "%74":
                //    str = "t";
                //    break;
                //case "%75":
                //    str = "u";
                //    break;
                //case "%76":
                //    str = "v";
                //    break;
                //case "%77":
                //    str = "w";
                //    break;
                //case "%78":
                //    str = "x";
                //    break;
                //case "%79":
                //    str = "y";
                //    break;
                //case "%7A":
                //    str = "z";
                //    break;
            }
            return str;
        }

        public static string GetReplaceWithValues(string str, DataTable dtUserDetails)
        {
            string res = String.Empty;

            switch (str)
            {
                case "[FirstName]":
                    res = str.Replace(str, dtUserDetails.Rows[0]["FirstName"].ToString());
                    break;
                case "[MiddleName]":
                    res = str.Replace(str, dtUserDetails.Rows[0]["MiddleName"].ToString());
                    break;
                case "[LastName]":
                    res = str.Replace(str, dtUserDetails.Rows[0]["LastName"].ToString());
                    break;
                case "[Email]":
                    res = str.Replace(str, dtUserDetails.Rows[0]["Email"].ToString());
                    break;
                case "[UserName]":
                    res = str.Replace(str, dtUserDetails.Rows[0]["UserName"].ToString());
                    break;
                case "[Password]":
                    string pswd = DataEncryption.Decrypt(dtUserDetails.Rows[0]["Password"].ToString(), "passKey");
                    res = str.Replace(str, pswd);
                    break;
                case "%2f":
                    char c = '/';
                    res = str.Replace(str, c.ToString());
                    break;
                case "%3d":
                    char ch = '=';
                    res = str.Replace(str, ch.ToString());
                    break;
            }
            return res;
        }

        #endregion

    }
}
