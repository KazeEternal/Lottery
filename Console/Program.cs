using Console.Settings;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
    public class Program
    {
        [CommandLineAttribute("playerFile", "The Location of the CSV file from containing the players names. Uses the name column.")]
        public static string PlayersFile { get; set; } = @"D:\Development\Lottery\JunkFiles\Players.csv";
        [CommandLineAttribute("recordFile", "The Location of the CSV file from containing the players names. Uses the name column.")]
        public static string LotteryRecordFile { get; set; } = @"D:\Development\Lottery\JunkFiles\LotteryRecord.xml";

        public static bool DisplayHelp { get; set; } = false;

        public static void Main(string[] args)
        {
            if(CmdLineParsing(args))
            {
                if(DisplayHelp)
                {
                    DisplayHelpInfo();
                }
                else
                {
                    DoLottery();
                }
            }
            else
            {
                System.Console.WriteLine("Invalid Arguements Found");
                DisplayHelpInfo();
            }
        }

        private static void DoLottery()
        {
            List<Player> players = SerializationHandler.LoadPlayers(PlayersFile);
            Records records = SerializationHandler.LoadRecords(LotteryRecordFile);

            records.ValidatePlayers(ref players);
            while (players.Count > 0)
            {
                Player winner = Lottery.GetWinner(records, players);
                SerializationHandler.SaveRecords(records, LotteryRecordFile);
            }
            
        }

        private static bool CmdLineParsing(string[] args)
        {
            bool isSuccessRetVal = true;
            Type type = typeof(Program);
            PropertyInfo[] programProperties = type.GetProperties();
            
            for(int i = 0; i < args.Length; ++i)
            {
                if(args[i].StartsWith("-"))
                {
                    bool isFound = false;
                    foreach (PropertyInfo pInfo in programProperties)
                    {
                        CommandLineAttribute cla = (CommandLineAttribute)pInfo.GetCustomAttribute(typeof(CommandLineAttribute));
                        if(cla != null && cla.Flag.ToLower().Equals(args[i].Substring(1)) )
                        {
                            pInfo.SetValue(null, args[i + 1]);
                            i++;
                            isFound = true;
                        }
                    }

                    if(!isFound)
                    {
                        isSuccessRetVal = false;
                        System.Console.WriteLine("Arg not Found! " + args[i]);
                        break;
                    }
                }
                else if(args[i].StartsWith("-help") || args[i].StartsWith("-h") || args[i].StartsWith("-?"))
                {
                    DisplayHelp = true;
                    break;
                }
                else
                {
                    return false;
                }
            }
            return isSuccessRetVal;
        }

        public static void DisplayHelpInfo()
        {

            Type type = typeof(Program);
            PropertyInfo[] programProperties = type.GetProperties();

            System.Console.WriteLine("Help for Lottery CommandLine Tool:\n");

            foreach (PropertyInfo pInfo in programProperties)
            {
                CommandLineAttribute cla = (CommandLineAttribute)pInfo.GetCustomAttribute(typeof(CommandLineAttribute));
                if (cla != null)
                {
                    System.Console.WriteLine("-{0}\t\t{1}", cla.Flag, cla.Help);

                }
            }
        }
    }
}
