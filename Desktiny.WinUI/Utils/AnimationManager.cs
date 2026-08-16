using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;

namespace Desktiny.WinUI.Utils
{
    public class AnimationManager
    {
        private IList<Storyboard> _storyboards = new List<Storyboard>();

        public AnimationManager AddStoryboard(Storyboard storyboard)
        {
            var lastStoryBoard = _storyboards.LastOrDefault();

            if (lastStoryBoard != null)
            {
                lastStoryBoard.Completed += (object? sender, object e) =>
                {
                    storyboard.Begin();
                };
            }

            _storyboards.Add(storyboard);
            return this;
        }

        public void BeginOnLoaded(FrameworkElement triggerControl)
        {
            if (!_storyboards.Any() || triggerControl == null)
                return;

            triggerControl.Loaded += (s, args) =>
            {
                _storyboards.First().Begin();
            };
        }

        public static AnimationManager CreateInstance()
        {
            return new AnimationManager();
        }
    }
}
