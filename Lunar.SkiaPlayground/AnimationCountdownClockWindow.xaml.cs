using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lunar.SkiaPlayground
{
    /// <summary>
    /// Interaction logic for AnimationCountdownClockWindow.xaml
    /// </summary>
    public partial class AnimationCountdownClockWindow : Window
    {
        public AnimationCountdownClockWindow()
        {
            InitializeComponent();

            StartAnimation(60);
        }

        private void StartAnimation(int inputSeconds)
        {
            double from = -90;
            double to = 270;
            int seconds = inputSeconds;
            TimeSpan duration = TimeSpan.FromSeconds(inputSeconds);

            DoubleAnimation animation = new DoubleAnimation(from, to, new Duration(duration));

            Storyboard.SetTarget(animation, Arc);
            Storyboard.SetTargetProperty(animation, new PropertyPath("EndAngle"));

            Storyboard storyboard = new Storyboard();
            storyboard.CurrentTimeInvalidated += (s, e) =>
            {
                int diff = (int)(s as ClockGroup).CurrentTime.Value.TotalSeconds;
                inputSeconds = seconds - diff;
            };

            storyboard.Children.Add(animation);
            storyboard.Begin();
        }
    }
}
