using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
   public  class Class1
    {

        public string ConnectTuLing(string p_strMessage)
        {
            string result = null;
            try
            {
                //注册码自己到网上注册去
                String APIKEY = "a78f97b4abff444390375daef5ab667c";
                String _strMessage = p_strMessage;
                String INFO = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(_strMessage));
                String getURL = "http://www.tuling123.com/openapi/api?key=" + APIKEY + "&info=" + INFO;
                HttpWebRequest MyRequest = (HttpWebRequest)HttpWebRequest.Create(getURL);
                HttpWebResponse MyResponse = (HttpWebResponse)MyRequest.GetResponse();
                HttpWebResponse  Response = MyResponse;
                using (Stream MyStream = MyResponse.GetResponseStream())
                {
                    long ProgMaximum = MyResponse.ContentLength;
                    long totalDownloadedByte = 0;
                    byte[] by = new byte[1024];
                    int osize = MyStream.Read(by, 0, by.Length);
                    Encoding encoding = Encoding.UTF8;
                    while (osize > 0)
                    {
                        totalDownloadedByte = osize + totalDownloadedByte;
                        result += encoding.GetString(by, 0, osize);
                        long ProgValue = totalDownloadedByte;
                        osize = MyStream.Read(by, 0, by.Length);
                    }
                }
                //解析json
                JsonReader reader = new JsonTextReader(new StringReader(result));
                while (reader.Read())
                {
                    
                    //text中的内容才是你需要的
                    if (reader.Path == "text")
                    {
                        //结果赋值
                        result = reader.Value.ToString();
                    }
                    if (reader.Path == "url")
                    {
                        //结果赋值
                        result += reader.Value.ToString();
                    }
                    Console.WriteLine(reader.TokenType + "\t\t" + reader.ValueType + "\t\t" + reader.Value);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }


}
