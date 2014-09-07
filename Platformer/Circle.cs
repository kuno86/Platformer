using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Circle : Geometry
    {
        private double radius;
        private Point pos;

        public Circle(Point position, double radius)
        {
            this.pos.x = position.x;
            this.pos.y = position.y;
            this.radius = radius;
        }

        public double fläche(double r)
        {
            return Math.PI * (r * r);
        }
        
        public double umfang(double r)
        {
            return Math.PI * 2 * r;
        }

        public bool collision(Point pExt)
        {
            double dis = distance(this.pos, pExt);
            if(dis<=this.radius)
                return true;
            return false;
        }
    }
}
