using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Master id list for the ASHFALL: THE HOLDFAST endings (Sprint 4 "Shelf &amp; endings").
    /// All ids come from docs/expansions/expansion_the_holdfast_plan.md §5.4
    /// (five endings, mutually exclusive — exactly one may be armed at a time).
    /// Do not invent ids outside this list; the Godot host guards SetEnding with IsKnown.
    /// </summary>
    public static class HoldfastEndings
    {
        public const string None = "";

        public const string Schedule = "ending_holdfast_schedule";
        public const string Reserve = "ending_holdfast_reserve";
        public const string DarkRoad = "ending_holdfast_dark_road";
        public const string Tender = "ending_holdfast_tender";
        public const string White = "ending_holdfast_white";

        /// <summary>Master list, order-preserved. The host cycles through this.</summary>
        public static readonly string[] All = { Schedule, Reserve, DarkRoad, Tender, White };

        public static bool IsKnown(string endingId)
        {
            if (string.IsNullOrEmpty(endingId)) return false;
            for (int i = 0; i < All.Length; i++)
                if (All[i] == endingId) return true;
            return false;
        }

        public static string DisplayName(string endingId)
        {
            switch (endingId)
            {
                case Schedule: return "The Schedule Holds";
                case Reserve: return "The Reserve";
                case DarkRoad: return "The Road Goes Dark";
                case Tender: return "Stand-Up";
                case White: return "The White";
                default: return endingId;
            }
        }

        public static string FlavorText(string endingId)
        {
            return endingId switch
            {
                Schedule => "The rigid structure of duty becomes both shield and cage. The bunker endures, but at what cost to those who keep its gears turning? Some of yours live numbered in Block C. The bunker is easier to feed. The duty roster on the wall has names on it that are not the names that slept there.",
                Reserve => "The final stores are opened. The last ration is shared. In the quiet between heartbeats, something stirs in the dark corners of memory. The Office took what it was owed. The unlisted became the reserve. The ledger is balanced, but the debt remains.",
                DarkRoad => "The path chosen was not the one marked on any map. The stars themselves seem to recoil from what was done to reach them. The Cutters withdrew their light. The ice claimed its due. District 8 continues without you. Forty empty apartments stay empty. Edor's return is found in a weigh-hut, incomplete, in a good hand.",
                Tender => "The final performance is given. The last note lingers in the stale air. Somewhere, a door opens that should have stayed shut. The Fleet stops being a rumor. The Cluster has to vote on beds. Migration/Icebreaker epilogues land in a place.",
                White => "The white light consumes all color, all sound, all thought. When the darkness returns, nothing is the same. The whispers continue. The records are corrupted. The bunker's logs show no such event ever occurred. The logs tell a different story.",
                _ => "The ending you chose remains unwritten..."
            };
        }

        public static string[] LoreFragments(string endingId)
        {
            return endingId switch
            {
                Schedule => new string[]
                {
                    "The night watch never ends. It simply waits for you to blink.",
                    "Routine is the only thing that hasn't rotted in here.",
                    "They say the generators hum the same tune every night. No one dares ask what it means.",
                    "The Office keeps perfect records. They will find you eventually.",
                    "Your name is in a column you were never meant to see."
                },
                Reserve => new string[]
                {
                    "The last crate of preserved peaches was opened on Day 472. No one remembers who ate them.",
                    "The emergency rations taste like ash and regret.",
                    "They say the bunker's stores were never meant to last this long. The math doesn't add up.",
                    "The ledger is balanced. The debt is not.",
                    "You are now part of the reconstruction pool."
                },
                DarkRoad => new string[]
                {
                    "The airlock cycle counted down to zero. No one was on the other side.",
                    "The radio picked up a transmission at 03:17. It was just static shaped like words.",
                    "The footprints led to the edge of the irradiated zone and stopped. As if something picked them up.",
                    "The Cutters do not forgive.",
                    "The ice remembers what you did."
                },
                Tender => new string[]
                {
                    "The final show was attended by exactly three people. One of them wasn't breathing.",
                    "The stage lights flickered in time with the emergency beacons. Coincidence? Or something watching?",
                    "They say the last song played backward reveals a different melody. No one has dared to try.",
                    "The Fleet does not forget.",
                    "You are now part of the crew."
                },
                White => new string[]
                {
                    "The white light came from everywhere and nowhere. It didn't illuminate. It revealed.",
                    "Afterward, the survivors spoke in whispers of colors they couldn't name.",
                    "The bunker's records show no such event ever occurred. The logs tell a different story.",
                    "The Witness watches.",
                    "The White is patient."
                },
                _ => Array.Empty<string>()
            };
        }

        public static string[] WarningSigns(string endingId)
        {
            return endingId switch
            {
                Schedule => new string[]
                {
                    "You filed your census return on time.",
                    "You honored the levy order.",
                    "You accepted the numbered apartment in Block C.",
                    "You let the Office keep its records.",
                    "You became part of the system."
                },
                Reserve => new string[]
                {
                    "You signed the Order 12-C.",
                    "You let the Office take your survivors.",
                    "You accepted the brass nameplates.",
                    "You trusted the ledger.",
                    "You became the reserve."
                },
                DarkRoad => new string[]
                {
                    "You refused the levy.",
                    "You blasted the ice.",
                    "You walked the dark segment.",
                    "You let the Cutters withdraw.",
                    "You became a ghost on the road."
                },
                Tender => new string[]
                {
                    "You boarded the tender without blasting.",
                    "You provided clean water.",
                    "You shared your radio frequencies.",
                    "You worked on the tender.",
                    "You became part of the crew."
                },
                White => new string[]
                {
                    "You left offerings at the White.",
                    "You worked in silence.",
                    "You shared cryptic knowledge.",
                    "You honored the Witness.",
                    "You became part of the mystery."
                },
                _ => Array.Empty<string>()
            };
        }
    }
}
