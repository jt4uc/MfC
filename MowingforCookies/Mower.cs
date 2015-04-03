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
        public int dir; //values 0 through 4.  Should be a typedef.
        const int time_between_moves = 10; //number of game loops between calling move
        public int current_time = 0; //tracks game loops
        public int moveIndex;
        private Rectangle collisionBox;
        public Spot currentLocation;
        public int cookies;
        public Spot targetLocation;
        public bool alive;
        public double speed;
        public Texture2D mowerTexture;
        public Texture2D deadMower;

        public int arrayRowX;
        public int arrayColY;

        //public Animated Sprite?? mowerTextureMap

        //Content Manager?
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

            //speed = 5;
            //movedX = 0;
            collisionBox = new Rectangle(x, y, 50, 50);
        }


        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("MiniMower.png");
            deadMower = content.Load<Texture2D>("DeadMiniMower.png");

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(x, y, 50, 50), Color.White);
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
                dir = 1;

            }
            else if (controls.onPress(Keys.Left, Buttons.DPadLeft))
            {
                dir = 2;
            }
            else if (controls.onPress(Keys.Down, Buttons.DPadDown))
            {
                dir = 3;
            }
            else if (controls.onPress(Keys.Up, Buttons.DPadUp))
            {
                dir = 4;
            }
            else
            {

            }

            if (current_time >= time_between_moves)
            {
                Move(dir, patches);
                current_time = 0;
            }
            else
            {
                current_time++;
            }

            if (!alive)
            {
                dir = 0;
            }


        }
        public void Move(int direction, Spot[,] patches)
        {
            int patchesRows = patches.GetLength(0);//24
            int patchesCols = patches.GetLength(1);//18
            int rowCoord = this.x;//0 
            int colCoord = this.y;//55

            

            // Sideways Acceleration 
            if (direction == 1)//right
            {
                if ((arrayRowX + 1 == patchesRows) || (collisionObject(patches[arrayRowX + 1, arrayColY]) == false))
                {
                }
                else
                {
                    this.x = patches[arrayRowX + 1, arrayColY].x;
                    this.collisionBox.X = patches[arrayRowX + 1, arrayColY].x;
                    this.arrayRowX = this.arrayRowX + 1;
                }
            }
            else if (direction == 2)//left
            {
                if ((arrayRowX - 1 == -1) || (collisionObject(patches[arrayRowX - 1, arrayColY]) == false))
                {
                }
                else
                {
                    this.x = patches[arrayRowX - 1, arrayColY].x;
                    this.collisionBox.X = patches[arrayRowX - 1, arrayColY].x;
                    this.arrayRowX = this.arrayRowX - 1;
                }

            }
            else if (direction == 3)//down
            {
                if ((arrayColY + 1 == patchesCols) || (collisionObject(patches[arrayRowX, arrayColY+1]) == false))
                {
                }
                else
                {
                    this.y = patches[arrayRowX, arrayColY+1].y;
                    this.collisionBox.Y = patches[arrayRowX, arrayColY+1].y;
                    this.arrayColY = this.arrayColY +1;
                }
            }
            else if (direction == 4)//down
            {
                if ((arrayColY - 1 == -1) || (collisionObject(patches[arrayRowX, arrayColY - 1]) == false))
                {
                }
                else
                {
                    this.y = patches[arrayRowX, arrayColY-1].y;
                    this.collisionBox.Y = patches[arrayRowX, arrayColY - 1].y;
                    this.arrayColY = this.arrayColY - 1;
                }
            }
        }


        public bool collisionObject(Spot objectSpot)
        {
            if (objectSpot.canTraverse == true)
            {
                return true;
            }
            else
            {
                if (objectSpot.getEnemy() != null)
                {

                    alive = false;
                    image = deadMower;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //collisionEnemy
        //updateCookieAmount
    }
}
