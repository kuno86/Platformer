using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Qm_e : BaseObj
    {
        private short frame = 0;
        private short frameDelay = 0;

        public Qm_e(double x, double y, short type=1)
            : base(x, y)
        {
            this.name = "?-Block_e";
            switch (type)
            {
                case 1: this.texture = Texture.smb1_qm_e; break;
                case 3: this.texture = Texture.smb3_qm_e; break;
                case 4: this.texture = Texture.smw_qm_e; break;
                default: this.texture = Texture.smb1_qm_e; break;
            }
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
            this.type = type;
            this.colWithOthers = true;
            this.colWithBlocks = false;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            
            Image.drawImage(texture, x, y);

        }




    }
}
