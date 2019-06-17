using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GUI.States
{
    public class DefaultRaffleState : RaffleStateInterface
    {
        private MediaPlayer mMediaPlayer = new MediaPlayer();
        private TextBlock mDisplayWinnerName = null;
        public override void DisplayAreaInitialize(StackPanel displayArea, TextBlock nameDisplay)
        {
            mDisplayWinnerName = nameDisplay;

            Image image = new Image();

            var uriSource = new Uri(@"Resources/maxresdefault.jpg", UriKind.Relative);
            image.Source = new BitmapImage(uriSource);
            
            
            displayArea.Children.Add(image);

            FileInfo fInfoAudio = new FileInfo("Resources/MarioKartBox.m4a");
            mMediaPlayer.Open(new System.Uri("file:///" + fInfoAudio.FullName));
        }

        public override void DisplayWinner(Player winner)
        {
            mDisplayWinnerName.Dispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                        mDisplayWinnerName.Foreground = new SolidColorBrush(Colors.Red);
                        mDisplayWinnerName.Text = winner.FullName;
                    })
                );
        }

        public override void Fireworks(List<Player> players)
        {
            const int TIME_INCREMENT = 10;
            int index = 0;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => 
            {
                mMediaPlayer.Position = TimeSpan.Zero;
                mMediaPlayer.Play();
            }));
            
            for (int time = 0; time < 3500; time += TIME_INCREMENT)
            {

                mDisplayWinnerName.Dispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                        if (index >= players.Count)
                        {
                            index = 0;
                        }
                        mDisplayWinnerName.Text = players[index].FullName;
                        index++;
                    })
                );
                Thread.Sleep(TIME_INCREMENT);

            }
        }
    }
}
