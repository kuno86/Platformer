﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Input;
using System.Threading;

namespace Game
{
    class Spring_p : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        public bool active;
        public bool onGround, falling;
        public double springVel;
        private ColRect cRect;

        public Spring_p(double x, double y)
            : base(x, y, 16, 16)
        {
            this.name = "Spring_p"; 
            this.texture = Texture.smw_spring_p; 
            this.frames = 3;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            colOffsetX = 0;
            colOffsetY = 8;
            colRect.w = 16;
            colRect.h = 8;
            this.springVel = -4.5;
            cRect.x = x;
            cRect.y = y;
            cRect.w = w;
            cRect.h = h;
            this.type = type;
            grabable = true;
            active = false;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();

            cRect.x = x;
            cRect.y = y;
            cRect.w = w;
            cRect.h = h;

            getColGrid();

            if (getColXY((int)x + (w / 2), (int)y + h + 1) == 1)    //floorCol
            {
                if (colBottom == 1)
                    y--;
                onGround = true;
                falling = false;
            }
            else
            {
                y++;
                onGround = false;
                falling = true;
            }

            if (getColXY((int)x + (w / 2), (int)y - 1) == 1)
            {
                if (colTop == 1)
                    y++;
                onGround = false;
                falling = true;
            }

            if (getColXY((int)x - 1, (int)y + (h / 2)) == 1)    //RightCol
            {
                xVel = 0;
                falling = true;
            }
            if (getColXY((int)x + w + 1, (int)y + (h / 2)) == 1)    //LeftCol
            {
                xVel = 0;
                falling = true;
            }
            x += xVel;
            frame = 0;

            var keyboard = Keyboard.GetState();
            //Thread.Sleep(30);
            for (int i = 0; i != Map.spriteList.Count(); i++)
            {
                if (Map.spriteList[i].name == "Player")
                {
                    if (getCol2Obj(Map.spriteList[i].colRect, cRect))
                    {

                        Image.drawText((Map.spriteList[i].colRect.y + Map.spriteList[i].colRect.h - y).ToString(), (int)x + w + 3, (int)y, Color.Red, Texture.ASCII);

                        if (Map.spriteList[i].colRect.y + Map.spriteList[i].colRect.h - y >= 4)
                            frame = 1;
                        else if (Map.spriteList[i].colRect.y + Map.spriteList[i].colRect.h - y >= 8)
                        {
                            frame = 2;
                            if(keyboard[Key.Z] || keyboard[Key.Y])
                                Map.spriteList[i].setXYVel(Map.spriteList[i].xVel, springVel - 3);
                            else
                                Map.spriteList[i].setXYVel(Map.spriteList[i].xVel, springVel);
                        }
                    }
                }
            }

            Image.drawTileFrame(texture, frame, frames, x, y);
        }


    }
}

