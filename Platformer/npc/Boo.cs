using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Boo : BaseObj
    {
        private double angle, adder, angleDeg;
        public int hbW = 16;
        public int hbH = 16;
        private double accel = 0.0395;
        private double velMax = 0.5;
        private double xVel, yVel;
        private short frame=0;
        
        public bool isDead = false; //does not respawn after it was killed   

        public Boo(double x, double y, bool dir=false)
            : base(x, y,16,16)
        {
            this.name = "Boo";
            this.texture = Texture.smw_boo;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.dir = dir; //Startdirection: true = Left ; false = Right
            this.colWithBlocks = false;
            this.colWithOthers = true;
            this.adder = 0.03;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            frame = 0;
            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null && Map.spriteArray[i].name == "Player")
                {
                    if ((Map.spriteArray[i].x < x) && (Map.spriteArray[i].dir))  //is right from player and looks away
                    {

                        dir = true;
                        frame = 1;
                        if (xVel > -velMax)
                            xVel -= accel;
                        else
                            xVel = -velMax;

                        if (Map.spriteArray[i].y > y)
                            y += 0.16;
                        if (Map.spriteArray[i].y < y + h)
                            y -= 0.16;

                    }
                    else if ((Map.spriteArray[i].x > x) && (!Map.spriteArray[i].dir))  //is left from player and looks away
                    {


                        dir = false;
                        frame = 1;
                        if (xVel < velMax)
                            xVel += accel;
                        else
                            xVel = velMax;

                        if (Map.spriteArray[i].y > y)
                            y += 0.16;
                        if (Map.spriteArray[i].y < y)
                            y -= 0.16;

                    }
                    else
                    {
                        if (xVel > 0)
                            xVel -= accel;
                        if (xVel < 0)
                            xVel += accel;
                        if ((xVel < 0 && xVel > -accel) || (xVel > 0 && xVel < accel))    //
                        { xVel = 0; }
                    }
                }
            }
            x += xVel;
        }

        public override void doRender()
        {
            MyImage.drawTileFrame(texture, frame, 2, x, y, dir);
        }

    }
}
