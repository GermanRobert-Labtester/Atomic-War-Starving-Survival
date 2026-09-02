import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/dive_sites.json"
with open(file_path, "r") as f:
    data = json.load(f)

replacements = {
    "Charted from the Cape Beacon commune's logbook; the mail-steamer's stern still breaks at low water.": "Scrawled in the margins of the Cape Beacon logbook. At low tide, the steamer's rusted stern breaks the oily surface like a drowning man's hand.",
    "Visible from the ice-road ferry crossing on any clear day; the ramp still points at the water.": "Visible from the ice-road on clear days, the rusted ferry ramp points down into the black, freezing depths—an invitation to the drowned.",
    "The Flotilla's own mooring hulks; the quartermaster trades the approach for honest cargo.": "A graveyard of tethered hulks groaning against their chains in the deep current. The quartermaster trades the safe approach coordinates for untainted cargo.",
    "The deep-coast survey marks it hull-down off the icebreaker dock, port side to the shelf.": "The deep-coast survey marks her hull-down in the freezing silt, her port side leaning heavily into the abyssal drop-off of the continental shelf.",
    "Lotte Verrill's wreck notes name the Barrik's berth — for a sealed lamp and no questions.": "Lotte Verrill's bloodstained notes mark the sub's final resting place in the crush-depths. She traded the coordinates for a sealed lamp and absolute silence.",
    "The Overflow grid maps mark the concourse as standing water; the Flotilla marked it 'quiet water, watch the gravel'.": "The old grid maps list this concourse as standing water. The Flotilla's scrawled addendum warns of 'quiet water' and crushed bone mistaken for gravel.",
    "The convoy never made the turn at Aurora Borealis Anchorage Shoal; three trucks stand on the shelf edge.": "Three military transports that failed to make the crossing. Now they idle in the silt at the edge of a deepwater trench, their cargo sealed behind pressure-warped doors.",
    "The Hydro-Barons' intake charts show a lower dock the schedules never mention.": "Smuggled intake charts reveal a subterranean loading dock the Hydro-Barons intentionally flooded. Whatever they drowned down there, they didn't want it found.",
    "The High Bluff foghorn's compressed-air log lists the relay run that stopped answering.": "The foghorn station's logbooks record the exact hour this relay stopped transmitting. Now, only the rhythmic ping of straining metal echoes in the crushing dark.",
    "The quarantine barge shows on the commune's oldest charts, still moored where the surge left it.": "Moored where the radioactive surge left it decades ago. The sealed quarantine bulkheads still hold, keeping the dark water out, and something else trapped within.",
    "The picket's mast still shows at slack water off the roadstead crane; the Flotilla dives it only for the crew's effects.": "The patrol boat's jagged mast breaks the surface during slack tide. Flotilla scavengers refuse to open the lower decks, muttering about scratch-marks on the inside of the bulkheads.",
    "The siphon station's intake grille shows at low water; the barons never paid to have it looked at.": "The massive intake grille breaches the surface at low tide, choked with strange, pale kelp. The Hydro-Barons sealed it off after the pumps began drawing up unidentifiable blood.",
    "Uma Tarran sells the strongroom bearing for two claim tags and no questions.": "Uma Tarran traded the strongroom's coordinates for two claim tags and a promise. She warned that the pressure doors were locked from the inside.",
    "The commune's cistern charts stop one gallery short; Lotte Verrill will say why for a sealed lamp.": "The commune's cistern charts abruptly end one gallery short. Lotte Verrill will explain why, but only if you bring her a sealed lamp and lock the door."
}

for site in data.get("dive_sites", []):
    if "discovery" in site and site["discovery"] in replacements:
        site["discovery"] = replacements[site["discovery"]]

with open(file_path, "w") as f:
    json.dump(data, f, indent=2)

print("Patch applied to dive_sites.json")
