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
        private bool isRepeat = false;
        private bool isShuffle = false;
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();
        private List<string> _playlist = new List<string>();
        private bool _isMediaPlaying = false;

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
                this.Dispatcher.Invoke(new Action(() =>
                {
                    Timer.Text = media.Position.Minutes + ":" + media.Position.Seconds;
                    
                }));
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
                    ListBox.Items.Add(files);
                }
            }

            media.Source = new Uri("C:\\Users\\valiu\\Desktop\\Мусорка\\Музыка\\Artik_Asti_-_Devochka_tancujj_68289048.mp3");
            media.Play();
            media.Volume = 0.5;
            
        
        
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

        private void UpdatePosition()
        {
            while (true)
            {
                if (_isMediaPlaying)
                {
                    Dispatcher.Invoke(() =>
                    {
                        var totalTime = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        var currentPosition = _mediaPlayer.Position.TotalSeconds;

                        SliderMusic.Minimum = 0;
                        SliderMusic.Maximum = totalTime;
                        SliderMusic.Value = currentPosition;
                    });
                }

                Thread.Sleep(100);
            }
        }

        private void SliderValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (a == 1)
            {
                media.Play();
                a--;
            }
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            if(a == 0)
            {
                media.Stop();
                a++;
            }
            
            
        }

        private void RepeatBtn_Click(object sender, RoutedEventArgs e)
        {
            isRepeat = !isRepeat;
            if (isRepeat)
            {
                repeatButton.Opacity = 1;
            }
            else
            {
                repeatButton.Opacity = 0.5;
            }
        }

        private void RandomBtn_Click(object sender, RoutedEventArgs e)
        {
            isShuffle = !isShuffle;
            if (isShuffle)
            {
                shuffleButton.Opacity = 1;
            }
            else
            {
                shuffleButton.Opacity = 0.5;
            }
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
