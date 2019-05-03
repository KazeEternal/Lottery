using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GUI.States
{
    public interface RaffleStateInterface
    {
        void DisplayAreaInitialize(StackPanel displayArea, TextBlock nameDisplay);
        void Fireworks(List<Player> mPlayers);
        void DisplayWinner(Player winner);
    }
}
