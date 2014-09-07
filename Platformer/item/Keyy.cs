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
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.dir=dir;
            this.type = type;
            grabable = true;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            getColGrid();

            if (getColXY((int)x + (w / 2), (int)y + h + 1) == 1)    //floorCol
            {
                if (colBottom == 1)
                    y--;
                onGround = true;
                falling = false;
            }

            if (getColXY((int)x + (w / 2), (int)y - 1) == 1)
            {
                if (colTop == 1)
                    y++;
                onGround = false;
                falling = true;
            }
            else
            {
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

           

            Image.drawImage(texture, x, y, dir);

        }

    }


}


