using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — 10 new
    /// survivor-backstory echoes (lore fragments). Mirrors the brief's
    /// echo table exactly: each row is a Title + Text pair. The catalog
    /// materialises them into <see cref="DiaryFragmentSO"/> instances
    /// for the existing <c>JournalSystem</c> (<c>AtomicWar._Game.Events</c>)
    /// to consume. Lives in the Data assembly alongside DiaryFragmentSO
    /// itself — Data already depends on Events, so putting this file in
    /// Events (as originally authored) would need the reverse reference
    /// and create a circular assembly dependency. Display is handled by
    /// the host's existing journal UI.
    /// </summary>
    public static class AshGetsDeeperEchoesCatalog
    {
        public class Spec
        {
            public string Id;
            public string Title;
            [TextArea(4, 12)] public string Text;
        }

        public static List<Spec> BuildAll()
        {
            return new List<Spec>
            {
                new Spec {
                    Id = "echo_shopping_list",
                    Title = "The Shopping List",
                    Text = "On the back of a receipt, in blue ink: milk, eggs, bread, birthday candles (8), the good chocolate. The receipt is from a supermarket that is now a crater. The birthday was Day 2. The child was in the house when the window went."
                },
                new Spec {
                    Id = "echo_bus_timetable",
                    Title = "The Last Bus",
                    Text = "A laminated timetable nailed to a shelter pole. Route 7, Tessarat to Ridgeline. Last departure: 18:45. The service ran until Day 0. The driver's name is printed at the bottom: K. ALDRIC. Someone has written underneath, in marker: \"He waited. He waited for everyone.\""
                },
                new Spec {
                    Id = "echo_voicemail_transcript",
                    Title = "Voicemail (Transcribed)",
                    Text = "Written out by hand on a napkin, because the phone battery died and someone needed to remember: \"Hi, it's Dad. I know you're at university. I know the trains are stopped. I need you to stay where you are. Do not come home. The roads are bad. I love you. Call me when you can. I'll keep the phone on.\" The napkin is folded eight times. It has been carried in a pocket for weeks."
                },
                new Spec {
                    Id = "echo_garden_marker",
                    Title = "The Garden Marker",
                    Text = "A wooden stake in the ground, hand-carved: \"TOMATOES — Suki's. DO NOT PICK. 14 days.\" The tomatoes are dead. The marker is still standing. Someone waters the dirt every morning."
                },
                new Spec {
                    Id = "echo_military_gravestone",
                    Title = "The Marker",
                    Text = "A helmet on a mound of earth. Dog tags hanging from the chin strap. The name is scratched out. Not by weather. By hand. Someone didn't want the name known. Or didn't want the name remembered. The mound is fresh."
                },
                new Spec {
                    Id = "echo_bunker_graffiti",
                    Title = "Wall Writing",
                    Text = "Scratched into the sub-pen wall, near the old 1962 lockdown switch: \"WE WERE HERE. WE WERE SCARED. WE STAYED.\" No names. No date. The scratches are old. The fear is not."
                },
                new Spec {
                    Id = "echo_pharmacy_board",
                    Title = "The Pharmacy Board",
                    Text = "A whiteboard behind the PharmaPlus counter: \"FLU SHOTS — WALK-IN. TUESDAY.\" Below it, in different handwriting, added later: \"IODINE — RATIONED. 2 PER FAMILY.\" Below that, in a third hand, barely legible: \"GONE. DON'T WAIT.\""
                },
                new Spec {
                    Id = "echo_child_toy",
                    Title = "The Wooden Train",
                    Text = "Under a collapsed shelf, a hand-painted wooden train. Red engine, blue cab. One wheel is missing. The paint is chipped but the name on the bottom is clear: \"For TOMAS, Age 4. From Grandpa.\" Tomas is not here. Grandpa is not here. The train is."
                },
                new Spec {
                    Id = "echo_ration_card",
                    Title = "The Ration Card",
                    Text = "A stamped cardboard card: \"CIVILIAN RATION — TESSARAT DISTRICT. HOLDER: M. VASQUEZ. ALLOCATION: 1 MEAL / DAY.\" The card is punched for eleven days. The twelfth punch is missing. M. Vasquez didn't make it to day twelve. Or she did, and stopped caring about meals."
                },
                new Spec {
                    Id = "echo_last_newspaper",
                    Title = "The Last Edition",
                    Text = "A newspaper, hand-printed, single page. The headline reads: \"SHELTER LOCATIONS CONFIRMED — PROCEED TO NEAREST MARKED SITE.\" The date is Day -1. The bottom of the page has a small notice: \"The Tessarat Market will be closed this week due to road maintenance.\" Someone circled it and wrote: \"Ha.\""
                },
            };
        }

        public static DiaryFragmentSO Materialise(Spec spec)
        {
            if (spec == null) return null;
            var so = ScriptableObject.CreateInstance<DiaryFragmentSO>();
            so.id = spec.Id;
            so.title = spec.Title;
            so.text = spec.Text ?? string.Empty;
            return so;
        }

        public static List<DiaryFragmentSO> MaterialiseAll()
        {
            var specs = BuildAll();
            var outList = new List<DiaryFragmentSO>(specs.Count);
            for (int i = 0; i < specs.Count; i++) outList.Add(Materialise(specs[i]));
            return outList;
        }
    }
}
