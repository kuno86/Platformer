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
        private int contentID = 103;

        public Qm(double x, double y, short type=1, int contentID=103, bool invisible=false )
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
            if(Map.sprites[contentID]!=null)
                this.contentID = contentID;
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


            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null && Map.spriteArray[i].name == "Player")
                {
                    if (Map.spriteArray[i].x + Map.spriteArray[i].w > x &&
                        Map.spriteArray[i].x < x + w &&
                        Map.spriteArray[i].y + Map.spriteArray[i].h > y &&
                        Map.spriteArray[i].y < y + h)
                    {
                        Map.spriteAdd(new Qm_e(x, y, type));
                        Map.spriteAdd(new Qm_open(x,y-16));
                        //Map.spriteAdd(new Starman(x,y-16));
                        Map.spriteAdd(Map.sprites[this.contentID]);
                        x = -100;
                        y = -100;
                    }

                }
            }

        }
               


    }
}


