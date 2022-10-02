using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Data.SqlClient;
using System.IO;

using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IDEA_X.IDEAX_Class
{
    public class IDEAX_Function
    {

        public String ip()
        {

            string output = "";

            string hostName = Dns.GetHostName();
            output += ("H:" + hostName);


            string IP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            output += ("|IP:" + IP);
            try
            {
                string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
                var externalIp = IPAddress.Parse(externalIpString);

                output += ("|PIP:" + externalIp.ToString());

            }
            catch (System.Net.WebException e)
            { 
            
            }
           


            return output;

        }

        public byte[] imageToByte(HttpPostedFileBase postedfile)
        {
            byte[] image = null;

            using (BinaryReader br = new BinaryReader(postedfile.InputStream))
            {
                image = br.ReadBytes(postedfile.ContentLength);
            }
            return image;

        }


     public byte[] defImageToByte(System.Drawing.Image imageIn)
        {
          
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }

        }






        public String time()
        {

            var y = DateTime.Now;
            return (y.ToString("dd/MM/yyyy") + "," + y.ToString("HH-mm-s") + "|" + TimeZone.CurrentTimeZone.StandardName);
      

        }

   public String date_text()
        {

            string today = new IDEAX_Function().time();

            //28/06/2022,12-14-6|Bangladesh Standard Time
            string[] firstSplit = today.Split('/');
            string[] scndSplit = firstSplit[2].Split(',');



            if (firstSplit[0] == "01")
            {
                firstSplit[0] = "1";
            }
            if (firstSplit[0] == "02")
            {
                firstSplit[0] = "2";
            }
            if (firstSplit[0] == "03")
            {
                firstSplit[0] = "3";
            }
            if (firstSplit[0] == "04")
            {
                firstSplit[0] = "4";
            }
            if (firstSplit[0] == "05")
            {
                firstSplit[0] = "5";
            }
            if (firstSplit[0] == "06")
            {
                firstSplit[0] = "6";
            }
            if (firstSplit[0] == "07")
            {
                firstSplit[0] = "7";
            }
            if (firstSplit[0] == "08")
            {
                firstSplit[0] = "8";
            }
            if (firstSplit[0] == "09")
            {
                firstSplit[0] = "9";
            }







            if (firstSplit[1] == "01")
            {
                firstSplit[1] = "January";
            }
            if (firstSplit[1] == "02")
            {
                firstSplit[1] = "February";
            }
            if (firstSplit[1] == "03")
            {
                firstSplit[1] = "March";
            }
            if (firstSplit[1] == "04")
            {
                firstSplit[1] = "April";
            }
            if (firstSplit[1] == "05")
            {
                firstSplit[1] = "May";
            }
            if (firstSplit[1] == "06")
            {
                firstSplit[1] = "June";
            }
            if (firstSplit[1] == "07")
            {
                firstSplit[1] = "July";
            }
            if (firstSplit[1] == "08")
            {
                firstSplit[1] = "August";
            }
            if (firstSplit[1] == "09")
            {
                firstSplit[1] = "September";
            }
            if (firstSplit[1] == "10")
            {
                firstSplit[1] = "October";
            }
            if (firstSplit[1] == "11")
            {
                firstSplit[1] = "November";
            }
            if (firstSplit[1] == "12")
            {
                firstSplit[1] = "December";
            }

            today = firstSplit[0] + " " + firstSplit[1] + " " + scndSplit[0];
            return today;
        }

        public String dtime()
        {

            //  var y = DateTime.Now;

            var y = DateTime.Now;
            return (y.ToString());
            // return (y.ToString("dd/MM/yyyy") + "," + y.ToString("HH-mm-s") + "|" + TimeZone.CurrentTimeZone.StandardName);


        }

        public String addDate(int x)
        {

            //  var y = DateTime.Now;

            var y = DateTime.Now.AddDays(x);
            return (y.ToString());
            // return (y.ToString("dd/MM/yyyy") + "," + y.ToString("HH-mm-s") + "|" + TimeZone.CurrentTimeZone.StandardName);


        }

        public String addDateWP(String pdate, int x)
        {

            //  var y = DateTime.Now;
            DateTime prev = Convert.ToDateTime(pdate);
            var y = prev.AddDays(x);
            return (y.ToString());
            // return (y.ToString("dd/MM/yyyy") + "," + y.ToString("HH-mm-s") + "|" + TimeZone.CurrentTimeZone.StandardName);


        }

        public String PastCounter(String a)
        {

            DateTime now = DateTime.Now;
            // DateTime then = DateTime.Now.AddDays(-7);

            DateTime then = Convert.ToDateTime(a);
            TimeSpan ts = now.Subtract(then);
            // return (ts.Days + " Days, " + ts.Hours + " Hours, " + ts.Minutes + " Minutes, " + ts.Seconds + " Seconds, " + ts.Milliseconds + " Milliseconds");
            return (ts.Days + "," + ts.Hours + "," + ts.Minutes + "," + ts.Seconds);


        }
        public String FutureCounter(String a)
        {

            DateTime now = DateTime.Now;
            // DateTime then = DateTime.Now.AddDays(-7);

            DateTime then = Convert.ToDateTime(a);
            TimeSpan ts = then.Subtract(now);

            // return (ts.Days + " Days, " + ts.Hours + " Hours, " + ts.Minutes + " Minutes, " + ts.Seconds + " Seconds, " + ts.Milliseconds + " Milliseconds");
            return (ts.Days + "," + ts.Hours + "," + ts.Minutes + "," + ts.Seconds);


        }

        public ContentResult SerializeJsonRequest(object data)
        {

            System.Diagnostics.Debug.WriteLine("This will be displayed in output window");

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            return new ContentResult
            {
                Content = serializer.Serialize(data),
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8,
            };
           // return returnVal;
        } 




    }
}