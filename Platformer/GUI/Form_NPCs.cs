using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Game.GUI
{
    class Form_NPCs
    {
        private int spriteId = 0;
        public Form_NPCs()
        {
            Form npcForm = new Form();
            npcForm.ShowInTaskbar = false;
            npcForm.Text = "NPCs";  //The text in the Title
            npcForm.SetBounds(12, 12, 256, 256);

            Button b1 = new Button();
            npcForm.Controls.Add(b1);
            b1.Text = "b1";
            b1.SetBounds(2, 2, 16, 16);
            b1.Click += new System.EventHandler(b_click);
            npcForm.Show();
        }

        private void b_click(Object sender, EventArgs e)
        { 
            switch(((Button)sender).Text)
            {
                case "b1": spriteId = 18; break;
        }
        }



    }
}
