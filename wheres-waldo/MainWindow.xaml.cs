using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using Tobii.Interaction;

namespace wheres_waldo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int startZ = 2;
        public int StartZ
        {
            get { return startZ; }
            set
            {
                startZ = value;
                OnPropertyChanged();
            }
        }

        private int stopZ = -1;
        public int StopZ
        {
            get { return stopZ; }
            set
            {
                stopZ = value;
                OnPropertyChanged();
            }
        }

        private string elapsedTime = "00:00.00";
        public string ElapsedTime
        {
            get { return elapsedTime; }
            set
            {
                elapsedTime = value;
                OnPropertyChanged();
            }
        }

        private double gazeX = 930;
        public double GazeX
        {
            get { return gazeX; }
            set
            {
                gazeX = value;
                OnPropertyChanged();
            }
        }

        private double gazeY = 510;
        public double GazeY
        {
            get { return gazeY; }
            set
            {
                gazeY = value;
                OnPropertyChanged();
            }
        }

        private string gazeLocation = "930,510,0,0";
        public string GazeLocation
        {
            get { return gazeLocation; }
            set
            {
                gazeLocation = value;
                OnPropertyChanged();
            }
        }

        private void SetGazeLocation(double gazePointX, double gazePointY)
        {
            gazeX = gazePointX;
            gazeY = gazePointY;
            GazeLocation = gazeX + "," + gazeY + ",0,0";
        }

        private DispatcherTimer timer;
        private Stopwatch stopwatch;

        private void Start_Game(object sender, RoutedEventArgs e)
        {
            StartZ = -1;
            timer = new DispatcherTimer();
            stopwatch = new Stopwatch();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += new EventHandler(dispatcherTimer_Tick);
            stopwatch.Start();
            timer.Start();
        }

        private void Stop_Game()
        {
            stopwatch.Stop();
            timer.Stop();
            StopZ = 2;
        }

        private int xMin = 1520;
        private int yMin = 80;
        private int xMax = 1555;
        private int yMax = 110;

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopwatch.Elapsed;
            ElapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            // Update eye tracking point + finish puzzle logic
            if (gazeX >= xMin && gazeX <= xMax && gazeY >= yMin && gazeY <= yMax)
            {
                Stop_Game();
            }
        }

        public MainWindow()
        {
            var gazePointDataStream = (Application.Current as wheres_waldo.App)._host.Streams.CreateGazePointDataStream();
            gazePointDataStream.GazePoint((gazePointX, gazePointY, _) => SetGazeLocation(gazePointX, gazePointY));
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}
