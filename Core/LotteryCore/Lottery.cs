using System;
using System.Collections.Generic;

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

        private static Player DefaultEngine(Records records, List<Player> players)
        {
            Player retVal = null;

            int value = RandNumGenerator.Next() % players.Count;
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
