using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace WX
{
    public  class help
    {
        /// <summary>
        /// 统一全局返回消息处理方法
        /// </summary>
        /// <param name="postStr"></param>
        /// <returns></returns>
        public string ReturnMessage(string postStr)
        {
            string responseContent = "";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("GB2312").GetBytes(postStr)));
            XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
            if (MsgType != null)
            {
                switch (MsgType.InnerText)
                {
                    case "text":
                        responseContent = TextHandle(xmldoc);//文本消息处理
                        break;
                    case "voice":
                        responseContent = VoiceHandle(xmldoc, postStr);//文本消息处理
                        break;
                    default:
                        break;
                }
            }
            return responseContent;
        }


        /// <summary>
        /// 接受文本消息并回复自定义消息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public string TextHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
            if (Content != null)
            {

                    var message = "";
                    int num;
                    if(Content.InnerText.Length==6)
                    {
                        if (int.TryParse(Content.InnerText, out num))
                        {
                            message = getdata(Content.InnerText);
                        }
                    }
                    else
                    {
                        Class1 a = new Class1();
                        message = a.ConnectTuLing(Content.InnerText);
                    }
                  
                   
                    responseContent = string.Format("<xml><ToUserName>{0}</ToUserName><FromUserName>{1}</FromUserName><CreateTime>{2}</CreateTime><MsgType>text</MsgType><Content>{3}</Content></xml>",
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    message);
              
            }
            return responseContent;
        }


        /// <summary>
        /// 接受语音消息并回复自定义消息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public string VoiceHandle(XmlDocument xmldoc,string qq)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Recongnition");
            if (Content != null)
            {

                var message = "";
                int num;
                if (Content.InnerText.Length == 6)
                {
                    if (int.TryParse(Content.InnerText, out num))
                    {
                        message = getdata(Content.InnerText);
                    }
                }
                else
                {
                    Class1 a = new Class1();
                    message = a.ConnectTuLing(Content.InnerText);
                }

                message= qq;
                responseContent = string.Format("<xml><ToUserName>{0}</ToUserName><FromUserName>{1}</FromUserName><CreateTime>{2}</CreateTime><MsgType>text</MsgType><Content>{3}</Content></xml>",
                FromUserName.InnerText,
                ToUserName.InnerText,
                DateTime.Now.Ticks,
                message);

            }
            return responseContent;
        }


        string getdata(string code)
        {
            var result = "";
            if (code.Substring(0, 1) == "0")
            {
                code = "sz" + code;
            }
            else if (code.Substring(0, 1) == "6")
            {
                code = "sh" + code;
            }
            String getURL = "http://hq.sinajs.cn/list="+code;
            HttpWebRequest MyRequest = (HttpWebRequest)HttpWebRequest.Create(getURL);
            HttpWebResponse MyResponse = (HttpWebResponse)MyRequest.GetResponse();
            HttpWebResponse Response = MyResponse;
            using (Stream MyStream = MyResponse.GetResponseStream())
            {
                long ProgMaximum = MyResponse.ContentLength;
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = MyStream.Read(by, 0, by.Length);
                Encoding encoding = Encoding.GetEncoding("GB2312");
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    result += encoding.GetString(by, 0, osize);
                    long ProgValue = totalDownloadedByte;
                    osize = MyStream.Read(by, 0, by.Length);
                }
            }
            if(result.Length<35)
            {
                return "无法找到该股票信息";
            }
            result = result.Remove(0, result.IndexOf("\"") + 1);
            result = result.Replace("\"", "");
            result = result.Replace(";", "");
            var resultdata = result.Split(',');
            string data = "";
            data += "股票名称:" + resultdata[0] + "元\r\n";
            data += "今日开盘价:" + resultdata[1] + "元\r\n";
            data += "昨日收盘价:" + resultdata[2] + "元\r\n";
            data += "当前价格:" + resultdata[3] + "元\r\n";
            data += "今日最高价:" + resultdata[4] + "元\r\n";
            data += "今日最低价:" + resultdata[5] + "元\r\n";
            data += "竞买价，即“买一”报价:" + resultdata[6] + "元\r\n";
            data += "竞卖价，即“卖一”报价:" + resultdata[7] + "元\r\n";
            data += "成交的股票数:" + (Convert.ToInt32(resultdata[8]) / 100).ToString() + "手\r\n";
            data += "成交金额:" + (Convert.ToDecimal(resultdata[9]) / 10000).ToString() + "万元\r\n";
            data += "时间:" + DateTime.Now.ToString() + "\r\n";
            //解析json
            return data;
            //
        }
    }
}