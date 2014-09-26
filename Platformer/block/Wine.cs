using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Wine : BaseObj
    {
        private short frame = 0;
        private short frameDelay = 0;
        private bool dir;

        public Wine(double x, double y, short type = 1, int head=0, bool dir=false)
            : base(x, y)
        {
            this.name = "Wine";
            switch (type)
            {
                case 1:
                default: this.texture = Texture.smb1_wine; break;
            }
            if (x >= 0 && y >= 0)
                Map.map[(int)y / 16, (int)x / 16, 0] = 3;
            this.dir = dir;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.type = type;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();

            Image.drawImage(texture, x, y,false,dir);

        }




    }
}

