using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

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
        public bool alize;
        public double speed;
        public Texture2D mowerTexture;
        //public Animated Sprite?? mowerTextureMap

        //Content Manager?
        public Mower(Spot currentLocation, int cookies)
        {
            
            this.currentLocation = currentLocation;
            this.moveIndex = 0;
            this.x = currentLocation.x;
            this.y = currentLocation.y;
            this.cookies = cookies;
            this.alize = true;
            
            //speed = 5;
            //movedX = 0;
            collisionBox = new Rectangle(x, y, 50, 50);
        }

        //public int getX()
        //{
        //    return spriteX;
        //}
        //public int getY()
        //{
        //    return spriteY;
        //}
        //public void setX(int x)
        //{
        //    collisionBox.X = x;
        //    spriteX = x;
        //}
        //public void setY(int y)
        //{
        //    collisionBox.Y = y;
        //    spriteY = y;
        //}
        //public void setSpeed(int s)
        //{
        //    speed = s;
        //}
        //public Rectangle getBox()
        //{
        //    return collisionBox;
        //}

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("MiniMower.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(x, y, 50, 50), Color.White);
        }

        public void Update(Controls controls, List<Spot> patches, GameTime gameTime)
        {
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

        }

        public void Move(int direction, List<Spot> patches)
        {
            // Sideways Acceleration 
            if (direction == 1)
            {
                moveIndex += 5;

                if (moveIndex > 54 || collisionObject(patches[moveIndex]) == false)
                {
                    moveIndex -= 5;
                }
                else
                {
                    x = patches[moveIndex].x;
                    y = patches[moveIndex].y;

                    collisionBox.X += patches[moveIndex].x;
                    collisionBox.Y += patches[moveIndex].y;
                }

                //this.x = patches[3].getBox().Center.X;
                //this.y = patches[3].getBox().Center.Y;
            }
            else if (direction == 2)
            {
                moveIndex -= 5;
                if (moveIndex < 0 || collisionObject(patches[moveIndex]) == false)
                {
                    moveIndex += 5;
                }
                else
                {
                    x = patches[moveIndex].x;
                    y = patches[moveIndex].y;

                    collisionBox.X += patches[moveIndex].x;
                    collisionBox.Y += patches[moveIndex].y;
                }

            }
            else if (direction == 3)
            {
                moveIndex += 1;

                if (moveIndex % 5 == 0 || collisionObject(patches[moveIndex]) == false)
                {
                    moveIndex -= 1;
                }
                else
                {
                    x = patches[moveIndex].x;
                    y = patches[moveIndex].y;

                    collisionBox.X += patches[moveIndex].x;
                    collisionBox.Y += patches[moveIndex].y;
                }

            }

            else if (direction == 4)
            {
                moveIndex -= 1;

                if ((moveIndex + 1) % 5 == 0 || collisionObject(patches[moveIndex]) == false)
                {
                    moveIndex += 1;
                }
                else
                {
                    x = patches[moveIndex].x;
                    y = patches[moveIndex].y;

                    collisionBox.X += patches[moveIndex].x;
                    collisionBox.Y += patches[moveIndex].y;
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
                    alize = false;
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
