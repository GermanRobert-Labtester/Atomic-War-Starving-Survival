# Round 6 — waystations.json: 8 new waystations, one per faction given a dossier in round 5.
#
# CONSUMER: Assets/Ashfall.Core/Waystation/WaystationCatalogLoader.cs -> WaystationNetworkSystem
# (condition/filter decay, 7-day resupply, RepairFilter, AssignWatch) and src/UI/WaystationNetworkPanel.cs
# which renders "{name} ({region})" to the player.
#
# REFERENCE INTEGRITY (verified before authoring):
#   * node_id -> all 8 resolve in locations.json (169 ids); none is already used by the 6 existing
#     waystations. (C# fallback defaults in the loader cover a DIFFERENT site set; no collision.)
#   * stock_item_ids -> every id resolves in items.json (660). lamp_oil / brass_fittings /
#     sewing_kit / welders_glass were rejected: they appear in currents.json wants but are NOT items.
#   * services -> restricted to the existing 9-token vocabulary (trade, staging, rest,
#     filter_recharge, blessing, repair, intelligence, saline_wash, grain_exchange).
#   * region -> settlement-accurate, following the file's own precedent of router-unknown regions
#     (existing entries already use high_scarp / dead_suburbs, which RegionalSupplyRouter does not
#     special-case; TagsForOrigin always includes "general", so unknown regions degrade safely).
#
# NUMERIC ENVELOPE (existing 6 entries): condition 70-90, filter_health 75-95, defense_rating 3-5.
# Values are grounded in each node's locations.json dangerLevel / baseRadsPerHour: St Brigid's
# (danger 7, 40 rads/hr) and the Switchbacks (danger 5, 52 rads/hr, 5 h travel) get the worst
# condition; South Beacon (danger 2, 9 rads/hr) the best.
#
# KEEPER NAMES: canon figures used as written where they exist — Mira Vos (the_cutters), Edor Vale
# (the_office), Kaspar Drej (the_long_walk). Others follow the file's title+surname convention
# (Warden Kessel, Deacon Vane, Foreman Taggart, Mistress Corvo, Diver Renn, Weigher Orlov).
# local_problem: 161-210 chars in the existing entries; new ones held to that band, each an
# unresolved operational trouble specific to its faction's canon.

