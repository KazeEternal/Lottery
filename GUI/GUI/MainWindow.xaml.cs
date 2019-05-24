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
using System.Linq;
using GUI.States;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string PlayersFile { get; set; } = "Players.csv"; // @"D:\Development\Lottery\JunkFiles\Players.csv";
        public static string LotteryRecordFile { get; set; } = "LotteryRecord.xml"; //@"D:\Development\Lottery\JunkFiles\LotteryRecord.xml";

        private List<Player> mPlayers = null;
        private Records mRecords = SerializationHandler.LoadRecords(LotteryRecordFile);

        private bool mIsRunningEffect = false;
        private Mutex mMutex = new Mutex();

        private RaffleStateInterface mRaffleStateHandler = null;

        public MainWindow()
        {
            InitializeComponent();

            ConfigureRaffleHandler();
            

            #region Configure Question's Answers loader
            int[] indexes = new List<string>(Properties.Settings.Default.AnswersIndex.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).Select(s => int.Parse(s)).ToArray();
            #endregion

            #region configure players and records
            mPlayers = SerializationHandler.LoadPlayers(PlayersFile, indexes);
            mRecords.ValidatePlayers(ref mPlayers);
            #endregion
            
            //MarketImage();
        }

        private void ConfigureRaffleHandler()
        {
            switch (Properties.Settings.Default.Theme)
            {
                case "Default":
                    mRaffleStateHandler = new DefaultRaffleState();
                    break;
                case "StarWars":
                    mRaffleStateHandler = new SortingHatAndAnswerState();        
                    break;
            }

            mRaffleStateHandler.DisplayAreaInitialize(displayArea, displayWinnerName);
            mRaffleStateHandler.StateChangeEvent += MRaffleStateHandler_StateChangeEvent;
        }

        private void MRaffleStateHandler_StateChangeEvent(RaffleStateInterface.States state)
        {
            if(state == RaffleStateInterface.States.FireworksDone)
            {
                Player winner = Lottery.GetWinner(mRecords, mPlayers);
            SerializationHandler.SaveRecords(mRecords, LotteryRecordFile);

            mRaffleStateHandler.DisplayWinner(winner);

            lock (mMutex)
            {
                mIsRunningEffect = false;
            }
            }
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

                //marketImage.Source = src;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool isRunningFireworks = false;

            lock (mMutex)
            {
                isRunningFireworks = !mIsRunningEffect;
            }

            if (isRunningFireworks)
            {
                if (Key.Space == e.Key)
                {
                    displayWinnerName.Foreground = new SolidColorBrush(Colors.Black);
                    mIsRunningEffect = true;

                    //displayWinnerName.Text = "Hello World";
                    Thread t = new Thread(DoRaffleRunningState);
                    t.Start();
                }
                else if (Key.Z == e.Key && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
                {
                    SerializationHandler.RestoreRecords(LotteryRecordFile);
                    mRecords = SerializationHandler.LoadRecords(LotteryRecordFile);
                }
                else if(Key.S == e.Key)
                {
                    mRaffleStateHandler.ShowAnswer();
                }

            }
        }

        private void DoRaffleRunningState()
        {
            //Quick and dirty, a more accurate version can be written
            mRaffleStateHandler.Fireworks(mPlayers);

            
        }
    }
}
