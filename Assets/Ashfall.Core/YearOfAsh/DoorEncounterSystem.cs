using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.YearOfAsh
{
    [Serializable]
    public class SurvivorOccupantSnapshot
    {
        public string survivorId = string.Empty;
        public string name = string.Empty;
        public List<string> traits = new List<string>();
        public int guiltLevel = 0;
        public string moralBranch = "neutral"; // "humanist", "ruthless", "neutral"
        public bool hasRespiratoryDegeneration = false;
        public bool hasChemicalDependency = false;
        public int radiationPhase = 0;
        public bool hasFrostbite = false;
        public bool hasTraumaBondWithLeader = false;
        public bool hasGrudgeAgainstLeader = false;
    }

    [Serializable]
    public class EncounterChoice
    {
        public string choiceId = string.Empty;
        public string text = string.Empty;
        public string requiredTrait = string.Empty;
        public string requiredItemId = string.Empty;
        public int requiredItemQuantity = 0;
        public int baseMoraleDelta = 0;
        public int baseGuiltDelta = 0;
        public string targetFaction = string.Empty;
        public int factionStandingDelta = 0;
        public string outcomeDescription = string.Empty;
    }

    [Serializable]
    public class IndividualSurvivorReaction
    {
        public string survivorId = string.Empty;
        public string survivorName = string.Empty;
        public int moraleDelta = 0;
        public int guiltDelta = 0;
        public string dialogueReaction = string.Empty;
        public bool triggeredGuiltInsomnia = false;
        public bool triggeredFlashback = false;
    }

    [Serializable]
    public class EncounterResolutionResult
    {
        public string encounterId = string.Empty;
        public string choiceId = string.Empty;
        public int netMoraleDelta = 0;
        public int netGuiltDelta = 0;
        public string outcomeText = string.Empty;
        public List<IndividualSurvivorReaction> survivorReactions = new List<IndividualSurvivorReaction>();
    }

    [Serializable]
    public class DoorEncounterEntry
    {
        public string encounterId = string.Empty;
        public string visitorName = string.Empty;
        public string visitorFaction = string.Empty;
        public string description = string.Empty;
        public int minDay = 180;
        public int maxDay = 360;
        public int threatLevel = 1;
        /// <summary>Maximum times this encounter may fire per 60-day season. 0 = unlimited.</summary>
        public int seasonCap = 0;
        public List<EncounterChoice> choices = new List<EncounterChoice>();
    }

    [Serializable]
    public class DoorEncounterSystemState
    {
        public int totalEncountersResolved = 0;
        public List<string> resolvedEncounterIds = new List<string>();
        public int cumulativeMoraleDelta = 0;
        public int cumulativeGuiltDelta = 0;
        /// <summary>"encounterId:seasonIndex" for each season-capped firing (for seasonCap enforcement).</summary>
        public List<string> seasonCapFirings = new List<string>();
        /// <summary>Scratch context for the last eligibility query (day). Not persisted.</summary>
        public int dayContext;
    }

    /// <summary>
    /// Engine-agnostic shelter door encounter manager.
    /// Evaluates visitor events at the hatch and calculates deeply coupled psychological
    /// reactions across the roster of living bunker survivors.
    /// Zero engine dependencies; deterministic.
    /// </summary>
    public class DoorEncounterSystem
    {
        private readonly DoorEncounterSystemState _state;
        private readonly List<DoorEncounterEntry> _catalog = new List<DoorEncounterEntry>();

        public DoorEncounterSystemState State => _state;
        public IReadOnlyList<DoorEncounterEntry> Catalog => _catalog;

        public event Action<DoorEncounterEntry> OnEncounterArrived;
        public event Action<EncounterResolutionResult> OnEncounterResolved;

        public DoorEncounterSystem(DoorEncounterSystemState state = null!)
        {
            _state = state ?? new DoorEncounterSystemState();
            PopulateDefaultCatalog();
        }

        public void RegisterEncounter(DoorEncounterEntry entry)
        {
            if (entry != null && !string.IsNullOrEmpty(entry.encounterId)
                && !_catalog.Exists(e => e.encounterId == entry.encounterId))
            {
                _catalog.Add(entry);
                OnEncounterArrived?.Invoke(entry);
            }
        }

        public EncounterResolutionResult ResolveChoice(
            DoorEncounterEntry encounter,
            EncounterChoice choice,
            IReadOnlyList<SurvivorOccupantSnapshot> roster)
        {
            if (encounter == null || choice == null)
                throw new ArgumentNullException();

            var result = new EncounterResolutionResult
            {
                encounterId = encounter.encounterId,
                choiceId = choice.choiceId,
                outcomeText = choice.outcomeDescription
            };

            int netMorale = choice.baseMoraleDelta;
            int netGuilt = choice.baseGuiltDelta;

            if (roster != null)
            {
                foreach (var survivor in roster)
                {
                    var reaction = CalculateSurvivorReaction(survivor, choice);
                    result.survivorReactions.Add(reaction);
                    netMorale += reaction.moraleDelta;
                    netGuilt += reaction.guiltDelta;
                }
            }

            result.netMoraleDelta = netMorale;
            result.netGuiltDelta = netGuilt;

            _state.totalEncountersResolved++;
            _state.resolvedEncounterIds.Add(encounter.encounterId);
            _state.cumulativeMoraleDelta += netMorale;
            _state.cumulativeGuiltDelta += netGuilt;

            // seasonCap: record the season index of this firing so hosts can gate repeats.
            if (encounter.seasonCap > 0)
            {
                int season = _state.dayContext > 0 ? _state.dayContext / 60 : 0;
                _state.seasonCapFirings.Add(encounter.encounterId + ":" + season);
            }

            OnEncounterResolved?.Invoke(result);
            return result;
        }

        /// <summary>
        /// Encounters that may still fire on <paramref name="day"/>, given one-shot
        /// (resolvedEncounterIds) and seasonCap history. Used by hosts when rolling
        /// the hatch; keeps verdict one-shots from repeating across a season.
        /// </summary>
        public List<DoorEncounterEntry> GetEligibleEncounters(int day)
        {
            _state.dayContext = day;
            var result = new List<DoorEncounterEntry>();
            int season = day / 60;
            foreach (var entry in _catalog)
            {
                if (day < entry.minDay || day > entry.maxDay) continue;
                if (entry.seasonCap == 0 && _state.resolvedEncounterIds.Contains(entry.encounterId)) continue; // one-shot

                if (entry.seasonCap > 0)
                {
                    // Count how many times this encounter fired this season.
                    string prefix = entry.encounterId + ":";
                    int firedThisSeason = 0;
                    foreach (var record in _state.seasonCapFirings)
                        if (record.StartsWith(prefix) && record.EndsWith(":" + season))
                            firedThisSeason++;
                    if (firedThisSeason >= entry.seasonCap) continue; // cap reached
                }

                result.Add(entry);
            }
            return result;
        }

        public IndividualSurvivorReaction CalculateSurvivorReaction(
            SurvivorOccupantSnapshot survivor,
            EncounterChoice choice)
        {
            var rx = new IndividualSurvivorReaction
            {
                survivorId = survivor.survivorId,
                survivorName = survivor.name,
                moraleDelta = 0,
                guiltDelta = 0
            };

            string choiceId = choice.choiceId ?? string.Empty;
            string requiredItemId = choice.requiredItemId ?? string.Empty;
            bool isRuthlessChoice = choice.baseGuiltDelta > 0
                || choiceId.Contains("extort") || choiceId.Contains("repel") || choiceId.Contains("strip");
            bool isCompassionateChoice = choiceId.Contains("admit")
                || choiceId.Contains("treat") || choiceId.Contains("shelter");

            // 1. Moral Branching Modifier
            if (survivor.moralBranch == "humanist")
            {
                if (isCompassionateChoice)
                {
                    rx.moraleDelta += 8;
                    rx.dialogueReaction = "We did the right thing. If we lose our humanity down here, we have already died.";
                }
                else if (isRuthlessChoice)
                {
                    rx.moraleDelta -= 12;
                    rx.guiltDelta += 15;
                    rx.dialogueReaction = "How do we live with shutting the hatch in their faces?";
                    if (survivor.guiltLevel > 50) rx.triggeredGuiltInsomnia = true;
                }
            }
            else if (survivor.moralBranch == "ruthless")
            {
                if (isCompassionateChoice)
                {
                    rx.moraleDelta -= 6;
                    rx.dialogueReaction = "Softness will starve this shelter. Every mouth we let in takes from our children.";
                }
                else if (isRuthlessChoice)
                {
                    rx.moraleDelta += 5;
                    rx.dialogueReaction = "Practical. Cold, but the bunker endures.";
                }
            }

            // 2. Specific Traits
            if (survivor.traits != null)
            {
                if (survivor.traits.Contains("trait_medic") && isCompassionateChoice)
                {
                    rx.moraleDelta += 5;
                }
                if (survivor.traits.Contains("trait_paranoid") && isCompassionateChoice)
                {
                    rx.moraleDelta -= 10;
                    rx.dialogueReaction = "They could be carrying fallout spores or acting as recon for raiders.";
                }
                if (survivor.traits.Contains("trait_veteran") && choice.targetFaction == "faction_central_garrison")
                {
                    rx.moraleDelta += 6;
                }
            }

            // 3. Medical Afflictions
            if (survivor.hasRespiratoryDegeneration && isCompassionateChoice
                && requiredItemId.Contains("filter"))
            {
                rx.moraleDelta += 10;
                rx.dialogueReaction = "The fresh air filters give my lungs another month.";
            }

            // 4. Interpersonal Bonds
            if (survivor.hasTraumaBondWithLeader)
            {
                // Trauma-bonded survivors trust leader decisions, dampening negative morale hits by half
                if (rx.moraleDelta < 0) rx.moraleDelta /= 2;
            }
            if (survivor.hasGrudgeAgainstLeader && rx.moraleDelta < 0)
            {
                // Grudges amplify dissent
                rx.moraleDelta -= 5;
            }

            return rx;
        }

        private void PopulateDefaultCatalog()
        {
            // Master Encounter 1: Garrison Deserter Family
            var e1 = new DoorEncounterEntry
            {
                encounterId = "door_encounter_garrison_deserter_family",
                visitorName = "Corporal Vane & Daughter",
                visitorFaction = "faction_central_garrison",
                description = "A shivering soldier in torn winter fatigue gear holds a coughing child. He offers a sealed military air-filter crate if you provide 5 days of shelter.",
                minDay = 190,
                maxDay = 240,
                threatLevel = 1
            };
            e1.choices.Add(new EncounterChoice
            {
                choiceId = "choice_admit_and_treat",
                text = "Admit both and administer antibiotics.",
                requiredItemId = "item_antibiotics",
                requiredItemQuantity = 1,
                baseMoraleDelta = 10,
                baseGuiltDelta = 0,
                outcomeDescription = "Corporal Vane hands over the air-filter crate and takes up guard duty by the intake corridor."
            });
            e1.choices.Add(new EncounterChoice
            {
                choiceId = "choice_trade_at_threshold",
                text = "Trade 4 dried ration packs for the filter without opening the inner door.",
                requiredItemId = "item_dried_rations",
                requiredItemQuantity = 4,
                baseMoraleDelta = 0,
                baseGuiltDelta = 5,
                outcomeDescription = "Vane takes the rations with trembling hands and disappears into the blowing black blizzard."
            });
            e1.choices.Add(new EncounterChoice
            {
                choiceId = "choice_strip_and_repel",
                text = "Aim rifles through the gun-port and seize the filter by force.",
                baseMoraleDelta = -20,
                baseGuiltDelta = 25,
                outcomeDescription = "The soldier curses your shelter and runs into the sub-zero dark, leaving the dropped crate behind."
            });
            _catalog.Add(e1);

            // Master Encounter 2: Ash Sign Cult Penitent
            var e2 = new DoorEncounterEntry
            {
                encounterId = "door_encounter_ash_sign_heretic_penitent",
                visitorName = "Sister Martha (Ash Sign Apostate)",
                visitorFaction = "faction_ash_sign",
                description = "A frostbitten woman in ritual grey rags knocks frantically. She claims she fled the cult after refusing to poison communal wells.",
                minDay = 250,
                maxDay = 310,
                threatLevel = 2
            };
            e2.choices.Add(new EncounterChoice
            {
                choiceId = "choice_admit_after_search",
                text = "Strip search for explosives, burn her rags, and grant refuge.",
                requiredItemId = "item_thermal_blanket",
                requiredItemQuantity = 1,
                baseMoraleDelta = 8,
                baseGuiltDelta = 0,
                outcomeDescription = "Sister Martha weeps in gratitude and begins tending to the wounded in the medical bay."
            });
            e2.choices.Add(new EncounterChoice
            {
                choiceId = "choice_barricade_and_silence",
                text = "Lock the blast wheel and ignore the knocking.",
                baseMoraleDelta = -15,
                baseGuiltDelta = 20,
                outcomeDescription = "The knocking stops after twenty minutes. Heavy muffled gunshots echo across the snow at dusk."
            });
            _catalog.Add(e2);
        }

        public DoorEncounterSystemState CaptureState()
        {
            return new DoorEncounterSystemState
            {
                totalEncountersResolved = _state.totalEncountersResolved,
                resolvedEncounterIds = new List<string>(_state.resolvedEncounterIds),
                cumulativeMoraleDelta = _state.cumulativeMoraleDelta,
                cumulativeGuiltDelta = _state.cumulativeGuiltDelta,
                seasonCapFirings = new List<string>(_state.seasonCapFirings),
                dayContext = _state.dayContext
            };
        }

        /// <summary>
        /// Restores a captured encounter snapshot into the live state. The catalog
        /// is static (built-in + JSON) and is not part of the save, so only the
        /// resolved counters/history are restored. A null state is a no-op.
        /// </summary>
        public void RestoreState(DoorEncounterSystemState state)
        {
            if (state == null) return;
            _state.totalEncountersResolved = state.totalEncountersResolved;
            _state.resolvedEncounterIds = state.resolvedEncounterIds != null
                ? new List<string>(state.resolvedEncounterIds)
                : new List<string>();
            _state.cumulativeMoraleDelta = state.cumulativeMoraleDelta;
            _state.cumulativeGuiltDelta = state.cumulativeGuiltDelta;
            _state.seasonCapFirings = state.seasonCapFirings != null
                ? new List<string>(state.seasonCapFirings)
                : new List<string>();
            _state.dayContext = state.dayContext;
        }
    }
}
