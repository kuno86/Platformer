﻿using System;
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
    class Koopa_green : BaseObj
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

        public Koopa_green(double x, double y, bool dir = false, short type = 1,  short behavior=1)
            : base(x, y, 16, 16)
        {
            switch (type)
            {
                case 1: this.texture = Texture.smb1_koopa_green; frames = 9; break;    //smb1 green koopa
                default: this.texture = Texture.smb1_koopa_green; frames = 9; break;   //smb1 green koopa
            }
            switch (behavior)
            {
                case 0: state = 0; this.name = "Koopa_green"; break;//
                case 1:
                default: state = 1; this.name = "Koopa_green"; break;
                case 2: state = 2; this.name = "Koopa_green_jmpLo"; break;
                case 3: state = 3; this.name = "Koopa_green_jmpHi"; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 24;
            this.colOffsetX = 2;
            this.colOffsetY = 10;
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

        public override void process()
        {
            refreshColRect();
            getColGrid();
            for (int i = 0; i != Map.spriteList.Count(); i++)
            {
                if (Map.spriteList[i].colWithOthers && this.id != Map.spriteList[i].id)
                {
                    if (getCol2Obj(colRect, Map.spriteList[i].colRect))
                    {
                        dir = !dir; //Map.spriteList[i].dir = !Map.spriteList[i].dir;
                    }
                }
            }
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
                            y--;
                        onGround = true;
                        falling = false;
                    }
                    else
                    {
                        y++;
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
                        y++;
                        onGround = false;
                        falling = true;
                    }
                    if (getColXY((int)colRect.x + (colRect.w / 2), (int)colRect.y - 1) == 1)
                        yVel = 0;
                    yVel += Map.gravity;
                    y = y + yVel;
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
                        if (colBottom == 1)
                            y--;
                        onGround = true;
                        falling = false;
                    }
                    else
                    {
                        y++;
                        onGround = false;
                        falling = true;
                    }
                    if (dir)
                        x -= 2.7;
                    else
                        x += 2.7;
                    break;
                ///////////////////////////////////////////////////////////////////////////////////
                case 6: //dead
                    state = 5;
                    isDead = true;
                    break;

            }

            if (dir)
                x -= 0.3;
            else
                x += 0.3;

            animate();

        }


        private void animate()
        {
            frameDelay++;
            if (frameDelay == 9)
            { frame++; frameDelay = 0; }
            if (frame > stateArr[state].Length - 1)
                frame = 0;
            Image.drawTileFrame(texture, (stateArr[state][frame].id), frames, x, y, stateArr[state][frame].flipV ^ dir, stateArr[state][frame].flipH);
        }
    }
}

