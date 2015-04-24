using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
namespace MowingforCookies
{
    class Obstacle : Sprite
    {
        public Spot currentLocation;
        public String obstacleType;
        public int x;
        public int y;
        public Boolean canTraverse;
        public Texture2D boom;
        public Texture2D gravel;

        public int arrayRowX;
        public int arrayColY;

        public Rectangle cbox;
        public Rectangle backupCbox;
        public int recX = 50;
        public int recY = 50;
        public Boolean exploding = false;
        public int tickCount = 99999;
        public int cookieCost;
        public int targetArrayRowX; //to be used to set a water target
        public int targetArrayColY;

        //used for water!
        public Obstacle(Spot currentLocation, String obstacleType, int arrayRowX, int arrayColY, int targetArrayRowX, int targetArrayColY)
        {
            this.currentLocation = currentLocation;
            this.x = this.currentLocation.x;
            this.y = this.currentLocation.y;
            this.obstacleType = obstacleType;
            this.canTraverse = true;
            this.arrayColY = arrayColY;
            this.arrayRowX = arrayRowX;
            this.targetArrayColY = targetArrayColY;
            this.targetArrayRowX = targetArrayRowX;

            this.cbox = new Rectangle(this.x, this.y, recX, recY);
            this.backupCbox = this.cbox;
            this.cookieCost = 5;
        }


        public Obstacle(Spot currentLocation, String obstacleType, int arrayRowX, int arrayColY)
        {
            this.currentLocation = currentLocation;
            this.x = this.currentLocation.x;
            this.y = this.currentLocation.y;
            this.obstacleType = obstacleType;
            if (this.obstacleType.Equals("gravel") || this.obstacleType.Equals("bush") || this.obstacleType.Equals("branch") )
            {
                this.canTraverse = true;
            }
            else
            {
                this.canTraverse = false;
            }
            this.arrayColY = arrayColY;
            this.arrayRowX = arrayRowX;
            if (this.obstacleType.Equals("house"))
            {
                this.cbox = new Rectangle(this.x, this.y, 2 * recX, 2 * recY);
            }
            else
            {
                this.cbox = new Rectangle(this.x, this.y, recX, recY); 
            }
            this.backupCbox = this.cbox;
            this.cookieCost = 15;
        }

        public void LoadContent(ContentManager content)
        {
            if (obstacleType.Equals("tree"))
            {
                image = content.Load<Texture2D>("Oak-Tree-Sprite.png");
            }
            else if (obstacleType.Equals("gravel"))
            {
                gravel = content.Load<Texture2D>("gravel.png");
                boom = content.Load<Texture2D>("boom.png");
                image =  gravel;    
            }
            else if (obstacleType.Equals("bush"))
            {
                image = content.Load<Texture2D>("bush.png");
            }
            else if (obstacleType.Equals("fence bottom"))
            {
                image = content.Load<Texture2D>("fence bottom.png");
            }
            else if (obstacleType.Equals("fence top"))
            {
                image = content.Load<Texture2D>("fence top.png");
            }
            else if (obstacleType.Equals("fence left"))
            {
                image = content.Load<Texture2D>("fence left.png");
            }
            else if (obstacleType.Equals("fence right"))
            {
                image = content.Load<Texture2D>("fence right.png");
            }
            else if (obstacleType.Equals("grandma"))
            {
                image = content.Load<Texture2D>("grandma.png");
            }
            else if (obstacleType.Equals("water"))
            {

                image = content.Load<Texture2D>("water.png");
            }
            else if (obstacleType.Equals("house"))
            {
                image = content.Load<Texture2D>("house.png");
            }
            else if (obstacleType.Equals("branch"))
            {
                image = content.Load<Texture2D>("branch.png");
            }
            // put uncut_grass here?
        }

        public void Draw(SpriteBatch sb)
        {
            if (exploding)
            {
                sb.Draw(image, cbox, Color.White);
            }
            else
            { 
                sb.Draw(image, backupCbox, Color.White);
            }
        }
        public void collidesEnemy(Enemy e)
        {
            if (this.cbox.Intersects(e.cbox))
            {
                e.alive = false;
            }
        }

        public Rectangle obRec(Spot[,] patches, Mower mower)
        {
            Rectangle result;
            switch (this.obstacleType)
            {
                case "gravel":
                    int xMin = mower.arrayRowX;
                    int xMax = mower.arrayRowX;
                    int yTop = mower.arrayColY;
                    int yBottom = mower.arrayColY;

                    int patchesXMax = patches.GetLength(0);
                    int patchesYBottom = patches.GetLength(1);

                    if (0 <= mower.arrayColY - 1)
                    {
                        yTop = mower.arrayColY - 1;
                    }
                    if (mower.arrayColY + 1 < patchesYBottom)
                    {
                        yBottom = mower.arrayColY + 1;
                    }
                    if (0 <= mower.arrayRowX - 1)
                    {
                        xMin = mower.arrayRowX - 1;
                    }
                    if (mower.arrayRowX + 1 < patchesXMax)
                    {
                        xMax = mower.arrayRowX + 1;
                    }
                    int stupidWidth = (xMax - xMin + 1) * 50;
                    int stupidHeight = (yBottom - yTop + 1) * 50;
                    result = new Rectangle(patches[xMin, yTop].x, patches[xMin, yTop].y, stupidWidth, stupidHeight);

                    return result;
                    
            }

            return result = new Rectangle(mower.x, mower.y, 50, 50);

        }
        public void changeBox(Rectangle lol)
        {
            this.cbox = lol;
            image = boom;
        }
        public void changeBoxBack()
        {
            this.cbox = this.backupCbox;
            image = gravel;
        }

        public void Update(Spot[,] patches, Mower mower, List<Enemy> enemies, int ticks)
        {
            if (obstacleType == "water")
            {
                
                    if (mower.x == this.x && mower.y == this.y)
                    {
                        int targetSpotXCoord = patches[this.targetArrayRowX, this.targetArrayColY].x;
                        int targetSpotYCoord = patches[this.targetArrayRowX, this.targetArrayColY].y;

                        mower.arrayRowX = this.targetArrayRowX;
                        mower.arrayColY = this.targetArrayColY;
                        mower.x = targetSpotXCoord;
                        mower.y = targetSpotYCoord;
                        mower.collisionBox.X = targetSpotXCoord;
                        mower.collisionBox.Y = targetSpotYCoord;
                        
                    
                }
                   
            }
            if (obstacleType == "gravel")
            {
                Rectangle r = obRec(patches, mower);
                if (mower.x == this.x && mower.y == this.y)
                {
                    tickCount = ticks;
                    exploding = true;
                    image = boom;
                    changeBox(r);
                }
                else
                {
                    if (ticks > (tickCount + 50))
                    {
                        exploding = false;
                        image = gravel;
                        changeBoxBack();
                    }
                }
                foreach (Enemy e in enemies)
                {
                    collidesEnemy(e);
                } 
            }
            
        }

        public void setSpot(Spot s)
        {
            this.currentLocation = s;
            this.x = s.x;
            this.y = s.y;
        }
    }
}
