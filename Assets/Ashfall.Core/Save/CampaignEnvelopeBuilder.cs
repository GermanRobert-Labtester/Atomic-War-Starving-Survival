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
        /// key. Missing keys represent a subsystem that was never created and
        /// are therefore omitted; an explicitly captured empty payload is
        /// omitted for an optional registry section and is rejected for a
        /// required section when <paramref name="rejectEmptyPayloads"/> is
        /// true.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="payloads"/> contains an unknown key, or
        /// when strict capture mode receives an empty required payload. The
        /// caller must abort the save and keep the previous envelope intact.
        /// </exception>
        public static AggregateSaveEnvelope Build(
            IReadOnlyDictionary<string, string> payloads,
            SaveManifest manifest)
        {
            return Build(payloads, manifest, rejectEmptyPayloads: false);
        }

        /// <summary>
        /// Build a V2 envelope with an explicit capture-failure policy. Hosts
        /// use strict mode so a failed required capture can never be silently
        /// turned into an absent section; the two-argument overload remains
        /// compatible with legacy callers that only use this pure packer.
        /// </summary>
        public static AggregateSaveEnvelope Build(
            IReadOnlyDictionary<string, string> payloads,
            SaveManifest manifest,
            bool rejectEmptyPayloads)
        {
            if (payloads == null) throw new ArgumentNullException(nameof(payloads));
            if (manifest == null) throw new ArgumentNullException(nameof(manifest));

            var unknown = new List<string>();
            foreach (var key in payloads.Keys)
            {
                if (string.IsNullOrWhiteSpace(key) || !SaveSectionRegistry.SectionFileNames.ContainsKey(key))
                    unknown.Add(key ?? "(null)");
            }
            if (unknown.Count > 0)
                throw new ArgumentException(
                    "Unknown section key(s) not in SaveSectionRegistry: " + string.Join(", ", unknown) +
                    ". The campaign envelope is registry-whitelisted.");

            if (string.IsNullOrEmpty(manifest.generationId))
                manifest.generationId = $"gen_{manifest.slotId.Value ?? string.Empty}_{manifest.lastSaveTick}";

            var sections = new List<SaveSectionEnvelope>();
            foreach (var meta in SaveSectionRegistry.All)
            {
                if (!payloads.TryGetValue(meta.SectionKey, out var payload))
                    continue;

                if (string.IsNullOrWhiteSpace(payload))
                {
                    if (rejectEmptyPayloads && meta.RequiresSetup)
                    {
                        throw new ArgumentException(
                            $"Section '{meta.SectionKey}' captured an empty payload. " +
                            "The aggregate save was aborted rather than omitting a required section.",
                            nameof(payloads));
                    }
                    continue;
                }

                var section = new SaveSectionEnvelope
                {
                    sectionName = meta.SectionKey,
                    schemaVersion = SaveSectionRegistry.SchemaVersionFor(meta.SectionKey),
                    generationId = manifest.generationId,
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
