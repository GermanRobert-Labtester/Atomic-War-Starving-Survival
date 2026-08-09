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
            Addiction.SetNeedsSystem(NeedsSystem);
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
            AmputationSystem.SetNeedsSystem(NeedsSystem);
            AmputationSystem.SetRng(new System.Random(_worldSeed + 56));
            AmputationSystem.Bind(
                id => Survivors?.Find(s => s.Id == id),
                (sv, afflictionId) => MedicalSystem?.Inflict(sv, afflictionId),
                (sv, afflictionId, surgeon) =>
                    MedicalSystem != null && MedicalSystem.CureOutright(sv, afflictionId, surgeon));

            ScurvySystem = new ScurvySystem();
            ScurvySystem.SetNeedsSystem(NeedsSystem);
            ScurvySystem.Bind(
                id => Survivors?.Find(s => s.Id == id),
                (sv, afflictionId) => MedicalSystem?.Inflict(sv, afflictionId));

            Mutagenesis = new RadiationMutagenesisSystem();
            Mutagenesis.SetNeedsSystem(NeedsSystem);
            Mutagenesis.Bind(
                getPartyAverageRadiation: GetPartyAverageLifetimeRadiation,
                inflictAffliction: (sv, afflictionId) => MedicalSystem?.Inflict(sv, afflictionId));

            // Capture orphans: constructed + save-wired so chem/graft/camo state survives load.
            BloodToxicity = new BloodToxicitySystem();
            GraftRejection = new GraftRejectionSystem();
            PheromoneMasking = new PheromoneMaskingSystem();
            ChemTolerance = new System_Tolerance();
            // Rogue-lite grave site (populated on wipe; restored from save mid-run).
            LastWill = new LastWillSystem();
            // Prompt #859 — ruined-bunker legacy start (seeded from Last Will on wipe).
            LegacyStart = new System_LegacyStart();
            // Prompt #829 — blood types for bag transfusions (person-to-person stays on BloodTransfusion).
            BloodTypes = new System_BloodTypes();
            BloodTypes.SetRng(new System.Random(_worldSeed + 829));
            AssignBloodTypesToExistingSurvivors();
            // Prompt #768 — epilogue counters + empty-bunker narrative.
            EpilogueStats = new System_EpilogueStats();
            // Prompt #861 — adaptive warlord gear across wipes (restored from save mid-run).
            AdaptiveWarlords = new System_AdaptiveWarlords();
        }

        /// <summary>
        /// Survivors are created in InitFoundation before medical systems exist;
        /// assign types once BloodTypes is constructed (and for later recruits).
        /// </summary>
        private void AssignBloodTypesToExistingSurvivors()
        {
            if (BloodTypes == null || Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || string.IsNullOrEmpty(sv.Id)) continue;
                string type = BloodTypes.EnsureBloodType(sv.Id);
                if (BloodTransfusion != null && !string.IsNullOrEmpty(type))
                    BloodTransfusion.SetBloodType(sv.Id, ParseBloodTypeEnum(type));
            }
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
            // Shared host path: inventory, AI, treatments, emergency halt, amputation morphine.
            ChemUse = new ChemUseRouter();
            ChemUse.Bind(
                Addiction,
                BloodToxicity,
                PolypharmacySystem,
                ChemTolerance,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 1,
                getGameHours: () => TimeSystem != null ? TimeSystem.TotalElapsedHours : 0f);

            System.Action<Survivor, string, int> onChem = (sv, itemId, day) =>
                ChemUse?.Notify(sv, itemId);

            if (MedicalSystem != null)
            {
                MedicalSystem.GetCurrentDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 1;
                MedicalSystem.OnTreatmentItemConsumed = onChem;
            }

            if (AmputationSystem != null)
                AmputationSystem.OnChemConsumed = onChem;

            WireAntiRadToleranceHooks();
        }

        /// <summary>Prompt #833 — UseAntiRad peeks duration/effectiveness before dose.</summary>
        private void WireAntiRadToleranceHooks()
        {
            if (Actions == null) return;
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i] is UseAntiRadActionSO anti)
                {
                    anti.GetChemEffectiveness = (sv, id) =>
                        ChemUse != null ? ChemUse.PeekEffectiveness(sv, id) : 1f;
                    anti.GetChemDurationHours = (sv, id) =>
                        ChemUse != null
                            ? ChemUse.PeekDurationHours(sv, id)
                            : System_Tolerance.BaseDurationHours;
                }
            }
        }

    }
}
