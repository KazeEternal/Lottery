using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Core;

namespace GUI.States
{
    public class SortingHatAndAnswerState : RaffleStateInterface
    {
        private MediaPlayer mMediaPlayer = new MediaPlayer();
        private TextBlock mDisplayWinnerName = null;
        public void DisplayAreaInitialize(StackPanel displayArea, TextBlock nameDisplay)
        {
            mDisplayWinnerName = nameDisplay;

            FileInfo fInfoAudio = new FileInfo("MarioKartBox.m4a");
            mMediaPlayer.Open(new System.Uri("file:///" + fInfoAudio.FullName));
        }

        public void DisplayWinner(Player winner)
        {
            mDisplayWinnerName.Dispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                        mDisplayWinnerName.Foreground = new SolidColorBrush(Colors.Red);
                        mDisplayWinnerName.Text = winner.FirstName + " " + winner.LastName;
                    })
                );
        }

        public void Fireworks(List<Player> players)
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
                        mDisplayWinnerName.Text = players[index].FirstName + " " + players[index].LastName;
                        index++;
                    })
                );
                Thread.Sleep(TIME_INCREMENT);

            }
        }
    }
}
