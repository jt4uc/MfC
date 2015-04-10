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
        public int recX = 45;
        public int recY = 45;
        public Boolean exploding = false;
        public int tickCount = 99999;

        public Obstacle(Spot currentLocation, String obstacleType, int arrayRowX, int arrayColY)
        {
            this.currentLocation = currentLocation;
            this.x = this.currentLocation.x;
            this.y = this.currentLocation.y;
            this.obstacleType = obstacleType;
            if (this.obstacleType.Equals("gravel"))
            {
                this.canTraverse = true;
            }
            else
            {
                this.canTraverse = false;
            }
            this.arrayColY = arrayColY;
            this.arrayRowX = arrayRowX;
            this.cbox = new Rectangle(this.x, this.y, recX, recY);
            this.backupCbox = this.cbox;
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
            else if (obstacleType.Equals("grandma"))
            {
                image = content.Load<Texture2D>("grandma.png");
            }
            // put uncut_grass here?
        }

        public void Draw(SpriteBatch sb)
        {
            if (exploding)
            {
                sb.Draw(image, cbox, Color.White);//lmao. only part of the image. weird.
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
                    //Console.WriteLine("here in gravel");
                    //now to get the 3x3 square array around this stupid thing. 
                    int xMin = mower.arrayRowX;
                    int xMax = mower.arrayRowX;
                    int yTop = mower.arrayColY;
                    int yBottom = mower.arrayColY;

                    int patchesXMax = patches.GetLength(0);
                    int patchesYBottom = patches.GetLength(1);

                    //Console.WriteLine("array x,y: " + arrayRowX + ", " + arrayColY);

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
                    //now we have array coordinates of the topLeft and bottomRight of the new Rectangle
                    //Console.WriteLine("things for rectangle: " + xMin + "," + yTop + ". " + xMax + "," + yBottom);
                    //UNTESTED. DON'T KNOW HOW TO SET GRAVEL SO I ONLY TESTED IT IN THE THING

                    int stupidWidth = (xMax - xMin + 1) * 50;
                    int stupidHeight = (yBottom - yTop + 1) * 50;
                    result = new Rectangle(patches[xMin, yTop].x, patches[xMin, yTop].y, stupidWidth, stupidHeight);

                    return result;
                    break;
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
            if (obstacleType == "gravel")
            {
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
