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
    class Cookie : Sprite
    {
        public int fuel;
        public String type;
        public int x, y;
        public Spot currentLocation;

        public Cookie()
        {
            fuel = 5;
            type = "chocolate chip";
        }

        public Cookie(int x, int y)
        {
            fuel = 5; //positive fuel gain
            type = "chocolate chip";
            this.x = x;
            this.y = y;
        }
        public Cookie(Spot s)
        {
            fuel = 5; //positive fuel gain
            type = "chocolate chip";
            this.x = s.x;
            this.y = s.y;
            this.currentLocation = s;
        }

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("chocolate chip.png");
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(x, y, 50, 50), Color.White);
        }

        public void Update()
        {

        }
        public void setX(int x){
            this.x = x;
        }
        public void setY(int y){
            this.y = y;
        }
        public void setType(String s)
        {
            this.type = s; 
        }
        public void setFuel(int f)
        {
            this.fuel = f;
        }
        public void setSpot(Spot s)
        {
            this.x = s.x;
            this.y = s.y;
            this.currentLocation = s;
        }
    }
}
