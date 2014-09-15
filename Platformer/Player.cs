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
    class Player : BaseObj
    {
        public int hbW = 11;
        public int hbH = 13;

        private double yVelMax = 1.5625;
        
        private int invulnerableTimer;       //Timer for Starpower
        private short jmpTimer;             //Max 8 ?
        private double jmpAccel = 0.6172;   //0.875 with gravity ?
        private short pMeter;                   // +=2 per frame with direction hold (without -=2 per frame)
        private double walkVelMax = 1.25;       //xVel-limit until pMeter == 112
        private double runVelMax = 2.25;        //xVel to this == running without Arms out
        private double sprintVelMax = 3.0;      //xVel to this == running with Arms out
        private double xAccel = 0.09375;            //Acceleration 
        private double xDecel = 0.0625;             //Deceleration if no diretion is pressed
        private double xWalkSkidDecel = 0.15625;    //Deceleration while walking -> pressing opposite direction of current direction
        private double xRunSkidDecel = 0.3125;      //Deceleration while running -> pressing opposite direction of current direction

        private bool falling, jumping, spinjumping, onGround, canJmp;
        private short state;

        private int frames;
        private int frame=0;
        private short frameDelay = 0;
        private AniFrame[][] stateArr=new AniFrame[22][]{                                       //AniFrame info: ( <frame>, <vertical flip ?>, <horizontal flip ?> )
            new AniFrame[]{new AniFrame(00,false,false),new AniFrame(01,false,false)},          //0  world
            new AniFrame[]{new AniFrame(02,false,false)},                                       //1  stand
            new AniFrame[]{new AniFrame(02,false,false),new AniFrame(03,false,false)},          //2  walk & run
            new AniFrame[]{new AniFrame(06,false,false),new AniFrame(07,false,false)},          //3  sprint
            new AniFrame[]{new AniFrame(04,false,false)},                                       //4  skid
            new AniFrame[]{new AniFrame(05,false,false)},                                       //5  jump
            new AniFrame[]{new AniFrame(25,false,false),new AniFrame(02,true,false),new AniFrame(26,true,false),new AniFrame(02,false,false)}, //6  spinjump
            new AniFrame[]{new AniFrame(08,false,false)},                                       //7  sprintjump
            new AniFrame[]{new AniFrame(09,false,false)},                                       //8  slide
            new AniFrame[]{new AniFrame(10,false,false)},                                       //9  grabstand
            new AniFrame[]{new AniFrame(11,false,false),new AniFrame(10,false,false)},          //10 grabmove
            new AniFrame[]{new AniFrame(12,false,false)},                                       //11 throw
            new AniFrame[]{new AniFrame(13,false,false),new AniFrame(14,false,false),new AniFrame(15,false,false),new AniFrame(16,false,false),new AniFrame(17,false,false)},   //12 swim
            new AniFrame[]{new AniFrame(18,false,false),new AniFrame(19,false,false)},          //13 climb
            new AniFrame[]{new AniFrame(20,false,false)},                                       //14 yoshi
            new AniFrame[]{new AniFrame(21,false,false)},                                       //15 yoshiducked
            new AniFrame[]{new AniFrame(23,false,false),new AniFrame(22,false,false)},          //16 pickup burried
            new AniFrame[]{new AniFrame(24,false,false)},                                       //17 vertical pipe
            new AniFrame[]{new AniFrame(27,false,false)},                                       //18 ducked
            new AniFrame[]{new AniFrame(28,false,false)},                                       //19 ducked itemgrab
            new AniFrame[]{new AniFrame(29,false,false),new AniFrame(30,false,false)},          //20 throw fireball
            new AniFrame[]{new AniFrame(31,false,false)},                                       //21 dying
        };

        public short power;     //0=small
        public short coins;
        public short lives;
        private short shotCoolDown = 30;
        
        public Key upBtn = Key.Up;
        public Key downBtn = Key.Down;
        public Key leftBtn = Key.Left;
        public Key rightBtn = Key.Right;
        public Key RunShootBtn = Key.C;
        public Key jumpBtn = Key.X;
        public Key spinjumpBtn = Key.Z; //its actually the "Y"-Key on my QWERTZ-Keyboard...

        public Player(double x, double y, short power = 0, short coins=0, short lives=5)
            : base(x, y, 16, 16, 10, 12, 2, 4)
        {
            this.name = "Player";
            switch (power)
            {
                case 0: setPowerUp_small(); break;
                case 1: setPowerUp_big(); break;
                case 2: setPowerUp_fire(); break;
                default: setPowerUp_small(); break;
            }
            this.state = 2;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.power = power;
            this.coins = coins;
            this.lives = lives;
            this.dir = false;
            falling = true;
            jumping = false;
            spinjumping = false;
            onGround = false;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            getColGrid();

            var keyboard = Keyboard.GetState();

            if (keyboard[upBtn] && colTop != 1)
            { ; }

            if (keyboard[downBtn])
            { 
                state=18; 
            }

            if (keyboard[leftBtn] && getColXY((int)x-1,(int)y+(h/2)) != 1)
            {
                if (xVel>0)    //direction != Inputdirection means skidding / breaking
                {
                    if (pMeter < 112)  //walking
                        xVel -= xWalkSkidDecel;
                    else if (pMeter >= 112)    //running / sprinting
                        xVel -= xRunSkidDecel;
                    state = 4;  //Skidding (frame 4)
                }
                if (xVel <= 0)
                    dir = true;
                if (dir == true)    //direction == Inputdirection means Acceleration
                {
                    if (pMeter >= 112)
                    {
                        if (keyboard[RunShootBtn])  //Player presses run-button
                        {
                            xVel -= xAccel;
                            if (xVel < sprintVelMax*-1)    //
                            {
                                xVel = sprintVelMax*-1;    //Limit to sprintspeed
                                state = 3;  //Sprinting (frames 6,7)
                            }
                        }
                        pMeter = 112;
                    }
                    else
                    {
                        xVel -= xAccel;
                        if (xVel < walkVelMax*-1)    //
                        {
                            xVel = walkVelMax*-1;    //Limit to walkspeed
                            state = 2;  //Walking (frames 2,3)
                        }
                    }    
                }
                pMeter += 2;
            }

            if (keyboard[rightBtn] && getColXY((int)x +w+ 1, (int)y + (h / 2)) != 1)
            {
                if (xVel<0)    //direction != Inputdirection means skidding / breaking
                {
                    if (pMeter < 112)  //walking
                        xVel += xWalkSkidDecel;
                    else if (pMeter >= 112)    //running / sprinting
                        xVel += xRunSkidDecel;
                    state = 4;  //Skidding (frame 4)
                }
                if(xVel >= 0)
                    dir = false;
                if (dir == false)    //direction == Inputdirection means Acceleration
                {
                    if (pMeter >= 112)
                    {
                        if (keyboard[RunShootBtn])  //Player presses run-button
                        {
                            xVel += xAccel;
                            if (xVel > sprintVelMax)    //
                            {
                                xVel = sprintVelMax;    //Limit to sprintspeed
                                state = 3;  //Sprinting (frames 6,7)
                            }
                        }
                        pMeter = 112;
                    }
                    else
                    {
                        xVel += xAccel;
                        if (xVel > walkVelMax)    //
                        {
                            xVel = walkVelMax;    //Limit to walkspeed
                            state = 2;  //Walking (frames 2,3)
                        }
                    }
                }
                pMeter += 2;
            }

            if (!keyboard[rightBtn] && !keyboard[leftBtn])
            {
                if (colBottom == 1)     //on ground
                {
                    if (xVel > 0)
                        xVel -= xDecel;
                    if (xVel < 0)
                        xVel += xDecel;
                }
                else                    //in Air (no friction
                {
                    if (xVel > 0)
                        xVel -= xDecel;
                    if (xVel < 0)
                        xVel += xDecel;
                }
                pMeter -= 2;
                if (pMeter < 0)
                    pMeter = 0;
            }


            
            if ((keyboard[jumpBtn] || keyboard[spinjumpBtn]) )//&& !falling)
            {
                if (jmpTimer <= 8 && canJmp)      //
                {                       //jump Velocity is incresed for max 8 frames
                    jmpTimer++;         //by jump-Acceleration
                    yVel -= jmpAccel;   //
                    if (keyboard[jumpBtn] && !spinjumping)
                    {
                        spinjumping = false; jumping = true;
                        if (state == 3) //sprinting ?
                            state = 8;  // -> sprintjump
                        if ((state == 1) || (state == 2) || (state == 4) || (state == 9) || (state == 13))
                            state = 5;
                    }
                    if (keyboard[spinjumpBtn])
                    {
                        spinjumping = true; 
                        jumping = false;
                        state = 6;
                    }
                    canJmp = true;
                    falling = false;
                    onGround = false;
                }
                else
                { canJmp = false; jmpTimer = 0; }
            }

            if (keyboard.IsKeyUp(spinjumpBtn) && spinjumping)
            {
                jmpTimer = 0; canJmp = false;
            }
            if (keyboard.IsKeyUp(jumpBtn) && jumping)
            {
                jmpTimer = 0; canJmp = false;
            }


            if (keyboard[RunShootBtn])
            {
                if (shotCoolDown == 0 && keyboard.IsKeyDown(RunShootBtn) && power==2)
                {
                    Map.spriteAdd(new Fireballshot(x, y, this, stateArr[state][frame].flipV ^ dir));
                    shotCoolDown = 5;//30
                }
            }
            if (shotCoolDown > 0 && keyboard.IsKeyUp(RunShootBtn))
                shotCoolDown--;


            if ((falling && !onGround) )  //in Air
            {                           //let gravity decrease it again
                yVel += Map.gravity;    //
                if (yVel > yVelMax)
                    yVel = yVelMax;
                jmpTimer = 0; 
                falling = true; 
                jumping = false;
            }
                


            if ((xVel < 0 && xVel > -xDecel) || (xVel > 0 && xVel < xDecel))    //
            { xVel = 0; }                                                       //Delete minimalistic double rests that let you never really stop
            //if ((yVel < 0 && yVel > xDecel) || (yVel > 0 && yVel < xDecel))     //
            //{yVel = 0;}                                                         //

            if (xVel == 0 && !jumping && !spinjumping && !falling)     //xVel == 0 means state standing
                state = 1;



            if (getColXY((int)x + (colRect.w / 2), (int)colRect.y + colRect.h + 1) == 1)   //Floor-Collision
            {
                if (colBottom == 1)
                {
                    y=(int)y-1;
                    
                }
                yVel = 0;
                onGround = true;
                jumping = false;
                spinjumping = false;
                falling = false;
                canJmp = true;
            }
            else
            {
                onGround = false;
                falling = true;
            }

            if (getColXY((int)colRect.x + (w / 2), (int)colRect.y - 1) == 1)  //Ceiling-Collision
            {
                if (colTop == 1)
                {
                    y=(int)y+1;

                }
                yVel = 0;
                onGround = false;
                jumping = false;
                falling = true;
                canJmp = false;
            }

            if (getColXY((int)colRect.x + colRect.w + 1, (int)colRect.y + (colRect.h / 2)) == 1)    //Right Wall Collsion
            {
                x--; 
                xVel=0; 
            }

            if (getColXY((int)colRect.x - 1, (int)colRect.y + (colRect.h / 2)) == 1)  //Left Wall Collision
            {
                x++; 
                xVel=0; 
            }

            if(keyboard[Key.LControl])
                yVel=-7;

            x += xVel;
            y += yVel;
                

            //get colliding Object and react to them
            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null)
                {
                    if (getCol2Obj(colRect, Map.spriteArray[i].colRect) && this.id != Map.spriteArray[i].id)
                    {
                        switch (Map.spriteArray[i].getName())
                        {
                            case "Coin":
                                getCoin();
                                Map.spriteArray[i].setXY(-100, -100);
                                Map.spriteAdd(new Smb1_getCoin(x, y));
                                break;

                            case "Mushroom":
                                if (power == 0)
                                    setPowerUp_big();
                                Map.spriteArray[i].setXY(-100, -100);
                                break;

                            case "Fireflower":
                                setPowerUp_fire();
                                Map.spriteArray[i].setXY(-100, -100);
                                break;

                            case "Starman":

                                Map.spriteArray[i].setXY(-100, -100);
                                invulnerableTimer = 1800;
                                break;

                            case "OneUp":
                                getLive();
                                Map.spriteArray[i].setXY(-100, -100);
                                break;

                            case "ThreeUp":
                                getLive();
                                getLive();
                                getLive();
                                Map.spriteArray[i].setXY(-100, -100);
                                break;

                            case "PSwitch_b":
                                Map.pSwitchTimer_b = 1800;  //1800 Frames = 30 seconds
                                break;

                            case "Mushroom_p":
                            case "Goomba":
                            case "Boo":
                                getDamage();
                                break;
                            default: break;
                        }
                    }
                }
            }


            if (invulnerableTimer > 0)
            {
                invulnerableTimer--;                        //Creates a Sparkle on a random Position withing the collision-box
                Map.spriteAdd(new Sparkle(Map.rnd.Next((int)colRect.x,(int)colRect.x+(int)colRect.w+1)-4,Map.rnd.Next((int)colRect.y,(int)colRect.y+(int)colRect.h)-4));
            }


            Image.drawText(("Player state   : " + state),640,480,Color.Aqua,Texture.ASCII);
            Image.drawText(("Player P-Meter : " + pMeter), 640, 492, Color.Aqua, Texture.ASCII);
            Image.drawText(("Player xVel    : " + xVel), 640, 504, Color.Aqua, Texture.ASCII);
            Image.drawText(("Player yVel    : " + yVel), 640, 516, Color.Aqua, Texture.ASCII);
            Image.drawText(("Player X       : " + x), 640, 528, Color.Aqua, Texture.ASCII);
            Image.drawText(("Player Y       : " + y), 640, 540, Color.Aqua, Texture.ASCII);
            Image.drawText(("Player onGround: " + onGround+"("+colBottom+")"), 640, 552, Color.Aqua, Texture.ASCII);
            Image.drawText(("Player jumping : " + jumping+"("+jmpTimer+")"), 640, 564, Color.Aqua, Texture.ASCII);
            Image.drawText(("Player falling : " + falling), 640, 576, Color.Aqua, Texture.ASCII);
            Image.drawText(("Player Coins   : " + coins), 640, 588, Color.Aqua, Texture.ASCII);
            Image.drawText(("Player Lives   : " + lives), 640, 600, Color.Aqua, Texture.ASCII);

            animate();



            Image.endDraw2D();
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Aqua);
            GL.Vertex2(colRect.x, colRect.y);
            GL.Vertex2(colRect.x+colRect.w, colRect.y);
            GL.Vertex2(colRect.x+colRect.w, colRect.y+colRect.h);
            GL.Vertex2(colRect.x, colRect.y+colRect.h);
            GL.End();
            Image.beginDraw2D();
            
           
        }


        private void animate()
        {
            frameDelay++;
            if (frameDelay == 3)
            { frame++; frameDelay = 0; }
            if (frame > stateArr[state].Length-1)
                frame = 0;
            Image.drawTileFrame(texture, (stateArr[state][frame].id), frames, x, y, stateArr[state][frame].flipV ^ dir, stateArr[state][frame].flipH); 
        }


        private void getCoin()
        {
            coins++;
            if (coins > 99)
            { coins = 0; getLive(); }       
        }

        private void getLive()
        {
            if (lives < 99)
                lives++;
        }

        private void getDamage()
        { ;}


        public void setPowerUp_small()  //0     (1 frame=16*16 Pixel)
        {
            colOffsetX = 3;
            colOffsetY = 18;

            colRect.w = 10;
            colRect.h = 14;
            frames = 28;
            texture = Texture.mario_small;

            power = 0;
        }

        public void setPowerUp_big()    //1     (1 frame=21*32 Pixel)
        {
            colOffsetX = 5;
            colOffsetY = 8;

            colRect.w = 12;
            colRect.h = 24;
            frames = 32;
            texture = Texture.mario_big;

            power = 1;
        }

        public void setPowerUp_fire()    //2    (1 frame=21*32 Pixel)
        {
            colOffsetX = 5;
            colOffsetY = 8;

            colRect.w = 12;
            colRect.h = 24;
            frames = 32;
            texture = Texture.mario_fire;

            power = 2;
        }

        public void setPowerUp_starman()
        {
            invulnerableTimer = 1800;
        }
        

    }
}
