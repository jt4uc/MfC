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


        public Cookie(int x, int y)
        {
            fuel = 5; //positive fuel gain
            type = "chocolate chip";
            this.x = x;
            this.y = y;
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

    }
}
