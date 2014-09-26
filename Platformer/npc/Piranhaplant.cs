using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing.Imaging;
using System.Threading;

namespace Game
{
    class Piranhaplant : BaseObj
    {
        public int hbW = 16;
        public int hbH = 16;
        private bool onGround;  //out position
        private short frames;
        private short frame = 0;
        private short frameDelay;
        public bool isDead = false; //does not respawn after it was killed
        private short state;
        private bool dir;
        private int timer;
        private bool ignorePlayer;
        private double y2;  //the point to which the plant extends out

        private AniFrame[][] stateArr = new AniFrame[1][]{                              //AniFrame info: ( <frame>, <vertical flip ?>, <horizontal flip ?> )
            new AniFrame[]{new AniFrame(00,false,false),new AniFrame(01,false,false)},  //munching
        };

        public Piranhaplant(double x, double y, short type = 1)
            : base(x, y, 16, 16)
        {
            switch (type)
            {
                default:
                case 1: this.texture = Texture.smb1_piranha_green; ignorePlayer = false; frames = 2; this.name = "Piranhaplant Green"; break;    //Brown smb1
                case 2: this.texture = Texture.smb1_piranha_red; ignorePlayer = true; frames = 2; this.name = "Piranhaplant Red"; break;    //Blue smb1
            }
            onGround = true;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 24;

            colOffsetX = 1;
            colOffsetY = -8;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            colRect.w = 14;
            colRect.h = 24;

            this.y2 = y+ colOffsetY + h;
            this.yVel = 1;
            this.dir = true;
            this.timer = 150;   //2,5 seconds
            this.type = type;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            getColGrid();
            
            //moves out -> stays 2,5s -> moves down -> stays 2,5s -> moves out ... 


            if (timer != 0)
            {
                timer--;
                if (timer<=h)
                {
                    y += yVel;
                }
            }
            if (ignorePlayer)
            {
                if (timer==0)
                {
                    yVel = yVel * -1;
                    onGround = !onGround;
                    timer = 180;
                }
            }
            else
            {
                for (int i = 0; i != Map.spriteArrMax; i++)
                {
                    if (Map.spriteArray[i] != null)
                    {
                        if (Map.spriteArray[i].name == "Player")
                        {
                            if (timer == 0 && Map.spriteArray[i].colRect.x - (Map.spriteArray[i].colRect.w / 2) - x - (w / 2) > 16)
                            {
                                yVel = yVel * -1;
                                onGround = !onGround;
                                timer = 180;
                            }

                            if (timer == 0 && Map.spriteArray[i].colRect.x + (Map.spriteArray[i].colRect.w / 2) - x + (w / 2) < -16)
                            {
                                yVel = yVel * -1;
                                onGround = !onGround;
                                timer = 180;
                            }

                            if ((Map.spriteArray[i].colRect.x - (Map.spriteArray[i].colRect.w / 2) - x - (w / 2) < 16) && (Map.spriteArray[i].colRect.x + (Map.spriteArray[i].colRect.w / 2) - x + (w / 2) > -16))
                            {
                                if (yVel == -1)
                                {
                                    yVel = 1;
                                    onGround = !onGround;
                                    timer = 180;
                                }
                                if (yVel == 1)
                                    timer = 90;
                            }
                        }
                    }
                }
            }
            

            animate();
            Image.drawText("t" + timer , (int)x, (int)y + 48, Color.White, Texture.ASCII);
            Image.drawText(";v" + yVel, (int)x, (int)y + 60, Color.White, Texture.ASCII);
            Image.drawText("Out" + onGround, (int)x, (int)y + 72, Color.White, Texture.ASCII);

            //Image.endDraw2D();
            //GL.Begin(PrimitiveType.LineLoop);
            //GL.Color3(Color.Aqua);
            //GL.Vertex2(colRect.x, colRect.y);
            //GL.Vertex2(colRect.x + colRect.w, colRect.y);
            //GL.Vertex2(colRect.x + colRect.w, colRect.y + colRect.h);
            //GL.Vertex2(colRect.x, colRect.y + colRect.h);
            //GL.End();
            //Image.beginDraw2D();

        }


        private void animate()
        {
            frameDelay++;
            if (frameDelay == 9)
            { frame++; frameDelay = 0; }
            if (frame > stateArr[state].Length - 1)
                frame = 0;
            Image.drawTileFrame(texture, (stateArr[state][frame].id), frames, x, y-8, stateArr[state][frame].flipV ^ dir, stateArr[state][frame].flipH);
        }
    }
}

