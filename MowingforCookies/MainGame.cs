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
        Spot[,] patches;
        Mower mower;

        List<Enemy> enemies;



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
            mower = new Mower(patches[0,1], 0);
            enemies = new List<Enemy>();

            //Obstacle test = new Obstacle("tree");
            //patches2[2].setObstacle(test);
            //test.setSpot(patches2[2]);
            //Obstacle test2 = new Obstacle("bush");
            //patches2[3].setObstacle(test2);
            //test2.setSpot(patches2[3]);

            //Cookie c1 = new Cookie();
            //patches2[30].setCookie(c1);
            //c1.setSpot(patches2[30]);
            

            Enemy gnome1 = new Enemy(patches[4, 5], 3, 4,5);
            Enemy gnome2 = new Enemy(patches[7, 6], 3, 7,6);
            Enemy gnome3 = new Enemy(patches[2, 1], 3, 2,1);
            enemies.Add(gnome1);
            enemies.Add(gnome2);
            enemies.Add(gnome3);
            foreach ( Enemy e in enemies){
                patches[e.arrayRowX, e.arrayColY].setEnemy(e);
            }
            


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
            foreach (Enemy e in enemies)
            {
                e.LoadContent(this.Content);
            }

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
            foreach (Enemy e in enemies)
            {
                e.Update(mower,controls,patches,gameTime);
            }
            
            base.Update(gameTime);
            if (mower.alize == false)
            {
                Exit();
            }

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
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }




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
