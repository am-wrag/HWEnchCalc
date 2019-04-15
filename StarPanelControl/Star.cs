using System.Windows;
using HWEnchCalc.Common;

namespace HWEnchCalc.StarPanelControl
{
    public class Star : NotifyPropertyChangedBase
    {
        
        public Visibility Visibility
        {
            get => _starVis;
            set
            {
                _starVis = value;
                PropertyChangedByMember();
            }
        }
        private Visibility _starVis = Visibility.Hidden;

        public Star()
        {
        }

        public void Hide()
        {
            Visibility = Visibility.Hidden;
        }

        public void Show()
        {
            Visibility = Visibility.Visible;
        }
    }
}
