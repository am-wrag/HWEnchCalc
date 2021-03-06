﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HWEnchCalc.Common;
using HWEnchCalc.Config;

namespace HWEnchCalc.Titan.Helper
{
    public class TitanSourceDataHelper
    {
        public int TitanMaxLevel;
        public ElementalArtefactHelper ElementArtefactHelper;
        public SealArtefactHelper SealArtefactHelper;
        public TotemHelper TotemHelper;
        public TitanGuiseHelper GuiseHelper;

        private List<TitanSourceInfo> _titanSourceInfo = new List<TitanSourceInfo>();

        //из игровых формул
        private const double TitanLevelPowCoefficient = 1.5;

        public TitanSourceDataHelper Fill(Configuration config)
        {
            if (config?.GameInfo?.TitanDatas != null)
            {
                TitanMaxLevel = config.GameInfo.TitanDatas.TitanMaxLevel;

                _titanSourceInfo = GetSourceTitanInfo(config.GameInfo.TitanDatas).ToList();

                ElementArtefactHelper = new ElementalArtefactHelper(config);
                SealArtefactHelper = new SealArtefactHelper(config);
                TotemHelper = new TotemHelper(config);
                GuiseHelper = new TitanGuiseHelper(config.GameInfo.TitanDatas);
            }
            return this;
        }

        public List<string> GetTitanNames()
        {
            return _titanSourceInfo.Select(t => t.Name).ToList();
        }

        public TitanSourceInfo GetTitanSourceInfo(string titanName)
        {
            return _titanSourceInfo.Find(t => t.Name == titanName);
        }

        public (double Hp, double Attack) GetHpAndAttack(TitanSourceInfo titan, int level, int starCount)
        {
            var levelModifier = Math.Pow(level, TitanLevelPowCoefficient);
            var statsPerStar = titan.StatsPerLevelPerStar.Find(s => s.StarCount == starCount);

            var hp = statsPerStar.Hp * levelModifier + titan.HpStart;
            var attack = statsPerStar.Attack * levelModifier + titan.AttackStart;

            return (Math.Round(hp, 1), Math.Round(attack, 1));
        }

        private IEnumerable<TitanSourceInfo> GetSourceTitanInfo(TitanData titanDatas)
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