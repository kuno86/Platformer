using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Game
{
    class BaseObj
    {
        public struct Point
        {
            public double x;
            public double y;
        }                                                           /////////////////////////////////////////
                                                                    //put refreshColRect() in every process()
        public struct ColRect                                       /////////////////////////////////////////
        {
            public double x;
            public double y;
            public int w;
            public int h;
        }
        public short colOffsetX;
        public short colOffsetY;

        protected int colTop, colBottom, colRight, colLeft;
        public ColRect colRect;

        public bool grabable = false;

        public bool despawnOffScreen = true;

        public static int idCount=0;
        public int id;

        public short type;
        public string name = "Base";
        public bool colWithOthers = false;
        public int texture;
        public bool dir;    //false=right ; true=left
        public double x;
        public double y;
        public short w;
        public short h;
        public double xVel;
        public double yVel;

        public BaseObj(double x, double y, short w, short h, short colW=16, short colH=16, short colOffsetX=0, short colOffsetY=0)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.colOffsetX = colOffsetX;
            this.colOffsetY = colOffsetY;
            colRect.x = x + colOffsetX;
            colRect.y = y + colOffsetY;
            colRect.w = colW;
            colRect.h = colH;
            id = idCount;
            idCount++;
        }

        public BaseObj(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
      
        public virtual string getName()
        {return name;}

        public virtual void process()
        {;}

        public virtual void setXY(double x, double y)
        { 
            this.x = x; 
            this.y = y; 
        }

        public void setXYVel(double xVel, double yVel)
        {
            this.xVel = xVel;
            this.yVel = yVel;
        }

        ///////////////////////////////////////////Return Collision Value for a Block on Map
        protected void getColGrid()
        {
            colTop = 0;
            colBottom = 0;
            colRight = 0;
            colLeft = 0;
            if (despawnOffScreen)
            {
                try
                {
                    if (Map.map[(int)(colRect.y / 16), (int)(colRect.x / 16), 0] == 1 || Map.map[(int)(colRect.y / 16), (int)((colRect.x + Map.tileSize) / 16), 0] == 1)  //ceiling-Collisions
                        colTop = 1;           //

                    if (Map.map[(int)((colRect.y + colRect.h) / 16), (int)(colRect.x / 16), 0] == 1 || Map.map[(int)((colRect.y + colRect.h) / 16), (int)((colRect.x + Map.tileSize) / 16), 0] == 1 || //Floor-Collisions (Solids-check)
                        Map.map[(int)((colRect.y + colRect.h) / 16), (int)(colRect.x / 16), 0] == 2 || Map.map[(int)((colRect.y + colRect.h) / 16), (int)((colRect.x + Map.tileSize) / 16), 0] == 2)  //Floor-Collisions (clouds-check)
                        colBottom = 1;

                    if (Map.map[(int)(colRect.y / 16), (int)((colRect.x + colRect.w) / 16), 0] == 1 || Map.map[(int)((colRect.y + Map.tileSize) / 16), (int)((colRect.x + colRect.w) / 16), 0] == 1)  //Right Walls Collsion
                        colRight = 1;

                    if (Map.map[(int)(colRect.y / 16), (int)(colRect.x / 16), 0] == 1 || Map.map[(int)((colRect.y + Map.tileSize) / 16), (int)(colRect.x / 16), 0] == 1)  //Left Wall Collision
                        colLeft = 1;
                }
                catch { ; }
            }
        }

        ///////////////////////////////////////////Return Collision Value for a Pixel on Map
        protected int getColXY(int x, int y)
        {
            if ((y < Map.map.GetLength(0)*Map.tileSize) && (x < Map.map.GetLength(1)*Map.tileSize) && (x >= 0) && (y >= 0))
                return Map.map[(int)y / 16, (int)x / 16, 0];
            else
                return 1;
        }


        ///////////////////////////////////////////Check if two BaseObj's collide with each other
        protected bool getCol2Obj(ColRect obj1, ColRect obj2)
        {
            if ((obj1.x + obj1.w > obj2.x && obj1.x < obj2.x + obj2.w) && (obj1.y + obj1.h > obj2.y && obj1.y < obj2.y + obj2.h))
                return true;
            else
                return false;
        }

        protected int toInt(double z)
        {return Convert.ToInt32(z); }

        protected void refreshColRect()
        {
            colRect.x = x + colOffsetX;
            colRect.y = y + colOffsetY;
        }

       

    }
}
