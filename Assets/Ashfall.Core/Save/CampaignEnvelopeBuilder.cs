using System;
using System.Collections.Generic;

namespace Ashfall.Core.Save
{
    /// <summary>
    /// Builds the canonical versioned campaign envelope
    /// (<see cref="AggregateSaveEnvelope"/>, manifestVersion
    /// <see cref="CurrentEnvelopeVersion"/>) from in-memory section payloads.
    ///
    /// The payload map is keyed by <see cref="SaveSectionRegistry"/> section
    /// keys; sections are emitted in registry order with their real schema
    /// versions and per-section checksums, plus the aggregate checksum.
    /// Unknown keys are rejected — the envelope is registry-whitelisted, so a
    /// stray file can never become a section (the V1 packer's file-scan had no
    /// such guarantee).
    ///
    /// Pure Core: no IO. The host captures payloads (SaveStore&lt;T&gt;.
    /// CapturePersisted), hands them here, and writes the result through
    /// SaveSlotService.WriteAggregateAtomically.
    /// </summary>
    public static class CampaignEnvelopeBuilder
    {
        /// <summary>Current envelope format version (registry-keyed sections).</summary>
        public const int CurrentEnvelopeVersion = 2;

        /// <summary>
        /// Build a V2 envelope. Payloads must be keyed by registry section
        /// key; empty/null payloads are skipped (the section is legitimately
        /// absent, e.g. its subsystem was never created this campaign).
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="payloads"/> contains a key that is not
        /// in <see cref="SaveSectionRegistry"/> — callers treat this as an
        /// abort-the-save condition, keeping the previous envelope intact.
        /// </exception>
        public static AggregateSaveEnvelope Build(
            IReadOnlyDictionary<string, string> payloads,
            SaveManifest manifest)
        {
            if (manifest == null) throw new ArgumentNullException(nameof(manifest));

            var unknown = new List<string>();
            foreach (var key in payloads.Keys)
            {
                if (!SaveSectionRegistry.SectionFileNames.ContainsKey(key))
                    unknown.Add(key);
            }
            if (unknown.Count > 0)
                throw new ArgumentException(
                    "Unknown section key(s) not in SaveSectionRegistry: " + string.Join(", ", unknown) +
                    ". The campaign envelope is registry-whitelisted.");

            var sections = new List<SaveSectionEnvelope>();
            foreach (var meta in SaveSectionRegistry.All)
            {
                if (!payloads.TryGetValue(meta.SectionKey, out var payload)) continue;
                if (string.IsNullOrWhiteSpace(payload)) continue;

                var section = new SaveSectionEnvelope
                {
                    sectionName = meta.SectionKey,
                    schemaVersion = SaveSectionRegistry.SchemaVersionFor(meta.SectionKey),
                    payloadJson = payload,
                };
                section.checksum = SaveSlotService.ComputeSectionChecksum(section);
                sections.Add(section);
            }

            var envelope = new AggregateSaveEnvelope
            {
                manifestVersion = CurrentEnvelopeVersion,
                manifest = manifest,
                sections = sections,
            };
            envelope.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(envelope);
            return envelope;
        }
    }
}
