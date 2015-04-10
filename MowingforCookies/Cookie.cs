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
        public int arrayRowX, arrayColY;
        public int recX = 50;
        public int recY = 50;

        public Cookie(Spot s, int arrayRowX, int arrayColY)
        {
            fuel = 5; //positive fuel gain
            type = "chocolatechip";
            this.x = s.x;
            this.y = s.y;
            this.currentLocation = s;
            this.arrayColY = arrayColY;
            this.arrayRowX = arrayRowX;
        }

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("chocolatechip.png");
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(x, y, recX, recY), Color.White);
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
