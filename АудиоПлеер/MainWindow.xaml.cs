using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using NPOI.OpenXmlFormats.Dml;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace АудиоПлеер
{
    public partial class MainWindow : Window
    {
        int a = 0;
        private bool _isPlaying = false;
        private List<string> _playlist = new List<string>();
        private bool isRepeat = false;
        private bool isShuffle = false;



        public MainWindow()
        {
            InitializeComponent();
            Thread t = new Thread(ChangeSeconds);
            t.Start();

        }
        private void ChangeSeconds()
        {
            while (true)
            {
                Thread.Sleep(1000);
                Dispatcher.Invoke(() =>
                {
                    Timer.Text = media.Position.Minutes + ":" + media.Position.Seconds;
                    var currentPosition = media.Position.Ticks;
                    SliderMusic.Value = currentPosition;
                });

                Thread.Sleep(100);
            }
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog oFD = new CommonOpenFileDialog { IsFolderPicker = true };
            var res = oFD.ShowDialog();
            if (res == CommonFileDialogResult.Ok)
            {
                _playlist = Directory.GetFiles(oFD.FileName)
                 .Where(file => file.EndsWith(".mp3") || file.EndsWith(".m4a") || file.EndsWith(".wav")).ToList();
                foreach (string files in _playlist)
                {
                    MediaList.Items.Add(files);
                }
            }
            media.Source = new Uri("C:\\Users\\valiu\\Desktop\\Свалка\\Музыка\\Artik_Asti_-_Devochka_tancujj_68289048.mp3");
            media.Play();
        }

        private void SliderMusic_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = new TimeSpan(Convert.ToInt64(SliderMusic.Value));
            Timer.Text = media.Position.Minutes + ":" + media.Position.Seconds;
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            SliderMusic.Maximum = media.NaturalDuration.TimeSpan.Ticks;
            Timer.Text = media.NaturalDuration.TimeSpan.Minutes + ":" + media.NaturalDuration.TimeSpan.Seconds;
        }


        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Volume = 0.2;
            media.Volume = SliderVolume.Value;
        }

        private void StartPauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
            {
                media.Pause();
                _isPlaying= false;
                
            }
            else
            {
                media.Play();
                _isPlaying= true;
            }
            
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
            {
                media.Stop();
                _isPlaying= false;
            }
        }

        private void RepeatBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void RandomBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = _playlist.IndexOf(media.Source.LocalPath);
            var newIndex = currentIndex == _playlist.Count - 1 ? 0 : currentIndex + 1;
            media.Source = new Uri(_playlist[newIndex]);
            _isPlaying = true;
            media.Play();
            Timer.Text = Path.GetFileNameWithoutExtension(_playlist[newIndex]);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = _playlist.IndexOf(media.Source.LocalPath);
            var newIndex = currentIndex == _playlist.Count - 1 ? 0 : currentIndex - 1;
            media.Source = new Uri(_playlist[newIndex]);
            _isPlaying = true;
            media.Play();
            Timer.Text = Path.GetFileNameWithoutExtension(_playlist[newIndex]);
        }

    }
}
