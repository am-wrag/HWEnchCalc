using System;
using HWEnchCalc.Titan;

namespace HWEnchCalc.DB
{
    public struct TitanInfoBuilder
    {
        public TitanInfoDbo GeTitanInfoDbo(TitanInfo titanInfo)
        {
            return new TitanInfoDbo()
            {
                Name = titanInfo.Name,
                Level = titanInfo.Level,
                Attack = titanInfo.Attack,
                Hp = titanInfo.Hp,
                StarCount = titanInfo.StarCount,
                Ticks = DateTime.Now.Ticks,
                FirstArtefact = new TitatnArtefactInfoDbo
                {
                    ArtefactType = titanInfo.FirstArt.ArtefactType,
                    LevelUpCost = titanInfo.FirstArt.LevelUpCost,
                    IncreaseStatValue = titanInfo.FirstArt.IncreaseStatValue,
                    LevelInfo = titanInfo.FirstArt.LevelInfo,
                    StarCount = titanInfo.FirstArt.StarCount,
                    StatValue = titanInfo.FirstArt.StatValue
                },
                SecondArtefact = new TitatnArtefactInfoDbo()
                {
                    ArtefactType = titanInfo.SecondArt.ArtefactType,
                    LevelUpCost = titanInfo.SecondArt.LevelUpCost,
                    IncreaseStatValue = titanInfo.SecondArt.IncreaseStatValue,
                    LevelInfo = titanInfo.SecondArt.LevelInfo,
                    StarCount = titanInfo.SecondArt.StarCount,
                    StatValue = titanInfo.SecondArt.StatValue
                },
                SealArtefact = new TitatnArtefactInfoDbo()
                {
                    ArtefactType = titanInfo.SealArt.ArtefactType,
                    LevelUpCost = titanInfo.SealArt.LevelUpCost,
                    IncreaseStatValue = titanInfo.SealArt.IncreaseStatValue,
                    LevelInfo = titanInfo.SealArt.LevelInfo,
                    StarCount = titanInfo.SealArt.StarCount,
                    StatValue = titanInfo.SealArt.StatValue
                }
            };
        }
    }
}