using System.Threading.Tasks;
using System.Windows;

namespace Metro;

public partial class MainWindow : Window
{
    private MetroVehicle metro;

    public MainWindow()
    {
        InitializeComponent();

        var stations = Station.GenerateStations(Canvas);
        metro = new MetroVehicle(metroIcon, stations);
    }

    private void startBtn_Click(object sender, RoutedEventArgs e)
    {
        Task.Run(() =>
        {
            metro.Go();
        });
    }
}




