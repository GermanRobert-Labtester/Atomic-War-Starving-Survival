// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store + Host Session : LetterDelivery
// Core State : Ashfall.Core.Narrative.LetterDeliverySystemState
// Host Caller: Main.Letters
// Purpose    : Plan 212 letter route — discovered letters as a separate Core
//              authority from capsules. One explicit delivery decision per
//              letter, recipient-matched, privacy-safe (no undelivered body
//              text through the wide read model), canonical Needs morale via
//              the Main seam. No new catalog: content comes from the existing
//              survivor letter catalog by stable ID.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    public static class LetterDeliverySaveStore
    {
        public const string FileName = "letter_delivery_save.json";
        public const string SectionName = "letter_delivery";

        private static readonly SaveStore<LetterDeliverySystemState> s_store =
            SaveStoreHub.Checksummed<LetterDeliverySystemState>(FileName, nameof(LetterDeliverySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static bool TrySave(LetterDeliverySystemState state) => s_store.TrySave(state);
        public static LetterDeliverySystemState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(LetterDeliverySystemState state) => s_store.CaptureBare(state);
    }

    /// <summary>Player-facing authoring row: one letter's discoverable body.</summary>
    public sealed class LetterContent
    {
        public string LetterId { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Tone { get; set; } = string.Empty;
    }

    /// <summary>Authored letter catalog (strict; schema_version 1).</summary>
    public sealed class LetterCatalog
    {
        public Dictionary<string, LetterContent> ById { get; } =
            new(StringComparer.OrdinalIgnoreCase);

        public int Count => ById.Count;

        public static LetterCatalog Load(string json)
        {
            var catalog = new LetterCatalog();
            if (string.IsNullOrWhiteSpace(json)) return catalog;
            var dto = System.Text.Json.JsonSerializer.Deserialize<LetterCatalogDto>(json,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (dto?.letters == null) return catalog;
            foreach (var l in dto.letters)
            {
                if (l == null || string.IsNullOrWhiteSpace(l.letter_id)) continue;
                catalog.ById[l.letter_id] = new LetterContent
                {
                    LetterId = l.letter_id,
                    Sender = l.sender ?? string.Empty,
                    Recipient = l.recipient ?? string.Empty,
                    Location = l.location ?? string.Empty,
                    Content = l.content ?? string.Empty,
                    Tone = l.tone ?? string.Empty,
                };
            }
            return catalog;
        }

        private sealed class LetterCatalogDto
        {
            public int schema_version { get; set; }
            public List<LetterDto>? letters { get; set; }
        }

        private sealed class LetterDto
        {
            public string letter_id { get; set; } = string.Empty;
            public string sender { get; set; } = string.Empty;
            public string recipient { get; set; } = string.Empty;
            public string location { get; set; } = string.Empty;
            public string content { get; set; } = string.Empty;
            public string tone { get; set; } = string.Empty;
        }
    }

    /// <summary>
    /// Plan 212 letter route host session over <see cref="LetterDeliverySystem"/>.
    /// </summary>
    public sealed class LetterDeliveryHostSession : HostSessionBase
    {
        private readonly LetterDeliverySystem _system;
        private readonly LetterCatalog _catalog;

        public LetterDeliverySystem System => _system;
        public LetterCatalog Catalog => _catalog;
        public string LastEvent { get; private set; } = string.Empty;

        public LetterDeliveryHostSession(LetterCatalog catalog)
        {
            _system = new LetterDeliverySystem();
            _catalog = catalog ?? new LetterCatalog();
        }

        public static LetterDeliveryHostSession Create(string dataDir)
        {
            var session = new LetterDeliveryHostSession(new LetterCatalog());
            string dir = string.IsNullOrWhiteSpace(dataDir) ? CatalogPath.ResolveDataDir() : dataDir;
            string narrative = Path.Combine(dir, "narrative", "letters_expansion.json");
            if (File.Exists(narrative))
            {
                try { session.LoadCatalogFile(narrative); }
                catch (Exception ex) { session.LastEvent = $"Letter catalog load warning: {ex.Message}"; }
            }
            else
            {
                session.LastEvent = "Authored letter catalog missing; letter route unarmed.";
            }
            return session;
        }

        private void LoadCatalogFile(string path)
        {
            var loaded = LetterCatalog.Load(File.ReadAllText(path));
            foreach (var kv in loaded.ById) _catalog.ById[kv.Key] = kv.Value;
            LastEvent = $"Authored letter catalog loaded ({loaded.Count} letters).";
        }

        /// <summary>
        /// Explicit discovery: a committed find (expedition/claim event) marks
        /// one authored letter found. Refusal when the id is not authored —
        /// no shadow table.
        /// </summary>
        public LetterDeliveryRecord? Discover(string letterId, int day)
        {
            if (!_catalog.ById.ContainsKey(letterId))
            {
                LastEvent = $"Letter {letterId} refused (not in the authored catalog).";
                return null;
            }
            var rec = _system.DiscoverLetter(letterId, day);
            if (rec != null)
            {
                LastEvent = $"Letter {letterId} discovered (day {day}).";
                RaiseStateChanged();
            }
            else
            {
                LastEvent = $"Letter {letterId} already tracked — discovery exactly-once.";
            }
            return rec;
        }

        public bool Address(string letterId, string recipientSurvivorId, int day)
        {
            bool ok = _system.AddressLetter(letterId, recipientSurvivorId, day);
            if (ok)
            {
                LastEvent = $"Letter {letterId} addressed to {recipientSurvivorId}.";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool Deliver(string letterId, int day, string notes = "")
        {
            bool ok = _system.DeliverLetter(letterId, day, notes);
            if (ok)
            {
                LastEvent = $"Letter {letterId} delivered (day {day}).";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool Withhold(string letterId, int day, string notes = "")
        {
            bool ok = _system.WithholdLetter(letterId, day, notes);
            if (ok)
            {
                LastEvent = $"Letter {letterId} withheld (privacy-respecting refusal to deliver).";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool MarkUnanswered(string letterId, int day, string notes = "")
        {
            bool ok = _system.MarkUnanswered(letterId, day, notes);
            if (ok)
            {
                LastEvent = $"Letter {letterId} marked unanswered (day {day}).";
                RaiseStateChanged();
            }
            return ok;
        }

        /// <summary>
        /// Privacy-safe read: undelivered letters project sender/state only —
        /// never the authored content until delivery.
        /// </summary>
        public IReadOnlyList<(LetterDeliveryRecord Record, string PublicBody)> VisibleLetters()
        {
            var list = new List<(LetterDeliveryRecord, string)>();
            foreach (var rec in _system.State.records)
            {
                string body = rec.state == LetterDeliveryState.Delivered
                    ? (_catalog.ById.TryGetValue(rec.letterId, out var c) ? c.Content : string.Empty)
                    : "[sealed until delivered]";
                list.Add((rec, body));
            }
            return list;
        }

        public IReadOnlyList<string> AuthoredLetterIds() => _catalog.ById.Keys.ToList();
        public LetterDeliverySystemState CaptureState() => _system.CaptureState();

        public void RestoreState(LetterDeliverySystemState? state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored letter-delivery state.";
            RaiseStateChanged();
        }

        public bool TrySave() => LetterDeliverySaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = LetterDeliverySaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
