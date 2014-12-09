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
    class Koopa_red : BaseObj
    {
        public int hbW = 16;
        public int hbH = 16;
        private bool onGround;
        private bool falling;
        private short frame = 0;
        private short frames;
        private short frameDelay;
        public bool isDead = false; //does not respawn after it was killed
        private short state;
        private short behavior;
        private int stuntTimer;


        private AniFrame[][] stateArr = new AniFrame[6][]{                              //AniFrame info: ( <frame>, <vertical flip ?>, <horizontal flip ?> )
            new AniFrame[]{new AniFrame(08,false,false)},                               //0 stunned
            new AniFrame[]{new AniFrame(02,false,false),new AniFrame(03,false,false)},  //1 walk
            new AniFrame[]{new AniFrame(00,false,false),new AniFrame(01,false,false)},  //2 fly            
            new AniFrame[]{new AniFrame(07,false,false),new AniFrame(08,false,false)},  //3 unStun
            new AniFrame[]{new AniFrame(04,false,false),new AniFrame(05,false,false),new AniFrame(06,false,false),new AniFrame(08,false,false),new AniFrame(06,false,false),new AniFrame(05,false,false)},  //4 shell-spin
            new AniFrame[]{new AniFrame(08,false,true)},                                //5 dead
        };

        public Koopa_red(double x, double y, bool dir = false, short type = 1, short behavior = 1)
            : base(x, y, 12, 14)
        {
            switch (type)
            {
                case 1: this.texture = Texture.smb1_koopa_red; frames = 9; break;    //smb1 green koopa
                default: this.texture = Texture.smb1_koopa_red; frames = 9; break;   //smb1 green koopa
            }
            switch (behavior)
            {
                case 0: state = 0; this.name = "Koopa_red"; break;//
                case 1:
                default: state = 1; this.name = "Koopa_red"; break;
                case 2: state = 2; this.name = "Koopa_red_jmpLo"; break;
                case 3: state = 3; this.name = "Koopa_red_jmpHi"; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 24;
            this.colOffsetX = 2;
            this.colOffsetY = 10;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = 12;
            this.colRect.h = 14;
            this.type = type;
            this.dir = dir; //Startdirection: true = Left ; false = Right
            this.behavior = behavior;
            this.stuntTimer = 0;
            this.colWithOthers = true;
            onGround = false;
            falling = true;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();

            switch (state)
            {
                case 0: //stunned
                    state = 0;
                    if (stuntTimer > 0)
                        stuntTimer--;
                    if (stuntTimer == 0)
                        state = 1;
                    break;
                /////////////////////////////////////////////////////////////////////////////////// 
                case 1: //walk
                default:
                    state = 1;
                    if (getColXY((int)colRect.x - 1, (int)colRect.y + (colRect.h / 2)) == 1 || getColXY((int)colRect.x, (int)colRect.y + 1) != 1) //Left wall ?
                    {
                        dir = false;
                        falling = true;
                    }
                    if (getColXY((int)colRect.x + colRect.w + 1, (int)colRect.y + (colRect.h / 2)) == 1 || getColXY((int)colRect.x + colRect.w, (int)colRect.y + 1) != 1)  //Right wall ?
                    {
                        dir = true;
                        falling = true;
                    }


                    if (getColXY((int)colRect.x + (colRect.w / 2), (int)colRect.y + colRect.h + 1) == 1)    //floorCol
                    {
                        while (colBottom == 1)
                        {
                            y--;
                            refreshColRect();
                            getColGrid();
                        }
                        yVel = 0;
                        xVel = 0.3;
                        onGround = true;
                        falling = false;
                    }
                    else
                    {
                        yVel += Map.gravity;
                        onGround = false;
                        falling = true;
                    }
                    break;
                ///////////////////////////////////////////////////////////////////////////////////
                case 2: //jumping low
                case 3: //jumping high

                    if (getColXY((int)colRect.x - 1, (int)colRect.y + (colRect.h / 2)) == 1) //Left wall ?
                    {
                        dir = false;
                        falling = true;
                    }
                    if (getColXY((int)colRect.x + colRect.w + 1, (int)colRect.y + (colRect.h / 2)) == 1)  //Right wall ?
                    {
                        dir = true;
                        falling = true;
                    }


                    if (getColXY((int)colRect.x + (colRect.w / 2), (int)colRect.y + colRect.h + 1) == 1)    //floorCol
                    {
                        if (colBottom == 1)
                        {
                            while (colBottom == 1)
                            {
                                y--;
                                refreshColRect();
                                getColGrid();
                            }
                            xVel = 0.3;
                            if (state == 2)
                                yVel = -3.32;
                            if (state == 3)
                                yVel = -6.64;
                        }
                        onGround = true;
                        falling = false;
                    }
                    else
                    {
                        yVel += Map.gravity;
                        onGround = false;
                        falling = true;
                    }
                    if (getColXY((int)colRect.x + (colRect.w / 2), (int)colRect.y - 1) == 1)
                        yVel = 0;
                    break;
                ///////////////////////////////////////////////////////////////////////////////////
                case 4:
                    state = 3;//unstun
                    break;
                ///////////////////////////////////////////////////////////////////////////////////
                case 5: //shellspin
                    state = 4;
                    if (getColXY((int)colRect.x - 1, (int)colRect.y + (colRect.h / 2)) == 1) //Left wall ?
                    {
                        dir = false;
                        falling = true;
                    }
                    if (getColXY((int)colRect.x + colRect.w + 1, (int)colRect.y + (colRect.h / 2)) == 1)  //Right wall ?
                    {
                        dir = true;
                        falling = true;
                    }
                    if (getColXY((int)colRect.x + (colRect.w / 2), (int)colRect.y + colRect.h + 1) == 1)    //floorCol
                    {
                        while (colBottom == 1)
                        {
                            y--;
                            refreshColRect();
                            getColGrid();
                        }
                        yVel = 0;
                        xVel = 2.7;
                        onGround = true;
                        falling = false;
                    }
                    else
                    {
                        yVel += Map.gravity;
                        onGround = false;
                        falling = true;
                    }
                    break;
                ///////////////////////////////////////////////////////////////////////////////////
                case 6: //dead
                    state = 5;
                    isDead = true;
                    break;

            }

            y = y + yVel;
            if (dir)
                x -= xVel;
            else
                x += xVel;
        }

        public override void doRender()
        {
            animate();
        }

        private void animate()
        {
            frameDelay++;
            if (frameDelay == 9)
            { frame++; frameDelay = 0; }
            if (frame > stateArr[state].Length - 1)
                frame = 0;
            MyImage.drawTileFrame(texture, (stateArr[state][frame].id), frames, x, y, stateArr[state][frame].flipV ^ dir, stateArr[state][frame].flipH);
        }
    }
}


