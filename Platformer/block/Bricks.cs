using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Bricks : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        private int contentID = 0;

        public Bricks(double x, double y, short type = 1, int contentID = 0)
            : base(x, y)
        {
            this.name = "Bricks";
            this.type = type;
            switch (type)
            {
                case 1: this.texture = Texture.smb1_bricks1; frames = 2; break;
                case 2: this.texture = Texture.smb1_bricks2; frames = 2; break;
                case 3: this.texture = Texture.smb1_bricks3; frames = 2; break;
                case 4: this.texture = Texture.smb3_bricks; frames = 4; break;
                default: this.texture = Texture.smb1_bricks1; frames = 2; break;
            }
            if (Map.sprites[contentID] != null)
                this.contentID = contentID;
            if(x>=0 && y>=0)
                Map.map[(int)y/16, (int)x/16, 0] = 1;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }
                
        public override void doSubAI()
        {
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

            refreshColRect();
            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null && Map.spriteArray[i].name == "Player")
                {
                    if (getCol2Obj(colRect, Map.spriteArray[i].colRect) && this.id != Map.spriteArray[i].id)
                    {
                        Map.spriteAdd(new Bricks_shatter(x, y, type));
                        Map.map[(int)y / 16, (int)x / 16, 0] = 0;
                        if (contentID != 0)
                        {
                            Map.spriteAdd(new Qm_open(x, y - 16));
                            int tmp = Map.spriteAdd(DeepCopySprite(this.contentID));
                            Map.spriteArray[tmp].setXY(x, y - h);
                            Map.spriteAdd(new Qm_e(x, y, type));
                            Map.spriteAdd(new Qm_open(x, y - 16));
                        }
                        x = -100;
                        y = -100;
                    }

                }
            }
        }

        public override void doRender()
        {
            MyImage.drawTileFrame(texture, frame, frames, x, y);
        }
        

    }
}



