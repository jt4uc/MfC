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
        public bool isTraversed;
        public bool canTraverse;
        public int travelCost; //of cookies
        public int cookiesGained;
        public Obstacle ob;
        public Enemy e;
        public Cookie c;

        //tileTexture: Texture2D

        private Rectangle collisionBox;
        public int cbWidth = 50;
        public int cbHeight = 50;

        public Spot(int x, int y, bool isTraversed, int travelCost, int cookiesGained
            , Obstacle ob)
        {
            this.x = x;
            this.y = y;
            this.isTraversed = isTraversed;
            this.travelCost = travelCost;
            this.cookiesGained = cookiesGained;
            this.ob = ob;
            this.e = null;
            this.c = null;
            this.canTraverse = true;
            if (ob.canTraverse == false)
            {
                this.canTraverse = false;
            }


            this.collisionBox = new Rectangle(x, y, cbWidth, cbHeight);

        }
        public Spot(int x, int y, bool isTraversed, int travelCost, int cookiesGained)
        {
            this.x = x;
            this.y = y;
            this.isTraversed = isTraversed;
            this.travelCost = travelCost;
            this.cookiesGained = cookiesGained;
            this.ob = null;
            this.e = null;
            this.canTraverse = true;
            this.c = null;

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

        public void Update(ContentManager content)
        {
            if (this.isTraversed == true)
            {
                image = content.Load<Texture2D>("mowed grass.png");
            }
        }

        public void traverseEffect(Obstacle o)
        {
            String oType = o.obstacleType;
            switch (oType)
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

        public Enemy getEnemy()
        {
            return this.e;
        }


        public void setEnemy(Enemy e)
        {
            this.e = e;
            this.canTraverse = false;


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
