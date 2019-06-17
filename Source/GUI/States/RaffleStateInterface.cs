using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GUI.States
{
    public abstract class RaffleStateInterface
    {
        public enum States
        {
            Running,
            FireworksDone,
            DisplayingWinner
        }

        public delegate void RaffleStateChange(States state);

        public abstract void DisplayAreaInitialize(StackPanel displayArea, TextBlock nameDisplay);
        public abstract void Fireworks(List<Player> mPlayers);
        public abstract void DisplayWinner(Player winner);

        protected virtual void OnShapeChanged(States state)
        {
            
            if (StateChangeEvent != null)
            {
                StateChangeEvent(state);
            }
        }

        public event RaffleStateChange StateChangeEvent;

        public virtual void ShowAnswer() { }
    }
}
