using System;
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
        public bool alive;
        public double speed;
        public Texture2D enemyTexture;
        //private Rectangle collisionBox;
        //public Animated Sprite?? mowerTextureMap

        public String type;
        public int arrayRowX;
        public int arrayColY;
        public Boolean visible;
        public int[] moveSequence;

        public int currentTime = 0;
        const int TIME_BETWEEN_MOVES = 30;

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
                if (currentTime >= TIME_BETWEEN_MOVES)
                {
                    Move(mower, patches);
                    currentTime = 0;
                }
                else
                {
                    currentTime++;
                }
            }

            //Move(mower, patches);
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
                moveIndex++;
                if (moveIndex >= moveSequence.Length)
                {
                    moveIndex = 0;
                }
            }

            

            if (nextDir == 1)//right
            {
                patches[arrayRowX, arrayColY].setEnemy(null); //leave current tile
                patches[arrayRowX, arrayColY].canTraverse = true;

                this.arrayRowX = this.arrayRowX + 1; //update grid position
                this.x = patches[arrayRowX, arrayColY].x; //update pixel position

                patches[arrayRowX, arrayColY].setEnemy(this); //enter new tile

            }
            else if (nextDir == 2)//left
            {
                patches[arrayRowX, arrayColY].setEnemy(null); //leave current tile
                patches[arrayRowX, arrayColY].canTraverse = true;

                this.arrayRowX = this.arrayRowX - 1; //update grid position
                this.x = patches[arrayRowX, arrayColY].x; //update pixel position

                patches[arrayRowX, arrayColY].setEnemy(this); //enter new tile

            }
            else if (nextDir == 3)//down
            {
                patches[arrayRowX, arrayColY].setEnemy(null); //leave current tile
                patches[arrayRowX, arrayColY].canTraverse = true;

                this.arrayColY = this.arrayColY + 1; //update grid position
                this.x = patches[arrayRowX, arrayColY].y; //update pixel position

                patches[arrayRowX, arrayColY].setEnemy(this); //enter new tile
            }
            else if (nextDir == 4)//up
            {
                patches[arrayRowX, arrayColY].setEnemy(null); //leave current tile
                patches[arrayRowX, arrayColY].canTraverse = true;

                this.arrayColY = this.arrayColY - 1; //update grid position
                this.x = patches[arrayRowX + 1, arrayColY].y; //update pixel position

                patches[arrayRowX, arrayColY].setEnemy(this); //enter new tile
            }
        }

    }
}
