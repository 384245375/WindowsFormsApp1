using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Class1 a = new Class1();
            var r = a.ConnectTuLing(richTextBox2.Text);
            getdata("600166");
            richTextBox1.Text += "\r\n" + r;
            richTextBox2.Text = "";
        }


         string getdata(string code)
        {
            var result = "";
            if (code.Substring(0, 1)=="0")
            {
                code = "sz" + code;
            }else if(code.Substring(0, 1) == "6")
            {
                code = "sh" + code;
            }
            String getURL = "http://hq.sinajs.cn/list=sh000000";
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
            var zzz= result;
            result = result.Remove(0, result.IndexOf("\"")+1);
            result = result.Replace("\"", "");
            result = result.Replace(";", "");
            var resultdata = result.Split(',');
            string data = "";
            data += "股票名称:" + resultdata[0]+"元\r\n";
            data += "今日开盘价:" + resultdata[1] + "元\r\n";
            data += "昨日收盘价:" + resultdata[2] + "元\r\n";
            data += "当前价格:" + resultdata[3] + "元\r\n";
            data += "今日最高价:" + resultdata[4] + "元\r\n";
            data += "今日最低价:" + resultdata[5] + "元\r\n";
            data += "竞买价，即“买一”报价:" + resultdata[6] + "元\r\n";
            data += "竞卖价，即“卖一”报价:" + resultdata[7] + "元\r\n";
            data += "成交的股票数:" +(Convert.ToInt32(resultdata[8])/100).ToString() + "手\r\n";
            data += "成交金额:" +(Convert.ToDecimal(resultdata[9])/10000).ToString()+ "万元\r\n";
            data += "时间:" + DateTime.Now.ToString() + "\r\n";
            //解析json
            JsonReader reader = new JsonTextReader(new StringReader(result));
            return "";
            //
        }
    }
}
