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



        //tileTexture: Texture2D

        public Rectangle collisionBox;
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

        public void Update(ContentManager content, Spot[,] patches, Mower mower, List<Enemy> enemies, int ticks)
        {
            if (this.isTraversed == true)
            {
                image = content.Load<Texture2D>("mowed_grass.png");
            }
        }
        
        public void traverseEffect(Obstacle ob, Rectangle obRec, Mower mower, List<Enemy> enemies, int ticks)
        {
            String obType = ob.obstacleType;
            switch (obType)
            {
                case "tree":
                    break;
                case "water":
                    break;
                case "gravel":
                    break;
                case "bush":
                    break;
                case "grandma":
                    break;
                case "fence":
                    break;
            }
        }

        public void setObstacle(Obstacle ob)
        {
            this.ob = ob;
            if (ob.canTraverse == false)
            {
                this.canTraverse = false;
            }

        }

    }
}
