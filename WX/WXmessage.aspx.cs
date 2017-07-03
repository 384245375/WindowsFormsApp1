
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WX
{
    public partial class WXmessage : System.Web.UI.Page
    {
        private readonly string Token = "tangyiming";//与微信公众账号后台的Token设置保持一致，区分大小写。
         public static readonly string EncodingAESKey = "pFXsye3G5XKITvsHzHvbroZzmTBHT9OS4K7gFqJStVx";//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
         public static readonly string AppId = "wx6c45db1ab8f0e40b";//与微信公众账号后台的AppId设置保持一致，区分大小写。
        protected void Page_Load(object sender, EventArgs e)
        {
            //string signature = Request["signature"];
            //string timestamp = Request["timestamp"];
            //string nonce = Request["nonce"];
            //string echostr = Request["echostr"];

            //if (Request.HttpMethod == "post")
            //{
            //    //get method - 仅在微信后台填写URL验证时触发
            //    if (CheckSignature.Check(signature, timestamp, nonce, Token))
            //    {
            //        WriteContent(echostr); //返回随机字符串则表示验证通过
            //    }
            //    else
            //    {
            //        WriteContent("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token));
            //    }

            //}
            //else
            //{
            //    //判断Post或其他方式请求
            //}

            //Stream s = System.Web.HttpContext.Current.Request.InputStream;
            //byte[] b = new byte[s.Length];
            //s.Read(b, 0, (int)s.Length);
            //WriteContent(Encoding.UTF8.GetString(b));
            //Response.End();


   
                //*********************************自动应答代码块*********************************
                string postString = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1348831860</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[thisisatest]]></Content><MsgId>1234567890123456</MsgId></xml>";
                using (Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    //接收的消息为GBK格式
                    postString = Encoding.GetEncoding("GBK").GetString(postBytes);
                    help h = new help();
                    string responseContent = h.ReturnMessage(postString);
                   
                    //返回的消息为UTF-8格式
                    HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                    HttpContext.Current.Response.Write(responseContent);
                    WriteContent(responseContent);
                    Label1.Text = responseContent;
                }
                //********************************自动应答代码块end*******************************
          
        }



        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>

      
        private void WriteContent(string str)
        {
            Response.Output.Write(str);
        }



    }
}