using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
       
        private static List<double> stationPositions = new List<double> { 204, 274, 377, 429, 515, 591, 791, 885, 1013 };
    
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SoundPlayer snd = new SoundPlayer(@"E:\Metro_project\Metro\Metro\Sounds\Örs_felé.wav");
                snd.Load();
                snd.Play();
            }
            catch (Exception ex)
            {

            }
        }

 

    }
}

    


