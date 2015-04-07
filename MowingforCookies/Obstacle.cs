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
                image = content.Load<Texture2D>("gravel.png");
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
            sb.Draw(image, new Rectangle(x, y, recX, recY), Color.White);
        }
        public void collidesEnemy(Enemy e)
        {
            if (this.cbox.Intersects(e.cbox))
            {
                //tada
                //look up how to remove things from board
            }
        }
        public void changeBox(Rectangle lol)
        {
            if (exploding == true)
            {
                this.cbox = lol;
            }
            else
            {
                this.cbox = this.backupCbox;
            }
        }


        public void Update()
        {
            //mower steps on spot. spot updates its obstacle object.
            //this is coming from spot or something, ideally. 

            //get spots 
            //boundary checking is done elsewhere
           
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
