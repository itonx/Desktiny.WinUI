using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media.Animation;

namespace Desktiny.WinUI.Utils
{
    public static class Animations
    {
        public static Storyboard CreateFadeInOut(
            UIElement control,
            string targetProperty,
            float from = 0.0f,
            float to = 1.0f,
            double durationInMs = 1000,
            EasingMode easingMode = EasingMode.EaseInOut
        )
        {
            var storyboard = new Storyboard();
            var doubleAnimation = CreateFadeAnimation(
                control,
                targetProperty,
                from,
                to,
                durationInMs,
                easingMode
            );
            storyboard.Children.Add(doubleAnimation);

            return storyboard;
        }

        public static void AddFadeAnimation(
            this Storyboard storyboard,
            UIElement control,
            string targetProperty,
            float from = 0.0f,
            float to = 1.0f,
            double durationInMs = 1000,
            EasingMode easingMode = EasingMode.EaseInOut,
            bool autoReverse = false,
            double beginTimeInMs = 0
        )
        {
            var doubleAnimation = CreateFadeAnimation(
                control,
                targetProperty,
                from,
                to,
                durationInMs,
                easingMode,
                autoReverse,
                beginTimeInMs
            );
            storyboard.Children.Add(doubleAnimation);
        }

        public static DoubleAnimation CreateFadeAnimation(
            UIElement control,
            string targetProperty,
            float from = 0.0f,
            float to = 1.0f,
            double durationInMs = 1000,
            EasingMode easingMode = EasingMode.EaseInOut,
            bool autoReverse = false,
            double beginTimeInMs = 0
        )
        {
            var doubleAnimation = new DoubleAnimation();

            Storyboard.SetTarget(doubleAnimation, control);
            Storyboard.SetTargetProperty(doubleAnimation, targetProperty);

            doubleAnimation.BeginTime = TimeSpan.FromMilliseconds(beginTimeInMs);
            doubleAnimation.AutoReverse = autoReverse;
            doubleAnimation.From = from;
            doubleAnimation.To = to;
            doubleAnimation.Duration = new Microsoft.UI.Xaml.Duration(
                TimeSpan.FromMilliseconds(durationInMs)
            );
            doubleAnimation.EasingFunction = new QuadraticEase() { EasingMode = easingMode };

            return doubleAnimation;
        }
    }
}
