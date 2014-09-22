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
using System.Diagnostics;

namespace Game
{
    class Map
    {
        public struct Point
        {
            public double x;
            public double y;
        }

        string exePath = Environment.CurrentDirectory;
        private int width = (int)Game.RootThingy.sceneX;
        private int heigth = Game.RootThingy.sceneY;
        private int sceneX = (int)RootThingy.sceneX;
        private int sceneY = (int)RootThingy.sceneY;
        private int mausRad, mausRadLast;
        private int currentDimension;
        private int[,] currentIDArr = new int[5, 2] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };   //first dimesion is dimension-ID, then {currentID,MaxID} 
        int bgW, bgH;
        private int tileSetW;
        private int tileSetH;
        public static int tileSize;
        public int frameCount = 0;
        public int frameDelay = 3;
        public int maxTextureSize;


        public static List<int> fpsLine = new List<int>();


        public static double xFriction = 0.0625;
        public static double xFrictionIce = 0.0078125;
        public static double gravity = 0.083;

        public static Random rnd = new Random();

        public static double temp = 0.25;

        public static short powActive;
        public static bool pSwitch_b = false;
        public static bool pSwitch_s = false;
        public static int pSwitchTimer_b = -1;   //Timer for blue P-Switch
        public static int pSwitchTimer_s = -1;   //Timer for silver P-Switch
        

        public static List<BaseObj> spriteList = new List<BaseObj>();   //List of all sprites that are currently active

        public static BaseObj[] spriteArray = new BaseObj[4096];
        public static int spriteArrMax;

        public static BaseObj[] sprites = new BaseObj[1024];  // this is a catalouge of all spawnable sprites



        public static int[, ,] map = new int[Game.RootThingy.sceneY / 16, (int)Game.RootThingy.sceneX / 16, 5];
        //int[, ,] map = new int[45, 80, 5];      //720p
        //int[, ,] map = new int[30, 53, 5];      //480p
        //int[, ,] map = new int[19, 26, 5];      //SMBX is 26x19 visible blocks

        short [] colInf= new short[143] {       //  0=non-solid     1=solid     2=cloud (only top-solid)
                    1,0,0,0,0,1,1,0,0,0,0,1,1,
                    0,0,0,0,0,1,0,0,1,2,1,1,1,
                    1,1,1,0,1,1,0,0,1,0,0,0,0,
                    1,1,0,0,2,1,0,1,0,1,0,2,2,
                    2,0,0,0,1,1,1,0,0,1,0,0,0,
                    0,0,0,0,1,1,1,0,0,0,1,1,0,
                    1,0,2,1,1,1,1,1,1,2,1,1,0,
                    0,0,0,0,2,2,2,0,2,0,0,0,0,
                    0,0,0,0,1,1,1,0,0,0,0,0,0,
                    0,0,0,0,1,0,1,1,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,0,0,0
                    };


