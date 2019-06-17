using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame
{
    public class LotteryEngine : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager LotteryGraphicsDevice { get; private set; } = null;
        public SpriteBatch Sprites { get; set; } = null;
        private static LotteryEngine mInstance = null;
        public static LotteryEngine Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new LotteryEngine();

                return mInstance;
            }
        }

        public LotteryEngine()
        {
            
            LotteryGraphicsDevice = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            Sprites = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
