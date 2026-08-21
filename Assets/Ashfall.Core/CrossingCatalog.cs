using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    public class CrossingFactionEntry
    {
        public string id;
        public string display_name;
        public string alignment;
        public string home_region;
        public bool is_active;
        public float trust;
        public string[] wants;
        public string[] offers;
        public string signature_quote;
        public string access_rule;
        public string badge_asset_id;
    }

    public class CrossingLocationEntry
    {
        public string id;
        public string displayName;
        public string inspect;
        public string description;
        public float dangerLevel;
        public float travelHours;
        public float baseRadsPerHour;
        public string region;
        public bool overlay_on_unlock;
        public bool recast_always;
    }

    public class CrossingQuestStageEntry
    {
        public string id;
        public string text;
    }

    public class CrossingQuestChoiceEntry
    {
        public string id;
        public string text;
        public string set_flag;
    }

    public class CrossingQuestEntry
    {
        public string id;
        public string display_name;
        public string type;
        public string briefing;
        public string prereq_quest_id;
        public int min_day;
        public CrossingQuestStageEntry[] stages;
        public CrossingQuestChoiceEntry[] choices;
        public string knowledge_key;
        public string target_location_id;

        public int StageCount => stages != null ? stages.Length : 0;
    }

    public class CrossingItemEntry
    {
        public string id;
        public string displayName;
        public string description;
        public string type;
        public int stackMax;
        public float weight;
        public float tradeValue;
        public float thirstRestore;
        public float hungerRestore;
        public float moraleEffect;
    }

    public class CrossingChoiceEntry
    {
        public string text;
        public string[] cost_items;
        public string result;
    }

    public class CrossingEncounterEntry
    {
        public string id;
        public string name;
        public string target_location;
        public string description;
        public string threat_level;
        public CrossingChoiceEntry[] choices;
    }

    public class CrossingCrisisEntry
    {
        public string id;
        public string name;
        public string[] phases;
        public string description;
        public string resolution;
    }

    public class CrossingEncountersContainer
    {
        public CrossingEncounterEntry[] encounters;
        public CrossingCrisisEntry[] crises;
    }

    public sealed class CrossingCatalog
    {
        public List<CrossingFactionEntry> Factions { get; } = new List<CrossingFactionEntry>();
        public List<CrossingLocationEntry> Locations { get; } = new List<CrossingLocationEntry>();
        public List<CrossingQuestEntry> Quests { get; } = new List<CrossingQuestEntry>();
        public List<CrossingItemEntry> Items { get; } = new List<CrossingItemEntry>();
        public List<CrossingEncounterEntry> Encounters { get; } = new List<CrossingEncounterEntry>();
        public List<CrossingCrisisEntry> Crises { get; } = new List<CrossingCrisisEntry>();

        public CrossingFactionEntry? GetFaction(string id) => Find(Factions, id, f => f.id);
        public CrossingLocationEntry? GetLocation(string id) => Find(Locations, id, e => e.id);
        public CrossingQuestEntry? GetQuest(string id) => Find(Quests, id, q => q.id);
        public CrossingItemEntry? GetItem(string id) => Find(Items, id, item => item.id);
        public CrossingEncounterEntry? GetEncounter(string id) => Find(Encounters, id, enc => enc.id);
        public CrossingCrisisEntry? GetCrisis(string id) => Find(Crises, id, c => c.id);

        private static T? Find<T>(List<T> list, string id, Func<T, string> getId) where T : class
        {
            if (string.IsNullOrEmpty(id) || list == null) return null;
            for (int i = 0; i < list.Count; i++)
                if (list[i] != null && getId(list[i]) == id)
                    return list[i];
            return null;
        }
    }

    /// <summary>
    /// Loads crossing_*.json from StreamingAssets/Data.
    /// Scale fix (2026-08-14): live catalogs use danger ~1–10 and rads ~18–52.
    /// Crossing cards were authored on a 0–1 / sub-1 scale; they are rescaled
    /// to danger ~3–6 and rads ~8–25 so the depot is not the safest site in the game.
    /// loc_crossing_weighbridge stays a distinct id; display is "The Deck Scale"
    /// so it is not a third Weighbridge thesis beside loc_weighbridge / loc_cut_weigh_hut.
    /// </summary>
    public sealed class CrossingCatalogLoader
    {
        public const string FactionsFile = "crossing_factions.json";
        public const string LocationsFile = "crossing_locations.json";
        public const string QuestsFile = "crossing_quests.json";
        public const string ItemsFile = "crossing_items.json";
        public const string EncountersFile = "crossing_encounters.json";

        /// <summary>Live schema: danger 3–6 after the unit fix.</summary>
        public const float MinDanger = 3f;
        public const float MaxDanger = 6f;
        /// <summary>Live schema: rads 8–25 after the unit fix.</summary>
        public const float MinRads = 8f;
        public const float MaxRads = 25f;

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;

        public CrossingCatalogLoader(IFileIO files, IJsonSerializer json, ILog log = null)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            _log = log ?? NullLog.Instance;
        }

        public CrossingCatalog Load(string dataDirectory)
        {
            var catalog = new CrossingCatalog();
            if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
            {
                _log.Warn("Crossing catalog directory missing: " + dataDirectory);
                return catalog;
            }

            LoadList(_files.Combine(dataDirectory, FactionsFile), catalog.Factions, "factions");
            LoadList(_files.Combine(dataDirectory, LocationsFile), catalog.Locations, "locations");
            LoadList(_files.Combine(dataDirectory, QuestsFile), catalog.Quests, "quests");
            LoadList(_files.Combine(dataDirectory, ItemsFile), catalog.Items, "items");
            LoadEncountersAndCrises(_files.Combine(dataDirectory, EncountersFile), catalog);
            return catalog;
        }

        private void LoadEncountersAndCrises(string path, CrossingCatalog catalog)
        {
            if (!_files.FileExists(path))
            {
                _log.Warn("Crossing encounters file missing: " + path);
                return;
            }

            try
            {
                string json = _files.ReadAllText(path);
                var container = _json.Deserialize<CrossingEncountersContainer>(json);
                if (container?.encounters != null)
                {
                    for (int i = 0; i < container.encounters.Length; i++)
                    {
                        if (container.encounters[i] != null)
                            catalog.Encounters.Add(container.encounters[i]);
                    }
                }
                if (container?.crises != null)
                {
                    for (int i = 0; i < container.crises.Length; i++)
                    {
                        if (container.crises[i] != null)
                            catalog.Crises.Add(container.crises[i]);
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error("Crossing encounters parse failed: " + e.Message);
            }
        }

        private void LoadList<T>(string path, List<T> dest, string label) where T : class
        {
            if (!_files.FileExists(path))
            {
                _log.Warn("Crossing " + label + " file missing: " + path);
                return;
            }

            try
            {
                string json = _files.ReadAllText(path);
                var items = _json.Deserialize<List<T>>(json);
                if (items == null) return;
                for (int i = 0; i < items.Count; i++)
                    if (items[i] != null)
                        dest.Add(items[i]);
            }
            catch (Exception e)
            {
                _log.Error("Crossing " + label + " parse failed: " + e.Message);
            }
        }
    }

    public static class CrossingIds
    {
        public const string Expansion = "expansion_nobodys_charter";
        public const string Region = "region_crossing";
        public const string TheVouch = "quest_crossing_the_vouch";
        public const string FirstWeigh = "quest_crossing_first_weigh";
        public const string ScaleIntegrity = "quest_crossing_scale_integrity";
        public const string TheStanding = "quest_crossing_the_standing";
        public const string TheTerms = "quest_crossing_the_terms";
        public const string ViaductGate = "loc_crossing_viaduct_gate";
        public const string Scalehouse = "loc_crossing_scalehouse";
        public const string Weighbridge = "loc_crossing_weighbridge";
        public const string NpcMattis = "npc_mattis_cray";
        public const string NpcOsran = "npc_osran_kell";
        public const string NpcWyn = "npc_wyn_sabler";
        public const string NpcIvo = "npc_ivo_fenn";
        public const string FactionScale = "faction_the_scale";
        public const string FactionUnderwrite = "faction_the_underwrite";
        public const string FactionCompact = "faction_the_compact";

        /// <summary>Canonical quest ids (bible §4.1 main questline + §4.2 side).</summary>
        public static class Quests
        {
            public const string TheVouch   = "quest_crossing_the_vouch";
            public const string FirstWeigh = "quest_crossing_first_weigh";
            public const string TheTerms   = "quest_crossing_the_terms";
            public const string ThePetition= "quest_crossing_the_petition";
            public const string TheStanding= "quest_crossing_the_standing";
            public const string TheMarker  = "quest_crossing_the_marker";
            public const string TheForfeit = "quest_crossing_the_forfeit";
            public const string TheVoteNot = "quest_crossing_the_vote_that_isnt";
            public const string ScaleIntegrity = "quest_crossing_scale_integrity";
            public const string CharterCore = "quest_crossing_three_dry_pages";
            public const string WhoHoldsLedger = "quest_crossing_who_holds_the_ledger";
            public const string CompanionMattis = "quest_crossing_companion_mattis";
        }

        /// <summary>Canonical location ids (bible §2.4).</summary>
        public static class Locations
        {
            public const string ViaductGate  = "loc_crossing_viaduct_gate";
            public const string Scalehouse   = "loc_crossing_scalehouse";
            public const string Stallrow     = "loc_crossing_stallrow";
            public const string Watchtower   = "loc_crossing_watchtower";
            public const string Weighbridge  = "loc_crossing_weighbridge";
            public const string Underwrite   = "loc_crossing_underwrite_hall";
            public const string RecordsRoom  = "loc_crossing_records_room";
            public const string Nightfire    = "loc_crossing_nightfire";
            public const string TheLockup    = "loc_crossing_the_lockup";
            public const string GranaryPledge = "loc_crossing_granary_pledge";
            public const string PetitionTent = "loc_crossing_petition_tent";
            public const string FoundersMarker = "loc_crossing_founders_marker";
            public const string TheAnnex     = "loc_crossing_the_annex";
        }

        /// <summary>Canonical item ids (bible §7).</summary>
        public static class Items
        {
            public const string VouchToken     = "item_vouch_token_crossing";
            public const string CalibrationWeight = "item_calibration_weight";
            public const string TradedGrain    = "item_crossing_traded_grain";
            public const string TradedSalt     = "item_crossing_traded_salt";
            public const string PledgeSlip     = "item_crossing_pledge_slip";
            public const string CharterPages   = "item_charter_three_pages";
            public const string DebtContractCopy = "item_debt_contract_copy";
            public const string MarkerRubbing  = "item_marker_rubbing";
            public const string DutyLogFragment = "item_duty_log_fragment";
            public const string TradeManifestBlank = "item_trade_manifest_blank";
            public const string WynReceiptPaid = "item_wyn_receipt_paid";
        }

        /// <summary>World / story flag keys (bible §3 branching table).</summary>
        public static class Flags
        {
            public const string VouchedClean   = "flag_crossing_vouched_clean";
            public const string VouchBurned    = "flag_crossing_vouch_burned";
            public const string AccessSoftened = "flag_crossing_access_softened";
            public const string UnderwriteUntested = "flag_crossing_underwrite_untested";
            public const string PetitionUnsigned = "flag_crossing_petition_unsigned";
            public const string StandingHonest = "flag_crossing_standing_honest";
            public const string StandingRigged = "flag_crossing_standing_rigged";
            /// <summary>Set when the Ostrowski rumour starts — never at boot.</summary>
            public const string ExpansionUnlocked = "exp_nobodys_charter_unlocked";
            /// <summary>The vouch quest reward (token + lore) has been granted once.</summary>
            public const string VouchRewarded = "flag_crossing_vouch_rewarded";
        }

        /// <summary>Named Crossing NPCs (ids must exist in characters.json).</summary>
        public static class Npcs
        {
            public const string OsranKell = "npc_osran_kell";
            public const string MattisCray = "npc_mattis_cray";
            public const string DessaVane = "npc_dessa_vane";
            public const string PerrinAshby = "npc_perrin_ashby";
            public const string IvoFenn = "npc_ivo_fenn";
            public const string WynSabler = "npc_wyn_sabler";
        }

        /// <summary>Knowledge keys granted by quest completion.</summary>
        public static class Knowledge
        {
            public const string TheVouch       = "lore_nc_the_vouch";
            public const string ReadAgain      = "lore_nc_read_again";
            public const string RubricAgain    = "lore_nc_the_rubric_again";
            public const string ThreeLegends   = "lore_nc_three_legends";
            public const string TheForfeit     = "lore_nc_the_forfeit";
            public const string TheStanding    = "lore_nc_the_standing";
        }

        /// <summary>World-state mutation ids (bible §6 endings / WorldStateConsequenceSystem).</summary>
        public static class Mutations
        {
            public const string CharterRevealed = "mutation_crossing_charter_revealed";
            public const string HonestTrader    = "mutation_crossing_honest_trader";
            public const string UnderwriteBurned = "mutation_crossing_underwrite_burned";
            public const string UnderwriteReliable = "mutation_crossing_underwrite_reliable";
            public const string PetitionRevised = "mutation_crossing_petition_revised";
            public const string PetitionLeaked = "mutation_crossing_petition_leaked";
            public const string StandingRigged  = "mutation_crossing_standing_rigged";
            public const string StandingHonest  = "mutation_crossing_standing_honest";
            public const string ForfeitHonoured = "mutation_crossing_forfeit_honoured";
            public const string VoteClean       = "mutation_crossing_vote_clean";
            public const string VoteSabotaged   = "mutation_crossing_vote_sabotaged";
        }

        /// <summary>Ending ids (bible §3 Endings).</summary>
        public static class Endings
        {
            public const string Scale       = "ending_crossing_scale";
            public const string Underwrite  = "ending_crossing_underwrite";
            public const string Compact     = "ending_crossing_compact";
            public const string None        = "ending_crossing_none";
            public const string Walked      = "ending_crossing_walked";
        }

        /// <summary>Canonical Crossing encounters (bible §6.2).</summary>
        public static class Encounters
        {
            public const string CollectorVisit = "enc_nc_collector_visit";
            public const string BackerPressure = "enc_nc_backer_pressure";
            public const string LockupMuscle = "enc_nc_lockup_muscle";
            public const string IronRaidersScout = "enc_nc_iron_raiders_scout";
            public const string DeserterPassage = "enc_nc_deserter_passage";
            public const string ScavengerDispute = "enc_nc_scavenger_dispute";
            public const string GrainExchangeEnvoy = "enc_nc_grain_exchange_envoy";
            public const string SunSeekersPass = "enc_nc_sun_seekers_pass";
            public const string ForfeitWitness = "enc_nc_forfeit_witness";
            public const string StandingAmbush = "enc_nc_standing_ambush";
        }

        /// <summary>Canonical multi-phase Crises (bible §6.3).</summary>
        public static class Crises
        {
            public const string TheForfeit = "crisis_the_forfeit";
            public const string TheVote = "crisis_the_vote";
            public const string TheStandingContested = "crisis_the_standing_contested";
            public const string TheCharterFound = "crisis_the_charter_found";
            public const string WhoHoldsTheLedger = "crisis_who_holds_the_ledger";
        }
    }
}
