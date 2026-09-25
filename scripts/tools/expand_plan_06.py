#!/usr/bin/env python3
# SPDX-License-Identifier: MIT
"""
Expansion tool for Plan 06 (Narrative Depth Trilogy) to reach >= 250,000 characters.
Anchored to docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md.
"""

import os
import sys

def main():
    filepath = "piagentsplans/06-narrative-depth-trilogy.md"
    with open(filepath, "r", encoding="utf-8") as f:
        content = f.read()

    print(f"Plan 06 current length: {len(content)}")

    if len(content) >= 252000:
        print("Plan 06 already exceeds 250k characters.")
        return

    # Generate additional rich content: Historical Transcripts, Diplomatic Summits, and Epilogue Permutations
    extra_content = """
# 26. Complete 20-Entry Faction Diplomatic Transcripts & Archival Intercepts

To satisfy **Volume 16 (Worked Content Tranches)** and **Volume 34 (Chronicle and Lore Codex)** of the Master Expansion Authority, the 20 official diplomatic transcripts, truce accords, and faction tribunal records are cataloged below.

### 26.1 Transcript #001 — The First Ashen Basin Boundary Accord
- **Document Registry**: `transcript_accord_ashen_basin_01`
- **Signatories**: Overseer Donald Vance (Consolidated Citadel Authority) & Elder Moira Stone (Valley Freehold)
- **Diegetic Setting**: Neutral rail crossing at Iron Junction 4, winter of Year 14.
- **Verbatim Text**:
> "Clause 1. The Consolidated Citadel Authority recognizes the agricultural autonomy of the Valley Freehold south of the basalt ridge line, provided forty percent of all unrefined grain yields are transferred bi-monthly to the Silo Terminus.
> Clause 2. In exchange, the Citadel promises twelve barrels of diesel fuel and four crates of industrial antibiotics at the spring equinox.
> Clause 3. Any carrier or wanderer found north of the rail spur without a stamped iron token will be interned in the lower quarry labor gangs."
- **Historical Analysis & Moral Weight**:
  This accord sealed the fate of the outer unmapped homesteads. By trading grain for diesel, the Valley Freehold surrendered their independence in exchange for temporary warmth, establishing the Citadel's economic monopoly across the river basins.

### 26.2 Transcript #002 — Tribunal of the Deserter Silas
- **Document Registry**: `transcript_tribunal_silas_deserter`
- **Interrogator**: Tribune Julia Kroll, Vanguard 7th Siege Battalion
- **Accused**: Corporal Silas Vance
- **Diegetic Setting**: Damp concrete bunker beneath the ruins of the northern communications relay.
- **Verbatim Text**:
> "Tribune: You abandoned your post on the parapet during the bombardment of Outpost 9. Why?
> Corporal Silas: The artillery shells weren't falling on marauders. They were hitting the refugee tents by the river. I saw the laundry drying on the razor wire. The captain knew they were unarmed civilians.
> Tribune: In the Vanguard, we do not weigh civilian blood against ballistic trajectory. The order was to clear the valley of foraging competition.
> Corporal Silas: Then shoot me now, Julia. Because if you give me that rifle back, I will turn around and fire at your observation post."
- **Gameplay Impact**:
  Recovering this audio wax cylinder from the flooded bunker unlocks the `trait_unyielding_conscience` for any survivor with military background, boosting moral resistance by 15% when defending the shelter against armed raids.

### 26.3 Transcript #003 — Merchant Cartel Price Fixing Treaty
- **Document Registry**: `transcript_cartel_price_fixing_03`
- **Signatories**: Guildmaster Burl (Iron Ridge Caravaners) & Factor Malo (Salt Flat Haulers)
- **Diegetic Setting**: Back room of the Rusty Piston Saloon, Junction 12.
- **Verbatim Text**:
> "Factor Malo: The winter will be bitter. The hydroponic dome in Sector 3 suffered a frost breach last Tuesday.
> Guildmaster Burl: Then clean water is five times its weight in lead ammunition. No merchant from the Ridge or the Flats sells a gallon of decontaminated well water for less than twelve copper tokens.
> Factor Malo: And what of the wandering families from the south?
> Guildmaster Burl: If they cannot pay, their pack beasts and boots will pay for them. A dry mouth makes an obedient tenant."
- **Gameplay Impact**:
  Presents the player with the economic choice to expose the cartel using the shelter radio, dropping regional market prices by 25% but triggering caravan hostility against shelter expeditions.

### 26.4 Transcript #004 — The Last Radio Transmission of Station Echo-6
- **Document Registry**: `transcript_last_transmission_echo_06`
- **Operator**: Chief Radioman Douglas Vance
- **Frequency**: 3.725 MHz Upper Sideband
- **Diegetic Setting**: Mountaintop repeater cabin during an active blizzard.
- **Verbatim Text**:
> "This is Douglas at Echo-6. Do not send a relief crew. I repeat, do not send relief.
> The radio mast collapsed under six inches of black ice twenty minutes ago. The backup battery acid has frozen solid. The temperature in the transmitter cabin is minus twenty-two degrees.
> My fingers are too numb to key the transmitter again. To my daughter Sarah: your music box is wrapped in sheepskin inside my green locker. Keep it away from the moisture. It was a good watch."
- **Gameplay Impact**:
  Unlocks the mountain cache `secret_echo_6_locker`, granting a pristine mechanical music box that permanently halts survivor trauma degradation when placed in the shelter common quarters.

### 26.5 Transcript #005 — Order of Banished Souls: The Heretic's Vow
- **Document Registry**: `transcript_heretics_vow_order_banished`
- **Speaker**: High Preacher Paul of the Ash-Cleansed
- **Diegetic Setting**: Abandoned cooling tower cathedral, illuminated by glowing radioactive sulfur pits.
- **Verbatim Text**:
> "We do not mourn the burning of the old world. The atom was not a catastrophe; it was an act of holy cauterization.
> The flesh that rots in the soil is the flesh that worshipped steel and false machines. Let the rad-spore flower in our blood.
> Those who hide in concrete deep beneath the earth are cowards who delay their redemption. Bring them the holy light. Strip the lead filters from their intake vents."
- **Gameplay Impact**:
  Provides crucial tactical intelligence regarding cultist suicide-bombing routes against shelter air-filtration systems.

### 26.6 Transcript #006 — Citadel Water Purification Secret Log
- **Document Registry**: `transcript_citadel_water_purification_secret`
- **Author**: Chief Chemist Helena Vance
- **Diegetic Setting**: Subterranean Filtration Level 4, Citadel Primary Spire.
- **Verbatim Text**:
> "The reverse-osmosis membranes are degraded beyond chemical cleaning. For three months, the drinking supply distributed to the Outer Settlement slums has contained strontium-90 levels twelve times above lethal safety limits.
> The Provost ordered me to falsify the daily test assays. He stated that the lower caste's life expectancy does not warrant the replacement cost of synthetic pre-filters.
> God forgive me. I have poured clean water into my own jug while handing slow poison to children through the ration hatch."
- **Gameplay Impact**:
  Can be leaked via radio to spark a slave uprising inside the Citadel, halving the Citadel's military patrol strength while flooding the region with refugees seeking asylum.

### 26.7 Transcript #007 — Vanguard Quartermaster Munitions Manifest
- **Document Registry**: `transcript_vanguard_quartermaster_munitions`
- **Author**: Quartermaster Sergeant Thorne
- **Diegetic Setting**: Rail Yard 3 Bunker.
- **Verbatim Text**:
> "Inventory reconciled: 450 crates of 7.62x54mm armor-piercing cartridges; 120 fragmentation mortars; 4 heavy flamethrowers with pressurized napalm cylinders.
> All munitions inspected and greased. We are prepared to flatten the Valley Freehold within forty-eight hours of order execution. No quarter to be offered to armed resistance."
- **Gameplay Impact**:
  Allows the player's expedition scouts to set fire to the rail yard munitions depot, averting the siege of the Freehold entirely.

### 26.8 Transcript #008 — Doctor Mikhail's Last Prescription
- **Document Registry**: `transcript_doctor_mikhail_last_prescription`
- **Physician**: Dr. Mikhail Voronov
- **Patient**: Little Anna (Age 6)
- **Diegetic Setting**: Isolation Tent, Valley Clinic.
- **Verbatim Text**:
> "Prescription: Pure mountain honey, one teaspoon morning and night. Two drops of willow bark tea for the fever.
> Note to Mother: The penicillin is gone. We traded the last ampoule for two bags of dried peas three days ago.
> Do not let her see you weep. Hold her hands until the fever breaks or until her breathing slows. The world was too cruel for her anyway."
- **Gameplay Impact**:
  Finding this prescription unlocks the survivor trait `trait_fierce_guardian`, making the mother impervious to morale breaks during starvation crises.

### 26.9 Transcript #009 — The Pact of the Iron Anvil
- **Document Registry**: `transcript_pact_iron_anvil_blacksmiths`
- **Signatories**: Five Independent Wasteland Weapon Smiths
- **Diegetic Setting**: Charred Foundry Hearth at Redoubt Point.
- **Verbatim Text**:
> "We declare: No smith among us shall forge bayonets for the Provost or the Vanguard.
> We forge plowshares, pickaxes, crosscut saws, and iron stove doors.
> If an army comes demanding blades and gun-barrels, we crack our crucibles, douse our coals, and take our hammers into the high mountains."
- **Gameplay Impact**:
  Unlocks the secret production schematic for high-durability cold-rolled agricultural tools in the shelter workshop.

### 26.10 Transcript #010 — Recon Scout Mara's Border Log
- **Document Registry**: `transcript_recon_scout_mara_border_log`
- **Scout**: Mara of the North Ridge
- **Diegetic Setting**: Camouflaged hunting blind overlooking the salt marsh.
- **Verbatim Text**:
> "Day 4: The Citadel gun-trucks are running night patrols without headlights, using pre-war infrared lamps.
> They are tracking refugee footpaths using bloodhounds in padded gas masks.
> I counted twenty-four people taken into the prison van last night. The marsh water smells of diesel and old grease."
- **Gameplay Impact**:
  Increases expedition stealth efficiency across wetland hexes by 20%.

### 26.11 Transcript #011 — Smuggler's Waybill: The Hidden Pass
- **Document Registry**: `transcript_smugglers_waybill_hidden_pass`
- **Author**: Courier Jack the Fox
- **Diegetic Setting**: Carved into the sandstone wall of Devil's Chasm.
- **Verbatim Text**:
> "Route verified. Avoid the canyon floor; Citadel seismic sensors are buried every hundred paces.
> Climb the western talus slope. When you reach the twin dead pines, slide through the narrow fissure behind the shale slab.
> It bypasses the toll gate completely and comes out behind the Old Mill pond. Keep your mules muzzled."
- **Gameplay Impact**:
  Reveals a hidden bypass route on the world map that avoids all toll fees and hostile military checkpoints.

### 26.12 Transcript #012 — The Last Council of the Valley Freehold
- **Document Registry**: `transcript_last_council_valley_freehold`
- **Moderator**: Councilwoman Sarah Vance
- **Diegetic Setting**: Town Hall Basement, Ashfall Valley.
- **Verbatim Text**:
> "We have three days of flour remaining. The Citadel vanguard is five miles away, demanding unconditional surrender of our grain silos.
> Half the council wants to burn the silos and flee into the basalt caves. The other half wants to open the gates with white flags.
> I will not live as a collar-wearing quarry slave. I have loaded my shotgun with two brass cartridges. When the council votes, I will know which door to walk through."
- **Gameplay Impact**:
  Triggers a dynamic radio decision event allowing the player to provide shelter asylum to the council refugees or secure their abandoned food stocks.

### 26.13 Transcript #013 — Intercepted Radio Cipher: Operation Black Winter
- **Document Registry**: `transcript_intercepted_cipher_black_winter`
- **Origin**: Vanguard High Command
- **Decoded By**: Shelter Cryptography Terminal
- **Verbatim Text**:
> "TO COMMANDER SECTOR 4: COMMENCE OPERATION BLACK WINTER ON DAY 560.
> ALL ARTILLERY BATTERIES TO EXPEND PHOSPHORUS MUNITIONS ON THE PERIMETER FARMS.
> DESTROY ALL HYDROELECTRIC DAM FLOODGATES. ENSURE THE LOWER BASIN IS INUNDATED WITH TOXIC SLUDGE BEFORE SPRING."
- **Gameplay Impact**:
  Provides early warning of flood hazards, prompting the player to build flood defenses and seal lower shelter bulkheads before Day 560.

### 26.14 Transcript #014 — The Diary of the Last Lineman
- **Document Registry**: `transcript_diary_last_lineman`
- **Author**: Peter Vance, Electrical Engineer
- **Diegetic Setting**: High-voltage pylon maintenance shack.
- **Verbatim Text**:
> "The transformer oil caught fire at noon. I stayed up on the gantry for six hours fighting the blaze with dry sand.
> The copper cables are humming at sixty hertz again. The valley clinic has light for another night.
> My boots are melted to my socks and my lungs taste like burnt varnish. But the lights are on. That is all that matters."
- **Gameplay Impact**:
  Unlocks advanced electrical wiring schematics, reducing shelter generator fuel consumption by 15%.

### 26.15 Transcript #015 — Confession of the Water Guard
- **Document Registry**: `transcript_confession_water_guard`
- **Author**: Guard Samuel
- **Diegetic Setting**: Guard Post 8, North Reservoir.
- **Verbatim Text**:
> "I took five gold coins from the merchant caravan yesterday. I looked the other way while they dumped two barrels of chemical effluent into the overflow creek.
> Now the children in the fishing village are vomiting blood. I have the gold coins in my pocket. They feel heavy like stones pulled from a furnace."
- **Gameplay Impact**:
  Provides proof to convict the corrupt merchant, recovering clean water reserves and restoring camp order.

### 26.16 Transcript #016 — The Botanist's Final Testament
- **Document Registry**: `transcript_botanists_final_testament`
- **Author**: Dr. Althea Green
- **Diegetic Setting**: Glass Greenhouse Dome 3.
- **Verbatim Text**:
> "The frost has cracked the outer safety glazing. The mutant winter squash has frozen black.
> But in my pocket I have preserved eleven viable seeds of pre-war heirloom sweet corn.
> Whoever finds this jar: do not eat them. Plant them in deep alluvial soil when the willow trees first put out green buds. Let the children taste sweet grain again."
- **Gameplay Impact**:
  Unlocks the rare crop `item_heirloom_sweet_corn`, providing +25 Morale when served in the shelter mess hall.

### 26.17 Transcript #017 — The Abandoned Mother's Letter
- **Document Registry**: `transcript_abandoned_mothers_letter`
- **Author**: Mary Vance
- **Recipient**: My Little Rose
- **Diegetic Setting**: Rusted bus shelter along Highway 9.
- **Verbatim Text**:
> "My darling Rose, the raiders took the bus. They left me behind because my leg was crushed in the ditch.
> I have crawled into this pipe where the wind cannot reach me.
> Remember that your mother loved you more than the stars. Sing the lullaby about the swallow when you are afraid in the dark."
- **Gameplay Impact**:
  Deeply poignant narrative discovery that triggers emotional bonding conversations between shelter survivors.

### 26.18 Transcript #018 — The Hunter's Secret Trapping Journal
- **Document Registry**: `transcript_hunters_secret_trapping_journal`
- **Author**: Old Man Silas
- **Diegetic Setting**: Cave shelter overlooking the marsh.
- **Verbatim Text**:
> "The radiation boars only travel downwind between dusk and midnight.
> If you bait the deadfall trap with rotten turnip greens and ash berries, they will stick their snouts under the cedar log.
> Avoid their tusks even when they appear dead; the poison in their glands remains active for three days after the heart stops."
- **Gameplay Impact**:
  Increases meat and hide yields from shelter trapping expeditions by 35%.

### 26.19 Transcript #019 — The Deserter's Manifesto
- **Document Registry**: `transcript_deserters_manifesto`
- **Author**: The Iron Brotherhood Deserters
- **Diegetic Setting**: Nailed to the wooden gate of the abandoned sawmill.
- **Verbatim Text**:
> "We refuse to bleed for the Provost's coal mines or the Vanguard's artillery lines.
> We are the Free Walkers of the Pine Crags. We shoot no man who does not raise a rifle against our camp.
> Bring us salt, needles, and medicine. We will trade dried venison, timber, and honest peace."
- **Gameplay Impact**:
  Opens an exclusive trade faction for high-grade pine timber and cured meats.

### 26.20 Transcript #020 — Epilogue Record: The Reclamation Memorial
- **Document Registry**: `transcript_epilogue_reclamation_memorial`
- **Author**: Scribe of the United Freehold
- **Diegetic Setting**: Carved into the granite face of the shelter entrance on Day 600.
- **Verbatim Text**:
> "Here stood Shelter 14 during the Long Ash Winter.
> Within these concrete walls, ninety-two souls resisted famine, frostbite, contagion, and the drums of war.
> We did not conquer the wasteland; we outlived its cruelty. We kept the seeds safe, we deciphered the dead's letters, and we kept the iron stove warm until the green grass returned."
- **Gameplay Impact**:
  The definitive campaign victory inscription reflecting all moral, economic, and diplomatic choices made throughout the 600-day simulation.
"""

    with open(filepath, "w", encoding="utf-8") as f:
        f.write(content + "\n" + extra_content)

    with open(filepath, "r", encoding="utf-8") as f:
        final_len = len(f.read())
    print(f"Plan 06 expansion finished! Final length: {final_len} characters")

if __name__ == "__main__":
    main()
