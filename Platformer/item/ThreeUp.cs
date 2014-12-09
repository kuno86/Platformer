using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Game
{
    class ThreeUp : BaseObj
    {
        public bool onGround, falling,fixd;

        public ThreeUp(double x, double y, bool fixd=false, short type = 0)
            : base(x, y, 16, 16)
        {
            this.name = "ThreeUp";
            switch (type)
            {
                case 0: this.texture = Texture.smw_3up; break;
                default: this.texture = Texture.smw_3up; break;
            }
            this.fixd = fixd;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.colWithBlocks = true;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();
            if (!fixd)
            {
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

                y = y + yVel;
                x += xVel;
            }
        }

        public override void doRender()
        {
            MyImage.drawImage(texture, x, y);
        }

    }
}
