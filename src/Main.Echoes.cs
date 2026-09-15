// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Ashfall.Core.Radiation;
using Ashfall.Core.Flags;
using AtomicWar.GodotApp.Audio;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private EchoHostSession? _echoes;
        private bool _echoesDirty;

        private void SetupEchoes()
        {
            if (_echoes == null)
            {
                _echoes = EchoHostSession.Create(_dataDir);
                _echoes.StateChanged += () => _echoesDirty = true;
                _echoes.Engine.OnEchoSurfaced += echo =>
                {
                    SetupJournal();
                    _journal?.TryAddRawEntry(
                        $"echo_surfaced_{echo.Id}",
                        $"Field echo surfaced: {echo.Title}.",
                        null!,
                        _simDay);
                    _journalDirty = true;
                };
                _echoes.Engine.OnDelayedConsequenceDue += consequence =>
                {
                    ApplyEchoDelayedConsequence(consequence);
                    SaveEchoes();
                };
            }

            _echoes.ConfigureFlags(_consequenceLedger);
            if (_echoes.Engine.Catalog.Count == 0 && !string.IsNullOrWhiteSpace(_echoes.LastEvent))
                _statusLabel?.SetDeferred(Godot.Label.PropertyName.Text, _echoes.LastEvent);
        }

        private void SaveEchoes()
        {
            if (_echoes == null) return;
            if (CaptureSection(EchoSaveStore.SectionName, EchoSaveStore.TryCapturePersisted(_echoes.CaptureSave())))
                _echoesDirty = false;
        }

        private void OpenEchoModal()
        {
            SetupEchoes();
            var pending = _echoes?.PendingEcho;
            if (pending == null)
            {
                _statusLabel?.SetDeferred(Godot.Label.PropertyName.Text, "No field echo is waiting for a decision.");
                return;
            }

            var presentation = new NarrativeArcEventDefinition(
                pending.Id,
                pending.Title,
                pending.BodyText,
                pending.Choices.Select(choice => (choice.ChoiceId, choice.Text)));
            _narrativeArcModal.Display(presentation, _simDay);
        }

        private bool ResolveEchoChoice(string echoId, string choiceId)
        {
            SetupEchoes();
            if (_echoes == null) return false;

            var result = _echoes.Resolve(echoId, choiceId, _simDay);
            if (!result.Succeeded || result.Choice == null)
            {
                _statusLabel?.SetDeferred(
                    Godot.Label.PropertyName.Text,
                    "Echo choice refused: " + result.Reason);
                return false;
            }

            ApplyEchoResolution(result);
            SaveEchoes();
            _journalDirty = true;
            _statusLabel?.SetDeferred(
                Godot.Label.PropertyName.Text,
                $"Echo recorded: {result.Echo?.Title ?? result.EchoId}.");
            _narrativeArcModal.DisplayOutcome(
                $"{result.Echo?.Title ?? result.EchoId} recorded in the journal.");
            return true;
        }

        private void ApplyEchoResolution(EchoResolutionResult result)
        {
            SetupSurvivors();
            SetupInventory();
            SetupJournal();

            if (result.MoraleDelta != 0d)
            {
                foreach (var survivor in LivingShelterResidents())
                    _survivors.Needs.Modify(survivor, NeedKind.Morale, (float)result.MoraleDelta);
            }

            foreach (var effect in result.Choice?.Effects ?? new List<EchoEffectDefinition>())
                ApplyEchoEffect(effect, result.EchoId, result.ChoiceId);

            var delayed = result.Choice?.DelayedConsequence;
            if (delayed != null)
            {
                // EchoSystem persists the due day. The host records that the
                // consequence was scheduled and applies its effects on due.
                _journal?.TryAddRawEntry(
                    $"echo_delayed_{result.EchoId}_{result.ChoiceId}",
                    $"Delayed echo consequence scheduled ({delayed.DelayHours:0.##}h): {delayed.Title}.",
                    null!,
                    _simDay);
            }
        }

        private void ApplyEchoDelayedConsequence(EchoDelayedConsequenceResult consequence)
        {
            SetupSurvivors();
            SetupInventory();
            SetupJournal();
            var delayed = consequence.Consequence;
            _journal?.TryAddRawEntry(
                $"echo_delayed_due_{consequence.EchoId}_{consequence.ChoiceId}",
                $"Delayed echo consequence: {delayed.Title}. {delayed.Description}",
                null!,
                _simDay);
            foreach (var effect in delayed.Effects)
                ApplyEchoEffect(effect, consequence.EchoId, consequence.ChoiceId);
            _journalDirty = true;
        }

        private void ApplyEchoEffect(EchoEffectDefinition effect, string echoId, string choiceId)
        {
            if (!string.IsNullOrWhiteSpace(effect.SetWorldFlag))
            {
                if (effect.WorldFlagValue)
                {
                    _consequenceLedger.Set(
                        effect.SetWorldFlag,
                        EchoSystem.SystemId,
                        echoId + "/" + choiceId,
                        _simDay);
                }
                else
                {
                    _consequenceLedger.Clear(effect.SetWorldFlag);
                }
            }

            if (!string.IsNullOrWhiteSpace(effect.ItemId) && effect.ItemAmount != 0)
                _inventory?.Inventory.AddById(effect.ItemId, effect.ItemAmount);

            if (!string.IsNullOrWhiteSpace(effect.TargetNeed) && Math.Abs(effect.NeedDelta) > double.Epsilon)
            {
                if (TryParseNeed(effect.TargetNeed, out NeedKind kind))
                {
                    foreach (var survivor in LivingShelterResidents())
                        _survivors.Needs.Modify(survivor, kind, (float)effect.NeedDelta);
                }
            }
        }

        private IEnumerable<SurvivorNeedsState> LivingShelterResidents()
        {
            SetupSurvivors();
            if (_survivors == null) yield break;
            foreach (var survivor in _survivors.RosterState
                .Where(s => s != null && s.IsAliveState)
                .OrderBy(s => s.Id, StringComparer.Ordinal))
            {
                if (_survivors.GetSurvivorLocation(survivor.Id).Kind == SurvivorExposureLocation.ShelterInterior)
                    yield return survivor;
            }
        }

        private static bool TryParseNeed(string authoredNeed, out NeedKind kind)
        {
            switch ((authoredNeed ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "hunger": kind = NeedKind.Hunger; return true;
                case "thirst": kind = NeedKind.Thirst; return true;
                case "fatigue": kind = NeedKind.Fatigue; return true;
                case "warmth": kind = NeedKind.Warmth; return true;
                case "morale": kind = NeedKind.Morale; return true;
                case "health": kind = NeedKind.Health; return true;
                case "hygiene": kind = NeedKind.Hygiene; return true;
                case "numbness": kind = NeedKind.Numbness; return true;
                case "radiationanxiety":
                case "radiation_anxiety":
                    kind = NeedKind.RadiationAnxiety;
                    return true;
                default:
                    kind = default;
                    return false;
            }
        }
    }
}
