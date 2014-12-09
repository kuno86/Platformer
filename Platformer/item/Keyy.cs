﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Game
{
    class Keyy : BaseObj
    {
        public bool onGround, falling;

        public Keyy(double x, double y, short type = 2, bool dir=false)
            : base(x, y, 16, 16)
        {
            this.name = "Key";
            switch (type)
            {
                case 1: this.texture = Texture.smw_key; break;
                case 2: this.texture = Texture.smb2_key; break;
                default: this.texture = Texture.smb2_key; break;
            }
            this.blockTop = true;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.dir=dir;
            this.type = type;
            this.grabable = true;
            this.colWithBlocks = true;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();

            if (getColXY((int)x + (w / 2), (int)y + h + 1) == 1)    //floorCol
            {
                while (colBottom == 1)
                {
                    y--;
                    refreshColRect();
                    getColGrid();
                }
                yVel = 0;
                onGround = true;
                falling = false;
            }
            else
            {
                yVel += Map.gravity;
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

            y += yVel;
            x += xVel;
        }

        public override void doRender()
        {
            MyImage.drawImage(texture, x, y, dir);
            if (RootThingy.debugInfo)
            {
                if (grabable)
                    MyImage.drawText("G1", (int)(x), (int)(y - 12), Color.White, Texture.ASCII);
                else
                    MyImage.drawText("G0", (int)(x), (int)(y - 12), Color.White, Texture.ASCII);
            }
        }
    }
}


