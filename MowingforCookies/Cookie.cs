﻿using System;
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
        public int tickCount = 9999999;
        public Boolean alive = true;

        public Cookie(Spot s, String type, int arrayRowX, int arrayColY)
        {
            fuel = 5; //positive fuel gain
            this.type = type;
            this.x = s.x;
            this.y = s.y;
            this.currentLocation = s;
            this.arrayColY = arrayColY;
            this.arrayRowX = arrayRowX;
            
        }

        public void LoadContent(ContentManager content)
        {
            if (this.type.Equals("chocolatechip")){
                image = content.Load<Texture2D>("chocolatechip.png");
            }
            else if (this.type.Equals("bombcookie"))
            {
                image = content.Load<Texture2D>("water.png");
            }
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(x, y, recX, recY), Color.White);
        }

        public void Update(Mower m, Grandma g, int ticks)
        {
            if (m.x == this.x && m.y == this.y && this.alive == true) { 
                if (type.Equals("chocolatechip"))
                {
                    tickCount = ticks;
                    g.SPEED = 3;
                    this.alive = false;
                }
                if (type.Equals("bombcookie"))
                {
                    this.currentLocation.isTraversed = true;
                }
                if (type.Equals("healthcookie"))
                {
                    m.cookies += 5;
                }
            }
            else
            {
                if (ticks > (tickCount + 200))
                {
                    g.SPEED = 1;
                }
            }
        }
    }
}
