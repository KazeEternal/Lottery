using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame
{
    public static class LotteryGL
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using (LotteryEngine lotteryEngine = LotteryEngine.Instance)
            {
                lotteryEngine.Run();
            }
        }
    }
}
