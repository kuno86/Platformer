using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Messagebox : BaseObj
    {

        public Messagebox(double x, double y, string message)
            : base(x, y)
        {
            this.name = "Messagebox";
            this.texture = Texture.smw_messagebox;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.colWithOthers = true;
            this.colWithBlocks = false;
            this.metaData.Add("Message", message);
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null && Map.spriteArray[i].name == "Player")
                {
                    if (getCol2Obj(colRect, Map.spriteArray[i].colRect) && this.id != Map.spriteArray[i].id)
                    {
                        this.showMessage();
                    }

                }
            }
        }

        public override void doRender()
        {
            MyImage.drawImage(texture, x, y);
        }

    }
}


