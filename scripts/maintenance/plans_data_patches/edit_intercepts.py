import json
import os

filepath = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/radio_intercepts.json"
with open(filepath, "r") as f:
    data = json.load(f)

replacements = {
    "Convoy route Bravo compromised by ash drifts. Diverting remaining fuel tankers to bulk storage depot.": "Bravo route is choked with white-hot ash. We just lost two rigs to melt-throughs. Rerouting the surviving tankers to the deep bulk storage. If we don't make it, burn the manifest.",
    "MAYDAY. Primary air intake collapsed under frozen sludge. Seven dwellers trapped in sub-level workshop.": "MAYDAY. The intake fan just screamed and died—we're buried under thirty feet of frozen sludge. Seven of us coughing in the sub-level. Oxygen's thinning. (coughing) We can't clear the hatch...",
    "AUTOMATED TELEMETRY: Strategic reserve silo offline. Breached ordnance cache open for salvage verification.": "AUTOMATED TELEMETRY: Silo doors forced. Micro-fractures in primary concrete casing. Sub-level three ordnance bays unsealed. Radiation spikes detected in retrieval corridors.",
    "ATMOSPHERIC REPORT: Geomagnetic pulse incoming. Auroral propagation window opening over northern mountain passes.": "ATMOSPHERIC ALERT: Sky is boiling green. Severe geomagnetic cascade expected in two hours. Expect massive static across all bands. The northern passes are entirely blacked out.",
    "DEORBIT WARNING: Kinetic rod release signature detected in suborbital pass. Shelter sky-layer impact imminent.": "DEORBIT WARNING: Six kinetic rods separated from orbit-tether. Thermal bloom detected. Thirty seconds to upper-crust impact. All remaining personnel, brace for seismic shear.",
    "Spike mats deployed across the bridge approaches. Wait for the grain caravan to commit before firing.": "(Static) ...caltrops are laid in the blind spot. Let the lead hauler hit the bottleneck, wait till they pop the hatches to check the tires, then we take them. Leave the grain, grab the water.",
    "Barge three secured inside the flooded drydock. Salt fish crates and copper wiring ready for night barter.": "Barge Three is tied off in the shadows. The water's glowing again tonight, but the salt-fish and stripped copper are dry. Signal with two red flares if the exchange is still on.",
    "Need medicine urgently. We have children and spare generator parts at the roadside motel. Please send team.": "(Sobbing) Please, is anyone on this frequency? We're at the old motel... we have kids here, we have generator parts to trade, we just need iodine... please, they're starting to bleed...",
    "Deep core sample confirms radioactive fallout layer matches third nuclear strike phase. Data vault preserved.": "Core extraction successful. The ice strata shows the soot layer from the third exchange. It's... it's thicker than we modeled. The data vault is intact, but the surface... there's nothing left up there.",
    "Thermite charges set on eastern blast hatch. Assault team entering at dusk. Leave no engineer unchained.": "Thermite is burning through the hinges now. The bunker rats are screaming through the comms. We breach at dusk. Collar the engineers, put a bullet in the rest.",
    "Sump pumps overrun by subterranean flood surge. Generator fuel at 4%. Evacuating toward surface control room.": "The water's black and it smells like sulfur. It just swallowed the secondary pumps. We're running on fumes. Sealing the bulkhead and praying the blast door holds the pressure...",
    "Sealed seed elevator still intact behind concrete blast barrier. Willing to trade winter rye for clean filters.": "We cracked the secondary elevator. Half a ton of viable winter rye, sealed in vacuum glass. We're choking on the dust though. Will trade fifty kilos for pristine carbon filters. No raiders.",
    "TRANSPONDER: Solar array operating at 30% efficiency. Automated navigation repeater pinging every sixty seconds.": "AUTOMATED PING: Solar degraded by ash accumulation. Gyros stabilizing. If you receive this signal, the Compact endures. Maintain your stations.",
    "Sluice valve mechanism jammed with concrete debris. Water level upstream rising two inches per hour.": "The whole mechanism is screaming. Somebody packed the gears with rebar and concrete. The water is backing up fast, and the pressure seals are starting to whine. We need a welding team NOW!",
    "Avalanche severed external power line. Thermal heaters dying. Two frostbite casualties requiring evacuation.": "The line snapped. We're in the dark. The heaters died ten minutes ago and the frost is already creeping up the walls. (shivering) We can't feel our hands. Please...",
    "Sunday evening communal broadcast. Singing and reading names of shelters still responding to roll call.": "(Static crackles, giving way to a choir of raspy, exhausted voices singing an old hymn)... reading the roll. Station Four, silent. Station Seven, silent. Station Nine... (a long pause) Station Nine, we pray for your souls."
}

for intercept in data['intercepts']:
    msg = intercept['message']
    if msg in replacements:
        intercept['message'] = replacements[msg]

with open(filepath, "w") as f:
    json.dump(data, f, indent=2)
