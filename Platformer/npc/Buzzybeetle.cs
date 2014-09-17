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
    class Buzzybeetle : BaseObj
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


        private AniFrame[][] stateArr = new AniFrame[4][]{                              //AniFrame info: ( <frame>, <vertical flip ?>, <horizontal flip ?> )
            new AniFrame[]{new AniFrame(02,false,false)},                               //0 stunned
            new AniFrame[]{new AniFrame(00,false,false),new AniFrame(01,false,false)},  //1 walk    
            new AniFrame[]{new AniFrame(02,false,false),new AniFrame(03,false,false),new AniFrame(04,false,false),new AniFrame(05,false,false),new AniFrame(06,false,false),new AniFrame(07,false,false)},  //2 shell-spin
            new AniFrame[]{new AniFrame(02,false,true)},                                //3 dead
        };

        public Buzzybeetle(double x, double y, bool dir = false, short type = 1)
            : base(x, y, 16, 16)
        {
            this.name = "Buzzybeetle";
            switch (type)
            {
                case 1: this.texture = Texture.smb1_buzzybeetle; frames = 6; break;    //smb1 green koopa
                default: this.texture = Texture.smb1_buzzybeetle; frames = 6; break;   //smb1 green koopa
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 24;
            this.colOffsetX = 2;
            this.colOffsetY = 0;
            this.colRect.w = 12;
            this.colRect.h = 16;
            this.type = type;
            this.dir = dir; //Startdirection: true = Left ; false = Right
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
            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null)
                {
                    if (Map.spriteArray[i].colWithOthers && this.id != Map.spriteArray[i].id)
                    {
                        if (getCol2Obj(colRect, Map.spriteArray[i].colRect))
                        {
                            dir = !dir; //Map.spriteArray[i].dir = !Map.spriteArray[i].dir;
                        }
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
                        yVel = 0;
                        onGround = true;
                        falling = false;
                    }
                    else
                    {
                        yVel+=Map.gravity;
                        onGround = false;
                        falling = true;
                    }
                    break;

                ///////////////////////////////////////////////////////////////////////////////////
                case 2: //shellspin
                    state=2;
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
                    if (dir)
                        x -= 2.7;
                    else
                        x += 2.7;
                    break;

                ///////////////////////////////////////////////////////////////////////////////////
                case 3: //dead
                    state = 3;
                    isDead = true;
                    break;

            }
            y = y + yVel;

            if (dir)
                x -= 0.3;
            else
                x += 0.3;

            animate();

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
            Image.drawTileFrame(texture, (stateArr[state][frame].id), frames, x, y, stateArr[state][frame].flipV ^ dir, stateArr[state][frame].flipH);
        }
    }
}


