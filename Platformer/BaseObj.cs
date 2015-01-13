using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Threading;


namespace Game
{
    class BaseObj
    {
        public struct Point
        {
            public double x;
            public double y;
        }                                                           /////////////////////////////////////////
                                                                    //put refreshColRect() in every process()
        public struct ColRect                                       /////////////////////////////////////////
        {
            public double x;
            public double y;
            public int w;
            public int h;
        }
        public short colOffsetX;
        public short colOffsetY;

        public short colType = 0;   ///0=Bounding box, 1=Point/circle, 3=bitmask

        public int colCntSprites = 0;
        public int colTop, colBottom, colRight, colLeft;
        public ColRect colRect;
        public bool onGround = false;
        //public bool inWater = false;

        public short renderOrder = 0;   // Negative(-) = Background, 0 = Middle, Positive(+) = Foreground
        public bool renderEnabled = true;   //if the object is rendered (!!! this alone wont turn off the Objects AI !!!)
        public bool AIEnabled = true;   //if the object will just sit there and does nothing (It still renders if 'renderEnabled' is true)

        public bool grabable = false;
        public bool blockTop = false;   //Acts like a solid block, stuff can land and stay on it
        public bool blockSides = false;
        public bool climbable = false;

        public bool colWithBlocks = true;
        public bool colWithOthers = false;

        protected double xDecel = 0.0625;

        public bool despawnOffScreen = true;

        public static int idCount=0;
        public int id;
        public static bool message = false;
        public static int messageId; 
                
        public Dictionary<string, string> metaData = new Dictionary<string, string>();

        public short type;
        public string name = "Base";
        public int texture;
        public bool dir;    //false=right ; true=left
        public double x;
        public double y;
        public short w;
        public short h;
        public double xVel;
        public double yVel;

        public BaseObj(double x, double y, short w, short h, short colW=16, short colH=16, short colOffsetX=0, short colOffsetY=0)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.colOffsetX = colOffsetX;
            this.colOffsetY = colOffsetY;
            colRect.x = x + colOffsetX;
            colRect.y = y + colOffsetY;
            colRect.w = colW;
            colRect.h = colH;
            //id = idCount;
            //idCount++;
        }

        public BaseObj DeepCopySprite(int id)
        {
            return (BaseObj)Map.sprites[id].MemberwiseClone();
        }

        public BaseObj(double x, double y)
        {
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colOffsetX = 0;
            this.colOffsetY = 0;
            colRect.x = x + colOffsetX;
            colRect.y = y + colOffsetY;
            colRect.w = w;
            colRect.h = h;
            //id = idCount;
            //idCount++;
        }

        public BaseObj()
        { ;}
      
        public virtual string getName()
        {return name;}

        //public void process()
        //{
        //    if (AIEnabled)
        //        doSubAI();
        //    if (renderEnabled)
        //        doRender();
        //}

        public void doAI()
        {
            var keyboard = Keyboard.GetState();
            if (keyboard[Key.Pause])
            { message = false; }
            if (!message)
                doSubAI();
            else
                Map.spriteArray[messageId].showMessage();   //Stop all AI while Messagebox is beeing read
        }

        public virtual void doSubAI()
        {;}

        public virtual void doRender()
        { ;}

