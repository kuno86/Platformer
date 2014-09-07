﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Game
{
    class Bulletbill : BaseObj
    {
        private short frames;
        private short frame = 0;
        private short frameDelay = 0;
        private bool flipV;
        private bool homing;
        private int HomingRange = 64;

        public Bulletbill(double x, double y, bool dir=false, short type = 1, bool homing = false)
            : base(x, y)
        {
            if(homing)
                this.name = "Bulletbill_homing";
            else
                this.name = "Bulletbill";
            switch (type)
            {
                case 1: this.texture = Texture.smb1_bulletbill; frames = 3; homing = false; break;
                case 2: this.texture = Texture.smb3_bulletbill_1; frames = 2; homing = false; break;
                case 3: this.texture = Texture.smb3_bulletbill_2; frames = 2; homing = true; break;
                case 4: this.texture = Texture.smw_bulletbill; frames = 2; homing = false; break;
                default: this.texture = Texture.smb1_bulletbill; frames = 3; homing = false; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.dir = dir;
            this.homing = homing;
            this.type = type;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();

            if (!homing)
            {
                if (dir)
                    xVel = -1.25;
                else
                    xVel = 1.25;
            }
            else
            {
                if (xVel > 0)
                    dir = false;
                if (xVel < 0)
                    dir = true;
            }

            for (int i = 0; i != Map.spriteList.Count(); i++)
            {
                if (Map.spriteList[i].name == "Player")
                {
                    if (homing)
                    {
                        if (Map.spriteList[i].colRect.y > y)
                            y++;
                        if (Map.spriteList[i].colRect.y < y)
                            y--;
                        if (Map.spriteList[i].colRect.x + Map.spriteList[i].colRect.w + HomingRange > x)
                        { xVel += 0.2; } 
                        if (Map.spriteList[i].colRect.x - HomingRange < x + w)
                        { xVel -= 0.2; }
                    }
                    if (getCol2Obj(this.colRect, Map.spriteList[i].colRect))
                        ;//hurt or get hurt
                }
            }

            frameDelay++;
            if (frameDelay == 3)
            {
                frame++;
                frameDelay = 0;
            }
            if (frame > frames)
            {
                frame = 1;
            }
            x += xVel;
            Image.drawTileFrame(texture, frame, frames, x, y, dir);

        }




    }
}
