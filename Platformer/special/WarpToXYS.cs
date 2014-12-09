using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Game
{
    class WarpToXYS : BaseObj
    {

        private short frame = 0;
        private short frameDelay = 0;
        private Bitmap warp_icons = (Bitmap)System.Drawing.Image.FromFile(RootThingy.exePath + @"\gfx\sprites\technical\warp-icons_8x8.bmp");
        private double targetX, targetY;
        private short section, type, direction;
        private bool isEntrance;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="targetX">X-Position (optional section)</param>
        /// <param name="targetY">Y-Position (optional section)</param>
        /// <param name="section">Target-Section of the Map</param>
        /// <param name="type">0=Pipe, 1=Door, 2=Instant</param>
        /// <param name="isEntrance">True=Entrance, False=Exit</param>
        /// <param name="direction">0=Moving Right, 1=Moving Down, 2=Moving Left, 3=Moving Up</param>
        public WarpToXYS(double x, double y, double targetX, double targetY, short section = 0, short type = 0, bool isEntrance = true, short direction = 0)
            : base(x, y)
        {
            this.name = "WarpToXYS";
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
            RectangleF ziel = new RectangleF(0, yOffset, 8, 8);


            if (!isEntrance)
            {
                quelle.X += 32; //Exit icons are offset by 32 pixels
                ziel.Y += 8;
            }
            g.DrawImage(warp_icons, ziel, quelle, GraphicsUnit.Pixel);

            g.DrawRectangle(p, tmpRect);
            //render.Save(@"C:\test.bmp");
            this.texture = MyImage.LoadTexture((Bitmap)render);
            this.targetX = targetX;
            this.targetY = targetY;
            this.section = section;
            this.type = type;
            this.direction = direction;
            this.isEntrance = isEntrance;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colWithBlocks = false;
            this.colWithOthers = false;
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
                                    Map.spriteArray[i].setXY(targetX, targetY);
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

