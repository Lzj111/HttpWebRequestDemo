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

            this.textBox1.Text = "http://192.168.16.34:89/collection/service/receivedata/803965824542617600";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string url = this.textBox1.Text;
                string data = this.textBox2.Text;
                object obj = JsonConvert.DeserializeObject(data);
                object ret = Post<object, object>(url, "", obj);
                //MessageBox.Show(JsonConvert.SerializeObject(ret));
                ShowResult(JsonConvert.SerializeObject(ret));
            }
            catch (Exception ex)
            {
                ShowResult(ex.ToString());
                //MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="server"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public T2 Post<T1, T2>(string server, string url, T1 data)
        {
            try
            {
                //TODO:序列化
                //var serializer = new JavaScriptSerializer();
                var jsonText = JsonConvert.SerializeObject(data); //serializer.Serialize(new ExampleModel() { ISBN = "1234" });
                var jsonBytes = Encoding.UTF8.GetBytes(jsonText);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(server + url);
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/json";
                request.ContentLength = jsonBytes.Length;

                // 必须添加否则超时
                request.ServicePoint.Expect100Continue = false;

                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(jsonBytes, 0, jsonBytes.Length);
                    requestStream.Flush();
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var result = reader.ReadToEnd();
                        //MessageBox.Show(result);
                        T2 list = JsonConvert.DeserializeObject<T2>(result);
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ShowResult(ex.ToString());
                throw;
            }
        }

        public void ShowResult(string msg)
        {
            this.textBox3.Text = msg;
        }

    }

}