        //////////////////////////////Water-Check
        protected bool inWater()
        {
            for (int i = 0; i <= Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null && Map.spriteArray[i].name == "WaterArea")
                {
                    if (getCol2Obj(this.colRect, Map.spriteArray[i].colRect))
                        return true;
                }
            }
            return false;
        }

        protected void showMessage(string msg ="")
        {
            if (!this.metaData.ContainsKey("Message"))
                return;
            else
            {
                if(msg=="")
                    msg = this.metaData["Message"];
                short boxW = 3 + 4 + (27 * 8) + 4 + 3;
                int boxX = (RootThingy.sceneX / 2) - (boxW / 2);
                int boxY = RootThingy.sceneY / 4;
                int drawTextY = boxY + 3 + 4;
                string[] msgP = msg.Split('¶');
                int boxH = 3 + 4 + (msgP.Count()*12) + 4 + 3;

                MyImage.endDraw2D();
                GL.Begin(PrimitiveType.Quads);
                GL.Color3(System.Drawing.Color.Black);
                GL.Vertex2(boxX, boxY);
                GL.Vertex2(boxX+boxW, boxY);
                GL.Vertex2(boxX+boxW, boxY+boxH);
                GL.Vertex2(boxX, boxY+boxH);
                GL.End();
                GL.Begin(PrimitiveType.Quads);
                GL.Color3(System.Drawing.Color.White);
                GL.Vertex2(boxX+1, boxY+1);
                GL.Vertex2(boxX + boxW-1, boxY+1);
                GL.Vertex2(boxX + boxW-1, boxY + boxH-1);
                GL.Vertex2(boxX+1, boxY + boxH-1);
                GL.End();
                GL.Begin(PrimitiveType.Quads);
                GL.Color3(System.Drawing.Color.Aqua);
                GL.Vertex2(boxX + 2, boxY + 2);
                GL.Vertex2(boxX + boxW - 2, boxY + 2);
                GL.Vertex2(boxX + boxW - 2, boxY + boxH - 2);
                GL.Vertex2(boxX + 2, boxY + boxH - 2);
                GL.End();
                MyImage.beginDraw2D();

                foreach( string s in msgP)
                {
                    MyImage.drawText(s, boxX + 2 + 4, drawTextY, System.Drawing.Color.White, Texture.ASCII);
                    drawTextY += 12;
                }
            }
            if (!message)
                message = true;
            messageId = this.id;
            
        }

        public void setMessage(string message)
        {
            string[] msgP = message.Split(' ');
            string line = "";
            string convMsg = "";
            for (int i = 0; i <= msgP.Length-1;i++ )
            {
                msgP[i] += " ";
                if ((line + msgP[i]).Length <= 27)
                { line += msgP[i]; }
                else
                {
                    convMsg += line + "¶";
                    line = msgP[i];
                }
            }
            convMsg += line;
            if (this.metaData.ContainsKey("Message"))
                this.metaData["Message"] = convMsg;
            else
                this.metaData.Add("Message", convMsg);
        }

        public virtual void setXY(double x, double y)
        { 
            this.x = x; 
            this.y = y; 
        }

        public void setXYVel(double xVel, double yVel)
        {
            this.xVel = xVel;
            this.yVel = yVel;
        }

        

        ///////////////////////////////////////////Return Collision Value for a Block on Map/Sprite
        public void getColGrid()
        {
            colTop = 0;
            colBottom = 0;
            colRight = 0;
            colLeft = 0;
            if (despawnOffScreen)
            {
                /////////////////////////////////////////////////For Block collision
                if (colWithBlocks)
                {
                    try
                    {
                        if (Map.map[(int)(colRect.y / 16), (int)(colRect.x / 16), 0] == 1 || Map.map[(int)(colRect.y / 16), (int)((colRect.x + Map.tileSize - 1) / 16), 0] == 1)  //ceiling-Collisions
                            colTop = 1;           //

                        if (Map.map[(int)((colRect.y + colRect.h) / 16), (int)(colRect.x / 16), 0] == 1 || Map.map[(int)((colRect.y + colRect.h) / 16), (int)((colRect.x + Map.tileSize - 1) / 16), 0] == 1 || //Floor-Collisions (Solids-check)
                            Map.map[(int)((colRect.y + colRect.h) / 16), (int)(colRect.x / 16), 0] == 2 || Map.map[(int)((colRect.y + colRect.h) / 16), (int)((colRect.x + Map.tileSize - 1) / 16), 0] == 2)  //Floor-Collisions (clouds-check)
                            colBottom = 1;

                        if (Map.map[(int)(colRect.y / 16), (int)((colRect.x + colRect.w) / 16), 0] == 1 || Map.map[(int)((colRect.y + Map.tileSize) / 16), (int)((colRect.x + colRect.w) / 16), 0] == 1)  //Right Walls Collsion
                            colRight = 1;

                        if (Map.map[(int)(colRect.y / 16), (int)(colRect.x / 16), 0] == 1 || Map.map[(int)((colRect.y + Map.tileSize) / 16), (int)(colRect.x / 16), 0] == 1)  //Left Wall Collision
                            colLeft = 1;
                    }
                    catch { ; }
                }
                /////////////////////////////////////////////////For sprites with collision
                if (colWithOthers)
                {
                    
                    for (int i = 0; i != Map.spriteArrMax; i++)
                    {
                        if (Map.spriteArray[i] != null)
                        {
                            
                            if (Map.spriteArray[i].colWithOthers)       //////
                            {                                           //////

                                if (getCol2Obj(Map.spriteArray[i].colRect, this.colRect))
                                {
                                    if (Map.spriteArray[i].blockTop)
                                    {
                                        if (this.colRect.y + this.colRect.h + this.yVel <= Map.spriteArray[i].colRect.y + Map.spriteArray[i].yVel)
                                        {
                                            colBottom = 1;
                                        }
                                    }

                                    if (Map.spriteArray[i].blockSides)
                                    {
                                        if (this.colRect.x <= Map.spriteArray[i].colRect.x + Map.spriteArray[i].colRect.w)
                                            colRight = 1;
                                        if (this.colRect.x + this.colRect.w >= Map.spriteArray[i].colRect.x)
                                            colLeft = 1;
                                    }
                                    
                                }
                            }
                        }
                    }
                }






            }
        }

        ///////////////////////////////////////////Return Collision Value for a Pixel on Map
        protected int getColXY(int x, int y)
        {
            int temp = 0;
            if (colWithOthers)
            {
                for (int i = 0; i != Map.spriteArrMax; i++)
                {
                    if (Map.spriteArray[i] != null)
                    {

                        if (Map.spriteArray[i].colWithOthers)       //////
                        {                                           //////

                            if ((x >= Map.spriteArray[i].colRect.x && Map.spriteArray[i].colRect.x + Map.spriteArray[i].colRect.w >= x) && (y >= Map.spriteArray[i].colRect.y && Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h >=y))
                            {
                                if (Map.spriteArray[i].blockTop)
                                {
                                    temp = 1;
                                }
                            }
                        }
                    }
                }
            }
            if (temp == 1)
            {
                return temp;
            }

            if ((y < Map.map.GetLength(0) * Map.tileSize) && (x < Map.map.GetLength(1) * Map.tileSize) && (x >= 0) && (y >= 0))
                return Map.map[(int)y / 16, (int)x / 16, 0];
            else
                return 1;

        }


        ///////////////////////////////////////////Check if two BaseObj's collide with each other
        public bool getCol2Obj(ColRect obj1, ColRect obj2)
        {
            if ((obj1.x + obj1.w >= obj2.x && obj1.x <= obj2.x + obj2.w) && (obj1.y + obj1.h >= obj2.y && obj1.y <= obj2.y + obj2.h))
                return true;
            else
                return false;
        }

        protected int toInt(double z)
        {return Convert.ToInt32(z); }

        public void refreshColRect()
        {
            colRect.x = x + colOffsetX;
            colRect.y = y + colOffsetY;
        }

       

    }
}
