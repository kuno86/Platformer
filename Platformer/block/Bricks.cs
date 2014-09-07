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
        private BaseObj content = null;

        public Bricks(double x, double y, short type = 1, BaseObj content = null)
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
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colWithOthers = true;
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
            if (frame > frames)
            {
                frame = 1;
            }

            for (int i = 0; i != Map.spriteList.Count(); i++)
            {
                if (Map.spriteList[i].name == "Player")
                {
                    if (Map.spriteList[i].x + Map.spriteList[i].w > x &&
                        Map.spriteList[i].x < x + w &&
                        Map.spriteList[i].y + Map.spriteList[i].h > y &&
                        Map.spriteList[i].y < y + h)
                    {
                        Map.spriteList.Add(new Bricks_shatter(x, y, type));
                        if(content!=null)
                            Map.spriteList.Add(new Qm_open(x, y - 16));
                        Map.spriteList.Add(new Fireflower(x, y - 16));
                        //Map.spriteList.Add(content);
                        x = -100;
                        y = -100;
                    }

                }
            }
            
            Image.drawTileFrame(texture, frame, frames, x, y);
            
        }
        

    }
}



