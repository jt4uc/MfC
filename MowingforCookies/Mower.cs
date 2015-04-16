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
       // public int nextDir; //values 0 through 4.  Should be a typedef.
        enum Direction {North, South, East, West, Stop};
        private Direction nextDir = Direction.Stop;
        private Direction curDir = Direction.Stop;
        private bool starting = true;
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

        public int SPEED = 5;

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
            this.totalMowed = 1;

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
            int patchesRows = patches.GetLength(0);
            int patchesCols = patches.GetLength(1);

            foreach (Spot s in patches)
            {
                if (this.x == s.x && this.y == s.y && s.canTraverse == true)
                {
                    s.isTraversed = true;
                }
            }
            
            if(starting)
            {
                if (controls.onPress(Keys.Right, Buttons.DPadRight))
                {
                    nextDir = Direction.East;
                    starting = false;
                    Move(controls, patches);

                }
                else if (controls.onPress(Keys.Left, Buttons.DPadLeft))
                {
                    nextDir = Direction.West;
                    starting = false;
                    Move(controls, patches);
                }
                else if (controls.onPress(Keys.Down, Buttons.DPadDown))
                {
                    nextDir = Direction.South;
                    starting = false;
                    Move(controls, patches);
                }
                else if (controls.onPress(Keys.Up, Buttons.DPadUp))
                {
                    nextDir = Direction.North;
                    starting = false;
                    Move(controls, patches);
                }
                else
                {
                    //spacebar = stop?
                }
                
                
            } 

            if (curDir == Direction.Stop)
            {
                curDir = nextDir;
            }
            if (!alive)
            {
                nextDir = Direction.Stop;
                image = deadMower;
            }
            else
            {
                Move(controls, patches);
            }
        }
        public void Move(Controls controls, Spot[,] patches)
        {
            int patchesRows = patches.GetLength(0);
            int patchesCols = patches.GetLength(1);
            if (curDir == Direction.East)//right
            {
                if ((arrayRowX + 1 == patchesRows) || (collisionObject(patches[arrayRowX + 1, arrayColY]) == false))
                {
                    curDir = nextDir;
                    if (controls.onPress(Keys.Left, Buttons.DPadLeft))
                    {
                        nextDir = Direction.West;
                    }
                    else if (controls.onPress(Keys.Down, Buttons.DPadDown))
                    {
                        nextDir = Direction.South;
                    }
                    else if (controls.onPress(Keys.Up, Buttons.DPadUp))
                    {
                        nextDir = Direction.North;
                    }
                    
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
                        if (patches[arrayRowX, arrayColY].ob != null)
                        {
                            cookies = cookies - patches[arrayRowX, arrayColY].ob.cookieCost;
                        }
                        else
                        {
                            cookies--;
                        }
                        if (cookies <= 0)
                        {
                            alive = false;
                        }
                    }
                } 
            }
            else if (curDir == Direction.West)//left
            {
                if ((arrayRowX - 1 == -1) || (collisionObject(patches[arrayRowX - 1, arrayColY]) == false))
                {
                    curDir = nextDir;
                    if (controls.onPress(Keys.Right, Buttons.DPadRight))
                    {
                        nextDir = Direction.East;

                    }
                    else if (controls.onPress(Keys.Down, Buttons.DPadDown))
                    {
                        nextDir = Direction.South;
                    }
                    else if (controls.onPress(Keys.Up, Buttons.DPadUp))
                    {
                        nextDir = Direction.North;
                    }
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
                        if (patches[arrayRowX, arrayColY].ob != null)
                        {
                            cookies = cookies - patches[arrayRowX, arrayColY].ob.cookieCost;
                        }
                        else
                        {
                            cookies--;
                        }
                        if (cookies <= 0)
                        {
                            alive = false;
                        }
                    }
                }
            }
            else if (curDir == Direction.South)//down
            {
                if ((arrayColY + 1 == patchesCols) || (collisionObject(patches[arrayRowX, arrayColY + 1]) == false))
                {
                    curDir = nextDir;
                    if (controls.onPress(Keys.Left, Buttons.DPadLeft))
                    {
                        nextDir = Direction.West;
                    }
                    else if (controls.onPress(Keys.Right, Buttons.DPadRight))
                    {
                        nextDir = Direction.East;

                    }
                    else if (controls.onPress(Keys.Up, Buttons.DPadUp))
                    {
                        nextDir = Direction.North;
                    }
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
                        if (patches[arrayRowX, arrayColY].ob != null)
                        {
                            cookies = cookies - patches[arrayRowX, arrayColY].ob.cookieCost;
                        }
                        else
                        {
                            cookies--;
                        }
                        if (cookies <= 0)
                        {
                            alive = false;
                        }
                    }
                }             
            }
            else if (curDir == Direction.North)//up
            {
                if ((arrayColY - 1 == -1) || (collisionObject(patches[arrayRowX, arrayColY - 1]) == false))
                {
                    curDir = nextDir;
                    if (controls.onPress(Keys.Left, Buttons.DPadLeft))
                    {
                        nextDir = Direction.West;
                    }
                    if (controls.onPress(Keys.Right, Buttons.DPadRight))
                    {
                        nextDir = Direction.East;

                    }
                    else if (controls.onPress(Keys.Down, Buttons.DPadDown))
                    {
                        nextDir = Direction.South;
                    }

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
                        if (patches[arrayRowX, arrayColY].ob != null)
                        {
                            cookies = cookies - patches[arrayRowX, arrayColY].ob.cookieCost;
                        }
                        else
                        {
                            cookies--;
                        }
                        if (cookies <= 0)
                        {
                            alive = false;
                        }
                    }
                }
            }
        }


        public bool collisionObject(Spot objectSpot) // returns if can traverse to that spot
        {
            return objectSpot.canTraverse;
        }
    }
}
