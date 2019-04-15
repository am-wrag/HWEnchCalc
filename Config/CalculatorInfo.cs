namespace HWEnchCalc.Config
{
    public class CalculatorInfo
    {
        public int LastCalcIndex { get; set; }

        public ArtefactLevelInfo ArtefactLevelInfo { get; set; }
    }

    public class ArtefactLevelInfo
    {
        public string FirstArtLevelsPath { get; set; }
        public string SecondArtLevelsPath { get; set; }
        public string ThirdArtActTypeLevelsPath { get; set; }
        public string ThirdArtDefTypeLevelsPath { get; set; }
        public string ThirdArtSuppTypeLevelsPath { get; set; }
    }
}