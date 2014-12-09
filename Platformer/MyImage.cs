using System;
using System.IO;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Game
{
    public class MyImage : GameWindow
    {
        public static int activeTexture;
        private static int ASCII;
        public MyImage()
            : base(800, 600, GraphicsMode.Default, "Hoard of Upgrades")
        {
            //GL.ClearColor(0, 0.1f, 0.4f, 1);
            ASCII = LoadTexture(RootThingy.exePath + @"\gfx\tilesets\smb1nes.bmp");
        }


        public static int LoadTexture(string file)
        {
            if (!File.Exists(file))
            {
                Console.WriteLine("Texture '" + file + "' could not be loaded!");
                Console.ReadKey();
                return 0;
            }
            
            Bitmap bitmap = new Bitmap(file);
            bitmap.MakeTransparent(Color.Magenta);
            int tex;
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out tex);
            GL.BindTexture(TextureTarget.Texture2D, tex);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);



            return tex;
        }


        public static int LoadTexture(Bitmap bitmap)
        {
            
            bitmap.MakeTransparent(Color.Magenta);
            int tex;
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out tex);
            GL.BindTexture(TextureTarget.Texture2D, tex);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            return tex;
        }


        public static Bitmap BMPfromTextureID(int id)
        {
            int w, h;
            GL.Color4(1, 1, 1, 1.0f);
            GL.BindTexture(TextureTarget.ProxyTexture2D, id);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out w);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out h);

            Bitmap bmp = new Bitmap(w, h);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), 
            ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            GL.Finish();

            //GL.GetTexImage(TextureTarget.ProxyTexture2D, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, data.Scan0);
            GL.TexSubImage2D(TextureTarget.ProxyTexture2D, 0, 0, 0, w, h, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            
            bmp.UnlockBits(data);
            //bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            bmp.Save(@"C:\test.bmp");
            return bmp;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// BEGIN DRAW 2D
        public static void beginDraw2D()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, RootThingy.windowX, RootThingy.windowY, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Disable(EnableCap.Lighting);
            GL.Enable(EnableCap.Texture2D);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// END DRAW 2D
        public static void endDraw2D()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// DRAW IMAGE WITH ROTATION
        public static void drawImageRot(int image, double x, double y, double angle=0, double z=0)
        {
            int w;
            int h;
            double x2, y2;
            Game.Geometry.Point p1 = new Game.Geometry.Point();
            GL.Color4(1, 1, 1, 1.0f);
            if (activeTexture != image)
                GL.BindTexture(TextureTarget.Texture2D, image);
            activeTexture = image;

            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out w);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out h);
            
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); 
            p1=Geometry.rotate(-w/2, -h/2, angle);
            GL.Vertex3(p1.x + x, p1.y + y, z);

            GL.TexCoord2(1, 0);
            p1=Geometry.rotate(w/2, -h/2, angle);
            GL.Vertex3(p1.x + x, p1.y + y, z);

            GL.TexCoord2(1, 1);
            p1=Geometry.rotate(w/2, h/2, angle);
            GL.Vertex3(p1.x + x, p1.y + y, z);

            GL.TexCoord2(0, 1);
            p1 = Geometry.rotate(-w / 2,h/2, angle);
            GL.Vertex3(p1.x + x, p1.y + y, z);
            GL.End();

            
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// DRAW IMAGE
        public static void drawImage(int image, double x, double y, bool flipV=false, bool flipH=false, double z=0)
        {
            int w;
            int h;
            GL.Color4(1, 1, 1, 1.0f);
            if (activeTexture != image)
                GL.BindTexture(TextureTarget.Texture2D, image);
            activeTexture = image;

            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out w);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out h);

            GL.Begin(PrimitiveType.Quads);
            if(!flipV && !flipH)    //No flip
            {
                GL.TexCoord2(0, 0);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(1, 0);
                GL.Vertex3(x + w, y, z);
                GL.TexCoord2(1, 1);
                GL.Vertex3(x + w, y + h, z);
                GL.TexCoord2(0, 1);
                GL.Vertex3(x, y + h, z);
            }
            else if (flipV && !flipH)   //Vertical(X) flip 
            {
                GL.TexCoord2(1, 0);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(0, 0);
                GL.Vertex3(x + w, y, z);
                GL.TexCoord2(0, 1);
                GL.Vertex3(x + w, y + h, z);
                GL.TexCoord2(1, 1);
                GL.Vertex3(x, y + h, z);
            }
            else if (!flipV && flipH)    //Horizontal(y) flip
            {
                GL.TexCoord2(0, 1);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(1, 1);
                GL.Vertex3(x + w, y, z);
                GL.TexCoord2(1, 0);
                GL.Vertex3(x + w, y + h, z);
                GL.TexCoord2(0, 0);
                GL.Vertex3(x, y + h, z);
            }
            else if (flipV && flipH)    //Vertical(x) + Horizontal(y) flip
            {
                GL.TexCoord2(1, 1);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(0, 1);
                GL.Vertex3(x + w, y, z);
                GL.TexCoord2(0, 0);
                GL.Vertex3(x + w, y + h, z);
                GL.TexCoord2(1, 0);
                GL.Vertex3(x, y + h, z);
            }
            GL.End();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// DRAW SQUARE TILE
        public static void drawTileSquare(int image, int tileID, int tileSize, double x, double y, bool flipV = false, bool flipH = false, double z=0)
        {
            int w;
            int h;
            if (activeTexture != image)
                GL.BindTexture(TextureTarget.Texture2D, image);
            activeTexture = image;
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out w);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out h);

            double tileX = 0;
            double tileY = 0;
            if (tileID * tileSize <= w * h)
                tileX = tileID * tileSize;
            tileY = 0;
            if (tileID * tileSize >= w)
            {
                tileX = (tileID * tileSize) - ((tileID * tileSize) / w) * w;
                tileY = ((tileID * tileSize) / w) * tileSize;
            }

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(1, 1, 1, 1.0f);
            if (!flipV && !flipH)    //No flip
            {
                GL.TexCoord2(tileX / w, tileY / h);
                GL.Vertex3(x, y, z);
                GL.TexCoord2((tileX + tileSize) / w, tileY / h);
                GL.Vertex3(x + tileSize, y, z);
                GL.TexCoord2((tileX + tileSize) / w, (tileY + tileSize) / h);
                GL.Vertex3(x + tileSize, y + tileSize, z);
                GL.TexCoord2(tileX / w, (tileY + tileSize) / h);
                GL.Vertex3(x, y + tileSize, z);
            }
            else if (flipV && !flipH)   //Vertical(X) flip 
            {
                GL.TexCoord2((tileX + tileSize) / w, tileY / h);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(tileX / w, tileY / h);
                GL.Vertex3(x + tileSize, y, z);
                GL.TexCoord2(tileX / w, (tileY + tileSize) / h);
                GL.Vertex3(x + tileSize, y + tileSize, z);
                GL.TexCoord2((tileX + tileSize) / w, (tileY + tileSize) / h);
                GL.Vertex3(x, y + tileSize, z);
            }
            else if (!flipV && flipH)    //Horizontal(y) flip
            {
                GL.TexCoord2(tileX / w, (tileY + tileSize) / h);
                GL.Vertex3(x, y, z);
                GL.TexCoord2((tileX + tileSize) / w, (tileY + tileSize) / h);
                GL.Vertex3(x + tileSize, y, z);
                GL.TexCoord2((tileX + tileSize) / w, tileY / h);
                GL.Vertex3(x + tileSize, y + tileSize, z);
                GL.TexCoord2(tileX / w, tileY / h);
                GL.Vertex3(x, y + tileSize, z);
            }
            else if (flipV && flipH)    //Vertical(x) + Horizontal(y) flip
            {
                GL.TexCoord2((tileX + tileSize) / w, (tileY + tileSize) / h);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(tileX / w, (tileY + tileSize) / h);
                GL.Vertex3(x + tileSize, y, z);
                GL.TexCoord2(tileX / w, tileY / h);
                GL.Vertex3(x + tileSize, y + tileSize, z);
                GL.TexCoord2((tileX + tileSize) / w, tileY / h);
                GL.Vertex3(x, y + tileSize, z);
            }
            GL.End();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// DRAW TILE
        public static void drawTileFrame(int image, int frameID, int frameCount, double x, double y, bool flipV = false, bool flipH = false, double z=0)
        {
            int w;
            int h;
            if (activeTexture != image)
                GL.BindTexture(TextureTarget.Texture2D, image);
            activeTexture = image;
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out w);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out h);

            while (frameID > frameCount - 1)
            {
                frameID -= frameCount;
            }
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(1, 1, 1, 1.0f);

            if (!flipV && !flipH)    //No flip
            {
                GL.TexCoord2((frameID) / (double)frameCount, 0);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(((frameID + 1) / (double)frameCount), 0);
                GL.Vertex3(x + (w / frameCount), y, z);
                GL.TexCoord2(((frameID + 1) / (double)frameCount), 1);
                GL.Vertex3(x + (w / frameCount), y + h, z);
                GL.TexCoord2((frameID) / (double)frameCount, 1);
                GL.Vertex3(x, y + h, z);
            }
            else if (flipV && !flipH)   //Vertical(X) flip 
            {
                GL.TexCoord2((frameID+1) / (double)frameCount, 0);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(((frameID) / (double)frameCount), 0);
                GL.Vertex3(x + (w / frameCount), y, z);
                GL.TexCoord2(((frameID) / (double)frameCount), 1);
                GL.Vertex3(x + (w / frameCount), y + h, z);
                GL.TexCoord2((frameID+1) / (double)frameCount, 1);
                GL.Vertex3(x, y + h, z);
            }
            else if (!flipV && flipH)    //Horizontal(y) flip
            {
                GL.TexCoord2((frameID) / (double)frameCount, 1);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(((frameID + 1) / (double)frameCount), 1);
                GL.Vertex3(x + (w / frameCount), y, z);
                GL.TexCoord2(((frameID + 1) / (double)frameCount), 0);
                GL.Vertex3(x + (w / frameCount), y + h, z);
                GL.TexCoord2((frameID) / (double)frameCount, 0);
                GL.Vertex3(x, y + h, z);
            }
            else if (flipV && flipH)    //Vertical(x) + Horizontal(y) flip
            {
                GL.TexCoord2((frameID + 1) / (double)frameCount, 1);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(((frameID) / (double)frameCount), 1);
                GL.Vertex3(x + (w / frameCount), y, z);
                GL.TexCoord2(((frameID) / (double)frameCount), 0);
                GL.Vertex3(x + (w / frameCount), y + h, z);
                GL.TexCoord2((frameID + 1) / (double)frameCount, 0);
                GL.Vertex3(x, y + h, z);
            }
            GL.End();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// DRAW TEXT
        public static void drawText(string text, int x, int y, Color textColor, int image, double z=0)
        {
            char[] textArr = new char[text.Length];
            textArr = text.ToCharArray();
            int w;
            int h;

            GL.Color4(textColor);
            if (activeTexture != image)
                GL.BindTexture(TextureTarget.Texture2D, image);
            activeTexture = image;
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out w);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out h);

            GL.Begin(PrimitiveType.Quads);
            foreach (char character in textArr)
            {
                GL.TexCoord2(((int)character) / 256.0, 0);
                GL.Vertex3(x, y, z);
                GL.TexCoord2((((int)character + 1) / 256.0), 0);
                GL.Vertex3(x + 8, y, z);
                GL.TexCoord2((((int)character + 1) / 256.0), 1);
                GL.Vertex3(x + 8, y + h, z);
                GL.TexCoord2(((int)character) / 256.0, 1);
                GL.Vertex3(x, y + h, z);
                x += 8;
            }
            GL.End();
        }



    }

}