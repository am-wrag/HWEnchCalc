using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using HWEnchCalc.Common;

namespace HWEnchCalc.StarPanelControl
{
    public class StarPanelViewModel : NotifyPropertyChangedBase
    {
        public Star Star1 { get; set; } = new Star {Visibility = Visibility.Visible};
        public Star Star2 { get; set; } = new Star();
        public Star Star3 { get; set; } = new Star();
        public Star Star4 { get; set; } = new Star();
        public Star Star5 { get; set; } = new Star();

        public int AbsStarWidth
        {
            get => _absStarWidth;
            set
            {
                _absStarWidth = value;
                PropertyChangedByMember();
            }
        }
        public WpfCommand ChangeStarCountCommand { get; set; }
        public BitmapImage AbsStarImage
        {
            get => _absStarImage;
            set
            {
                _absStarImage = value;
                PropertyChangedByMember();
            }
        }

        public int StarCount { get; } = 1;

        public List<Star> Stars;

        private BitmapImage _absStarImage;
        private int _absStarWidth;

        public StarPanelViewModel()
        {
            Stars = new List<Star>{Star1, Star2, Star3, Star4, Star5};
            SetSimpleMiddleStar();
            ChangeStarCountCommand = new WpfCommand(ChangeStarCount); 
        }

        public void ChangeStarCount()
        {
            PropertyChangedByName(nameof(StarCount));
        }

        public void SetSimpleMiddleStar()
        {
            AbsStarImage = new BitmapImage(new Uri("Images/SimplStar.png", UriKind.RelativeOrAbsolute));
            AbsStarWidth = 25;
        }
        public void SetAbsMiddleStar()
        {
            AbsStarImage = new BitmapImage(new Uri("Images/AbsStar.png", UriKind.RelativeOrAbsolute));
            AbsStarWidth = 40;
            Star3.Show();
        }
    }
}
