using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageHandle
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ResourceDictionary resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri("/Resources/Dictionary1.xaml", UriKind.Relative);
            DrawingBrush brush = (DrawingBrush)resourceDictionary[Save];
            _openFileDialog.Filter = "图像文件(*.bmp, *.jpg)|*.bmp;*.jpg";
        }

        OpenFileDialog _openFileDialog=new OpenFileDialog();
        string _filepath=null;
        Bitmap bitmap =null;
        Bitmap newbitmap = null;

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_filepath);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (_openFileDialog.ShowDialog()==true)
            {
                _filepath= _openFileDialog.FileName;
                bitmap?.Dispose();
            }
            bitmap= (Bitmap)Bitmap.FromFile(_filepath);
            imageSource.Source = ChangeBitmapToImageSource(bitmap);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        #region 图像处理
        private void DarkCorner_Click(object sender, RoutedEventArgs e)
        {
            if (bitmap == null)
            {
                return;
            }
            newbitmap?.Dispose();
            newbitmap = ImageHandleHelp.DarkCorner(bitmap);
            if(newbitmap!=null)
                ImageTarget.Source = ChangeBitmapToImageSource(newbitmap);
        }

        private void Brightness_Click(object sender, RoutedEventArgs e)
        {
            newbitmap?.Dispose();
            newbitmap = ImageHandleHelp.Brightness(bitmap);
            if (newbitmap != null)
                ImageTarget.Source = ChangeBitmapToImageSource(newbitmap);
        }

        private void De_Color_Click(object sender, RoutedEventArgs e)
        {
            if (bitmap == null)
            {
                return;
            }
            newbitmap?.Dispose();
            newbitmap = ImageHandleHelp.De_Color(bitmap);
            if (newbitmap != null)
                ImageTarget.Source = ChangeBitmapToImageSource(newbitmap);
        }
        private void Gray_Click(object sender, RoutedEventArgs e)
        {
            if (bitmap == null)
            {
                return;
            }
            newbitmap?.Dispose();
            newbitmap = ImageHandleHelp.Gray(bitmap);
            if(newbitmap!=null)
                ImageTarget.Source = ChangeBitmapToImageSource(newbitmap);
        }
        private void Macais_Click(object sender, RoutedEventArgs e)
        {
            if (bitmap == null)
            {
                return;
            }
            newbitmap?.Dispose();
            newbitmap = ImageHandleHelp.Masaic(bitmap);
            if (newbitmap != null)
                ImageTarget.Source = ChangeBitmapToImageSource(newbitmap);
        }

        private void BlackImage_Click(object sender, RoutedEventArgs e)
        {
            if (bitmap == null)
            {
                return;
            }
            newbitmap?.Dispose();
            newbitmap = new Bitmap(bitmap.Width, bitmap.Height);
            if (newbitmap != null)
                ImageTarget.Source = ChangeBitmapToImageSource(newbitmap);
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            if (bitmap == null)
            {
                return;
            }
            newbitmap?.Dispose();
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(bitmap.Width/4,bitmap.Height/4,bitmap.Width/2,bitmap.Height/2);
            newbitmap = ImageHandleHelp.CutBitmap(bitmap, rectangle);
            if (newbitmap != null)
                ImageTarget.Source = ChangeBitmapToImageSource(newbitmap);
        }

        private void Fill_Click(object sender, RoutedEventArgs e)
        {
            if (bitmap == null)
            {
                return;
            }
            newbitmap?.Dispose();
            if (_openFileDialog.ShowDialog() == true)
            {
                _filepath = _openFileDialog.FileName;
            }
            Bitmap sourceBitmap = new Bitmap(_filepath);

            newbitmap = ImageHandleHelp.FillBitmap(sourceBitmap,bitmap, new System.Drawing.Point(sourceBitmap.Width/2,0));
            if (newbitmap != null)
                ImageTarget.Source = ChangeBitmapToImageSource(newbitmap);
            sourceBitmap?.Dispose();

        }
        #endregion

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static ImageSource ChangeBitmapToImageSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return wpfBitmap;
        }
    }

}
