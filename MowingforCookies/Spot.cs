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
    class Spot : Sprite
    {
        public int x;
        public int y;
        public int arrayRowX;
        public int arrayColY;
        public bool grassMowed;
        public bool isTraversed;
        public bool canTraverse;
        public int travelCost; //of cookies
        public int cookiesGained;
        public Obstacle ob;
        public Cookie c;



        //tileTexture: Texture2D

        private Rectangle collisionBox;
        public int cbWidth = 50;
        public int cbHeight = 50;

        public Spot(int x, int y, bool isTraversed, int travelCost, int cookiesGained
            , int arrayRowX, int arrayColY, Obstacle ob)
        {
            this.x = x;
            this.y = y;
            this.grassMowed = false;
            this.isTraversed = isTraversed;
            this.travelCost = travelCost;
            this.cookiesGained = cookiesGained;
            this.ob = ob;
            this.c = null;
            this.arrayRowX = arrayRowX;
            this.arrayColY = arrayColY;
            this.canTraverse = true;
            if (ob.canTraverse == false)
            {
                this.canTraverse = false;
            }


            this.collisionBox = new Rectangle(x, y, cbWidth, cbHeight);

        }
        public Spot(int x, int y, bool isTraversed, int travelCost, int cookiesGained, int arrayRowX, int arrayColY)
        {
            this.x = x;
            this.y = y;
            this.isTraversed = isTraversed;
            this.travelCost = travelCost;
            this.cookiesGained = cookiesGained;
            this.ob = null;
            this.canTraverse = true;
            this.c = null;
            this.arrayRowX = arrayRowX;
            this.arrayColY = arrayColY;

            this.collisionBox = new Rectangle(x, y, cbWidth, cbHeight);

        }

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("grass.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(x, y, 50, 50), Color.White);
        }

        public bool mowerHasTraversed()
        {
            return this.isTraversed;
        }

        public void Update(ContentManager content, Spot[,] patches, Mower mower)
        {
            if (this.isTraversed == true)
            {
                image = content.Load<Texture2D>("mowed grass.png");
            }
            if ( this.ob != null && this.x == mower.x && this.y == mower.y) //if ob exists
            {
                //Console.WriteLine("mower at: " + arrayRowX + ", " + arrayColY + ". ob:  " + ob.obstacleType);
                Rectangle result = obRec(patches, ob.obstacleType, mower); //return dynamically sized rectangle if gravel
                traverseEffect(this.ob, result, mower);

            }
        }
        
        public Rectangle obRec(Spot[,] patches, String obType, Mower mower)
        {
            Rectangle result;
            switch (obType)
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
                    
                    if (0<=mower.arrayColY - 1)
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
                    int stupidHeight = (yBottom - yTop + 1) *50;
                    result = new Rectangle(patches[xMin,yTop].x, patches[xMin,yTop].y, stupidWidth, stupidHeight);

                    return result;
                    break;
            }
            return result = new Rectangle(mower.x,mower.y,50,50);
            
        }
        public void traverseEffect(Obstacle ob, Rectangle obRec, Mower mower)
        {
            String obType = ob.obstacleType;
            switch (obType)
            {
                case "tree":
                    break;
                case "water":
                    break;
                case "gravel":
                    ob.Update(obRec, mower );
                    break;
                case "bush":
                    break;
                case "grandma":
                    break;
                case "fence":
                    break;
            }
        }




        public Obstacle getObstacle()
        {
            return ob;
        }

        public void setObstacle(Obstacle ob)
        {
            this.ob = ob;
            if (ob.canTraverse == false)
            {
                this.canTraverse = false;
            }

        }

        public Rectangle getBox()
        {
            return collisionBox;
        }
        public void setCookie(Cookie c)
        {
            this.c = c;
        }
    }
}
