using System;
using UnityEngine;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

using AtomicWar._Game.Encounters;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Prompts #901/#903/#904 — resolution for the three narrative encounters.
    ///
    /// <see cref="NarrativeEncounters"/> registered their EncounterSOs, so they
    /// already appeared on expeditions, but the SOs carried no choices and nothing
    /// constructed the Encounter_* classes that hold the outcomes. The encounters
    /// printed their description and ended: no loot, no morale, no faction standing,
    /// nothing saved. This dispatches each choice to the class that owns it, on the
    /// same seam <see cref="Encounter_Roadblock"/> uses.
    /// </summary>
    public partial class ExpeditionSystem
    {
        /// <summary>Ration spent on Matej when the player shares food.</summary>
        private const string PianistFoodItemId = "canned_food";

        /// <summary>
        /// Test helper: resolve one named narrative choice without waiting for the
        /// encounter roll and the belief-weighted choice pick to land on it.
        /// </summary>
        public void ForceNarrativeChoiceForTests(
            ExpeditionState exp, EncounterSO selected, EventChoice chosen) =>
            TryDispatchNarrativeEncounter(exp, selected, chosen, fled: false);

        private void TryDispatchNarrativeEncounter(
            ExpeditionState exp,
            EncounterSO selected,
            EventChoice chosen,
            bool fled)
        {
            if (exp == null || selected == null || fled) return;

            string encounterId = selected.id ?? string.Empty;
            string choiceId = chosen?.ChoiceId ?? string.Empty;

            if (string.Equals(encounterId, NarrativeEncounters.DeadLetterOfficeId, StringComparison.Ordinal))
                ResolveDeadLetterOffice(exp, choiceId);
            else if (string.Equals(encounterId, NarrativeEncounters.WeatherStationId, StringComparison.Ordinal))
                ResolveWeatherStation(exp, choiceId);
            else if (string.Equals(encounterId, NarrativeEncounters.PianistId, StringComparison.Ordinal))
                ResolvePianist(exp, choiceId);
        }

        // ── Prompt #901 — The Dead Letter Office ────────────────────────

        private void ResolveDeadLetterOffice(ExpeditionState exp, string choiceId)
        {
            if (_deadLetterOffice == null) return;
            _deadLetterOffice.DiscoverVan();

            var sv = exp.Survivor;
            switch (choiceId)
            {
                case NarrativeEncounters.ChoiceReadLetters:
                    // ReadAllLetters returns the accumulated penalty rather than
                    // applying it, so the caller decides who absorbs the grief.
                    // Zero means the van was already emptied — no grief, no keepsake.
                    float grief = _deadLetterOffice.ReadAllLetters();
                    if (grief > 0f)
                    {
                        ApplyMoraleDelta(sv, -grief);
                        // They keep one that had nowhere left to go.
                        GrantNarrativeItems(exp, Encounter_DeadLetterOffice.LetterItemId, 1);
                    }
                    break;

                case NarrativeEncounters.ChoiceDeliverLetter:
                    if (_deadLetterOffice.DeliverFactionLetter())
                    {
                        _modifyFactionTrust?.Invoke(
                            Encounter_DeadLetterOffice.TargetFactionId,
                            Encounter_DeadLetterOffice.DeliveryTrustBoost);
                    }
                    break;

                case NarrativeEncounters.ChoiceTakeSupplies:
                    GrantNarrativeItems(
                        exp,
                        Encounter_DeadLetterOffice.SupplyItemId,
                        _deadLetterOffice.TakeSupplies());
                    break;

                case NarrativeEncounters.ChoiceBurnVan:
                    // BurnVan silently no-ops on an already-looted van, so read
                    // the flag first rather than charging morale for nothing.
                    if (!_deadLetterOffice.State.isLooted)
                    {
                        _deadLetterOffice.BurnVan();
                        // Documented as party-wide: everyone still alive watches
                        // the smoke from the hatch, not just whoever lit it.
                        ApplyMoraleToWholeParty(-Encounter_DeadLetterOffice.BurnMoraleHit);
                    }
                    break;
            }
        }

        // ── Prompt #903 — Automated Weather Station ─────────────────────

        private void ResolveWeatherStation(ExpeditionState exp, string choiceId)
        {
            if (_weatherStation == null) return;
            _weatherStation.Discover();

            switch (choiceId)
            {
                case NarrativeEncounters.ChoiceExtractData:
                    if (_weatherStation.ExtractData())
                        _setWorldFlag?.Invoke(Encounter_WeatherStation.ForecastBoostFlag, true);
                    break;

                case NarrativeEncounters.ChoiceTakeSolarPanel:
                    GrantNarrativeItems(
                        exp,
                        Encounter_WeatherStation.SolarCellItemId,
                        _weatherStation.TakeSolarPanel());
                    break;

                case NarrativeEncounters.ChoiceScavengeElectronics:
                    GrantNarrativeItems(
                        exp,
                        Encounter_WeatherStation.ElectronicScrapItemId,
                        _weatherStation.ScavengeElectronics());
                    break;

                case NarrativeEncounters.ChoiceLeaveRunning:
                    ApplyMoraleDelta(exp.Survivor, _weatherStation.LeaveRunning());
                    break;
            }
        }

        // ── Prompt #904 — The Pianist ───────────────────────────────────

        private void ResolvePianist(ExpeditionState exp, string choiceId)
        {
            if (_pianist == null) return;
            _pianist.Meet();

            var sv = exp.Survivor;
            switch (choiceId)
            {
                case NarrativeEncounters.ChoiceListen:
                    ApplyMoraleDelta(sv, _pianist.Listen());
                    break;

                case NarrativeEncounters.ChoiceShareFood:
                    // Sharing costs a ration. Without the consume the choice would
                    // be a free morale boost, so an empty larder means the offer
                    // cannot be made at all. He only needs feeding once, so check
                    // that before spending the food.
                    if (!_pianist.State.hasReceivedFood
                        && _consumeItem != null
                        && _consumeItem(PianistFoodItemId, 1))
                    {
                        ApplyMoraleDelta(sv, _pianist.ShareFood());
                    }
                    break;

                case NarrativeEncounters.ChoiceTellAboutBunker:
                    if (_pianist.TellAboutBunker())
                        _setWorldFlag?.Invoke(Encounter_Pianist.ToldAboutBunkerFlag, true);
                    break;

                case NarrativeEncounters.ChoiceDestroyPiano:
                    // Zero wire means it was already stripped: no second helping
                    // of guilt for a piano that is not there any more.
                    int wire = _pianist.DestroyPiano();
                    if (wire > 0)
                    {
                        GrantNarrativeItems(exp, Encounter_Pianist.WireCutterItemId, wire);
                        ApplyMoraleDelta(sv, -Encounter_Pianist.DestroyPianoMoralePenalty);
                    }
                    break;
            }
        }

        // ── Shared helpers ──────────────────────────────────────────────

        /// <summary>
        /// Add <paramref name="count"/> copies of an item to the expedition's haul.
        /// Stops early when the pack is full — TryAddLoot enforces the weight cap.
        /// </summary>
        private void GrantNarrativeItems(ExpeditionState exp, string itemId, int count)
        {
            if (exp == null || count <= 0 || string.IsNullOrEmpty(itemId)) return;
            for (int i = 0; i < count; i++)
            {
                var def = ResolveWorldItemDefinition(itemId);
                if (def == null) return;
                if (!exp.TryAddLoot(def)) return;
            }
        }

        /// <summary>
        /// Apply a morale delta to every living survivor, for outcomes the fiction
        /// describes as landing on the whole bunker rather than one scavenger.
        /// </summary>
        private void ApplyMoraleToWholeParty(float delta)
        {
            if (Mathf.Approximately(delta, 0f)) return;
            var all = _getAllSurvivors != null ? _getAllSurvivors() : _survivors;
            if (all == null) return;
            for (int i = 0; i < all.Count; i++)
            {
                var s = all[i];
                if (s != null && s.IsAlive) ApplyMoraleDelta(s, delta);
            }
        }
    }
}
