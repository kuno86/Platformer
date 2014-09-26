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

        public Qm(double x, double y, short type=1, int contentID=0, bool invisible=false )
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
            if (x >= 0 && y >= 0)
                Map.map[(int)y / 16, (int)x / 16, 0] = 1;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
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
                    if (getCol2Obj(colRect, Map.spriteArray[i].colRect) && this.id != Map.spriteArray[i].id)
                    {
                        Map.spriteAdd(new Qm_e(x, y, type));
                        Map.spriteAdd(new Qm_open(x,y-16));
                        int tmp =Map.spriteAdd(Map.sprites[this.contentID]);
                        Map.spriteArray[tmp].setXY(x, y - h);
                        x = -100;
                        y = -100;
                    }

                }
            }

        }
               


    }
}


