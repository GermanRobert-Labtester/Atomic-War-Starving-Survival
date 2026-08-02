using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar.Data
{
    public static class SurvivorCatalog
    {
        public static List<DetailedSurvivorProfile> CreateAll12Survivors()
        {
            return new List<DetailedSurvivorProfile>
            {
                new DetailedSurvivorProfile
                {
                    Id = "survivor_aris",
                    CharacterName = "Dr. Aris Thorne",
                    Age = 44,
                    PreWarOccupation = "Radiotherapy Specialist & Dosimetry Inspector",
                    PersonalityTrait = "Clinical detachment under stress, hyper-analytical",
                    UsefulSkill = "Triage & Radiation Decontamination (treats radiation sickness & tests food/water)",
                    Weakness = "Chronic insomnia & mild hand tremors from early fallout exposure",
                    SecretFear = "Dying of the very cancers he spent twenty years treating in others",
                    MoralLine = "Will perform agonizing field procedures without anesthesia to save lives, but refuses to kill or abandon anyone suffering from acute radiation sickness.",
                    Biography = "Aris spent decades calibrating radiation therapy units in the city general hospital. When the sirens blared, he didn't run for the shelters; he stayed behind to secure cobalt-60 medical sources from leaking into the water table. He carries a brass pocket dosimeter and views survival not as hope, but as a series of chemical equations that must balance.",
                    MaxHealth = 90f,
                    BaseHungerDecayRate = 1.8f,
                    BaseFatigueDecayRate = 1.6f,
                    MoralSensitivity = 0.8f,
                    CraftingSpeedMultiplier = 1.2f,
                    CombatEfficiency = 0.7f
                },
                new DetailedSurvivorProfile
                {
                    Id = "survivor_marek",
                    CharacterName = "Marek Vance",
                    Age = 38,
                    PreWarOccupation = "Municipal Boiler Technician & Heating Maintenance",
                    PersonalityTrait = "Stoic, quiet worker who speaks only when necessary",
                    UsefulSkill = "Resourceful Improvised Engineering (repairs heaters, water stills & fortifies shelter)",
                    Weakness = "Heavy nicotine dependency; suffers tremors and irritability when deprived of tobacco",
                    SecretFear = "Claustrophobia—terrified of being buried alive beneath collapsed concrete basements",
                    MoralLine = "Will steal supplies from abandoned structures without hesitation, but will not steal from occupied homes with children.",
                    Biography = "Before the strikes, Marek spent fifteen years in the labyrinthine steam tunnels beneath the industrial district. His technical knowledge keeps the shelter warm and water drinkable, but the memory of hearing his crew trapped behind a collapsed bulkhead during the firestorms haunts every dark room he enters.",
                    MaxHealth = 100f,
                    BaseHungerDecayRate = 2.2f,
                    BaseFatigueDecayRate = 1.4f,
                    MoralSensitivity = 1.0f,
                    CraftingSpeedMultiplier = 1.4f,
                    CombatEfficiency = 1.1f
                },
                new DetailedSurvivorProfile
                {
                    Id = "survivor_elena",
                    CharacterName = "Elena Rostova",
                    Age = 29,
                    PreWarOccupation = "High School Chemistry Teacher",
                    PersonalityTrait = "Empathetic, overly protective, prone to survivor guilt",
                    UsefulSkill = "Pharmaceutical Synthesis & Distillation (distills clean water & brews crude antiseptics)",
                    Weakness = "Physical frailty; poor stamina and highly susceptible to ash-induced respiratory infections",
                    SecretFear = "Becoming the reason someone else dies due to a mistake in her chemical mixtures",
                    MoralLine = "Will lie to keep peace in the shelter, but refuses to trade medicine for weapons or ammunition.",
                    Biography = "Elena was grading midterm papers when the sky turned white. She led eleven of her students into a subway cellar, but only two survived the subsequent fallout week. She keeps her surviving students' chalk-smeared notebooks in her coat lining, driven by an overwhelming need to preserve life at any personal cost.",
                    MaxHealth = 85f,
                    BaseHungerDecayRate = 1.7f,
                    BaseFatigueDecayRate = 1.8f,
                    MoralSensitivity = 1.4f,
                    CraftingSpeedMultiplier = 1.3f,
                    CombatEfficiency = 0.6f
                },
                new DetailedSurvivorProfile
                {
                    Id = "survivor_kael",
                    CharacterName = "Kaelen 'Kael' Miller",
                    Age = 52,
                    PreWarOccupation = "Civil Defense Logistics Clerk & Warehouse Manager",
                    PersonalityTrait = "Pragmatic, obsessive organizer, cynical about human nature",
                    UsefulSkill = "Supply Rationing & Micro-Inventory (stretches shelter food and fuel by 25%)",
                    Weakness = "Suspicious of newcomers; refuses to share rations with non-contributors",
                    SecretFear = "Being outvoted or exiled from the shelter group",
                    MoralLine = "Will strictly hoard supplies and refuse aid to starving strangers, but will never physically harm a shelter mate.",
                    Biography = "Kael worked in municipal emergency storage, cataloging crates of gas masks and canned biscuits that were embezzled or neglected long before the bombs fell. His cynicism is his armor. He knows precisely how many calories a human body needs to survive sixty days of siege, and he counts every cracker like gold.",
                    MaxHealth = 95f,
                    BaseHungerDecayRate = 1.6f,
                    BaseFatigueDecayRate = 1.3f,
                    MoralSensitivity = 0.7f,
                    CraftingSpeedMultiplier = 1.1f,
                    CombatEfficiency = 0.9f
                },
                new DetailedSurvivorProfile
                {
                    Id = "survivor_nadia",
                    CharacterName = "Nadia Petrov",
                    Age = 24,
                    PreWarOccupation = "Amateur Radio Operator & Electrical Repair Apprentice",
                    PersonalityTrait = "Curious, restless, fiercely independent",
                    UsefulSkill = "Electronic Scavenging & Signal Intercept (builds radios & intercepts emergency broadcasts)",
                    Weakness = "Impulsive; takes unnecessary physical risks during night scavenging runs",
                    SecretFear = "Total silence—terrified that there are no other survivors left anywhere on earth",
                    MoralLine = "Will eavesdrop, hack, or scavenge restricted military zones, but will not execute an unarmed person under any circumstance.",
                    Biography = "Nadia lived on the top floor of a high-rise with her ham radio kit. When the electrical grid collapsed, she spent seventy-two hours tuning through static, recording faint distress signals from dying cities. She carries a hand-crank radio everywhere, searching for a sign that humanity exists beyond their ruined horizon.",
                    MaxHealth = 90f,
                    BaseHungerDecayRate = 2.0f,
                    BaseFatigueDecayRate = 1.5f,
                    MoralSensitivity = 1.1f,
                    CraftingSpeedMultiplier = 1.2f,
                    CombatEfficiency = 1.0f
                },
                new DetailedSurvivorProfile
                {
                    Id = "survivor_borys",
                    CharacterName = "Borys 'The Bear' Kowalski",
                    Age = 49,
                    PreWarOccupation = "Butcher & Abattoir Manager",
                    PersonalityTrait = "Gentle demeanor hiding immense physical strength; deeply remorseful",
                    UsefulSkill = "Butchery & Preserving Food (processes meat, smokes provisions & heavy lifting)",
                    Weakness = "Slow-moving; requires high caloric intake to maintain strength",
                    SecretFear = "Losing control of his own strength and hurting someone innocent in panic",
                    MoralLine = "Will kill in direct self-defense of shelter members, but refuses to scavenge from or desecrate bodies of the dead.",
                    Biography = "Borys ran a modest neighborhood butcher shop known for handmade sausages. When food riots erupted in the first week after the strikes, he defended his store with an iron bar, witnessing horrors that shattered his gentle spirit. Now, he uses his massive build to clear rubble and protect the shelter, though he refuses to touch a firearm.",
                    MaxHealth = 120f,
                    BaseHungerDecayRate = 2.5f,
                    BaseFatigueDecayRate = 1.6f,
                    MoralSensitivity = 1.2f,
                    CraftingSpeedMultiplier = 1.0f,
                    CombatEfficiency = 1.4f
                },
                new DetailedSurvivorProfile
                {
                    Id = "survivor_vera",
                    CharacterName = "Vera Solokova",
                    Age = 33,
                    PreWarOccupation = "Architectural Historian & Draftsman",
                    PersonalityTrait = "Observant, quiet, meticulous planner",
                    UsefulSkill = "Structural Evaluation & Stealth (identifies safe routes in crumbling buildings & hidden caches)",
                    Weakness = "Poor night vision; struggles to navigate in absolute darkness without light",
                    SecretFear = "Blindness from flash-burns or fallout dust",
                    MoralLine = "Will abandon a scavenging run if innocent lives are put at risk, but will not share structural blueprints with hostile scavengers.",
                    Biography = "Vera documented historic stone buildings for the city heritage board. Her intimate knowledge of pre-war floor plans, hidden cellars, and drainage systems allows her to navigate ruined structures where others see only impenetrable debris.",
                    MaxHealth = 90f,
                    BaseHungerDecayRate = 1.9f,
                    BaseFatigueDecayRate = 1.4f,
                    MoralSensitivity = 1.0f,
                    CraftingSpeedMultiplier = 1.1f,
                    CombatEfficiency = 0.8f
                },
                new DetailedSurvivorProfile
                {
                    Id = "survivor_tomek",
                    CharacterName = "Tomas 'Tomek' Hasek",
                    Age = 19,
                    PreWarOccupation = "Bicycle Courier & Mechanics Apprentice",
                    PersonalityTrait = "Energetic, eager to prove himself, masking deep insecurity",
                    UsefulSkill = "Rapid Scavenging & Agility (sprints through hazard zones and retrieves heavy items quickly)",
                    Weakness = "Naive about human malice; easily manipulated by hostile survivors",
                    SecretFear = "Being perceived as useless or discarded by the group",
                    MoralLine = "Will steal supplies if ordered by the group, but collapses emotionally if forced to witness violence.",
                    Biography = "Tomek spent his teens weaving through city traffic delivering packages. When the bombs fell, he survived on speed and instinct alone. He views the shelter members as his replacement family and works himself to exhaustion to earn their approval.",
                    MaxHealth = 95f,
                    BaseHungerDecayRate = 2.3f,
                    BaseFatigueDecayRate = 1.5f,
                    MoralSensitivity = 1.3f,
                    CraftingSpeedMultiplier = 1.1f,
                    CombatEfficiency = 0.9f
                },
                new DetailedSurvivorProfile
                {
                    Id = "survivor_irina",
                    CharacterName = "Dr. Irina Danilova",
                    Age = 57,
                    PreWarOccupation = "Veterinary Surgeon",
                    PersonalityTrait = "Blunt, no-nonsense, deeply compassionate toward animals and the vulnerable",
                    UsefulSkill = "Emergency Trauma Surgery & Infection Control (performs field surgeries with crude tools)",
                    Weakness = "Severe arthritis in knees; cannot run or climb ladders quickly",
                    SecretFear = "Running out of pain medication when someone is dying in agony",
                    MoralLine = "Will treat wounded enemies if they lay down arms, but will euthanize anyone suffering untreatable, agonizing terminal sickness if requested.",
                    Biography = "Irina spent thirty years treating livestock and pets in the agricultural belt. When urban hospitals burned, her rural clinic became a triage center for burn victims. She carries a battered leather surgical kit and speaks with authority born of decades of saving lives under harsh conditions.",
                    MaxHealth = 85f,
                    BaseHungerDecayRate = 1.6f,
                    BaseFatigueDecayRate = 1.7f,
                    MoralSensitivity = 1.1f,
                    CraftingSpeedMultiplier = 1.2f,
                    CombatEfficiency = 0.7f
                },
                new DetailedSurvivorProfile
                {
                    Id = "survivor_luka",
                    CharacterName = "Luka Kovac",
                    Age = 41,
                    PreWarOccupation = "Locksmith & Safe Technician",
                    PersonalityTrait = "Reserved, distrustful, hyper-vigilant",
                    UsefulSkill = "Lockpicking & Security Fortification (breaches locked vaults & crafts door barricades)",
                    Weakness = "Paranoia; insists on sleeping near exits and mistrusts group decisions",
                    SecretFear = "Being betrayed while asleep",
                    MoralLine = "Will pick locks and break into sealed government or corporate stockpiles, but refuses to break into private homes where families hide.",
                    Biography = "Luka ran a small key-duplication and locksmith shop. He knows every lock mechanism produced in the last fifty years. In the fallout, his specialized skills are invaluable for opening sealed shelter doors and reinforcing their own shelter against night intruders.",
                    MaxHealth = 100f,
                    BaseHungerDecayRate = 2.0f,
                    BaseFatigueDecayRate = 1.4f,
                    MoralSensitivity = 0.9f,
                    CraftingSpeedMultiplier = 1.3f,
                    CombatEfficiency = 1.2f
                },
                new DetailedSurvivorProfile
                {
                    Id = "survivor_sonia",
                    CharacterName = "Sonia Varga",
                    Age = 36,
                    PreWarOccupation = "Primary School Cook & Urban Gardener",
                    PersonalityTrait = "Warm, maternal, resilient sense of humor despite tragedy",
                    UsefulSkill = "Indoor Hydroponics & Foraging (cultivates food in low-light setups & identifies edible wild plants)",
                    Weakness = "Grief-stricken; breaks down when reminded of lost family members",
                    SecretFear = "Starving to death in total darkness",
                    MoralLine = "Will share her last morsel of food with a child, but will fiercely defend the shelter garden against thieves with lethal force.",
                    Biography = "Sonia cooked daily meals for four hundred elementary students before the war. She managed to salvage seeds and UV growing lamps from a damaged greenhouse center, turning dark basement corners into life-saving micro-gardens.",
                    MaxHealth = 90f,
                    BaseHungerDecayRate = 1.8f,
                    BaseFatigueDecayRate = 1.5f,
                    MoralSensitivity = 1.3f,
                    CraftingSpeedMultiplier = 1.2f,
                    CombatEfficiency = 0.8f
                },
                new DetailedSurvivorProfile
                {
                    Id = "survivor_gideon",
                    CharacterName = "Gideon Cross",
                    Age = 61,
                    PreWarOccupation = "Retired Meteorological Station Operator",
                    PersonalityTrait = "Reflective, weather-wise, stubborn",
                    UsefulSkill = "Fallout Weather Forecasting (predicts toxic fallout winds, acid rain & temperature drops 24h in advance)",
                    Weakness = "Reduced physical stamina; suffers from chronic asthmatic cough in dusty air",
                    SecretFear = "The black ash sky never clearing, leaving the earth in perpetual nuclear winter",
                    MoralLine = "Will prioritize weather safety warnings over scavenging targets, but will never abandon a lost teammate during a toxic storm.",
                    Biography = "Gideon spent forty years monitoring barometer trends and wind patterns at an isolated hill station. His ability to read atmospheric shifts enables the shelter to prepare for deadly fallout rain and sub-zero cold waves hours before they strike.",
                    MaxHealth = 80f,
                    BaseHungerDecayRate = 1.5f,
                    BaseFatigueDecayRate = 1.8f,
                    MoralSensitivity = 0.9f,
                    CraftingSpeedMultiplier = 1.0f,
                    CombatEfficiency = 0.6f
                }
            };
        }
    }
}
