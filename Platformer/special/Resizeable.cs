using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Resizeable : BaseObj
    {

        private short frame = 0;
        private short frameDelay = 0;
        private short ww, hh;

        public Resizeable(double x, double y, short wBlocks, short hBlocks, int texture)
            : base(x, y, (short)(16 * wBlocks), (short)(16 * hBlocks))
        {
            this.name = "Resizeable";
            this.texture = texture;
            this.texture = Texture.resizeable001;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.ww = wBlocks;
            this.hh = hBlocks;
            if (ww > 1)
                ww--;
            if (hh > 1)
                hh--;
            this.w = (short)(16 * wBlocks);
            this.h = (short)(16 * hBlocks);
            this.colRect.x = x;
            this.colRect.y = y;
            this.colRect.w = w;
            this.colRect.h = h;
            this.colWithOthers = true;
            this.colWithBlocks = true;

            for (int yy = 0; yy <= (hh); yy++)
            {
                for (int xx = 0; xx <= (ww); xx++)
                {
                    if (xx == 0 && yy == 0)
                        Map.map[(int)((y/16)+yy), (int)((x/16)+xx), 0] = 1;
                    if ((xx > 0 && xx < ww) && (yy == 0))
                        Map.map[(int)((y / 16) + yy), (int)((x / 16) + xx), 0] = 1;
                    if ((xx == ww) && (yy == 0))
                        Map.map[(int)((y / 16) + yy), (int)((x / 16) + xx), 0] = 1;

                    if (xx == 0 && (yy > 0 && yy < hh))
                        Map.map[(int)((y / 16) + yy), (int)((x / 16) + xx), 0] = 1;
                    if ((xx > 0 && xx < ww) && (yy > 0 && yy < hh))
                        Map.map[(int)((y / 16) + yy), (int)((x / 16) + xx), 0] = 0;
                    if ((xx == ww) && (yy > 0 && yy < hh))
                        Map.map[(int)((y / 16) + yy), (int)((x / 16) + xx), 0] = 1;

                    if (xx == 0 && yy == hh)
                        Map.map[(int)((y / 16) + yy), (int)((x / 16) + xx), 0] = 1;
                    if ((xx > 0 && xx < ww) && (yy == hh))
                        Map.map[(int)((y / 16) + yy), (int)((x / 16) + xx), 0] = 1;
                    if ((xx == ww) && (yy == hh))
                        Map.map[(int)((y / 16) + yy), (int)((x / 16) + xx), 0] = 1;
                }
            }

            Console.WriteLine("W: " + ww + "H: " + hh);
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
        }

        public override void doRender()
        {
            for (int yy = 0; yy <= (hh); yy++)
            {
                for (int xx = 0; xx <= (ww); xx++)
                {
                    if(xx == 0 && yy == 0)
                        MyImage.drawTileFromXY(texture, 0, 0, 16, x + (xx * 16), y + (yy * 16));
                    if ((xx > 0 && xx < ww) && (yy == 0))
                        MyImage.drawTileFromXY(texture, 1, 0, 16, x + (xx * 16), y + (yy * 16));
                    if ((xx == ww) && (yy == 0))
                        MyImage.drawTileFromXY(texture, 2, 0, 16, x + (xx * 16), y + (yy * 16));

                    if (xx == 0 && (yy > 0 && yy < hh))
                        MyImage.drawTileFromXY(texture, 0, 1, 16, x + (xx * 16), y + (yy * 16));
                    if ((xx > 0 && xx < ww) && (yy > 0 && yy < hh))
                        MyImage.drawTileFromXY(texture, 1, 1, 16, x + (xx * 16), y + (yy * 16));
                    if ((xx == ww) && (yy > 0 && yy < hh))
                        MyImage.drawTileFromXY(texture, 2, 1, 16, x + (xx * 16), y + (yy * 16));

                    if (xx == 0 && yy == hh)
                        MyImage.drawTileFromXY(texture, 0, 2, 16, x + (xx * 16), y + (yy * 16));
                    if ((xx > 0 && xx < ww) && (yy == hh))
                        MyImage.drawTileFromXY(texture, 1, 2, 16, x + (xx * 16), y + (yy * 16));
                    if ((xx == ww) && (yy == hh))
                        MyImage.drawTileFromXY(texture, 2, 2, 16, x + (xx * 16), y + (yy * 16));
                }
            }


        }
    }
}
