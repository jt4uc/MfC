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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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
        

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int ticks;
        int tickCount;
        int waterTickCount;

        Controls controls;
        Spot[,] patches;
        Mower mower;
        Grandma grandma;
        List<Enemy> enemies;
        List<Cookie> cookies;
        List<Obstacle> obstacles;
        private SpriteFont font;
        private Texture2D menu;
        private Texture2D levelRect;
        public int win_Num;
        public int startingFuel;
        public int optimalFuel;
        public bool youWinYet;
        public bool youDoneYet;
        public int mowablePatches;
        public bool narrativeGiven;
        public bool musicToggle = true;
        TmxMap map;

        String[] levels;
        String level;
        String[][] narrative;
        int line;

        public SoundEffect backgroundMusic;
        public SoundEffectInstance myBgMusic;

        ContentManager content;
        SpriteFont gameFont;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(GraphicsDeviceManager graphics, String level)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);



            this.graphics = graphics;
            this.level = level;
            this.narrativeGiven = false;
            line = 0;
            tickCount = 0;

            map = new TmxMap("./Content/" + level + ".tmx");
            graphics.PreferredBackBufferWidth = map.Width * 50; // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = map.Height * 50+100; // set this value to the desired height of your window
            graphics.ApplyChanges();

            levels = new String[] { 
                "The Owner's Quick Guide to a Cookie-Powered Mower",
                "Oh Gnome! America's Lastest Pestilence",
                "Once You Go Gnome...",
                "...You Can't Go Home",
                "This is not Mowing for Inheritance",
                "The Dreaded 41",
                
                "water test",
                "Maybe Grandma Should Go Apartment Hunting",
                "Perfection is the Enemy of Good Enough"
            };
            narrative = new String[][] {
                new String[] {
                    "(On the phone) Grandma: Hello, hello, can you hear me?",
                    "You: Yes, Grandma.",
                    "Grandma: Sweetie, could you come over and mow the lawn for me?",
                    "You: Ok...",
                    "Grandma: Eh? Could you say that again?...", 
                    "Grandma: I'll bake some cookies for you!",
                    "You: YES!",
                    "Grandma: No need to yell! I can hear just fine.",
                    "You: ...",
                    "Grandma: Just so you know, I've bought a cookie-powered mower...",
                    "Grandma: It's a little hard to control...",
                    "Grandma: You can only change directions once you've hit something.",
                    "You: ...?"
                },
                new String[] {
                    "Grandma: Always be prepared, my dear.",
                    "You: Prepared for...?",
                    "Grandma: Anything. That's why I planted land mines in the yard.",
                    "You: ...WHAT?",
                    "Grandma: I had a lot leftover from 'Nam.",
                    "You: ...But I'm here to mow the yard...",
                    "Grandma: Don't worry. The cookies I made you and the mower...",
                    "Grandma: will protect you.",
                    "You: ...It's not like I had my whole life ahead of me or anything..."
                },
                new String[] {
                    "You: Grandma, where are the cookies you promised?",
                    "Grandma: Don't you see them?",
                    "You: ...The thing that looks like a mushroom that's bigger than the tree?",
                    "Grandma: Yes. It's strange they've never won at the state fair..."
                    
                },
                new String[] {
                    
                },
                new String[] {                   
                    "Grandma: It's so nice to see my landscaping again.",
                    "Grandma: Oh, but be careful of those bushes, dearie.",
                    "Grandma: They're also from 'Nam.",
                    "You: Grandma. Why.",
                    "Grandma: Also, I'm going to take a walk outside. Be sure not to hit me!",
                    "You: !!!"
                    },
                new String[] {
                    "Grandma: *cackles*",
                    "You: ...!?",
                },
                new String[] {},
                new String[] {},
                new String[]{}
            };
            
            ticks = 0;
            mowablePatches = 0;
            levelRect = new Texture2D(graphics.GraphicsDevice, 1, 1);
            levelRect.SetData(new[] { Color.ForestGreen });


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

            youWinYet = false;
            youDoneYet = false;

            // map.ObjectGroups[0].Objects.RemoveAt(0); // for when objects are removed

            patches = new Spot[map.Width, map.Height];
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
                    if (name.Equals("mower"))
                    {   
                        String values = map.ObjectGroups[i].Objects[j].Name;
                        int[] valuesArray = new int[] { };
                        valuesArray = Array.ConvertAll(values.Split(','), int.Parse);
                        mower = new Mower(patches[x, y], valuesArray[0]);
                        win_Num = valuesArray[1];
                        optimalFuel = valuesArray[2];
                    }
                    else if (name.Equals("gnome"))
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
                        String type = map.ObjectGroups[i].Objects[j].Name;
                        Cookie c = new Cookie(patches[x, y], type, x, y);
                        cookies.Add(c);
                    }
                    else if (name.Equals("water"))
                    {
                        String path = map.ObjectGroups[i].Objects[j].Name;
                        int[] targetCoords = new int[] { };

                        if (!path.Equals(""))
                        {
                            targetCoords = Array.ConvertAll(path.Split(','), int.Parse);
                        }

                        Obstacle water = new Obstacle(patches[x, y], name, x, y, targetCoords[0], targetCoords[1]);
                        System.Diagnostics.Debug.WriteLine("water: " + targetCoords[0] + ", " + targetCoords[1]);
                        obstacles.Add(water);
                        patches[x, y].setObstacle(water);
                    }
                    else if (!name.Equals("grass"))
                    {
                        Obstacle o = new Obstacle(patches[x, y], name, x, y);
                        obstacles.Add(o);
                        patches[x, y].setObstacle(o);
                    }
                    


                }

            }
            if (mower == null)
                mower = new Mower(patches[0, 1], 150);
            mowablePatches = map.Width * map.Height - obstacles.Count;
            startingFuel = mower.cookies;

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

            gameFont = content.Load<SpriteFont>("spriteFont2");

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

            if (musicToggle == true)
            {
                backgroundMusic = content.Load<SoundEffect>("tiny_forest");
                myBgMusic = backgroundMusic.CreateInstance();
                //backgroundMusic.Play();
               myBgMusic.IsLooped = true;
               myBgMusic.Play();
                musicToggle = false;
            }
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false); 

            if (IsActive)
            {
                
                ticks++;
                controls.Update();
                if (narrativeGiven && !youDoneYet)
                {
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Environment.Exit(0); 

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        LoadContent();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.P))
                    {
                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new PauseScreen(graphics), null);
                    }

                        mower.Update(controls, patches, gameTime);
                    if (grandma != null)
                    {
                        grandma.Update(mower, controls, patches, gameTime);
                        if (grandma.cbox.Intersects(mower.collisionBox))
                        {
                            grandma.alive = false;
                            mower.alive = false;
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
                    foreach (Spot s in patches)
                    {
                        s.Update(content, patches, mower, enemies, ticks);
                    }
                    foreach (Obstacle o in obstacles)
                    {
                        if (!o.obstacleType.Equals("water"))
                        {

                        }
                        else
                        {
                            //if (o.obstacleType.Equals("water"))
                            //{
                            //System.Diagnostics.Debug.WriteLine("-->" + o.getArrayRowX() + ", " + o.getArrayColY()); //o.isWaterTraversed()
                            o.Update(patches, mower, enemies, ticks);
                            if (patches[mower.arrayRowX, mower.arrayColY].ob == null || !patches[mower.arrayRowX, mower.arrayColY].ob.obstacleType.Equals("water"))
                            {
                                o.setWaterTraversedFalse();
                            }
                            if (o.isWaterTraversed() == true)
                            {
                                waterTickCount = ticks;
                                break;
                            }

                            //                            }
                        }

                    }
                    foreach (Obstacle o in obstacles)
                    {
                        if (!o.obstacleType.Equals("water"))
                        {
                            o.Update(patches, mower, enemies, ticks);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine(o.getArrayRowX() + ", " + o.getArrayColY() + ", " + o.obstacleType);
                        }
                        
                    }
                    
                    //System.Diagnostics.Debug.WriteLine("-----------------------------------");
      
                    foreach (Cookie c in cookies)
                    {
                        c.Update(mower, grandma, ticks, patches);
                    }

                    if (mower.totalMowed >= win_Num)
                    {
                        //mower.Update(controls, patches, gameTime);//no good here
                        youWinYet = true;
                        youDoneYet = true;
                        win_Num = mowablePatches; // mowable patches isn't even close to being accurate... can't calculate it - it will have to be included in the maps
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && youWinYet)
                    {
                        if (Array.IndexOf(levels, level) == levels.Length - 1)
                        {
                            ScreenManager.AddScreen(new BackgroundScreen(), null);
                            ScreenManager.AddScreen(new LevelSelectionMenuScreen(graphics), 0);
                            
                        } 
                        else
                            LoadingScreen.Load(ScreenManager, true, 0,
                              new GameplayScreen(graphics, levels[Array.IndexOf(levels, level) + 1]));
                    }
                }
                else
                {
                    
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        youDoneYet = false;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        LoadContent();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && youWinYet)
                    {
                        if (Array.IndexOf(levels, level) == levels.Length - 1)
                        {
                            ScreenManager.AddScreen(new BackgroundScreen(), null);
                            ScreenManager.AddScreen(new LevelSelectionMenuScreen(graphics), 0);

                        }
                        else
                            LoadingScreen.Load(ScreenManager, true, 0,
                              new GameplayScreen(graphics, levels[Array.IndexOf(levels, level) + 1]));
                    }
                }
                
                if (narrativeGiven == false) {
                    if (narrative[Array.IndexOf(levels, level)].Length == 0)
                    {
                        narrativeGiven = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && line < narrative[Array.IndexOf(levels, level)].Length && ticks > (tickCount + 15))
                    {
                        line++;
                        tickCount = ticks;
                        if (line == narrative[Array.IndexOf(levels, level)].Length)
                        {
                            narrativeGiven = true;
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && line < narrative[Array.IndexOf(levels, level)].Length && ticks > (tickCount + 15))
                    {
                        narrativeGiven = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.P))
                    {
                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new PauseScreen(graphics), null);
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Environment.Exit(0); 
                }
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(Controls control)
        {
  
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
            spriteBatch.Draw(menu, new Rectangle(0, map.Height*50, map.Width*50, 100), Color.White);
            if (narrativeGiven == false && narrative[Array.IndexOf(levels, level)].Length != 0)
            {
                spriteBatch.DrawString(font, narrative[Array.IndexOf(levels, level)][line], new Vector2(15, map.Height * 50 + 20), Color.Black);
                spriteBatch.DrawString(font, "(Enter to Continue, Space to Skip)", new Vector2(400, map.Height * 50 + 60), Color.Black);
            }
            else
            {
                // Level Name rectangle
                int levelRectSize = (int)(level.Length * 11.2);
                spriteBatch.Draw(levelRect, new Rectangle(400-(levelRectSize/2), 0, levelRectSize, 30), Color.YellowGreen); // find a better color
                spriteBatch.DrawString(font, level, new Vector2(400-(levelRectSize/2)+5, 0), Color.White);
                if (!youWinYet)
                {
                    spriteBatch.DrawString(font, "Grass mowed:" + mower.totalMowed + "/" + win_Num, new Vector2(15, map.Height * 50 + 20), Color.Black);
                    spriteBatch.DrawString(font, "Fuel Remaining:" + mower.cookies + ", Optimal Fuel Remaining: " + optimalFuel, new Vector2(15, map.Height * 50 + 50), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "YOU WIN!!!", new Vector2(15, map.Height * 50 + 20), Color.Black);
                    spriteBatch.DrawString(font, "Fuel Remaining: " + mower.cookies + ", Optimal Fuel Remaining: " + optimalFuel, new Vector2(15, map.Height * 50 + 40), Color.Black);
                    spriteBatch.DrawString(font, "(Press Enter for next level)", new Vector2(15, map.Height * 50 + 60), Color.Black);
                }
            }
            
            

        }

        #endregion
    }
}
