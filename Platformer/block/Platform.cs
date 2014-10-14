using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Platform : BaseObj
    {
        private short frame = 0;
        private short frameDelay = 0;
        private short size;

        /// <summary>
        /// Platform Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="size">the number of Segments (each 8Pixel)</param>
        /// <param name="startAtHalfSize">start at 16-Grid or with +8 Pixels</param>
        public Platform(double x, double y, short size = 6, bool startAtHalfSize = true)
            : base(x, y)
        {
            this.name = "Platform";
            switch (type)
            {
                case 1: this.texture = Texture.smb1_platform; break;
                default: this.texture = Texture.smb1_platform; break;
            }
            this.yVel = -0.1;
            this.size = size;
            if (startAtHalfSize)
                this.x = x + 8;
            this.blockTop = true;
            this.y = y;
            this.w = (short)(8 * size);
            this.h = 8;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.type = type;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            
            xVel = Map.mausXVel;
            yVel = Map.mausYVel;
            y += yVel;
            x += xVel;

            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null)
                {
                    if (
                        //getCol2Obj(this.colRect, Map.spriteArray[i].colRect) &&
                        this.id != Map.spriteArray[i].id &&                                                                 //don't check collision with yourself
                        (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h > this.colRect.y) &&                  
                        (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h <= this.colRect.y +this.colRect.h) &&
                        (Map.spriteArray[i].colRect.x <= this.colRect.x + this.colRect.w && Map.spriteArray[i].colRect.x + Map.spriteArray[i].colRect.w >= this.colRect.x)
                        )
                    {
                        Map.spriteArray[i].setXYVel(this.xVel, this.yVel);
                        Map.spriteArray[i].setXY(Map.spriteArray[i].x + this.xVel, Map.spriteArray[i].y + this.yVel);

                        Image.drawText("!!", (int)Map.spriteArray[i].x, (int)Map.spriteArray[i].y - 12, System.Drawing.Color.White, Texture.ASCII);
                    }

                }
            }

            for (int i = 0; i != this.size; i++)
            {
                Image.drawImage(texture, x + (8 * i), y);
            }

        }




    }
}

