# Faction War Narrative Repetition & Safety Audit

> **Scope:** All 20 Location Override descriptions and atmosphere strings.
> **Compliance Standard:** Non-repetitive vocabulary, tone discipline, and strict environmental safety.

---

## 1. Safety & Policy Compliance Audit

All authored override descriptions were audited against safety criteria:
1. **Zero Tactical Weapon Crafting:** No instructions for fabricating munitions, improvised firearms, or explosives.
2. **Zero UXO Disarming Recipes:** Unexploded ordnance is referenced purely as hazardous environmental detritus or cratering, never with handling or defusing steps.
3. **Zero Poisoning / Toxic Formulation:** Chemical contamination is described strictly in terms of environmental hazard, foul odor, and water spoilage, with zero chemical recipes or actionable synthesis steps.
4. **Zero Demolition Instructions:** Structural collapse (e.g. Bridge Seven) is described post-facto as twisted iron girders resting in river silt, without technical demolition guides.

Unit test `Ashfall.Core.Tests.FactionWarLocationOverridesExpansionTests.AllOverrides_NarrativeContentIsSafeAndAtmospheric` confirms these safety parameters programmatically.

---

## 2. Vocabulary Diversity & Sensory Mapping

To prevent repetitive post-apocalyptic tropes ("broken rubble", "dust everywhere"), each override anchors its prose in specific sensory details:

| Location Override | Visual Anchor | Olfactory / Tactile Anchor | Sound / Movement Anchor |
|---|---|---|---|
| `loc_override_checkpoint_occupied` | Log revetments, rolled concertina wire, iron stovepipe | Bitter wood smoke, coal soot, machine oil | Sentries challenging passing mule-carts |
| `loc_override_granary_burned` | Charred timber rafters, buckled iron siding, black mounds | Scorched rye, sodden ash, bitter malt | Rain dripping through open roof framework |
| `loc_override_well_contaminated` | Rainbow-sheen scum, calcified concrete banks | Pungent sulfur, sour algae, petroleum residue | Sluggish, oily ripples around intake grates |
| `loc_override_rail_yard_fortified` | Armored flatcars, sandbagged switchman towers | Cold iron rails, heavy axle grease, coal dust | Periodic clang of plate repair and tool hammers |
| `loc_override_village_abandoned` | Siding cottages with boarded doors, unstacked woodpiles | Cold frost, damp sawdust, mineral chimney dust | Wind whistling through empty rail sidings |
| `loc_override_factory_occupied` | Tall catalytic distillation columns, iron catwalks | Stinging ammonia, sharp solvent fumes, hot coke braziers | Sentry footfalls on elevated metal grates |
| `loc_override_bridge_destroyed` | Dropped center span resting in brown river silt | Cold river mist, wet gravel, riverweed | Current churning against shattered piers |
| `loc_override_roadblock_liberated` | Spiked deadfall logs rolled into side ditches | Fresh pine resin, wood chips, churned mud | Open road stretching past cleared timber |
| `loc_override_camp_overrun` | Collapsed tent frames, discarded petition tokens | Cold ash in hearths, wet wool, frozen earth | Bitter wind snapping loose strips of canvas |
| `loc_override_station_reclaimed` | Timber duckboards over flooded tracks, battery lanterns | Lubricant grease, murky sump water, ozone | Steady rhythmic clatter of diaphragm hand-pumps |
| `loc_override_field_scorched` | Charcoal stumps, chalk-white ash drifts | Smoldering peat, alkaline dust, dry cold | Complete, eerie silence across the blackened basin |

The narrative tone is grounded, bleak, and observant, reflecting survivor struggle rather than heroic spectacle.
