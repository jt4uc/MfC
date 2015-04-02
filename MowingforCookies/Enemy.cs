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

        //Content Manager?
        public Enemy(Spot currentLocation, int cookies, int arrayRowX, int arrayColY)
        {

            this.currentLocation = currentLocation;
            this.moveIndex = 0;
            this.x = currentLocation.x;
            this.y = currentLocation.y;
            this.type = "gnome";
            this.arrayColY = arrayColY;
            this.arrayRowX = arrayRowX;

        }

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("gnome.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(x, y, 48, 50), Color.White);
        }

        public void Update(Mower mower, Controls controls, Spot[,] patches, GameTime gameTime)
        {
            Move(mower, patches);
        }

        public void setType(String s)
        {
            this.type = s;
        }

        public void Move(Mower mower, Spot[,] patches)
        {
            // for the beta
        }

    }
}
