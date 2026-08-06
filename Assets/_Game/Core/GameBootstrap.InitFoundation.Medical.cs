using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {

        private void InitAddictionAndMedicalPrompts()
        {
            InitAddictionSystem();
            InitMedicalBodySystems();
            WireMedicalAddictionHooks();
        }

        private void InitAddictionSystem()
        {
            Addiction = new AddictionSystem(CreateSaltedRng(_worldSeed, "addiction"));
            if (_itemCatalog != null)
            {
                string[] addictiveIds = { "morphine", "anti_rad", "painkiller", "stimulant" };
                foreach (var id in addictiveIds)
                {
                    var item = _itemCatalog.GetById(id);
                    if (item != null)
                        Addiction.RegisterAddictiveItem(item.id);
                }
            }
            Addiction.RegisterAddictiveItem("morphine");
            Addiction.RegisterAddictiveItem("anti_rad");
            Addiction.RegisterAddictiveItem("amphetamines");
            Addiction.PanicDestroyHandler = (sv, rng) => ForceAddictionPanicDestroy(sv, rng);
        }

        private void InitMedicalBodySystems()
        {
            BloodTransfusion = new BloodTransfusionSystem(new System.Random(_worldSeed + 55));
            BloodTransfusion.Bind(
                id => Survivors?.Find(s => s.Id == id),
                (sv, afflictionId) => MedicalSystem?.Inflict(sv, afflictionId));

            AmputationSystem = new AmputationSystem();
            AmputationSystem.SetRng(new System.Random(_worldSeed + 56));
            AmputationSystem.Bind(
                id => Survivors?.Find(s => s.Id == id),
                (sv, afflictionId) => MedicalSystem?.Inflict(sv, afflictionId));

            ScurvySystem = new ScurvySystem();
            ScurvySystem.Bind(
                id => Survivors?.Find(s => s.Id == id),
                (sv, afflictionId) => MedicalSystem?.Inflict(sv, afflictionId));

            Mutagenesis = new RadiationMutagenesisSystem();
            Mutagenesis.Bind(
                getPartyAverageRadiation: GetPartyAverageLifetimeRadiation,
                inflictAffliction: (sv, afflictionId) => MedicalSystem?.Inflict(sv, afflictionId));
        }

        private float GetPartyAverageLifetimeRadiation()
        {
            if (Survivors == null || Survivors.Count == 0) return 0f;
            float sum = 0f; int n = 0;
            for (int i = 0; i < Survivors.Count; i++)
            {
                if (Survivors[i] != null && Survivors[i].IsAlive)
                {
                    sum += Survivors[i].LifetimeRadiationExposure;
                    n++;
                }
            }
            return n > 0 ? sum / n : 0f;
        }

        private void WireMedicalAddictionHooks()
        {
            if (MedicalSystem == null) return;
            MedicalSystem.GetCurrentDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 1;
            MedicalSystem.OnTreatmentItemConsumed = (sv, itemId, day) =>
            {
                Addiction?.OnItemConsumed(sv, itemId, day);
            };
        }

    }
}
