using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion VII — Mega-Location: The Tessarat Deep Freeze (Municipal Seed Vault).
    /// Built in 1998 beneath the permafrost line. 40,000 sealed canisters of heirloom
    /// seeds. The holy grail of Victory_TheMartian and Victory_UndergroundCity.
    /// The automated geothermal heating failed on Day 12. Interior is -35°C.
    /// The caretaker, Elias, sealed himself inside on Day 0 and has been eating the seeds.
    /// </summary>
    public class Location_MunicipalSeedVault
    {
        public const string LocationId = "location_municipal_seed_vault";
        public const string DisplayName = "The Tessarat Deep Freeze";
        public const int TravelHours = 12;
        public const int DangerLevel = 9;
        public const float BaseRads = 5f;
        public const float AmbientTemp = -35f;

        // ── Required gear ─────────────────────────────────────────────
        public const string RequiredGear_WinterCoat = "winter_coat";
        public const string RequiredGear_WoolGloves = "wool_gloves";
        public const string RequiredGear_ProtectiveGoggles = "protective_goggles";

        // ── Unique loot ───────────────────────────────────────────────
        public const string Item_SeedCanisterWheat = "seed_envelope_wheat";
        public const string Item_SeedCanisterPotato = "seed_envelope_potato";
        public const string Item_SeedCanisterTomato = "seed_envelope_tomato";
        public const int MaxCanistersPerVisit = 5;
        public const int TotalCanistersAvailable = 10;

        // ── Hazard constants ──────────────────────────────────────────
        public const float FrostbiteCheckPerHour = 0.20f; // 20% chance per hour
        public const float FuelBurnMultiplier = 2f;       // fuel burns 2× faster
        public const float HeaterFailureDeathChance = 0.80f;

        // ── Elias the caretaker ───────────────────────────────────────
        public const string NPC_Elias = "npc_elias_caretaker";
        public const string Item_PoppyLatex = "item_poppy_latex";
        public const string Item_OpiumRaw = "item_opium_raw";

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnFrostbiteCheck;
        public event Action<string> OnEliasEncounter;
        public event Action<string> OnEliasSabotage;
        public event Action<string, int> OnSeedsCollected;
        public event Action<string> OnEliasAbandoned;

        private readonly System.Random _rng;
        private int _canistersCollected;
        private bool _eliasEncountered;
        private bool _eliasBroughtToBunker;
        private bool _eliasSabotaged;
        private bool _eliasAbandoned;
        private int _visitCount;

        public int CanistersCollected => _canistersCollected;
        public bool IsEliasEncountered => _eliasEncountered;
        public bool IsEliasBroughtToBunker => _eliasBroughtToBunker;
        public bool IsVaultExhausted => _canistersCollected >= TotalCanistersAvailable;

        public Location_MunicipalSeedVault(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(10000);
        }

        // ── Vault entry ───────────────────────────────────────────────

        /// <summary>
        /// Enter the vault. Requires vehicle or mobile camp. Extreme cold.
        /// Every hour triggers frostbite check.
        /// </summary>
        public VaultEntryResult EnterVault(string survivorId, bool hasWinterCoat,
            bool hasWoolGloves, bool hasGoggles, float fuelAvailable)
        {
            _visitCount++;
            var result = new VaultEntryResult { Success = true };

            // Gear check
            if (!hasWinterCoat || !hasWoolGloves || !hasGoggles)
            {
                result.MissingGear = true;
                result.Message = "Without proper gear, the cold will kill you in hours.";
                return result;
            }

            // Fuel check
            if (fuelAvailable <= 0f)
            {
                result.NoFuel = true;
                result.Message = "The heater is empty. The survivors will freeze in their sleep.";
                return result;
            }

            // Frostbite check
            if (_rng.NextDouble() < FrostbiteCheckPerHour)
            {
                result.FrostbiteRisk = true;
                OnFrostbiteCheck?.Invoke(survivorId);
            }

            return result;
        }

        // ── Elias encounter ───────────────────────────────────────────

        /// <summary>
        /// Encounter Elias, the pre-war caretaker. He has been alone for 80 days.
        /// He believes the surface is fine. He has rigged the doors with explosives.
        /// </summary>
        public EliasEncounterResult EncounterElias(string survivorId)
        {
            if (_eliasEncountered) return new EliasEncounterResult { AlreadyEncountered = true };
            _eliasEncountered = true;
            OnEliasEncounter?.Invoke(survivorId);

            return new EliasEncounterResult
            {
                Success = true,
                Message = "A man stands in the dark. He is thin. His eyes are wide. " +
                    "He has been eating seeds for 80 days. He believes the surface " +
                    "is still perfectly fine. He thinks the ash is a government psy-op. " +
                    "The blast doors behind you click. He has rigged them with explosives."
            };
        }

        /// <summary>
        /// Play along with Elias's delusion. Take 5 canisters but bring him back.
        /// He will sabotage the bunker when he realizes the truth.
        /// </summary>
        public EliasResult PlayAlong(string survivorId)
        {
            int yield = Mathf.Min(MaxCanistersPerVisit, TotalCanistersAvailable - _canistersCollected);
            _canistersCollected += yield;
            _eliasBroughtToBunker = true;

            OnSeedsCollected?.Invoke(survivorId, yield);

            return new EliasResult
            {
                Success = true,
                SeedsCollected = yield,
                EliasBroughtToBunker = true,
                WillSabotage = true,
                Message = "You pretend to be government inspectors. Elias smiles. " +
                    "He gives you " + yield + " canisters. He wants to come with you. " +
                    "When he sees the ash, the truth will break him."
            };
        }

        /// <summary>
        /// Leave Elias in the dark. Take the seeds by force or stealth.
        /// </summary>
        public EliasResult LeaveElias(string survivorId)
        {
            int yield = Mathf.Min(MaxCanistersPerVisit, TotalCanistersAvailable - _canistersCollected);
            _canistersCollected += yield;
            _eliasAbandoned = true;

            OnSeedsCollected?.Invoke(survivorId, yield);
            OnEliasAbandoned?.Invoke(survivorId);

            return new EliasResult
            {
                Success = true,
                SeedsCollected = yield,
                EliasAbandoned = true,
                Message = "You take the seeds. You leave Elias in the cold. " +
                    "The seeds will grow. Elias will not."
            };
        }

        // ── Elias sabotage (after bringing him to bunker) ─────────────

        /// <summary>
        /// Check if Elias should sabotage the bunker after realizing the truth.
        /// </summary>
        public bool CheckEliasSabotage()
        {
            if (!_eliasBroughtToBunker || _eliasSabotaged) return false;
            if (_rng.NextDouble() < 0.60f)
            {
                _eliasSabotaged = true;
                OnEliasSabotage?.Invoke(NPC_Elias);
                return true;
            }
            return false;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public SeedVaultSave CaptureState()
        {
            return new SeedVaultSave
            {
                CanistersCollected = _canistersCollected,
                EliasEncountered = _eliasEncountered,
                EliasBroughtToBunker = _eliasBroughtToBunker,
                EliasSabotaged = _eliasSabotaged,
                EliasAbandoned = _eliasAbandoned,
                VisitCount = _visitCount
            };
        }

        public void RestoreState(SeedVaultSave save)
        {
            _canistersCollected = 0;
            _eliasEncountered = false;
            _eliasBroughtToBunker = false;
            _eliasSabotaged = false;
            _eliasAbandoned = false;
            _visitCount = 0;
            if (save == null) return;
            _canistersCollected = save.CanistersCollected;
            _eliasEncountered = save.EliasEncountered;
            _eliasBroughtToBunker = save.EliasBroughtToBunker;
            _eliasSabotaged = save.EliasSabotaged;
            _eliasAbandoned = save.EliasAbandoned;
            _visitCount = save.VisitCount;
        }
    }

    [Serializable]
    public class VaultEntryResult
    {
        public bool Success;
        public bool MissingGear;
        public bool NoFuel;
        public bool FrostbiteRisk;
        public string Message;
    }

    [Serializable]
    public class EliasEncounterResult
    {
        public bool Success;
        public bool AlreadyEncountered;
        public string Message;
    }

    [Serializable]
    public class EliasResult
    {
        public bool Success;
        public int SeedsCollected;
        public bool EliasBroughtToBunker;
        public bool EliasAbandoned;
        public bool WillSabotage;
        public string Message;
    }

    [Serializable]
    public class SeedVaultSave
    {
        public int CanistersCollected;
        public bool EliasEncountered;
        public bool EliasBroughtToBunker;
        public bool EliasSabotaged;
        public bool EliasAbandoned;
        public int VisitCount;
    }
}
