using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lunar.SkiaPlayground.Models
{
    internal class Arc : Shape
    {
        public Point Center
        {
            get => (Point)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        // Using a DependencyProperty as the backing store for Center.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(Point), typeof(Arc),
                new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsRender));

        // Start angle in degrees
        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        // End angle in degrees
        public double EndAngle
        {
            get => (double)GetValue(EndAngleProperty);
            set => SetValue(EndAngleProperty, value);
        }

        // Using a DependencyProperty as the backing store for EndAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc),
                new FrameworkPropertyMetadata(90.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(Arc),
                new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool SmallAngle
        {
            get => (bool)GetValue(SmallAngleProperty);
            set => SetValue(SmallAngleProperty, value);
        }

        // Using a DependencyProperty as the backing store for SmallAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SmallAngleProperty =
            DependencyProperty.Register("SmallAngle", typeof(bool), typeof(Arc),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        static Arc() => DefaultStyleKeyProperty.OverrideMetadata(typeof(Arc), new FrameworkPropertyMetadata(typeof(Arc)));

        protected override Geometry DefiningGeometry
        {
            get
            {
                double startAngleRadians = StartAngle * Math.PI / 180;
                double endAngleRadians = EndAngle * Math.PI / 180;

                double a0 = StartAngle < 0 ? startAngleRadians + 2 * Math.PI : startAngleRadians;
                double a1 = EndAngle < 0 ? endAngleRadians + 2 * Math.PI : endAngleRadians;

                if (a1 < a0)
                    a1 += Math.PI * 2;

                SweepDirection d = SweepDirection.Counterclockwise;
                bool large;

                if (SmallAngle)
                {
                    large = false;
                    double t = a1;
                    d = (a1 - a0) > Math.PI ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;
                }
                else
                    large = (Math.Abs(a1 - a0) < Math.PI);

                Point p0 = Center + new Vector(Math.Cos(a0), Math.Sin(a0)) * Radius;
                Point p1 = Center + new Vector(Math.Cos(a1), Math.Sin(a1)) * Radius;

                List<PathSegment> segments = new List<PathSegment>
            {
                new ArcSegment(p1, new Size(Radius, Radius), 0.0, large, d, true)
            };

                List<PathFigure> figures = new List<PathFigure>
            {
                new PathFigure(p0, segments, true)
                {
                    IsClosed = false
                }
            };

                return new PathGeometry(figures, FillRule.EvenOdd, null);
            }
        }
    }
}
