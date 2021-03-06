﻿#region Using Statements
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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // for the background
        Texture2D background;
        int SCREENWIDTH; 
        int SCREENHEIGHT;

        Controls controls;
        Spot[,] patches;
        Mower mower;

        List<Enemy> enemies;
        List<Cookie> cookies;
        List<Obstacle> obstacles;//?


        Texture2D patch;

        // for Tiled
        TmxMap map;
        Texture2D[] tiles;


        

        public MainGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this); /// default is 800x600
            Content.RootDirectory = "Content";

            map = new TmxMap("./Content/10x10checkpoint_map.tmx");
            SCREENWIDTH = map.Width * 50;
            SCREENHEIGHT = map.Height * 50;

            graphics.PreferredBackBufferWidth = SCREENWIDTH;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = SCREENHEIGHT;   // set this value to the desired height of your window
            graphics.ApplyChanges();
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
           // patches = new Spot[24, 18];

            // map.ObjectGroups[0].Objects.RemoveAt(0); // for when objects are removed

            patches = new Spot[map.Height, map.Width];
            System.Diagnostics.Debug.WriteLine("mapHeightWidth: " + map.Height + ", " + map.Width);
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

            //patches = new Spot[8, 8];
            //for (int row = 0; row < patches.GetLength(0); row++)
            //{
            //    for (int col = 0; col < patches.GetLength(1); col++)
            //    {
            //        int rows = row * 50;
            //        int cols = col * 50;
            //        Spot testT = new Spot(rows, cols, false, 3, 3, row, col);
            //        patches[row, col] = testT;

            //    }
            //}


            // hard coding Mowers, Obstacles, and Enemies
            mower = new Mower(patches[0,1], 0); // current location represented by spot
            enemies = new List<Enemy>();
            cookies = new List<Cookie>();
            obstacles = new List<Obstacle>();

            //Obstacle test = new Obstacle("tree");
            //patches2[2].setObstacle(test);
            //test.setSpot(patches2[2]);
            //Obstacle test2 = new Obstacle("bush");
            //patches2[3].setObstacle(test2);
            //test2.setSpot(patches2[3]);
            

            //Obstacle o1 = new Obstacle(patches[0,0],"tree",0,0);
            //Obstacle o2 = new Obstacle(patches[3,3],"gravel", 3, 3);
            //Obstacle o3 = new Obstacle(patches[7,7],"grandma", 7, 7);
            //obstacles.Add(o1);
            //obstacles.Add(o2);
            //obstacles.Add(o3);
            //foreach (Obstacle o in obstacles)

            int numTrees = map.ObjectGroups[0].Objects.Count; // assuming map.ObjectGroups[0] is the layer corresponding to trees
            // can change to ObjectLayer Type later
            for (int i = 0; i < numTrees; i++)
            {
                int x = (int)map.ObjectGroups[0].Objects[i].X/50; // divide by 50 because that's the size of the tile
                int y = ((int)map.ObjectGroups[0].Objects[i].Y - 50) / 50; // -50, because apparently tiled goes by bottom left corner
                System.Diagnostics.Debug.WriteLine("x, y: " + x + ", " + y);
                Obstacle o = new Obstacle(patches[x, y], "tree", x, y);
                obstacles.Add(o);
                patches[x, y].setObstacle(o);
            }

            int numGravel = map.ObjectGroups[1].Objects.Count; // assuming map.ObjectGroups[1] is the layer corresponding to gravel
            // can change to ObjectLayer Type later
            for (int i = 0; i < numGravel; i++)
            {
                int x = (int)map.ObjectGroups[1].Objects[i].X/50;
                int y = ((int)map.ObjectGroups[1].Objects[i].Y - 50) / 50;
                Obstacle o = new Obstacle(patches[x, y], "gravel", x, y);
                obstacles.Add(o);
                patches[x, y].setObstacle(o);
            }

            
            //Obstacle o1 = new Obstacle(patches[0,0],"tree",0,0);
            //Obstacle o2 = new Obstacle(patches[3,3],"gravel", 3, 3);
            //Obstacle o3 = new Obstacle(patches[7,7],"grandma", 7, 7); // will be part of enemy class
            //obstacles.Add(o1);
            //obstacles.Add(o2);
            //obstacles.Add(o3);
            //foreach (Obstacle o in obstacles)
            //{
            //    patches[o.arrayRowX, o.arrayColY].setObstacle(o);
            //}

            // can convert this into Tiled stuff later
            Cookie c1 = new Cookie(patches[4,0],4, 4);
            cookies.Add(c1);
            foreach (Cookie c in cookies)
            {
                patches[c.arrayRowX, c.arrayColY].setCookie(c);
            }

            // will have to think of a way to import enemy information
            int [] kiddingMe = new int[] {1, 1, 1, 2, 2, 2}; // gives enemies path to patrol
            int [] notCoolBro = new int[] {};

            Enemy gnome1 = new Enemy(patches[4, 5], 3, 4,5, kiddingMe);
            Enemy gnome2 = new Enemy(patches[7, 6], 3, 7,6, notCoolBro);
            Enemy gnome3 = new Enemy(patches[2, 1], 3, 2,1, notCoolBro);
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
            foreach (Cookie c in cookies)
            {
                c.LoadContent(this.Content);
            }
            foreach (Obstacle o in obstacles)
            {
                o.LoadContent(this.Content);
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
            if (mower.alive == false)
            {
                //Exit();
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
                if (e.visible == true)
                {
                    e.Draw(spriteBatch);
                }
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
