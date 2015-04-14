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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
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

        public MainGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this); /// default is 800x600
            Content.RootDirectory = "Content";

            //map = new TmxMap("./Content/gravel_in_corner_test.tmx");
            map = new TmxMap("./Content/beta.tmx");
            SCREENWIDTH = map.Width * 50;
            SCREENHEIGHT = map.Height * 60;

            win_Num = 50;
            youWinYet = false;

            graphics.PreferredBackBufferWidth = SCREENWIDTH;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = SCREENHEIGHT;   // set this value to the desired height of your window
            graphics.ApplyChanges();
            ticks = 0;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            Window.Title = "MOWING FOR COOKIES";

            // map.ObjectGroups[0].Objects.RemoveAt(0); // for when objects are removed

            patches = new Spot[map.Height, map.Width];
            //System.Diagnostics.Debug.WriteLine("mapHeightWidth: " + map.Height + ", " + map.Width);
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
            // hard coding Mowers, Obstacles, and Enemies
            mower = new Mower(patches[0,1], 150); // current location represented by spot
            //grandma = new Grandma(patches[6,6], 6, 6);
            enemies = new List<Enemy>();
            cookies = new List<Cookie>();
            obstacles = new List<Obstacle>();

            // go through each object layer
            for (int i = 0; i < map.ObjectGroups.Count; i++)
            {
                String name = map.ObjectGroups[i].Name; // object layer names are labeled <png filename>
                int numObjects = map.ObjectGroups[i].Objects.Count;
                System.Diagnostics.Debug.WriteLine("obj layer name: " + name);
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

            //Cookie c2 = new Cookie(patches[5, 0], 5, 0);
            //cookies.Add(c2);

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
 
            menu = Content.Load<Texture2D>("menu.png");
            foreach (Spot s in patches)
            {
                s.LoadContent(this.Content);
            }

            mower.LoadContent(this.Content);
            grandma.LoadContent(this.Content);
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
            font = Content.Load<SpriteFont>("spriteFont");
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
            if (!youWinYet)
            {
                ticks++;
                controls.Update();
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    Initialize();
                }

                mower.Update(controls, patches, gameTime);
                grandma.Update(mower, controls, patches, gameTime);

                if (grandma.cbox.Intersects(mower.collisionBox))
                {
                    grandma.alive = false;
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
                        if (e.cbox.Intersects(grandma.cbox))
                        {
                            e.visible = false;
                        }
                    }
                }

                base.Update(gameTime);

                foreach (Spot s in patches)
                {
                    s.Update(Content, patches, mower, enemies, ticks);
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            GraphicsDevice.Clear(Color.LimeGreen);
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
            grandma.Draw(spriteBatch);
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
            base.Draw(gameTime);
        }

        private void DrawStatusBar()
        {
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


    }
}
