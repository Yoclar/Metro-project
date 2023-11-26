using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Metro;

class MetroVehicle
{
    private Direction direction;
    private Image metroIcon;
    private List<Station> stations;
    private bool isAtStation;
    private Station? currentStation;
    private int currentStationIndex;
    private Station? nextStation;
    private bool isAtFirstStation;
    private bool isAtLastStation;

    public MetroVehicle(Image metroIcon, List<Station> stations)
    {
        direction = Direction.LeftToRight;
        this.metroIcon = metroIcon;
        this.stations = stations;
    }

    public void UpdateProperties()
    {
        var metroPosition = Utilities.GetCenter(metroIcon);
        currentStation = stations.FirstOrDefault(x => x.Position == metroPosition);
        isAtStation = currentStation is not null;
        if(!isAtStation)
        {
            nextStation = null;
            currentStationIndex = -1;
            isAtFirstStation = false;
            isAtLastStation = false;
            return;
        }

        currentStationIndex = stations.IndexOf(currentStation!);
        isAtFirstStation = currentStationIndex == 0;
        isAtLastStation= currentStationIndex == stations.Count - 1;
        if (direction == Direction.LeftToRight && !isAtLastStation)
        {
            nextStation = stations[currentStationIndex + 1];
        }
        else if(direction == Direction.RightToLeft && !isAtFirstStation)
        {
            nextStation = stations[currentStationIndex - 1];
        }
    }

    public void ChangeDirection()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            int scaleX;
            if (direction == Direction.LeftToRight)
            {
                direction = Direction.RightToLeft;
                scaleX = -1;
            }
            else
            {
                direction = Direction.LeftToRight;
                scaleX = 1;
            }

            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(scaleX, 1));
            metroIcon.RenderTransform = transformGroup;
            metroIcon.RenderTransformOrigin = new Point(0.5, 0.5);
        });
    }

    public void MoveForward()
    {
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
    }

    public void Go()
    {
        bool isRunning = true;
        while (isRunning)
        {
            UpdateProperties();
            if (isAtStation)
            {
                isRunning = PlaySoundsAtStation();
            }

            MoveForward();
            Thread.Sleep(50);
        }

        ChangeDirection();
    }

    private bool PlaySoundsAtStation()
    {
        TryPlayingStartingSound();

        var isNotAtStartStation = (direction == Direction.LeftToRight && !isAtFirstStation) ||
            (direction == Direction.RightToLeft && !isAtLastStation);
        if (isNotAtStartStation)
        {
            Utilities.PlaySound(currentStation!.AnnouncementAudio);
            Thread.Sleep(500);

            if (nextStation is not null)
            {
                TryPlayingChangeMetroSound();
                Utilities.PlaySound(nextStation.NextStationAudio);
            }
        }
        
        return nextStation is not null;
    }

    private void TryPlayingChangeMetroSound()
    {
        if (direction == Direction.LeftToRight && (currentStationIndex == 4 || currentStationIndex == 7))
        {
            Utilities.PlaySound("M2-es metró az Örs felé.wav");
        }
        else if (direction == Direction.RightToLeft && (currentStationIndex == 7 || currentStationIndex == 4))
        {
            Utilities.PlaySound("M2-es a déli felé.wav");
        }
    }

    private void TryPlayingStartingSound()
    {
        if (direction == Direction.LeftToRight && isAtFirstStation)
        {
            Utilities.PlaySound("Örs felé.wav");
        }
        else if (direction == Direction.RightToLeft && isAtLastStation)
        {
            Utilities.PlaySound("Déli felé.wav");
        }
    }
}
