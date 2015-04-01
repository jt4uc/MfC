#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace MowingforCookies
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // for the background
        Texture2D background;
        int screenWidth;
        int screenHeight;

        Controls controls;
        List<Spot> patches2;
        Spot[,] patches;
        Mower mower;
        Enemy gnome1;



        Texture2D patch;

        public MainGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this); /// default is 800x600
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            Window.Title = "TEST";

            patches2 = new List<Spot>();
            patches = new Spot[8, 8];
            for (int row = 0; row < patches.GetLength(0); row++)
            {
                for (int col = 0; col < patches.GetLength(1); col++)
                {
                    int rows = row * 50;
                    int cols = col * 50;
                    Spot testT = new Spot(rows, cols, false, 3, 3,row,col);
                    patches[row, col] = testT;

                }
            }

            //for (int row = 100; row < 670; row += 55)
            //{
            //    for (int col = 50; col < 300; col += 55)
            //    {
            //        Spot t = new Spot(row, col, false, 3, 3);
            //        patches2.Add(t);

            //    }
            //}
            mower = new Mower(patches[0,1], 0);

            //Obstacle test = new Obstacle("tree");
            //patches2[2].setObstacle(test);
            //test.setSpot(patches2[2]);
            //Obstacle test2 = new Obstacle("bush");
            //patches2[3].setObstacle(test2);
            //test2.setSpot(patches2[3]);

            //Cookie c1 = new Cookie();
            //patches2[30].setCookie(c1);
            //c1.setSpot(patches2[30]);

            //gnome1 = new Enemy(patches2[15], 3);
            //patches2[15].setEnemy(gnome1);


            base.Initialize();
            controls = new Controls();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("white.png");
            patch = Content.Load<Texture2D>("Patch.png");
            foreach (Spot s in patches)
            {
                s.LoadContent(this.Content);
                if (s.ob != null)
                {
                    s.ob.LoadContent(this.Content);
                }
                if (s.c != null)
                {
                    s.c.LoadContent(this.Content);
                }
            }

            mower.LoadContent(this.Content);
            //gnome1.LoadContent(this.Content);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            controls.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mower.Update(controls, patches, gameTime);
            //gnome1.Update(mower, controls, patches2, gameTime);
            base.Update(gameTime);
            //if (mower.alize == false)
            //{
            //    Exit();
            //}

            foreach (Spot s in patches)
            {
                s.Update(Content);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            GraphicsDevice.Clear(Color.LimeGreen);
            spriteBatch.Begin();
            DrawBackground();
            foreach (Spot s in patches)
            {
                s.Draw(spriteBatch);
                if (s.ob != null)
                {
                    s.ob.Draw(spriteBatch);
                }
                if (s.c != null)
                {
                    s.c.Draw(spriteBatch);
                }
            }
            mower.Draw(spriteBatch);
            //gnome1.Draw(spriteBatch);




            spriteBatch.End();



            base.Draw(gameTime);
        }

        private void DrawBackground()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, 800, 800);
            spriteBatch.Draw(background, screenRectangle, Color.White);

        }
    }
}
