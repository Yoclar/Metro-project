using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Metro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static List<Station> stations = new();
        private static SoundPlayer soundPlayer = new SoundPlayer();
        private static Direction direction = Direction.LeftToRight;

        public MainWindow()
        {
            InitializeComponent();
            foreach (var child in Canvas.Children)
            {
                if (child.GetType() == typeof(Ellipse))
                {
                    var ellipse = (Ellipse)child;
                    stations.Add(new Station()
                    {
                        Position = GetCenter(ellipse),
                        Name = ellipse.Name,
                        AnnouncementAudio = ellipse.Name.Replace('_', ' ') + ".wav",
                        NextStationAudio = ellipse.Name.Replace('_', ' ') + " következik.wav",
                    });
                }
            }
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                int metroPosition = default!;
                bool isRunning = true;
                while (isRunning)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        metroPosition = GetCenter(metroIcon);
                    });

                    var isAtStation = stations.Any(x => x.Position == metroPosition);
                    if (isAtStation)
                    {
                        var currentStation = stations.First(x => x.Position == metroPosition);
                        var indexOffset = (direction == Direction.LeftToRight ? 1 : -1);
                        var currentIndex = stations.IndexOf(currentStation);
                        
                        if (direction == Direction.LeftToRight && currentIndex == 0)
                        {
                            PlaySound("Örs felé.wav");
                        }
                        else if (direction == Direction.RightToLeft && currentIndex == stations.Count - 1)
                        {
                            PlaySound("Déli felé.wav");
                        }
                        if ((direction == Direction.LeftToRight && currentIndex != 0) ||
                            direction == Direction.RightToLeft && currentIndex != stations.Count - 1)
                        {
                            PlaySound(currentStation.AnnouncementAudio);
                            Task.Delay(40);
                        }
                        var nextIndex = currentIndex + indexOffset;
                        if (nextIndex >= 0 && nextIndex < stations.Count)
                        {
                            var nextStation = stations[nextIndex];
                            if ((direction == Direction.LeftToRight && currentIndex != 0) ||
                                direction == Direction.RightToLeft && currentIndex != stations.Count - 1)
                            {
                                if (direction == Direction.LeftToRight && currentIndex == 4 || currentIndex == 7)
                                {
                                    PlaySound("M2-es metró az Örs felé.wav");
                                }
                                else if (direction == Direction.RightToLeft &&  currentIndex == 7 || currentIndex == 4)
                                {
                                    PlaySound("M2-es a déli felé.wav");
                                }
                                PlaySound(nextStation.NextStationAudio);
                            }
                        }
                        if ((direction == Direction.LeftToRight && currentIndex == stations.Count - 1) ||
                            (direction == Direction.RightToLeft && currentIndex == 0))
                        {
                            isRunning = false;
                        }
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var left = Canvas.GetLeft(metroIcon);
                        if (direction == Direction.LeftToRight)
                        {
                            left++;
                        }
                        else
                        {
                            left--;
                        }
                        Canvas.SetLeft(metroIcon, left);
                    });

                    Thread.Sleep(50);
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var transformGroup = new TransformGroup();
                    var scaleX = direction == Direction.LeftToRight ? -1 : 1;
                    transformGroup.Children.Add(new ScaleTransform(scaleX, 1));
                    metroIcon.RenderTransform = transformGroup;
                    metroIcon.RenderTransformOrigin = new Point(0.5, 0.5);
                    direction = direction == Direction.LeftToRight ? Direction.RightToLeft : Direction.LeftToRight;
                });
            });

        }

        private static void PlaySound(string path)
        {
            try
            {
                Debug.WriteLine($"Playing sound {path}");
                soundPlayer.SoundLocation = $@"..\..\..\Sounds\{path}";
                soundPlayer.Load();
                soundPlayer.PlaySync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static int GetCenter(FrameworkElement control)
        {
            var left = (int)Canvas.GetLeft(control);
            var width = (int)control.Width;
            return left + width / 2;
        }

    }
}




