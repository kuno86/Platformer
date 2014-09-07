using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Pow : BaseObj
    {
        private short frame = 0;
        private short frameDelay = 0;
        public bool onGround;

        public Pow(double x, double y)
            : base(x, y)
        {
            this.name = "POW-Block";
            this.texture = Texture.smb2_pow;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colWithOthers = true;
            grabable = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            frameDelay++;
            if (frameDelay == 3)
            {
                frame++;
                frameDelay = 0;
            }
            if (frame > 8)
            {
                frame = 1;
            }
            Image.drawTileFrame(texture, frame, 8, x, y);

            
            for (int i = 0; i != Map.spriteList.Count(); i++)
            {
                if (Map.spriteList[i].name == "Player")
                {
                    if (Map.spriteList[i].x + Map.spriteList[i].w > x &&
                        Map.spriteList[i].x < x + w &&
                        Map.spriteList[i].y + Map.spriteList[i].h > y &&
                        Map.spriteList[i].y < y + h)
                    {
                        Map.spriteList.Add(new Smb2_pow_on(x, y));
                        x = -100;
                        y = -100;
                    }

                }
            }

        }


    }
}

