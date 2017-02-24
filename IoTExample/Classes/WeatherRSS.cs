using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Data.Xml.Dom;
using Windows.UI.Popups;

namespace IoTExample.Classes
{
    class WeatherRSS
    {
        public async static void GetWeather()
        {
            XmlDocument docX = new XmlDocument(); // XmlDocument 생성
            Regex reg = new Regex(@"\S*</wfKor>");
            string[] splited;
            string FForecast = "";
            string SForecast = "";
            String source = "";
            try
            {
                HttpClient http = new HttpClient();
                var response = await http.GetByteArrayAsync("http://www.kma.go.kr/wid/queryDFS.jsp");
                source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                source = source.Substring(40);
                splited = source.Split("<wfKor>".ToCharArray());
                FForecast = splited[69];
                SForecast = splited[191];
               // docX.LoadXml(source);
            }
            catch(Exception e)
            {
                var dlg = new MessageDialog(e.Message);
                await dlg.ShowAsync();
                //return;
            }
            var Dialog = new MessageDialog(FForecast);
            await Dialog.ShowAsync();
        }
    }
}
