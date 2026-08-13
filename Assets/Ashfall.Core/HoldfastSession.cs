using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Core bootstrap-equivalent for Holdfast. Wires ice road, census, brine,
    /// waystation, and quests without GameBootstrap. Godot selftest drives this.
    /// </summary>
    public sealed class HoldfastSession
    {
        public IceRoadSystem IceRoad { get; }
        public CensusClaimSystem Census { get; }
        public BrineWaterSystem Brine { get; }
        public WaystationSystem Waystation { get; }
        public HoldfastQuestSystem Quests { get; }
        public HoldfastCatalog Catalog { get; }

        public HoldfastSession(
            IceRoadSystem ice,
            CensusClaimSystem census,
            BrineWaterSystem brine,
            WaystationSystem waystation,
            HoldfastQuestSystem quests,
            HoldfastCatalog catalog)
        {
            IceRoad = ice ?? new IceRoadSystem();
            Census = census ?? new CensusClaimSystem();
            Brine = brine ?? new BrineWaterSystem();
            Waystation = waystation ?? new WaystationSystem();
            Quests = quests ?? new HoldfastQuestSystem();
            Catalog = catalog ?? new HoldfastCatalog();
            Wire();
        }

        public static HoldfastSession Load(string dataDirectory, int seedSalt, bool expansionUnlocked, ILog log = null)
        {
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), log);
            var catalog = loader.Load(dataDirectory, expansionUnlocked);
            var quests = new HoldfastQuestSystem();
            quests.BindCatalog(catalog.Quests);
            var session = new HoldfastSession(
                new IceRoadSystem(seedSalt),
                new CensusClaimSystem(),
                new BrineWaterSystem(),
                new WaystationSystem(),
                quests,
                catalog);
            if (expansionUnlocked)
                session.IceRoad.Unlock(1);
            return session;
        }

        /// <summary>B1 — arrival at a quest's target location advances that quest.</summary>
        public bool NotifyArrival(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return false;
            bool any = false;
            for (int i = 0; i < HoldfastQuestSystem.MainQuestIds.Length; i++)
            {
                string id = HoldfastQuestSystem.MainQuestIds[i];
                if (!Quests.IsStarted(id) || Quests.IsCompleted(id)) continue;
                var def = Quests.GetDef(id);
                if (def == null || string.IsNullOrEmpty(def.target_location_id)) continue;
                if (def.target_location_id != locationId) continue;
                if (Quests.Advance(id)) any = true;
            }
            return any;
        }

        /// <summary>B1 — event/choice advances via branch + Advance().</summary>
        public bool ApplyChoice(string questId, string choiceId)
        {
            if (!Quests.IsStarted(questId))
                Quests.TryStart(questId, 1);
            return Quests.ChooseBranch(questId, choiceId);
        }

        public bool HonourLevy() => Census.HonourLevy();

        public bool RefuseLevy(int day)
        {
            bool ok = Census.RefuseLevy(day);
            IceRoad.BeginLampsOut(permanentWithdraw: false);
            if (Quests.IsStarted(HoldfastQuestSystem.Levy))
                Quests.ChooseBranch(HoldfastQuestSystem.Levy, CensusClaimSystem.FlagLevyRefuse);
            return ok;
        }

        /// <summary>B2 — membrane resolution activates Order 12-C and advances the membrane quest.</summary>
        public void ResolveMembrane(bool stripSector4, int day)
        {
            if (stripSector4) Brine.ResolveMembraneStripSector4();
            else Brine.ResolveMembraneLetDrop();
            Census.Activate12C();
            Quests.TryStart(HoldfastQuestSystem.Membrane, day);
            if (Quests.IsStarted(HoldfastQuestSystem.Membrane) && !Quests.IsCompleted(HoldfastQuestSystem.Membrane))
                Quests.Advance(HoldfastQuestSystem.Membrane);
        }

        public void UnlockDistrict(int day)
        {
            IceRoad.Unlock(day);
        }

        public void TickDaily(
            int day,
            WeatherKind weather,
            float outdoorC,
            bool hasMapItem,
            bool hasFormulaLore,
            bool hasLettersLore)
        {
            IceRoad.TickDaily(day, weather, outdoorC);
            Census.TickDaily(day);
            Brine.TickDaily(day, weather, outdoorC, outfallShifted: false);
            Waystation.TickDaily(IceRoad.IsOpen);
            Quests.TickDaily(day, hasMapItem, hasFormulaLore, hasLettersLore);
        }

        public string BriefingText(string questId) => Quests.GetBriefing(questId);

        public string StageText(string questId) => Quests.GetStageText(questId);

        private void Wire()
        {
            Quests.OnQuestStarted += id =>
            {
                if (id == HoldfastQuestSystem.Sheet)
                    IceRoad.Unlock(1);
                if (id == HoldfastQuestSystem.Clerk)
                    IceRoad.NotifyClerkStarted();
                if (id == HoldfastQuestSystem.Window)
                    Waystation.Unlock();
            };
            Quests.OnQuestCompleted += id =>
            {
                if (id == HoldfastQuestSystem.Window)
                    Waystation.Unlock();
                if (id == HoldfastQuestSystem.Plant)
                    Brine.UnlockSaltTrade();
            };
            Brine.OnSteamTrip += () =>
            {
                Quests.TryStart(HoldfastQuestSystem.Membrane, 1);
            };
        }
    }
}
