using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Survivors
{
    [Serializable]
    public class ContagionSourceSaveEntry
    {
        public string eventId = string.Empty;
        public int emotion; // (int) MoraleEmotion
        public string sourceSurvivorId = string.Empty;
        public float intensity;
        public float bondMultiplier = 1f;
        public float proximityMultiplier = 1f;
        public float recoveryPerDay = 0.2f;
        public int startedDay;
        public int expiresDay;
    }

    [Serializable]
    public class SurvivorContagionPressureSaveEntry
    {
        public string survivorId = string.Empty;
        public float hopePressure;
        public float despairPressure;
        public float panicPressure;
        public int lastBreakdownDay = -1;
        public bool wasInBreakdownBand;
        public int isolationEndsDay = -1;
    }

    [Serializable]
    public class SubgroupSchismPressureSaveEntry
    {
        public string subgroupId = string.Empty;
        public int consecutivePressureDays;
    }

    /// <summary>
    /// ASHFALL — morale contagion save state (Version 1). Owns the authoritative
    /// contagion state: active sources (snapshot template values), per-survivor
    /// channel pressure, isolation markers, breakdown bookkeeping, and the schism
    /// pressure ledger + cooldown. Versioned + checksummed via
    /// <see cref="MoraleContagionSaveCodec"/>.
    /// </summary>
    [Serializable]
    public class MoraleContagionSaveState
    {
        public int saveVersion = MoraleContagionSaveCodec.CurrentSaveVersion;
        public List<ContagionSourceSaveEntry> activeSources = new List<ContagionSourceSaveEntry>();
        public List<SurvivorContagionPressureSaveEntry> survivors = new List<SurvivorContagionPressureSaveEntry>();
        public List<SubgroupSchismPressureSaveEntry> subgroupPressure = new List<SubgroupSchismPressureSaveEntry>();
        public int schismCooldownUntilDay = -1;
        public int lastSchismDay = -1;

        /// <summary>Host-owned HopeBeacon installation day (-1 = not installed).</summary>
        public int hopeBeaconInstalledDay = -1;

        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Morale contagion save codec: checksum recomputed on encode, hard-reject on
    /// decode for tamper / checksumless / newer-version payloads (mirrors
    /// <see cref="RadioSaveCodec"/>). Old saves without this section simply load
    /// as "no contagion state" (the host keeps fresh state on null).
    /// </summary>
    public static class MoraleContagionSaveCodec
    {
        public const int CurrentSaveVersion = 1;

        public static string Encode(MoraleContagionSaveState state, IJsonSerializer json)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (json == null) throw new ArgumentNullException(nameof(json));
            state.saveVersion = CurrentSaveVersion;
            state.Checksum = SaveChecksum.Compute(state);
            return json.Serialize(state);
        }

        public static bool TryDecode(string json, IJsonSerializer serializer, out MoraleContagionSaveState state)
        {
            state = null!;
            if (string.IsNullOrEmpty(json) || serializer == null) return false;
            try
            {
                var decoded = serializer.Deserialize<MoraleContagionSaveState>(json);
                if (decoded == null) return false;
                if (decoded.saveVersion > CurrentSaveVersion) return false;  // future — reject
                if (decoded.saveVersion < CurrentSaveVersion) return false;  // no older format exists

                if (string.IsNullOrEmpty(decoded.Checksum)) return false;    // malformed new format — reject
                if (!string.Equals(SaveChecksum.Compute(decoded), decoded.Checksum, StringComparison.Ordinal))
                    return false;                                            // tampered

                EnsureCollections(decoded);
                state = decoded;
                return true;
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("<decode>", "MoraleContagionSaveState", ex_CATDIAG);
                return false;
            }
        }

        private static void EnsureCollections(MoraleContagionSaveState state)
        {
            if (state.activeSources == null) state.activeSources = new List<ContagionSourceSaveEntry>();
            if (state.survivors == null) state.survivors = new List<SurvivorContagionPressureSaveEntry>();
            if (state.subgroupPressure == null) state.subgroupPressure = new List<SubgroupSchismPressureSaveEntry>();
        }

        // ------------------------------------------------------ domain mapping

        public static MoraleContagionSaveState ToSaveState(MoraleContagionState state)
        {
            var save = new MoraleContagionSaveState
            {
                schismCooldownUntilDay = state.schismCooldownUntilDay,
                lastSchismDay = state.lastSchismDay
            };
            foreach (var source in state.activeSources)
                save.activeSources.Add(new ContagionSourceSaveEntry
                {
                    eventId = source.eventId,
                    emotion = source.emotion,
                    sourceSurvivorId = source.sourceSurvivorId,
                    intensity = source.intensity,
                    bondMultiplier = source.bondMultiplier,
                    proximityMultiplier = source.proximityMultiplier,
                    recoveryPerDay = source.recoveryPerDay,
                    startedDay = source.startedDay,
                    expiresDay = source.expiresDay
                });
            foreach (var survivor in state.survivors)
                save.survivors.Add(new SurvivorContagionPressureSaveEntry
                {
                    survivorId = survivor.survivorId,
                    hopePressure = survivor.hopePressure,
                    despairPressure = survivor.despairPressure,
                    panicPressure = survivor.panicPressure,
                    lastBreakdownDay = survivor.lastBreakdownDay,
                    wasInBreakdownBand = survivor.wasInBreakdownBand,
                    isolationEndsDay = survivor.isolationEndsDay
                });
            foreach (var ledger in state.subgroupPressure)
                save.subgroupPressure.Add(new SubgroupSchismPressureSaveEntry
                {
                    subgroupId = ledger.subgroupId,
                    consecutivePressureDays = ledger.consecutivePressureDays
                });
            return save;
        }

        public static MoraleContagionState FromSaveState(MoraleContagionSaveState save)
        {
            var state = new MoraleContagionState
            {
                schismCooldownUntilDay = save.schismCooldownUntilDay,
                lastSchismDay = save.lastSchismDay
            };
            foreach (var source in save.activeSources)
                state.activeSources.Add(new ContagionSourceState
                {
                    eventId = source.eventId,
                    emotion = source.emotion,
                    sourceSurvivorId = source.sourceSurvivorId,
                    intensity = source.intensity,
                    bondMultiplier = source.bondMultiplier,
                    proximityMultiplier = source.proximityMultiplier,
                    recoveryPerDay = source.recoveryPerDay,
                    startedDay = source.startedDay,
                    expiresDay = source.expiresDay
                });
            foreach (var survivor in save.survivors)
                state.survivors.Add(new SurvivorContagionPressureState
                {
                    survivorId = survivor.survivorId,
                    hopePressure = survivor.hopePressure,
                    despairPressure = survivor.despairPressure,
                    panicPressure = survivor.panicPressure,
                    lastBreakdownDay = survivor.lastBreakdownDay,
                    wasInBreakdownBand = survivor.wasInBreakdownBand,
                    isolationEndsDay = survivor.isolationEndsDay
                });
            foreach (var ledger in save.subgroupPressure)
                state.subgroupPressure.Add(new SubgroupSchismPressureState
                {
                    subgroupId = ledger.subgroupId,
                    consecutivePressureDays = ledger.consecutivePressureDays
                });
            return state;
        }
    }
}
