using System;
using System.Collections.Generic;
using AtomicWar._Game.Data;
using AtomicWar._Game.Narrative;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Events;
using AtomicWar._Game.Utilities;
using Ashfall.Core.Journal;

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
            _registry.RegisterPerSubstep("narrative_arc",
                h => TickNarrativeArcSurvivors(h));

            // Lore bible 05_FACTIONS — the Currents' code layer.
            BootCurrents();
            // ASHFALL: THE HOLDFAST — Ice Road, census, brine, waystation.
            BootHoldfast();
            // ASHFALL: THE DUTY ROSTER — wall chart, marks, Kess, Ansel.
            BootDutyRoster();
            // ASHFALL: THE STANDING RECORD — room layouts, memory strata, site encounters.
            BootStandingRecord();
            // ASHFALL: NOBODY'S CHARTER — the Crossing's social gate.
            BootNobodyCharter();
            // ASHFALL: THE GLASS ORCHARD (Expansion XI) — bunker greenhouse.
            BootGreenhouse();
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
                Action<Ashfall.Core.Journal.JournalEntry> onEntry = entry =>
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

        // ── Located knowledge (lore bible) ────────────────────────────

        /// <summary>
        /// Expedition first arrival → world_history discovery. Entries land in
        /// the journal via TryAddRawEntry (fire-once per knowledge_key), which
        /// raises OnEntryAdded and unlocks the matching LoreCodexPanel entry.
        /// </summary>
        private void OnExpeditionFirstArrival(string locationId)
        {
            if (JournalSystem == null || string.IsNullOrEmpty(locationId)) return;
            var entries = LoreDiscoveryIndex.EntriesAtLocation(locationId);
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;

            if (entries != null && entries.Count > 0)
            {
                int added = 0;
                for (int i = 0; i < entries.Count; i++)
                {
                    if (JournalSystem.TryAddRawEntry(entries[i].Key, entries[i].Value, null, day) != null)
                        added++;
                }
                if (added > 0)
                    GameLog.Log($"[GameBootstrap] Lore: discovered {added} history entries at '{locationId}'.");
            }

            // Lore bible 04_ENCOUNTERS Part I — meeting the people who live there.
            DiscoverCharactersAtLocation(locationId);

            // Lore bible 05_FACTIONS §8 — the Kittiwake chart (Undertow interlock).
            DiscoverKittiwakeChart(locationId, day);
        }

        /// <summary>
        /// Arrival at the survey launch finds the logbook. The journal records it
        /// and the kittiwake_chart_found flag opens the chart-decision event.
        /// </summary>
        private void DiscoverKittiwakeChart(string locationId, int day)
        {
            if (locationId != "loc_bathymetric_boat") return;

            JournalSystem?.TryAddRawEntry(
                "kittiwake_chart",
                "The Kittiwake's logbook continues eleven days past the Exchange: soundings " +
                "in metres, with timestamps, kept as the flooding happened. It is the only " +
                "accurate chart of the Drown that exists, and the reason any of it can be " +
                "navigated at all.",
                null,
                day);

            SaveSystem?.SetWorldFlag(EventRunner.FlagKittiwakeChartFound, true);
        }

        /// <summary>
        /// First arrival at a location also introduces the characters bound to
        /// it (characters.json location_id), as fire-once journal entries.
        /// </summary>
        private void DiscoverCharactersAtLocation(string locationId)
        {
            var chars = CharactersCatalogLoader.AtLocation(locationId);
            if (chars == null || chars.Count == 0 || JournalSystem == null) return;

            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
            for (int i = 0; i < chars.Count; i++)
            {
                var c = chars[i];
                if (c == null) continue;
                JournalSystem.TryAddRawEntry(
                    "npc_met_" + c.id,
                    $"{c.display_name}. {c.bio}",
                    null,
                    day);
            }
        }

        /// <summary>
        /// Bunker inspection: discover the player_shelter inspection entries
        /// (the Layer-1 wrongnesses, the open door, the nameplates). Called once
        /// when the lore HUD wires — opening the bunker's own history is the
        /// inspection. Fire-once via KnowledgeBase.
        /// </summary>
        private void DiscoverShelterInspectionLore()
        {
            if (JournalSystem == null) return;
            var entries = LoreDiscoveryIndex.ShelterInspectionEntries();
            if (entries == null || entries.Count == 0) return;

            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
            for (int i = 0; i < entries.Count; i++)
                JournalSystem.TryAddRawEntry(entries[i].Key, entries[i].Value, null, day);
        }
    }
}
