using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace moneysender
{
    public class ControlUI
    {
        private List<UIElement> _UIElementsFirstWindow = new List<UIElement>();
        private List<UIElement> _UIElementsSecondWindow = new List<UIElement>();
        private List<UIElement> _UIElementsThirdWindow = new List<UIElement>();
        private List<UIElement> _UIElementsLastWindow = new List<UIElement>();
        public void AddRangeUISFirstWindow(UIElement[] elements)
        {
            _UIElementsFirstWindow.AddRange(elements);
        }
        public void AddRangeUISecondWindow(UIElement[] elements)
        {
            _UIElementsSecondWindow.AddRange(elements);
        }
        public void AddRangeUIThirdWindow(UIElement[] elements)
        {
            _UIElementsThirdWindow.AddRange(elements);
        }
        public void AddRangeUILastWindow(UIElement[] elements)
        {
            _UIElementsLastWindow.AddRange(elements);
        }
        public void Show(int whatTheNumberWindow)
        {
            if (whatTheNumberWindow == 1)
            {
                foreach (UIElement element in _UIElementsFirstWindow)
                {
                    element.Visibility = Visibility.Visible;
                }
            }
            else if (whatTheNumberWindow == 2)
            {
                foreach (UIElement element in _UIElementsSecondWindow)
                {
                    element.Visibility = Visibility.Visible;
                }
            }
            else if (whatTheNumberWindow == 3)
            {
                foreach (UIElement element in _UIElementsThirdWindow)
                {
                    element.Visibility = Visibility.Visible;
                }
            }
            else if (whatTheNumberWindow == 4)
            {
                foreach (UIElement element in _UIElementsLastWindow)
                {
                    element.Visibility = Visibility.Visible;
                }
            }
        }
        public void Hide(int whatTheNumberWindow)
        {
            if (whatTheNumberWindow == 1)
            {
                foreach (UIElement element in _UIElementsFirstWindow)
                {
                    element.Visibility = Visibility.Hidden;
                }
            }
            else if (whatTheNumberWindow == 2)
            {
                foreach (UIElement element in _UIElementsSecondWindow)
                {
                    element.Visibility = Visibility.Hidden;
                }
            }
            else if (whatTheNumberWindow == 3)
            {
                foreach (UIElement element in _UIElementsThirdWindow)
                {
                    element.Visibility = Visibility.Hidden;
                }
            }
            else if (whatTheNumberWindow == 4)
            {
                foreach (UIElement element in _UIElementsLastWindow)
                {
                    element.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
