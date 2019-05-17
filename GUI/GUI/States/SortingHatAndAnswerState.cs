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
        private MediaPlayer[] mRandomPreSelectLine = null;
        private MediaPlayer[] mRandomOnSelectLine = null;

        private TextBlock mDisplayWinnerName = null;
        private SortingHatAndAnswerView mDisplayArea = null;
        public void DisplayAreaInitialize(StackPanel displayArea, TextBlock nameDisplay)
        {
            mDisplayWinnerName = nameDisplay;

            mDisplayArea = new SortingHatAndAnswerView();
            displayArea.Children.Add(mDisplayArea);

            FileInfo fInfoAudio = new FileInfo("MarioKartBox.m4a");
            mMediaPlayer.Open(new System.Uri("file:///" + fInfoAudio.FullName));

            string[] loaders = Properties.Settings.Default.PreSelectAudioList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] selected = Properties.Settings.Default.PostSelectAudioList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            mRandomPreSelectLine = LoadAudioFiles(loaders);
            mRandomOnSelectLine = LoadAudioFiles(selected);
        }

        private MediaPlayer[] LoadAudioFiles(string[] items)
        {
            var retVal = new MediaPlayer[items.Length];
            for (int iter = 0; iter < items.Length; iter++)
            {
                FileInfo fInfoAudio = new FileInfo(items[iter]);
                retVal[iter] = new MediaPlayer();
                retVal[iter].Open(new System.Uri("file:///" + fInfoAudio.FullName));
            }
            return retVal;
        }

        public void DisplayWinner(Player winner)
        {
            mDisplayWinnerName.Dispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                        mDisplayWinnerName.Foreground = new SolidColorBrush(Colors.Red);
                        mDisplayWinnerName.Text = winner.FirstName + " " + winner.LastName;

                        mDisplayArea.Answer.Text = winner.Answers[0].Value;

                        if (mRandomOnSelectLine.Length > 0)
                        {
                            int item = Lottery.GenerateValue(0, mRandomOnSelectLine.Length - 1);
                            mRandomOnSelectLine[item].Position = TimeSpan.Zero;
                            mRandomOnSelectLine[item].Play();
                        }
                    })
                );
        }

        public void Fireworks(List<Player> players)
        {
            const int TIME_INCREMENT = 10;
            int index = 0;


            int item = Lottery.GenerateValue(0, mRandomPreSelectLine.Length - 1);

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                mMediaPlayer.Position = TimeSpan.Zero;
                mMediaPlayer.Play();

                if (mRandomPreSelectLine.Length > 0)
                {
                    mRandomPreSelectLine[item].Position = TimeSpan.Zero;
                    mRandomPreSelectLine[item].Play();
                }
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
