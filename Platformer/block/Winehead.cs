using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Winehead : BaseObj
    {
        private short frame = 0;
        private short frameDelay = 0;
        private short growth = -1;
        private double yOrigin;
        private double yMove;
        
        private List<int> wines = new List<int>();

        public Winehead(double x, double y, short type = 1, bool dir = false)
            : base(x, y)
        {
            this.name = "Winehead";
            //despawnOffScreen = false;
            switch (type)
            {
                case 1: this.texture = Texture.smb1_winehead; break;
                default: this.texture = Texture.smb1_winehead; break;
            }
            this.dir = dir;
            if (x > 0 && y > 0 && x < RootThingy.sceneX && y < RootThingy.sceneY)
            {
                if (Map.map[(int)y / 16, (int)x / 16, 0] == 0)
                {
                    Map.map[(int)y / 16, (int)x / 16, 0] = 3;
                    Map.spriteAdd(new Wine(x, this.y, type, this.id, this.dir));
                }
            }
            if (dir)
                this.yMove = 1; //Growing down
            else
                this.yMove = -1;  //Growing up
            this.yOrigin = y;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.type = type;
            this.colWithOthers = false;
            this.colWithBlocks = true;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();
            if (colTop == 0)
            {
                this.growth++;
                if (this.growth >= 16)
                {
                    this.growth = 0;
                    if (yMove > 0)
                        Map.spriteAdd(new Wine(x, this.y, type, this.id, this.dir));
                    if (yMove < 0)
                        Map.spriteAdd(new Wine(x, this.y, type, this.id, this.dir));

                    if (x > 0 && y > 0 && x < RootThingy.sceneX && y < RootThingy.sceneY)
                        Map.map[(int)(y - 16) / 16, (int)x / 16, 0] = 3;
                }

                this.y += this.yMove;
            }
            else
            {
                this.yMove = 0;
                this.x = -100;
                this.y = -100;
            }
        }

        public override void doRender()
        {
            MyImage.drawImage(texture, x, y, false, dir);
        }
    }
}


