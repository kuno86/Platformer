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
        
        private int invulnerableTimer;      //Timer for Starpower
        private bool invulnerable = false;
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
        public short coins;     //100Coin = 1 extralive
        public short lives;     //guess what...
        private short shotCoolDown = 30;

        private int grabbedItemId = -1;     //an ID from SpritArray[] (-1 = no item)
        private bool grabbedItem_ColBlocksTemp;     //backup these item states for temporary disable
        private bool grabbedItem_ColOthersTemp;     //during grabbed
        private bool grabbedItem_despawnOffScreen;  //

        private bool canWarpRight = false;
        private bool canWarpDown = false;
        private bool canWarpLeft = false;
        private bool canWarpUp = false;
        private bool canWarpDoor = false;   
        private BaseObj canWarpObjStart = null; //holds the startpoint-warp-obj
        private BaseObj canWarpObjEnd = null;   //holds the Endpoint-warp-obj
        private bool isWarping = false;     //Between two warp-points (also pipe-entering/exiting)
        private bool warpState = false;     //false = enters, true = exits
        private short warpMove = 0;         //how far the above is
        
        
        public Key upBtnK = Key.Up; 
        public Key downBtnK = Key.Down;
        public Key leftBtnK = Key.Left;
        public Key rightBtnK = Key.Right;
        public Key RunShootBtnK = Key.C;
        public Key jumpBtnK = Key.X;
        public Key spinjumpBtnK = Key.Z; //its actually the "Y"-Key on a QWERTZ-Keyboard...

        private bool upBtn = false; 
        private bool downBtn = false;
        private bool leftBtn = false;
        private bool rightBtn = false;
        private bool RunShootBtn = false;
        private bool jumpBtn = false;
        private bool spinjumpBtn = false;

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
            this.colWithBlocks = true;
            metaData.Add("setFrame", "-1");
            metaData.Add("Control", "--------");
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();
            var keyboard = RootThingy.keyboard;
            
            /////////////////////////////////////// ('T' = Key active ; 'F' = Key inactive ; '-' = get Key value fom keyboard)
            /////////////////////////////////////// Up-Key
            if (metaData["Control"][0] == 'T')
                upBtn = true;
            if (metaData["Control"][0] == 'F')
                upBtn = false;
            if (metaData["Control"][0] == '-')
            { upBtn = keyboard[upBtnK];}
            /////////////////////////////////////// Down-Key
            if (metaData["Control"][1] == 'T')
                downBtn = true;
            if (metaData["Control"][1] == 'F')
                downBtn = false;
            if (metaData["Control"][1] == '-')
            { downBtn = keyboard[downBtnK];}
            /////////////////////////////////////// Left-Key
            if (metaData["Control"][2] == 'T')
                leftBtn = true;
            if (metaData["Control"][2] == 'F')
                leftBtn = false;
            if (metaData["Control"][2] == '-')
            { leftBtn = keyboard[leftBtnK];}
            /////////////////////////////////////// Right-Key
            if (metaData["Control"][3] == 'T')
                rightBtn = true;
            if (metaData["Control"][3] == 'F')
                rightBtn = false;
            if (metaData["Control"][3] == '-')
            { rightBtn = keyboard[rightBtnK];}
            /////////////////////////////////////// Run/Shoot-Key
            if (metaData["Control"][4] == 'T')
                RunShootBtn = true;
            if (metaData["Control"][4] == 'F')
                RunShootBtn = false;
            if (metaData["Control"][4] == '-')
            { RunShootBtn = keyboard[RunShootBtnK];}
            /////////////////////////////////////// Jump-Key
            if (metaData["Control"][5] == 'T')
                jumpBtn = true;
            if (metaData["Control"][5] == 'F')
                jumpBtn = false;
            if (metaData["Control"][5] == '-')
            { jumpBtn = keyboard[jumpBtnK];}
            /////////////////////////////////////// SpinJump-Key
            if (metaData["Control"][6] == 'T')
                spinjumpBtn = true;
            if (metaData["Control"][6] == 'F')
                spinjumpBtn = false;
            if (metaData["Control"][6] == '-')
            { spinjumpBtn = keyboard[spinjumpBtnK];}

            if (keyboard[upBtnK])
                metaData["Control2"] = "T------";
            if (downBtn)
                metaData["Control2"] = "-T-----";
            if (leftBtn)
                metaData["Control2"] = "--T----";
            if (rightBtn)
                metaData["Control2"] = "---T---";
            if (RunShootBtn)
                metaData["Control2"] = "----T--";
            if (jumpBtn)
                metaData["Control2"] = "-----T-";
            if (spinjumpBtn)
                metaData["Control2"] = "------T";

            this.setMessage("Hallo ich bin der Mario, und mein Ziel isses die Peach zu befreien. Die hängt wahscheinlich schonwieder bei Bowser ab... mmmm");

            if (keyboard[Key.Enter])
                this.showMessage();


            if (upBtn && colTop != 1)
            { ; }

            if (downBtn)
            {
                state = 18;
            }

            if (leftBtn && getColXY((int)x - 1, (int)y + (h / 2)) != 1)
            {
                if (xVel > 0)    //direction != Inputdirection means skidding / breaking
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
                        if (RunShootBtn)  //Player presses run-button
                        {
                            xVel -= xAccel;
                            if (xVel < sprintVelMax * -1)    //
                            {
                                xVel = sprintVelMax * -1;    //Limit to sprintspeed
                                state = 3;  //Sprinting (frames 6,7)
                            }
                        }
                        pMeter = 112;
                    }
                    else
                    {
                        xVel -= xAccel;
                        if (xVel < walkVelMax * -1)    //
                        {
                            xVel = walkVelMax * -1;    //Limit to walkspeed
                            state = 2;  //Walking (frames 2,3)
                        }
                    }
                }
                pMeter += 2;
            }

            if (rightBtn && getColXY((int)x + w + 1, (int)y + (h / 2)) != 1)
            {
                if (xVel < 0)    //direction != Inputdirection means skidding / breaking
                {
                    if (pMeter < 112)  //walking
                        xVel += xWalkSkidDecel;
                    else if (pMeter >= 112)    //running / sprinting
                        xVel += xRunSkidDecel;
                    state = 4;  //Skidding (frame 4)
                }
                if (xVel >= 0)
                    dir = false;
                if (dir == false)    //direction == Inputdirection means Acceleration
                {
                    if (pMeter >= 112)
                    {
                        if (RunShootBtn)  //Player presses run-button
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

            if (!rightBtn && !leftBtn)
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



            if ((jumpBtn || spinjumpBtn))//&& !falling)
            {
                if (jmpTimer <= 8 && canJmp)      //
                {                       //jump Velocity is incresed for max 8 frames
                    jmpTimer++;         //by jump-Acceleration
                    yVel -= jmpAccel;   //
                    if (jumpBtn && !spinjumping)
                    {
                        spinjumping = false; jumping = true;
                        if (state == 3) //sprinting ?
                            state = 8;  // -> sprintjump
                        if ((state == 1) || (state == 2) || (state == 4) || (state == 9) || (state == 13))
                            state = 5;
                    }
                    if (spinjumpBtn)
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

            if (keyboard.IsKeyUp(spinjumpBtnK) && spinjumping)
            {
                jmpTimer = 0; canJmp = false;
                //if(stateArr[state][frame]!=null)
                //    dir = stateArr[state][frame].flipV;
            }
            if (keyboard.IsKeyUp(jumpBtnK) && jumping)
            {
                jmpTimer = 0; canJmp = false;
            }

            ////////////////////////////////////////////////////////Warp-Handling
            //wartoWarp metaData:
            //====================
            //metaData["warpId"]        warpId int
            //metaData["isEntrance"]    isEntrance True or False    (True=Entrance, False=Exit)
            //metaData["warpType"]      type 0-2                    (0=Pipe, 1=Door, 2=Instant)
            //metaData["warpDir"]       direction 0-3               (0=Moving Right, 1=Moving Down, 2=Moving Left, 3=Moving Up)
            if (canWarpRight || canWarpDown || canWarpLeft || canWarpUp || canWarpDoor)
            {
                for (int i = 0; i != Map.spriteArrMax; i++)
                {
                    if (Map.spriteArray[i] != null)
                    {
                        if ((canWarpObjStart.metaData["warpId"] == Map.spriteArray[i].metaData["warpId"]) &&
                            (canWarpObjStart.metaData["isEntrance"] != Map.spriteArray[i].metaData["isEntrance"]) &&
                            (Map.spriteArray[i].id != this.id))
                        {
                            this.canWarpObjEnd = Map.spriteArray[i];
                        }
                    }
                }
                if (keyboard.IsKeyDown(rightBtnK))
                {
                    invulnerable = true;
                    isWarping = true;
                    warpState = false;
                }
                if (keyboard.IsKeyDown(downBtnK))
                {
                    invulnerable = true;
                    isWarping = true;
                    warpState = false;
                }
                if (keyboard.IsKeyDown(leftBtnK))
                {
                    invulnerable = true;
                    isWarping = true;
                    warpState = false;
                }
                if (keyboard.IsKeyDown(upBtnK))
                {
                    if (canWarpDoor)
                    {
                        invulnerable = true;
                        isWarping = true;
                        warpState = false;
                    }
                    if (keyboard.IsKeyDown(spinjumpBtnK | jumpBtnK) && canWarpUp)
                    {
                        invulnerable = true;
                        isWarping = true;
                        warpState = true;
                    }
                }
            }

            if (warpState == false && isWarping == true)    //Is warping and entering
            {
                if (canWarpObjEnd != null && canWarpObjStart.metaData["warpType"] == "0")  //type Pipe
                {
                    switch (int.Parse(canWarpObjStart.metaData["warpDir"]))
                    {
                        case 0: //warp Right
                            if (x <= canWarpObjStart.x + canWarpObjStart.w)
                                x++;
                            else
                                warpState = true;
                            break;
                        case 1: //warp Down
                            if (y <= canWarpObjStart.y + canWarpObjStart.h)
                                y++;
                            else
                                warpState = true;
                            break;
                        case 2: //warp Left
                            if (x + w >= canWarpObjStart.x)
                                x--;
                            else
                                warpState = true;
                            break;
                        case 3: //warp Up
                            if (y + h >= canWarpObjStart.y)
                                y--;
                            else
                                warpState = true;
                            break;
                    }
                }
                if (canWarpObjStart.metaData["warpType"] == "1")  //type Door
                {
                    setXY(canWarpObjEnd.x, canWarpObjEnd.y);
                    warpState = false;
                    canWarpObjEnd = null;
                    isWarping = false;
                }
            }
            if (warpState == true && isWarping == true)    //Is warping and exiting
            {
                if (canWarpObjEnd != null && canWarpObjEnd.metaData["warpType"] == "0")  //type Pipe
                {
                    switch (int.Parse(canWarpObjEnd.metaData["warpDir"]))
                    {
                        case 0: //warp Right
                            if (x <= canWarpObjEnd.x)
                                x++;
                            else
                            { warpState = false; isWarping = false; }
                            break;
                        case 1: //warp Down
                            if (y <= canWarpObjEnd.y)
                                y++;
                            else
                            { warpState = false; isWarping = false; }
                            break;
                        case 2: //warp Left
                            if (x + w >= canWarpObjEnd.x)
                                x--;
                            else
                            { warpState = false; isWarping = false; }
                            break;
                        case 3: //warp Up
                            if (y + h >= canWarpObjEnd.y)
                                y--;
                            else
                            { warpState = false; isWarping = false; }
                            break;
                    }
                }
            }
            canWarpRight = false;
            canWarpDown = false;
            canWarpLeft = false;
            canWarpUp = false;
            canWarpDoor = false;

            ////////////////////////////////////////////////////////End Warp-Handling

            if (RunShootBtn)
            {
                if (shotCoolDown == 0 && keyboard.IsKeyDown(RunShootBtnK) && power == 2)
                {
                    Map.spriteAdd(new Fireballshot(x, y, this, stateArr[state][frame].flipV ^ dir));
                    shotCoolDown = 5;//30
                }
                //This code is for item-grabbing
                for (int i = 0; i != Map.spriteArrMax; i++)
                {
                    if ((Map.spriteArray[i] != null) && ((getCol2Obj(Map.spriteArray[i].colRect, this.colRect) && Map.spriteArray[i].grabable) ||   // not null && colliding && grabable ?
                        (Map.spriteArray[i].id == grabbedItemId)))
                    {
                        grabbedItemId = Map.spriteArray[i].id;
                        this.grabbedItem_ColBlocksTemp = Map.spriteArray[grabbedItemId].colWithBlocks;
                        Map.spriteArray[grabbedItemId].colWithBlocks = false;
                        this.grabbedItem_ColOthersTemp = Map.spriteArray[grabbedItemId].colWithOthers;
                        Map.spriteArray[grabbedItemId].colWithOthers = false;
                        this.grabbedItem_despawnOffScreen = Map.spriteArray[grabbedItemId].despawnOffScreen;
                        Map.spriteArray[grabbedItemId].despawnOffScreen = false;
                    }
                }

            }         
            else  //this throws a grabed item
            {
                if (grabbedItemId != -1)
                {
                    short throwDir = 0;
                    if (dir)    //Left
                    { throwDir = -1; }
                    else        //Right
                    { throwDir = 1; }

                    if (upBtn || downBtn)
                    {
                        if (upBtn)
                        {
                            Map.spriteArray[grabbedItemId].setXYVel(this.xVel, -6);
                            Map.spriteArray[grabbedItemId].metaData["kicked"] = "";
                            Map.spriteArray[grabbedItemId].colWithBlocks = this.grabbedItem_ColBlocksTemp;
                            Map.spriteArray[grabbedItemId].colWithOthers = this.grabbedItem_ColOthersTemp;
                            Map.spriteArray[grabbedItemId].despawnOffScreen = this.grabbedItem_despawnOffScreen;
                            grabbedItemId = -1;
                        }
                        if (downBtn)
                        {
                            Map.spriteArray[grabbedItemId].setXYVel(this.xVel + (throwDir * 0.25), -0.5);
                            Map.spriteArray[grabbedItemId].metaData["kicked"] = "";
                            Map.spriteArray[grabbedItemId].colWithBlocks = this.grabbedItem_ColBlocksTemp;
                            Map.spriteArray[grabbedItemId].colWithOthers = this.grabbedItem_ColOthersTemp;
                            Map.spriteArray[grabbedItemId].despawnOffScreen = this.grabbedItem_despawnOffScreen;
                            grabbedItemId = -1;
                        }
                    }
                    else
                    {
                        Map.spriteArray[grabbedItemId].setXYVel(this.xVel + (throwDir * 2.7), -0.5);
                        Map.spriteArray[grabbedItemId].metaData["kicked"] = "kicked";
                        Map.spriteArray[grabbedItemId].colWithBlocks = this.grabbedItem_ColBlocksTemp;
                        Map.spriteArray[grabbedItemId].colWithOthers = this.grabbedItem_ColOthersTemp;
                        Map.spriteArray[grabbedItemId].despawnOffScreen = this.grabbedItem_despawnOffScreen;
                        grabbedItemId = -1;
                    }
                }
            }

            if (grabbedItemId != -1)
            {
                Map.spriteArray[grabbedItemId].setXYVel(this.xVel, this.yVel);
                if (dir) //Left
                {
                    Map.spriteArray[grabbedItemId].setXY(colRect.x - Map.spriteArray[grabbedItemId].colRect.w, colRect.y + colRect.h - Map.spriteArray[grabbedItemId].colRect.h-2);
                    Map.spriteArray[grabbedItemId].dir = this.dir;
                }
                else
                {
                    Map.spriteArray[grabbedItemId].setXY(colRect.x + colRect.w, colRect.y + colRect.h - Map.spriteArray[grabbedItemId].colRect.h-2);
                    Map.spriteArray[grabbedItemId].dir = this.dir;
                }
            }

            if (shotCoolDown > 0 && keyboard.IsKeyUp(RunShootBtnK))
                shotCoolDown--;


            if ((falling && !onGround))  //in Air
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
                while (colBottom == 1)
                {
                    y--;
                    refreshColRect();
                    getColGrid();
                    yVel = 0;
                }
                if (!jumping && !spinjumping)
                {
                    onGround = true;
                    falling = false;
                    canJmp = true;
                }
                else
                {
                    jumping = false;
                    spinjumping = false;
                    falling = false;
                    canJmp = true;
                }
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
                    y++;
                    yVel = 0;
                }

                onGround = false;
                jumping = false;
                falling = true;
                canJmp = false;
            }

            if (getColXY((int)colRect.x + colRect.w + 1, (int)colRect.y + (colRect.h / 2)) == 1)    //Right Wall Collsion
            {
                x--;
                xVel = 0;
            }

            if (getColXY((int)colRect.x - 1, (int)colRect.y + (colRect.h / 2)) == 1)  //Left Wall Collision
            {
                x++;
                xVel = 0;
            }

            if (keyboard[Key.LControl])
                yVel = -7;

            x += xVel;
            y += yVel;


            //get colliding Object and react to them
            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null)
                {
                    if (getCol2Obj(colRect, Map.spriteArray[i].colRect) && this.id != Map.spriteArray[i].id)
                    {
                        Console.WriteLine("Player touches something");
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

                            case "WarpToWarp":
                                processWarp(Map.spriteArray[i]);
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

            invulnerable = false;
            if (invulnerableTimer > 0)
            {
                invulnerableTimer--;                        //Creates a Sparkle on a random Position withing the collision-box
                invulnerable = true;
                Map.spriteAdd(new Sparkle(Map.rnd.Next((int)colRect.x, (int)colRect.x + (int)colRect.w + 1) - 4, Map.rnd.Next((int)colRect.y, (int)colRect.y + (int)colRect.h) - 4));
            }
        }

        public override void doRender()
        {
            String canWarp = "";
            if (canWarpRight) canWarp += "R ";
            if (canWarpDown) canWarp += "D ";
            if (canWarpLeft) canWarp += "L ";
            if (canWarpUp) canWarp += "U ";
            if (canWarpDoor) canWarp += "Door ";
            MyImage.drawText(("Player state   : " + state), 640, 480, Color.Aqua, Texture.ASCII);
            MyImage.drawText(("Player P-Meter : " + pMeter), 640, 492, Color.Aqua, Texture.ASCII);
            MyImage.drawText(("Player xVel    : " + xVel), 640, 504, Color.Aqua, Texture.ASCII);
            MyImage.drawText(("Player yVel    : " + yVel), 640, 516, Color.Aqua, Texture.ASCII);
            MyImage.drawText(("Player X       : " + x), 640, 528, Color.Aqua, Texture.ASCII);
            MyImage.drawText(("Player Y       : " + y), 640, 540, Color.Aqua, Texture.ASCII);
            MyImage.drawText(("Player onGround: " + onGround + "(" + colBottom + ")"), 640, 552, Color.Aqua, Texture.ASCII);
            MyImage.drawText(("Player jumping : " + jumping + "(" + jmpTimer + ")"), 640, 564, Color.Aqua, Texture.ASCII);
            MyImage.drawText(("Player falling : " + falling), 640, 576, Color.Aqua, Texture.ASCII);
            MyImage.drawText(("Player Coins   : " + coins), 640, 588, Color.Aqua, Texture.ASCII);
            MyImage.drawText(("Player Lives   : " + lives), 640, 600, Color.Aqua, Texture.ASCII);
            MyImage.drawText(("Player can Warp: " + canWarp), 640, 612, Color.Green, Texture.ASCII);

            animate();
        }

        private void animate()
        {
            if (metaData["setFrame"]!="-1")
            {
                int setFrame = - 1;
                try
                {
                    setFrame = int.Parse(metaData["setFrame"]);
                    Console.WriteLine("SetFrame: " + setFrame);
                }
                catch{;}
                MyImage.drawTileFrame(texture, setFrame, frames, x, y, dir);
                metaData["setFrame"] = "-1";
            }
            else
            {
                frameDelay++;
                if (frameDelay == 3)
                { frame++; frameDelay = 0; }
                if (frame > stateArr[state].Length - 1)
                    frame = 0;
                MyImage.drawTileFrame(texture, (stateArr[state][frame].id), frames, x, y, stateArr[state][frame].flipV ^ dir, stateArr[state][frame].flipH);
            }
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

            colRect.x = x + colOffsetX;
            colRect.y = y + colOffsetY;

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

            colRect.x = x + colOffsetX;
            colRect.y = y + colOffsetY;

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

            colRect.x = x + colOffsetX;
            colRect.y = y + colOffsetY;

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

        public void processWarp(BaseObj warpSprite)
        {   //wartoWarp metaData:
            //====================
            //metaData["warpId"]        warpId int
            //metaData["isEntrance"]    isEntrance True or False    (True=Entrance, False=Exit)
            //metaData["warpType"]      type 0-2                    (0=Pipe, 1=Door, 2=Instant)
            //metaData["warpDir"]       direction 0-3               (0=Moving Right, 1=Moving Down, 2=Moving Left, 3=Moving Up)

            canWarpRight = false;
            canWarpDown = false;
            canWarpLeft = false;
            canWarpUp = false;
            canWarpDoor = false;
            canWarpObjStart = null;
            switch (int.Parse(warpSprite.metaData["warpType"]))
            {
                case 0:     //Pipes
                    {
                        switch (int.Parse(warpSprite.metaData["warpDir"]))
                        {
                            case 0:
                            default: if (onGround) canWarpRight = true; canWarpObjStart = warpSprite; break;
                            case 1: if (onGround) canWarpDown = true; canWarpObjStart = warpSprite; break;
                            case 2: if (onGround) canWarpLeft = true; canWarpObjStart = warpSprite; break;
                            case 3: canWarpUp = true; canWarpObjStart = warpSprite; break;
                        }
                    }
                    break;
                case 1:
                    {
                        canWarpDoor = true; 
                        canWarpObjStart = warpSprite;
                    }
                    break;
            }

        }
        

    }
}
