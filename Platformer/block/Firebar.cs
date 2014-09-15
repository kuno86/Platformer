using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Firebar : BaseObj
    {
        private double angle,adder;
        private List<int> segmentObj=new List<int>();
        private bool spawned = false;
        private bool clockwise = true;
        private short segments;
        public Firebar(double x, double y, short segments = 12, bool clockwise = true)
            : base(x, y)
        {
            this.despawnOffScreen = false;
            this.name = "Firebar";
            this.texture = Texture.smb1_firebarBlock;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.segments = segments;
            this.clockwise = clockwise;
            angle = 0;
            if (clockwise)
                adder = -0.03;
            else
                adder = 0.03;
            Console.WriteLine("Firebar Segments:");
            
            //Console.ReadKey();
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            if (!spawned)
            {
                for (int i = 1; i != segments; i++)
                {
                    segmentObj.Add(Map.spriteAdd(new Firebar_segment((x * i) + 4, y + 4, clockwise)));
                    Console.WriteLine(segmentObj[i - 1]);
                }
                spawned = true;
            }

            angle+=adder;
            for (int i = 0; i < segmentObj.Count; i++)
            {
                if (Map.spriteArray[segmentObj[i]] == null)
                    segmentObj.RemoveAt(i);
                else
                {
                    Map.spriteArray[segmentObj[i]].x = Math.Sin(angle) * 8 * i + x + 4;
                    Map.spriteArray[segmentObj[i]].y = Math.Cos(angle) * 8 * i + y + 4;
                }
            }

            Image.drawImage(texture, x, y);
            //Image.drawImage(texture, Math.Sin(angle)*10+x,Math.Cos(angle)*10+y);
        }



    }
}



