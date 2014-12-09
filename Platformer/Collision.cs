using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Collision : Geometry
    {
        public static bool PointsArrayCollision(Point[] points, ColRect obst,bool fixIt)
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

        public static bool col_circlePoint(double cx, double cy, double r, Point pExt)
        {
            Point pC = new Point();
            pC.x = cx;
            pC.y = cy;

            double x = Math.Abs(pC.x - pExt.x);
            double y = Math.Abs(pC.x - pExt.y);
            double dis = ((x * x) + (y * y));
            //Console.WriteLine("Distance: " + dis);
            if (dis * dis <= r)
                return true;
            return false;
        }

        public static bool col_circlePoint(double cx, double cy, double r, double px, double py)
        {
            Point pC = new Point();
            pC.x = cx;
            pC.y = cy;
            Point pE = new Point();
            pE.x = px;
            pE.y = py;

            double x = Math.Abs(pC.x - pE.x);
            double y = Math.Abs(pC.x - pE.y);
            double dis =((x * x) + (y * y));
            //Console.WriteLine("Distance: " + dis);
            if (dis*dis <= r)
                return true;
            return false;
        }


        public static bool col_circleRect(double circleX, double circleY, double circleR, double rectX, double rectY, double rectW, double rectH)
        {
            Point circleDistance = new Point();
            circleDistance.x = Math.Abs(circleX - rectX);
            circleDistance.y = Math.Abs(circleY - rectY);

            if (circleDistance.x > (rectW / 2 + circleR)) { return false; }
            if (circleDistance.y > (rectH / 2 + circleR)) { return false; }

            if (circleDistance.x <= (rectW / 2)) { return true; }
            if (circleDistance.y <= (rectH / 2)) { return true; }

            double cornerDistance_sq = (circleDistance.x - rectW / 2) * (circleDistance.x - rectW / 2) +
                                        (circleDistance.y - rectH / 2) * (circleDistance.y - rectH / 2);

            return (cornerDistance_sq <= (circleR * circleR));
        }


        public static bool col_circleRect(double circleX, double circleY, double circleR, BaseObj.ColRect rect)
        {
            Point circleDistance = new Point();
            circleDistance.x = Math.Abs(circleX - rect.x);
            circleDistance.y = Math.Abs(circleY - rect.y);

            if (circleDistance.x > (rect.w / 2 + circleR)) { return false; }
            if (circleDistance.y > (rect.h / 2 + circleR)) { return false; }

            if (circleDistance.x <= (rect.w / 2)) { return true; }
            if (circleDistance.y <= (rect.h / 2)) { return true; }

            double cornerDistance_sq = (circleDistance.x - rect.w / 2) * (circleDistance.x - rect.w / 2) +
                                        (circleDistance.y - rect.h / 2) * (circleDistance.y - rect.h / 2);

            return (cornerDistance_sq <= (circleR * circleR));
        }
        
//=================================================================
    }
}
