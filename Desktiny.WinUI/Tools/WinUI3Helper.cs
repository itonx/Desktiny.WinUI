using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace Desktiny.WinUI.Tools
{
    public static class WinUI3Helper
    {
        public static DependencyObject FindChildElementByName(DependencyObject tree, string sName)
        {
            for (
                int i = 0;
                i < Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(tree);
                i++
            )
            {
                DependencyObject child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(tree, i);
                if (child != null && ((FrameworkElement)child).Name == sName)
                    return child;
                else
                {
                    DependencyObject childInSubtree = FindChildElementByName(child, sName);
                    if (childInSubtree != null)
                        return childInSubtree;
                }
            }
            return null;
        }

        public static T FindParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            // Get the immediate parent node
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // End of the tree reached
            if (parentObject == null)
                return null;

            // Check if the parent matches the type we are looking for
            if (parentObject is T parent)
            {
                return parent;
            }
            else
            {
                // Recursively travel further up the tree
                return FindParent<T>(parentObject);
            }
        }
    }
}
