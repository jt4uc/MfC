using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;

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
        public SoundEffect munch;
        public SoundEffectInstance munchInstance;
        public bool munchLimit = true;

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
                image = content.Load<Texture2D>("chocolatecookie.png");
            }
            else if (this.type.Equals("bombcookie"))
            {
                image = content.Load<Texture2D>("bombcookie.png");
            }
            else if (this.type.Equals("healthcookie"))
            {
                image = content.Load<Texture2D>("heartcookie.png");
            }
            munch = content.Load<SoundEffect>("bite");
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(x, y, recX, recY), Color.White);
        }

        public void Update(Mower m, Grandma g, int ticks, Spot[,] patches)
        {
            if (m.x == this.x && m.y == this.y && this.alive == true) { 
                if (type.Equals("chocolatechip"))
                {
                    tickCount = ticks;
                    g.SPEED = 3;
                    this.alive = false;
                    if (munchLimit == true)
                    {
                        munchInstance = munch.CreateInstance();
                        munchInstance.IsLooped = false;
                        munchInstance.Play();
                        munchLimit = false;
                    }
                }
                if (type.Equals("bombcookie"))
                {
                    //top
                    if (patches[this.currentLocation.arrayRowX, this.currentLocation.arrayColY - 1].canTraverse && patches[this.currentLocation.arrayRowX, this.currentLocation.arrayColY - 1].isTraversed == false)
                    {
                        patches[this.currentLocation.arrayRowX, this.currentLocation.arrayColY - 1].isTraversed = true;
                        m.totalMowed++;
                    }
                    
                    //top-right
                    if (patches[this.currentLocation.arrayRowX + 1, this.currentLocation.arrayColY - 1].canTraverse && patches[this.currentLocation.arrayRowX + 1, this.currentLocation.arrayColY - 1].isTraversed == false)
                    {
                        patches[this.currentLocation.arrayRowX + 1, this.currentLocation.arrayColY - 1].isTraversed = true;
                        m.totalMowed++;
                    }

                    //right
                    if (patches[this.currentLocation.arrayRowX + 1, this.currentLocation.arrayColY].canTraverse && patches[this.currentLocation.arrayRowX + 1, this.currentLocation.arrayColY].isTraversed == false)
                    {
                        patches[this.currentLocation.arrayRowX + 1, this.currentLocation.arrayColY].isTraversed = true;
                        m.totalMowed++;
                    }
                    
                    //bottom-right
                    if (patches[this.currentLocation.arrayRowX + 1, this.currentLocation.arrayColY + 1].canTraverse && patches[this.currentLocation.arrayRowX + 1, this.currentLocation.arrayColY + 1].isTraversed == false)
                    {
                        patches[this.currentLocation.arrayRowX + 1, this.currentLocation.arrayColY + 1].isTraversed = true;
                        m.totalMowed++;
                    }
                    
                    //bottom
                    if (patches[this.currentLocation.arrayRowX, this.currentLocation.arrayColY + 1].canTraverse && patches[this.currentLocation.arrayRowX, this.currentLocation.arrayColY + 1].isTraversed == false)
                    {
                        patches[this.currentLocation.arrayRowX, this.currentLocation.arrayColY + 1].isTraversed = true;
                        m.totalMowed++;
                    }
                    //bottom-left
                    if (patches[this.currentLocation.arrayRowX - 1, this.currentLocation.arrayColY + 1].canTraverse && patches[this.currentLocation.arrayRowX - 1, this.currentLocation.arrayColY + 1].isTraversed == false)
                    {
                        patches[this.currentLocation.arrayRowX - 1, this.currentLocation.arrayColY + 1].isTraversed = true;
                        m.totalMowed++;
                    }
                    //left
                    if (patches[this.currentLocation.arrayRowX - 1, this.currentLocation.arrayColY].canTraverse && patches[this.currentLocation.arrayRowX - 1, this.currentLocation.arrayColY].isTraversed == false)
                    {
                        patches[this.currentLocation.arrayRowX - 1, this.currentLocation.arrayColY].isTraversed = true;
                        m.totalMowed++;
                    }
                    //top-left
                    if (patches[this.currentLocation.arrayRowX - 1, this.currentLocation.arrayColY - 1].canTraverse && patches[this.currentLocation.arrayRowX - 1, this.currentLocation.arrayColY - 1].isTraversed == false)
                    {
                        patches[this.currentLocation.arrayRowX - 1, this.currentLocation.arrayColY - 1].isTraversed = true;
                        m.totalMowed++;
                    }
                    m.totalMowed--;
                    this.alive = false;
                    if (munchLimit == true)
                    {
                        munchInstance = munch.CreateInstance();
                        munchInstance.IsLooped = false;
                        munchInstance.Play();
                        munchLimit = false;
                    }
                }
                if (type.Equals("healthcookie"))
                {
                    m.cookies += 5;
                    this.alive = false;
                    if (munchLimit == true)
                    {
                        munchInstance = munch.CreateInstance();
                        munchInstance.IsLooped = false;
                        munchInstance.Play();
                        munchLimit = false;
                    }
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
