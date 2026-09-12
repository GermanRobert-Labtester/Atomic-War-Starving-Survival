// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Radio
{
    [Serializable]
    public class PsyOpsCampaignSaveEntry
    {
        public string campaignId = string.Empty;
        public string targetFactionId = string.Empty;
        public string messageTheme = string.Empty;
        public float baseReach;
        public float powerDemandWatts;
        public float receptiveness;
        public float loyaltyPressurePerDay;
        public int startedDay;
        public int durationDays;
        public int daysElapsed;
        public int status;
    }

    [Serializable]
    public class PsyOpsJammingSaveEntry
    {
        public string targetFactionId = string.Empty;
        public float strength;
        public int daysRemaining;
    }

    [Serializable]
    public class PsyOpsCounterSaveEntry
    {
        public string campaignId = string.Empty;
        public int daysRemaining;
    }

    /// <summary>
    /// ASHFALL — psyops save state (Version 1): campaign instances, jamming,
    /// counter-propaganda, ideological-pressure ledger, fatigue. Versioned +
    /// checksummed via <see cref="PsyOpsSaveCodec"/>.
    /// </summary>
    [Serializable]
    public class PsyOpsSaveState
    {
        public int saveVersion = PsyOpsSaveCodec.CurrentSaveVersion;
        public List<PsyOpsCampaignSaveEntry> campaigns = new List<PsyOpsCampaignSaveEntry>();
        public List<PsyOpsJammingSaveEntry> jamming = new List<PsyOpsJammingSaveEntry>();
        public List<PsyOpsCounterSaveEntry> counters = new List<PsyOpsCounterSaveEntry>();
        public List<string> pressureFactionIds = new List<string>();
        public List<float> pressureValues = new List<float>();
        public List<string> fatigueFactionIds = new List<string>();
        public List<int> fatigueDays = new List<int>();

        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Psyops save codec: checksum recomputed on encode, hard-reject on decode
    /// for tamper / checksumless / newer-version payloads. Old saves without
    /// this section load as "no campaigns, neutral pressure".
    /// </summary>
    public static class PsyOpsSaveCodec
    {
        public const int CurrentSaveVersion = 1;

        public static string Encode(PsyOpsSaveState state, IJsonSerializer json)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (json == null) throw new ArgumentNullException(nameof(json));
            state.saveVersion = CurrentSaveVersion;
            state.Checksum = SaveChecksum.Compute(state);
            return json.Serialize(state);
        }

        public static bool TryDecode(string json, IJsonSerializer serializer, out PsyOpsSaveState state)
        {
            state = null!;
            if (string.IsNullOrEmpty(json) || serializer == null) return false;
            try
            {
                var decoded = serializer.Deserialize<PsyOpsSaveState>(json);
                if (decoded == null) return false;
                if (decoded.saveVersion > CurrentSaveVersion) return false;
                if (decoded.saveVersion < CurrentSaveVersion) return false;

                if (string.IsNullOrEmpty(decoded.Checksum)) return false;
                if (!string.Equals(SaveChecksum.Compute(decoded), decoded.Checksum, StringComparison.Ordinal))
                    return false;

                decoded.campaigns ??= new List<PsyOpsCampaignSaveEntry>();
                decoded.jamming ??= new List<PsyOpsJammingSaveEntry>();
                decoded.counters ??= new List<PsyOpsCounterSaveEntry>();
                decoded.pressureFactionIds ??= new List<string>();
                decoded.pressureValues ??= new List<float>();
                decoded.fatigueFactionIds ??= new List<string>();
                decoded.fatigueDays ??= new List<int>();
                state = decoded;
                return true;
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("<decode>", "PsyOpsSaveState", ex_CATDIAG);
                return false;
            }
        }

        public static PsyOpsSaveState ToSaveState(PsyOpsState state)
        {
            var save = new PsyOpsSaveState();
            foreach (var c in state.campaigns)
                save.campaigns.Add(new PsyOpsCampaignSaveEntry
                {
                    campaignId = c.campaignId, targetFactionId = c.targetFactionId,
                    messageTheme = c.messageTheme, baseReach = c.baseReach,
                    powerDemandWatts = c.powerDemandWatts, receptiveness = c.receptiveness,
                    loyaltyPressurePerDay = c.loyaltyPressurePerDay, startedDay = c.startedDay,
                    durationDays = c.durationDays, daysElapsed = c.daysElapsed, status = c.status
                });
            foreach (var j in state.jamming)
                save.jamming.Add(new PsyOpsJammingSaveEntry
                { targetFactionId = j.targetFactionId, strength = j.strength, daysRemaining = j.daysRemaining });
            foreach (var c in state.counters)
                save.counters.Add(new PsyOpsCounterSaveEntry
                { campaignId = c.campaignId, daysRemaining = c.daysRemaining });
            save.pressureFactionIds.AddRange(state.pressureFactionIds);
            save.pressureValues.AddRange(state.pressureValues);
            save.fatigueFactionIds.AddRange(state.fatigueFactionIds);
            save.fatigueDays.AddRange(state.fatigueDays);
            return save;
        }

        public static PsyOpsState FromSaveState(PsyOpsSaveState save)
        {
            var state = new PsyOpsState();
            foreach (var c in save.campaigns)
                state.campaigns.Add(new PsyOpsCampaignState
                {
                    campaignId = c.campaignId, targetFactionId = c.targetFactionId,
                    messageTheme = c.messageTheme, baseReach = c.baseReach,
                    powerDemandWatts = c.powerDemandWatts, receptiveness = c.receptiveness,
                    loyaltyPressurePerDay = c.loyaltyPressurePerDay, startedDay = c.startedDay,
                    durationDays = c.durationDays, daysElapsed = c.daysElapsed, status = c.status
                });
            foreach (var j in save.jamming)
                state.jamming.Add(new PsyOpsJammingState
                { targetFactionId = j.targetFactionId, strength = j.strength, daysRemaining = j.daysRemaining });
            foreach (var c in save.counters)
                state.counters.Add(new PsyOpsCounterSaveEntry
                { campaignId = c.campaignId, daysRemaining = c.daysRemaining }.ToState());
            state.pressureFactionIds.AddRange(save.pressureFactionIds);
            state.pressureValues.AddRange(save.pressureValues);
            state.fatigueFactionIds.AddRange(save.fatigueFactionIds);
            state.fatigueDays.AddRange(save.fatigueDays);
            return state;
        }
    }

    internal static class PsyOpsCounterSaveEntryExtensions
    {
        public static PsyOpsCounterState ToState(this PsyOpsCounterSaveEntry entry) =>
            new PsyOpsCounterState { campaignId = entry.campaignId, daysRemaining = entry.daysRemaining };
    }
}
