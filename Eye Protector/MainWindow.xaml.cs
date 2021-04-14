using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Eye_Protector
{
    public partial class MainWindow : Window
    {
        private readonly TimeSpan _workTime = new TimeSpan(0, 20, 0);
        private readonly TimeSpan _breakTime = new TimeSpan(0, 0, 20);

        private readonly Window _blackWindow;

        private TimeSpan _elapsedTime;

        public MainWindow()
        {
            InitializeComponent();

            _elapsedTime = _workTime;

            var timer = new Timer(1000);
            timer.Elapsed += TimerOnElapsed;
            timer.Start();

            _blackWindow = new Window
            {
                Background = new SolidColorBrush(Colors.Black),
                WindowState = WindowState.Maximized,
                WindowStyle = WindowStyle.None,
                Topmost = true,
                Cursor = Cursors.None,
                Content = new Label
                {
                    Content = "Take a break. Look at an object 20 feet away.",
                    Foreground = new SolidColorBrush(Colors.LightGray),
                    FontSize = 64,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                }
            };

            _blackWindow.Closing += (sender, args) => args.Cancel = true;
            Closing += (sender, args) => Application.Current.Shutdown();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            _elapsedTime -= TimeSpan.FromSeconds(1);

            Dispatcher.Invoke(() =>
            {
                ElapsedTimeLabel.Content = _elapsedTime.ToString("mm\\:ss");

                if (_elapsedTime != TimeSpan.Zero) return;
                if (!_blackWindow.IsVisible)
                {
                    _blackWindow.Show();
                    _elapsedTime = _breakTime;
                }
                else
                {
                    _blackWindow.Hide();
                    _elapsedTime = _workTime;
                }
            });
        }
    }
}
