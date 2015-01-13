using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Game
{
    class WarpToWarp : BaseObj
    {

        private short frame = 0;
        private short frameDelay = 0;
        private Bitmap warp_nr = (Bitmap)System.Drawing.Image.FromFile(RootThingy.exePath + @"\gfx\sprites\technical\warp-nr_4x8.bmp");
        private Bitmap warp_icons = (Bitmap)System.Drawing.Image.FromFile(RootThingy.exePath + @"\gfx\sprites\technical\warp-icons_8x8.bmp");
        private bool isEntrance;
        private short direction, type, warpId;

        /// <summary>
        /// Bla
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isEntrance">True=Entrance, False=Exit</param>
        /// <param name="direction">0=Moving Right, 1=Moving Down, 2=Moving Left, 3=Moving Up</param>
        /// <param name="type">0=Pipe, 1=Door, 2=Instant</param>
        /// <param name="warpId">entrance & exit with the same warpId get linked</param>
        public WarpToWarp(double x, double y, bool isEntrance = true, short direction = 0, short type = 2, short warpId = 0)
            : base(x, y,16,16)
        {
            //wartoWarp metaData:
            //====================
            //metaData["warpId"]        warpId int
            //metaData["isEntrance"]    isEntrance True or False    (True=Entrance, False=Exit)
            //metaData["warpType"]      type 0-2                    (0=Pipe, 1=Door, 2=Instant)
            //metaData["warpDir"]       direction 0-3               (0=Moving Right, 1=Moving Down, 2=Moving Left, 3=Moving Up)

            this.name = "WarpToWarp";
            this.metaData.Add("warpId", warpId.ToString());         //warpId int
            this.metaData.Add("isEntrance", isEntrance.ToString()); //isEntrance True or False
            this.metaData.Add("warpType", type.ToString());         //type 0-2
            this.metaData.Add("warpDir", direction.ToString());     //direction 0-3

            ImageFormat bmpFormat = ImageFormat.Bmp;
            Image render = (Image)(new Bitmap(16, 16));
            Graphics g = Graphics.FromImage(render);

            Rectangle tmpRect = new Rectangle(0, 0, 15, 8);
            SolidBrush b = new SolidBrush(Color.FromArgb(255, 0, 254));
            if (!isEntrance)
                tmpRect.Y = 8;
            g.FillRectangle(b, tmpRect);
            Pen p = new Pen(Color.FromArgb(255, 0, 254));
            tmpRect.Height = 15;
            tmpRect.Y = 0;
            g.DrawRectangle(p, tmpRect);

            int yOffset = 0;
            RectangleF quelle = new RectangleF(direction * 8, 0, 8, 8);
            RectangleF ziel = new RectangleF(0,yOffset,8,8);
            
            if (!isEntrance)
            {
                quelle.X += 32; //Exit icons are offset by 32 pixels
                ziel.Y += 8;
            }
            g.DrawImage(warp_icons,ziel,quelle,GraphicsUnit.Pixel);

            if (warpId > -1 && warpId < 10)  //One-digit number needs offset by 4
            {
                quelle.X = warpId * 4;
                quelle.Width = 4;
                ziel.X=12;
                ziel.Width = 4;
                g.DrawImage(warp_nr, ziel, quelle, GraphicsUnit.Pixel);
            }

            if (warpId > 9 && warpId < 100)  //two-digit number needs no offset
            {
                quelle.X = int.Parse(warpId.ToString().Substring(0, 1)) * 4;
                quelle.Width = 4;
                ziel.X=8;
                ziel.Width = 4;
                g.DrawImage(warp_nr, ziel, quelle, GraphicsUnit.Pixel);   //draw the left digit
                quelle.X = int.Parse(warpId.ToString().Substring(1, 1)) * 4;
                quelle.Width = 4;
                ziel.X = 12;
                ziel.Width = 4;
                g.DrawImage(warp_nr, ziel, quelle, GraphicsUnit.Pixel);   //draw the right digit
            }
            g.DrawRectangle(p, tmpRect);
            this.texture = MyImage.LoadTexture((Bitmap)render);

            this.type = type;
            this.direction = direction;
            this.isEntrance = isEntrance;
            this.direction=direction;
            this.type=type;
            this.warpId=warpId;

            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colWithBlocks = false;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();

            switch (type)
            {
                case 0: ; break;    //Pipes
                case 1: ; break;    //Doors
                case 2:
                    {            //Instant
                        for (int i = 0; i != Map.spriteArrMax; i++)
                        {
                            if (Map.spriteArray[i] != null)
                            {
                                if ((Map.spriteArray[i].colWithBlocks || Map.spriteArray[i].colWithOthers) && (getCol2Obj(colRect, Map.spriteArray[i].colRect)) && Map.spriteArray[i].id != this.id)
                                {
                                    //============================================================================================================================================
                                    for (int j = 0; j != Map.spriteArrMax; j++)
                                    {
                                        if (Map.spriteArray[j] != null)
                                        {
                                            if ((Map.spriteArray[j].name == "WarpToWarp") && (Map.spriteArray[j].metaData["warpId"] == this.metaData["warpId"]))  //is this a Warp with the same ID ?
                                            {
                                                if (Map.spriteArray[j].metaData["isEntrance"] != this.metaData["isEntrance"].ToString() && this.metaData["isEntrance"] == true.ToString())
                                                    Map.spriteArray[i].setXY(Map.spriteArray[j].x, Map.spriteArray[j].y);

                                            }

                                        }
                                    }
                                    //============================================================================================================================================


                                }

                            }
                        }
                        break;
                    }
            }
        }

        public override void doRender()
        {
            MyImage.drawImage(this.texture, x, y);
        }

    }
}

