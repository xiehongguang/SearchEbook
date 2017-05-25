//using Microsoft.Analytics.Interfaces;
//using Microsoft.Analytics.Types.Sql;
using Newtonsoft.Json;
using SearchEbook.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SearchEbook.Controller
{
    class CommonController
    {
        public Object FromJson(string method, string str)
        {

            /*
             1：车辆信息CarInfo
			 2：发送命令CommandInfo
			 3：航班信息FlightInfo
			 4: 机位信息SeatInfo
			 5：训练信息TrainInfo
             */
            switch (method)
            {
                case "SearchBook":
                    return JsonConvert.DeserializeObject<SearchBook>(str);
                case "BookDetialInfo":
                    return JsonConvert.DeserializeObject<BookDetialInfo>(str);
                default:
                    return "";
            }
            /*判断属于哪一个Model*/
            //命令消息
            //string response = str;
            ////CommandInfo obj = JsonConvert.DeserializeObject(response.ToString(), typeof(CommandInfo)) as CommandInfo;
            //model = JsonConvert.DeserializeObject<model>(str);
            //return obj;
        }

        public string GetPage(string requestUrl)
        {
            // 准备请求...
            try
            {
                Stream instream = null;
                StreamReader sr = null;
                HttpWebResponse response = null;
                HttpWebRequest request = null;
                // 设置参数
                request = WebRequest.Create(requestUrl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "GET"; //请求方式GET或POST
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Authorization", "Basic YWRtaW46YWRtaW4=");
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}