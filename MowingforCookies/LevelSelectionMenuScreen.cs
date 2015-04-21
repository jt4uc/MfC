#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MowingforCookies
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class LevelSelectionMenuScreen : MenuScreen
    {
        #region Fields

        GraphicsDeviceManager graphics;
        MenuEntry backMenuEntry;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public LevelSelectionMenuScreen(GraphicsDeviceManager graphics)
            : base("") // Options
        {
            this.graphics = graphics;
            // Create our menu entries.
            MenuEntry level1 = new MenuEntry("The Owner's Quick Guide to a Cookie-Powered Mower");
            MenuEntry level2 = new MenuEntry("Perfection is the Enemy of Good Enough"); //Thar be another level arrrrr
            MenuEntry level3 = new MenuEntry("Oh Gnome! America's Lastest Pestilence");
            MenuEntry level4 = new MenuEntry("The Dreaded 41"); //Maybe Grandma Should Go Apartment Hunting
            MenuEntry level5 = new MenuEntry("This is not Mowing for Inheritance");  //Watch for the Roamin' Gnomes

           

            MenuEntry backMenuEntry = new MenuEntry("\nBack");
            MenuEntry exitEntry = new MenuEntry("\nExit");

            

            // Hook up menu event handlers.
            level1.Selected += Level1Selected;
            level2.Selected += Level2Selected;
            level3.Selected += Level3Selected;
            level4.Selected += Level4Selected;
            level5.Selected += Level5Selected;
            backMenuEntry.Selected += OnCancel;
            exitEntry.Selected += OnExit;

            // Add entries to the menu.
            MenuEntries.Add(level1);
            MenuEntries.Add(level2);
            MenuEntries.Add(level3);
            MenuEntries.Add(level4);
            MenuEntries.Add(level5);
            MenuEntries.Add(backMenuEntry);
            MenuEntries.Add(exitEntry);

            SetMenuEntryText();
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            //ungulateMenuEntry.Text = "Preferred ungulate: " + currentUngulate;
            //languageMenuEntry.Text = "Language: " + languages[currentLanguage];
            //frobnicateMenuEntry.Text = "Frobnicate: " + (frobnicate ? "on" : "off");
            //elfMenuEntry.Text = "elf: " + elf;
            
            
        }


        #endregion

        #region Handle Input

        void Level1Selected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen(graphics, "intro level 1A"));
        }

        void Level2Selected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen(graphics, "intro level 2A"));
        }

        void Level3Selected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen(graphics, "intro level 3A"));
        }

        void Level4Selected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen(graphics, "level 1A"));
        }

        void Level5Selected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen(graphics, "ice_level_10"));
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            GameScreen[] screens = ScreenManager.GetScreens();
            bool noPauseOrMain = true;
            foreach (GameScreen screen in screens)
            {
                if (screen.GetType().Name.Equals("PauseScreen") || screen.GetType().Name.Equals("MainMenuScreen"))
                {
                    ScreenManager.RemoveScreen(this);
                    noPauseOrMain = false;
                }
            }
            if (noPauseOrMain)
            {
                ScreenManager.AddScreen(new BackgroundScreen(), null);
                ScreenManager.AddScreen(new MainMenuScreen(graphics), null);
            }
            

        }

        void OnExit(object sender, PlayerIndexEventArgs e)
        {
            Environment.Exit(0);

        }

        #endregion
    }
}