        public Map(int tilesize)   //Constructor
        {
            int x=0;
            int y=0;
            tileSize = tilesize;
            currentDimension = 2;
            spriteArrMax = 0;
            sprites[000] = null;
            sprites[001] = new Player(-200, -200);
            sprites[002] = new Boo(-200, -200);
            sprites[003] = new Goomba(-200, -200, false, (short)rnd.Next(1, 4));
            sprites[004] = new Koopa_green(-200, -200);
            sprites[005] = new Koopa_green(-200, -200, false, 1, 2);  //jumping
            sprites[006] = new Koopa_green(-200, -200, false, 1, 3);  //high jumping
            sprites[007] = new Koopa_red(-200, -200);
            sprites[008] = new Koopa_red(-200, -200, false, 1, 2);  //jumping
            sprites[009] = new Koopa_red(-200, -200, false, 1, 3);  //high jumping
            sprites[010] = new Bulletbill(-200, -200, false, (short)rnd.Next(1, 5));
            sprites[011] = new Bulletbill(-200, -200, false, 3,true);   //homing version (only type 3 !!!)
            sprites[012] = new Buzzybeetle(-200, -200, false);
            sprites[013] = new Spiny(-200, -200, false, 1, false);  //Spiny
            sprites[014] = new Spiny(-200, -200, false, 1, true);   //Spiny-Egg
            sprites[015] = new Piranhaplant(-200, -200, 1);     //Green Piranha Plant
            sprites[016] = new Piranhaplant(-200, -200, 2);     //Red Piranha Plant
            sprites[017] = new Hammerbros(-200, -200);

            sprites[100] = new Bricks(-200, -200, (short)rnd.Next(1, 5));
            sprites[101] = new Pow(-200, -200);
            sprites[102] = new Qm(-200, -200);
            sprites[103] = new Coin(-200, -200, (short)rnd.Next(1, 6));
            sprites[104] = new Mushroom(-200, -200, false, (short)rnd.Next(1, 3));
            sprites[105] = new Mushroom_p(-200, -200);
            sprites[106] = new Fireflower(-200, -200, (short)rnd.Next(1, 4));
            sprites[107] = new Starman(-200, -200, false, (short)rnd.Next(1, 5));
            sprites[108] = new OneUp(-200, -200);
            sprites[109] = new ThreeUp(-200, -200);
            sprites[110] = new PSwitch_b(-200, -200);
            sprites[111] = new Spring_p(-200, -200);
            sprites[112] = new Cannon(-200, -200);
            sprites[113] = new Keyy(-200, -200, (short)rnd.Next(1, 3));

            sprites[114] = new Lava(-200, -200);
            sprites[115] = new Potaboo(-200, -200);
            
            maxTextureSize = GL.GetInteger(OpenTK.Graphics.OpenGL.GetPName.MaxTextureSize);
            Console.WriteLine("BG: " + Texture.backGround);
            Image.activeTexture = Texture.backGround;
            GL.BindTexture(TextureTarget.Texture2D, Texture.backGround);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out bgW);     //Get Width and
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out bgH);    //Height from the loaded Background

            Image.activeTexture = Texture.tileSet;
            GL.BindTexture(TextureTarget.Texture2D, Texture.tileSet);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out tileSetW);    //Get Width and
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out tileSetH);   //Height from the loaded tileset

            currentIDArr[0, 1] = 2;                                                 //numer of diffrent collision-types
            currentIDArr[1, 1] = Texture.bgTilesArray.GetLength(0) - 1;             //Max Back- / Fore-ground Object ID
            currentIDArr[2, 1] = ((tileSetW / tileSize) * (tileSetH / tileSize))-1; //max tileID
            currentIDArr[3, 1] = sprites.GetLength(0) - 1;                          //max spriteID
            currentIDArr[4, 1] = Texture.bgTilesArray.GetLength(0) - 1;             //Max Back- / Fore-ground Object ID

            while (y < map.GetLength(0))    //fill a new map with empty and no collision
            {
                while (x < map.GetLength(1))
                {
                    map[y, x, 0] = 0;           //collision info, 0=non-solid     1=solid     2=cloud (only top-solid)      3=Ladder        4=Lava/Instant-death      5=Spikes/1Hit
                    map[y, x, 1] = 0;           //Background Objects (not the actual Background !!!)
                    map[y, x, 2] = currentIDArr[2,1]-3; //Tile-IDs, -1 means no block there
                    map[y, x, 3] = 0;           //sprite-IDs, -1 means that the sprites was spawned and won't spawn endless each loop
                    map[y, x, 4] = 0;           //Foreground Objects (no collision)
                    
                    x++;
                    if (x == map.GetLength(1))
                    { y++; x = 0; }
                    if (y == map.GetLength(0))
                        break;
                }
            }

            map[9, 9, 3] = 5; //Test Player
            map[10, 10, 3] = 2; //Test Boo
            map[11, 11, 3] = 300;   //Test Fireball (harms player)

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// SAVE
        public bool save(string filename)
        {
            
            return true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// LOAD
        public bool load(string filename)
        {

            return true;
        }
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// PROCESS
        public void process(int mausX, int mausY)
        {
            
            int fpsX = 642;
            int fpsY = 367;
            
            if (fpsLine.Count >= 1)
            {
                for (int i = 0; i != fpsLine.Count - 1; i++)
                {
                    GL.Begin(PrimitiveType.Lines);
                    GL.Color3(Color.Yellow);
                    GL.Vertex2(fpsX + i, fpsY + 100);
                    GL.Vertex2(fpsX + i, fpsY + 100 - fpsLine[i]);
                    GL.End();
                }
            }

            Image.beginDraw2D();

            if (fpsLine.Count >= 1)
                Image.drawText("FPS-Graph:"+(int)fpsLine[0], 705, 468, Color.Yellow, Texture.ASCII);

            frameCount++;
            if (frameCount == 10)
            {
                frameDelay++;
                frameCount=0;
                if (frameDelay == 10)
                    frameDelay = 0;
            }

            var keyboard = Keyboard.GetState();     
            var maus = Mouse.GetState();        //Don't use this mouse for coordinates, their origin is not from the gameWindow !!!


            Image.drawText("Press F12 to toggle Debug-Info", 870, 38, Color.Blue, Texture.ASCII);
            for (int i = 0; i <= spriteArrMax; i++)
            {
                if (spriteArray[i] != null)
                {
                    if(RootThingy.debugInfo)
                        Image.drawText("[" + i + "]=" + spriteArray[i].name + " X=" + (int)spriteArray[i].x + " Y=" + (int)spriteArray[i].y, 870, (i * 12) + 50, Color.Red, Texture.ASCII);

                    spriteArray[i].process();
                    if (spriteArray[i].despawnOffScreen)
                    //{
                        if (spriteArray[i].x + spriteArray[i].w > RootThingy.sceneX || spriteArray[i].x < 0 || spriteArray[i].y < 0 || spriteArray[i].y + spriteArray[i].h > RootThingy.sceneY)
                        {
                            spriteArray[i] = null;
                            //spriteArrayCount--; 
                        }   //Delete sprites that are out of scene Borders
                    //}
                }
            }



            if (powActive > 0)
                powActive--;
                                                                        
            if (pSwitchTimer_b > -1)        //Handle Blue P-Switches
                pSwitchTimer_b--;                   //Count down the switch duration
            if (pSwitchTimer_b == 0)                                                    //
            {                                                                             //
                pSwitch_b = false;                                                          //
                double tempX;                                                                   //
                double tempY;                                                                       //
                short tempType;                                                                         //
                for (int i = 0; i != spriteArrMax; i++)                                               //
                {
                    if (spriteArray[i] != null)                                                            //
                    {
                        if (spriteArray[i].name == "Coin")                                                       //
                        {                                                                                       //
                            tempX = spriteArray[i].x;                                            //
                            tempY = spriteArray[i].y;                                            //Buffer X, Y and type of the object to be replaced
                            tempType = spriteArray[i].type;                                      //
                            spriteArray[i] = new Qm_e(tempX, tempY, tempType);               //Overwrite position with new object data from Buffers
                        }                                                                                       //
                        else if (spriteArray[i].name == "?-Block_e")                                             //
                        {                                                                                       //
                            tempX = spriteArray[i].x;                                                            //
                            tempY = spriteArray[i].y;                                                            //
                            tempType = spriteArray[i].type;                                                      //
                            spriteArray[i] = new Coin(tempX, tempY, tempType);                                   //
                        }
                    }                                                                                         //
                }                                                                                           //
            }

            if (pSwitchTimer_b == 1)
                pSwitch_b = false;

                                    //Handle Silver P-Switches
            if (pSwitchTimer_s > -1)    //Count down the switch duration
                pSwitchTimer_s--;       //
            if (pSwitchTimer_s == 1)   //Revert effect after time is up
                pSwitch_s = false;      //

            
            Image.drawText("Mouse-Grid X: " + (mausX / 16), 0, 504, Color.Red, Texture.ASCII);
            Image.drawText("Mouse-Grid Y: " + (mausY / 16), 0, 516, Color.Red, Texture.ASCII);
            Image.drawText("Mouse X: " + mausX, 0, 528, Color.Red, Texture.ASCII);
            Image.drawText("Mouse Y: " + mausY, 0, 540, Color.Red, Texture.ASCII);

            mausX = mausX / 16;
            mausY = mausY / 16;
            
            mausRadLast = mausRad;
            mausRad = maus.Wheel;
            int mausRadDelta = mausRad - mausRadLast;
            
            if (currentIDArr[currentDimension, 0] < currentIDArr[currentDimension, 1] && mausRadDelta > 0)
                currentIDArr[currentDimension, 0] += mausRadDelta;
            if (currentIDArr[currentDimension, 0] > 0 && mausRadDelta < 0)
                currentIDArr[currentDimension, 0] += mausRadDelta;

            Image.drawText(getCurrentDimensionStr() + " (" + currentDimension + ")", 0, 492, Color.Red, Texture.ASCII);
            Image.drawText(" | ID " + currentIDArr[currentDimension, 0] + " / " + currentIDArr[currentDimension, 1], 204, 492, Color.Red, Texture.ASCII);
            
            switch (currentDimension)
            {
                case 0: ; break;    //COllision
                case 1:
                    if (Texture.bgTilesArray[currentIDArr[currentDimension, 0], 1] == 1)
                        Image.drawImage(Texture.bgTilesArray[currentIDArr[currentDimension, 0],0], mausX * 16, mausY * 16);   //Background
                    if (Texture.bgTilesArray[currentIDArr[currentDimension, 0], 1] > 1)
                        Image.drawTileFrame(Texture.bgTilesArray[currentIDArr[currentDimension, 0], 0], frameDelay, Texture.bgTilesArray[currentIDArr[currentDimension, 0], 1], mausX * 16, mausY * 16);
                break;

                case 2: 
                    Image.drawTileSquare(Texture.tileSet, currentIDArr[currentDimension, 0], tileSize, mausX * 16, mausY * 16); 
                    break;  //tiles

                case 3:
                    if(sprites[currentIDArr[currentDimension, 0]]!=null)
                        Image.drawText("Id" + currentIDArr[currentDimension, 0] + " = " + sprites[currentIDArr[currentDimension, 0]].getName(), (mausX * 16), (mausY * 16) - 12, Color.White, Texture.ASCII);
                    else
                        Image.drawText("Id" + currentIDArr[currentDimension, 0]+" = Leer", (mausX * 16), (mausY * 16) - 12, Color.White, Texture.ASCII);
                    break;    //sprites

                case 4: ; break;    //Foreground
                default: break;
            }

            if ((mausX >= 0 && mausX < map.GetLength(1)) && (mausY >= 0 && mausY < map.GetLength(0))) //Mouse in range of map-array ?
            {
                if (maus[MouseButton.Left]) //Place
                {
                    map[mausY, mausX, currentDimension] = currentIDArr[currentDimension, 0];
                    if(currentDimension!=3)
                        map[mausY, mausX, 0] = colInf[currentIDArr[currentDimension, 0]];
                    //Console.WriteLine("wrote TileID " + mausRad + "to [" + mausX + "][" + mausY + "].");
                    if(currentDimension==3)
                        Thread.Sleep(180);
                }
                if (maus[MouseButton.Right]) //remove
                {
                    map[mausY, mausX, currentDimension] = -1;
                    map[mausY, mausX, 0] = 0;
                    //Console.WriteLine("removed Tile from [" + mausX + "][" + mausY + "].");
                }
            }

            if (keyboard[Key.D])
            {
                currentDimension++;
                if (currentDimension > map.GetLength(2) - 1)
                    currentDimension = 0;
                Thread.Sleep(150);
            }


            if (keyboard[Key.F11])               // F11
            { spriteAdd(new Firebar(mausX*16,mausY*16)); Thread.Sleep(150); }     // Spawn something with End (Ende) Key

            if (keyboard[Key.F12])               // +
            { RootThingy.debugInfo = !RootThingy.debugInfo; Thread.Sleep(150); }

            if (keyboard[Key.PageUp])               // +
            { temp += 0.01; Thread.Sleep(20); }     // A temp
            if (keyboard[Key.PageDown])             // Variable
            { temp -= 0.01; Thread.Sleep(20); }     // -

            Image.drawText("Temp: " + Math.Round(temp,3), 0, 612, Color.Red, Texture.ASCII);

            Image.endDraw2D();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// DRAW
        public void draw()
        {            
            Image.beginDraw2D();
            double tileX = 0;
            double tileY = 0;
            int tileID;
            int x = 0;
            int y = 0;
            int arrX = 0;
            int arrY = 0;
            int animatedCounter = 0;

            //Render Background
            if (Image.activeTexture != Texture.backGround)
                GL.BindTexture(TextureTarget.Texture2D, Texture.backGround);
            Image.activeTexture = Texture.backGround;

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(1, 1, 1, 1.0f);
            GL.TexCoord2(0.0, 0.0); //Top left
            GL.Vertex2(0, 0 );

            GL.TexCoord2((sceneX / (double)bgW), 0.0);  //Top right
            GL.Vertex2(sceneX, 0 );

            GL.TexCoord2((sceneX / (double)bgW), 1.0);  //Bottom right
            GL.Vertex2(sceneX, sceneY);
            
            GL.TexCoord2(0.0, 1.0); //Bottom left
            GL.Vertex2(0, sceneY);
            GL.End();

            while (arrY < map.GetLength(0))
            {
                while (arrX < map.GetLength(1))
                {
                    if (map[arrY, arrX, 3] > -1)
                    {
                        //Console.WriteLine("arrY=" + arrY + " | arrX=" + arrX);
                        switch (map[arrY, arrX, 3])    //Sprite-ID
                        {
                            //spriteArray.Add()
                            //spriteArray.Add(new BaseObj(0, 0, 1, 1));
                            case 000: break;
                            case 001: spriteAdd(new Player(arrX * 16, arrY * 16)); break;
                            case 002: spriteAdd(new Boo(arrX * 16, arrY * 16)); break;
                            case 003: spriteAdd(new Goomba(arrX * 16, arrY * 16, false, (short)rnd.Next(1, 4))); break;
                            case 004: spriteAdd(new Koopa_green(arrX * 16, arrY * 16)); break;
                            case 005: spriteAdd(new Koopa_green(arrX * 16, arrY * 16, false, 1, 2)); break;  //jumping
                            case 006: spriteAdd(new Koopa_green(arrX * 16, arrY * 16, false, 1, 2)); break;  //high jumping
                            case 007: spriteAdd(new Koopa_red(arrX * 16, arrY * 16)); break;
                            case 008: spriteAdd(new Koopa_red(arrX * 16, arrY * 16, false, 1, 2)); break;  //jumping
                            case 009: spriteAdd(new Koopa_red(arrX * 16, arrY * 16, false, 1, 2)); break;  //high jumping
                            case 010: spriteAdd(new Bulletbill(arrX * 16, arrY * 16, false, (short)rnd.Next(1, 5))); break;
                            case 011: spriteAdd(new Bulletbill(arrX * 16, arrY * 16, false, 3, true)); break;   //homing version (only type 3 !!!)
                            case 012: spriteAdd(new Buzzybeetle(arrX * 16, arrY * 16, false)); break;
                            case 013: spriteAdd(new Spiny(arrX * 16, arrY * 16, false, 1, false)); break;  //Spiny
                            case 014: spriteAdd(new Spiny(arrX * 16, arrY * 16, false, 1, true)); break;    //Spiny-Egg
                            case 015: spriteAdd(new Piranhaplant(arrX * 16, arrY * 16, 1)); break;    //Green Piranha Plant
                            case 016: spriteAdd(new Piranhaplant(arrX * 16, arrY * 16, 2)); break;    //Red Piranha Plant
                            case 017: spriteAdd(new Hammerbros(arrX * 16, arrY * 16)); break; 

                            case 100: spriteAdd(new Bricks(arrX * 16, arrY * 16, (short)rnd.Next(1, 5))); break;
                            case 101: spriteAdd(new Pow(arrX * 16, arrY * 16)); break;
                            case 102: spriteAdd(new Qm(arrX * 16, arrY * 16)); break;
                            case 103: spriteAdd(new Coin(arrX * 16, arrY * 16, (short)rnd.Next(1, 6))); break;
                            case 104: spriteAdd(new Mushroom(arrX * 16, arrY * 16, false, (short)rnd.Next(1, 3))); break;
                            case 105: spriteAdd(new Mushroom_p(arrX * 16, arrY * 16)); break;
                            case 106: spriteAdd(new Fireflower(arrX * 16, arrY * 16, (short)rnd.Next(1, 4))); break;
                            case 107: spriteAdd(new Starman(arrX * 16, arrY * 16, false, (short)rnd.Next(1, 5))); break;
                            case 108: spriteAdd(new OneUp(arrX * 16, arrY * 16)); break;
                            case 109: spriteAdd(new ThreeUp(arrX * 16, arrY * 16)); break;
                            case 110: spriteAdd(new PSwitch_b(arrX * 16, arrY * 16)); break;
                            case 111: spriteAdd(new Spring_p(arrX * 16, arrY * 16)); break;
                            case 112: spriteAdd(new Cannon(arrX * 16, arrY * 16)); break;
                            case 113: spriteAdd(new Keyy(arrX * 16, arrY * 16, (short)rnd.Next(1, 3))); break;

                            case 114: spriteAdd(new Lava(arrX * 16, arrY * 16)); break;
                            case 115: spriteAdd(new Potaboo(arrX * 16, arrY * 16)); break;

                            case 300: spriteAdd(new Fireballshot(0, 0, new BaseObj(0, 0))); map[arrY, arrX, 3] = 0; break;

                            default: Console.WriteLine("Invalid Sprite-ID: " + map[arrY, arrX, 3] + " @[" + arrX + "][" + arrY + "] "); break; 
                        }
                        map[arrY, arrX, 3] = -1; //so we don't spawn everything every frame again and again...
                     }
                    arrX++;
                    if (arrX == map.GetLength(1))
                    {
                        arrY++;
                        arrX = 0;
                        //Console.Write("\n");
                    }
                    if (arrY == map.GetLength(0))
                        break;
                }
            }
            arrY = 0;
            arrX = 0;

            while (arrY < map.GetLength(0))
            {
                while (arrX < map.GetLength(1))
                {
                    if (map[arrY, arrX, 1] > -1)
                    {
                        if (Texture.bgTilesArray[map[arrY, arrX, 1], 1] == 1)
                        { Image.drawImage(Texture.bgTilesArray[map[arrY, arrX, 1], 0], arrX * 16, arrY * 16); }  //BG-Object-ID
                        if (Texture.bgTilesArray[map[arrY, arrX, 1], 1] > 1)
                        {
                            Image.drawTileFrame(Texture.bgTilesArray[map[arrY, arrX, 1], 0], frameDelay, Texture.bgTilesArray[map[arrY, arrX, 1], 1], arrX * 16, arrY * 16);
                            animatedCounter++;
                        }
                    }
                    arrX++;
                    if (arrX == map.GetLength(1))
                    {
                        arrY++;
                        arrX = 0;
                        //Console.Write("\n");
                    }
                    if (arrY == map.GetLength(0))
                        break;
                }
            }
            Image.drawText("Animated Objects: " + animatedCounter, 0, 552, Color.Red, Texture.ASCII);
            Image.drawText("Sprites: " + spriteArrMax, 0, 564, Color.Red, Texture.ASCII);

            Image.drawText("P-Switch b timer: " + pSwitchTimer_b, 0, 576, Color.Blue, Texture.ASCII);
            Image.drawText("P-Switch s timer: " + pSwitchTimer_s, 0, 588, Color.Silver, Texture.ASCII);

            if (Image.activeTexture != Texture.tileSet)
                GL.BindTexture(TextureTarget.Texture2D, Texture.tileSet);
            Image.activeTexture = Texture.tileSet;
            GL.Begin(PrimitiveType.Quads);
            arrY = 0;
            arrX = 0;            
            while (arrY < map.GetLength(0))
            {
                while (arrX < map.GetLength(1))
                {
                    tileID = map[arrY, arrX, 2];  //Block-ID
                    if (tileID > -1)
                    {
                        if (tileID * tileSize <= tileSetW * tileSetH)
                            tileX = tileID * tileSize;
                        tileY = 0;
                        if (tileID * tileSize >= tileSetW)
                        {
                            tileX = (tileID * tileSize) - ((tileID * tileSize) / tileSetW) * tileSetW;
                            tileY = ((tileID * tileSize) / tileSetW) * tileSize;
                        }
                        x = arrX * 16;
                        y = arrY * 16;
                        GL.Color4(1, 1, 1, 1.0f);
                        GL.TexCoord2(tileX / tileSetW, tileY / tileSetH);   // Top Left
                        GL.Vertex2(x, y);                                   //
                        GL.TexCoord2((tileX + tileSize) / tileSetW, tileY / tileSetH);  // Top Right
                        GL.Vertex2(x + tileSize, y);                                    //
                        GL.TexCoord2((tileX + tileSize) / tileSetW, (tileY + tileSize) / tileSetH); // Lower Right
                        GL.Vertex2(x + tileSize, y + tileSize);                                     //
                        GL.TexCoord2(tileX / tileSetW, (tileY + tileSize) / tileSetH);  // Lower left
                        GL.Vertex2(x, y + tileSize);                                    //
                    }
                    arrX++;
                    if (arrX == map.GetLength(1))
                    {
                        arrY++;
                        arrX = 0;
                        //Console.Write("\n");

                    }
                    if (arrY == map.GetLength(0))
                        break;
                }
            }

            GL.End();
            Image.drawImage(Texture.tileSet, RootThingy.sceneX, 0);
            //drawImage(ASCII, 0, 480);
            //for (int ii = 0; ii < 256; ii++)
            //{drawText((Convert.ToChar(ii).ToString()), (ii*8), 492, Color.Red);}

            Image.drawText(("Max texture Size: " + maxTextureSize), 0, 480, Color.Aqua, Texture.ASCII);

            Image.endDraw2D();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ADD_A_SPRITE
        public static int spriteAdd(BaseObj spriteObj)
        {
            bool done = false;
            int i = 0;
            while (!done && i<=spriteArray.Count())
            {
                if (spriteArray[i] == null)
                {
                    spriteArray[i] = spriteObj;
                    done = true;
                    if (i > spriteArrMax)
                        spriteArrMax = i;
                }
                else
                {
                    Console.WriteLine("spriteAdd("+spriteArray[i].getName()+")");
                    //Console.ReadKey();
                    i++;
                }
            }
            return i;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// RETURN_A_RANDOM_BOOL-Value
        public static bool rndBool()
        {
            return (rnd.Next(2) == 0);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// GET_CURRENT_DIMENSION
        public int getCurrentDimension()
        { return currentDimension; }
        
        public string getCurrentDimensionStr()
        {
            switch(currentDimension)
            {
                case 0: return "Collision";
                case 1: return "Background";
                case 2: return "Tiles";
                case 3: return "Sprites";
                case 4: return "Foreground";
                default: return "Invalid Dimension";
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// SET_CURRENT_DIMENSION
        public bool setCurrentDimension(int dimension)
        {
            if (dimension > 0 && dimension < map.GetLength(2))
            {
                currentDimension = dimension;
                return true;
            }
            else
                return false;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// GET_TILE_ID_NOW
        public int getTileIDNow()
        { return currentIDArr[currentDimension, 0]; }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// GET_BG_DATA
        public int getBgData(int x, int y)
        {
            if ((x > 0 && x < map.GetLength(1) - 1) && (y > 0 && y < map.GetLength(0) - 1)) //check if position is valid
                return map[y, x, 1];
            else
                return -1;  //error
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// SET_BG_DATA
        public bool setBgData(int x, int y, int bgData)
        {
            if ((x > 0 && x < map.GetLength(1) - 1) && (y > 0 && y < map.GetLength(0) - 1)) //check if position is valid
            {
                map[y, x, 1] = bgData;
                return true;
            }
            else
                return false;
        }
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// GET_TILE_ID
        public int getTileID(int x, int y)
        {
            if ((x > 0 && x < map.GetLength(1) - 1) && (y > 0 && y < map.GetLength(0) - 1)) //check if position is valid
                return map[y, x, 2];
            else
                return -1;  //error
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// SET_TILE_ID
        public bool setTileID(int x, int y, int id)
        {
            if ((id > -1 && id <= currentIDArr[2,1]) &&                                             //check if ID is valid
                (x > 0 && x < map.GetLength(1) - 1) && (y > 0 && y < map.GetLength(0) - 1)) //check if position is valid
            {
                map[y, x, 2] = id;
                map[y, x, 0] = colInf[id];
                return true;
            }
            else
                return false;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// GET_COL_DATA
        public int getColData(int x, int y)
        {
            if ((x > 0 && x < map.GetLength(1) - 1) && (y > 0 && y < map.GetLength(0) - 1)) //check if position is valid
                return map[y, x, 0];
            else
                return -1;  //error
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// SET_COL_DATA
        public bool setColData(int x, int y, int colInf)
        {
            if ((colInf > -1 && colInf <= 2) &&                                             //check if Collision-ID is valid
                (x > 0 && x < map.GetLength(1) - 1) && (y > 0 && y < map.GetLength(0) - 1)) //check if position is valid
            {
                map[y, x, 0] = colInf;
                return true;
            }
            else
                return false;
        }


    }
}
