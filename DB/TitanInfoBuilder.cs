using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HWEnchCalc.Calculators.TitanCalc;
using HWEnchCalc.Common;
using HWEnchCalc.Titan;
using HWEnchCalc.Titan.ArtefactData;
using HWEnchCalc.Titan.Guise;
using HWEnchCalc.Titan.Helper;

namespace HWEnchCalc.DB
{
    public class TitanInfoBuilder
    {
        private readonly TitanSourceDataHelper _titanHelper;

        public TitanInfoBuilder(TitanSourceDataHelper titanHelper)
        {
            _titanHelper = titanHelper;
        }

        public TitanInfoDbo GeTitanInfoDbo(TitanInfo titanInfo)
        {
            return new TitanInfoDbo
            {
                Name = titanInfo.Name,
                Level = titanInfo.Level,
                TotalAttack = titanInfo.TotalAttack,
                TotalHp = titanInfo.TotalHp,
                StarCount = titanInfo.StarCount,
                TotemLevel = titanInfo.TotemLevel,
                TotemStars = titanInfo.TotemStars,
                Ticks = DateTime.Now.Ticks,

                Guises = titanInfo.Guises.Select(
                        g => new GuiseInfoDbo
                        {
                            GuiseType = g.GuiseType,
                            Level = g.Level,
                            StatValue = g.StatValue,
                            IncreaseStatValue = g.IncreaseStatValue,
                            LvlUpCost = g.LvlUpCost
                        })
                    .ToList(),

                ElementalOffenceArtefact = new ElementArtInfoDbo
                {
                    ArtefactType = titanInfo.ElementalOffenceArt.ArtefactType,
                    LevelUpCost = titanInfo.ElementalOffenceArt.LevelUpCost,
                    IncreaseStatValue = titanInfo.ElementalOffenceArt.IncreaseStatValue,
                    Level = titanInfo.ElementalOffenceArt.Level,
                    StarCount = titanInfo.ElementalOffenceArt.StarCount,
                    StatValue = titanInfo.ElementalOffenceArt.StatValue
                },
                ElementalDefenceAtrefact = new ElementArtInfoDbo
                {
                    ArtefactType = titanInfo.ElementalDefenceArt.ArtefactType,
                    LevelUpCost = titanInfo.ElementalDefenceArt.LevelUpCost,
                    IncreaseStatValue = titanInfo.ElementalDefenceArt.IncreaseStatValue,
                    Level = titanInfo.ElementalDefenceArt.Level,
                    StarCount = titanInfo.ElementalDefenceArt.StarCount,
                    StatValue = titanInfo.ElementalDefenceArt.StatValue
                },
                SealArtefact = new SealArtInfoDbo
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

        public ObservableCollection<TitanShowedData> GetIitanShowedDatas(List<TitanInfoDbo> titanInfoDbo)
        {
            return titanInfoDbo.Select(GetTitanShowedData).ToObservable();
        }

        public TitanShowedData GetTitanShowedData(TitanInfoDbo titanDbo)
        {

            var elementalOffenceArt =
            new ElementalArtInfo(titanDbo.ElementalOffenceArtefact.ArtefactType, _titanHelper)
            {
                LevelUpCost = titanDbo.ElementalOffenceArtefact.LevelUpCost,
                IncreaseStatValue = titanDbo.ElementalOffenceArtefact.IncreaseStatValue,
                Level = titanDbo.ElementalOffenceArtefact.Level,
                StarCount = titanDbo.ElementalOffenceArtefact.StarCount,
                StatValue = titanDbo.ElementalOffenceArtefact.StatValue
            };
            var elementalDefenceArt =
            new ElementalArtInfo(titanDbo.ElementalDefenceAtrefact.ArtefactType, _titanHelper)
            {
                LevelUpCost = titanDbo.ElementalDefenceAtrefact.LevelUpCost,
                IncreaseStatValue = titanDbo.ElementalDefenceAtrefact.IncreaseStatValue,
                Level = titanDbo.ElementalDefenceAtrefact.Level,
                StarCount = titanDbo.ElementalDefenceAtrefact.StarCount,
                StatValue = titanDbo.ElementalDefenceAtrefact.StatValue
            };
            var sealArt = new SealArtInfo(titanDbo.SealArtefact.ArtefactType, _titanHelper)
            {
                Level = titanDbo.SealArtefact.Level,
                Attack = titanDbo.SealArtefact.Attack,
                Hp = titanDbo.SealArtefact.Hp,
                StarCount = titanDbo.SealArtefact.StarCount,
                LevelUpCost = titanDbo.SealArtefact.LevelUpCost
            };

            var guises = GetGuises(titanDbo.Guises);

            var titanInfo = new TitanInfo(
                titanDbo.Name,
                titanDbo.Level,
                titanDbo.TotalAttack,
                titanDbo.TotalHp,
                titanDbo.StarCount,
                titanDbo.TotemLevel,
                titanDbo.TotemStars,
                elementalOffenceArt,
                elementalDefenceArt,
                sealArt,
                guises,
                _titanHelper);

            return new TitanShowedData(titanDbo.Id, titanDbo.Ticks, titanInfo);
        }

        private ObservableCollection<GuiseInfo> GetGuises(List<GuiseInfoDbo> guisesDbo)
        {
            var result = new ObservableCollection<GuiseInfo>();

            foreach (var guiseDbo in guisesDbo)
            {
                var guise = new GuiseInfo(guiseDbo.GuiseType, _titanHelper)
                {
                    Level = guiseDbo.Level,
                    StatValue = guiseDbo.StatValue,
                    IncreaseStatValue = guiseDbo.IncreaseStatValue,
                    LvlUpCost = guiseDbo.LvlUpCost
                };
                result.Add(guise);
            }

            return result;
        }
    }
}