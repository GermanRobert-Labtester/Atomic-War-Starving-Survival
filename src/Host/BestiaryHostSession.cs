// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Bestiary;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class BestiarySaveStore
    {
        public const string SectionName = "bestiary_knowledge";
        public const string FileName = "bestiary_knowledge_save.json";

        private static readonly SaveStore<BestiaryState> s_store =
            SaveStoreHub.Checksummed<BestiaryState>(FileName, nameof(BestiarySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string? TryCapturePersisted(BestiaryState state) => s_store.CaptureBare(state);
        public static BestiaryState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(BestiaryState state) => s_store.TrySave(state);
        public static BestiaryState? TryLoad() => s_store.TryLoad();
    }

    public sealed class BestiaryHostSession : HostSessionBase
    {
        public BestiarySystem System { get; }

        public BestiaryHostSession(BestiarySystem? system = null)
        {
            System = system ?? new BestiarySystem();
            System.OnCreatureDiscovered += _ => RaiseStateChanged();
            System.OnCreatureEncountered += (_, _) => RaiseStateChanged();
            System.OnCreatureKilled += (_, _) => RaiseStateChanged();
            System.OnNoteUnlocked += (_, _) => RaiseStateChanged();
        }

        public static BestiaryHostSession Create(string dataDir, BestiaryState? restoredState = null)
        {
            var system = new BestiarySystem(restoredState ?? new BestiaryState());
            string catalogPath = Path.Combine(dataDir, "narrative", "wasteland_wildlife_bestiary.json");
            if (File.Exists(catalogPath))
            {
                system.LoadCatalog(File.ReadAllText(catalogPath));
            }
            return new BestiaryHostSession(system);
        }

        public CreatureDiscoveryRecord RecordEncounter(string creatureId, int day, string locationId = "", string witnessId = "")
        {
            var record = System.RecordEncounter(creatureId, day, locationId, witnessId);
            RaiseStateChanged();
            return record;
        }

        public void RecordKill(string creatureId, int day, string locationId = "")
        {
            System.RecordKill(creatureId, day, locationId);
            RaiseStateChanged();
        }

        public void RecordButcher(string creatureId, int day)
        {
            System.RecordButcher(creatureId, day);
            RaiseStateChanged();
        }

        public BestiaryCensus GetCensus() => System.GetCensus();

        public override void Save()
        {
            if (!IsDirty) return;
            BestiarySaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
