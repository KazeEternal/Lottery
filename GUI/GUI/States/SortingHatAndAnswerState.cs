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
        private MediaPlayer mMPlayerSpinningBox = new MediaPlayer();
        private MediaPlayer mMPlayerSelectedSound = new MediaPlayer();
        private MediaPlayer[] mRandomPreSelectLine = null;
        private MediaPlayer[] mRandomOnSelectLine = null;

        private TextBlock mDisplayWinnerName = null;
        private SortingHatAndAnswerView mDisplayArea = null;
        private bool mIsRunning;

        public override void DisplayAreaInitialize(StackPanel displayArea, TextBlock nameDisplay)
        {
            mDisplayWinnerName = nameDisplay;

            mDisplayArea = new SortingHatAndAnswerView();
            displayArea.Children.Add(mDisplayArea);

            FileInfo fInfoSpinningBox = new FileInfo("Audio/SpinningBox.wav");
            mMPlayerSpinningBox.Open(new System.Uri("file:///" + fInfoSpinningBox.FullName));
            mMPlayerSpinningBox.MediaEnded += MMPlayerSpinningBox_MediaEnded;
            mMPlayerSpinningBox.MediaFailed += MMPlayerSpinningBox_MediaFailed;

            FileInfo fInfoSelectedSound = new FileInfo("Audio/SelectedSound.wav");
            mMPlayerSelectedSound.Open(new System.Uri("file:///" + fInfoSelectedSound.FullName));

            string[] loaders = Properties.Settings.Default.PreSelectAudioList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] selected = Properties.Settings.Default.PostSelectAudioList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            mRandomPreSelectLine = LoadAudioFiles(loaders);
            mRandomOnSelectLine = LoadAudioFiles(selected);
        }

        private void MMPlayerSpinningBox_MediaFailed(object sender, ExceptionEventArgs e)
        {
            throw new NotImplementedException();
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

        public override void DisplayWinner(Player winner)
        {
            mDisplayWinnerName.Dispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                        mDisplayWinnerName.Foreground = new SolidColorBrush(Colors.Red);
                        mDisplayWinnerName.Text = winner.FirstName + " " + winner.LastName;

                        mDisplayArea.Answer.Text = winner.Answers[0].Value;

                        mMPlayerSelectedSound.Position = TimeSpan.Zero;
                        mMPlayerSelectedSound.Play();

                        if (mRandomOnSelectLine.Length > 0)
                        {
                            int item = Lottery.GenerateValue(0, mRandomOnSelectLine.Length - 1);
                            mRandomOnSelectLine[item].Position = TimeSpan.Zero;
                            mRandomOnSelectLine[item].Play();
                        }
                    })
                );
        }

        public override void Fireworks(List<Player> players)
        {
            int item = Lottery.GenerateValue(0, mRandomPreSelectLine.Length - 1);
            const int TIME_INCREMENT = 10;
            int index = 0;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                mMPlayerSpinningBox.Position = TimeSpan.Zero;
                mMPlayerSpinningBox.Play();
                
                if (mRandomPreSelectLine.Length > 0)
                {
                    mRandomPreSelectLine[item].Position = TimeSpan.Zero;
                    mRandomPreSelectLine[item].Play();
                }
            }));

            mIsRunning = true;
            while(mIsRunning)
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

        private void MMPlayerSpinningBox_MediaEnded(object sender, EventArgs e)
        {
            
            mIsRunning = false;
            OnShapeChanged(States.FireworksDone);

            
            mDisplayArea.Answer.Visibility = Visibility.Hidden;
        }

        public override void ShowAnswer()
        {
            mDisplayArea.Answer.Visibility = Visibility.Visible;
        }
    }
}
