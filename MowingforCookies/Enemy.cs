﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace MowingforCookies
{
    class Enemy : Sprite
    {
        public int x;
        public int y;
        public int moveIndex;   
        public Spot currentLocation;
        public Spot targetLocation;
        public double speed;
        public Texture2D enemyTexture;
        //private Rectangle collisionBox;
        //public Animated Sprite?? mowerTextureMap

        public String type;
        public int arrayRowX;
        public int arrayColY;
        public Boolean visible;
        public int[] moveSequence;
        public Boolean alive = true;

        const int SPEED = 2;

        public Rectangle cbox;
        public int recX = 48;
        public int recY = 50;

        //Content Manager?
        public Enemy(Spot currentLocation, int cookies, int arrayRowX, int arrayColY, int[] sequence)
        {

            this.currentLocation = currentLocation;
            this.moveIndex = 0;
            this.x = currentLocation.x;
            this.y = currentLocation.y;
            this.type = "gnome";
            this.arrayColY = arrayColY;
            this.arrayRowX = arrayRowX;
            this.visible = false;
            this.moveSequence = sequence;

            this.cbox = new Rectangle(x, y, recX, recY);


        }

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("gnome.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(x, y, recX, recY), Color.White);
        }

        public void Update(Mower mower, Controls controls, Spot[,] patches, GameTime gameTime)
        {
            if (mower.arrayRowX > this.arrayRowX - 3 && mower.arrayRowX < this.arrayRowX + 3 && mower.arrayColY > this.arrayColY - 3 && mower.arrayColY < this.arrayColY + 3)
            {
                this.visible = true;
            }

            if (this.visible)
            {
                Move(mower, patches);
            }

        }

        public void setType(String s)
        {
            this.type = s;
        }

        public void Move(Mower mower, Spot[,] patches)
        {
            int nextDir;
            if (moveSequence.Length == 0)
            {
                nextDir = 0;
            }
            else
            {
                nextDir = moveSequence[moveIndex];

            }

            

            if (nextDir == 1)//right
            {

                this.x = this.x + SPEED;
                this.cbox.X = this.x;
                if (this.x >= patches[arrayRowX + 1, arrayColY].x)
                {

                    this.arrayRowX = this.arrayRowX + 1; //update grid position

                    moveIndex++;
                    if (moveIndex >= moveSequence.Length)
                    {
                        moveIndex = 0;
                    }
                }

            }
            else if (nextDir == 2)//left
            {
                this.x = this.x - SPEED;
                this.cbox.X = this.x;
                if (this.x <= patches[arrayRowX - 1, arrayColY].x)
                {
                    this.arrayRowX = this.arrayRowX - 1; //update grid position

                    moveIndex++;
                    if (moveIndex >= moveSequence.Length)
                    {
                        moveIndex = 0;
                    }
                }

            }
            else if (nextDir == 3)//down
            {
                this.y = this.y + SPEED;
                this.cbox.Y = this.y;
                if (this.y >= patches[arrayRowX, arrayColY + 1].y)
                {

                    this.arrayColY = this.arrayColY + 1; //update grid position

                    moveIndex++;
                    if (moveIndex >= moveSequence.Length)
                    {
                        moveIndex = 0;
                    }
                }
            }
            else if (nextDir == 4)//up
            {
                this.y = this.y - SPEED;
                this.cbox.Y = this.y;
                if (this.y >= patches[arrayRowX, arrayColY - 1].y)
                {

                    this.arrayColY = this.arrayColY - 1; //update grid position

                    moveIndex++;
                    if (moveIndex >= moveSequence.Length)
                    {
                        moveIndex = 0;
                    }
                }
            }
        }

    }
}
