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

        //Content Manager?
        public Enemy(Spot currentLocation, int cookies)
        {

            this.currentLocation = currentLocation;
            this.moveIndex = 0;
            this.x = currentLocation.x;
            this.y = currentLocation.y;

        }

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("gravel.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(x, y, 48, 50), Color.White);
        }

        public void Update(Mower mower, Controls controls, List<Spot> patches, GameTime gameTime)
        {
            Move(mower, patches);
        }

        public void Move(Mower mower, List<Spot> patches)
        {
            // for the beta
        }

    }
}
