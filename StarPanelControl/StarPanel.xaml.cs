using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HWEnchCalc.StarPanelControl
{
    /// <summary>
    /// Логика взаимодействия для StarPanel.xaml
    /// </summary>
    public partial class StarPanel 
    {
        //public static readonly DependencyProperty StarCountProperty = DependencyProperty.Register(
        //    "StarCount",
        //    typeof(int),
        //    typeof(StarPanel),
        //    new UIPropertyMetadata(1, StarCountChanged),
        //    ValidateMyNumbere);

        private const int AbsoluteStartCount = 6;


        public static readonly DependencyProperty StarCountProperty = DependencyProperty.Register(
            "StarCount",
            typeof(int),
            typeof(StarPanel),
            new PropertyMetadata(
                1, StarCountChanged),
            ValidateMyNumbere);


        private static void StarCountChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            var panel = (StarPanel)depObj;
            var starCount = panel.StarCount;
            var viewModel = panel.StarPanelViewModel;

            viewModel.ShowStars(starCount);

            if (starCount == AbsoluteStartCount)
            {
                viewModel.SetAbsMiddleStar();
                return;
            }

            viewModel.SetSimpleMiddleStar();
        }

        private static bool ValidateMyNumbere(object value)
        {
            if (!int.TryParse(value.ToString(), out var res)) return false;
            return res > 0 && res <= AbsoluteStartCount;
        }

        public int StarCount
        {
            get => (int)GetValue(StarCountProperty);
            set => SetValue(StarCountProperty, value);
        }
        
        public StarPanelViewModel StarPanelViewModel = new StarPanelViewModel();

        public StarPanel()
        {
            InitializeComponent();
            SPanel.DataContext = StarPanelViewModel;
            StarPanelViewModel.PropertyChanged += ChangeStarCount;
        }

        private void ChangeStarCount(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName != nameof(StarCount)) return;

            if (StarCount >= AbsoluteStartCount)
            {
                StarCount = 1;
            }
            else
            {
                StarCount++;
            }
        }
    }
}
