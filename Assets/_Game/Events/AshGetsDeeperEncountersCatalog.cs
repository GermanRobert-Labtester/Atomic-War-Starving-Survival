using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — 15 new
    /// encounters authored as a data-driven catalog builder. Each Spec
    /// row maps to a <see cref="GameEvent"/> with id, title, bodyText,
    /// minDay, weight, and a free-text trigger-condition note. The
    /// trigger text is stashed in <c>bodyText</c>'s tail as a designer
    /// hint so the data is JSON-importable without losing the brief's
    /// "Trigger" column.
    /// </summary>
    public static class AshGetsDeeperEncountersCatalog
    {
        public static class Ids
        {
            // Shelter
            public const string PowerSurge         = "enc_power_surge";
            public const string OxygenThin         = "enc_oxygen_thin";
            public const string PipeBurst          = "enc_pipe_burst";
            public const string CondensationMold   = "enc_condensation_mold";
            public const string SilentDeath        = "enc_silent_death";
            public const string RatKing            = "enc_rat_king";
            // Surface
            public const string AshQuicksand       = "enc_ash_quicksand";
            public const string DeadRadioOperator  = "enc_dead_radio_operator";
            public const string FrozenFamily       = "enc_frozen_family";
            public const string MinefieldRemnant   = "enc_minefield_remnant";
            public const string BlackRainPocket    = "enc_black_rain_pocket";
            // Moral / social
            public const string WhoEats            = "enc_who_eats";
            public const string TheChildAsks       = "enc_the_child_asks";
            public const string ExileVote          = "enc_exile_vote";
            public const string MercyRequest       = "enc_mercy_request";
        }

        public class Spec
        {
            public string Id;
            public string Title;
            [TextArea(2, 4)] public string Description;
            public int MinDay;
            public float Weight;
            public string TriggerNote; // brief's "Trigger" column, free-form
        }

        public static List<Spec> BuildAll()
        {
            var list = new List<Spec>();
            AddShelter(list);
            AddSurface(list);
            AddMoral(list);
            return list;
        }

        // ── Shelter encounters ──────────────────────────────────────────
        private static void AddShelter(List<Spec> list)
        {
            list.Add(new Spec {
                Id = Ids.PowerSurge, Title = "The Surge",
                MinDay = 8, Weight = 1.5f,
                TriggerNote = "PowerGrid > 80%",
                Description = "The generator screams. A voltage spike blows two breakers and fries the radio's capacitor bank. The lights come back, but the radio is dead until someone finds replacement parts. The smell of burnt copper fills the bunker for two days."
            });
            list.Add(new Spec {
                Id = Ids.OxygenThin, Title = "Thin Air",
                MinDay = 15, Weight = 2.0f,
                TriggerNote = "AirFilter < 30%",
                Description = "Everyone starts yawning. Then the headaches come. Then the confusion. CO2 is building up. The scrubbers can't keep pace. Someone has to go to the surface and clear the intake. The surface is 40 mSv/h today."
            });
            list.Add(new Spec {
                Id = Ids.PipeBurst, Title = "The Pipe Screams",
                MinDay = 6, Weight = 1.8f,
                TriggerNote = "Random",
                Description = "A water main in the lower corridor ruptures. Cold, black water floods the storage room. Rations are soaking. The water is contaminated. Someone has to shut the valve. The valve is in the flooded section. The flooded section is knee-deep and getting deeper."
            });
            list.Add(new Spec {
                Id = Ids.CondensationMold, Title = "The Walls Breathe",
                MinDay = 20, Weight = 1.2f,
                TriggerNote = "Humidity > 70%",
                Description = "Black mold in the bunkroom ceiling. Spores in the air. Two survivors develop coughing fits. The mold spreads if the humidity isn't controlled. The only fix is ventilation, heat, or cutting out the infected panels. All three cost fuel."
            });
            list.Add(new Spec {
                Id = Ids.SilentDeath, Title = "The Quiet One",
                MinDay = 10, Weight = 0.8f,
                TriggerNote = "Survivor Morale < 15",
                Description = "A survivor stops talking. Stops eating. Stops moving from their bunk. Their eyes are open. They respond to nothing. This isn't sleep. This isn't a break. This is what happens when the mind decides the body is no longer worth operating."
            });
            list.Add(new Spec {
                Id = Ids.RatKing, Title = "The Rat King",
                MinDay = 12, Weight = 1.0f,
                TriggerNote = "WasteLevel > 80",
                Description = "The rats have nested in the waste chute. There are too many. They've started coming into the food storage. One survivor was bitten in the night. The wound is already red. Without antibiotics, the infection will spread in three days."
            });
        }

        // ── Surface / expedition encounters ────────────────────────────
        private static void AddSurface(List<Spec> list)
        {
            list.Add(new Spec {
                Id = Ids.AshQuicksand, Title = "The Ground Gives",
                MinDay = 5, Weight = 1.5f,
                TriggerNote = "Expedition, AshSwamp biome",
                Description = "The ash is three metres deep here. The top layer is crust. Under it, powder. Under the powder, mud. The scavenger's boot breaks through. Then the other boot. Then the knees. They're in to the waist. The ash is warm. It shouldn't be warm."
            });
            list.Add(new Spec {
                Id = Ids.DeadRadioOperator, Title = "Still Transmitting",
                MinDay = 8, Weight = 1.0f,
                TriggerNote = "Expedition, Radio Tower",
                Description = "A body in a chair, headset on, hand on the transmit key. The radio is still broadcasting a loop. The message is a grid reference and a single word: \"HOLD.\" The body has been dead for weeks. The batteries are still going."
            });
            list.Add(new Spec {
                Id = Ids.FrozenFamily, Title = "The Car That Didn't Make It",
                MinDay = 4, Weight = 1.2f,
                TriggerNote = "Expedition, Highway",
                Description = "A sedan off the road, doors open, engine dead. Four seats. Four shapes under blankets. They didn't make it to the shelter. The fuel tank is half full. The trunk has a suitcase of canned food. You need the fuel. You need the food. They're still in the car."
            });
            list.Add(new Spec {
                Id = Ids.MinefieldRemnant, Title = "The Field That Clicks",
                MinDay = 10, Weight = 1.8f,
                TriggerNote = "Expedition, Military zone",
                Description = "The ground is marked with faded red tape. The tape is torn. The mines are not. One step in the wrong place and the expedition ends. The safe path is marked on a map that's in a bunker three kilometres back. Or you walk very, very slowly."
            });
            list.Add(new Spec {
                Id = Ids.BlackRainPocket, Title = "Black Rain Pocket",
                MinDay = 7, Weight = 1.5f,
                TriggerNote = "Expedition, Weather: BlackSnow",
                Description = "The rain starts mid-expedition. It's not water. It's dissolved soot, ash, and fallout particulate. It coats everything. The Geiger counter goes from clicking to screaming. There's no shelter. There's a culvert two hundred metres back. Two hundred metres in black rain."
            });
        }

        // ── Moral / social encounters ───────────────────────────────────
        private static void AddMoral(List<Spec> list)
        {
            list.Add(new Spec {
                Id = Ids.WhoEats, Title = "The Last Tin",
                MinDay = 10, Weight = 1.5f,
                TriggerNote = "Food < 5 rations",
                Description = "One tin of food left. Four survivors. The math is simple. The choice isn't. Someone doesn't eat tonight. Someone doesn't eat tomorrow. The tin sits on the table and everyone looks at it and no one reaches for it. That's worse than if someone grabbed it."
            });
            list.Add(new Spec {
                Id = Ids.TheChildAsks, Title = "Why Is the Sky Grey",
                MinDay = 5, Weight = 0.8f,
                TriggerNote = "Child survivor present",
                Description = "The child asks why the sky is grey. Why the birds are gone. Why Mom isn't coming back. You can lie. You can tell the truth. You can say nothing. The child will remember whichever you choose."
            });
            list.Add(new Spec {
                Id = Ids.ExileVote, Title = "The Vote",
                MinDay = 15, Weight = 1.0f,
                TriggerNote = "Survivor conflict > 3",
                Description = "Two survivors can't share the same air. One has to leave. The bunker votes. The loser goes out the hatch. The hatch leads to 30 mSv/h and minus twenty. The vote is democratic. The result is a death sentence. Everyone's hands are raised. Everyone's eyes are on the floor."
            });
            list.Add(new Spec {
                Id = Ids.MercyRequest, Title = "Please",
                MinDay = 20, Weight = 0.6f,
                TriggerNote = "Survivor Health < 10, incurable",
                Description = "They ask you to stop. Not to fight anymore. Not to treat anymore. They want the morphine. All of it. They want to close their eyes and not open them. They're lucid. They're calm. They're asking you to kill them. The morphine is in your hand."
            });
        }

        // ── Materialise ──────────────────────────────────────────────────
        public static GameEvent Materialise(Spec spec)
        {
            if (spec == null) return null;
            var evt = ScriptableObject.CreateInstance<GameEvent>();
            evt.id = spec.Id;
            evt.title = spec.Title;
            // Embed the trigger note in bodyText's tail so a designer reading
            // the runtime event can see the brief's "Trigger" column without
            // needing to cross-reference the catalog file.
            string body = spec.Description ?? string.Empty;
            if (!string.IsNullOrEmpty(spec.TriggerNote))
                body += "\n\n[Trigger: " + spec.TriggerNote + "]";
            evt.bodyText = body;
            evt.weight = spec.Weight;
            if (evt.conditions == null) evt.conditions = new EventConditions();
            evt.conditions.MinDay = spec.MinDay;
            return evt;
        }

        public static List<GameEvent> MaterialiseAll()
        {
            var specs = BuildAll();
            var outList = new List<GameEvent>(specs.Count);
            for (int i = 0; i < specs.Count; i++) outList.Add(Materialise(specs[i]));
            return outList;
        }
    }
}
