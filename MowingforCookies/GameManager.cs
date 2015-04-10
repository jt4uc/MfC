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
    enum Screen
    {
        StartScreen,
        MainGame,
        GameOverScreen
    }
    public class GameManager : MainGame
    {
        SpriteBatch spriteBatch;
        StartScreen startScreen;
        MainGame gamePlayScreen;
        Screen currentScreen;

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            startScreen = new StartScreen(this);
            currentScreen = Screen.StartScreen;

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            switch (currentScreen)
            {
                case Screen.StartScreen:
                    if (startScreen != null)
                        startScreen.Update();
                    break;
                case Screen.MainGame:
                    if (gamePlayScreen != null)
                        gamePlayScreen.Update2(gameTime);
                    break;
                case Screen.GameOverScreen:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch (currentScreen)
            {
                case Screen.StartScreen:
                    if (startScreen != null)
                        startScreen.Draw(spriteBatch);
                    break;
                case Screen.MainGame:
                    if (gamePlayScreen != null)
                        gamePlayScreen.Draw(gameTime);
                    break;
                case Screen.GameOverScreen:
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void StartGame()
        {
            gamePlayScreen = new MainGame();
            currentScreen = Screen.MainGame;

            startScreen = null;
        }

    }
}
