using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Lakitu : BaseObj
    {
        public int hbW = 16;
        public int hbH = 16;
        private double accel = 0.1;
        private double velMax = 1;
        private double xVel, yVel;
        private short frame = 0;
        private int throwDelay;
        private int throwObjectId;
        
        public bool isDead = false; //does not respawn after it was killed   

        public Lakitu(double x, double y, bool dir = false, int throwObjectId = 14)
            : base(x, y, 14, 15)
        {
            this.name = "Lakitu";
            this.texture = Texture.smb1_lakitu;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colOffsetX = 1;
            this.colOffsetY = 9;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = 14;
            this.colRect.h = 15;
            this.dir = dir; //Startdirection: true = Left ; false = Right
            this.colWithBlocks = true;
            this.colWithOthers = true;
            this.throwDelay = Map.rnd.Next(60, 120);
            this.throwObjectId = throwObjectId;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            throwDelay--;
            if (throwDelay == 45)
                frame = 1;
            if (throwDelay == 0)
            {
                frame = 0;
                throwDelay = Map.rnd.Next(60, 120);
                int tmpId=Map.spriteAdd(DeepCopySprite(this.throwObjectId));
                Map.spriteArray[tmpId].setXY(x, y - Map.spriteArray[tmpId].h - 2);
                Map.spriteArray[tmpId].setXYVel(4, -2);

            }

            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null && Map.spriteArray[i].name == "Player")
                {
                    if ((Map.spriteArray[i].x < x))  //is right from player an d looks away
                    {
                        dir = true;
                        if (xVel > -velMax)
                            xVel -= accel;
                        else
                            xVel = -velMax;

                        if (64 > y)
                            y += 0.16;
                        if (64 < y + h)
                            y -= 0.16;
                    }
                    else if ((Map.spriteArray[i].x > x))  //is left from player an d looks away
                    {
                        dir = false;
                        if (xVel < velMax)
                            xVel += accel;
                        else
                            xVel = velMax;

                        if (64 > y)
                            y += 0.16;
                        if (64 < y)
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
            Image.drawTileFrame(texture, frame, 2, x, y, dir);
        }


    }
}

