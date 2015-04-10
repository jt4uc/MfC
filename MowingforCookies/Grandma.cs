﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace MowingforCookies
{
    class Grandma : Sprite
    {
        public int x;
        public int y;
        public int moveIndex;
        public Spot currentLocation;
        public Texture2D deadGrandma;

        public int arrayRowX;
        public int arrayColY;
        public Boolean visible;
        public int[] moveSequence;
        public Boolean alive = true;

        const int SPEED = 2;

        public Rectangle cbox;
        public int recX = 48;
        public int recY = 50;

        private Random rand = new Random();
        private int nextDir = 0;

        //Content Manager?
        public Grandma(Spot currentLocation, int arrayRowX, int arrayColY)
        {

            this.currentLocation = currentLocation;
            this.moveIndex = 0;
            this.x = currentLocation.x;
            this.y = currentLocation.y;
            this.arrayColY = arrayColY;
            this.arrayRowX = arrayRowX;
            this.visible = true;

            this.cbox = new Rectangle(x, y, recX, recY);


        }

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("grandma.png");
            deadGrandma = content.Load<Texture2D>("deadGrandma2.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(x, y, recX, recY), Color.White);
        }

        public void Update(Mower mower, Controls controls, Spot[,] patches, GameTime gameTime)
        {
            if (this.alive)
            {
                Move(mower, patches);
            }
            else
            {
                image = deadGrandma;
            }

        }

        public void Move(Mower mower, Spot[,] patches)
        {

            if (nextDir == 0)
            {
               nextDir = rand.Next(5);
            }

            if (nextDir == 1)//right
            {

                this.x = this.x + SPEED;
                this.cbox.X = this.x;
                if (this.x >= patches[arrayRowX + 1, arrayColY].x)
                {

                    this.arrayRowX = this.arrayRowX + 1; //update grid position

                    nextDir = rand.Next(5);
                }

            }
            else if (nextDir == 2)//left
            {
                this.x = this.x - SPEED;
                this.cbox.X = this.x;
                if (this.x <= patches[arrayRowX - 1, arrayColY].x)
                {
                    this.arrayRowX = this.arrayRowX - 1; //update grid position

                    nextDir = rand.Next(5);
                }

            }
            else if (nextDir == 3)//down
            {
                this.y = this.y + SPEED;
                this.cbox.Y = this.y;
                if (this.y >= patches[arrayRowX, arrayColY + 1].y)
                {

                    this.arrayColY = this.arrayColY + 1; //update grid position

                    nextDir = rand.Next(5);
                }
            }
            else if (nextDir == 4)//up
            {
                this.y = this.y - SPEED;
                this.cbox.Y = this.y;
                if (this.y >= patches[arrayRowX, arrayColY - 1].y)
                {

                    this.arrayColY = this.arrayColY - 1; //update grid position

                    nextDir = rand.Next(5);
                }
            }
        }

    }
}
