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
                TotemLevel = titanInfo.TotemLevel,
                TotemStars = titanInfo.TotemStars,
                Ticks = DateTime.Now.Ticks,
                FirstArtefact = new ElementArtInfoDbo
                {
                    ArtefactType = titanInfo.FirstArt.ArtefactType,
                    LevelUpCost = titanInfo.FirstArt.LevelUpCost,
                    IncreaseStatValue = titanInfo.FirstArt.IncreaseStatValue,
                    LevelInfo = titanInfo.FirstArt.LevelInfo,
                    StarCount = titanInfo.FirstArt.StarCount,
                    StatValue = titanInfo.FirstArt.StatValue
                },
                SecondArtefact = new ElementArtInfoDbo()
                {
                    ArtefactType = titanInfo.SecondArt.ArtefactType,
                    LevelUpCost = titanInfo.SecondArt.LevelUpCost,
                    IncreaseStatValue = titanInfo.SecondArt.IncreaseStatValue,
                    LevelInfo = titanInfo.SecondArt.LevelInfo,
                    StarCount = titanInfo.SecondArt.StarCount,
                    StatValue = titanInfo.SecondArt.StatValue
                },
                SealArtefact = new SealArtInfoDbo()
                {
                    ArtefactType = titanInfo.SealArt.ArtefactType,
                    Level = titanInfo.SealArt.Level,
                    Attack = titanInfo.SealArt.Attack,
                    Hp = titanInfo.SealArt.Hp,
                    StarCount = titanInfo.SealArt.StarCount,
                    LevelUpCost = titanInfo.SealArt.LevelUpCost
                }
            };
        }
    }
}