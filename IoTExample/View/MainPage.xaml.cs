using System;
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

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace IoTExample
{
    
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public DispatcherTimer Timer = new DispatcherTimer();
        public MainPage()
        {
            this.InitializeComponent();
            LabelTime.Text = DateTime.Now.ToString("hh:mm");
            LabelWeekDay.Text = DateTime.Now.ToString("ddd");
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();

        }

        private void Timer_Tick(object sender, object e)
        {
            LabelTime.Text = DateTime.Now.ToString("hh:mm");
            LabelWeekDay.Text = DateTime.Now.ToString("ddd");
        }
    }
}
