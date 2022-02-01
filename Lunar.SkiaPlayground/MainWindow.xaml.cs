using Lunar.SkiaPlayground.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

namespace Lunar.SkiaPlayground
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Circle> _circles = new List<Circle>();

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Canvas_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            foreach (Circle circle in _circles)
            {
                SKPaint paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = circle.BorderColor,
                    StrokeWidth = 2
                };

                canvas.DrawCircle(circle.CenterX, circle.CenterY, circle.Radius, paint);
            }

            //// Create the path
            //SKPath path = new SKPath();

            //// Define the first contour
            //path.MoveTo(0.5f * info.Width, 0.1f * info.Height);
            //path.LineTo(0.2f * info.Width, 0.4f * info.Height);
            //path.LineTo(0.8f * info.Width, 0.4f * info.Height);
            //path.LineTo(0.5f * info.Width, 0.1f * info.Height);

            //// Define the second contour
            //path.MoveTo(0.5f * info.Width, 0.6f * info.Height);
            //path.LineTo(0.2f * info.Width, 0.9f * info.Height);
            //path.LineTo(0.8f * info.Width, 0.9f * info.Height);
            ////path.Close();

            //// Create two SKPaint objects
            //SKPaint strokePaint = new SKPaint
            //{
            //    Style = SKPaintStyle.Stroke,
            //    Color = SKColors.Magenta,
            //    StrokeWidth = 50
            //};

            //canvas.DrawPath(path, strokePaint);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var cursorPosition = e.GetPosition(this);

            var dpiScale = VisualTreeHelper.GetDpi(this);

            var dpiScaleX = dpiScale.DpiScaleX;
            var dpiScaleY = dpiScale.DpiScaleY;

            var rand = new Random();

            var color = new SKColor((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));

            _circles.Add(new Circle() { 
                CenterX = (float)(cursorPosition.X * dpiScaleX), 
                CenterY = (float)(cursorPosition.Y * dpiScaleY),
                Radius = rand.NextSingle() * 200,
                BorderColor = color,
            });

            Canvas.InvalidateVisual();
        }
    }
}
