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
    class Bowser_smb1 : BaseObj
    {
        private bool onGround;
        private bool falling;
        private int frames;
        private short frame = 0;
        private short frameDelay;
        public bool isDead = false; //does not respawn after it was killed
        private short state;
        private int jumpDelay;
        private bool usesHammers;
        private bool usesFireballs;
        private int throwDelay;
        private int fireDelay;
        private bool playerOnTheLeft = true;
        
        private AniFrame[][] stateArr = new AniFrame[5][]{                                                          //AniFrame info: ( <frame>, <vertical flip ?>, <horizontal flip ?> )
            new AniFrame[]{new AniFrame(00,false,false),new AniFrame(01,false,false),new AniFrame(02,false,false)}, //0 idle / walk
            new AniFrame[]{new AniFrame(05,false,false)},                                                           //1 breathe in before fire spit
            new AniFrame[]{new AniFrame(06,false,false)},                                                           //2 spit fire
            new AniFrame[]{new AniFrame(03,false,false),new AniFrame(04,false,false)},                              //3 panik
            new AniFrame[]{new AniFrame(03,false,true)},                                                            //4 dead
        };

        public Bowser_smb1(double x, double y, bool dir = false, bool usesHammers = false)
            : base(x, y, 24, 32)
        {
            if (usesHammers)
                this.name = "Bowser_smb1_hammer";
            else
                this.name = "Bowser_smb1";
            this.texture = Texture.smb1_bowser;
            this.frames = 7;
            this.x = x;
            this.y = y;
            this.w = 32;
            this.h = 40;
            colOffsetX = 4;
            colOffsetY = 8;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            colRect.w = 24;
            colRect.h = 32;

            this.dir = dir; //Startdirection: true = Left ; false = Right
            onGround = false;
            falling = true;
            this.usesFireballs = true;
            this.usesHammers = usesHammers;
            throwDelay = Map.rnd.Next(30, 90);
            fireDelay = Map.rnd.Next(70, 100);
            jumpDelay = Map.rnd.Next(90, 150);
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();
            //////////////////////////AI-Part for Hammer-throwing
            if (usesHammers)
            {
                if (throwDelay > 0)
                    throwDelay--;
                if (throwDelay == 3)
                {
                    if (dir)     //Left
                        Map.spriteAdd(new Hammer_thrown(x + 1, y - 10, dir, -3 - (Map.rnd.NextDouble()), type));
                    else
                        Map.spriteAdd(new Hammer_thrown(x - 1, y - 10, dir, -3 - (Map.rnd.NextDouble()), type));
                }
                if (throwDelay == 0)
                { throwDelay = Map.rnd.Next(30, 90); }
            }

            //////////////////////////AI-Part for Fire-spitting
            if (usesFireballs)
            {
                if (fireDelay > 0)
                    fireDelay--;
                if (fireDelay <= 40 && fireDelay > 0)
                {
                    state = 1;
                }
                if (fireDelay == 0)
                { 
                    fireDelay = Map.rnd.Next(70, 100); 
                    state = 2;
                    if (dir)     //Left
                        Map.spriteAdd(new Fireball_Bowser_smb1(x - 4, y + 14, this.dir));
                    else
                        Map.spriteAdd(new Fireball_Bowser_smb1(x + 28, y + 14, this.dir));
                }
            }
            
            if (getColXY((int)x - 1, (int)y + (h / 2)) == 1) //Left wall ?
            {
                dir = false;
                falling = true;
            }
            if (getColXY((int)x + w + 1, (int)y + (h / 2)) == 1)  //Right wall ?
            {
                dir = true;
                falling = true;
            }


            if (getColXY((int)x + (w / 2), (int)y + h + 1) == 1)    //floorCol
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

            //////////////////////////AI-Part for jumping
            if (jumpDelay > 0 && onGround)
                jumpDelay--;
            if (jumpDelay <= 0)
            {
                jumpDelay = Map.rnd.Next(90, 150);
                yVel = -2.5;  //jumps ~2 blocks up
            }

            if (jumpDelay > 0 && !falling)
            {
                jumpDelay--;
                state = 0;
            }

            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null)
                {
                    if (Map.spriteArray[i].name == "Player")
                    {
                        if (Map.spriteArray[i].colRect.x + Map.spriteArray[i].colRect.w < this.colRect.x)     //player is left from this
                            playerOnTheLeft = true;
                        if (Map.spriteArray[i].colRect.x > this.colRect.x + this.colRect.w)     //player is right from this
                            playerOnTheLeft = false;
                    }
                }
            }

            int goesTo =Map.rnd.Next(0, 19);
            if (playerOnTheLeft)
            {
                if (goesTo >= 2)
                    x += xVel - 0.5;
                else
                    x += xVel + 0.5;
            }
            else
            {
                if (goesTo >= 2)
                    x -= xVel - 0.5;
                else
                    x -= xVel + 0.5;
            }
            y += yVel;


            if (playerOnTheLeft)
                dir = true;
            else
                dir = false;
                
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



