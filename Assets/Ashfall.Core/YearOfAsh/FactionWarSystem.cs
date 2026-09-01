using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.YearOfAsh
{
    [Serializable]
    public class FactionStandingRecord
    {
        public string factionId = string.Empty;
        public int standing = 0; // -100 (Blood Feud) to +100 (Allied)
        public int territorialControlPercent = 20; // 0 to 100
        public bool isHostile = false;
        public bool isAllied = false;
    }

    [Serializable]
    public class FactionWarSystemState
    {
        public List<FactionStandingRecord> factions = new List<FactionStandingRecord>();
        public int activeWarTension = 50; // 0 to 100
        public string dominantFactionId = "faction_central_garrison";
        public List<string> enactedDecrees = new List<string>();
        public int totalArtilleryStrikesLogged = 0;
    }

    /// <summary>
    /// Engine-agnostic multi-faction geopolitical war simulation for Days 180 to 360.
    /// Simulates territorial battles, supply-line cutoffs, and decree enforcement.
    /// Zero engine dependencies; deterministic.
    /// </summary>
    public class FactionWarSystem
    {
        public const string SystemId = "faction_war_system";
        /// <summary>Standing at or below this is hostile (isHostile). Credit and
        /// other trust-gated offers must use this constant, never a copied -50.</summary>
        public const int HostileStandingThreshold = -50;

        private readonly FactionWarSystemState _state;

        public FactionWarSystemState State => _state;
        public int WarTension => _state.activeWarTension;
        public string DominantFactionId => _state.dominantFactionId;

        public event Action<string, int> OnFactionStandingChanged;
        public event Action<string> OnDecreeEnacted;
        public event Action<string, string> OnTerritorialClashOccurred;

        public FactionWarSystem(FactionWarSystemState? state = null)
        {
            _state = state ?? new FactionWarSystemState();
            if (_state.enactedDecrees == null) _state.enactedDecrees = new List<string>();
            if (_state.factions == null) _state.factions = new List<FactionStandingRecord>();
            EnsureDefaultFactions();
        }

        public int GetStanding(string factionId)
        {
            var record = _state.factions.Find(f => f.factionId == factionId);
            return record != null ? record.standing : 0;
        }

        public void ModifyStanding(string factionId, int delta)
        {
            var record = _state.factions.Find(f => f.factionId == factionId);
            if (record == null)
            {
                record = new FactionStandingRecord { factionId = factionId, standing = 0 };
                _state.factions.Add(record);
            }

            record.standing += delta;
            if (record.standing > 100) record.standing = 100;
            if (record.standing < -100) record.standing = -100;

            record.isHostile = record.standing <= -50;
            record.isAllied = record.standing >= 50;

            OnFactionStandingChanged?.Invoke(factionId, record.standing);
        }

        public void EnactDecree(string decreeId)
        {
            if (!_state.enactedDecrees.Contains(decreeId))
            {
                _state.enactedDecrees.Add(decreeId);
                _state.activeWarTension = Math.Min(100, _state.activeWarTension + 15);
                OnDecreeEnacted?.Invoke(decreeId);
            }
        }

        public void SimulateDailyFriction(int day)
        {
            if (day <= 240) return; // Full war kicks off in Phase 5 (day 241+)

            _state.activeWarTension = Math.Min(100, _state.activeWarTension + 1);

            // Shifting territories deterministically based on day modulo
            if (day % 15 == 0)
            {
                var garrison = _state.factions.Find(f => f.factionId == "faction_central_garrison");
                var rebuilders = _state.factions.Find(f => f.factionId == "faction_rebuilders");

                if (garrison != null && rebuilders != null)
                {
                    garrison.territorialControlPercent += 3;
                    rebuilders.territorialControlPercent = Math.Max(5, rebuilders.territorialControlPercent - 3);
                    _state.totalArtilleryStrikesLogged++;
                    OnTerritorialClashOccurred?.Invoke("faction_central_garrison", "faction_rebuilders");
                }
            }
        }

        private void EnsureDefaultFactions()
        {
            string[] defaultFactions = new[]
            {
                "faction_central_garrison",
                "faction_rebuilders",
                "faction_black_ops",
                "faction_ash_sign",
                "faction_hydro_barons",
                "faction_forward_roster"
            };

            foreach (var fId in defaultFactions)
            {
                if (!_state.factions.Exists(f => f.factionId == fId))
                {
                    _state.factions.Add(new FactionStandingRecord
                    {
                        factionId = fId,
                        standing = 0,
                        territorialControlPercent = 20,
                        isHostile = false,
                        isAllied = false
                    });
                }
            }
        }

        public FactionWarSystemState CaptureState()
        {
            var copy = new FactionWarSystemState
            {
                activeWarTension = _state.activeWarTension,
                dominantFactionId = _state.dominantFactionId,
                totalArtilleryStrikesLogged = _state.totalArtilleryStrikesLogged,
                enactedDecrees = _state.enactedDecrees != null
                    ? new List<string>(_state.enactedDecrees)
                    : new List<string>(),
                factions = new List<FactionStandingRecord>()
            };

            if (_state.factions != null)
            {
                foreach (var f in _state.factions)
                {
                    if (f == null) continue;
                    copy.factions.Add(new FactionStandingRecord
                    {
                        factionId = f.factionId,
                        standing = f.standing,
                        territorialControlPercent = f.territorialControlPercent,
                        isHostile = f.isHostile,
                        isAllied = f.isAllied
                    });
                }
            }

            return copy;
        }

        /// <summary>
        /// Restores a captured faction-war snapshot into the live state, then
        /// re-applies the default faction rows so a roster added after the save
        /// was written still has a record. A null state is a no-op.
        /// </summary>
        public void RestoreState(FactionWarSystemState state)
        {
            if (state == null) return;
            _state.activeWarTension = state.activeWarTension;
            _state.dominantFactionId = state.dominantFactionId ?? "faction_central_garrison";
            _state.totalArtilleryStrikesLogged = state.totalArtilleryStrikesLogged;
            _state.enactedDecrees = state.enactedDecrees != null
                ? new List<string>(state.enactedDecrees)
                : new List<string>();

            _state.factions.Clear();
            if (state.factions != null)
            {
                foreach (var f in state.factions)
                {
                    if (f == null || string.IsNullOrEmpty(f.factionId)) continue;
                    _state.factions.Add(new FactionStandingRecord
                    {
                        factionId = f.factionId,
                        standing = f.standing,
                        territorialControlPercent = f.territorialControlPercent,
                        isHostile = f.isHostile,
                        isAllied = f.isAllied
                    });
                }
            }
            EnsureDefaultFactions();
        }
    }
}
