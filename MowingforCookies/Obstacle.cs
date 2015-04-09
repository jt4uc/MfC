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
        public int recX = 40;
        public int recY = 40;
        public Boolean exploding;

        public Obstacle(Spot currentLocation, String obstacleType, int arrayRowX, int arrayColY)
        {
            this.currentLocation = currentLocation;
            this.x = this.currentLocation.x;
            this.y = this.currentLocation.y;
            this.obstacleType = obstacleType;
            this.exploding = false;

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

        }

        public void Draw(SpriteBatch sb)
        {
            if (exploding == true)
            {
                Console.WriteLine(cbox.X);
                sb.Draw(image, cbox, Color.White);//lmao. only part of the image. weird.
            }
            else
            {
                sb.Draw(image, new Rectangle(x, y, recX, recY), Color.White);
            }
        }
        public void collidesEnemy(Enemy e)
        {
            if (this.cbox.Intersects(e.cbox))
            {
                e.alive = false;
            }
        }
        public void changeBox(Rectangle lol)
        {
            if (exploding)
            {
                this.cbox = lol;
                image = boom;
              
            }
            else
            {
                
                this.cbox = this.backupCbox;
                image =  gravel;
            }
        }


        public void Update(Rectangle r, Mower mower, Enemy e)
        {
            if (mower.x == this.x && mower.y == this.y)
            {
                exploding = true;
                changeBox(r);
            }
            else
            {
                exploding = false;
                changeBox(r);
            }
            if (e != null)
            {
                collidesEnemy(e);
            }

           
        }



        public void setSpot(Spot s)
        {
            this.currentLocation = s;
            this.x = s.x;
            this.y = s.y;
        }

        public Spot getSpot()
        {
            return this.currentLocation;
        }


    }
}
