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

        //obstacleTexture: Texture2D
        //obstacleTextureMap: Animated Sprite

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
            sb.Draw(image, new Rectangle(x, y, 40, 40), Color.White);
        }


        public void Update()
        {

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
