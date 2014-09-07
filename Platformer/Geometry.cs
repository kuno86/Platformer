using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Geometry
     {
        public struct Point
        {
            public double x;
            public double y;
        }
        
        public struct Rect
        {
            public double x;
            public double y;
            public double w;
            public double h;
        }

        public Point rotate(double x, double y, double angle)   //Rotate a xy-coordinate to the specified angle
        {
            angle = angle * 0.0174532925;
            Point rotated = new Point();
            rotated.x = x * Math.Cos(angle) + y * Math.Sin(angle);
            rotated.y = y * Math.Cos(angle) - x * Math.Sin(angle);
            return rotated;
        }

        public Point rotate(Point p, double angle)  //Rotate a point to the specified angle
        {
            angle = angle * 0.0174532925;
            Point rotated = new Point();
            rotated.x = p.x * Math.Cos(angle) + p.y * Math.Sin(angle);
            rotated.y = p.y * Math.Cos(angle) - p.x * Math.Sin(angle);
            return rotated;
        }

        public double distance(double x1, double y1, double x2, double y2)  //distance between xy1 and xy2
        {
            double x = Math.Abs(x1 - x2);
            double y = Math.Abs(y1 - y2);
            return Math.Sqrt((x * x) + (y * y));
        }

        public double distance(Point p1, Point p2)  //distance between 2 points
        {
            double x = Math.Abs(p1.x - p2.x);
            double y = Math.Abs(p1.x - p2.y);
            return Math.Sqrt((x * x) + (y * y));
        }

        public Point center(Point[] edges)  //get the center-point of an array of points
        {
            double xGes=0;
            double yGes=0;
            for(int i=0;i!=edges.Length-1;i++)
            {
                xGes+=edges[i].x;
                yGes+=edges[i].y;
            }
            Point center =new Point();
            center.x=xGes/edges.Length;
            center.y=yGes/edges.Length;
            return center;
        }
    }
}
