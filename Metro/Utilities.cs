using System;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Controls;

namespace Metro;

class Utilities
{
    private static SoundPlayer soundPlayer = new SoundPlayer();

    public static int GetCenter(FrameworkElement control)
    {
        return Application.Current.Dispatcher.Invoke(() =>
        {
            var left = (int)Canvas.GetLeft(control);
            var width = (int)control.Width;
            return left + width / 2;
        });
    }

    public static void PlaySound(string path)
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
}
