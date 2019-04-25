using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using HWEnchCalc.Common;

namespace HWEnchCalc.StarPanelControl
{
    public class StarPanelViewModel : NotifyPropertyChangedBase
    {
        public Star Star1 { get; set; } = new Star(5, 5);
        public Star Star2 { get; set; } = new Star(3, 5);
        public Star Star3 { get; set; } = new Star(1, 6) { Visibility = Visibility.Visible };
        public Star Star4 { get; set; } = new Star(2, 5);
        public Star Star5 { get; set; } = new Star(4, 5);
      
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

        public StarPanelViewModel()
        {
            Stars = new List<Star>{ Star1, Star2, Star3, Star4, Star5 };
            SetSimpleMiddleStar();
            ChangeStarCountCommand = new WpfCommand(ChangeStarCount); 
        }

        public void ShowStars(int starCount)
        {
            foreach (var star in Stars)
            {
                star.UpdateVision(starCount);
            }
        }

        public void ChangeStarCount()
        {
            PropertyChangedByName(nameof(StarCount));
        }

        public void SetSimpleMiddleStar()
        {
            AbsStarImage = new BitmapImage(new Uri("Images/SimplStar.png", UriKind.RelativeOrAbsolute));
        }
        public void SetAbsMiddleStar()
        {
            AbsStarImage = new BitmapImage(new Uri("Images/AbsStar.png", UriKind.RelativeOrAbsolute));
        }
    }
}
