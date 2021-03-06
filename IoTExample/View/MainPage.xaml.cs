﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using IoTExample.Classes;
using Windows.Media.Capture;
using Windows.ApplicationModel;
using Windows.System.Display;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Media.SpeechSynthesis;
using Windows.ApplicationModel.Resources.Core;
using System.Diagnostics;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace IoTExample
{

    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MediaCapture _mediaCapture;
        public bool _isPreviewing;
        public DisplayRequest _displayRequest;
        public DispatcherTimer Timer = new DispatcherTimer();
        MusicLoader _musicLoader = new MusicLoader();
        private SpeechSynthesizer synthesizer;
        public MainPage()
        {
            this.InitializeComponent();
            WeatherRSS.GetWeather();
            synthesizer = new SpeechSynthesizer();
            LabelTime.Text = DateTime.Now.ToString("hh:mm");
            LabelWeekDay.Text = DateTime.Now.ToString("yyyy-MM-dd, ddd");
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
            StartCapture();
            var voices = SpeechSynthesizer.AllVoices;
            foreach (var voice in voices)
            {
                if (voice.Language == "ko-KR")
                {
                    synthesizer.Voice = voice;
                }
            }
        }

        public async void StartCapture()
        {
            await StartPreviewAsync();
        }

        private void Timer_Tick(object sender, object e)
        {
            LabelTime.Text = DateTime.Now.ToString("hh:mm");
            LabelWeekDay.Text = DateTime.Now.ToString("yyyy-MM-dd, ddd");
            ImgWeather.Source = WeatherRSS.GetWeatherName(WeatherRSS.FForecast);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ImgWeather.Source = WeatherRSS.GetWeatherName(WeatherRSS.FForecast);
            ImgNextweather.Source = WeatherRSS.GetWeatherName(WeatherRSS.SForecast);
            _musicLoader.Load_MusicAsync();
        }

        private async Task StartPreviewAsync()
        {
            try
            {
                _mediaCapture = new MediaCapture();
                await _mediaCapture.InitializeAsync();

                PreviewControl.Source = _mediaCapture;
                await _mediaCapture.StartPreviewAsync();
                _isPreviewing = true;

                _displayRequest.RequestActive();
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            }
            catch (UnauthorizedAccessException)
            {
                // This will be thrown if the user denied access to the camera in privacy settings
                System.Diagnostics.Debug.WriteLine("The app was denied access to the camera");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("MediaCapture initialization failed. {0}", ex.Message);
            }
        }

        private async void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                string order = textBox.Text;
                string[] Parsed_Order = order.Split(' ');
                int cnt = 0;
                string returnValue = "";

               
                textBox.Text = "";
                if (order.Contains("노래"))
                {

                    foreach (var music in MusicLoader.MusicDB)
                    {
                        if (cnt <= 10)
                        {
                            if (music.Emotion.Contains(Parsed_Order[0]))
                            {
                                cnt++;
                                returnValue += $" {music.Title} - {music.Singer}, {music.Emotion} \r\n";
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (order.Contains("유튜브"))
                {
                    WebView1.Navigate(new Uri("https://www.youtube.com/watch?v=" + Parsed_Order[0]));
                    WebView1.Visibility = Visibility.Visible;
                }
                try
                {
                    SpeechSynthesisStream synthesisStream = await synthesizer.SynthesizeTextToStreamAsync(returnValue);

                    // Set the source and start playing the synthesized audio stream.
                    media.AutoPlay = true;
                    media.SetSource(synthesisStream, synthesisStream.ContentType);
                    media.Play();
                }
                catch (Exception)
                {

                }
                ContentDialog c = new ContentDialog()
                {
                    Content = returnValue
                };
                await c.ShowAsync();
            }
        }
    }
}
