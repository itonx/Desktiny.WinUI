using System;

namespace Desktiny.WinUI.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GlyphAttribute : Attribute
    {
        public string Icon { get; }

        public GlyphAttribute(string icon)
        {
            Icon = icon;
        }
    }
}
