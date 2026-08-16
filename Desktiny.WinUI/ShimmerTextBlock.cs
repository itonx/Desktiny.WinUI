using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Desktiny.WinUI.Tools;
using Desktiny.WinUI.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktiny.WinUI
{
    public sealed partial class ShimmerTextBlock : Control
    {
        public static readonly DependencyProperty TextBlockGroupProperty =
            DependencyProperty.Register(
                "TextBlockGroup",
                typeof(ObservableCollection<TextBlock>),
                typeof(ShimmerTextBlock),
                new PropertyMetadata(null)
            );

        public ObservableCollection<TextBlock> TextBlockGroup
        {
            get { return (ObservableCollection<TextBlock>)GetValue(TextBlockGroupProperty); }
            set { SetValue(TextBlockGroupProperty, value); }
        }

        public static readonly DependencyProperty AnimationDurationMsProperty =
            DependencyProperty.Register(
                "AnimationDurationMs",
                typeof(int),
                typeof(ShimmerTextBlock),
                new PropertyMetadata(120)
            );

        public int AnimationDurationMs
        {
            get { return (int)GetValue(AnimationDurationMsProperty); }
            set { SetValue(AnimationDurationMsProperty, value); }
        }

        public static readonly DependencyProperty DelayPerCharacterMsProperty =
            DependencyProperty.Register(
                "DelayPerCharacterMs",
                typeof(int),
                typeof(ShimmerTextBlock),
                new PropertyMetadata(80)
            );

        public int DelayPerCharacterMs
        {
            get { return (int)GetValue(DelayPerCharacterMsProperty); }
            set { SetValue(DelayPerCharacterMsProperty, value); }
        }

        public static readonly DependencyProperty TextBlockStyleProperty =
            DependencyProperty.Register(
                "TextBlockStyle",
                typeof(Style),
                typeof(ShimmerTextBlock),
                new PropertyMetadata(null)
            );

        public Style TextBlockStyle
        {
            get { return (Style)GetValue(TextBlockStyleProperty); }
            set { SetValue(TextBlockStyleProperty, value); }
        }

        public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
            "From",
            typeof(double),
            typeof(ShimmerTextBlock),
            new PropertyMetadata(0.0d)
        );

        public double From
        {
            get { return (double)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
            "To",
            typeof(double),
            typeof(ShimmerTextBlock),
            new PropertyMetadata(1.0d)
        );

        public double To
        {
            get { return (double)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        public ShimmerTextBlock()
        {
            DefaultStyleKey = typeof(ShimmerTextBlock);
        }

        public Storyboard CreateShimmerTextAnimation(string text)
        {
            var storyboard = new Storyboard();
            TextBlockGroup = new ObservableCollection<TextBlock>();

            int index = 0;

            foreach (var character in text)
            {
                TextBlock tb = new TextBlock();
                tb.FontSize = this.FontSize;
                tb.Style = TextBlockStyle;

                if (character == ' ')
                {
                    tb.Text = "_";
                    tb.Opacity = 0;
                }
                else
                {
                    tb.Text = character.ToString();
                    tb.Opacity = this.Opacity;

                    storyboard.AddFadeAnimation(
                        control: tb,
                        targetProperty: "Opacity",
                        from: (float)From,
                        to: (float)To,
                        durationInMs: this.AnimationDurationMs,
                        easingMode: EasingMode.EaseInOut,
                        autoReverse: true,
                        beginTimeInMs: DelayPerCharacterMs * index
                    );
                }

                index++;
                TextBlockGroup.Add(tb);
            }

            return storyboard;
        }
    }
}
