using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Threading;
using System.Windows.Media.Imaging;

namespace HWEnchCalc.Titan
{
    public static class TitanHelper
    {
        public const string Ara = "Араджи";
        public const string Moloh = "Молох";
        public const string Vulkan = "Вулкан";
        public const string Ignis = "Игнис";

        public const string Giper = "Гиперион";
        public const string Siga = "Сигурд";
        public const string Nova = "Нова";
        public const string Mairy = "Маири";

        public const string Edem = "Эдем";
        public const string Angus = "Ангус";
        public const string Silva = "Сильва";
        public const string Avalon = "Авалон";

        public static List<string> TitanNames = new List<string>
        {
            Ara, Moloh, Vulkan, Ignis,
            Giper, Siga, Nova, Mairy,
            Edem, Angus, Silva, Avalon
        };

        public static List<int> LevelVariants { get; set; } = new List<int> {1, 2, 3, 96};

        public static BitmapImage GetTitanFaceImgByName(string titanName)
        {
            switch (titanName)
            {
                case Ara:
                    return new BitmapImage(new Uri("Titan/TitanPic/Ara.png", UriKind.Relative));
                case Moloh:
                    return new BitmapImage(new Uri("Titan/TitanPic/Moloh.png", UriKind.Relative));
                case Vulkan:
                    return new BitmapImage(new Uri("Titan/TitanPic/Vulkan.png", UriKind.Relative));
                case Ignis:
                    return new BitmapImage(new Uri("Titan/TitanPic/Ignis.png", UriKind.Relative));

                case Giper:
                    return new BitmapImage(new Uri("Titan/TitanPic/Giper.png", UriKind.Relative));
                case Siga:
                    return new BitmapImage(new Uri("Titan/TitanPic/Siga.png", UriKind.Relative));
                case Nova:
                    return new BitmapImage(new Uri("Titan/TitanPic/Nova.png", UriKind.Relative));
                case Mairy:
                    return new BitmapImage(new Uri("Titan/TitanPic/Mairy.png", UriKind.Relative));

                case Edem:
                    return new BitmapImage(new Uri("Titan/TitanPic/Edem.png", UriKind.Relative));
                case Angus:
                    return new BitmapImage(new Uri("Titan/TitanPic/Angus.png", UriKind.Relative));
                case Silva:
                    return new BitmapImage(new Uri("Titan/TitanPic/Silva.png", UriKind.Relative));
                case Avalon:
                    return new BitmapImage(new Uri("Titan/TitanPic/Avalon.png", UriKind.Relative));

                default:
                    return new BitmapImage();
            }
        }

        public static BitmapImage GetTitanBorderImgByName(string titanName)
        {
            switch (titanName)
            {
                case Ara:
                    return new BitmapImage(new Uri("Titan/TitanPic/SuperBorder.png", UriKind.Relative));
                case Moloh:
                    return new BitmapImage(new Uri("Titan/TitanPic/NormalBorder.png", UriKind.Relative));
                case Vulkan:
                    return new BitmapImage(new Uri("Titan/TitanPic/NormalBorder.png", UriKind.Relative));
                case Ignis:
                    return new BitmapImage(new Uri("Titan/TitanPic/NormalBorder.png", UriKind.Relative));

                case Giper:
                    return new BitmapImage(new Uri("Titan/TitanPic/SuperBorder.png", UriKind.Relative));
                case Siga:
                    return new BitmapImage(new Uri("Titan/TitanPic/NormalBorder.png", UriKind.Relative));
                case Nova:
                    return new BitmapImage(new Uri("Titan/TitanPic/NormalBorder.png", UriKind.Relative));
                case Mairy:
                    return new BitmapImage(new Uri("Titan/TitanPic/NormalBorder.png", UriKind.Relative));

                case Edem:
                    return new BitmapImage(new Uri("Titan/TitanPic/SuperBorder.png", UriKind.Relative));
                case Angus:
                    return new BitmapImage(new Uri("Titan/TitanPic/NormalBorder.png", UriKind.Relative));
                case Silva:
                    return new BitmapImage(new Uri("Titan/TitanPic/NormalBorder.png", UriKind.Relative));
                case Avalon:
                    return new BitmapImage(new Uri("Titan/TitanPic/NormalBorder.png", UriKind.Relative));

                default:
                    return new BitmapImage();
            }
           
        }
        public static double GetArtefactStarRaito(ArtefactType art, int starCount)
        {
            switch (art)
            {
                case ArtefactType.ElementalAtc:
                    return GetElementalArtRaito(starCount);
                case ArtefactType.ElementalDef:
                    return GetElementalArtRaito(starCount);
                default: return GetThridArtRaito(starCount);
            }
        }

        private static double GetElementalArtRaito(int starCount)
        {
            switch (starCount)
            {
                case 1:
                    return 1;
                case 2:
                    return 1.2;
                case 3:
                    return 1.5;
                case 4:
                    return 2;
                case 5:
                    return 2.5;
                case 6:
                    return 3;
                default: return 0;
            }
        }
        private static double GetThridArtRaito(int starCount)
        {
            switch (starCount)
            {
                case 1:
                    return 1;
                case 2:
                    return 1.25;
                case 3:
                    return 1.5;
                case 4:
                    return 2;
                case 5:
                    return 2.5;
                case 6:
                    return 3.75;
                default: return 0;
            }
        }
    }
}