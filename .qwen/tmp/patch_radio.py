# Round 4 — NEW radio broadcasts for radio.json (base feed, loaded by RadioBroadcastCatalog.LoadBaseRadioJson).
#
# Station resolution is frequency-driven in the loader:
#   88.4 -> station_garrison_overlord | 104.2 -> station_vitrified_crater | 91.3 -> station_open_classroom
#   142.85/other -> station_civil_defense (Automated Relay persona lives at 142.85 in radio_stations.json)
# Genre is intelType-driven: Military->MilitaryEdict, Emergency->EmergencyAlert, NumbersStation->NumbersStation,
#   Survivor->SurvivorTestimony, else CivilianNews.
#
# GAP BEING FILLED (verified 2026-09-07):
#   * 104.2 "Voice of the Vitrified Crater" — persona = deep liturgical chanting in sealed chambers.
#     radio.json had ONE entry there (agricultural insect notice); faction_radio_corpus has patrol/weather
#     traffic. The liturgical voice itself had ZERO authored content anywhere.
#   * 91.3 "The Open Airwaves (Classroom & Lineman)" — persona = warm patient voices, chalk taps.
#     radio.json had ZERO entries; corpus has supply/distress/intercepts, no actual lessons.
#   * 142.85 "Automated Emergency Beacon Array" — radio.json had ZERO entries.
#   * Numbers-station material at 99.0 (reachable) tying to quest_numbers_in_the_dark /
#     quest_the_voice_answers (ninth-night schedule, groups of five, rail-cut bearing).
#
# NOT authored, deliberately: station_numbers_sigint at 14.487 MHz. RadioReceiverPlan bands start at
# 50.0 MHz (VHF-Low 50-150), so the dial clamps and 14.487 is UNREACHABLE by the player. Authoring
# content there would be dead data. Reported as a finding instead.

