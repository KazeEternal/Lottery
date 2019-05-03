using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Core
{
    public static class Lottery
    {
        public enum LotteryEngine
        {
            Default,
            Causality
        }

        public static LotteryEngine CurrentEngine { get; set; } = LotteryEngine.Default;

        private delegate Player EngineCall(Records records, List<Player> players);

        private static EngineCall[] mLotteryCores = new EngineCall[] 
        {
            DefaultEngine,
            CausalityEngine
        };

        private static Random RandNumGenerator = new Random();

        public static Player GetWinner(Records records, List<Player> players)
        {
            return mLotteryCores[(int)CurrentEngine](records, players);
        }

        private static readonly RNGCryptoServiceProvider mRandNumGenerator = new RNGCryptoServiceProvider();

        public static int GenerateValue(int minValue, int maxValue)
        {
            byte[] randomNumber = new byte[1];

            mRandNumGenerator.GetBytes(randomNumber);

            double randCharValue = Convert.ToDouble(randomNumber[0]);
            double multiplier = Math.Max(0d, (randCharValue / 255d) - 0.00000000001d);
            
            int range = maxValue - minValue + 1;
            double deltaFromMinValue = Math.Floor(multiplier * range);

            return (int)(minValue + deltaFromMinValue);
        }

        private static Player DefaultEngine(Records records, List<Player> players)
        {
            Player retVal = null;

            //int value = RandNumGenerator.Next() % players.Count;
            int value = GenerateValue(0, players.Count - 1);
            retVal = players[value];
            records.ApplyWinner(retVal);
            players.RemoveAt(value);

            return retVal;
        }

        private static Player CausalityEngine(Records records, List<Player> players)
        {
            throw new NotImplementedException("Causality Lottery Engine is currently not supported");
        }
    }
}
