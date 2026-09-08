# Round 4 — trim pass on the new radio broadcasts.
# Rationale: existing radio.json messages run 132-395 chars; the new batch landed at 376-611.
# Radio is heard, not read — trim to <= ~450 while keeping the voice and the best lines.

TRIM = {
"radio_broadcast_classroom_sign_off_names":
"End of the morning block, 91.3. [The chalk is put down.] Before I close, the register — the part of teaching that outlasts the lesson. Present: four. Absent: two. One since the cold, one since the road closed. [A breath.] I am not removing them. A register with names struck out has decided something it has no authority to decide. If either of you is hearing this, the lesson is on the slate, and I will read it again at the same hour tomorrow. Come in from the cold if you can.",

"radio_broadcast_classroom_valley_geography":
"Geography of home, 91.3. [A map unfolded on a table; the paper is stiff.] The valley as it was and as it is, because you cannot navigate the second without the first. The rail cut ran north from the siding. The water tower stood where the count is highest now — not a coincidence: weather and low ground. The quarry turn is where the road crust fails in wet seasons. Name three features you can see from your own door, aloud, to whoever shares it. A named place is a place you can give directions in, and directions are the beginning of neighbors.",

"radio_broadcast_classroom_lineman_splice":
"Lineman's hour, 91.3. [Wind; tools set down on a metal tray.] You are listening to a mast in weather and this is the sound of the job. Today: the splice that survives a winter. Strip back two fingers, not more. Twist with the grain of the strand, never against — against it the wire work-hardens and fails in the cold, at the worst hour. Bind it, seal it, pull it hard once. If it holds the pull it holds the season. If you are hearing this on a repaired set, somebody did this for you. Say their name out loud. They will not mind.",

"radio_broadcast_classroom_ration_arithmetic":
"Arithmetic for shelters, 91.3. [Pencil on paper, unhurried.] Today is division, and it is not an abstraction. Eleven people, forty tins, six days. How much does each eat each day, and what is left over? Do it before I say it — the doing matters more than the answer, because whoever in your shelter cannot do this sum will be told what the sum decided. [A pause. Somebody in the room gets it wrong and is corrected kindly, on air.] Good. Now do it with your real numbers, and write the remainder down. The remainder is where the arguments live.",

"radio_broadcast_classroom_water_lesson":
"Lesson for the well-tenders, 91.3. [Chalk, then a slate wiped.] If your water comes green, do not immediately fear it and do not immediately drink it. Green at the mouth of a well after storms is usually mineral — a bloom in the draw, ugly and often harmless. Green with a film that breaks when you stir it is different. Boiling does not remove what makes water green; boiling kills what lives, and the mineral stays. So: stir it, watch the film, smell the cup, write down the day. Three days of notes is a diagnosis. One frightened evening is only a rumor.",

"radio_broadcast_relay_continuity_roll":
"CONTINUITY CHANNEL. AUTOMATED ARRAY 142.85. [Teletype, slower than the weather traffic.] STATION POLL FOLLOWS. CIVIL DEFENSE CARRIER: ABSENT. GARRISON TACTICAL: PRESENT, INTERMITTENT. OPEN AIRWAYS: PRESENT, HUMAN OPERATOR DETECTED. CRATER RIM TRANSMITTER: PRESENT, UNREGISTERED FORMAT. NUMBERS ARRAY, SHORTWAVE: NOT POLLABLE FROM THIS BAND. [A pause of exactly four seconds.] FOUR OF SIX ANSWER. THE POLL REPEATS EVERY SIX HOURS. THE ARRAY HAS NO AUTHORITY TO CONCLUDE ANYTHING. THE ARRAY CONTINUES.",

"radio_broadcast_crater_rim_invitation":
"Notice from the rim, 104.2. [Wind across the transmitter mouth; a bell struck twice — not brass, something thicker.] If you walk to the crater, bring a name and leave it at the marker stones. Do not bring food; we have food. Do not bring fuel; we will refuse it. Bring a name on paper or in your mouth and put it down where the others are. We will not ask what you believe, and we will not ask again on the way out. The stones are on the north side, below the wire, where the count is lowest. Come in daylight. We are not what the roads say.",

"radio_broadcast_classroom_reading_lesson":
"The Open Airwaves, 91.3. Morning lesson. [Chalk taps a slate, twice, to start.] If there is a child near this speaker, put them where they can hear and sit beside them, because they will ask you the sounds and you should know them too. Today: the letter that opens the mouth wide, and the letter that closes it. Say them with me. [A pause — in it you can hear a room of perhaps four people, and wind in the antenna mast outside.] Good. That is reading. Reading is not the paper. Reading is the sound somebody agreed to.",

"radio_broadcast_crater_sermon_second_sun":
"Sermon of the second sun, 104.2. [One voice, close to the microphone, patient.] Nine of us in this hall saw it. It was not beautiful and we will not tell you it was beautiful. It was a light that made shadows out of things that had never had them — the fence, the wheel, the hand. We have preached the second sun for eleven years, and the newest among us still ask what we are worshipping. Answer: nothing. We are keeping the hour. Somebody has to keep the hour, and the ones who kept it before us are the shadows on the fence.",

"radio_broadcast_crater_roll_call_of_dead":
"Roll call, 104.2, read by the keeper of the stair. [Each name is followed by a pause long enough for one breath; the pauses are the point.] Alderman. Present in the glass. Brigg. Present in the glass. Corra, who sang the lower hall. Present. Dov. Present. Esme, aged nine. Present in the glass. [Thirty-one more follow.] That is the hall as it stood. If you hear a name you carried, you are not the only one carrying it, and the rim has a stone with room on it. We read this list every ninth day. We have never shortened it.",

"radio_broadcast_relay_battery_countdown":
"AUTOMATED EMERGENCY BEACON ARRAY. 142.85. STATUS. [Two navigational pings, then the teletype.] BANK STATE: TWENTY-TWO PERCENT. TRANSMIT POWER REDUCED TO MAINTAIN SCHEDULE. PROJECTED SERVICE: ONE HUNDRED FORTY DAYS AT CURRENT DUTY. THIS ARRAY WILL NOT ANNOUNCE ITS FINAL TRANSMISSION. THE FINAL TRANSMISSION WILL SIMPLY BE THE LAST ONE. IF A PARTY IS ABLE TO SERVICE THIS SITE, THE ARRAY IS AT THE NORTH MAST AND THE HATCH TAKES A STANDARD KEY. THE ARRAY DOES NOT REQUEST SERVICE. THE ARRAY REPORTS.",

"radio_broadcast_numbers_municipal_codes":
"[Music box, three notes. The flat voice reads at a pace meant for a hand writing it down.] FOUR ONE NINE. WATER, MUNICIPAL RESERVE, SEALED. [Pause.] TWO TWO SEVEN. WATER, MUNICIPAL RESERVE, SEALED. [Pause.] EIGHT ZERO THREE. DO NOT. [The word repeats three times, in the same tone as the numbers, with no change of any kind.] EIGHT ZERO THREE. DO NOT. [Pause.] THREE ONE FIVE. CISTERNS, SIX ACCESS, FIVE SEALED, ONE MARKED. [Three notes. Carrier drops.]",

"radio_broadcast_crater_sermon_glass_mirror":
"Voice of the Vitrified Crater, 104.2. [A deep chamber; the words arrive slower than speech, as if carried up a stair.] The heat went through the valley in one second and left the floor of it as glass. Consider that. A mirror the size of a district. Everything standing at the rim can see itself in the place where the town was. We do not say the glass is holy. We say the glass is honest, and that it took a fire to make a surface that would hold this valley's face.",

"radio_broadcast_crater_liturgy_of_counts":
"The liturgy of counts, 104.2. [Voices in unison, uneven, some out of breath.] We read the numbers aloud at the sixth hour. Four point one at the north rim. Six point eight in the culvert. Two point two where the children are permitted to walk. We read them because a number spoken in a room is a number somebody heard, and a number nobody heard is only weather. Read your own counts aloud tonight, whoever you are. Say the number, then say the day. That is the whole prayer.",

"radio_broadcast_numbers_interval_notice":
"[The same flat voice. This transmission is shorter than the others and contains no groups.] SCHEDULE NOTICE. THIS STATION TRANSMITS ON THE NINTH NIGHT. TEN MINUTES AFTER THE HOUR. THE SCHEDULE HAS BEEN KEPT FOR EIGHT HUNDRED SIX TRANSMISSIONS. THE SCHEDULE WILL BE KEPT. IF YOU HAVE ANSWERED THIS STATION, YOU HAVE CHANGED THE SCHEDULE, AND THE SCHEDULE HAS CHANGED FOR YOU. [Three notes. A pause long enough that the squelch almost opens. Carrier drops.]",

"radio_broadcast_crater_sealed_chamber_names":
"From the sealed chamber beneath the rim, 104.2. [The recording is older than the voice introducing it; tape hiss under a chant.] What follows was cut eleven winters ago in a room nobody has opened since. Twenty-six names, sung in the manner of the lower hall. If your name is among them, the chamber is wrong and you are welcome at the rim. If your name is not among them, you are the reason we still transmit it. We will not edit the tape. The tape is the record.",
}