NEW_RADIO = [
# ── Voice of the Vitrified Crater — 104.2 MHz — liturgical ─────────────────────
{
  "id": "radio_broadcast_crater_sermon_glass_mirror",
  "frequency": 104.2,
  "minDay": 15,
  "maxDay": 120,
  "intelType": "Civilian",
  "confidence": 0.45,
  "message": "Voice of the Vitrified Crater, 104.2. [A deep chamber; the words arrive slower than speech, as if carried up a stair.] The heat went through the valley in one second and left the floor of it as glass. Consider what that means. A mirror the size of a district. Everything that stands at the rim can see itself in the place where the town was. We do not say the glass is holy. We say the glass is honest, and that it took a fire to make a surface that would hold this valley's face."
},
{
  "id": "radio_broadcast_crater_liturgy_of_counts",
  "frequency": 104.2,
  "minDay": 30,
  "maxDay": 200,
  "intelType": "Civilian",
  "confidence": 0.4,
  "message": "The liturgy of counts, 104.2. [Voices in unison, uneven, some out of breath.] We read the numbers aloud at the sixth hour. Four point one at the north rim. Six point eight in the culvert. Two point two where the children are permitted to walk. We read them because a number spoken in a room is a number somebody heard, and a number nobody heard is only weather. Read your own counts aloud tonight, whoever you are. Say the number and then say the day. That is the whole prayer."
},
{
  "id": "radio_broadcast_crater_sealed_chamber_names",
  "frequency": 104.2,
  "minDay": 60,
  "maxDay": 260,
  "intelType": "Survivor",
  "confidence": 0.5,
  "message": "From the sealed chamber beneath the rim, 104.2. [The recording is older than the voice introducing it; tape hiss under a chant.] What follows was cut eleven winters ago in a room nobody has opened since. Twenty-six names, sung in the manner of the lower hall. If your name is among them, the chamber is wrong and you are welcome at the rim. If your name is not among them, you are the reason we still transmit it. We will not edit the tape. The tape is the record."
},
{
  "id": "radio_broadcast_crater_sermon_second_sun",
  "frequency": 104.2,
  "minDay": 45,
  "maxDay": 240,
  "intelType": "Civilian",
  "confidence": 0.45,
  "message": "Sermon of the second sun, 104.2. [One voice, close to the microphone, patient.] Nine of us in this hall saw it. It was not beautiful and we will not tell you it was beautiful. It was a light that made shadows out of things that had never had them — the fence, the wheel, the hand. We have preached the second sun for eleven years and the newest among us still ask what we are worshipping. Answer: nothing. We are keeping the hour. Somebody has to keep the hour, and the ones who kept it before us are the shadows on the fence."
},
{
  "id": "radio_broadcast_crater_rim_invitation",
  "frequency": 104.2,
  "minDay": 90,
  "maxDay": 300,
  "intelType": "Civilian",
  "confidence": 0.55,
  "message": "Notice from the rim, 104.2. [Wind across the mouth of the transmitter; a bell struck twice, not brass, something thicker.] If you walk to the crater, bring a name and leave it at the marker stones. Do not bring food; we have food. Do not bring fuel; we will refuse it. Bring a name on paper or in your mouth and put it down where the others are. We do not ask what you believe and we will not ask again on the way out. The stones are on the north side, below the wire, where the count is lowest. Come in daylight. We are not what the roads say."
},
{
  "id": "radio_broadcast_crater_roll_call_of_dead",
  "frequency": 104.2,
  "minDay": 150,
  "maxDay": 340,
  "intelType": "Survivor",
  "confidence": 0.5,
  "message": "Roll call, 104.2, read by the keeper of the stair. [Each name is followed by a pause long enough for a breath, and the pauses are the point.] Alderman. Present in the glass. Brigg. Present in the glass. Corra, who sang the lower hall. Present. Dov. Present. Esme, aged nine. Present in the glass. [Thirty-one more follow.] That is the hall as it stood. If you hear a name you carried, you are not the only one carrying it, and the rim has a stone with room on it. We read this list every ninth day. We have never shortened it."
},
# ── The Open Airwaves: Classroom & Lineman — 91.3 MHz ─────────────────────────
{
  "id": "radio_broadcast_classroom_reading_lesson",
  "frequency": 91.3,
  "minDay": 10,
  "maxDay": 180,
  "intelType": "Civilian",
  "confidence": 0.7,
  "message": "The Open Airwaves, 91.3. Morning lesson. [Chalk tapping a slate, twice, to start.] If there is a child near this speaker, put them where they can hear and sit down beside them, because they will ask you the sounds and you should know them too. Today: the letter that opens the mouth wide, and the letter that closes it. Say them with me. [A pause, and in the pause you can hear a room of perhaps four people, and a wind in the antenna mast outside.] Good. That is reading. Reading is not the paper. Reading is the sound somebody agreed to."
},
{
  "id": "radio_broadcast_classroom_ration_arithmetic",
  "frequency": 91.3,
  "minDay": 25,
  "maxDay": 220,
  "intelType": "Civilian",
  "confidence": 0.7,
  "message": "Arithmetic for shelters, 91.3. [Pencil on paper, unhurried.] Today is division and it is not an abstraction. Eleven people, forty tins, six days. How much does each person eat each day, and what is left over? Do it before I say it — the answer matters less than the doing, because whoever in your shelter cannot do this sum is going to be told what the sum decided. [A long pause. Somebody in the room gets it wrong and is corrected kindly, on air, without embarrassment.] Good. Now do it with your real numbers. Write the remainder down. The remainder is where the arguments live."
},
{
  "id": "radio_broadcast_classroom_lineman_splice",
  "frequency": 91.3,
  "minDay": 40,
  "maxDay": 260,
  "intelType": "Civilian",
  "confidence": 0.75,
  "message": "Lineman's hour, 91.3. [Wind, and the rattle of tools set down on a metal tray.] You are listening to a mast in weather and this is the sound of the job. Today: the splice that survives a winter. Strip back two fingers, not more. Twist with the grain of the strand, never against it, because against it the wire work-hardens and it will fail in the cold, and it will fail at the worst hour. Then bind it and seal it and pull it hard once. If it holds the pull it holds the season. If you are hearing this on a repaired set, somebody did this for you. Say their name out loud, they will not mind."
},
{
  "id": "radio_broadcast_classroom_water_lesson",
  "frequency": 91.3,
  "minDay": 55,
  "maxDay": 280,
  "intelType": "Civilian",
  "confidence": 0.7,
  "message": "Lesson for the well-tenders, 91.3. [Chalk, then the squeak of a slate being wiped.] If your water comes green, do not immediately fear it and do not immediately drink it. Green at the mouth of a well after storms is usually mineral — a bloom in the draw, ugly and often harmless. Green with a film that breaks when you stir it is different. Boiling does not remove what makes water green; boiling kills what lives, and the mineral stays. So: stir it, watch the film, smell the cup, and write down the day. Three days of notes is a diagnosis. One frightened evening is only a rumor."
},
{
  "id": "radio_broadcast_classroom_valley_geography",
  "frequency": 91.3,
  "minDay": 70,
  "maxDay": 300,
  "intelType": "Civilian",
  "confidence": 0.65,
  "message": "Geography of home, 91.3. [A map being unfolded on a table; the paper is stiff.] Today the valley as it was and as it is, because you cannot navigate the second without the first. The rail cut ran north from the siding. The water tower stood where the count is highest now — that is not a coincidence, that is weather and low ground. The quarry turn is where the road crust fails in wet seasons. Learn three features you can see from your own door and name them aloud to whoever shares it. A place that is named is a place you can give directions in, and directions are the beginning of neighbors."
},
{
  "id": "radio_broadcast_classroom_sign_off_names",
  "frequency": 91.3,
  "minDay": 120,
  "maxDay": 340,
  "intelType": "Survivor",
  "confidence": 0.6,
  "message": "End of the morning block, 91.3. [The chalk is put down. The teacher does not hurry.] Before I close, the register, because the register is the part of teaching that outlasts the lesson. Present this week: four. Absent: two. One absent since the cold, one absent since the road closed. [A breath.] I am not removing them. A register with the names struck out is a register that has decided something it has no authority to decide. If either of you is hearing this, the lesson is on the slate and I will read it again tomorrow at the same hour. That is the whole of the sign-off. Come in from the cold if you can."
},
# ── Automated Emergency Beacon Array — 142.85 MHz ─────────────────────────────
{
  "id": "radio_broadcast_relay_teletype_ash_front",
  "frequency": 142.85,
  "minDay": 20,
  "maxDay": 300,
  "intelType": "Emergency",
  "confidence": 0.95,
  "message": "AUTOMATED EMERGENCY BEACON ARRAY. 142.85. TELETYPE FOLLOWS. [A synthesizer with no inflection, characters arriving at a fixed rate.] ASH FRONT BEARING TWO NINE ZERO. VELOCITY ELEVEN KILOMETRES PER HOUR. PARTICULATE LOAD RISING. VISIBILITY FORECAST BELOW ONE KILOMETRE AT SIX HOURS. SHELTER INTAKES. SECURE LOOSE MATERIAL. THIS MESSAGE REPEATS UNTIL SUPERSEDED. NO OPERATOR IS PRESENT. NO OPERATOR IS REQUIRED."
},
{
  "id": "radio_broadcast_relay_battery_countdown",
  "frequency": 142.85,
  "minDay": 80,
  "maxDay": 365,
  "intelType": "Emergency",
  "confidence": 1.0,
  "message": "AUTOMATED EMERGENCY BEACON ARRAY. 142.85. STATUS. [Two navigational pings, then the teletype.] BANK STATE: TWENTY-TWO PERCENT. TRANSMIT POWER REDUCED TO MAINTAIN SCHEDULE. PROJECTED SERVICE: ONE HUNDRED AND FORTY DAYS AT CURRENT DUTY. THIS ARRAY WILL NOT ANNOUNCE ITS FINAL TRANSMISSION. THE FINAL TRANSMISSION WILL SIMPLY BE THE LAST ONE. IF A PARTY IS ABLE TO SERVICE THIS SITE, THE ARRAY IS AT THE NORTH MAST AND THE ACCESS HATCH TAKES A STANDARD KEY. THE ARRAY DOES NOT REQUEST SERVICE. THE ARRAY REPORTS."
},
{
  "id": "radio_broadcast_relay_continuity_roll",
  "frequency": 142.85,
  "minDay": 200,
  "maxDay": 365,
  "intelType": "MilitaryDeadHand",
  "confidence": 0.9,
  "message": "CONTINUITY CHANNEL. AUTOMATED ARRAY 142.85. [Teletype, slower than the weather traffic, as though the schedule itself is reluctant.] STATION POLL FOLLOWS. CIVIL DEFENSE CARRIER: ABSENT SINCE DAY UNKNOWN. GARRISON TACTICAL: PRESENT, INTERMITTENT. OPEN AIRWAYS: PRESENT, HUMAN OPERATOR DETECTED. CRATER RIM TRANSMITTER: PRESENT, UNREGISTERED FORMAT. NUMBERS ARRAY, SHORTWAVE: NOT POLLABLE FROM THIS BAND. [A pause of exactly four seconds.] FOUR OF SIX ANSWER. THE POLL REPEATS EVERY SIX HOURS. THE ARRAY HAS NO AUTHORITY TO CONCLUDE ANYTHING. THE ARRAY CONTINUES."
},
# ── Numbers station, ninth-night schedule — 99.0 MHz (reachable) ──────────────
{
  "id": "radio_broadcast_numbers_ninth_night_groups",
  "frequency": 99.0,
  "minDay": 110,
  "maxDay": 330,
  "intelType": "NumbersStation",
  "confidence": 0.35,
  "message": "[A flat voice, no accent, no breath where breath should be. A music box plays three notes before the first group and three after the last.] SEVEN. TWO. NINE. FOUR. ONE. [Pause.] THREE. THREE. EIGHT. ZERO. SIX. [Pause.] SEVEN. TWO. NINE. FOUR. ONE. [The groups repeat for nine minutes without deviation. Then, once, breaking the pattern:] RAIL. CUT. [Two notes. Carrier drops.]"
},
{
  "id": "radio_broadcast_numbers_interval_notice",
  "frequency": 99.0,
  "minDay": 130,
  "maxDay": 340,
  "intelType": "NumbersStation",
  "confidence": 0.3,
  "message": "[The same flat voice. This transmission is shorter than the others and does not contain groups.] SCHEDULE NOTICE. THIS STATION TRANSMITS ON THE NINTH NIGHT. TEN MINUTES AFTER THE HOUR. THE SCHEDULE HAS BEEN KEPT FOR EIGHT HUNDRED AND SIX TRANSMISSIONS. THE SCHEDULE WILL BE KEPT. IF YOU HAVE ANSWERED THIS STATION, YOU HAVE CHANGED THE SCHEDULE, AND THE SCHEDULE HAS CHANGED FOR YOU. [Three notes. A pause long enough that the receiver's squelch almost opens. Carrier drops.]"
},
{
  "id": "radio_broadcast_numbers_municipal_codes",
  "frequency": 99.0,
  "minDay": 160,
  "maxDay": 350,
  "intelType": "NumbersStation",
  "confidence": 0.4,
  "message": "[Music box, three notes. The flat voice reads at a pace meant for a hand writing it down.] FOUR ONE NINE. WATER, MUNICIPAL RESERVE, SEALED. [Pause.] TWO TWO SEVEN. WATER, MUNICIPAL RESERVE, SEALED. [Pause.] EIGHT ZERO THREE. DO NOT. [The word is repeated three times, in the same tone as the numbers, with no change of any kind.] EIGHT ZERO THREE. DO NOT. EIGHT ZERO THREE. DO NOT. [Pause.] THREE ONE FIVE. CISTERNS, SIX ACCESS, FIVE SEAL, ONE MARKED. [Three notes. Carrier drops.]"
}
]
