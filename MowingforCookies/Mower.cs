using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.Diagnostics;

namespace MowingforCookies
{
    class Mower : Sprite
    {
        public int x;
        public int y;
        public int nextDir; //values 0 through 4.  Should be a typedef.
        public int curDir = 0;
        public int moveIndex;
        public Rectangle collisionBox;
        public Spot currentLocation;
        public int cookies;
        public bool alive;
        public Texture2D deadMower;
        public int totalMowed;
        public int arrayRowX;
        public int arrayColY;

        public int recX = 50;
        public int recY = 50;

        const int SPEED = 5;

        public Mower(Spot currentLocation, int cookies)
        {

            this.currentLocation = currentLocation;
            this.moveIndex = 0;
            this.x = currentLocation.x;
            this.y = currentLocation.y;
            this.cookies = cookies;
            this.alive = true;
            this.arrayRowX = currentLocation.arrayRowX;
            this.arrayColY = currentLocation.arrayColY;
            this.totalMowed = 0;

            collisionBox = new Rectangle(x, y, recX, recY);
        }


        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("MiniMower.png");
            deadMower = content.Load<Texture2D>("DeadMiniMower.png");
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(x, y, recX, recY), Color.White);
        }
        public void Update(Controls controls, Spot[,] patches, GameTime gameTime)
        {
            foreach (Spot s in patches)
            {
                if (this.x == s.x && this.y == s.y && s.canTraverse == true)
                {
                    s.isTraversed = true;
                }
            }
            if (controls.onPress(Keys.Right, Buttons.DPadRight))
            {
                nextDir = 1;

            }
            else if (controls.onPress(Keys.Left, Buttons.DPadLeft))
            {
                nextDir = 2;
            }
            else if (controls.onPress(Keys.Down, Buttons.DPadDown))
            {
                nextDir = 3;
            }
            else if (controls.onPress(Keys.Up, Buttons.DPadUp))
            {
                nextDir = 4;
            }
            else
            {
                //spacebar = stop?
            }
            if (curDir == 0)
            {
                curDir = nextDir;
            }
            if (!alive)
            {
                nextDir = 0;
                image = deadMower;
            }
            else
            {
                Move(patches);
            }
        }
        public void Move(Spot[,] patches)
        {
            int patchesRows = patches.GetLength(0);
            int patchesCols = patches.GetLength(1);
            if (curDir == 1)//right
            {
                if ((arrayRowX + 1 == patchesRows) || (collisionObject(patches[arrayRowX + 1, arrayColY]) == false))
                {
                    curDir = nextDir;
                }
                else
                {
                    this.x = this.x + SPEED;
                    this.collisionBox.X = this.x;
                    if (this.x >= patches[arrayRowX + 1, arrayColY].x)
                    {
                        this.arrayRowX = this.arrayRowX + 1;
                        curDir = nextDir;
                        if (patches[arrayRowX, arrayColY].grassMowed == false)
                        {
                            totalMowed++;
                            patches[arrayRowX, arrayColY].grassMowed = true;
                        }
                        cookies--;
                        if (cookies == 0)
                        {
                            alive = false;
                        }
                    }
                } 
            }
            else if (curDir == 2)//left
            {
                if ((arrayRowX - 1 == -1) || (collisionObject(patches[arrayRowX - 1, arrayColY]) == false))
                {
                    curDir = nextDir;
                }
                else
                {
                    this.x = this.x - SPEED;
                    this.collisionBox.X = this.x;
                    if (this.x <= patches[arrayRowX - 1, arrayColY].x)
                    {
                        this.arrayRowX = this.arrayRowX - 1;
                        curDir = nextDir;
                        if (patches[arrayRowX, arrayColY].grassMowed == false)
                        {
                            totalMowed++;
                            patches[arrayRowX, arrayColY].grassMowed = true;
                        }
                        cookies--;
                        if (cookies == 0)
                        {
                            alive = false;
                        }
                    }
                }
            }
            else if (curDir == 3)//down
            {
                if ((arrayColY + 1 == patchesCols) || (collisionObject(patches[arrayRowX, arrayColY + 1]) == false))
                {
                    curDir = nextDir;
                }
                else
                {
                    this.y = this.y + SPEED;
                    this.collisionBox.Y = this.y;
                    if (this.y >= patches[arrayRowX, arrayColY + 1].y)
                    {
                        this.arrayColY = this.arrayColY + 1;
                        curDir = nextDir;
                        if (patches[arrayRowX, arrayColY].grassMowed == false)
                        {
                            totalMowed++;
                            patches[arrayRowX, arrayColY].grassMowed = true;
                        }
                        cookies--;
                        if (cookies == 0)
                        {
                            alive = false;
                        }
                    }
                }             
            }
            else if (curDir == 4)//up
            {
                if ((arrayColY - 1 == -1) || (collisionObject(patches[arrayRowX, arrayColY - 1]) == false))
                {
                    curDir = nextDir;
                }
                else
                {
                    this.y = this.y - SPEED;
                    this.collisionBox.Y = this.y;
                    if (this.y <= patches[arrayRowX, arrayColY - 1].y)
                    {
                        this.arrayColY = this.arrayColY - 1;
                        curDir = nextDir;
                        if (patches[arrayRowX, arrayColY].grassMowed == false)
                        {
                            totalMowed++;
                            patches[arrayRowX, arrayColY].grassMowed = true;
                        }
                        cookies--;
                        if (cookies == 0)
                        {
                            alive = false;
                        }
                    }
                }
            }
        }


        public bool collisionObject(Spot objectSpot) // used in move
        {
            if (objectSpot.canTraverse == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //collisionEnemy
        //updateCookieAmount
    }
}
