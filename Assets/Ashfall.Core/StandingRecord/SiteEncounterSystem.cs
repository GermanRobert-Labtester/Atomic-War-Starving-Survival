using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE STANDING RECORD — encounters keyed to room, not generic combat.
    /// Aftermath changes the room. Overlay access granted/withdrawn like a Current
    /// (mirror Blank Rows: no raid, rooms go dark of Overlay labour).
    /// Spec: docs/expansions/expansion_03_the_standing_record_plan.md §5.3.
    /// Engine-agnostic; no UnityEngine / Godot / JsonUtility.
    /// </summary>
    [Serializable]
    public class SiteEncounterRecord
    {
        public string id;
        public string roomId;
        public string kind;
        public int dayStarted;
        public int dayResolved = -1;
        public string payload;
        public string aftermathFlag;
    }

    [Serializable]
    public class SiteEncounterState
    {
        public string systemId = SiteEncounterSystem.SystemId;
        public bool expansionUnlocked;
        public int seedSalt = SiteEncounterSystem.SeedOffset;
        public bool overlayAccess = true;
        public int platesScraped;
        public List<string> resolvedIds = new List<string>();
        public List<SiteEncounterRecord> history = new List<SiteEncounterRecord>();
    }

    /// <summary>
    /// Room-keyed site encounters. One per room visit unless crisis. Overlay access:
    /// scrape three plates with no replacement name and labour withdraws (rooms go dark,
    /// not a raid). Seed _worldSeed + 1808.
    /// </summary>
    public sealed class SiteEncounterSystem
    {
        public const string SystemId = "site_encounter_system";
        /// <summary>Utility salt. Spec: _worldSeed + 1808.</summary>
        public const int SeedOffset = 1808;
        public const int OverlayWithdrawPlateCount = 3;

        // Encounter kinds (spec §4.3)
        public const string KindPlateScrewer = "plate_screwer";
        public const string KindIvyOil = "ivy_oil";
        public const string KindPigment = "pigment";
        public const string KindGreaseCopy = "grease_copy";
        public const string KindBrickDig = "brick_dig";
        public const string KindDetonatorCheck = "detonator_check";
        public const string KindGaugeRead = "gauge_read";
        public const string KindReclaimPlate = "reclaim_plate";
        public const string KindStencilRefresh = "stencil_refresh";
        public const string KindVaultSpeak = "vault_speak";

        private readonly HashSet<string> _resolved = new HashSet<string>();

        private SiteEncounterState _state = new SiteEncounterState();

        public event Action<SiteEncounterRecord> OnSiteEncounterStarted;
        public event Action<SiteEncounterRecord> OnSiteEncounterResolved;
        public event Action<bool> OnOverlayAccessChanged;
        public event Action<SiteEncounterState> OnStateChanged;

        public SiteEncounterState State => _state;
        public bool IsUnlocked => _state.expansionUnlocked;
        public bool OverlayAccess => _state.overlayAccess;
        public int PlatesScraped => _state.platesScraped;

        public SiteEncounterSystem() : this(SeedOffset)
        {
        }

        public SiteEncounterSystem(int seedSalt)
        {
            _state.seedSalt = seedSalt;
        }

        public void Initialise(int seedSalt)
        {
            _state.seedSalt = seedSalt;
        }

        public void Unlock()
        {
            if (_state.expansionUnlocked) return;
            _state.expansionUnlocked = true;
            RaiseChanged();
        }

        public bool StartEncounter(string id, string roomId, string kind, int day,
string? aftermathFlag = null, string? payload = null)
        {
            if (!_state.expansionUnlocked) return false;
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(roomId) || string.IsNullOrEmpty(kind))
                return false;
            if (_resolved.Contains(id)) return false;

            var rec = new SiteEncounterRecord
            {
                id = id,
                roomId = roomId,
                kind = kind,
                dayStarted = day,
                aftermathFlag = aftermathFlag,
                payload = payload
            };
            _state.history.Add(rec);
            OnSiteEncounterStarted?.Invoke(rec);
            RaiseChanged();
            return true;
        }

        public bool ResolveEncounter(string id, int day)
        {
            for (int i = 0; i < _state.history.Count; i++)
            {
                SiteEncounterRecord rec = _state.history[i];
                if (rec != null && rec.id == id && rec.dayResolved < 0)
                {
                    rec.dayResolved = day;
                    _resolved.Add(id);
                    _state.resolvedIds.Add(id);
                    OnSiteEncounterResolved?.Invoke(rec);
                    RaiseChanged();
                    return true;
                }
            }
            return false;
        }

        public bool IsResolved(string id)
        {
            return !string.IsNullOrEmpty(id) && _resolved.Contains(id);
        }

        /// <summary>
        /// Scrape a plate without writing a lived name or a Continuity number.
        /// Three scrapes withdraw Overlay labour (rooms go dark; no raid).
        /// </summary>
        public void ScrapePlate(int day)
        {
            _state.platesScraped++;
            if (_state.platesScraped >= OverlayWithdrawPlateCount && _state.overlayAccess)
            {
                _state.overlayAccess = false;
                OnOverlayAccessChanged?.Invoke(false);
            }
            RaiseChanged();
        }

        /// <summary>Overlay access can return (e.g. lived map ending).</summary>
        public void RestoreOverlayAccess()
        {
            if (_state.overlayAccess) return;
            _state.overlayAccess = true;
            OnOverlayAccessChanged?.Invoke(true);
            RaiseChanged();
        }

        public SiteEncounterState CaptureState()
        {
            var copy = new SiteEncounterState
            {
                systemId = _state.systemId,
                expansionUnlocked = _state.expansionUnlocked,
                seedSalt = _state.seedSalt,
                overlayAccess = _state.overlayAccess,
                platesScraped = _state.platesScraped,
                resolvedIds = _state.resolvedIds != null ? new List<string>(_state.resolvedIds) : new List<string>(),
                history = new List<SiteEncounterRecord>()
            };
            if (_state.history != null)
            {
                for (int i = 0; i < _state.history.Count; i++)
                {
                    SiteEncounterRecord r = _state.history[i];
                    if (r == null) continue;
                    copy.history.Add(new SiteEncounterRecord
                    {
                        id = r.id,
                        roomId = r.roomId,
                        kind = r.kind,
                        dayStarted = r.dayStarted,
                        dayResolved = r.dayResolved,
                        payload = r.payload,
                        aftermathFlag = r.aftermathFlag
                    });
                }
            }
            return copy;
        }

        public void RestoreState(SiteEncounterState saved)
        {
            if (saved == null) _state = new SiteEncounterState();
            else
            {
                // Deep-copy: the live system must never alias the envelope's lists.
                var fresh = new SiteEncounterState
                {
                    systemId = saved.systemId,
                    expansionUnlocked = saved.expansionUnlocked,
                    seedSalt = saved.seedSalt,
                    overlayAccess = saved.overlayAccess,
                    platesScraped = saved.platesScraped,
                    resolvedIds = saved.resolvedIds != null
                        ? new List<string>(saved.resolvedIds)
                        : new List<string>()
                };
                fresh.history = new List<SiteEncounterRecord>();
                if (saved.history != null)
                {
                    for (int i = 0; i < saved.history.Count; i++)
                    {
                        SiteEncounterRecord r = saved.history[i];
                        if (r == null) continue;
                        fresh.history.Add(new SiteEncounterRecord
                        {
                            id = r.id,
                            roomId = r.roomId,
                            kind = r.kind,
                            dayStarted = r.dayStarted,
                            dayResolved = r.dayResolved,
                            payload = r.payload,
                            aftermathFlag = r.aftermathFlag
                        });
                    }
                }
                _state = fresh;
            }
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            if (_state.resolvedIds == null) _state.resolvedIds = new List<string>();
            if (_state.history == null) _state.history = new List<SiteEncounterRecord>();
            _resolved.Clear();
            for (int i = 0; i < _state.resolvedIds.Count; i++)
                if (!string.IsNullOrEmpty(_state.resolvedIds[i]))
                    _resolved.Add(_state.resolvedIds[i]);
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
