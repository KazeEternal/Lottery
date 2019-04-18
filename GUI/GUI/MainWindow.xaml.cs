using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string PlayersFile { get; set; } = "Players.csv"; // @"D:\Development\Lottery\JunkFiles\Players.csv";
        public static string LotteryRecordFile { get; set; } = "LotteryRecord.xml"; //@"D:\Development\Lottery\JunkFiles\LotteryRecord.xml";

        private List<Player> mPlayers = SerializationHandler.LoadPlayers(PlayersFile);
        private Records mRecords = SerializationHandler.LoadRecords(LotteryRecordFile);

        private bool mIsRunningEffect = false;
        private Mutex mMutex = new Mutex();

        System.Windows.Media.MediaPlayer mMediaPlayer = new System.Windows.Media.MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();
            mRecords.ValidatePlayers(ref mPlayers);
            FileInfo fInfoAudio = new FileInfo("MarioKartBox.m4a");
            mMediaPlayer.Open(new System.Uri("file:///" + fInfoAudio.FullName));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            displayWinnerName.Foreground = new SolidColorBrush(Colors.Black);
            lock (mMutex)
            {
                if (!mIsRunningEffect)
                {
                    mIsRunningEffect = true;
                    mMediaPlayer.Position = TimeSpan.Zero;
                    mMediaPlayer.Play();
                    //displayWinnerName.Text = "Hello World";
                    Thread t = new Thread(RunEffect);
                    t.Start();

                }
            }
        }

        private void RunEffect()
        {
            //Quick and dirty, a more accurate version can be written
            const int TIME_INCREMENT = 10;
            int index = 0;
            

            for (int time = 0; time < 3500; time += TIME_INCREMENT)
            {
                
                displayWinnerName.Dispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                        if (index >= mPlayers.Count)
                        {
                            index = 0;
                        }
                        displayWinnerName.Text = mPlayers[index].FirstName + " " + mPlayers[index].LastName;
                        index++;
                    })
                );
                Thread.Sleep(TIME_INCREMENT);
                
            }

            Player winner = Lottery.GetWinner(mRecords, mPlayers);
            SerializationHandler.SaveRecords(mRecords, LotteryRecordFile);

            displayWinnerName.Dispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                        displayWinnerName.Foreground = new SolidColorBrush(Colors.Red);
                        displayWinnerName.Text = winner.FirstName + " " + winner.LastName;
                    })
                );

            lock (mMutex)
            {
                mIsRunningEffect = false;
            }
        }
    }
}