NEW_WAYSTATIONS = [
{
  "id": "waystation_south_beacon",
  "name": "South Beacon Lamp Post",
  "node_id": "loc_south_beacon_tower",
  "region": "deep_coast",
  "keeper_name": "Lamptender Orris Vail",
  "specialty": "Lamp Oil & Reflector Glass",
  "condition": 88.0,
  "filter_health": 92.0,
  "defense_rating": 3,
  "services": ["trade", "staging", "rest", "filter_recharge"],
  "stock_item_ids": ["fuel", "clean_water", "battery_pack"],
  "local_problem": "The seaward reflector is pitted, so the beam scatters instead of carrying. Vail has ground two replacements from salvage and neither holds a polish. Until one does, the lamp lights the yard and not the road."
},
{
  "id": "waystation_tinkers_notch",
  "name": "Tinker's Notch Swap Post",
  "node_id": "loc_settlement_tinkers_notch",
  "region": "dead_suburbs",
  "keeper_name": "Registrar Odie Vant",
  "specialty": "Claim Registry & Machine Tools",
  "condition": 85.0,
  "filter_health": 88.0,
  "defense_rating": 4,
  "services": ["trade", "staging", "repair"],
  "stock_item_ids": ["scrap_metal", "scrap_mechanical", "electronic_scrap"],
  "local_problem": "A claim was struck at the drop shaft and two crews are working it on the strength of two different marks. Vant has read both doorframes aloud twice. The Guild registrar is eight days out and neither crew will wait."
},
{
  "id": "waystation_brine_pans",
  "name": "Brine-Pan Hollow Waystation",
  "node_id": "loc_settlement_brine_pans",
  "region": "the_toll",
  "keeper_name": "Assayer Mira Vos",
  "specialty": "Salt Cure & Brine Assay",
  "condition": 80.0,
  "filter_health": 84.0,
  "defense_rating": 4,
  "services": ["trade", "staging", "saline_wash"],
  "stock_item_ids": ["clean_water", "dried_rations", "scrap_metal"],
  "local_problem": "The east pan's assay came back low twice running and Vos will not sell a sack until she knows why. Either the brine is thinning or somebody is cutting the cure, and she has not yet decided which of those is worse."
},
{
  "id": "waystation_slate_hollow",
  "name": "Slate Hollow Enclave Post",
  "node_id": "loc_settlement_slate_hollow",
  "region": "high_scarp",
  "keeper_name": "Counter Ilse Marr",
  "specialty": "Low-Background Assay & Shielding Lead",
  "condition": 78.0,
  "filter_health": 95.0,
  "defense_rating": 3,
  "services": ["trade", "staging", "rest", "intelligence"],
  "stock_item_ids": ["medical_kit", "clean_water", "battery_pack"],
  "local_problem": "The lab is three sheets of shielding lead short and the quarry roof will not wait on a delivery. Marr offers assay work to anyone who can carry plate up the scree, and has had two takers and one arrival."
},
{
  "id": "waystation_iron_siding",
  "name": "Iron Siding Redoubt Post",
  "node_id": "loc_settlement_iron_siding",
  "region": "industrial_belt",
  "keeper_name": "Sergeant-Marshal Ada Kesk",
  "specialty": "Weapon Trueing & Armour Plate",
  "condition": 75.0,
  "filter_health": 80.0,
  "defense_rating": 5,
  "services": ["trade", "staging", "repair"],
  "stock_item_ids": ["ammo_308", "scrap_metal", "medical_kit"],
  "local_problem": "A Garrison patrol walks the yard perimeter at dusk now, unhurried, and the Coalition cannot be seen to answer to it. Kesk keeps the shutters and the schedule, and the schedule has an hour in it nobody sleeps."
},
{
  "id": "waystation_weighbridge",
  "name": "The Weighbridge Freight Post",
  "node_id": "loc_weighbridge",
  "region": "industrial_belt",
  "keeper_name": "Clerk Edor Vale",
  "specialty": "Freight Receipts & Hardware",
  "condition": 82.0,
  "filter_health": 78.0,
  "defense_rating": 5,
  "services": ["trade", "staging", "intelligence"],
  "stock_item_ids": ["scrap_metal", "canned_food", "electronic_scrap"],
  "local_problem": "Two consignments arrived on the same receipt number and one of them is not on the manifest. Vale has filed both and weighed neither, which is correct procedure and also why the queue stands out into the yard."
},
{
  "id": "waystation_pilgrim_switchbacks",
  "name": "Pilgrim Switchbacks Waystation",
  "node_id": "loc_pilgrim_switchbacks",
  "region": "high_scarp",
  "keeper_name": "Walker Kaspar Drej",
  "specialty": "Trail News & Footwear Repair",
  "condition": 70.0,
  "filter_health": 75.0,
  "defense_rating": 3,
  "services": ["rest", "staging", "trade"],
  "stock_item_ids": ["clean_water", "dried_rations", "thermal_blanket"],
  "local_problem": "The upper landing has washed out twice this season and the Walk will not stop for it, so the crossing is a rope and a decision. Drej has posted odds on the rope and, as always, does not intend to collect."
},
{
  "id": "waystation_st_brigids",
  "name": "St Brigid's Almshouse Post",
  "node_id": "loc_st_brigids_almshouse",
  "region": "the_cluster",
  "keeper_name": "Runner Tev Alderney",
  "specialty": "Hospice Berth & Spring Water",
  "condition": 72.0,
  "filter_health": 76.0,
  "defense_rating": 3,
  "services": ["rest", "staging", "blessing"],
  "stock_item_ids": ["medical_kit", "clean_water", "bandage"],
  "local_problem": "The spring still runs clean, which is the only reason anyone walks out here, and the almshouse has four beds against eleven names on the tag board. Alderney is not turning anyone away and has run out of floor."
}
]
