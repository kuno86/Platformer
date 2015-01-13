using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Flagpole : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        private int texture2;
        private double flagX, flagY;
        private bool activated = false;
        private bool runOff = false;
        private bool done = false;
        private int activatorId = -1;
        
        public Flagpole(double x, double y)
            : base(x, y)
        {
            this.name = "Flagpole";
            this.texture = Texture.smb1_flagpole;
            this.texture2 = Texture.smb1_flagpoleflag;
            this.frames=4;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 160;

            flagX = x + 8;
            flagY = y + 18;
            Map.map[(int)((y+h-1) / 16),(int)((x+1) / 16), 0] = 1;  //make the block at the bottom solid
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = 9;
            this.colRect.h = 135;
            this.colOffsetX = 7;
            this.colOffsetY = 8;
            this.colWithOthers = true;
            this.colWithBlocks = false;
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
            if (!activated)
            {
                for (int i = 0; i != Map.spriteArrMax; i++)
                {
                    if (Map.spriteArray[i] != null && Map.spriteArray[i].name == "Player")
                    {
                        if (getCol2Obj(this.colRect, Map.spriteArray[i].colRect))
                        {
                            activated = true;
                            activatorId = Map.spriteArray[i].id;
                        }
                    }
                }
            }
            else
            {
                if (flagY + 16 <= colRect.y + colRect.h - 3)
                {
                    Map.spriteArray[activatorId].metaData["Control"] = "FFFFFFF";   //Block all input
                    Map.spriteArray[activatorId].setXYVel(0, 0);
                    Map.spriteArray[activatorId].metaData["setFrame"] = "20";
                    Map.spriteArray[activatorId].setXY(Map.spriteArray[activatorId].x, Map.spriteArray[activatorId].y + 1);
                    flagY++;
                }
                else
                { runOff = true;}

                if (runOff && getCol2Obj(this.colRect, Map.spriteArray[activatorId].colRect) && activated)
                {
                    Map.spriteArray[activatorId].metaData["Control"] = "FFFTFFF";   //Goes Right
                }
                else
                {
                    Map.spriteArray[activatorId].metaData["Control"] = "-------";   //Gives back control
                    runOff = false;
                    activated = false;
                    activatorId = -1;
                }
            }
        }
           

        public override void doRender()
        {
            MyImage.drawImage(texture, x, y);
            MyImage.drawTileFrame(texture2, frame, frames, flagX, flagY);
        }

    }
}




