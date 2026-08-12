using System;
using System.Collections.Generic;
using AtomicWar._Game.Data;
using AtomicWar._Game.Narrative;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Deep Lore — SurvivorNarrativeArcSystem wiring, world history
    /// discovery hooks, and faction lore integration.
    /// </summary>
    public partial class GameBootstrap
    {
        public SurvivorNarrativeArcSystem NarrativeArcSystem { get; private set; }

        private void InitDeepLore()
        {
            NarrativeArcSystem = new SurvivorNarrativeArcSystem
            {
                GrantTrait = (sv, traitId) =>
                {
                    if (sv?.Traits != null && !sv.HasTrait(traitId))
                        sv.Traits.Add(traitId);
                },
                GrantLatentTrait = (sv, traitId) =>
                {
                    if (sv != null) sv.LatentExpertTraitId = traitId;
                },
                ApplyMoraleDelta = (sv, delta) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Morale, delta);
                },
                ApplyStressDelta = (sv, delta) =>
                {
                    NarrativeArcSystem?.AccumulateStress(sv, delta);
                },
                FireNarrativeEvent = (eventId, context) =>
                {
                    TriggerEventById(eventId);
                },
                FindSurvivor = id => FindSurvivorById(id),
                GetDay = () => TimeSystem?.CurrentDay ?? 1,
                GetSurvivors = () => Survivors,
                Rng = new System.Random(_worldSeed + 91)
            };
            _registry.RegisterPerSubstep("narrativeArc",
                h => TickNarrativeArcSurvivors(h));
            _registry.Register<SurvivorNarrativeArcSystem>(NarrativeArcSystem);

            // Personalised identity fields for the 4 named arc survivors
            // (keepsake, profession, belief profile, philosophical stance).
            DeepLoreSurvivorFieldsLoader.LoadInto(Survivors);

            // Wire narrative arc milestone checks
            WireNarrativeArcTriggers();
        }

        private void WireNarrativeArcTriggers()
        {
            if (NarrativeArcSystem == null) return;

            // Aris: triggers on structural damage events
            if (EventRunner != null)
            {
                Action<GameEvent, EventChoice, EventContext> onChoice =
                    (evt, choice, ctx) =>
                    {
                        if (evt?.id == null) return;
                        foreach (var sv in Survivors ?? new List<Survivor>())
                        {
                            if (sv == null || !sv.IsAlive) continue;
                            NarrativeArcSystem.CheckArisMilestone(sv, evt.id);
                            NarrativeArcSystem.CheckVanceMilestone(sv, evt.id);
                        }
                    };
                EventRunner.OnChoiceApplied += onChoice;
                _subscriptions.Track(() => EventRunner.OnChoiceApplied -= onChoice);
            }

            // Maya: triggers on radio frequency decode
            if (RadioTunerSystem != null)
            {
                // Hook into frequency decode events
            }

            // Elena: triggers on ARS diagnosis
            if (MedicalSystem != null)
            {
                // Hook into affliction diagnosis events
            }

            // Wire narrative arc events (branch choices) to the arc system
            if (EventRunner != null)
            {
                Action<GameEvent, EventChoice, EventContext> onChoiceApplied =
                    (evt, choice, ctx) =>
                    {
                        if (evt?.id == null || choice?.ChoiceId == null) return;
                        if (choice.ChoiceId.Contains("branch_a"))
                        {
                            string svId = ExtractSurvivorIdFromEvent(evt.id);
                            var sv = FindSurvivorById(svId);
                            if (sv != null)
                                NarrativeArcSystem.ChooseBranch(sv, "a");
                        }
                        else if (choice.ChoiceId.Contains("branch_b"))
                        {
                            string svId = ExtractSurvivorIdFromEvent(evt.id);
                            var sv = FindSurvivorById(svId);
                            if (sv != null)
                                NarrativeArcSystem.ChooseBranch(sv, "b");
                        }
                        // Advance milestone on advance_narrative_arc effects
                        if (choice.ChoiceId.Contains("advance_narrative_arc") ||
                            choice.ChoiceId.Contains("investigate") ||
                            choice.ChoiceId.Contains("assess") ||
                            choice.ChoiceId.Contains("trace") ||
                            choice.ChoiceId.Contains("review"))
                        {
                            string svId = ExtractSurvivorIdFromEvent(evt.id);
                            if (!string.IsNullOrEmpty(svId))
                            {
                                var sv = FindSurvivorById(svId);
                                if (sv != null && sv.NarrativeArcMilestone < 2)
                                    NarrativeArcSystem.AdvanceMilestone(sv,
                                        sv.NarrativeArcMilestone + 1);
                            }
                        }
                    };
                EventRunner.OnChoiceApplied += onChoiceApplied;
                _subscriptions.Track(() => EventRunner.OnChoiceApplied -= onChoiceApplied);
            }

            // World history discovery via JournalSystem
            if (JournalSystem != null)
            {
                Action<JournalEntry> onEntry = entry =>
                {
                    if (entry?.KnowledgeKey != null &&
                        entry.KnowledgeKey.StartsWith("lore_"))
                    {
                        // Lore codex populated by UI subscription
                    }
                };
                JournalSystem.OnEntryAdded += onEntry;
                _subscriptions.Track(() => JournalSystem.OnEntryAdded -= onEntry);
            }
        }

        private string ExtractSurvivorIdFromEvent(string eventId)
        {
            if (string.IsNullOrEmpty(eventId)) return null;
            if (eventId.Contains("aris_thorne")) return "aris_thorne";
            if (eventId.Contains("maya_lin")) return "maya_lin";
            if (eventId.Contains("victor_vance")) return "victor_vance";
            if (eventId.Contains("elena_rostov")) return "elena_rostov";
            return null;
        }

        private void TickNarrativeArcSurvivors(float gameHours)
        {
            if (Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
                NarrativeArcSystem?.Tick(Survivors[i], gameHours);
        }
    }
}
