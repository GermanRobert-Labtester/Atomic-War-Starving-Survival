// SPDX-License-Identifier: MIT
// ASHFALL survivor narrative questline host triad (Plan 104 runtime wiring).
// Save enrollment for the narrative_questlines section.

using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Quests;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private NarrativeQuestlineHostSession? _narrativeQuestlines;
        private bool _narrativeQuestlinesDirty;

        /// <summary>
        /// Feedback strip for the survivor-arc commands. Mirrors the single
        /// <c>LastEvent</c> convention used by the greenhouse and other domains:
        /// one string describing the outcome of the action just performed, with
        /// player-readable blockers rather than raw ids.
        /// </summary>
        public string LastNarrativeArcEvent { get; private set; } = string.Empty;

        public NarrativeQuestlineHostSession? NarrativeQuestlines => _narrativeQuestlines;

        private void SetupNarrativeQuestlines()
        {
            if (_narrativeQuestlines != null) return;
            _narrativeQuestlines = NarrativeQuestlineHostSession.Create(_dataDir, new GodotLog());
            _narrativeQuestlines.StateChanged += () => _narrativeQuestlinesDirty = true;

            var saved = NarrativeQuestlineSaveStore.TryLoad();
            if (saved != null)
                _narrativeQuestlines.RestoreState(saved);
        }

        private void SaveNarrativeQuestlines()
        {
            if (_narrativeQuestlines == null) return;
            if (CaptureSection("narrative_questlines", _narrativeQuestlines.TryCapturePersisted()))
                _narrativeQuestlinesDirty = false;
        }

        private void FlushNarrativeQuestlinesIfDirty()
        {
            if (_narrativeQuestlinesDirty) SaveNarrativeQuestlines();
        }

        private int NarrativeArcDay() => _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;

        private void SetNarrativeArcFeedback(string message)
        {
            LastNarrativeArcEvent = message;
            if (_statusLabel != null) _statusLabel.Text = message;
        }

        /// <summary>
        /// Open the authored arc belonging to a survivor. A survivor carries at
        /// most one arc and an arc is never restarted once opened.
        /// </summary>
        public bool BeginSurvivorArc(string survivorId)
        {
            SetupNarrativeQuestlines();
            if (_narrativeQuestlines == null || string.IsNullOrEmpty(survivorId)) return false;

            var def = _narrativeQuestlines.GetDefinitionForSurvivor(survivorId);
            if (def == null)
            {
                SetNarrativeArcFeedback("This survivor has no unfinished business of their own on record.");
                return false;
            }

            if (_narrativeQuestlines.GetArc(survivorId) != null)
            {
                SetNarrativeArcFeedback($"{def.title} is already open. It does not start twice.");
                return false;
            }

            if (!_narrativeQuestlines.TryBegin(survivorId, NarrativeArcDay()))
            {
                SetNarrativeArcFeedback($"{def.title} could not be opened.");
                return false;
            }

            var stage = def.FindStage(def.stages.Count > 0 ? def.stages[0].stage : 0);
            SetNarrativeArcFeedback($"{def.title} opened. {stage?.name}: {stage?.description}");
            return true;
        }

        /// <summary>
        /// Record one objective item handed over for a survivor's arc. Only items
        /// the current stage actually asks for are accepted; the host inventory
        /// authority performs the removal, so this reports the arc-side result.
        /// </summary>
        public bool DeliverSurvivorArcObjective(string survivorId, string itemId)
        {
            SetupNarrativeQuestlines();
            if (_narrativeQuestlines == null) return false;

            var arc = _narrativeQuestlines.GetArc(survivorId);
            if (arc == null)
            {
                SetNarrativeArcFeedback("No arc is open for that survivor.");
                return false;
            }

            var def = _narrativeQuestlines.GetDefinitionForSurvivor(survivorId);
            var stage = def?.FindStage(arc.currentStage);
            if (arc.status != NarrativeArcStatus.Active || stage == null)
            {
                SetNarrativeArcFeedback($"{def?.title} is waiting on a decision, not on supplies.");
                return false;
            }

            // Read-only preflight: only an item the current stage still owes can be
            // accepted, so stores are never spent on a delivery the arc will refuse.
            if (!_narrativeQuestlines.GetOutstandingObjectives(survivorId).Contains(itemId))
            {
                SetNarrativeArcFeedback($"{def?.title} does not need that at this stage.");
                return false;
            }

            var stores = _inventory?.Inventory;
            if (stores == null)
            {
                SetNarrativeArcFeedback("The stores are not available to draw from.");
                return false;
            }

            // The arc records the delivery in the same commit that spends the item,
            // so the two authorities can never disagree about whether it was handed over.
            bool recorded = false;
            if (!stores.TryConsume(itemId, 1,
                    onCommitted: () => recorded = _narrativeQuestlines.TryDeliverItem(survivorId, itemId, NarrativeArcDay())))
            {
                SetNarrativeArcFeedback($"{def?.title} needs {itemId}, and it is not in the stores.");
                return false;
            }

            if (!recorded)
            {
                stores.TryProduce(itemId, 1);
                SetNarrativeArcFeedback($"{def?.title} could not record that delivery; the item was returned to stores.");
                return false;
            }

            var advanced = _narrativeQuestlines.GetArc(survivorId);
            var nextStage = def?.FindStage(advanced?.currentStage ?? arc.currentStage);
            if (advanced != null && advanced.status == NarrativeArcStatus.AwaitingBranch)
            {
                SetNarrativeArcFeedback(
                    $"{def?.title} — {nextStage?.name}. {nextStage?.description}");
            }
            else
            {
                var outstanding = _narrativeQuestlines.GetOutstandingObjectives(survivorId);
                SetNarrativeArcFeedback(outstanding.Count == 0
                    ? $"{def?.title} — {nextStage?.name}. {nextStage?.description}"
                    : $"{def?.title} — still owed: {string.Join(", ", outstanding)}.");
            }
            return true;
        }

        /// <summary>
        /// Resolve the crisis fork. Applies the authored morale delta through the
        /// existing narrative morale authority (preflighted, so an unavailable
        /// survivor is reported rather than silently ignored) and records the
        /// granted trait on the arc, matching how other systems record grants.
        /// </summary>
        public bool ChooseSurvivorArcBranch(string survivorId, string branchId)
        {
            SetupNarrativeQuestlines();
            if (_narrativeQuestlines == null) return false;

            var arc = NarrativeArcOrNull(survivorId);
            if (arc == null) return false;

            if (arc.status != NarrativeArcStatus.AwaitingBranch)
            {
                SetNarrativeArcFeedback("That decision is not open. The arc is waiting on supplies, or already ended.");
                return false;
            }

            var def = _narrativeQuestlines!.GetDefinitionForSurvivor(survivorId);
            int day = NarrativeArcDay();

            if (!_narrativeQuestlines.TryChooseBranch(survivorId, branchId, day, out var branch) || branch == null)
            {
                SetNarrativeArcFeedback("That is not one of the two choices this moment offers.");
                return false;
            }

            var (ok, reason) = CanApplyArcMorale(survivorId);
            if (ok) ApplyArcMorale(survivorId, branch.moraleDelta);

            var resolution = def?.FindStage(def.FinalStageIndex);
            SetNarrativeArcFeedback(
                $"{branch.label}. {branch.description} " +
                (ok
                    ? $"Morale {branch.moraleDelta:+0;-0;0}."
                    : $"Morale not applied ({reason}).") +
                (string.IsNullOrEmpty(branch.traitGranted)
                    ? string.Empty
                    : $" Recorded: {branch.traitGranted}.") +
                (resolution != null ? $" {resolution.description}" : string.Empty));

            _narrativeQuestlinesDirty = true;
            return true;
        }

        private NarrativeQuestlineArcState? NarrativeArcOrNull(string survivorId)
        {
            var arc = _narrativeQuestlines?.GetArc(survivorId);
            if (arc == null)
            {
                SetNarrativeArcFeedback("No arc is open for that survivor.");
                return null;
            }
            return arc;
        }

        /// <summary>Survivor ids that have an authored arc available to open.</summary>
        public IReadOnlyList<NarrativeQuestlineDef> NarrativeArcDefinitions()
        {
            SetupNarrativeQuestlines();
            return _narrativeQuestlines?.Definitions ?? (IReadOnlyList<NarrativeQuestlineDef>)new List<NarrativeQuestlineDef>();
        }

        /// <summary>
        /// Single bind path for the quest journal so every entry point (registry
        /// bind action, keyboard route, developer handler) presents the same
        /// surfaces, including the survivor arcs and their label resolvers.
        /// </summary>
        private void BindQuestsPanel()
        {
            SetupNarrativeQuestlines();
            SetupPlans166To169();
            _questsPanel.Bind(
                _core.Quests,
                _expansions?.CrossingQuests,
                _dutyRoster,
                _holdfastRuntime?.Day ?? _simDay,
                _factionBranch?.Coordinator,
                _moralChoice,
                _moralChoiceDefs,
                _narrativeQuestlines,
                ResolveSurvivorArcName,
                ResolveArcItemLabel,
                _proceduralNarrative169);
        }

        /// <summary>
        /// Survivor display name from the enrichment authority. Returns empty when
        /// unresolved so the panel falls back to its own humanized label rather
        /// than ever printing a raw survivor id.
        /// </summary>
        private string ResolveSurvivorArcName(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return string.Empty;
            SetupEnrichment();
            SetupSurvivors();
            var def = _survivors?.Roster?.FindDefinition(survivorId);
            var view = _enrichmentService?.GetView(survivorId, def);
            return string.IsNullOrWhiteSpace(view?.DisplayName) ? string.Empty : view!.DisplayName;
        }

        /// <summary>Item display name from the loaded item catalog; empty when unresolved.</summary>
        private string ResolveArcItemLabel(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return string.Empty;
            SetupInventory();
            var name = _inventory?.Catalog?.Get(itemId)?.displayName;
            return string.IsNullOrWhiteSpace(name) ? string.Empty : name!;
        }

        /// <summary>
        /// Morale preflight for an arc branch. Uses the survivor needs authority
        /// directly rather than the Plan 143 narrative-arc adapter, so this triad
        /// has no dependency on that wiring being present.
        /// </summary>
        private (bool ok, string reason) CanApplyArcMorale(string survivorId)
        {
            SetupSurvivors();
            if (_survivors == null) return (false, "the roster is not available");
            var survivor = _survivors.Find(survivorId);
            if (survivor == null) return (false, "that survivor is not on the roster");
            if (!survivor.IsAliveState) return (false, "that survivor is not alive to feel it");
            return (true, string.Empty);
        }

        private void ApplyArcMorale(string survivorId, int delta)
        {
            SetupSurvivors();
            if (_survivors == null || delta == 0) return;
            _survivors.Needs.Modify(survivorId, NeedKind.Morale, delta);
        }
    }
}
