using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Metro;

public class Station
{
    public string Name { get; init; }
    public string AnnouncementAudio { get; init; }
    public string NextStationAudio { get; init; }
    public int Position { get; init; }

    public Station(string name, string announcementAudio, string nextStationAudio, int position)
    {
        Name = name;
        AnnouncementAudio = announcementAudio;
        NextStationAudio = nextStationAudio;
        Position = position;
    }

    public static List<Station> GenerateStations(Canvas canvas)
    {
        List<Station> stations = new();

        foreach (var child in canvas.Children)
        {
            if (child.GetType() == typeof(Ellipse))
            {
                var ellipse = (Ellipse)child;
                var soundName = ellipse.Name.Replace('_', ' ');
                var station = new Station(
                    name: ellipse.Name,
                    announcementAudio: $"{soundName}.wav",
                    nextStationAudio: $"{soundName} következik.wav",
                    position: Utilities.GetCenter(ellipse)
                );
                stations.Add(station);
            }
        }

        return stations;
    }
}
