using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE DUTY ROSTER — the bunker as a stage.
    /// Timed, flagged, save-safe scenes. Not a procedural chatterbox.
    /// Spec: docs/expansions/expansion_02_the_duty_roster_plan.md §5.2 (Sprint 2).
    /// Engine-agnostic; no UnityEngine / Godot / JsonUtility.
    ///
    /// Hatch-dilemma magnitudes are owned by ExpeditionSystem (Prompt #26).
    /// Do not retune. This system BRIDGES the existing hatch phase — it does
    /// not replace it and does not retune it.
    /// </summary>
    [Serializable]
    public class ShelterEncounterRecord
    {
        public string id;
        public string kind;      // visitor / hatch / night / meal / crowd / illness
        public int dayStarted;
        public int dayResolved = -1;
        public string payload;
        public string visitorId; // e.g. npc_edor_vale, npc_len_quill
    }

    [Serializable]
    public class ShelterEncounterSystemState
    {
        public string systemId = ShelterEncounterSystem.SystemId;
        public bool expansionUnlocked;
        public int seedSalt = ShelterEncounterSystem.SeedOffset;
        public int lastEncounterDay = -1;
        public int encountersThisNight;
        public float encounterWeightMultiplier = 1f;
        public int secondWinterActiveSince = -1;
        public List<ShelterEncounterRecord> history = new List<ShelterEncounterRecord>();
        public List<string> activeVisitorQueue = new List<string>();
        public List<string> resolvedIds = new List<string>();
    }

    public class ShelterEncounterSystem
    {
        public const string SystemId = "shelter_encounter_system";
        /// <summary>Utility AI salt. Spec: _worldSeed + 1208.</summary>
        public const int SeedOffset = 1208;

        // Encounter kinds (spec §4.4)
        public const string KindNightSlate = "night_slate";
        public const string KindHatchReturn = "hatch_return";
        public const string KindMealShort = "meal_short";
        public const string KindIntakeSleep = "intake_sleep";
        public const string KindLevyAbsence = "levy_absence";
        public const string KindIcePack = "ice_pack";
        public const string KindEdorStool = "edor_stool";
        public const string KindPellMachine = "pell_machine";
        public const string KindStackFever = "stack_fever";
        public const string KindChildChart = "child_chart";
        public const string KindTinAgain = "tin_again";
        public const string KindIntercomOffice = "intercom_office";
        public const string KindRoadDarkCrowd = "road_dark_crowd";
        public const string KindSelaRow = "sela_row";

        public const string VisitorEdor = "npc_edor_vale";
        public const string VisitorLen = "npc_len_quill";
        public const string VisitorPell = "npc_sergeant_pell";
        public const string VisitorOffice = "faction_the_office";
        public const string VisitorOverflow = "overflow_runner";

        private ShelterEncounterSystemState _state = new ShelterEncounterSystemState();
        private readonly HashSet<string> _resolved = new HashSet<string>();
        private readonly List<string> _visitorQueue = new List<string>();

        public event Action<ShelterEncounterRecord> OnShelterEncounterStarted;
        public event Action<ShelterEncounterRecord> OnShelterEncounterResolved;
        public event Action<ShelterEncounterSystemState> OnStateChanged;

        public ShelterEncounterSystemState State => _state;
        public bool IsUnlocked => _state.expansionUnlocked;
        public int LastEncounterDay => _state.lastEncounterDay;
        public int EncountersThisNight => _state.encountersThisNight;
        public IReadOnlyList<string> ActiveVisitorQueue => _visitorQueue;

        /// <summary>Reset the per-night encounter counter at the start of a new day.</summary>
        public void ResetNightCounter(int day)
        {
            if (_state.lastEncounterDay != day)
            {
                _state.lastEncounterDay = day;
                _state.encountersThisNight = 0;
                RaiseChanged();
            }
        }

        public float EncounterWeightMultiplier => _state.encounterWeightMultiplier;
        public bool IsSecondWinterActive => _state.secondWinterActiveSince >= 0;

        /// <summary>Second Winter profile: shelter encounters more likely (×1.6).</summary>
        public void SetSecondWinter(float multiplier, int day)
        {
            _state.encounterWeightMultiplier = multiplier <= 0f ? 1f : multiplier;
            _state.secondWinterActiveSince = day;
            RaiseChanged();
        }

        public void ClearSecondWinter()
        {
            _state.encounterWeightMultiplier = 1f;
            _state.secondWinterActiveSince = -1;
            RaiseChanged();
        }

        public ShelterEncounterSystem() : this(SeedOffset)
        {
        }

        public ShelterEncounterSystem(int seedSalt)
        {
            _state.seedSalt = seedSalt;
            EnsureLists();
        }

        public void Initialise(int seedSalt)
        {
            _state.seedSalt = seedSalt;
        }

        public void Unlock(int day)
        {
            if (_state.expansionUnlocked) return;
            _state.expansionUnlocked = true;
            _state.lastEncounterDay = day;
            RaiseChanged();
        }

        /// <summary>
        /// Queue a visitor on the stool/apron. One at a time. If a visitor is
        /// already waiting, the new one waits (spec: do not double-book).
        /// </summary>
        public bool QueueVisitor(string visitorId, int day)
        {
            if (!_state.expansionUnlocked) return false;
            if (string.IsNullOrEmpty(visitorId)) return false;
            if (_visitorQueue.Contains(visitorId)) return false;
            _visitorQueue.Add(visitorId);
            _state.activeVisitorQueue.Add(visitorId);
            RaiseChanged();
            return true;
        }

        public string PeekVisitor()
        {
            return _visitorQueue.Count > 0 ? _visitorQueue[0] : null;
        }

        public bool ResolveVisitor(string visitorId)
        {
            if (string.IsNullOrEmpty(visitorId)) return false;
            int idx = _visitorQueue.IndexOf(visitorId);
            if (idx < 0) return false;
            _visitorQueue.RemoveAt(idx);
            _state.activeVisitorQueue.Remove(visitorId);
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// Start a shelter encounter. Max one per night unless crisis.
        /// Returns false if the cooldown/limit blocks it.
        /// </summary>
        public bool StartEncounter(string id, string kind, int day, string visitorId = null, string payload = null)
        {
            if (!_state.expansionUnlocked) return false;
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(kind)) return false;
            if (_resolved.Contains(id)) return false;

            // One encounter per night unless crisis (spec §5.2 Balance).
            if (_state.lastEncounterDay == day && _state.encountersThisNight >= 1)
                return false;

            var rec = new ShelterEncounterRecord
            {
                id = id,
                kind = kind,
                dayStarted = day,
                visitorId = visitorId,
                payload = payload
            };
            _state.history.Add(rec);
            if (_state.lastEncounterDay != day)
            {
                _state.lastEncounterDay = day;
                _state.encountersThisNight = 0;
            }
            _state.encountersThisNight++;
            OnShelterEncounterStarted?.Invoke(rec);
            RaiseChanged();
            return true;
        }

        /// <summary>Crisis mode: allow multiple encounters in one night (quest_roster_window).</summary>
        public bool StartEncounterCrisis(string id, string kind, int day, string visitorId = null, string payload = null)
        {
            if (!_state.expansionUnlocked) return false;
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(kind)) return false;
            if (_resolved.Contains(id)) return false;

            var rec = new ShelterEncounterRecord
            {
                id = id,
                kind = kind,
                dayStarted = day,
                visitorId = visitorId,
                payload = payload
            };
            _state.history.Add(rec);
            if (_state.lastEncounterDay != day)
            {
                _state.lastEncounterDay = day;
                _state.encountersThisNight = 0;
            }
            _state.encountersThisNight++;
            OnShelterEncounterStarted?.Invoke(rec);
            RaiseChanged();
            return true;
        }

        public bool ResolveEncounter(string id, int day)
        {
            for (int i = 0; i < _state.history.Count; i++)
            {
                var rec = _state.history[i];
                if (rec != null && rec.id == id && rec.dayResolved < 0)
                {
                    rec.dayResolved = day;
                    _resolved.Add(id);
                    _state.resolvedIds.Add(id);
                    OnShelterEncounterResolved?.Invoke(rec);
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

        public ShelterEncounterRecord GetActive(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < _state.history.Count; i++)
            {
                var rec = _state.history[i];
                if (rec != null && rec.id == id && rec.dayResolved < 0)
                    return rec;
            }
            return null;
        }

        public ShelterEncounterSystemState CaptureState()
        {
            var copy = new ShelterEncounterSystemState
            {
                systemId = _state.systemId,
                expansionUnlocked = _state.expansionUnlocked,
                seedSalt = _state.seedSalt,
                lastEncounterDay = _state.lastEncounterDay,
                encountersThisNight = _state.encountersThisNight,
                encounterWeightMultiplier = _state.encounterWeightMultiplier,
                secondWinterActiveSince = _state.secondWinterActiveSince
            };
            copy.history = new List<ShelterEncounterRecord>();
            if (_state.history != null)
            {
                for (int i = 0; i < _state.history.Count; i++)
                {
                    var r = _state.history[i];
                    if (r == null) continue;
                    copy.history.Add(new ShelterEncounterRecord
                    {
                        id = r.id,
                        kind = r.kind,
                        dayStarted = r.dayStarted,
                        dayResolved = r.dayResolved,
                        payload = r.payload,
                        visitorId = r.visitorId
                    });
                }
            }
            copy.activeVisitorQueue = _state.activeVisitorQueue != null
                ? new List<string>(_state.activeVisitorQueue)
                : new List<string>();
            copy.resolvedIds = _state.resolvedIds != null
                ? new List<string>(_state.resolvedIds)
                : new List<string>();
            return copy;
        }

        public void RestoreState(ShelterEncounterSystemState saved)
        {
            if (saved == null) _state = new ShelterEncounterSystemState();
            else
            {
                // Deep-copy: the live system must never alias the envelope's lists.
                var fresh = new ShelterEncounterSystemState
                {
                    systemId = saved.systemId,
                    expansionUnlocked = saved.expansionUnlocked,
                    seedSalt = saved.seedSalt,
                    lastEncounterDay = saved.lastEncounterDay,
                    encountersThisNight = saved.encountersThisNight,
                    encounterWeightMultiplier = saved.encounterWeightMultiplier,
                    secondWinterActiveSince = saved.secondWinterActiveSince
                };
                fresh.history = new List<ShelterEncounterRecord>();
                if (saved.history != null)
                {
                    for (int i = 0; i < saved.history.Count; i++)
                    {
                        var r = saved.history[i];
                        if (r == null) continue;
                        fresh.history.Add(new ShelterEncounterRecord
                        {
                            id = r.id,
                            kind = r.kind,
                            dayStarted = r.dayStarted,
                            dayResolved = r.dayResolved,
                            payload = r.payload,
                            visitorId = r.visitorId
                        });
                    }
                }
                fresh.activeVisitorQueue = saved.activeVisitorQueue != null
                    ? new List<string>(saved.activeVisitorQueue)
                    : new List<string>();
                fresh.resolvedIds = saved.resolvedIds != null
                    ? new List<string>(saved.resolvedIds)
                    : new List<string>();
                _state = fresh;
            }
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            EnsureLists();
            RebuildIndexes();
            RaiseChanged();
        }

        private void EnsureLists()
        {
            if (_state.history == null) _state.history = new List<ShelterEncounterRecord>();
            if (_state.activeVisitorQueue == null) _state.activeVisitorQueue = new List<string>();
            if (_state.resolvedIds == null) _state.resolvedIds = new List<string>();
        }

        private void RebuildIndexes()
        {
            _resolved.Clear();
            _visitorQueue.Clear();
            for (int i = 0; i < _state.resolvedIds.Count; i++)
            {
                if (!string.IsNullOrEmpty(_state.resolvedIds[i]))
                    _resolved.Add(_state.resolvedIds[i]);
            }
            for (int i = 0; i < _state.activeVisitorQueue.Count; i++)
            {
                if (!string.IsNullOrEmpty(_state.activeVisitorQueue[i]))
                    _visitorQueue.Add(_state.activeVisitorQueue[i]);
            }
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}