using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Geometry : BaseObj
     {
        public static Point rotate(double x, double y, double angle)   //Rotate a xy-coordinate to the specified angle
        {
            angle = angle * 0.0174532925;
            Point rotated = new Point();
            rotated.x = x * Math.Cos(angle) + y * Math.Sin(angle);
            rotated.y = y * Math.Cos(angle) - x * Math.Sin(angle);
            return rotated;
        }

        public static Point rotate(Point p, double angle)  //Rotate a point to the specified angle
        {
            angle = angle * 0.0174532925;
            Point rotated = new Point();
            rotated.x = p.x * Math.Cos(angle) + p.y * Math.Sin(angle);
            rotated.y = p.y * Math.Cos(angle) - p.x * Math.Sin(angle);
            return rotated;
        }

        public static double distance(double x1, double y1, double x2, double y2)  //distance between xy1 and xy2
        {
            double x = Math.Abs(x1 - x2);
            double y = Math.Abs(y1 - y2);
            return Math.Sqrt((x * x) + (y * y));
        }

        public static double distance(Point p1, Point p2)  //distance between 2 points
        {
            double x = Math.Abs(p1.x - p2.x);
            double y = Math.Abs(p1.x - p2.y);
            Console.WriteLine(Math.Sqrt((x * x) + (y * y)));
            return Math.Sqrt((x * x) + (y * y));
        }

        public static Point center(Point[] edges)  //get the center-point of an array of points
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
