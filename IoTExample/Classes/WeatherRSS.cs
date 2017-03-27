using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Data.Xml.Dom;
using Windows.UI.Popups;
using IoTExample.Classes;
using Windows.UI.Xaml.Media.Imaging;

namespace IoTExample.Classes
{
    class WeatherRSS
    {
        public static string FForecast { get; set; }
        public static string SForecast { get; set; }
        public async static void GetWeather()
        {
            Regex reg = new Regex(@"\S*</wfKor>");
            string[] splited;
            string source = "";
            try
            {
                HttpClient http = new HttpClient();
                var response = await http.GetByteArrayAsync("http://www.kma.go.kr/wid/queryDFS.jsp");
                source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                source = source.Substring(40);
                splited = source.Split("<wfKor>".ToCharArray());
                FForecast = splited[69];
                SForecast = splited[191];
            }
            catch(Exception e)
            {
                var dlg = new MessageDialog(e.Message);
                await dlg.ShowAsync();
            }
            
        }
        
        public static BitmapImage GetWeatherName(string Weather)
        {
            /*
            맑음
            ○ 구름조금
            ○ 구름많음, 구름많고 비, 구
            름많고 비/눈, 구름많고 눈/비,
            구름많고 눈
            ○ 흐림, 흐리고 비, 흐리고 비/
            눈, 흐리고 눈/비, 흐리고 눈
             */
            //BitmapImage bitmapImage;
            switch (Weather)
            {
                case "맑음":
                    return new BitmapImage(new Uri("ms-appx:///Resources/Weather/Sunny.png"));
                case "구름조금":
                    return new BitmapImage(new Uri("ms-appx:///Resources/Weather/Cloudy_Sunny.png"));
                case "구름많음":
                    return new BitmapImage(new Uri("ms-appx:///Resources/Weather/Cloudy_Sun.png"));
                case "구름많고 비":
                    return new BitmapImage(new Uri("ms-appx:///Resources/Weather/Shower.png"));
                case "구름많고 비/눈":
                    return new BitmapImage(new Uri("ms-appx:///Resources/Weather/Shower.png"));
                case "구름많고 눈/비":
                    return new BitmapImage(new Uri("ms-appx:///Resources/Weather/Snow.png"));
                case "구름많고 눈":
                    return new BitmapImage(new Uri("ms-appx:///Resources/Weather/Snow.png"));
                case "흐림":
                    return new BitmapImage(new Uri("ms-appx:///Resources/Weather/Cloudy.png"));
                case "흐리고 비":
                    return new BitmapImage(new Uri("ms-appx:///Resources/Weather/Rainy.png"));
                case "흐리고 비/눈":
                    return new BitmapImage(new Uri("ms-appx:///Resources/Weather/Rainy.png"));
                case "흐리고 눈/비":
                    return new BitmapImage(new Uri("ms-appx:///Resources/Weather/Snow.png"));
                case "흐리고 눈":
                    return new BitmapImage(new Uri("ms-appx:///Resources/Weather/Snow.png"));
                default:
                   return new BitmapImage(new Uri("ms-appx://IoTExample/Resources/Weather/Sunny.png"));
            }
        }

    }
}
