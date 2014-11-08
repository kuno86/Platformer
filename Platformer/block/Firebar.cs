using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Game
{
    class Firebar : BaseObj
    {
        private double angle,adder,angleDeg;
        private List<int> segmentObj=new List<int>();
        private bool spawned = false;
        private bool clockwise = true;
        private short segments;
        public Firebar(double x, double y, short segments = 6, bool clockwise = true, short startAngle=0)
            : base(x, y)
        {
            this.despawnOffScreen = false;
            this.name = "Firebar";
            this.texture = Texture.smb1_firebarBlock;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            if (x >= 0 && y >= 0)
                Map.map[(int)y / 16, (int)x / 16, 0] = 1;
            this.segments = segments;
            this.clockwise = clockwise;
            angle = startAngle + 180;
            if (clockwise)
                adder = 0.03;
            else
                adder = -0.03;
            this.colWithBlocks = false;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            angle += adder;

            angleDeg = angle / 0.0174532925; //0.002777778;
            while (angleDeg > 360)
                angleDeg -= 360;
            while (angleDeg < -360)
                angleDeg += 360;
            //Image.drawText(Math.Round(angleDeg).ToString(), (int)x, (int)y + 48, Color.White, Texture.ASCII);

            if (!spawned)
            {
                for (int i = 1; i != segments; i++)
                {
                    segmentObj.Add(Map.spriteAdd(new Firebar_segment((Math.Cos(angle) * x * i) + 4,Math.Sin(angle)* y + 4, clockwise)));
                }
                spawned = true;
            }
                        
            for (int i = 0; i < segmentObj.Count; i++)
            {
                if (Map.spriteArray[segmentObj[i]] == null)
                    segmentObj.RemoveAt(i);
                else
                {
                    Map.spriteArray[segmentObj[i]].x = Math.Cos(angle) * 8 * i + x + 4;
                    Map.spriteArray[segmentObj[i]].y = Math.Sin(angle) * 8 * i + y + 4;
                }
            }

            Image.drawImage(texture, x, y);
        }



    }
}



