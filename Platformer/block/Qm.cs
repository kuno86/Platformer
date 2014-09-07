using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Qm : BaseObj
    {
        private bool invisible;
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        private BaseObj content = null;

        public Qm(double x, double y, short type=1, BaseObj content=null, bool invisible=false )
            : base(x, y)
        {
            this.name = "?-Block";
            switch(type)
            {
                case 1: this.texture = Texture.smb1_qm; frames = 4; break;
                case 3: this.texture = Texture.smb3_qm; frames = 4; break;
                case 4: this.texture = Texture.smw_qm; frames = 4; break; 
                default: this.texture = Texture.smb1_qm; frames = 4; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colWithOthers = true;
            this.invisible = invisible;
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
            if (frame > 4)
            {
                frame = 1;
            }
            if(!invisible)
                Image.drawTileFrame(texture, frame, 4, x, y);


            for (int i = 0; i != Map.spriteList.Count(); i++)
            {
                if (Map.spriteList[i].name == "Player")
                {
                    if (Map.spriteList[i].x + Map.spriteList[i].w > x &&
                        Map.spriteList[i].x < x + w &&
                        Map.spriteList[i].y + Map.spriteList[i].h > y &&
                        Map.spriteList[i].y < y + h)
                    {
                        Map.spriteList.Add(new Qm_e(x, y, type));
                        Map.spriteList.Add(new Qm_open(x,y-16));
                        Map.spriteList.Add(new Starman(x,y-16));
                        //Map.spriteList.Add(content);
                        x = -100;
                        y = -100;
                    }

                }
            }

        }
               


    }
}


