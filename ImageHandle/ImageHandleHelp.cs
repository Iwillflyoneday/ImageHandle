using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageHandle
{
    public class ImageHandleHelp
    {
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 暗角
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap DarkCorner(Bitmap bitmap)
        {
            if (bitmap==null)
            {
                return null;
            }
            Bitmap newBitmap = bitmap.Clone() as Bitmap;
            if (bitmap != null)
            {
                int height = newBitmap.Height;
                int width = newBitmap.Width;
                float centerX = width / 2;
                float centerY = height / 2;
                float maxdis = centerX * centerX + centerY * centerY;
                float rate;
                Color color;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        rate =1- ((i - centerX) * (i - centerX) + (j - centerY) * (j - centerY))/maxdis;
                        color = newBitmap.GetPixel(i, j);
                        newBitmap.SetPixel(i, j, Color.FromArgb((int)(color.R * rate), (int)(color.G * rate), (int)(color.B * rate)));
                    }
                }
            }
            return newBitmap;
        }

        /// <summary>
        /// 明亮
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Brightness(Bitmap bitmap)
        {
            if (bitmap==null)
            {
                return null;
            }
            Bitmap newBitmap = bitmap.Clone() as Bitmap;
            if (bitmap != null)
            {
                int height = newBitmap.Height;
                int width = newBitmap.Width;
                float centerX = width / 2;
                float centerY = height / 2;
                float rate=1.2F;
                Color color;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        color = newBitmap.GetPixel(i, j);
                        newBitmap.SetPixel(i, j, Color.FromArgb((int)(color.R * rate)>255?255: (int)(color.R * rate), (int)(color.G * rate) > 255 ? 255 : (int)(color.G * rate), (int)(color.B * rate) > 255 ? 255 : (int)(color.B* rate)));
                    }
                }
            }
            return newBitmap;
        }

        /// <summary>
        /// 暗化
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap De_Color(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return null;
            }
            Bitmap newBitmap = bitmap.Clone() as Bitmap;
            if (bitmap != null)
            {
                int height = newBitmap.Height;
                int width = newBitmap.Width;
                float centerX = width / 2;
                float centerY = height / 2;
                float rate = 0.6F;
                Color color;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        color = newBitmap.GetPixel(i, j);
                        newBitmap.SetPixel(i, j, Color.FromArgb((int)(color.R * rate), (int)(color.G * rate), (int)(color.B * rate)));
                    }
                }
            }
            return newBitmap;
        }

        /// <summary>
        /// 灰度值
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Gray(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return null;
            }
            Bitmap newBitmap=bitmap.Clone() as Bitmap;
            Rectangle rect = new Rectangle(0, 0, newBitmap.Width, newBitmap.Height);
            BitmapData data = newBitmap.LockBits(rect, ImageLockMode.ReadWrite, newBitmap.PixelFormat);
            IntPtr intPtr = data.Scan0;
            int length = 3 * newBitmap.Height * newBitmap.Width;
            byte[] bytes = new byte[length];
            Marshal.Copy(intPtr, bytes, 0, length);
            double temp;
            for (int i = 0; i < length; i=i+3)
            {
                temp = bytes[i + 2] *0.144 + bytes[i + 1] * 0.587 + bytes[i] * 0.299;
                bytes[i] = bytes[i + 1] = bytes[i + 2] = (byte)temp;
            }
            Marshal.Copy(bytes, 0, intPtr, length);
            newBitmap.UnlockBits(data);
            Rectangle rectangle = new Rectangle(0, 0, 0, 0);
            return newBitmap;
        }

        /// <summary>
        /// 马赛克
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Masaic(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return null;
            }
            Bitmap newBitmap=bitmap.Clone() as Bitmap;
            Rectangle rect = new Rectangle(0, 0, newBitmap.Width, newBitmap.Height);
            BitmapData data = newBitmap.LockBits(rect, ImageLockMode.ReadWrite, newBitmap.PixelFormat);
            int  red=0,green=0,blue=0;
            int count = 0, total = 0, i,j=0;

            unsafe
            {
                byte* prt = (byte*)data.Scan0,q;
                q = prt;
                int heightGap = 50 - newBitmap.Height % 50;
                int widthGap = 50 - newBitmap.Width % 50;
                for (i = 0; i< newBitmap.Height; i+=50)
                {
                    for (j = 0; j< newBitmap.Width; j+=50)
                    {
                        for (int n = 0; n < 50&& n + i < newBitmap.Height; n++)
                        {
                            for (int m = 0; m < 50&&m + j < newBitmap.Width; m++)
                            {
                                red += q[0];
                                green += q[1];
                                blue += q[2];
                                q = q + 3;
                                count++;
                                total++;
                            }
                            q = q + data.Stride - count * 3;
                            count = 0;
                        }
                        q = prt;
                        red = red / total;
                        green = green / total;
                        blue = blue / total;
                        total = 0;
                        for (int n = 0; n < 50&&n +i < newBitmap.Height; n++)
                        {
                            for (int m = 0; m < 50&& m + j< newBitmap.Width; m++)
                            {
                                q[0] = (byte)red;
                                q[1] = (byte)green;
                                q[2] = (byte)blue;
                                q += 3;
                                count++;
                            }
                            q = q + data.Stride - count * 3;
                            count = 0;
                        }
                        red = blue = green = 0;
                        prt += 150;
                        q = prt;
                    }
                    prt += data.Stride*50- data.Width * 3-widthGap*3;
                    q = prt;
                }
            }
            newBitmap.UnlockBits(data);
            return newBitmap;
        }

        /// <summary>
        /// 将sourceBitmap填充在targetBitmap中,起始位置为startPoint
        /// </summary>
        /// <param name="sourceBitmap"></param>
        /// <param name="targetBitmap"></param>
        /// <param name="startPoint"></param>
        /// <returns></returns>
        public static Bitmap FillBitmap(Bitmap sourceBitmap, Bitmap targetBitmap, Point startPoint)
        {
            if (sourceBitmap == null || targetBitmap == null)
            {
                return null;
            }
            Bitmap newBitmap = new Bitmap(targetBitmap);

            //贴上去原图位置大小
            Rectangle rect1 = new Rectangle(startPoint,new Size(sourceBitmap.Width, sourceBitmap.Height));
            //被覆盖图片的位置大小，起始位置为（0，0）
            Rectangle rect2 = new Rectangle(0,0, newBitmap.Width, newBitmap.Height);
            //相交部分
            Rectangle rect3 = Rectangle.Intersect(rect1, rect2);
            if (rect3.Width <=0 || rect3.Height <= 0)
            {
                return newBitmap;
            }

            //相交部分在原图的位置大小，用于将其裁剪下来
            Rectangle rect4 = new Rectangle(rect3.X-startPoint.X,rect3.Y-startPoint.Y,rect3.Width,rect3.Height);
            //将其裁剪下来
            Bitmap temp = sourceBitmap.Clone(rect4, sourceBitmap.PixelFormat);
            
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.DrawImage(temp, rect3.Location);
            }
            temp.Dispose();
            //BitmapData tempBitmapdata = temp.LockBits(new Rectangle(0, 0, rect3.Width, rect3.Height), ImageLockMode.ReadWrite, targetBitmap.PixelFormat);
            //int length = temp.Width * temp.Height * 3;
            //byte[] bytes = new byte[length];
            //Marshal.Copy(tempBitmapdata.Scan0, bytes, 0, length);


            //int newlength = newBitmap.Width * newBitmap.Height * 3;
            //byte[] newbytes = new byte[newlength];
            //BitmapData newBitmapData = newBitmap.LockBits(rect2, ImageLockMode.ReadWrite, newBitmap.PixelFormat);
            //Marshal.Copy(newBitmapData.Scan0, newbytes, 0, newlength);

            //int start = newBitmap.Width * rect3.X * 3;
            //for (int i = 0; i < length; i = i + 3)
            //{
            //    newbytes[start] = bytes[i];
            //    newbytes[start + 1] = bytes[i + 1];
            //    newbytes[start + 2] = bytes[i + 2];
            //    if (i % (rect3.Width * 3) == 0 && i > 0)
            //    {
            //        start = start + newBitmap.Width * 3 - rect3.Width * 3;
            //    }
            //    start = start + 3;
            //}

            //Marshal.Copy(newbytes, 0, newBitmapData.Scan0, newlength);
            //Marshal.Copy(newbytes, 0, tempBitmapdata.Scan0, length);
            //temp.UnlockBits(tempBitmapdata);
            //newBitmap.UnlockBits(newBitmapData);
            return newBitmap;
        }

        /// <summary>
        /// 剪切图片的部分
        /// </summary>
        /// <param name="bitmap1"></param>
        /// <param name="bitmap2"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Bitmap CutBitmap(Bitmap bitmap,Rectangle rect)
        {
            if (bitmap == null)
            {
                return null;
            }
            Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            Rectangle intersectRect = Rectangle.Intersect(rect, rectangle);//求出相交位置

            Bitmap newBitmap = bitmap.Clone(intersectRect, bitmap.PixelFormat);
            //Bitmap newBitmap = new Bitmap(intersectRect.Width, intersectRect.Height,bitmap.PixelFormat);

            ////处理原始图
            //BitmapData bitmapData = bitmap.LockBits(rectangle,ImageLockMode.ReadWrite,bitmap.PixelFormat);
            //IntPtr bitmapIntPtr = bitmapData.Scan0;
            //int length =rect.Height * bitmapData.Stride;
            //byte[] bytes = new byte[length];
            //Marshal.Copy(bitmapIntPtr, bytes, 0, length);//复制到数组中

            ////切割后的新图
            //BitmapData newBitmapData = newBitmap.LockBits(intersectRect, ImageLockMode.WriteOnly, newBitmap.PixelFormat);
            //IntPtr newBitmapDataIntPtr = newBitmapData.Scan0;
            //int newLength = intersectRect.Width * intersectRect.Height * 3;
            //byte[] newbytes = new byte[newLength];
            //int start = intersectRect.Y * bitmapData.Stride + intersectRect.X * 3+1;
            //for (int i = 0; i < newLength; i++)
            //{
            //    for (int j = 0; j < intersectRect.Width*3; j++)
            //    {
            //        newbytes[i] = bytes[start];
            //    }
            //    start = start + bitmapData.Stride - intersectRect.Width * 3;
            //}

            //Marshal.Copy(newbytes, 0, newBitmapDataIntPtr, length);
            //bitmap.UnlockBits(bitmapData);
            //newBitmap.UnlockBits(newBitmapData);
            return newBitmap;
        }
    }
}
