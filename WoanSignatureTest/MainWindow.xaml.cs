using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//添加WoanSignatureTest.dll的引用
namespace WoanSignatureTest{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //get方式调用服务器接口
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //填写查寻密钥
            string queryKey = "abc";
            //定义要访问的云平台接口
            string uri = "/api/20140928/task_list";
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long Sticks = (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
            string timeStamp = Sticks.ToString();
            //填写服务器提供的服务码
            string queryString = "service_code=TESTING";
            WoanSignature.Signature Signature = new WoanSignature.Signature();
            string signature = Signature.SignatureForGet(uri, queryKey, queryString, timeStamp);
            WoanSignature.HttpTool HttpTool = new WoanSignature.HttpTool();
            string response = HttpTool.get("http://c.zhiboyun.com/api/20140928/task_list?" + queryString, signature, timeStamp);
            MessageBox.Show(response);
        }
        //post方式调用服务器接口
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //填写查寻密钥
            string queryKey = "abc";
            //定义要访问的云平台接口--接口详见服务器开发文档
            string uri="/api/20140928/management";
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long Sticks = (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
            string timeStamp = Sticks.ToString();
            //填写服务器提供的服务码
            string queryString = "service_code=TESTING";
            WoanSignature.Signature Signature = new WoanSignature.Signature();
            //定义请求数据，以下json是一个删除文件的json请求
            JObject json = new JObject(
                new JProperty("function", "delete_video"),
                new JProperty("params",
                    new JObject(
                    new JProperty("service_code", "TESTING"),
                    new JProperty("file_name", "1.m3u8")
                    )
                 )
            );
            string signature = Signature.SignatureForPost(uri, queryKey, queryString, json.ToString(), timeStamp);
             WoanSignature.HttpTool HttpTool = new WoanSignature.HttpTool();
             string response = HttpTool.post("http://c.zhiboyun.com/api/20140928/management?" + queryString, signature, timeStamp, json.ToString());
            MessageBox.Show(response);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //challengestr,视频服务器在配置认证接口后，会将此值通知给接口
            WoanSignature.Challenge Challenge = new WoanSignature.Challenge();
            MessageBox.Show(Challenge.GetChallenge(this.pwd.Text, this.challengestr.Text));
        }
    }
}
