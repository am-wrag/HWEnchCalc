using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace HWEnchCalc.Titan
{
    public class TitanSourceInfo
    {
        public string Name { get; private set; }
        public string RusName { get; private set; }
        public int HpStart { get; private set; }
        public int AttackStart { get; private set; }
        public readonly Dictionary<int, int> HpPerStar = new Dictionary<int, int>();
        public readonly Dictionary<int, int> AttackPerStar = new Dictionary<int, int>();

        public BitmapImage FaceImage { get; private set; }
        public BitmapImage BorderImage { get; private set; }


    }
}