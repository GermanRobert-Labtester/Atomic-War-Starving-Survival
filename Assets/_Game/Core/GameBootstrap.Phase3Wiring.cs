using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Events;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Phase 3 — Wire somatic flashbacks, moral branching, and chemical
    /// dependency expansion into live gameplay.
    ///
    ///   SomaticFlashbackSystem → AudioEventBus (siren/explosion events)
    ///   MoralBranchingSystem   → EventRunner.OnChoiceApplied
    ///   ChemicalDependencySystem → Item consumption + AddictionSystem hooks
    ///
    /// Also adds the chemical dependency item tag mapping.
    /// </summary>
    public partial class GameBootstrap
    {
        /// <summary>
        /// Call during InitializeSystems, after AudioEventBus and
        /// EventRunner and AddictionSystem are constructed.
        /// </summary>
        private void InitPhase3Wiring()
        {
            // ── Somatic Flashback: wire into AudioEventBus ──────────────
            WireSomaticFlashbackToAudio();

            // ── Moral Branching: wire into EventRunner choices ──────────
            WireMoralBranchingToEventRunner();

            // ── Chemical Dependency: wire into AddictionSystem ──────────
            WireChemicalDependencyToAddiction();
        }

        // ── Somatic Flashback Wiring ───────────────────────────────────

        private void WireSomaticFlashbackToAudio()
        {
            if (SomaticFlashbackSystem == null) return;

            // AudioEventBus is a process-wide service — ensure one exists
            // and share it with FalloutStormHazardSystem if it was built
            // without one.
            if (_audioBus == null)
            {
                _audioBus = new AudioEventBus();
                if (FalloutStormHazard != null)
                    FalloutStormHazard.SetAudioBus(_audioBus);
            }

            // Subscribe to emergency siren state changes — sirens are the
            // strongest somatic flashback trigger.
            Action<EmergencySirenAudioEvent> onSirenChanged = (sirenEvent) =>
            {
                if (sirenEvent.IsActive)
                    SomaticFlashbackSystem.OnAudioEvent("siren", 0.9f);
            };
            _audioBus.OnEmergencySirenStateChanged += onSirenChanged;
            _subscriptions.Track(() =>
                _audioBus.OnEmergencySirenStateChanged -= onSirenChanged);

            // Subscribe to raid resolutions as explosion-like triggers.
            // The EventBus is process-wide and is intentionally NOT cleared on
            // scene teardown (see GameBootstrap.Lifecycle.cs), so this lambda
            // must be tracked for explicit unsubscribe — otherwise it leaks the
            // old bootstrap and accumulates one extra stale handler per reload
            // (each raid would then fire the explosion flashback N times).
            Action<RaidResolution> onRaidResolved = (resolution) =>
            {
                if (resolution != null && resolution.Launched)
                    SomaticFlashbackSystem.OnAudioEvent("explosion", 0.7f);
            };
            EventBus.Subscribe(onRaidResolved);
            _subscriptions.Track(() => EventBus.Unsubscribe(onRaidResolved));
        }

        // ── Moral Branching Wiring ─────────────────────────────────────

        private void WireMoralBranchingToEventRunner()
        {
            if (MoralBranchingSystem == null || EventRunner == null) return;

            Action<GameEvent, EventChoice, EventContext> onChoiceApplied =
                (gameEvent, choice, context) =>
                {
                    if (choice == null) return;
                    string choiceId = choice.ChoiceId ?? "";

                    // Determine if this choice is empathy-driven or pragmatism-driven
                    bool isEmpathy = IsEmpathyChoice(choiceId);

                    if (Survivors == null) return;
                    for (int i = 0; i < Survivors.Count; i++)
                    {
                        var sv = Survivors[i];
                        if (sv == null || !sv.IsAlive) continue;
                        if (sv.HasMoralBranch) continue; // already branched
                        MoralBranchingSystem.RegisterMoralChoice(sv, isEmpathy);
                    }
                };

            EventRunner.OnChoiceApplied += onChoiceApplied;
            _subscriptions.Track(() =>
                EventRunner.OnChoiceApplied -= onChoiceApplied);

            // Also wire tragedy witnessing
            Action<Survivor> onSurvivorDied = (dead) =>
            {
                if (dead == null || Survivors == null) return;
                for (int i = 0; i < Survivors.Count; i++)
                {
                    if (Survivors[i] != dead)
                        MoralBranchingSystem.OnTragedyWitnessed(Survivors[i]);
                }
            };

            if (NeedsSystem != null)
            {
                NeedsSystem.OnDied += onSurvivorDied;
                _subscriptions.Track(() => NeedsSystem.OnDied -= onSurvivorDied);
            }
        }

        /// <summary>
        /// Classify a choice as empathy-driven or pragmatism-driven based
        /// on its choice ID keyword heuristics.
        /// </summary>
        private bool IsEmpathyChoice(string choiceId)
        {
            if (string.IsNullOrEmpty(choiceId)) return false;

            // Empathy-driven keywords
            if (choiceId.Contains("share") || choiceId.Contains("help") ||
                choiceId.Contains("save") || choiceId.Contains("rescue") ||
                choiceId.Contains("accept") || choiceId.Contains("forgive") ||
                choiceId.Contains("comfort") || choiceId.Contains("protect") ||
                choiceId.Contains("sacrifice") || choiceId.Contains("give"))
                return true;

            // Pragmatism-driven keywords
            if (choiceId.Contains("refuse") || choiceId.Contains("deny") ||
                choiceId.Contains("hoard") || choiceId.Contains("keep") ||
                choiceId.Contains("execute") || choiceId.Contains("abandon") ||
                choiceId.Contains("leave") || choiceId.Contains("take_all") ||
                choiceId.Contains("prioritize") || choiceId.Contains("sacrifice_other"))
                return false;

            // Default: slightly weighted toward empathy for ambiguous choices
            return true;
        }

        // ── Chemical Dependency Wiring ─────────────────────────────────

        private void WireChemicalDependencyToAddiction()
        {
            if (ChemicalDependencySystem == null) return;

            // Map item IDs to chemical dependency kinds
            RegisterChemicalDependencyItems();

            // Hook into existing AddictionSystem's consumption tracking
            if (Addiction != null)
            {
                Action<Survivor> onAddicted = (sv) =>
                {
                    // When AddictionSystem detects addiction, also register
                    // with ChemicalDependencySystem if the item is known
                };
                Addiction.OnAddicted += onAddicted;
                _subscriptions.Track(() => Addiction.OnAddicted -= onAddicted);
            }

            // Hook into item consumption via Inventory
            if (Inventory != null)
            {
                Action<ItemDefinition, int> onItemRemoved =
                    (itemDef, amount) =>
                    {
                        if (itemDef == null || amount <= 0) return;
                        var kind = GetChemicalDependencyKindForItem(itemDef.id);
                        if (kind == null) return;

                        if (Survivors == null) return;
                        for (int i = 0; i < Survivors.Count; i++)
                        {
                            var sv = Survivors[i];
                            if (sv == null || !sv.IsAlive) continue;
                            if (sv.State != SurvivorState.Incapacitated &&
                                sv.State != SurvivorState.Dead)
                            {
                                ChemicalDependencySystem.OnSubstanceConsumed(
                                    sv, itemDef.id, kind.Value);
                                break; // only one survivor per consumption
                            }
                        }
                    };

                Inventory.OnItemRemoved += onItemRemoved;
                _subscriptions.Track(() => Inventory.OnItemRemoved -= onItemRemoved);
            }
        }

        private void RegisterChemicalDependencyItems()
        {
            // Opioids
            AddDependencyItem("morphine", ChemicalDependencyKind.Opioid);
            AddDependencyItem("opium", ChemicalDependencyKind.Opioid);
            AddDependencyItem("painkiller_opioid", ChemicalDependencyKind.Opioid);

            // Alcohol
            AddDependencyItem("alcohol", ChemicalDependencyKind.Alcohol);
            AddDependencyItem("vodka", ChemicalDependencyKind.Alcohol);
            AddDependencyItem("whiskey", ChemicalDependencyKind.Alcohol);
            AddDependencyItem("moonshine", ChemicalDependencyKind.Alcohol);

            // Stimulants
            AddDependencyItem("amphetamines", ChemicalDependencyKind.Stimulant);
            AddDependencyItem("caffeine_pills", ChemicalDependencyKind.Stimulant);
            AddDependencyItem("stimulant", ChemicalDependencyKind.Stimulant);

            // Sedatives
            AddDependencyItem("sedative", ChemicalDependencyKind.Sedative);
            AddDependencyItem("sleeping_pills", ChemicalDependencyKind.Sedative);
            AddDependencyItem("tranquilizer", ChemicalDependencyKind.Sedative);
        }

        private readonly Dictionary<string, ChemicalDependencyKind> _chemicalItemKinds =
            new Dictionary<string, ChemicalDependencyKind>();

        private void AddDependencyItem(string itemId, ChemicalDependencyKind kind)
        {
            _chemicalItemKinds[itemId] = kind;
        }

        private ChemicalDependencyKind? GetChemicalDependencyKindForItem(string itemId)
        {
            if (_chemicalItemKinds.TryGetValue(itemId, out var kind))
                return kind;
            return null;
        }
    }
}
