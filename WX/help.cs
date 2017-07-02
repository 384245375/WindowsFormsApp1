using System;
using System.Collections.Generic;
using System.Linq;
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
                            
                        }
                     }
                  
                    Class1 a = new Class1();
                    var r = a.ConnectTuLing(Content.InnerText);
                    responseContent = string.Format("<xml><ToUserName>{0}</ToUserName><FromUserName>{1}</FromUserName><CreateTime>{2}</CreateTime><MsgType>text</MsgType><Content>{3}</Content></xml>",
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    r);
              
            }
            return responseContent;
        }
    }
}