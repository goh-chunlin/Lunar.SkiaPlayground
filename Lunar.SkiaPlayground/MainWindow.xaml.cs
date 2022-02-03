﻿using Lunar.SkiaPlayground.Models;
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
        private bool _isDraggingCursor;
        private readonly List<Circle> _circles = new();
        private List<List<Dot>> _paths = new();

        const float strokeWidth = 10;
        static readonly float[] dashArray = { 0, 2 * strokeWidth };

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

            foreach (var dots in _paths) 
            {
                if (dots.Count == 0) continue;

                SKPath path = new();

                path.MoveTo(dots.First().X, dots.First().Y);

                for (int i = 1; i < dots.Count; i++) 
                {
                    path.LineTo(dots[i].X, dots[i].Y);
                }

                SKPaint strokePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = dots.First().BorderColor,
                    StrokeCap = SKStrokeCap.Square,
                    StrokeWidth = 6
                };

                SKPathEffect translatePathEffect = SKPathEffect.Create1DPath(
                    SKPath.ParseSvgPathData("M -10 -10 L 10 -10, 10 10, -10 10 Z"),
                    24, 0, SKPath1DPathEffectStyle.Translate);

                SKPathEffect rotatePathEffect = SKPathEffect.Create1DPath(
                    SKPath.ParseSvgPathData("M -10 0 L 0 -10, 10 0, 0 10 Z"),
                    20, 0, SKPath1DPathEffectStyle.Rotate);

                SKPathEffect morphPathEffect = SKPathEffect.Create1DPath(
                    SKPath.ParseSvgPathData("M -25 -10 L 25 -10, 25 10, -25 10 Z"),
                    55, 0, SKPath1DPathEffectStyle.Morph);

                // Animate the phase; t is 0 to 1 every second
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks);
                float t = (float)(timeSpan.TotalSeconds % 1 / 1);
                float phase = -t * 2 * strokeWidth;

                //// Create dotted line effect based on dash array and phase
                //using (SKPathEffect dashEffect = SKPathEffect.CreateDash(dashArray, phase))
                //{
                //    // Set it to the paint object
                //    strokePaint.PathEffect = dashEffect;
                //}

                strokePaint.PathEffect = morphPathEffect;

                canvas.DrawPath(path, strokePaint);
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var cursorPosition = e.GetPosition(this);
            _isDraggingCursor = true;

            var dpiScale = VisualTreeHelper.GetDpi(this);

            var dpiScaleX = dpiScale.DpiScaleX;
            var dpiScaleY = dpiScale.DpiScaleY;

            var rand = new Random();

            var color = new SKColor((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));

            _circles.Add(new Circle()
            {
                CenterX = (float)(cursorPosition.X * dpiScaleX),
                CenterY = (float)(cursorPosition.Y * dpiScaleY),
                Radius = rand.NextSingle() * 200,
                BorderColor = color,
            });

            var firstDot = new Dot(cursorPosition.X * dpiScaleX, cursorPosition.Y * dpiScaleX);
            firstDot.BorderColor = color;

            _paths.Add(new List<Dot> { firstDot } );

            Canvas.InvalidateVisual();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDraggingCursor) 
            {
                var cursorPosition = e.GetPosition(this);

                var dpiScale = VisualTreeHelper.GetDpi(this);

                var dpiScaleX = dpiScale.DpiScaleX;
                var dpiScaleY = dpiScale.DpiScaleY;

                _paths.Last().Add(new Dot(cursorPosition.X * dpiScaleX, cursorPosition.Y * dpiScaleX));

                Canvas.InvalidateVisual();
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDraggingCursor = false;

            Canvas.InvalidateVisual();
        }
    }
}
