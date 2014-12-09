using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Game
{
    class Platform_pulley : BaseObj
    {
        public BaseObj owner;
        //jumps 16 
        private short blockDistance, distL, distR;
        private int platformL, platformR;
        private bool fallOnLimit;
        private bool cutOff = false;
        private bool dead = false;

        public Platform_pulley(double x, double y, short PlatformsSize, bool startAtHalfSize = false, short blockDistance=1, short distL=2, short distR=2, bool fallOnLimit=false)
            : base(x, y, 8, 8)
        {
            this.name = "Platform_pulley";
            this.texture = Texture.smb1_pulley;
            this.blockDistance = blockDistance;
            this.distL = distL;
            this.distR = distR;
            this.fallOnLimit = fallOnLimit;
            this.x = x;
            this.y = y;
            this.w = 9;
            this.h = 9;
            this.colOffsetX = 7;
            this.colOffsetY = 0;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.colWithBlocks = false;
            this.colWithOthers = false;
            this.platformL = Map.spriteAdd(new Platform(1, 1, PlatformsSize, 7, startAtHalfSize));
            this.platformR = Map.spriteAdd(new Platform(1, 1, PlatformsSize, 7, startAtHalfSize));
            Map.spriteArray[platformL].setXY(this.colRect.x - Map.spriteArray[platformL].w / 2 + 8, this.colRect.y + this.colRect.h + (16 * distL));
            Map.spriteArray[platformR].setXY(this.colRect.x + (16 * blockDistance) + this.colRect.w - Map.spriteArray[platformR].w / 2 - 1, this.colRect.y + this.colRect.h + (16 * distR));
            Console.WriteLine("AddedPulley: L=" + platformL + " R=" + platformR);
        }

        public override string getName()
        { return this.name; }
                
        public override void doSubAI()
        {
            if (!dead)
            {
                if (cutOff && Map.spriteArray[platformL].y > RootThingy.sceneY + 100 && Map.spriteArray[platformR].y > RootThingy.sceneY + 100)
                {
                    Map.spriteArray[platformL].despawnOffScreen = true;
                    Map.spriteArray[platformR].despawnOffScreen = true;
                    dead = true;
                }
                if (!cutOff)
                {
                    if (Map.spriteArray[platformL].colCntSprites > Map.spriteArray[platformR].colCntSprites)   //Has Left Platform more Passengers ?
                    {
                        Map.spriteArray[platformL].yVel += 0.01;
                        Map.spriteArray[platformR].yVel = Map.spriteArray[platformL].yVel * -1;
                    }

                    if (Map.spriteArray[platformL].colCntSprites < Map.spriteArray[platformR].colCntSprites)   //Has Right Platform more Passengers ?
                    {
                        Map.spriteArray[platformR].yVel += 0.01;
                        Map.spriteArray[platformL].yVel = Map.spriteArray[platformR].yVel * -1;
                    }

                    if (Map.spriteArray[platformL].colRect.y <= colRect.y + colRect.h)  //Is a Platform hitting the limit (Roller)
                    {
                        Map.spriteArray[platformL].yVel = 0.005;
                        Map.spriteArray[platformR].yVel = -0.005;
                        if (fallOnLimit && !cutOff)
                            cutOff = true;
                    }
                    if (Map.spriteArray[platformR].colRect.y <= colRect.y + colRect.h)  //Is a Platform hitting the limit (Roller)
                    {
                        Map.spriteArray[platformL].yVel = -0.005;
                        Map.spriteArray[platformR].yVel = 0.005;
                        if (fallOnLimit && !cutOff)
                        { cutOff = true; }
                    }

                    if (Map.spriteArray[platformL].colCntSprites == Map.spriteArray[platformR].colCntSprites)       //PassengerCount is in Balance
                    {
                        if (Map.spriteArray[platformL].yVel > 0.01)
                        {
                            Map.spriteArray[platformL].yVel -= 0.01;
                            Map.spriteArray[platformR].yVel = Map.spriteArray[platformL].yVel * -1;
                        }
                        if (Map.spriteArray[platformL].yVel < -0.01)
                        {
                            Map.spriteArray[platformL].yVel += 0.01;
                            Map.spriteArray[platformR].yVel = Map.spriteArray[platformL].yVel * -1;
                        }
                        if (Map.spriteArray[platformL].yVel <= 0.01 && Map.spriteArray[platformL].yVel >= -0.01)
                        {
                            Map.spriteArray[platformL].yVel = 0;
                            Map.spriteArray[platformR].yVel = 0;
                        }
                    }
                }
                else
                {
                    Map.spriteArray[platformL].yVel += Map.gravity;
                    Map.spriteArray[platformR].yVel += Map.gravity;
                }
            }
        }

        public override void doRender()
        {
            if (!dead && !cutOff)
            {
                ////////////////////////////////////////////////////////////////////// All the below Code draws the pulley
                MyImage.endDraw2D();
                GL.Begin(PrimitiveType.Lines);                                     // O--
                GL.Color4(System.Drawing.Color.WhiteSmoke);                        // |
                GL.Vertex2(this.colRect.x + 1, this.colRect.y + this.colRect.h);   //Withe part of the Left UD-Line 
                GL.Vertex2(this.colRect.x + 1, Map.spriteArray[platformL].y);      //
                GL.End();                                                          //
                GL.Begin(PrimitiveType.Lines);                                     // O--                   
                GL.Color4(System.Drawing.Color.HotPink);                           // |
                GL.Vertex2(this.colRect.x + 2, this.colRect.y + this.colRect.h);   //Pink part of the Left UD-Line 
                GL.Vertex2(this.colRect.x + 2, Map.spriteArray[platformL].y);      //
                GL.End();                                                          //

                GL.Begin(PrimitiveType.Lines);                                                                      // --O
                GL.Color4(System.Drawing.Color.WhiteSmoke);                                                         //   |
                GL.Vertex2(x + this.colRect.w + (16 * blockDistance) - 1, this.colRect.y + this.colRect.h);         //Withe part of the Right UD-Line 
                GL.Vertex2(x + this.colRect.w + (16 * blockDistance) - 1, Map.spriteArray[platformR].y);            //
                GL.End();                                                                                           //
                GL.Begin(PrimitiveType.Lines);                                                              // --O
                GL.Color4(System.Drawing.Color.HotPink);                                                    //   |
                GL.Vertex2(x + this.colRect.w + (16 * blockDistance), this.colRect.y + this.colRect.h);     //Withe part of the Right UD-Line 
                GL.Vertex2(x + this.colRect.w + (16 * blockDistance), Map.spriteArray[platformR].y);        //
                GL.End();                                                                                   //
                MyImage.beginDraw2D();
            }

            MyImage.drawTileFrame(texture, 0, 2, x, y);                       //Left Pulley
            MyImage.drawTileFrame(texture, 1, 2, x + (blockDistance * 16), y);//Right Pulley
            MyImage.endDraw2D();
            GL.Begin(PrimitiveType.Lines);                  //O---O
            GL.Color4(System.Drawing.Color.WhiteSmoke);     //
            GL.Vertex2(x + 16, y + 1);                      //Withe part of the LR-Line 
            GL.Vertex2(x + (blockDistance * 16), y + 1);    //
            GL.End();                                       //
            GL.Begin(PrimitiveType.Lines);              //O---O
            GL.Color4(System.Drawing.Color.HotPink);    //
            GL.Vertex2(x + 16, y + 2);                  //Pink part of the LR-line
            GL.Vertex2(x + (blockDistance * 16), y + 2);//
            GL.End();                                   //
            MyImage.beginDraw2D();
        }
    }
}
