using System;

namespace AtomicWar._Game.Factions
{
    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — canonical
    /// id registry for the 12 new NPC / fauna archetypes. Host code can
    /// use <see cref="AshGetsDeeperNpcIds"/> to look up snake_case ids
    /// without hard-coding strings; the per-NPC classes hold behaviour.
    /// </summary>
    public static class AshGetsDeeperNpcIds
    {
        public static class Ids
        {
            // Hostile / ambiguous
            public const string AshWidows         = "npc_ash_widows";
            public const string TheTollman        = "npc_the_tollman";
            public const string BurnedPatrol      = "npc_burned_patrol";
            public const string TheCollector      = "npc_the_collector";
            public const string FeralChildren     = "npc_feral_children";
            public const string SurgeonsCaravan   = "npc_the_surgeon_caravan";
            // Mutated fauna
            public const string IrradiatedDogs    = "fauna_irradiated_dogs";
            public const string AshCrows          = "fauna_crows_ash";
            public const string BloatedCattle     = "fauna_bloated_cattle";
            public const string RatSwarm          = "fauna_rat_swarm";
            // Echo / lore fragments (10)
            public const string EchoShoppingList       = "echo_shopping_list";
            public const string EchoBusTimetable       = "echo_bus_timetable";
            public const string EchoVoicemailTranscript = "echo_voicemail_transcript";
            public const string EchoGardenMarker       = "echo_garden_marker";
            public const string EchoMilitaryGravestone = "echo_military_gravestone";
            public const string EchoBunkerGraffiti     = "echo_bunker_graffiti";
            public const string EchoPharmacyBoard      = "echo_pharmacy_board";
            public const string EchoChildToy           = "echo_child_toy";
            public const string EchoRationCard         = "echo_ration_card";
            public const string EchoLastNewspaper      = "echo_last_newspaper";
        }
    }
}
