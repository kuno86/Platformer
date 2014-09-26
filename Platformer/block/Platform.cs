using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Platform : BaseObj
    {
        private short frame = 0;
        private short frameDelay = 0;
        private short size;

        /// <summary>
        /// Platform Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="size">the number of Segments (each 8Pixel)</param>
        /// <param name="startAtHalfSize">start at 16-Grid or with +8 Pixels</param>
        public Platform(double x, double y, short size = 6, bool startAtHalfSize = true)
            : base(x, y)
        {
            this.name = "Platform";
            switch (type)
            {
                case 1: this.texture = Texture.smb1_platform; break;
                default: this.texture = Texture.smb1_platform; break;
            }
            this.yVel = -0.1;
            if (x >= 0 && y >= 0)
                Map.map[(int)y / 16, (int)x / 16, 0] = 1;
            this.size = size;
            if (startAtHalfSize)
                this.x = x + 8;
            this.blockTop = true;
            this.y = y;
            this.w = (short)(8 * size);
            this.h = 8;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.type = type;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            y += yVel;
            for (int i = 0; i != this.size; i++)
            {
                Image.drawImage(texture, x + (8 * i), y);
            }

        }




    }
}

