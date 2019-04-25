using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

            MarketImage();
        }

        private void MarketImage()
        {
            FileInfo fInfo = new FileInfo(Properties.Settings.Default.DisplayImage);
            if (fInfo.Exists)
            {
                
                Image i = new Image();
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri("file:///" + fInfo.FullName);
                src.EndInit();
                i.Source = src;
                i.Stretch = Stretch.Uniform;

                marketImage.Source = src;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
           
            
            lock (mMutex)
            {
                if (!mIsRunningEffect)
                {
                    if (Key.Space == e.Key)
                    {
                        displayWinnerName.Foreground = new SolidColorBrush(Colors.Black);
                        mIsRunningEffect = true;

                        //displayWinnerName.Text = "Hello World";
                        Thread t = new Thread(RunEffect);
                        t.Start();

                        mMediaPlayer.Position = TimeSpan.Zero;
                        mMediaPlayer.Play();
                    }
                    else if (Key.Z == e.Key && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
                    {
                        SerializationHandler.RestoreRecords(LotteryRecordFile);
                        mRecords = SerializationHandler.LoadRecords(LotteryRecordFile);
                    }

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
