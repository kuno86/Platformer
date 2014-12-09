using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Game
{
    class Fireflower : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        public bool fixd, falling;

        public Fireflower(double x, double y, short type = 1, bool fixd = true)
            : base(x, y, 16, 16)
        {
            this.name = "Fireflower";
            switch (type)
            {
                case 1: this.texture = Texture.smb1_fireflower; frames = 4; break;
                case 2: this.texture = Texture.smb3_fireflower; frames = 2; break;
                case 3: this.texture = Texture.smw_fireflower; frames = 2; break;
                default: this.texture = Texture.smb1_fireflower; frames = 4; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.fixd = fixd;
            this.type = type;
            this.colWithBlocks = true;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();
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
                if (onGround)
                {
                    if (xVel > xDecel)
                        xVel -= xDecel;
                    if (xVel < xDecel)
                        xVel += xDecel;
                    if (xVel <= xDecel && xVel >= xDecel * -1)
                        xVel = 0;
                }
            }
            else
            {
                yVel += Map.gravity;
                onGround = false;
                falling = true;
            }

            y = y + yVel;
        }

        public override void doRender()
        {
            MyImage.drawTileFrame(texture, frame, frames, x, y);
        }

    }
}

