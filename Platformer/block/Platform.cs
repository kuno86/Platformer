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
        private short totalTravel = 0;  //Only for behaivor 5 & 6
        public short size;
        public int behaivor;
        public bool startAtHalfSize;
        private double adder, angle, angleDeg, xOrigin, yOrigin, xLast, yLast;
        private bool falling = false;   //only behaivor 1, 2 & 7
        /// <summary>
        /// Platform Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="size">the number of Segments (each 8Pixel)</param>
        /// <param name="behaivor">0=frozen, 1=fallOnPlayerTouch, 2=fallWhilePlayerTouch, 3=loopDown, 4=loopUp, 5=sinusLR, 6=sinusUD, 7=onPulley</param>
        /// <param name="startAtHalfSize">start at 16-Grid or with +8 Pixels</param>
        public Platform(double x, double y, short size = 6, int behaivor = 0, bool startAtHalfSize = false, short totalTravelPixel=64)
            : base(x, y)
        {
            this.name = "Platform";
            switch (type)
            {
                case 1: this.texture = Texture.smb1_platform; break;
                default: this.texture = Texture.smb1_platform; break;
            }
            //this.yVel = -0.1;
            this.size = size;
            if (startAtHalfSize)
                this.x = x + 8;
            this.behaivor = behaivor;
            this.totalTravel = totalTravelPixel;
            this.startAtHalfSize = startAtHalfSize;
            this.despawnOffScreen = false;
            this.blockTop = true;
            this.yOrigin = y;
            this.xOrigin = x;
            this.y = y;
            this.w = (short)(8 * size);
            this.h = 8;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.type = type;
            this.colWithOthers = true;
            this.colWithBlocks = false;
            this.adder = 0.015;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            getColGrid();
            
            //xVel = Map.mausXVel;
            //yVel = Map.mausYVel;
            //yVel=-0.1;
            y += yVel;
            x += xVel;

            switch (behaivor)
            {
                case 0:     ///////////////////////////////////////////////////Frozen 
                    {
                        yVel = 0;
                        xVel = 0;
                    } break;
                case 1:     ///////////////////////////////////////////////////fallOnPlayerTouch
                    {
                        for (int i = 0; i != Map.spriteArrMax; i++)
                        {
                            if (Map.spriteArray[i] != null)
                            {
                                if (
                                    Map.spriteArray[i].getName() == "Player" &&
                                    this.id != Map.spriteArray[i].id &&                                                         //don't check collision with yourself
                                    (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h > this.colRect.y) &&                       //
                                    (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h <= this.colRect.y + this.colRect.h) &&      //does the bottom of the other object touch the platform ?
                                    (Map.spriteArray[i].colRect.x <= this.colRect.x + this.colRect.w && Map.spriteArray[i].colRect.x + Map.spriteArray[i].colRect.w >= this.colRect.x)      //check the correct x-range of the platform
                                    )
                                { 
                                    falling = true; 
                                    this.despawnOffScreen = true; 
                                    yVel = 1; 
                                }
                            }
                        }
                    } break;
                case 2:     ///////////////////////////////////////////////////fallWhilePlayerTouch
                    {
                        for (int i = 0; i != Map.spriteArrMax; i++)
                        {
                            if (Map.spriteArray[i] != null)
                            {
                                if (
                                    Map.spriteArray[i].getName() == "Player" &&
                                    this.id != Map.spriteArray[i].id &&                                                         //don't check collision with yourself
                                    (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h > this.colRect.y) &&                       //
                                    (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h <= this.colRect.y + this.colRect.h) &&      //does the bottom of the other object touch the platform ?
                                    (Map.spriteArray[i].colRect.x <= this.colRect.x + this.colRect.w && Map.spriteArray[i].colRect.x + Map.spriteArray[i].colRect.w >= this.colRect.x)      //check the correct x-range of the platform
                                    )
                                { 
                                    falling = true; 
                                    this.despawnOffScreen = true; 
                                    yVel = 1;
                                }
                                else
                                { 
                                    falling = false;
                                    yVel = 0;
                                }
                            }
                        }
                    } break;
                case 3:     ///////////////////////////////////////////////////loopDown 
                    {
                        yVel = 1;
                        if (colRect.y + colRect.h > RootThingy.sceneY)
                            y = 1;
                    } break;
                case 4:     ///////////////////////////////////////////////////loopUp 
                    {
                        yVel = -1;
                        if (colRect.y < 0)
                            y = RootThingy.sceneY;
                    } break;
                case 5:     ///////////////////////////////////////////////////sinusLR 
                    {
                        xVel = x - xLast;
                        xLast = x;
                        angle += adder;
                        angleDeg = angle / 0.0174532925; //0.002777778;
                        while (angleDeg > 360)
                            angleDeg -= 360;
                        x = Math.Cos(angle) * totalTravel + xOrigin;

                    } break;
                case 6:     ///////////////////////////////////////////////////sinusUD
                    {
                        yVel = y - yLast;
                        yLast = y;
                        angle += adder;
                        angleDeg = angle / 0.0174532925; //0.002777778;
                        while (angleDeg > 360)
                            angleDeg -= 360;
                        y = Math.Cos(angle) * totalTravel + yOrigin;

                    } break;
                case 7:     ///////////////////////////////////////////////////onPulley 
                    {
                    } break;
            }

            
            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null)
                {
                    if (
                        //getCol2Obj(this.colRect, Map.spriteArray[i].colRect) &&
                        Map.spriteArray[i].colWithBlocks &&                     //Only Object that would be influenced by block-collision (Example: no Boo's)
                        this.id != Map.spriteArray[i].id &&                     //don't check collision with yourself
                        (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h > this.colRect.y) &&                       //
                        (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h <= this.colRect.y +this.colRect.h) &&      //does the bottom of the other object touch the platform ?
                        (Map.spriteArray[i].colRect.x <= this.colRect.x + this.colRect.w && Map.spriteArray[i].colRect.x + Map.spriteArray[i].colRect.w >= this.colRect.x)      //check the correct x-range of the platform
                        )
                    {
                        Map.spriteArray[i].setXYVel(this.xVel, this.yVel);
                        Map.spriteArray[i].setXY(Map.spriteArray[i].x + this.xVel, Map.spriteArray[i].y + this.yVel);
                    }

                }
            }

            this.colCntSprites = 0;
            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null)
                {
                    if (
                        //getCol2Obj(this.colRect, Map.spriteArray[i].colRect) &&
                        Map.spriteArray[i].colWithBlocks &&                     //Only Object that would be influenced by block-collision (Example: no Boo's)
                        this.id != Map.spriteArray[i].id &&                     //don't check collision with yourself
                        (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h +2 > this.colRect.y) &&                       //
                        (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h <= this.colRect.y + this.colRect.h) &&      //does the bottom of the other object touch the platform ?
                        (Map.spriteArray[i].colRect.x <= this.colRect.x + this.colRect.w && Map.spriteArray[i].colRect.x + Map.spriteArray[i].colRect.w >= this.colRect.x)      //check the correct x-range of the platform
                        )
                    {
                        
                        colCntSprites++;
                    }

                }
            }

            Image.drawText("Col: "+colCntSprites, (int)x, (int)(y + h), System.Drawing.Color.White, Texture.ASCII);

            for (int i = 0; i != this.size; i++)
            {
                Image.drawImage(texture, x + (8 * i), y);
            }

        }




    }
}

