using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HWEnchCalc.Common;
using HWEnchCalc.Config;

namespace HWEnchCalc.Titan.Helper
{
    public class TitanSourceDataHelper
    {
        private List<TitanSourceInfo> _titanSourceInfo = new List<TitanSourceInfo>();
        public ElementalArtefactHelper ElementArtefactHelper;
        public SealArtefactHelper SealArtefactHelper;
        public TotemHelper TotemHelper;

        public TitanSourceDataHelper Fill(Configuration config)
        {
            if (config?.GameInfo?.TitanDatas != null)
            {
                ElementArtefactHelper = new ElementalArtefactHelper(config);
                SealArtefactHelper = new SealArtefactHelper(config);
                TotemHelper = new TotemHelper(config);
                _titanSourceInfo = GetSourceTitanInfo(config.GameInfo.TitanDatas).ToList();
            }

            return this;
        }

        public List<string> GetTitanNames()
        {
            return _titanSourceInfo.Select(t => t.Name).ToList();
        }

        public TitanSourceInfo GetTitanSourseInfo(string titanName)
        {
            return _titanSourceInfo.Find(t => t.Name == titanName);
        }

        //взято из игровых формул
        private const double TitalLevelPowСoefficient = 1.5;

        public (double Hp, double Attack) GetHpAndAttack(TitanSourceInfo titan, int level, int starCount)
        {
            var levelModyfier = Math.Pow(level, TitalLevelPowСoefficient);
            var statsPerStar = titan.StatsPerLevelPerStar.Find(s => s.StarCount == starCount);

            var hp = statsPerStar.Hp * levelModyfier + titan.HpStart;
            var atack = statsPerStar.Attack * levelModyfier + titan.AttackStart;

            return (Math.Round(hp, 1), Math.Round(atack, 1));
        }

        private IEnumerable<TitanSourceInfo> GetSourceTitanInfo(TitanDatas titanDatas)
        {
            var titansDir = new DirectoryInfo(titanDatas.TitanFolder);

            foreach (var titanFile in titansDir.GetFiles())
            {
                var titanString = File.ReadAllText(titanFile.FullName);
                yield return JsonParser<TitanSourceInfo>.Deserialize(titanString);
            }
        }
    }
}