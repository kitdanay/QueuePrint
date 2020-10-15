using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQueue_PR9.Models;
using WebQueue_PR9.Controllers;
using Management.Model;
using System.Configuration;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Npgsql;
using System.Data;
using System.IO;

namespace WebQueue_PR9.Controllers.ScanQueue
{
    public class ScanQueueController : Controller
    {
        // GET: ScanQueue
        private static string api_url = ConfigurationManager.AppSettings["api_url"];
        private static string api_test = ConfigurationManager.AppSettings["api_test"];
        private static string api_key = ConfigurationManager.AppSettings["api_key"];
        private static string api_key_val = ConfigurationManager.AppSettings["api_key_val"];
        //private static string api_url = "http://10.99.23.184/api/";
        //private static string api_key = "API_KEY";
        //private static string api_key_val = "77c7da47-65d1-4c7c-a213-020bcc7b6f35";
        public ActionResult frmScanQueue()
        {
            return View();
        }
      

        public ActionResult onprintsave(string abc)
        {
            clsData checkdata = new clsData();
            molScanQueue moldtScanQueue = new molScanQueue();
            //molData.mdPrint pt = new molData.mdPrint();
            //นับจำนวน printuid
            int a = 0;
            int b = 0;
            //string queryCount = "SELECT count(printuid)+1 FROM public.queueprint where printuid like '%" + abc + "%'";
            string queryCount = "SELECT count(queue)+1 FROM public.queueprint where queue like '%" + abc + "%' and printwhen::date = now()::date";
            string queue = checkdata.Return(queryCount);
            queue = abc + a + b + queue;
            

            //Insert queueprint
            string queryInsert = "INSERT INTO public.queueprint(queue, printwhen,location_id) VALUES ('" + queue + "',   Now(),'" + 1 + "' ) RETURNING  uid ";
            //string queryInsert = "INSERT INTO public.queueprint(queue, printwhen,location_id,close_status) VALUES ('" + queue + "',   Now(),'" + 1 + "','" + 'N' + "' ) RETURNING  uid ";
            string patid = "0";
            //string dtm = DateTime.Now.ToString("HH:mm");
            //TimeSpan now = DateTime.Now.TimeOfDay;
            string success = "";
            try
            {
                patid = checkdata.Return(queryInsert);


                PrintQueueByTua("th","1", queue,DateTime.Now.ToString());
            }
            catch 
            {
                patid = "0";
            }
            if(patid != "0")
            {
                 success = "สำเร็จ";
            }

            //return Json(success, JsonRequestBehavior.AllowGet);
            return RedirectToAction("");

            //return View();
        }



       


        private void PrintQueueByTua(string language, string location_id, string queue, string printwhen)
        {
            string tmplanguage = string.Empty;
            string locationid = "1";
            string print_when = string.Empty;

            if (language == "")
            {
                tmplanguage = "th";
            }
            else
            {
                if (language.Contains("ไทย"))
                {
                    tmplanguage = "en";
                }
                else
                {
                    tmplanguage = "th";
                }
            }
       

            //var request = (HttpWebRequest)WebRequest.Create(api_print);
            //var postData = "location_id=" + Uri.EscapeDataString(LocationQueue_uid);
            var request = (HttpWebRequest)WebRequest.Create("http://10.99.23.180:6004/print");
            var postData = "&language=" + Uri.EscapeDataString(tmplanguage);
            postData += "&printwhen=" + Uri.EscapeDataString(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            postData += "&queue=" + Uri.EscapeDataString(queue);
            postData += "&location_id=" + Uri.EscapeDataString(locationid);
            
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        private void PrintQueue(string phn, string pvn, string pnation, string ptmpfullname, string pvisitstart)
        {
            string tmplanguage = string.Empty;
            string tmpvisitstart = string.Empty;
            string tmpfullname = string.Empty;

            if (pnation == "")
            {
                tmplanguage = "th";
            }
            else
            {
                if (pnation.Contains("ไทย"))
                {
                    tmplanguage = "th";
                }
                else
                {
                    tmplanguage = "en";
                }
            }
            tmpfullname = ptmpfullname;
            tmpvisitstart = pvisitstart;

            //var request = (HttpWebRequest)WebRequest.Create(api_print);
            //var postData = "location_id=" + Uri.EscapeDataString(LocationQueue_uid);
            var request = (HttpWebRequest)WebRequest.Create("http://10.99.23.180:6004/print_covid?");
            var postData = "location_id=" + Uri.EscapeDataString("19");
            postData += "&hn=" + Uri.EscapeDataString(phn);
            postData += "&en=" + Uri.EscapeDataString(pvn);
            postData += "&language=" + Uri.EscapeDataString(tmplanguage);
            postData += "&visitstart=" + Uri.EscapeDataString(tmpvisitstart);
            postData += "&printwhen=" + Uri.EscapeDataString(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            postData += "&queue=" + Uri.EscapeDataString(pvn);
            postData += "&secure=" + Uri.EscapeDataString("1234");
            postData += "&fullname=" + Uri.EscapeDataString(tmpfullname);
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

       
    }
}