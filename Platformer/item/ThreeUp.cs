using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Game
{
    class ThreeUp : BaseObj
    {
        public bool onGround, falling;

        public ThreeUp(double x, double y, short type = 0)
            : base(x, y, 16, 16)
        {
            this.name = "ThreeUp";
            switch (type)
            {
                case 0: this.texture = Texture.smw_3up; break;
                default: this.texture = Texture.smw_3up; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
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
            else
            {
                y++;
                onGround = false;
                falling = true;
            }

            Image.drawImage(texture, x, y);
        }


    }
}
