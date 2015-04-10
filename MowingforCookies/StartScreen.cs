#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using TiledSharp;
#endregion

namespace MowingforCookies
{
    public class StartScreen
    {
        private Texture2D texture;
        private GameManager game;
        private KeyboardState lastState;

        public StartScreen(GameManager game1)
        {
            this.game = game1;
            texture = game1.Content.Load<Texture2D>("shitty_menu");
            lastState = Keyboard.GetState();
        }

        public void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter) && lastState.IsKeyUp(Keys.Enter))
            {
                game.StartGame();
            }

            lastState = keyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(texture, new Vector2(0f, 0f), Color.White);
        }
    }
}
