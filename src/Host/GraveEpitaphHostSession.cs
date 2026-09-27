// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Grave Epitaph host binding.
//
// MemorialSystem already contains the selection rule (fall back to
// GraveEpitaphCatalog.SelectEpitaph(cause, rng) when a death has no bespoke
// epitaph) but the two seams it reads — EpitaphCatalog and EpitaphRng — were
// never assigned anywhere in the host, so the authored
// wasteland_grave_epitaphs.json was unreachable and inscriptions came out empty.
// ============================================================================

using System;
using Ashfall.Core;
using Ashfall.Core.Memorial;

namespace AtomicWar.GodotApp
{
    public sealed class GraveEpitaphHostSession : HostSessionBase
    {
        private readonly Func<MemorialSystem?> _memorialProvider;
        private readonly Func<ISeededRng?> _rngProvider;
        private GraveEpitaphCatalog? _catalog;

        public string LastEvent { get; private set; } = string.Empty;
        public GraveEpitaphCatalog? Catalog => _catalog;
        public int EpitaphCount => _catalog?.TotalCount ?? 0;

        public GraveEpitaphHostSession(
            Func<MemorialSystem?> memorialProvider,
            Func<ISeededRng?> rngProvider)
        {
            _memorialProvider = memorialProvider ?? throw new ArgumentNullException(nameof(memorialProvider));
            _rngProvider = rngProvider ?? throw new ArgumentNullException(nameof(rngProvider));
        }

        /// <summary>
        /// Loads the authored epitaph table and assigns it plus a campaign-forked
        /// RNG to the live memorial owner. Idempotent.
        /// </summary>
        public int Bind(string dataDir)
        {
            var memorial = _memorialProvider();
            if (memorial == null)
            {
                LastEvent = "Memorial owner unavailable; epitaphs not bound.";
                return 0;
            }

            _catalog ??= GraveEpitaphCatalog.LoadFromDataDir(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            memorial.EpitaphCatalog = _catalog;
            memorial.EpitaphRng = _rngProvider();

            LastEvent = _catalog.TotalCount > 0
                ? $"Bound {_catalog.TotalCount} authored grave epitaph(s) to the memorial owner."
                : "No authored grave epitaph rows found.";
            RaiseStateChanged();
            return _catalog.TotalCount;
        }

        /// <summary>Authored epitaph for a cause, or empty when the table is absent.</summary>
        public string EpitaphFor(string cause)
        {
            if (_catalog == null) return string.Empty;
            return _catalog.SelectEpitaph(cause, _rngProvider() ?? new SeededRng(1));
        }

        public bool IsBoundToLiveMemorial()
        {
            var memorial = _memorialProvider();
            return memorial != null && ReferenceEquals(memorial.EpitaphCatalog, _catalog);
        }

        public string StatusLine() =>
            _catalog == null ? "epitaphs unbound" : $"{_catalog.TotalCount} authored epitaph(s)";
    }
}
