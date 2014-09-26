using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Game
{
    class Coin : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        public bool fixd;
        public bool onGround, falling;

        public Coin(double x, double y, short type = 1, bool fixd = true) 
            : base(x, y,16,16)
        {
            this.name = "Coin";
            switch(type){
                case 0: this.texture = Texture.smb1_coin1; frames = 4; break;
                case 1: this.texture = Texture.smb1_coin2; frames = 4; break;
                case 2: this.texture = Texture.smb2_coin; frames = 7; break;
                case 3: this.texture = Texture.smb3_coin1; frames = 4; break;
                case 4: this.texture = Texture.smw_coin1; frames = 4; break;
                default: this.texture = Texture.smb1_coin1; frames = 4; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            //this.colRect.x = (short)this.x;
            //this.colRect.y = (short)this.y;
            this.colOffsetX = 1;
            this.colOffsetY = 1;
            this.colRect.w = 14;
            this.colRect.h = 14;
            this.fixd = fixd;
            this.type = type;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            getColGrid();
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
            if (Map.powActive == 1)
            {
                fixd = false;
                xVel = Map.rnd.Next(-1, 1) * Map.rnd.NextDouble()*0.5;
            }
                       
            if (!fixd && (x > 0 && y > 0))
            {
                if (getColXY((int)x + (w / 2), (int)y + h + 1) == 1)    //floorCol
                {
                    if(colBottom==1)
                        y--;
                    yVel = 0;
                    onGround = true;
                    falling = false;
                }
                else
                {
                    yVel += Map.gravity;
                    onGround = false;
                    falling = true;
                }
                

                if (getColXY((int)x - 1, (int)y + (h / 2)) == 1)    //RightCol
                {
                    xVel = 0;
                    falling = true;
                }
                if (getColXY((int)x + w + 1, (int)y + (h / 2)) == 1)    //LeftCol
                {
                    xVel = 0;
                    falling = true;
                }

                y = y + yVel;
                x += xVel;
            }
            Image.drawTileFrame(texture, frame, frames, x, y);
        }
    
    
    }
}
