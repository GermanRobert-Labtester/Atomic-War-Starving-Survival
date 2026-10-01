// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private WorldIncidentHostSession? _worldIncidents;
        private bool _worldIncidentsDirty;

        private void SetupWorldIncidents()
        {
            if (_worldIncidents == null)
            {
                _worldIncidents = WorldIncidentHostSession.Create(_dataDir);
                _worldIncidents.StateChanged += () => _worldIncidentsDirty = true;
                _worldIncidents.Engine.OnIncidentSurfaced += incident =>
                {
                    SetupJournal();
                    _journal?.TryAddRawEntry(
                        $"world_incident_{incident.Id}",
                        $"World incident: {incident.Title}. {incident.BodyText}",
                        null!,
                        _simDay);
                    _journalDirty = true;
                };
                _worldIncidents.Engine.OnIncidentResolved += result =>
                {
                    SetupJournal();
                    _journal?.TryAddRawEntry(
                        $"world_incident_resolved_{result.EventId}",
                        $"World incident resolved: {result.Incident?.Title ?? result.EventId} ({result.ChoiceId}).",
                        null!,
                        _simDay);
                    _journalDirty = true;
                };
                _worldIncidents.Engine.Consequences = new MainWorldIncidentConsequencePort(this);
            }
        }

        private void SaveWorldIncidents()
        {
            if (_worldIncidents == null) return;
            if (CaptureSection(
                    WorldIncidentSaveStore.SectionName,
                    WorldIncidentSaveStore.TryCapturePersisted(_worldIncidents.CaptureSave())))
                _worldIncidentsDirty = false;
        }

        private void OpenWorldIncidentModal()
        {
            SetupWorldIncidents();
            var pending = _worldIncidents?.PendingIncident;
            if (pending == null)
            {
                _statusLabel?.SetDeferred(
                    Godot.Label.PropertyName.Text,
                    "No world incident is waiting for a decision.");
                return;
            }

            var presentation = new NarrativeArcEventDefinition(
                pending.Id,
                pending.Title,
                pending.BodyText,
                pending.Choices
                    .Where(choice => choice.IsExecutable)
                    .Select(choice => (choice.ChoiceId, choice.Text)));
            _narrativeArcModal.Display(presentation, _simDay);
        }

        private bool ResolveWorldIncidentChoice(string eventId, string choiceId)
        {
            SetupWorldIncidents();
            if (_worldIncidents == null) return false;

            var result = _worldIncidents.Resolve(eventId, choiceId, _simDay);
            if (!result.Succeeded || result.Choice == null)
            {
                _statusLabel?.SetDeferred(
                    Godot.Label.PropertyName.Text,
                    "World incident choice refused: " + result.Reason);
                return false;
            }

            SaveWorldIncidents();
            _journalDirty = true;
            _statusLabel?.SetDeferred(
                Godot.Label.PropertyName.Text,
                $"Incident recorded: {result.Incident?.Title ?? result.EventId}.");
            _narrativeArcModal.DisplayOutcome(
                $"{result.Incident?.Title ?? result.EventId} recorded in the journal.");
            return true;
        }

        /// <summary>
        /// Binds the incident engine's consequence port to the existing host
        /// owners: shared inventory, shelter-resident needs and radiation
        /// dose, the campaign flag ledger, weather, morale, and faction
        /// standing. The authored need id "radiation" is bound to the dose
        /// ledger (AdjustDose on shelter residents); every other unmapped
        /// need fails closed with a reason.
        /// </summary>
        private sealed class MainWorldIncidentConsequencePort : IWorldIncidentConsequencePort
        {
            private readonly Main _m;
            public MainWorldIncidentConsequencePort(Main m) => _m = m;

            public bool CanApplyMorale(double delta, out string reason)
            {
                _m.SetupSurvivors();
                if (_m._survivors == null)
                { reason = "survivor needs authority is not bound"; return false; }
                reason = string.Empty;
                return true;
            }

            public void ApplyMorale(double delta)
            {
                if (_m._survivors == null) return;
                foreach (var survivor in _m.LivingShelterResidents())
                    _m._survivors.Needs.Modify(survivor, NeedKind.Morale, (float)delta);
            }

            public bool CanGrantItem(string itemId, int amount, out string reason)
            {
                _m.SetupInventory();
                var inventory = _m._inventory?.Inventory;
                if (inventory == null)
                { reason = "inventory authority is not bound"; return false; }
                if (amount < 0 && inventory.CountById(itemId) < -amount)
                { reason = "not enough '" + itemId + "' in storage"; return false; }
                reason = string.Empty;
                return true;
            }

            public void GrantItem(string itemId, int amount)
            {
                _m.SetupInventory();
                var inventory = _m._inventory?.Inventory;
                if (inventory == null) return;
                if (amount > 0) inventory.AddById(itemId, amount);
                else if (amount < 0) inventory.RemoveById(itemId, -amount);
            }

            public bool CanApplyNeedDelta(string needId, double delta, out string reason)
            {
                _m.SetupSurvivors();
                if (_m._survivors == null)
                { reason = "survivor needs authority is not bound"; return false; }
                var name = (needId ?? string.Empty).Trim().ToLowerInvariant();
                if (name == "radiation" || TryParseNeed(name, out _))
                { reason = string.Empty; return true; }
                reason = "need '" + needId + "' has no bound owner";
                return false;
            }

            public void ApplyNeedDelta(string needId, double delta)
            {
                if (_m._survivors == null) return;
                var name = (needId ?? string.Empty).Trim().ToLowerInvariant();
                if (name == "radiation")
                {
                    foreach (var survivor in _m.LivingShelterResidents())
                    {
                        var rad = _m._survivors.RadStateFor(survivor.Id);
                        if (rad != null) _m._survivors.Radiation.AdjustDose(rad, (float)delta);
                    }
                    return;
                }
                if (TryParseNeed(name, out var kind))
                    foreach (var survivor in _m.LivingShelterResidents())
                        _m._survivors.Needs.Modify(survivor, kind, (float)delta);
            }

            public bool CanSetWorldFlag(string flagId, bool value, out string reason)
            {
                if (string.IsNullOrWhiteSpace(flagId))
                { reason = "world flag id is empty"; return false; }
                reason = string.Empty;
                return true;
            }

            public void SetWorldFlag(string flagId, bool value)
            {
                if (value)
                    _m._consequenceLedger.Set(
                        flagId,
                        WorldIncidentSystem.SystemId,
                        flagId,
                        _m._simDay);
                else
                    _m._consequenceLedger.Clear(flagId);
            }

            public bool IsWorldFlagSet(string flagId)
                => _m._consequenceLedger.IsSet(flagId);

            public bool WeatherIs(string weatherKindName)
            {
                var world = _m._world;
                if (world == null) return false;
                if (!Enum.TryParse<Ashfall.Core.WeatherKind>(
                        weatherKindName, ignoreCase: true, out var kind))
                    return false;
                return world.Weather.Current == kind;
            }

            public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason)
            {
                var (ok, why) = _m.CanApplyNarrativeStanding(canonicalFactionId, delta);
                reason = why;
                return ok;
            }

            public void ApplyFactionStanding(string canonicalFactionId, int delta)
                => _m.ApplyNarrativeStanding(canonicalFactionId, delta);
        }
    }
}
