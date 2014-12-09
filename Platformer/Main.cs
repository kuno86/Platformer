using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing.Imaging;
using System.Threading;

namespace Game
{
    class RootThingy
    {
        public static int windowX = 1280;
        public static int windowY = 688;
        public static string exePath = Environment.CurrentDirectory;
        public static int sceneX = 640;
        public static int sceneY = 480;
        public static bool debugInfo=true;
        public static bool zoomed = false;

        public static double mouseX;
        public static double mouseY;
        public static int mouseWheel;
        public static KeyboardState keyboard;

        public static Random rnd = new Random();

        public struct Point
        {
            public double x;
            public double y;
        }

        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [STAThread]
        public static void Main()
        {
            System.Diagnostics.Process.GetCurrentProcess().ProcessorAffinity = (System.IntPtr) (System.Environment.ProcessorCount); //Assign the process to the last CPU-core of the System
            Console.WriteLine("We have " + System.Environment.ProcessorCount + " CPU-Cores.");
            Random rnd = new Random();
            using (var game = new GameWindow())
            {
                game.Load += (sender, e) =>
                {
                    // setup settings, load textures, sounds
                    FormWindow();
                    game.X = 32;
                    game.Y = 16;
                    game.VSync = VSyncMode.On;
                    game.Width = windowX;
                    game.Height = windowY;
                    game.WindowBorder = WindowBorder.Fixed; //Disables the resizable windowframe
                    GL.Enable(EnableCap.Blend);                                                     //These lines
                    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);  //enable transparency using alpha-channel
                    
                    //////////////////////////////////////////////////////////////////////////////////////////////// Load all Background-Objects from the Folder
                    int i = 0;
                    foreach (string bmpFile in Directory.GetFiles((exePath + @"\gfx\backgrounds\tiles"), "*.bmp"))  //get all background tiles and load them
                    {
                        Texture.bgTilesArray[i, 0] = MyImage.LoadTexture(bmpFile);
                        string framesCount = (bmpFile.Replace((exePath + @"\gfx\backgrounds\tiles\"), "").Substring(4, 2));   //get Animation FrameCount ( "<id><id><id>_<frameCount><frameCount>.bmp")
                        if (!int.TryParse(framesCount, out Texture.bgTilesArray[i, 1]))
                            Texture.bgTilesArray[i, 1] = 1;
                        i++;
                    }
                    foreach (string bmpFile in Directory.GetFiles((exePath + @"\gfx\backgrounds\tiles"), "*.png"))  //get all background tiles and load them
                    {
                        Texture.bgTilesArray[i, 0] = MyImage.LoadTexture(bmpFile);
                        string framesCount = (bmpFile.Replace((exePath + @"\gfx\backgrounds\tiles\"), "").Substring(4, 2));   //get Animation FrameCount ( "<id><id><id>_<frameCount><frameCount>.bmp")
                        if (!int.TryParse(framesCount, out Texture.bgTilesArray[i, 1]))
                            Texture.bgTilesArray[i, 1] = 1;
                        i++;
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////

                };

                

                Map karte = new Map(16); 

                game.Resize += (sender, e) =>
                {
                    //sceneX = game.Height;
                    //sceneY = game.Width;

                    
                    GL.Viewport(0, 0, windowX, windowY);  //unZoomed
                    
                    
                };
                var mouse = Mouse.GetState();
                game.UpdateFrame += (sender, e) =>
                {
                    // add game logic, input handling
                    mouse = Mouse.GetState();
                    
                    keyboard = Keyboard.GetState();

                    if (keyboard[Key.Escape])
                    {
                        game.Exit();
                    }
                                        
                    game.Title = (("FPS: " + (int)(game.RenderFrequency) +" ; "+ Math.Round(game.RenderTime*1000,2)+"ms/frame  Zoomed="+zoomed));
                    
                    
                    Map.fpsLine.Insert(0,(int)game.RenderFrequency);
                    while (Map.fpsLine.Count > 230)
                        Map.fpsLine.RemoveAt(Map.fpsLine.Count - 1);

                };
                
                game.RenderFrame += (sender, e) =>
                {
                    // render graphics
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();

                    //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

                    if (zoomed)
                    {
                        GL.Viewport(0,(int) -374, (int)(sceneX * 4.17), (int)(sceneY * 2.78)); //2x Zoomed

                        game.Width = sceneX * 2+320;
                        game.Height = sceneY * 2;
                        GL.Ortho(0, windowX, windowY, 0, -1000, 1000);  //Render  distant objects smaller
                        GL.MatrixMode(MatrixMode.Modelview);
                        karte.draw();    ///////////////////////////////////////// MAP-Object
                        karte.process((int)(game.Mouse.X / 2.084), (int)(game.Mouse.Y / 1.937));
                    }
                    else
                    {
                        GL.Viewport(0, 0, windowX, windowY);  //unZoomed
                        game.Width = windowX;
                        game.Height = windowY;
                        GL.Ortho(0, windowX, windowY, 0, -1000, 1000);  //Render  distant objects smaller
                        GL.MatrixMode(MatrixMode.Modelview);
                        karte.draw();    ///////////////////////////////////////// MAP-Object
                        karte.process(game.Mouse.X, game.Mouse.Y);
                    }
                    

                    //GL.Ortho((int)sceneX, (int)sceneX * 2, (int)sceneY * 2, (int)sceneY, -1000, 1000);    


                    
                    
                                        
                    game.SwapBuffers();
                };

                // Run the game at 60 updates per second
                game.Run(199.0);
            }
        }

        public static void FormWindow()
        {
            Form npcForm = new Form();
            npcForm.ShowInTaskbar = false;
            npcForm.Text = "NPCs";  //The text in the Title
            npcForm.SetBounds(12, 12, 256, 256);

            Button b1 = new Button();
            b1.Text = "Lakitu";
            npcForm.Controls.Add(b1);
            b1.SetBounds(2, 2, 16, 16);
            npcForm.Show();

        }

//End of Root =============================================================

    }
}