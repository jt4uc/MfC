using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

//code source: http://rbwhitaker.wikidot.com/circle-collision-detection
//used to create a circle class for gravel. maybe

namespace MowingforCookies
{
    struct Circle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }


        //checks to see whether a point is inside the Circle or not
        public bool Contains(Vector2 point)
        {
            Vector2 relativePosition = point - Center;
            float distanceBetweenPoints = relativePosition.Length();
            if (distanceBetweenPoints <= Radius) { return true; }
            else { return false; }
        }

        public bool Intersects(Circle other)
        {
            Vector2 relativePosition = other.Center - this.Center;
            float distanceBetweenCenters = relativePosition.Length();
            if (distanceBetweenCenters <= this.Radius + other.Radius) { return true; }
            else { return false; }
        }

        //my test code for seeing if a rectable will intersect with a circle
        //may have to write a similar code for rectangle.intersects(circle). somehow.s
        public bool Intersects(Rectangle other)
        {
            //THIS REQUIRES KNOWING WHERE RECTANGLE'S "CENTER" IS. 
            /*
             * Rectangle's properties: https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.rectangle_properties.aspx
             * */
            //ugh. this is going to be ugly.s
            int rLeftX = other.Left;
            int rRightX = other.Right;
            int rTopY = other.Top;
            int rBottomY = other.Bottom;

            int cLeftX = (int)this.Center.X; //minus? plus?

            //omfg. this is riduculous.

            //8 directions for the circle, because gravel spews out in a 8 thing
            

            return true;
        }
    }
}
