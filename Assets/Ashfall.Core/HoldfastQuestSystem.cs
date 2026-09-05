using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE HOLDFAST — main questline runtime.
    /// Stage text comes from holdfast_quests.json. Advance() is driven by
    /// arrival / choice via HoldfastSession (B1). Sheet start is a story gate, not day-90-everyone (S1).
    /// </summary>
    [Serializable]
    public class HoldfastQuestProgress
    {
        public string questId;
        public int stage;
        public bool started;
        public bool completed;
        public bool failed;
        public string branchId;
    }

    [Serializable]
    public class HoldfastQuestSystemState
    {
        public string systemId = HoldfastQuestSystem.SystemId;
        public List<HoldfastQuestProgress> quests = new List<HoldfastQuestProgress>();
        public string endingId;
        public bool sheetObtained;
        public bool plantVisited;
        public bool authenticated;
        public bool drawerRead;
    }

    public class HoldfastQuestSystem
    {
        public const string SystemId = "holdfast_quest_system";
        public const string Sheet = "quest_holdfast_the_sheet";
        public const string Clerk = "quest_holdfast_the_clerk";
        public const string Window = "quest_holdfast_the_window";
        public const string Plant = "quest_holdfast_the_plant";
        public const string Authentication = "quest_holdfast_authentication";
        public const string Drawer = "quest_holdfast_the_drawer";
        public const string Levy = "quest_holdfast_the_levy";
        public const string Membrane = "quest_holdfast_the_membrane";
        public const string SecondList = "quest_holdfast_the_second_list";
        public const string Hatch = "quest_holdfast_the_hatch";

        public static readonly string[] MainQuestIds =
        {
            Sheet, Clerk, Window, Plant, Authentication, Drawer, Levy, Membrane, SecondList, Hatch
        };

        public const int SheetMinDay = 90;
        public const int ClerkFallbackDay = 110;

        private HoldfastQuestSystemState _state = new HoldfastQuestSystemState();
        private IReadOnlyList<HoldfastQuestEntry> _catalog = Array.Empty<HoldfastQuestEntry>();

        public event Action<string, int> OnQuestStageChanged;
        public event Action<string> OnQuestStarted;
        public event Action<string> OnQuestCompleted;
        public event Action<HoldfastQuestSystemState> OnStateChanged;

        public HoldfastQuestSystemState State => _state;

        public void BindCatalog(IReadOnlyList<HoldfastQuestEntry> catalog)
        {
            _catalog = catalog ?? Array.Empty<HoldfastQuestEntry>();
        }

        public HoldfastQuestEntry? GetDef(string questId)
        {
            if (_catalog == null || string.IsNullOrEmpty(questId)) return null;
            for (int i = 0; i < _catalog.Count; i++)
                if (_catalog[i] != null && _catalog[i].id == questId)
                    return _catalog[i];
            return null;
        }

        public string GetBriefing(string questId)
        {
            var def = GetDef(questId);
            return def != null ? (def.briefing ?? "") : "";
        }

        public string GetStageText(string questId)
        {
            var def = GetDef(questId);
            if (def == null || def.stages == null || def.stages.Length == 0) return "";
            var p = GetProgress(questId);
            int idx = p == null ? 0 : p.stage;
            if (idx < 0) idx = 0;
            if (idx >= def.stages.Length) idx = def.stages.Length - 1;
            var stage = def.stages[idx];
            return stage != null ? (stage.text ?? "") : "";
        }

        public string GetDisplayName(string questId)
        {
            var def = GetDef(questId);
            return def != null ? (def.display_name ?? questId) : questId ?? "";
        }

        public HoldfastQuestProgress? GetProgress(string questId)
        {
            for (int i = 0; i < _state.quests.Count; i++)
                if (_state.quests[i] != null && _state.quests[i].questId == questId)
                    return _state.quests[i];
            return null;
        }

        public bool IsStarted(string questId)
        {
            var p = GetProgress(questId);
            return p != null && p.started;
        }

        public bool IsCompleted(string questId)
        {
            var p = GetProgress(questId);
            return p != null && p.completed;
        }

        public bool TryStart(string questId, int day)
        {
            if (string.IsNullOrEmpty(questId)) return false;
            if (_catalog.Count > 0 && GetDef(questId) == null && !IsBuiltInQuestId(questId))
                return false;

            var existing = GetProgress(questId);
            if (existing != null && (existing.started || existing.completed || existing.failed))
                return false;
            if (!PrereqsMet(questId, day)) return false;

            var p = existing ?? GetOrCreate(questId);
            p.started = true;
            p.stage = 0;
            OnQuestStarted?.Invoke(questId);
            OnQuestStageChanged?.Invoke(questId, 0);
            RaiseChanged();
            return true;
        }

        public bool Advance(string questId)
        {
            var p = GetProgress(questId);
            if (p == null || !p.started || p.completed || p.failed) return false;
            var def = GetDef(questId);
            int max = def?.StageCount ?? 4;
            p.stage++;
            if (p.stage >= max)
            {
                p.completed = true;
                if (questId == Sheet) _state.sheetObtained = true;
                if (questId == Plant) _state.plantVisited = true;
                if (questId == Authentication) _state.authenticated = true;
                if (questId == Drawer) _state.drawerRead = true;
                OnQuestCompleted?.Invoke(questId);
            }
            OnQuestStageChanged?.Invoke(questId, p.stage);
            RaiseChanged();
            return true;
        }

        public bool ChooseBranch(string questId, string branchId)
        {
            var p = GetProgress(questId);
            if (p == null || !p.started || p.completed) return false;
            p.branchId = branchId ?? "";
            RaiseChanged();
            return Advance(questId);
        }

        public void SetEnding(string endingId)
        {
            _state.endingId = endingId ?? "";
            RaiseChanged();
        }

        /// <summary>
        /// Daily auto-start. Sheet requires a story key (map or lore), not calendar alone (S1).
        /// </summary>
        public void TickDaily(int day, bool hasMapItem, bool hasFormulaLore, bool hasLettersLore)
        {
            bool storyGate = hasMapItem || hasFormulaLore || hasLettersLore;
            if (storyGate && day >= SheetMinDay && !IsStarted(Sheet))
                TryStart(Sheet, day);
            if ((IsStarted(Sheet) || (storyGate && day >= ClerkFallbackDay)) && !IsStarted(Clerk))
                TryStart(Clerk, day);
            if (IsStarted(Clerk) && !IsStarted(Window))
                TryStart(Window, day);
            if (IsCompleted(Window) && !IsStarted(Plant))
                TryStart(Plant, day);
            if (IsCompleted(Plant) && !IsStarted(Authentication))
                TryStart(Authentication, day);
            if (IsCompleted(Authentication) && !IsStarted(Drawer))
                TryStart(Drawer, day);
            if (IsCompleted(Drawer) && !IsStarted(Levy))
                TryStart(Levy, day);
            if (!IsStarted(SecondList) && (IsCompleted(Membrane) || HasRefuseBranch()))
                TryStart(SecondList, day);
        }

        public bool HasRefuseBranch()
        {
            var levy = GetProgress(Levy);
            return levy != null && levy.branchId == CensusClaimSystem.FlagLevyRefuse;
        }

        public HoldfastQuestSystemState CaptureState()
        {
            var copy = new HoldfastQuestSystemState
            {
                systemId = _state.systemId,
                endingId = _state.endingId,
                sheetObtained = _state.sheetObtained,
                plantVisited = _state.plantVisited,
                authenticated = _state.authenticated,
                drawerRead = _state.drawerRead,
                quests = new List<HoldfastQuestProgress>()
            };
            for (int i = 0; i < _state.quests.Count; i++)
            {
                var q = _state.quests[i];
                if (q == null) continue;
                copy.quests.Add(new HoldfastQuestProgress
                {
                    questId = q.questId,
                    stage = q.stage,
                    started = q.started,
                    completed = q.completed,
                    failed = q.failed,
                    branchId = q.branchId
                });
            }
            return copy;
        }

        public void RestoreState(HoldfastQuestSystemState saved)
        {
            // Deep-copy: the deserialized DTO must not become the live state.
            // Otherwise the caller's save object and the running system alias
            // the same list and a later mutation corrupts the envelope.
            _state = saved == null ? new HoldfastQuestSystemState() : CloneState(saved);
            if (_state.quests == null) _state.quests = new List<HoldfastQuestProgress>();
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            RaiseChanged();
        }

        private static HoldfastQuestSystemState CloneState(HoldfastQuestSystemState src)
        {
            var copy = new HoldfastQuestSystemState
            {
                systemId = src.systemId,
                endingId = src.endingId,
                sheetObtained = src.sheetObtained,
                plantVisited = src.plantVisited,
                authenticated = src.authenticated,
                drawerRead = src.drawerRead,
                quests = new List<HoldfastQuestProgress>()
            };
            if (src.quests != null)
            {
                for (int i = 0; i < src.quests.Count; i++)
                {
                    var q = src.quests[i];
                    if (q == null) continue;
                    copy.quests.Add(new HoldfastQuestProgress
                    {
                        questId = q.questId,
                        stage = q.stage,
                        started = q.started,
                        completed = q.completed,
                        failed = q.failed,
                        branchId = q.branchId
                    });
                }
            }
            return copy;
        }

        private bool PrereqsMet(string questId, int day)
        {
            switch (questId)
            {
                case Sheet: return day >= SheetMinDay;
                case Clerk: return IsStarted(Sheet) || day >= ClerkFallbackDay;
                case Window: return IsStarted(Clerk);
                case Plant: return IsCompleted(Window) || IsStarted(Window);
                case Authentication: return IsCompleted(Plant) || _state.plantVisited;
                case Drawer: return IsCompleted(Authentication) || _state.authenticated;
                case Levy: return IsCompleted(Drawer) || _state.drawerRead;
                case Membrane: return IsStarted(Levy) || IsCompleted(Levy);
                case SecondList: return IsCompleted(Membrane) || HasRefuseBranch();
                case Hatch: return IsStarted(SecondList) || !string.IsNullOrEmpty(_state.endingId);
                default: return true;
            }
        }

        private HoldfastQuestProgress GetOrCreate(string questId)
        {
            var p = GetProgress(questId);
            if (p != null) return p;
            p = new HoldfastQuestProgress { questId = questId };
            _state.quests.Add(p);
            return p;
        }

        private static bool IsBuiltInQuestId(string questId)
        {
            for (int i = 0; i < MainQuestIds.Length; i++)
                if (MainQuestIds[i] == questId) return true;
            return false;
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
