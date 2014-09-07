using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Collision : Geometry
    {
        public bool PointsArrayCollision(Point[] points, Rect obst,bool fixIt)
        {
            bool col = false;
            for (int i = 0; i != points.Length; i++)
            {
                //========================================================================================Collision from above (-Y)
                if (points[i].y >= obst.y && (points[i].x <= obst.x + obst.w && points[i].x >= obst.x))
                {
                    col = true;
                    if (fixIt)
                    {
                        double pushBack = Math.Abs(points[i].y - obst.y + obst.h);
                        points[i].y -= pushBack;
                        for (int j = 0; j != points.Length; j++)
                        { points[j].y -= pushBack; }
                    }
                }
                //========================================================================================Collision from beneath (+Y)
                if (points[i].y<= obst.y + obst.h && (points[i].x <= obst.x + obst.w && points[i].x >= obst.x))
                {
                    col = true;
                    if (fixIt)
                    {
                        double pushBack = Math.Abs(points[i].y - obst.y);
                        points[i].y += pushBack;
                        for (int j = 0; j != points.Length; j++)
                        { points[j].y += pushBack; }
                    }
                }
                //========================================================================================Collision from left (-X)
                if (points[i].x <= obst.x + obst.w && (points[i].y <= obst.y + obst.h && points[i].y >= obst.y))
                {
                    col = true;
                    if (fixIt)
                    {
                        double pushBack = Math.Abs(points[i].x - obst.x + obst.w);
                        points[i].y -= pushBack;
                        for (int j = 0; j != points.Length; j++)
                        { points[j].y -= pushBack; }
                    }
                }
                //========================================================================================Collision from right (-X)
                if (points[i].y >= obst.x && (points[i].y <= obst.y + obst.h && points[i].y >= obst.y))
                {
                    col = true;
                    if (fixIt)
                    {
                        double pushBack = Math.Abs(points[i].x - obst.x);
                        points[i].y += pushBack;
                        for (int j = 0; j != points.Length; j++)
                        { points[j].y += pushBack; }
                    }
                }
            }
            return col;
        }

        public bool Circlecollision(double cx, double cy, double r,Point pExt)
        {
            Point circle = new Point();
            circle.x = cx;
            circle.y = cy;
            double dis = distance(circle, pExt);
            if (dis <= r)
                return true;
            return false;
        }

        public bool Circlecollision(double cx, double cy, double r, double px, double py)
        {
            Point circle = new Point();
            circle.x = cx;
            circle.y = cy;
            Point pExt = new Point();
            pExt.x = px;
            pExt.y = py;
            double dis = distance(circle, pExt);
            Console.WriteLine("Distance: " + dis);
            if (dis <= r)
                return true;
            return false;
        }

        
        
//=================================================================
    }
}
