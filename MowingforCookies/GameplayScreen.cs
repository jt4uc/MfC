#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
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
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int SCREENWIDTH;
        int SCREENHEIGHT;
        int ticks;

        Controls controls;
        Spot[,] patches;
        Mower mower;
        Grandma grandma;
        List<Enemy> enemies;
        List<Cookie> cookies;
        List<Obstacle> obstacles;
        private SpriteFont font;
        private Texture2D menu;
        public int win_Num;
        public bool youWinYet;
        // for Tiled
        TmxMap map;
        //Texture2D[] tiles;




        ContentManager content;
        SpriteFont gameFont;

        //Vector2 playerPosition = new Vector2(100, 100);
        //Vector2 enemyPosition = new Vector2(100, 100);

        //Random random = new Random();

        #endregion

        #region Initialization

       


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);




            //graphics = new GraphicsDeviceManager(this); /// default is 800x600
            //Content.RootDirectory = "Content";

            //map = new TmxMap("./Content/gravel_in_corner_test.tmx");
            map = new TmxMap("./Content/beta.tmx");
            SCREENWIDTH = map.Width * 50;
            SCREENHEIGHT = map.Height * 60;

            win_Num = 50;
            youWinYet = false;

            //graphics.PreferredBackBufferWidth = SCREENWIDTH;  // set this value to the desired width of your window
            //graphics.PreferredBackBufferHeight = SCREENHEIGHT;   // set this value to the desired height of your window
            //graphics.ApplyChanges();
            ticks = 0;


        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize() // protected override
        {

            //Window.Title = "MOWING FOR COOKIES";

            

            // map.ObjectGroups[0].Objects.RemoveAt(0); // for when objects are removed

            patches = new Spot[map.Height, map.Width];
            //System.Diagnostics.Debug.WriteLine("mapHeightWidth: " + map.Height + ", " + map.Width);
            for (int row = 0; row < patches.GetLength(0); row++)
            {
                for (int col = 0; col < patches.GetLength(1); col++)
                {
                    int rows = row * 50;
                    int cols = col * 50;
                    Spot testT = new Spot(rows, cols, false, 3, 3, row, col);
                    patches[row, col] = testT;

                }
            }
            // hard coding Mower
            mower = new Mower(patches[0, 1], 150); // current location represented by spot
            enemies = new List<Enemy>();
            cookies = new List<Cookie>();
            obstacles = new List<Obstacle>();

            // go through each object layer
            for (int i = 0; i < map.ObjectGroups.Count; i++)
            {
                String name = map.ObjectGroups[i].Name; // object layer names are labeled <png filename>
                int numObjects = map.ObjectGroups[i].Objects.Count;
                // go through each object of the object layer
                for (int j = 0; j < numObjects; j++)
                {
                    int x = (int)map.ObjectGroups[i].Objects[j].X / 50; // divide by 50 because that's the size of the tile
                    int y = ((int)map.ObjectGroups[i].Objects[j].Y - 50) / 50; // -50, because apparently tiled goes by bottom left corner
                    //System.Diagnostics.Debug.WriteLine("x, y: " + x + ", " + y);

                    if (name.Equals("gnome"))
                    {

                        String path = map.ObjectGroups[i].Objects[j].Name;
                        int[] pathArray = new int[] { };
                        if (!path.Equals(""))
                        {
                            pathArray = Array.ConvertAll(path.Split(','), int.Parse); // splits the path specified in Tiled and converts into int array

                        }

                        Enemy gnome = new Enemy(patches[x, y], x, y, pathArray);
                        enemies.Add(gnome);
                    }
                    else if (name.Equals("grandma"))
                    {
                        grandma = new Grandma(patches[x, y], x, y);
                    }
                    else if (name.Equals("cookies"))
                    {
                        Cookie c = new Cookie(patches[x, y], x, y);
                        cookies.Add(c);
                    }
                    else if (!name.Equals("grass"))
                    {
                        Obstacle o = new Obstacle(patches[x, y], name, x, y);
                        obstacles.Add(o);
                        patches[x, y].setObstacle(o);
                    }


                }

            }


            //base.Initialize();
            controls = new Controls();
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            Initialize();

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("spriteFont");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();


            menu = content.Load<Texture2D>("menu.png");
            foreach (Spot s in patches)
            {
                s.LoadContent(content);
            }

            mower.LoadContent(content);
            if (grandma != null)
            {
                grandma.LoadContent(content);
            }
            foreach (Enemy e in enemies)
            {
                e.LoadContent(content);
            }
            foreach (Cookie c in cookies)
            {
                c.LoadContent(content);
            }
            foreach (Obstacle o in obstacles)
            {
                o.LoadContent(content);
            }
            font = content.Load<SpriteFont>("spriteFont");


        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive)
            {
                if (!youWinYet)
                {
                    ticks++;
                    controls.Update();
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        //Exit();

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        Initialize();
                    }

                    mower.Update(controls, patches, gameTime);
                    if (grandma != null)
                    {
                        grandma.Update(mower, controls, patches, gameTime);
                        if (grandma.cbox.Intersects(mower.collisionBox))
                        {
                            grandma.alive = false;
                        }
                    }


                    foreach (Enemy e in enemies)
                    {
                        if (e.alive)
                        {
                            e.Update(mower, controls, patches, gameTime);
                            if (e.cbox.Intersects(mower.collisionBox))
                            {
                                mower.alive = false;
                            }
                            if (grandma != null && e.cbox.Intersects(grandma.cbox))
                            {
                                e.visible = false;
                            }
                        }
                    }

                    //base.Update(gameTime);
                    if (mower.alive == false)
                    {

                    }

                    foreach (Spot s in patches)
                    {
                        s.Update(content, patches, mower, enemies, ticks);
                    }
                    foreach (Obstacle o in obstacles)
                    {

                        o.Update(patches, mower, enemies, ticks);
                    }
                    foreach (Cookie c in cookies)
                    {
                        c.Update(mower, grandma, ticks);
                    }

                    if (mower.totalMowed >= win_Num)
                    {
                        youWinYet = true;
                        win_Num = 100;

                    }
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        youWinYet = false;
                    }
                }
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(Controls control)
        {
            //if (input == null)
            //    throw new ArgumentNullException("input");

            //// Look up inputs for the active player profile.
            //int playerIndex = (int)ControllingPlayer.Value;

            //KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            //GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            //// The game pauses either if the user presses the pause button, or if
            //// they unplug the active gamepad. This requires us to keep track of
            //// whether a gamepad was ever plugged in, because we don't want to pause
            //// on PC if they are playing with a keyboard and have no gamepad at all!
            //bool gamePadDisconnected = !gamePadState.IsConnected &&
            //                           input.GamePadWasConnected[playerIndex];

            //if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            //{
            //    //ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            //}
            //else
            //{
            //    // Otherwise move the player position.
            //    Vector2 movement = Vector2.Zero;

            //    if (keyboardState.IsKeyDown(Keys.Left))
            //        movement.X--;

            //    if (keyboardState.IsKeyDown(Keys.Right))
            //        movement.X++;

            //    if (keyboardState.IsKeyDown(Keys.Up))
            //        movement.Y--;

            //    if (keyboardState.IsKeyDown(Keys.Down))
            //        movement.Y++;

            //    Vector2 thumbstick = gamePadState.ThumbSticks.Left;

            //    movement.X += thumbstick.X;
            //    movement.Y -= thumbstick.Y;

            //    if (movement.Length() > 1)
            //        movement.Normalize();

            //    playerPosition += movement * 2;
            //}
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            foreach (Spot s in patches)
            {
                s.Draw(spriteBatch);
            }
            foreach (Obstacle o in obstacles)
            {
                o.Draw(spriteBatch);
            }
            foreach (Cookie c in cookies)
            {
                if (c.alive)
                {
                    c.Draw(spriteBatch);
                }
            }
            mower.Draw(spriteBatch);
            if (grandma != null)
            {
                grandma.Draw(spriteBatch);
            }
            foreach (Enemy e in enemies)
            {
                if (e.visible)
                {
                    if (e.alive)
                    {
                        e.Draw(spriteBatch);
                    }
                }
            }
            DrawStatusBar();

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }

        private void DrawStatusBar()
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Draw(menu, new Rectangle(0, 500, 500, 100), Color.White);
            if (!youWinYet)
            {
                spriteBatch.DrawString(font, "Grass mowed: " + mower.totalMowed + "/" + win_Num, new Vector2(25, 520), Color.Black);
                spriteBatch.DrawString(font, "Fuel: " + mower.cookies, new Vector2(25, 550), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(font, "YOU GET COOKIES!!!", new Vector2(25, 520), Color.Black);
            }

        }

        #endregion
    }
}
