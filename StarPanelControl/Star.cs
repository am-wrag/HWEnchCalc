using System.Windows;
using HWEnchCalc.Common;

namespace HWEnchCalc.StarPanelControl
{
    public class Star : NotifyPropertyChangedBase
    {
        private readonly int _visionRangeStart;
        private readonly int _visionRangeEnd;

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

        public Star(int visionRangeStart, int visionRangeEnd)
        {
            _visionRangeStart = visionRangeStart;
            _visionRangeEnd = visionRangeEnd;
        }

        public void UpdateVision(int starCount)
        {
            Visibility = (starCount >= _visionRangeStart && starCount <= _visionRangeEnd)
                ? Visibility.Visible
                : Visibility.Hidden;
        }
    }
}
