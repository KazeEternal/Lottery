using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLVersionGUI.States
{
    interface StateInterface
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
