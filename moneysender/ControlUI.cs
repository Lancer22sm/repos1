using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace moneysender
{
    public class ControlUI
    {
        private List<UIElement> _UIElementsFirstWindow = new List<UIElement>();
        private List<UIElement> _UIElementsSecondWindow = new List<UIElement>();
        private List<UIElement> _UIElementsThirdWindow = new List<UIElement>();
        private List<UIElement> _UIElementsLastWindow = new List<UIElement>();
        public void AddRangeUIWindow(UIElement[] elementsFirst,
                                     UIElement[] elementsSecond,
                                     UIElement[] elementsThird,
                                     UIElement[] elementsLast)
        {
            _UIElementsFirstWindow.AddRange(elementsFirst);
            _UIElementsSecondWindow.AddRange(elementsSecond);
            _UIElementsThirdWindow.AddRange(elementsThird);
            _UIElementsLastWindow.AddRange(elementsLast);
        }
        public void Show(int whatTheNumberWindow)
        {
            switch (whatTheNumberWindow)
            {
                case 1:
                    foreach (UIElement element in _UIElementsFirstWindow)
                    {
                        element.Visibility = Visibility.Visible;
                    }
                    break;
                case 2:
                    foreach (UIElement element in _UIElementsSecondWindow)
                    {
                        element.Visibility = Visibility.Visible;
                    }
                    break;
                case 3:
                    foreach (UIElement element in _UIElementsThirdWindow)
                    {
                        element.Visibility = Visibility.Visible;
                    }
                    break;
                case 4:
                    foreach (UIElement element in _UIElementsLastWindow)
                    {
                        element.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }
        public void Hide(int whatTheNumberWindow)
        {
            switch (whatTheNumberWindow)
            {
                case 1:
                    foreach (UIElement element in _UIElementsFirstWindow)
                    {
                        element.Visibility = Visibility.Hidden;
                    }
                    break;
                case 2:
                    foreach (UIElement element in _UIElementsSecondWindow)
                    {
                        element.Visibility = Visibility.Hidden;
                    }
                    break;
                case 3:
                    foreach (UIElement element in _UIElementsThirdWindow)
                    {
                        element.Visibility = Visibility.Hidden;
                    }
                    break;
                case 4:
                    foreach (UIElement element in _UIElementsLastWindow)
                    {
                        element.Visibility = Visibility.Hidden;
                    }
                    break;
            }
        }
    }
}
