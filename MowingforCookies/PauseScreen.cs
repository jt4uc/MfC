#region File Description
//-----------------------------------------------------------------------------
// 
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
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class PauseScreen : MenuScreen
    {

        GraphicsDeviceManager graphics;
        ContentManager content;
        Texture2D backgroundTexture;

        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public PauseScreen(GraphicsDeviceManager graphics)
            : base("")
        {
            this.graphics = graphics;
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Continue");
            MenuEntry levelSelectionEntry = new MenuEntry("Level Selection");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            levelSelectionEntry.Selected += LevelSelectionEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(levelSelectionEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            GameScreen[] screens = ScreenManager.GetScreens();
            foreach (GameScreen screen in screens)
            {
                if (screen.GetType().Name.Equals("BackgroundScreen"))
                {
                    ScreenManager.RemoveScreen(screen);
                }
            }
            ScreenManager.RemoveScreen(this);
        }

        void LevelSelectionEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
            //                   new GameplayScreen(graphics));
            ScreenManager.AddScreen(new LevelSelectionMenuScreen(graphics), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {


            Environment.Exit(0);
            
        }




        #endregion
    }
}
