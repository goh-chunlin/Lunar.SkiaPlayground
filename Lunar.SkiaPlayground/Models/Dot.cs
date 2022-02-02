using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunar.SkiaPlayground.Models
{
    internal class Dot
    {
        public float X { get; set; }

        public float Y { get; set; }

        public SKColor BorderColor { get; set; } = SKColors.Black;

        public Dot(double x, double y)
        {
            X = (float)x;
            Y = (float)y;
        }
    }
}
