# EXPANSION 67 — THE TWO NAMES AT LOW SLACK
## A Dead-Drop Command Shelter Story Plan
### Wave 12: The Quiet Threshold

**Primary map target:** Dead-Drop Command Shelter, Deep Coast Saline Shelf  
**Location ID:** loc_deaddrop_command_shelter  
**Related existing narrative chain:** Last Rotation  
**Document type:** prose-first game-content expansion proposal  
**Target:** at least 120,000 characters of authored game narrative  
**Story question:** When two surviving records name the same mapped place differently, what may a careful reader claim?  
**Status:** proposed content; no implementation authority or data change

---

## 1. Player-facing premise

A shortwave standby broadcast repeats a phrase about “last rotation.” An archive index cylinder completes the existing cipher chain and reveals a place already present in the world map: the Dead-Drop Command Shelter. The player follows a chart and a hand-annotated tidal-lock manual recovered from the Deep Coast Saline Shelf.

The two sheets appear to describe one location. The chart marks an artificial underwater trench excavated for intake lines. The manual gives bypass timing for Lock Gate Four. Yet the catalog calls the destination an automated contingency shelter, while the damaged-map record describes a submerged maritime relay station beyond tidal locks. Both records point to the same location ID. Neither explains the difference.

An old clerk’s copy, found with the papers, carries two headings on its cover: “reserve shelter” and “coast relay.” A pencil note beneath them says, “Do not make the copy choose.” It may be an instruction about how to file two documents, or a reminder from someone who never had authority to decide what the place was. The player is asked to preserve the map evidence and leave a useful route annotation for the next reader.

This is an exploration story about labels, custody, and what remains unresolved when a route has two institutional names. It is not a reveal that one catalog is secretly wrong. It does not turn the shelter into a sentient machine, activate a defense system, or claim that the tide chart is safe current guidance. It does not create a new access puzzle. The player reads the records, hears how different people used them, and chooses how to describe what the papers establish.

## 2. Short pitch

The coast is quiet when the signal arrives. The broadcast is thin and regular: a standby phrase, an interval, then the same phrase again. It does not call the player by name. It asks for an index, or seems to. One record calls the needed object an archive index cylinder; the quest synopsis calls it a microfilm index cylinder. The current chain definition binds the required item to the first name. The plan uses the neutral phrase “index cylinder” until a premise audit resolves that wording.

The decrypted destination is a map node with an old name and a new question. The first fragment is a Bathymetric Sounding Chart. It shows the line of an artificial trench and identifies old intake work. The second is a Tidal Lock Manual. Its annotations concern Lock Gate Four and a sluice bypass. No surviving note says that the two pages were drafted together. No bearing, scale, or page number proves that the shelter icon on one copy belongs to the service drawing on the other.

At the site, the player finds more paperwork than machinery: a sleeve for a duty roster, a carbon copy of a filing label, a chart with a blank legend, and one private note written for the next reader. The records describe procedures, not a complete history. Former map copyist Leth Varo remembers the relay name. Retired lock runner Sima Dorr remembers a shelter name spoken by an absent office. Neither was in a position to verify the other’s route.

The player returns with a paired-source annotation: “Deep Coast Saline Shelf papers point to this location. Shelter and relay labels both survive. Relationship between the descriptions has not been reconciled. Historical lock timing is not a present-day travel instruction.” The map is no less useful for being honest about its limits.

## 3. What this proposal contains

The authored package is intended to use existing narrative, radio, cipher, journal, expedition, map, and readable-record owners after a current schema and route audit. Proposed material includes:

- a restrained opening transmission attached to the existing Last Rotation chain;
- a short sequence for the player hearing the transmission and finding the required index item;
- an expedition request that frames the visit as record recovery and map clarification;
- arrival prose tied to the existing coast location and map evidence;
- several source documents whose voice, purpose, and confidence differ;
- optional conversations with three witnesses who know different parts of the route;
- a return scene in which the player writes a bounded map annotation;
- two archive outcomes that preserve the same facts with different levels of detail;
- journal copy that separates catalog wording, fragment wording, and inference;
- accessibility, tone, and continuity notes for a later content implementation.

No implementation in this plan changes the current location, map graph, route distance, danger value, radiation value, expedition loot, discoverability, cipher-state ownership, or save path.

## 4. Explicit non-goals

This is not a new underwater traversal feature or a gate-operating feature. It does not add:

- diving equipment, breath, pressure, currents, flooding, swimming, submersible use, or an underwater movement mode;
- a tide clock, low-slack timer, sluice minigame, timed door, pressure simulation, or route hazard;
- a new marine faction, maritime law system, port economy, salvage license, or water-rights ledger;
- a sentient shelter, rogue intelligence, enemy machine, personalized radio voice, or automated combat encounter;
- a new cipher engine, second Last Rotation chain, duplicate item, alternate map reveal owner, or parallel quest state;
- a new persistent “record ambiguity” variable or a save section for the story choice;
- a claim that current lock infrastructure works because a historical manual describes its timing;
- a claim that the player has authority to open, repair, flood, drain, or permanently enter any bulkhead;
- a mechanical reward for selecting a preferred interpretation;
- a replacement for the existing Dead Hand Core storyline or its UXO register;
- a new current route to the site or a claim that all map nodes are currently reachable.

The phrase “at low slack tide” appears in the existing damaged-map installation description. In this plan it is historical route language and a clue to the source’s intended setting. It is not a safe-window calculation or an invitation to invent tide behavior.

## 5. Evidence from current data and code

### 5.1 Location record

The location catalog has a row for loc_deaddrop_command_shelter. Its display name is “Dead-Drop Command Shelter.” Its description calls it an automated contingency shelter revealed through the Last Rotation dead-hand protocol and says it contains classified directives and high-grade technical relics. The row also contains danger, travel, and radiation values. Those values remain authoritative if the story is implemented.

### 5.2 Map-zone record

The damaged-map catalog has a Deep Coast Saline Shelf zone and assigns loc_deaddrop_command_shelter as its hidden installation. The installation description instead calls it a submerged maritime relay station situated past tidal locks, with watertight bulkhead entries accessible at low slack tide. The same zone provides two fragments:

- **Bathymetric Sounding Chart:** a nautical chart indicating an artificial underwater trench excavated before the war for intake lines.
- **Tidal Lock Manual:** a hand-annotated operational manual for Lock Gate Four, indicating sluice bypass timing.

This is a source-level descriptive discrepancy. The plan preserves it as evidence requiring editorial review. It does not repair the data, join the two descriptions into a definitive architecture, or create an explanation that the game’s current canon has not established.

### 5.3 Existing Last Rotation chain

CipherQuestChainEngine defines a chain with ID last_rotation, quest ID quest_cipher_last_rotation, broadcast radio_broadcast_last_rotation, cipher station cipher_station_last_rotation, required item item_archive_index_cylinder, and target location loc_deaddrop_command_shelter. The engine records whether the broadcast has been heard, the key acquired, the cipher decoded, the location revealed, and the chain resolved. It can reveal the map node when the existing conditions are met.

The questline master synopsis says a military dead-hand standby broadcast prompts for authentication at Waypoint November and that a “Microfilm index cylinder” decodes the shelter coordinates. The name used by that synopsis differs from the RequiredItemId in the chain definition. The plan does not invent or add either item. A future content owner must verify the current data consumers and decide which existing authored item name is correct before shipping any line that names it.

### 5.4 Other references and limits

A unique survivor-map collectible has a location-clue effect targeting the same shelter. That is a second existing clue source and must not be rewritten by this plan. The world-map data includes the node and connecting edges, but a route entry alone does not prove current discovery, travel, or encounter behavior. The existing coast waystation quest describes a frozen bypass at Lock Gate Four and a need for anti-corrosive solvent and grease. It does not prove that the shelter’s old manual is operational or that this narrative visit repairs that system.

The Dead-Drop Command Shelter is not the location called The Dead Hand Core. The latter has its own catalog entry, existing quest content, and UXO register. Earlier narrative content explicitly frames its machine as a system following a written mandate, not a character to be persuaded. This proposal keeps that boundary: “dead-hand” is a protocol label in the existing chain; it is not evidence of awareness.

## 6. Theme: the two names stay visible

The central image is a card with two headings and a ruled line between them. One heading came from a catalog copy. One came from a coast chart. A later hand used a question mark, then rubbed it out. The player is not asked to decide what the place “really” was. They can see which source says what and what the sources do not establish.

The emotional movement is small but concrete. People tried to leave directions for strangers. Their labels were written for separate offices, so one page describes purpose and another describes geography. Years later, both pages meet in the player’s hands. A useful map can keep both captions without turning the unresolved relation into a mystery box.

The story should not suggest that facts are meaningless. The location ID matches. The two fragments are assigned to the zone. The catalog and quest chain name a shelter. Those are real facts about the current game data. But the label “submerged relay station” and the label “automated contingency shelter” do not, on their own, establish whether the installation combined both functions, was repurposed, or was described inconsistently. The player can report the source statements and keep that distinction legible.

## 7. Tone and prose rules

Keep the language practical and human. The people speak about filing, weather, paper, a route, and a name someone copied. No one provides a lecture about epistemology. The theme should emerge because people rely on a label for different tasks.

The coast may be cold, briny, and difficult to reach because existing data and setting permit that texture. Do not add new measurements, flood depths, tide tables, or specific safe conditions. Do not describe a dramatic underwater descent. The arrival scene can remain on the mapped approach, at a landing, or in a record room, depending on the later scene contract.

Avoid militarized spectacle. The standby transmission is a loop, not an imminent threat. No minefield wakes, weapons acquire the player, or server speaks. No faction claims the shelter as a prize. No one dies during a cinematic crossing. An absence of dramatic action is deliberate: the player’s work is to distinguish a quoted description from a fact they have personally witnessed.

The ending should not “solve” the place. It should resolve the player’s task by producing an accurate note. A reader who later learns more can update the note without treating this one as a failure.

## 8. Main cast

### 8.1 Nera Vale — route-board keeper

Nera maintains a small route board at a caravan rest point. She did not visit the shelter before the Exchange and has no claim to its history. She is asked to keep the route board usable, which means a short caption is more helpful than a page of caveats. Her conflict is practical: if she writes “relay station,” a traveler might look for signal equipment; if she writes “contingency shelter,” someone might expect a dry room and supplies.

Nera is not careless. She makes concise notes because route cards are carried in rain, folded in pockets, and read under poor light. Her arc is recognizing that the two source names can both fit on a route card when the note distinguishes them as descriptions rather than confirmed functions.

### 8.2 Leth Varo — former map copyist

Leth copied coast charts for a regional office and later for whoever still needed a route. Leth recognizes the mark style on the Bathymetric Sounding Chart but cannot identify the drafter. Leth remembers that “relay” was often used as a location category even when a site contained several rooms or contracts. That memory is not proof that this location was a relay.

Leth’s voice is exact but not cold. They were trained to copy the mark as it appeared, not to reconcile every source. Their regret is that their clean copies made uncertain pages look more certain than the originals. Leth offers a careful distinction: “I can tell you how a line was copied. I cannot tell you what the line was supposed to contain.”

### 8.3 Sima Dorr — retired lock runner

Sima carried messages and hand tools between the shore approach and the Lock Gate Four work point before the region became unsafe. She remembers the manual’s phrase “at low slack” as a direction used by a specific crew. She does not remember the full schedule and refuses to reconstruct it from memory. She was never inside the mapped installation.

Sima’s role is to keep the tide language grounded in people’s labor. A timing annotation could matter to a worker without defining a safe route for everyone else. She dislikes when people treat an old work note as permission. She also dislikes when later readers call the note useless merely because it is incomplete.

### 8.4 Evin Rusk — former radio clerk

Evin copied standby traffic into a paper register. They remember the Last Rotation carrier as a repeating status phrase with an authentication prompt. Evin does not claim that the voice belongs to a living person or that the transmission knows who is listening. They have seen people call any repeated voice “a message,” even when it is a loop.

Evin helps the player hear the difference between the broadcast’s words and the chain’s interpretation of them. The voice may sound directed because its sentence is in the imperative. The existing engine treats it as a broadcast event. The story adds no proof of awareness or targeting.

### 8.5 Hara Fen — present-day salvage recorder

Hara records what a team brings back and where it came from. She has not entered the shelter. She wants a clear custody note because people keep combining the chart and manual into one purported blueprint. Her concern is the archive record, not a claim to authority over the site.

Hara can be impatient with long caveats, but she is fair about evidence. She asks that every sentence say whether it comes from the catalog, a fragment, a witness, or the player’s observation. This gives the player a practical model for making a useful entry without presenting a thesis.

### 8.6 Jori Kest — junior map apprentice

Jori is learning to keep local notes readable. They draw a single icon for the destination and leave a blank label line. At first, they think the blank means that nobody has finished the work. Nera explains that a blank can also mean the source does not answer the question.

Jori asks useful questions without turning into a device for explaining the plot. They test whether the final route card can be understood quickly. If the player chooses an overlong caption, Jori suggests putting the source distinction on the back.

## 9. Supporting voices

Optional dialogue may include:

- **Mara Hedd, former dock tally clerk:** remembers that several offices used “shelter” for rooms where a worker could wait out weather, but cannot link that usage to this site.
- **Orren Dall, radio repairer:** remembers tuning a receiver to the standby carrier and copying no words beyond the repeated phrase.
- **Pava Rill, chart seller:** remembers a coast map with a similar trench symbol, but not this fragment’s sheet or author.
- **Tess Anor, former stores runner:** remembers carrying empty tins toward the lock work point. She did not see what the tins were used for.
- **Kiva Marn, census assistant:** remembers that the office index had separate headings for “shelter” and “relay,” but says her recollection may refer to the form rather than the site.
- **Rell Oss, current route reader:** has heard that the site contains a dry storeroom. They have not been there and accept a correction when asked where the claim came from.
- **Sima Dorr:** knows one crew’s timing shorthand, not a current access window.
- **Leth Varo:** knows the copy marks, not the original purpose.
- **Evin Rusk:** knows the radio register, not the broadcast’s origin.
- **Hara Fen:** knows how the current recovered pages were handled, not what happened before they entered the archive.

Every witness must be skippable. No optional testimony should contain the only evidence needed to understand the core distinction. Every witness has a limited vantage point and may say “I don’t know” without sounding evasive.

## 10. Narrative spine

1. A repeating standby broadcast becomes available through the existing Last Rotation content route.
2. The player acquires the existing index item and resolves the existing cipher chain under its current owner.
3. The existing reveal points to loc_deaddrop_command_shelter. The plan adds no second reveal path.
4. Nera asks for a copy of the source notes, not a verdict about the site.
5. The player reads the Bathymetric Sounding Chart and the Tidal Lock Manual as two separate fragments.
6. The destination’s catalog description and coast-zone description are presented as source labels, not reconciled exposition.
7. Leth, Sima, and Evin explain the limits of their own memories.
8. The player finds a cover card that preserved both headings without a completed equivalence mark.
9. Hara helps the player write an annotation that attributes each phrase and states what is unknown.
10. Nera places the note on the existing route board or in the existing journal path, according to the current implementation contract.
11. The chain may be marked resolved only through its existing owner and criteria. “Resolved” means the content objective is done; it does not mean the site’s identity question is answered.


## 11. Opening transmission: “Last Rotation”

The first broadcast is short enough to be mistaken for a test tone. It repeats twice before any optional response is offered. The player does not hear a personal greeting. The voice does not react to noise, movement, or the player’s presence.

**Proposed transmission, first pass:**

> Stand by for last rotation.
>
> Index authentication is required before routing.
>
> Repeat: stand by for last rotation.
>
> Carrier ends in six seconds.

The words are deliberately plain. No dramatic static swell or synthetic voice effect should imply that the system has noticed the player. If the existing radio owner can provide a log view, Evin’s annotation may appear beside the transmission; otherwise the text remains a readable record or ordinary dialogue.

**Evin’s optional comment:**

> “That is how it sounded in the register. Two copies, same gap, same cut. People hear ‘stand by’ and think it means somebody is waiting at the other end. It can mean the machine was told to leave a space before the next line.”

If the player asks whether the broadcast is still being sent, Evin answers:

> “I can tell you the copy repeated. I cannot tell you whether the transmitter is still there.”

If asked whether the phrase predicts an operation, Evin says:

> “It predicts another phrase on the next line. That is the only promise the page makes.”

A second optional clip can be found after the existing cipher key is obtained. It contains no new coordinates and no extra puzzle. It is the same carrier, recorded at lower volume:

> Last rotation. Index required. Stand by.

The shorter clip is not a second quest stage. It is included to show how truncation can make a sentence sound more urgent. Evin notes that the shorter copy begins after the first syllable. No character claims that the missing part contains a secret instruction.

## 12. Radio scene: what a loop can and cannot say

The player may replay the full transmission at a small radio table. Three subtitles are available: “full register copy,” “cropped field copy,” and “play both.” The labels describe the authored source files; they do not create a radio management feature. If the runtime cannot support multiple clips, the distinction stays in a short written transcript.

When both are played in sequence, the full clip ends with a clean carrier cut. The cropped version begins mid-word and leaves the hiss tail. A listener could mistake the second copy for a response because its timing differs. Evin explains:

> “The clean one was filed before it passed through a pocket receiver. The ragged one was copied after. Same phrase. Worse edges.”

The player can ask Evin to write “broadcast repeated twice” or “two copies found.” Evin recommends the first phrase only if the original register shows two transmissions. The second is safer for the two recovered media copies. Either may appear in optional journal text, but neither alters the chain.

The player may ask, “Why use the word ‘last’?” Evin replies:

> “It could be a rotation number. It could be the last rotation on a shift sheet. It could mean the final rotation. The copy does not say which. I learned not to let one adjective do three jobs.”

This line is a thematic cue, not a new item fact. The system’s Last Rotation chain already has that name. The story does not identify the particular rotation, provide an hour, or explain what “last” refers to. If the underlying broadcast content is later found to define it more precisely, this optional line must be revised to match.

## 13. Approach scene: a route card, not a descent

The player arrives at the mapped approach through the existing world route. The scene should not narrate a deep dive or assert that the player crosses a flooded tunnel. The available prose depends on the current map, scene, and expedition presentation. A text-first version begins with the evidence available at the outer staging point:

> Salt has dried in a pale line along the lower boards. Above it, someone has tied a paper sleeve to a peg with cord. The sleeve is turned inward against the wind. Its face carries two headings, both written in the same narrow hand:
>
> SHELTER RECORD
>
> COAST RELAY COPY
>
> Between the headings is a pencil rule. The rule does not join them. It stops a finger’s width short of each.

The first inspect action looks at the sleeve’s fastening:

> The cord has been knotted twice, once for the paper and once for the peg. The lower knot is older and has darkened at the turns. It could have held the sleeve through several seasons. It cannot tell you who tied it.

The second inspect action reads the handwriting:

> The headings are written with the same pencil pressure. Beneath them, in lighter strokes, someone added: “Keep both leaves. Do not make the copy choose.”
>
> The word “copy” has a small underline. The underline is straight at the start and shaky at the end.

The player can ask Hara whether the note belongs to the chart packet. She says:

> “It was tied to the sleeve. That is custody. It is not authorship.”

If the player asks whether “both leaves” means the two map fragments, Hara answers:

> “It may. There are two sheets in the packet. There are also two headings. I would not make those counts the same without another mark.”

The line teaches the reader to resist an easy equivalence. The art direction should support the same restraint: paper, cord, salt, and two labels. Avoid a sealed blast door with flashing lights or a map that glows when the player approaches.

## 14. First map fragment: Bathymetric Sounding Chart

The fragment should be presented with its source title intact. The player can rotate or zoom the image only if that is part of the existing map-fragment presentation. No new cartography tool is required.

**Proposed readable text:**

> BATHYMETRIC SOUNDING — SHEET 4, COPY NOT FOR SOUNDING WORK
>
> Shelf line: black.
>
> Intake trench, pre-war cut: double blue.
>
> Soundings copied from north to south. Numbers below line are from working book; verify before transfer.
>
> Site mark: a square with a short bar at the western edge.
>
> Legend: missing.

The final typed line in the sheet reads:

> Intake line enters the trench before the shelf mark. The return line is not shown here.

A pencil addition beneath it says:

> There is room for another mark. No one wrote what it is.

The “square with a short bar” is a proposed visual description of the chart’s surviving site symbol; final content must use the asset or text representation supported by the existing fragment schema. It does not establish a particular door or building footprint. The note about room for another mark should not become a hidden clue that reveals a map mechanic. It is a map reader observing a blank legend.

**Player inspection variants:**

- “The artificial trench is labeled as pre-war intake work.”
- “The site symbol has no surviving legend.”
- “The chart says the return line is not shown.”
- “The sheet warns that its copied numbers require verification.”
- “The line is useful for understanding the coast paper. It is not a present-day depth survey.”

The final wording may differ, but every version preserves attribution and limitation. The player never sees a precise depth, coordinate, or “safe” water line invented for dramatic convenience.

## 15. Second map fragment: Tidal Lock Manual

The manual has a grease mark over the word “bypass.” It includes handwritten timing notes, but the numbers are partly obscured and the current game data does not supply a full timing table. The plan therefore avoids publishing exact durations.

**Proposed readable text:**

> LOCK GATE FOUR — BYPASS HAND
>
> The bypass is not the gate.
>
> When the tide mark is low, wait for the lower tell-tale to settle. Do not use the bell count from the outer board; that count was copied for a different shift.
>
> Hold at the bracket until the return mark is entered.
>
> If the mark is absent, do not infer an opening from silence.

Below this typed instruction, a pencil hand adds:

> I was told to wait. I was not told whether that meant the water, the runner, or the page.

At the bottom, a second writer has written:

> Keep the page with the chart. Separate at the shelf.

The passage is proposed authored text, not an assertion that the original fragment currently contains these exact sentences. A later content implementation must distinguish base fragment copy from new story material. The supplied catalog currently guarantees only the fragment’s name and that it indicates Lock Gate Four bypass timing. Any extension must remain subordinate to those verified facts.

The player can inspect the stain:

> The grease crosses a number, but not the word beside it. The paper was folded through the stain. The fold may have transferred the mark from one side to the other.

The player can inspect the final note:

> “Separate at the shelf” might describe where to file the sheets. It might describe where a runner divided a route. The manual does not specify.

If the player asks Sima which reading is right, she responds:

> “I can tell you how our crew used the phrase. I cannot tell you what this writer meant by it.”

Sima does not recite a timing instruction. The story does not ask players to operate the bypass, and the manually recorded timing is never presented as a current-safe route.

## 16. A paired-reading sequence

At the return desk, Hara lays the chart on the left and the manual on the right. The sheets can be viewed separately, side by side, or with the manual’s fold held across the chart’s blank legend. This is an authored reading presentation; it need not be a puzzle interface. No combination produces a hidden code.

The player sees three things:
1. the chart names an artificial trench and intake lines;
2. the manual names Lock Gate Four and a bypass;
3. both sources leave some relation unstated.

Nera suggests a possible caption:

> “The coast installation is behind Gate Four.”

Hara objects gently:

> “That joins the route marks. The papers do not.”

The player can ask Nera what she meant. She says:

> “I meant the note would be short. I did not mean the pages proved it.”

This is not a confrontation. The scene shows how a useful summary can overstate its source without anyone intending to mislead. The player can choose one of three draft headers:

- **Two papers, one mapped location.**
- **Shelter and relay: labels from separate records.**
- **Coast documents; exact relationship unresolved.**

The choices are editorial phrasing. They do not branch the world, change access, or affect a witness’s standing. Each can lead to the same accurate body text.

## 17. The cover card with two headings

The card is found in a transparent sleeve with a clean face and an empty reverse. Its edges are square, unlike the water-worn map fragments. The front contains a typed heading, a copied heading, and an instruction about filing.

**Front of card:**

> DESTINATION INDEX
>
> Entry: Dead-Drop Command Shelter
>
> Secondary heading copied from coast sheet: Deep Coast Saline Shelf / maritime relay station
>
> Relationship between headings: ______

The blank line is not crossed out. There is no checkmark, staff initial, or date. A smudged graphite mark appears at the left margin, where a writer may have rested a hand. It cannot be read as a letter.

**Reverse of card:**

> If the source sheets remain separate, keep this card at the front.
>
> If a later reader can reconcile them, write the source and date.
>
> Do not erase a heading to make room.

This is a proposed object, not an existing data-defined item. If the current readable-document owner cannot attach a card to the site without adding persistent state, the content can appear as a one-time inspection passage. The important authored fact is simply that a prior copyist kept two headings in view. No unseen person’s identity is required.

## 18. Dialogue: Nera and the route-board problem

Nera’s request is modest. She wants a line she can fit beside the existing map symbol. She does not ask the player to decide the site’s function.

**Nera, first conversation:**

> “I have a square for it and two names for the square. Someone keeps asking me which one to put in the large letters.”

**Player:** “What happens if you use both?”

> “The card gets crowded. The map does not.”

**Player:** “Which name do you use?”

> “The one the source beside me uses. That works until the next person sees the other sheet.”

**Player:** “Do you think they describe one place?”

> “They were filed under one place. That is enough for a route note. It is not enough for a building plan.”

If the player presses for a preferred name:

> “I can tell you which name helps a traveler recognize the entry. I cannot tell you which name was true before the papers reached us.”

If the player says that uncertainty will confuse people, Nera does not dismiss the concern:

> “Then make the first line short and the second one exact. A traveler needs to find the entry. A clerk needs to know what the entry does not prove.”

A follow-up scene after the player reads both fragments:

> Nera turns the route card so the symbol faces her. She traces the margin, not the square.
>
> “The mark is steady. It is the words around it that move.”
>
> She hands the pencil over before adding anything.

If asked how she wants to be credited, Nera says the route board belongs to whoever maintains it that season. The story does not invent a signature authority or a standing title beyond her established role in this plan.

## 19. Dialogue: Leth Varo and the copied mark

Leth recognizes a copier’s mark but does not claim to have drawn the original. The conversation can occur near the chart table or by radio if the character is not physically present.

**Player:** “You recognize this line?”

**Leth:** “The double stroke, yes. The hand, no.”

**Player:** “What does the double stroke mean?”

**Leth:** “On our copies? A cut made before the survey. On another office’s copy, it might mean a cable trench. Same mark family, different legend.”

**Player:** “Could it mean a shelter?”

**Leth:** “It could be near one. The chart does not give it that meaning.”

**Player:** “Someone wrote that the trench was for intake lines.”

**Leth:** “That is a source note. Keep the note attached. If you pull it away, the line gets promoted to a fact by itself.”

Leth then studies the symbol with the short western bar:

> “This is a location mark, not a floor plan. People see a square and start drawing rooms inside it. I used to do that when I was young. The first correction I got was written in red ink: ‘A square is not a promise.’”

If asked who wrote the correction, Leth says they cannot remember. If asked what office used the form, they say the heading was trimmed from the top. The player can preserve these memory limits as dialogue, but they should not become an unresolved conspiracy thread.

Leth’s personal regret emerges late:

> “My copies were clean. A clean copy makes a reader feel as if the person who drew it was sure. They were often sure about the line and unsure about the place. I knew that. I still made the line clean.”

The response is not an apology for falsifying the chart. It explains why presentation and certainty should not be confused.


## 20. Dialogue: Sima Dorr and the phrase “low slack”

Sima is careful whenever someone asks about the tide mark. She remembers a work phrase, not a navigational instruction. The exchange should feel like an ordinary person refusing to make a memory carry more than it can.

**Player:** “Does low slack mean the entrance was safe?”

**Sima:** “It meant our shift could begin its check. Safe was a different column.”

**Player:** “Was the check at Gate Four?”

**Sima:** “The manual says Gate Four. I remember a crew at the gate. I cannot tell you that this page is the one we used.”

**Player:** “What did you wait for?”

**Sima:** “A mark on the board. Water settled against the lower tell-tale. Then the runner took the page to the next station.”

**Player:** “Did you wait for a person?”

**Sima:** “Sometimes. The page does not say.”

Sima has a small habit of folding the corner of a paper before handing it over. If the player asks about the folded manual, she says:

> “Fold tells you where it traveled. It does not tell you who carried it.”

If the player says the timing note could still help today, she answers:

> “It might help someone understand what the old crew intended. It is not a license to try their work.”

If the player asks whether she ever entered the shelter, she says:

> “No. My run ended at the lock board. Past that, the next crew had its own key and its own list.”

This answer stays stable across branches. Sima has no secret route map, no hidden key, and no authority to open the site. Her memory is valuable because it distinguishes a crew’s procedure from a universal rule.

A later optional line lets the player ask what a route note should say:

> “Write that the old page names low slack. Write that you do not have a current reading. Leave enough room for the next hand to add what they actually see.”

Sima’s suggested wording is not mechanically enforced. It is an example of useful humility in a field record.

## 21. Dialogue: Evin Rusk and the “standby” voice

Evin’s scene can begin after the player finds the two different transcriptions. Evin refuses to identify the broadcast as an order, a threat, or a response without the surrounding record.

**Player:** “The voice says ‘stand by.’”

**Evin:** “Yes.”

**Player:** “Does that mean it was waiting for someone?”

**Evin:** “It means the words on the strip are ‘stand by.’”

**Player:** “You cannot tell?”

**Evin:** “I can tell what the phrase usually did in a register. I cannot tell what this transmitter was connected to. There is a difference.”

Evin can compare the two copies:

> “This one was copied at the radio desk. This one has a crease across the second line. Same heading, same date field, different paper. If the date field was copied from the first onto the second, then it is not independent corroboration. It is a duplicate.”

The player asks whether the two copies can establish the broadcast’s date. Evin says the heading gives a month but the year is missing. The date cannot be reconstructed from the transmission’s repetition count.

**Optional follow-up:**

> “When I was young, I thought a repeated message was a person insisting. Later I learned that a repeat can be a setting. The voice is still a voice. It just might have ended before any of us heard it.”

No supernatural ambiguity is implied. The voice is a recording or broadcast in the story’s ordinary world. Its human source, if any, is not revealed. The plan does not invent a person trapped in the shelter, an operator still at the console, or an intelligence answering through the signal.

If the player tells Evin that the location was revealed by the cipher, Evin says:

> “That is what the chain did. It tells you where the paper points. It does not tell you who is still there.”

This line carefully distinguishes map reveal from site occupancy.

## 22. Dialogue: Hara Fen and source custody

Hara helps the player prepare the final annotation. She uses four labels while reading: catalog, fragment, witness, observation. These are ordinary filing terms, not a new game taxonomy or collectible system.

**Hara:** “Read each sentence and tell me where it came from.”

**Player:** “The place is called a shelter.”

**Hara:** “Which page?”

**Player:** “The location entry.”

**Hara:** “Good. Write ‘the location entry calls it a shelter.’”

**Player:** “The place is submerged.”

**Hara:** “Which page?”

**Player:** “The coast-zone description.”

**Hara:** “Then write that. Do not let the verb disappear.”

**Player:** “The chart shows intake lines.”

**Hara:** “The chart description says artificial trench for intake lines. If you have the actual sheet, quote its line. If you only have the fragment title in the catalog, say what the catalog guarantees.”

Hara is not pedantic for its own sake. The archive contains summaries copied from longer records. When the source is summarized, the note should say that. The player’s task is to stop a future reader from assuming that all four voices—catalog, fragment, witness, and present observation—came from one authoritative blueprint.

If the player asks whether this makes the final note too long, Hara says:

> “Long notes can be hard to use. Short notes can be wrong. Put the route first and the limits after it.”

Her proposed card layout is:
1. destination name and map reference;
2. the two source labels;
3. the location of the source sheets;
4. one sentence warning that the historical timing is not current access guidance.

The layout must use the existing journal or map annotation affordance if one exists. The plan does not require adding a writable map system.

## 23. The rumor at the rest point

Rell Oss has heard that the shelter is dry and stocked. They have no source for either claim. Their rumor is not malicious: a person once returned from the coast carrying a clean canister, and someone else called it “shelter salvage.” The phrase changed as it traveled.

The player may hear Rell’s first version:

> “There is a dry room past the lock. Old stock still on shelves. That is what the route card says.”

If the player shows the fragment descriptions, Rell reads them quietly.

> “This is not what the card says.”

**Player:** “Who told you there were shelves?”

**Rell:** “Mara, I think. She said a driver heard it from the people who brought the canister.”

**Player:** “Did you see the card?”

**Rell:** “No.”

**Player:** “Did Mara?”

**Rell:** “I never asked.”

Rell does not defend the claim once its source is unclear. They offer to correct the route board. The correction does not shame the speaker:

> “I should write ‘unverified storeroom rumor’ or remove it. If I write nothing, the next person may still hear it from me.”

The player can choose between “remove the stock claim” and “preserve it as a rumor with no verified source.” Both choices keep the map annotation truthful. The correct implementation may represent the correction as one dialogue outcome and a single text line; it should not add rumor persistence or a new trust score.

Rell’s scene gives the story a present-day consequence without creating a loot promise. The danger is not that the player will miss a reward. It is that an unverified phrase can become an expectation and send someone into a place under the wrong assumption.

## 24. Scene: the route board in poor light

At evening, Nera tries the draft card beneath a lamp with a low wick. The lamp is not an asset requirement. It is an optional context for reading the card. The important observation is that a long statement becomes hard to parse when folded.

**Nera:** “If I write both full descriptions on the front, nobody can find the symbol.”

**Jori:** “Put the descriptions on the back.”

**Nera:** “Then someone copies the front and loses the back.”

The player can test three proposed front lines:

1. **Dead-Drop Command Shelter — coast map references also call it a submerged relay station.**
2. **Dead-Drop Command Shelter — see coast sheet; relay label not reconciled.**
3. **Dead-Drop Command Shelter — historical coast records attached.**

Nera reads each aloud. She says the first may make the relay name sound like an accepted alias. The second is precise but cramped. The third is easier to carry but does not tell the reader why two labels matter. Jori proposes using the third on the front and the second on the reverse.

No option is treated as a moral failure. The final player choice reflects practical needs: what is legible at a glance, and what must stay attached for a later reader.

**If the player chooses the short front label:**

> Nera copies it in block letters. On the reverse she writes, “Location catalog: automated contingency shelter. Coast zone record: submerged maritime relay station. Relationship not established by these records.”

**If the player chooses the detailed front label:**

> Nera writes smaller but leaves the final word on its own line. Jori asks whether “unreconciled” means the map is wrong. Nera adds “source labels” before it, so the qualification has something to attach to.

**If the player defers:**

> Hara files the unfinished card with the source sleeve. The route board remains unchanged until a current map owner can place the note. The journal records the evidence without claiming that the field card was updated.

The final path must not imply that the route board has a new feature. It is an authored scene with a result in current narrative content.

## 25. Scene: a blank legend

The chart’s blank legend recurs in a quiet moment. Leth sees Jori trying to fill the empty box with a relay symbol.

**Jori:** “If we leave this blank, someone will think we forgot.”

**Leth:** “They might.”

**Jori:** “Would you rather they think we forgot or that we do not know?”

**Leth:** “Those are not the only two choices. You can write ‘legend not present on surviving copy.’”

Jori tries the phrase aloud and finds it too long. Nera asks what the copy needs to do.

**Jori:** “Let someone see that the square is a location mark. Let them know it has a short bar.”

**Leth:** “Then say that much. The blank belongs to the source. Your note can describe the blank without filling it.”

The player may choose a marginal note:

- “Symbol appears as a square with a short western bar; original legend missing.”
- “Chart’s site symbol is unkeyed.”
- “No legend survives on this copy.”

The first version is most descriptive. The third is shortest. The content should not manufacture a conventional symbol or map icon meaning. Jori can draw a boxed question mark on the apprentice copy only if the underlying asset makes clear that this is a modern annotation, not a source mark.

Later, a returning player can see the erased pencil outline where Jori first tried the relay symbol. Leth has not destroyed it. The apprentice copy is a working sheet, not an original fragment. The small erasure becomes a sign of learning: a tempting interpretation can be removed from the copy without pretending it was never considered.

## 26. Scene: two routes described from two ends

Sima and Leth talk over the chart. Sima describes the work route outward from Gate Four; Leth reads the chart from the shelf inward. The directions sound contradictory until each person says where they stood.

**Sima:** “We carried the book from the lower board toward the lock.”

**Leth:** “The sheet is copied from north to south.”

**Sima:** “Then it is not our walking order.”

**Leth:** “No. It is the survey order.”

They draw neither route over the other. The player can ask whether the two lines meet. Sima says she has no reason to know. Leth says the chart has no scale marker in the surviving copy. Both statements remain true.

The scene demonstrates that different ordering does not mean one person is wrong. A survey can be copied in one direction while a crew walks in another. The player can preserve both observations without deciding whether the specific documents shared a physical route.

**Optional journal line:**

> “Leth remembers chart-copy order running north to south. Sima remembers the lock crew carrying its book from the lower board toward Gate Four. Their memories concern different tasks and do not establish where the routes meet.”

The journal line should appear only if both conversations have been heard, if the current journal supports conditional prose. Otherwise it can be written as an optional conversation recap. It does not need a new state variable if the existing quest content can display both records together.


## 27. Document: the forwarding slip

This slip is the first page in the recovered sleeve. It is short, administrative, and addressed to no named person.

> FORWARD WITH COAST PAPERS
>
> Keep the chart and lock sheet in this order until the shelf copy is checked.
>
> If one sheet is requested alone, copy its source heading onto the sleeve.
>
> If a later copy gives the destination a different name, retain both names at the top.
>
> No correction entered.
>
> Signature line: blank.

The slip may have been used by a clerk, a runner, or a later archivist. The paper has a faint vertical impression where another sheet rested on it, but there is no legible text in the impression. A player can inspect it, but cannot recover a hidden signature. The words “No correction entered” refer to a blank correction field, not necessarily to a failed decision.

**Optional archive note:**

> “Forwarding slip found with coast papers. It instructs a copyist to retain differing destination names. The surviving copy does not identify the writer or explain why the names differ.”

If the player asks Hara whether the slip proves that an earlier clerk noticed the mismatch, she says:

> “It proves someone allowed for different names on copies. It does not prove these two names were the reason.”

The distinction keeps the artifact useful without treating it as a confession or a final answer.

## 28. Document: the duty-card fragment

A small card is clipped to a blank roster form. Only the lower half survives. The word “rotation” appears on the top line. The names have been removed with a blade or lost to water; no names can be read.

> ROTATION — COAST RUN
>
> Take the clean copy.
>
> Leave the marked sheet.
>
> On return, put the two headings back under the same sleeve.
>
> No one carries the register past the lower board.

The phrase “no one carries the register past the lower board” may tempt a reader to connect it to the shelter’s locked or inaccessible interiors. The scene explicitly cautions against that jump. Sima says the lower board was a routine checkpoint on her crew. It might be a different “lower board” from the one in the manual.

**Sima:**

> “People use ‘lower board’ for whatever board is below the one they are standing at. There were several.”

**Player:** “Could this be your crew’s card?”

**Sima:** “It could be. I would know my own marks if they were still there. They are not.”

The player can annotate the card “possible coast run; author unknown.” A more certain label is unavailable. The story does not add an NPC name to the damaged card.

## 29. Document: the catalog extract

A clean extract of the location catalog is printed as a piece of player-facing archive content. It contains only the relevant entry and its current fields.

> DEAD-DROP COMMAND SHELTER
>
> An automated contingency shelter revealed through the Last Rotation dead-hand protocol. Contains classified directives and high-grade technical relics.
>
> Danger: recorded in location authority.
>
> Travel: recorded in location authority.
>
> Radiation: recorded in location authority.

For the actual game text, the source values should be generated or referenced from the current authority, not copied into a separate mutable note. This design plan reproduces only the descriptive wording relevant to the narrative. It does not propose that all raw data fields be exposed to players.

Hara points out that the catalog says what the location is called and summarizes its content category. It does not mention the bathymetric trench or Lock Gate Four. The absence of those details from one catalog is not evidence that they do not exist; the map-zone data contains them.

## 30. Document: the map-zone extract

A second extract carries the map-zone description and fragment titles.

> DEEP COAST SALINE SHELF
>
> Hidden installation: Dead-Drop Command Shelter.
>
> Submerged maritime relay station situated past the tidal locks. Watertight bulkhead entries accessible at low slack tide.
>
> Fragments:
> Bathymetric Sounding Chart.
> Tidal Lock Manual.

Jori reads “watertight” and asks whether that means the entrance is sealed. No character answers for the source. The word describes an entry condition in the map-zone record; it does not tell the player whether any particular door is open now.

If asked whether “accessible at low slack tide” is a current guarantee, Hara answers:

> “It is how the installation description phrases the route. We have no present measurement here. Treat it as a record, not a clock.”

This protects the story from turning a map fragment into an unverified travel mechanic.

## 31. Document: the duplicated location code

Inside the sleeve, the location ID appears on a carbon strip: loc_deaddrop_command_shelter. The strip is crisp enough to read. On the back, an unknown hand copied it again and underlined “same entry.”

The player can compare this with the map-zone ID. The two references match. This is meaningful evidence: current records assign both descriptions to the same data key. Yet it does not prove that the historical descriptions are compatible. A shared key can be an editorial merge, a deliberate combination, or a current data inconsistency.

Hara says:

> “The ID is the same in our catalog. That tells me how our game data joins these records. It does not tell me what the pre-war office called the rooms.”

This line is meta only in the design document; in-world, it should be phrased as an archivist describing a shared entry number.

**In-world variant:**

> “Our shelf code is the same on both sheets. That tells me where we filed them. It does not tell me what the old office meant.”

The player can record “same current map entry” without claiming “same original installation.”

## 32. Document: the index cylinder label

The chain’s key-name mismatch must be resolved before an implementation uses a specific item description. The narrative can still show the sleeve label in a version-neutral way:

> INDEX CYLINDER
>
> Last Rotation: cross-reference
>
> Contents not transcribed on this wrapper.

Do not add “microfilm,” a new item ID, a second required key, or a new decoding rule until the current item schema and chain consumers are audited. The existing chain engine currently expects item_archive_index_cylinder. The questline synopsis currently uses “Microfilm index cylinder.” The plan records both names only in its evidence note, not as two collectible objects.

A future narrative writer can replace this generic label with the current authoritative item name. If the catalog already defines a material or function, use that existing description. If it does not, describe only the cylinder and its printed heading. The final player-facing passage should not make the naming discrepancy feel like a mystery that characters solve through speculation.

## 33. Document: the recovered register leaf

A torn leaf contains columns and shorthand, but not enough row headings to reconstruct its purpose.

> COAST / ROTATION
>
> copy out — 1
>
> copy back — 1
>
> shelf entry — blank
>
> next holder — blank
>
> received mark — smudged

The player can inspect each column. “Copy out” and “copy back” are legible. “Shelf entry” may refer to filing. “Next holder” may refer to custody or assignment. The blank fields tell us that the copy is incomplete, not that nobody received the packet.

Leth says the handwriting is consistent with office bookkeeping from several decades ago, but not enough to identify a particular office. If the player asks whether the sheet could be a relay log, Leth says:

> “It could be attached to a relay file. It is not a signal log. There is no frequency, call sign, or interval.”

If asked whether it could be a shelter occupancy roster, Leth says:

> “There are no people listed.”

The player can file it as an unclassified register leaf. It adds texture to the archival mystery but does not provide a secret answer.

## 34. Document: Evin’s carrier note

Evin’s personal notebook contains a short note written after copying the standby transmission.

> Repeated at the same interval.
>
> No answer heard between copies.
>
> Entry ends where carrier ends.
>
> Do not write “operator replied.”

Under it, in another pencil:

> I wrote “operator replied” on the first copy. Crossed it out.

The page does not identify who made the first note or the correction. Evin recognizes the second pencil as theirs, but cannot place when they changed it. The player can ask if the writer was embarrassed.

**Evin:**

> “Probably. I was embarrassed often. That does not tell you which day.”

The altered phrase is a small echo of the plan’s central issue. One writer first described the sound as a reply, then changed the verb when they reconsidered the evidence. It is not proof of a hidden second voice.

## 35. Document: Nera’s old route card

Nera keeps an older route card on which the destination is named “Coast Relay.” She admits that the caption came from a hand-me-down copy and that she copied it without looking at the location catalog.

> COAST RELAY
>
> Ask for low slack.
>
> Follow the channel mark after Gate Four.
>
> Spare paper in sleeve.

The note is useful as history of the route board but should not be kept as current guidance. Nera crosses out “Ask for low slack” on the new version, not because she knows it was wrong, but because the phrase is ambiguous and not safely current.

**Nera:**

> “I thought it meant a person on the route knew when to go. Someone else could read it as a time. I do not have a current reading to give them.”

The player may keep the old card in the archive, marked superseded, or leave it attached to the source sleeve with a note that it is not current route guidance. Both paths preserve the card’s existence and prevent it from becoming an operational instruction.

## 36. Document: a letter from a copyist

The letter is in an envelope marked only “for the next copyist.” It describes the ordinary inconvenience of reconciling forms.

> The office sent two requests for the same shelf code.
>
> The first asked for the shelter heading.
>
> The second asked for the coast relay heading.
>
> I returned both with the sheets attached because I did not know which box had been entered first.
>
> A clerk brought the packet back and said to leave the headings.
>
> I thought they meant until the question was answered.
>
> The next time I saw the packet, it had a new sleeve.
>
> I do not know whether that was an answer.

The letter can be interpreted as administrative context. It does not confirm that both requests refer to this exact site; the shelf code is smudged. If the player compares the surviving digits, the first three match the current archive suffix, but the last digit is missing. The game should not present partial code matching as proof.

A closing line reads:

> If you can tell what the old words mean, add it in the margin. Do not rub one out.

This is the emotional ancestor of the paired-label card. The letter’s writer hoped for a resolution but also wanted the source text preserved.

## 37. Document: two unposted messages

Two short messages have been folded together. They appear to be notes between coworkers. The text is not addressed and must not be treated as radio dialogue.

**Message one:**

> The shelf copy is back. It still has the square.
>
> I have put the lock sheet underneath it.
>
> If you want them separated, say which one you need.

**Message two:**

> Keep them together until the map clerk returns.
>
> If the clerk does not return, the sleeve is the record.

A reader could infer that a map clerk was expected to review the packet. The text does not prove who that clerk was or whether they returned. The player can ask Leth whether one message resembles a standard office hand.

**Leth:**

> “The wording is familiar. The hand is not mine.”

The pair sets up the possibility of a delayed review. The story refuses to turn that into a tragedy by implication. No character is confirmed dead or missing.

## 38. Document: an undated inspection tick

A small tick mark appears beside the phrase “shelf entry.” The mark is made in ink and has a hook at the end. A separate tick on the chart is straight. The two may be different people’s marks, or the same person’s marks made with different pens.

A player who examines the ink can see that the hooked mark sits beneath an old water stain. The mark is still visible because the stain dried around it. That suggests an order of events but not a date. The player can record “ink mark beneath stain” as a physical observation.

Hara cautions:

> “Sequence is not identity. It tells you one thing happened before another. It does not tell you who held the pen.”

No autopsy of paper chemistry or forensic mechanic is introduced. This is ordinary close inspection in prose. The player may leave it unrecorded.

## 39. Document: the back of the manual

The back cover of the Tidal Lock Manual contains a pencil table with its column labels gone. Two rows survive:

> low mark — hold
>
> return mark — carry

The numbers once written in a third column have bled into the paper and cannot be read. Sima identifies “carry” as a word used when a page or tool had to move to the next station. She cannot say whether it refers to a person, a message, or a device.

When the player asks whether “hold” refers to the water, she says:

> “It could. It could also mean do not send the page yet. The missing headings are the part I would need.”

This document deliberately uses procedural language with more than one plausible referent. The ambiguity is bounded: the fragment is a manual for a gate bypass, so a water-related reading is plausible; Sima’s memory allows a work-route reading. The story does not declare both equally likely, only unresolved.


## 40. Document: the half-written correction

A correction begins in blue pencil and ends before the word “therefore.”

> These two entries refer to the same—
>
> [rest of line blank]

There is no crossing-out, no later date, and no signature. The writer may have stopped because they were interrupted, because the sentence was wrong, or because the information was unavailable. The player cannot identify which.

If asked what the correction should say, Hara answers:

> “That is your line to write now. The old one stopped.”

The player can add a separate annotation rather than completing the original sentence. This preserves the difference between an unfinished historical attempt and a current careful note. If the player chooses to complete the old line in a proposed draft, Nera gently advises against it:

> “Then someone will think the old writer finished it.”

The draft can instead say, “A prior correction is unfinished. This copy does not supply its missing conclusion.” This text is optional flavor but reinforces the story’s source handling.

## 41. Document: a small receipt

A narrow receipt says that two sleeves were issued from the records desk. The first line is legible:

> map copy sleeve — 2

The next lines are mostly lost. A visible word near the bottom looks like “returned,” but the final letters are missing. The receipt has no destination, date, or signature.

Jori wants to use it as evidence that two maps existed. Leth points out that “two sleeves” means two sleeves, not necessarily two maps. They might have been issued for two different people, or one could have been replacement stock.

**Jori:** “It still tells us someone expected two.”

**Leth:** “It tells us two were issued.”

**Jori:** “That sounds like the same thing.”

**Leth:** “It sounds close. That is why it is easy to copy wrong.”

The player can add a note: “Receipt records two map-copy sleeves; destination and contents unknown.” The receipt should not be expanded into a missing page mystery with an implied answer.

## 42. Document: Nera’s corrected caption

Nera drafts three versions on scrap card. Her first attempt reads:

> Dead-Drop Command Shelter / Underwater Relay.

She crosses out the slash because it can mean either “also known as” or “two entries combined.” Her second attempt reads:

> Dead-Drop Command Shelter; submerged relay station.

She crosses out the semicolon because it still suggests a direct equivalence. The third attempt is:

> Dead-Drop Command Shelter. Coast-zone record describes a submerged relay station. See attached papers.

She keeps that one. It is a little longer, but the verbs remain attached to their source. The player can choose an even shorter field label and preserve the detail on the reverse. The final card must not suggest that the catalog and zone descriptions have been reconciled.

## 43. Document: the source sleeve inventory

Hara makes a simple inventory of the packet:

1. forwarding slip, no author;
2. bathymetric chart fragment, partial legend;
3. tidal lock manual fragment, handwritten bypass notes;
4. catalog extract, current location description;
5. coast-zone extract, current installation description;
6. carrier note, origin of register copy unknown;
7. duty-card fragment, no names;
8. correction draft, unfinished;
9. two-message fold, dates absent;
10. archive index cross-reference, item wording to verify.

The inventory is not a new collectible ledger. It is a readable content page that can appear as a single list if the existing document interface supports it. If not, each item can be found independently and the player journal can show only the final summary. Nothing depends on collecting all ten papers. The core route and ethical choice remain available with the two cataloged map fragments alone.

## 44. Optional sound scene: the carrier ends

If the current audio catalog already supports a short radio carrier, the player may hear the transmission again while the chart is open. The sound should be restrained: a low hiss, a brief phrase, then silence. Do not add a voice that calls out the player’s location.

The transcript appears in full. After the carrier ends, Evin says:

> “That gap is the part people fill in.”

The player can answer:

- “It sounds like someone should speak next.”
- “It could be a scheduled pause.”
- “We only have the recording.”

Evin accepts any response. They do not argue the player out of an impression. If the player says it sounds like an order, Evin replies:

> “It is phrased like one. We do not have the next page.”

If the player says it sounds like a person, Evin says:

> “It was spoken by a voice. That much is true.”

The distinction is not an audio puzzle. It adds a moment of human interpretation without implying the recording can hear.

## 45. Scene: the packet arrives at the archive

Hara sets the two fragments on separate cloths. She does not place one on top of the other; the water damage could transfer. She reads the catalog extracts aloud and then stops.

**Hara:** “These came to us in one sleeve.”

**Player:** “Does that mean they belong together?”

**Hara:** “It means someone kept them in one sleeve.”

**Player:** “Could the one page explain the other?”

**Hara:** “It could. I do not see a note that says it does.”

Nera asks whether they should make a new packet. Hara suggests keeping the two source sheets together but attaching their descriptions separately. That practical arrangement becomes the plan’s key motif: the source documents can remain physically adjacent without being collapsed into one claim.

The scene should avoid a curator who insists on a perfect archival process. Hara has limited space and ordinary responsibilities. Her choice to keep the papers together is an act of stewardship, not a new archive feature.

## 46. Scene: first return to the rest point

When the player returns with the fragments, Nera has already heard Rell’s stock-room rumor. She asks whether the player found shelves. The player can answer:
- “The records do not establish that.”
- “The destination’s catalog says it contains technical relics.”
- “I found two descriptions, not a verified inventory.”

Nera is disappointed but does not scold the player for withholding a promise.

> “A route card is not a shopping list. I know. I still hoped it would tell me more.”

The player can ask what she hoped for. She says her crew was looking for a dry place to store paper during the wet season. She did not believe the shelter was safe; she wanted to know whether the label referred to a useful room. The story makes the rumor’s practical origin clear without confirming the inventory.

If the player repeats the catalog phrase about high-grade technical relics, Nera says:

> “That is an inventory category. It is not a count of what is still there.”

The line reminds players that data describes a site while actual scavenging remains under current game systems.

## 47. Scene: the board is read by someone new

A new traveler approaches the route board while Nera and Jori are checking the card. The traveler reads only the front and asks if the shelter is under the water. Nera begins to answer, then glances at the reverse.

**Nera:** “The coast-zone description calls it submerged. The location record calls it an automated contingency shelter.”

**Traveler:** “So which way do I go?”

**Nera:** “Use the route entry that is active for your map. This card tells you what the records call the place. It does not replace the route.”

The traveler asks whether low slack is the right time. Nera replies:

> “That timing is printed in a historical manual. I do not have a current reading.”

The traveler nods and leaves with no new promise. Jori asks if the card failed. Nera says:

> “It answered the question it could. The traveler asked a different one.”

This brief scene gives the note an observable purpose: it prevents a historical phrase from being mistaken for live navigation guidance.

## 48. Scene: the player declines to write

The player may choose to leave the route card unchanged and archive a note instead. Hara does not treat this as avoidance.

**Hara:** “The source packet is filed either way.”

**Player:** “Would a short note be better?”

**Hara:** “A correct note is better. A short correct note is easier to carry.”

**Player:** “I do not know which one is correct.”

**Hara:** “Then write what you know. You know where the words came from.”

If the player declines every proposed caption, the story still resolves when the journal contains the two attributed descriptions. The existing chain’s resolution rules remain authoritative; this narrative plan does not require a new Boolean for “annotation accepted.” If the chain owner requires an explicit content stage, use its existing resolved state and keep the authored completion text neutral.

The ending for this branch is not silence. The player chooses not to make a field card. The source papers remain together in a sleeve, and Nera leaves the existing map entry untouched.

## 49. Scene: the player writes a concise note

A concise note can read:

> Dead-Drop Command Shelter: coast papers also describe a submerged relay station. See source sleeve. Historical tide notes are not current access guidance.

This line is the shortest complete version proposed in the plan. Before it ships, a narrative owner must verify that the current interface can store or display it without a new parallel map-note system. If it cannot, the same text can be presented in dialogue or journal prose.

Nera tests the wording aloud. “Also describe” might imply that the labels describe the same thing, but because it names the source papers, it is sufficiently bounded for a field card. Hara suggests “coast-zone record describes” for maximum attribution. The player can choose the compact phrase or the precise phrase. Both lead to the same story outcome.

## 50. Scene: the player writes the full note

The full note reads:

> The current location entry calls this place the Dead-Drop Command Shelter and describes it as an automated contingency shelter. The Deep Coast Saline Shelf record assigns the same mapped location ID to a submerged maritime relay station past tidal locks. The Bathymetric Sounding Chart fragment describes a pre-war artificial trench for intake lines; the Tidal Lock Manual fragment concerns Lock Gate Four bypass timing. These records do not establish how the shelter and relay descriptions relate. The manual’s low-slack wording is historical and does not provide present-day access guidance.

Nera says it will not fit on the route card. Hara says it can stay in the archive cover. Jori draws a box around “source note” so a later copyist will not mistake the paragraph for route instructions.

The full version is not the “best” ending; it is useful for a reference binder. A future game UI may not have space for it. It can live as a readable note or dialogue exchange in the existing narrative flow.

## 51. Scene: one label must fit the front

If the current card has room for only one large heading, the player chooses which name appears first. The choice changes prominence, not the truth of the archived summary.

**Shelter first:**
> Nera leaves Dead-Drop Command Shelter as the large heading. Below it, in smaller writing, she attributes the relay description to the coast-zone record.

**Relay first:**
> Nera uses “Deep Coast Saline Shelf” as the large heading. The current location name is copied in parentheses, with a note that it is the location catalog’s display name.

**No preferred heading:**
> The card reads “Coast installation — see source sleeve.” The two names are listed together in the archive note.

The player may ask whether one choice favors an institution. Hara says:

> “A heading tells the reader where to begin. It does not have to tell them where to end.”

The plan does not add faction standing or relationship outcomes. It is an editorial choice with a visible textual result.

## 52. Scene: someone wants the old wording erased

A route reader named Pellan Cale—an optional one-scene character who has no connection to other similarly named characters—asks Nera to remove the phrase “automated contingency shelter.” They worry that travelers will expect a dry refuge.

Nera explains that removing it would erase a current source description. Pellan counters that keeping it may mislead people. The player can propose:
- keep the phrase and attribute it to the location entry;
- move it to the reverse of the card;
- keep both descriptions in the archive and use a neutral front label.

Pellan accepts the third option if the front card remains clear. They do not need to be persuaded that the record matters; they need to know the route board will not promise shelter. The final line is:

> “I wanted the name gone because I thought it was a promise. If it is a source label, say so.”

This scene shows why attribution matters to present-day readers. It does not make Pellan a villain for wanting less confusing directions.


## 53. Scene: someone wants the two descriptions merged

A well-meaning copyist asks Nera to shorten the note to “shelter relay.” It is a compact phrase, and it sounds as if both catalogs are in agreement. The player can accept it, reject it, or ask for a source-specific line.

If accepted as a private working shorthand, Hara allows it only with a clear label: “unofficial shorthand; not a source title.” The public card does not use it. If rejected, the copyist explains that they wanted to save room, not hide the discrepancy. If the player asks for a more concise accurate phrase, Jori suggests:

> “Shelter / relay labels both recorded.”

Hara notes that the slash remains ambiguous. Jori changes it to:

> “Shelter and relay labels retained; relation unknown.”

This is the best short archive phrase, though it may be too cryptic for a route board. The scene offers a small example of editing prose to reduce false certainty.

## 54. Scene: the map apprentice makes a clean copy

Jori draws the destination symbol in a fresh notebook. The shape is neat: square, short bar on the western edge. They ask whether the bar should point left or right on the route board.

Leth explains that “western edge” belongs to the chart’s orientation, while left and right depend on how a reader turns the paper. Jori rotates the page and sees the symbol change sides without changing its source orientation.

The player may select one caption:
- “symbol copied as shown”;
- “square with short western bar”;
- “orientation follows the chart’s north mark.”

The last caption is only valid if the surviving copy has a legible north mark; the current fragment catalog does not guarantee that. Unless a later asset audit finds it, the implementation should prefer the first or second.

Jori decides to copy the mark with a tiny arrow showing which edge was called west in the source. If the arrow’s reference is uncertain, the character draws no arrow and writes “as shown.” The lesson is physical, not abstract: a map mark can remain the same while the paper is turned.

## 55. Scene: the lower board is not a door

The phrase “lower board” appears on both the duty-card fragment and the manual. The player asks Sima whether it refers to an entrance. She says it might refer to a board where shift notes were posted, a tide gauge board, or a lower-level board in a two-board office.

**Sima:** “There were boards at different heights. Some were for water. Some were for people. Some were just the next wall that had a nail.”

If the player asks whether one board was physically below the other, Sima says:

> “Often. Not always.”

The scene does not add a hidden room or vertical map. It gives the player permission not to turn every repeated phrase into a route clue. In the journal, the repeated wording can be described as “the same phrase appears in two surviving papers; the referent is not identified.”

## 56. Scene: Hara sets the source order

Hara places the documents in a working order:
1. the forwarding slip;
2. the location catalog extract;
3. the coast-zone extract;
4. the Bathymetric Sounding Chart;
5. the Tidal Lock Manual;
6. the register and carrier fragments;
7. the unfinished correction.

The order is not a historical chronology. It is an order for current readers. She places the catalog extracts before the fragments so that the reader sees the two current labels before reading the older papers.

Nera proposes putting the map fragments first because they are what a traveler finds. Hara agrees that the packet could use a separate field order. They make two tabs: “route reading” and “source reading.” The plan should represent these as section headings in a single document, not a second filing system. The dialogue makes clear that the same papers can be arranged for different tasks without changing their content.

## 57. Scene: the paper that stayed dry

A small corner of the chart remains dry beneath the sleeve. The player can inspect the boundary where the paper protected the ink. The stain follows the sleeve edge, not the chart’s fold. Hara says this shows that the sleeve was on the page when the water reached it, but it does not date the exposure.

Nera says the dry corner makes the paper seem preserved on purpose. Leth says the sleeve could have been placed there for ordinary storage. No one can say why it was kept.

The player may write:
- “Sleeve protected a corner from water.”
- “Paper was deliberately preserved.”
- “A dry corner survives.”

Only the first and third are supported by what is visible. The second claims intent. The archive’s final note uses “sleeve protected” as a description of effect, not purpose.

## 58. Scene: two hands on one card

The cover card has two sets of pencil pressure. One heading is slightly indented from writing on top of a hard surface. The second appears to have been copied on a softer backing. This may indicate two separate writing moments, but it does not prove two people.

Leth says:

> “A hand can change its pressure when the table changes. A person can change their hand when they are tired.”

The player can compare the strokes in a magnified illustration if the current UI supports inspection, or simply hear this line. No fingerprint system or forensic skill is added. The card does not provide character authorship.

If asked whether the heading was added later, Leth answers:

> “Later than the paper beneath it? Probably. Later than the first heading? I cannot tell.”

This is a calibrated statement about visible layering, not a mystery reveal.

## 59. Scene: one of the sources may be a copy

Evin spots that the catalog extract is a current printed copy, while the coast-zone description appears as a transcription from a damaged-map record. Their physical pages are both new, but their source types differ.

**Evin:** “Two fresh sheets do not mean two independent old records.”

**Hara:** “They are copies of two different current data entries.”

**Evin:** “That is what we know. If one old heading came from the other, we do not have that part.”

The player asks whether the two catalog descriptions might have been authored by the same writer. Hara says the current files do not identify authors. The story does not need that answer to complete its task.

This scene should not speak to the player about source code. In the game, characters speak about copies, entries, and records. In this planning document, the distinction is a design constraint: the game may expose catalog text, but it does not expose implementation internals.

## 60. Scene: an old route argument

Nera remembers a disagreement at the board: one traveler called the place a relay, another asked for the shelter. They had different destination names and nearly left in opposite directions. Nera intervened by showing them the same map symbol.

She does not remember their names or whether they reached the site. The incident is not a catastrophe. It is a minor confusion that made her reluctant to keep a single caption.

**Nera:**

> “They were both looking at the same square. One said, ‘That is not where I’m going.’ The other said, ‘It is exactly where you’re going.’ They were arguing over the noun.”

The player can ask how Nera settled the argument. She says she read the two route cards aloud and they recognized the shared reference. This anecdote justifies her present request without claiming a new route was discovered.

If the player asks whether she remembers which person was right, she replies:

> “They were both right about the name on their paper. I do not know whether either paper was right about the place.”

## 61. Optional encounter: the traveler with the wrong expectation

An optional traveler arrives expecting a dry room. They have no injury and do not require a rescue sequence. They say a friend told them the shelter was still supplied. If the player has already chosen to preserve the rumor as unverified, Nera can point to the note. If not, she asks the player to explain what the records actually say.

The traveler’s reaction is plain:

> “I thought ‘shelter’ meant a room. I did not think about the name coming from a file.”

They decide not to continue on the strength of the rumor. The plan does not tell them whether to travel or forbid them. They check their own route authority and leave the board.

This is not a punishment or a failure state. It is a small demonstration that accurate wording helps people make their own decisions. The scene should be optional and should not be used to frighten the player into choosing one caption.

## 62. Optional encounter: the patient reader

A cartographer named Seli Mar looks at the chart and asks why the trench is labeled “intake.” They specialize in water routes and wonder whether the installation was designed as part of a larger system. Leth says the source establishes a trench for intake lines, not the larger system.

Seli asks if the manual’s bypass timing supports the connection. Sima replies that the manual identifies Lock Gate Four but the chart does not name the gate. Together, the sheets make a plausible route question. They do not settle the question.

Seli is satisfied with that distinction:

> “Then the right thing to draw is a line from the question mark to the page number.”

No new map line is drawn between locations. The phrase is figurative: preserve the cross-reference so the next reader can inspect the same sources.

## 63. Optional encounter: the old shorthand

Mara Hedd sees “reserve shelter” in the catalog extract and says her old office used “reserve” to describe a room kept empty for a night shift. She does not know if that was the same usage here.

If the player asks whether the site could have been a resting room for relay staff, Mara says:

> “It could. A heading tells you what the form expected, not what the room did every day.”

She remembers one place where a “contingency room” stored filing boxes and another where people slept during a storm. Her examples are from unrelated buildings; they establish that institutional labels can be broader than their everyday use, not that this shelter had those functions.

The optional encounter enriches the phrase “contingency shelter” without resolving the setting mismatch. It should be removed if continuity review identifies an existing, more authoritative definition of the term.

## 64. Optional encounter: the map dealer’s mark

Pava Rill remembers a mark like the short-barred square on an old coast map. On that map, it meant “survey point,” not “relay.” She cannot guarantee that the mark family is the same; the line weight looks similar, but the sheet is from another office.

The player may ask to see Pava’s map. She has only a tracing on cloth, missing its title strip. The tracing cannot be added as an authoritative map fragment without review. It is optional dialogue and does not reveal a new location.

Pava says:

> “If this is the same mark, it says where someone measured. If it is not, it says how much a person can make one line carry.”

Leth replies:

> “That is a good sentence. It is still not a legend.”

The exchange is slightly dry, not a joke at either character’s expense. Pava leaves her tracing with her own records.

## 65. Optional encounter: a repairer’s interpretation

Orren Dall studies the phrase “automated contingency shelter” and imagines a generator room, emergency lighting, and a hand-cranked radio. He immediately admits that he is describing what the phrase makes him picture, not what the site contains.

**Orren:** “I hear ‘automated’ and I imagine a panel.”

**Player:** “Does the record say there is one?”

**Orren:** “No. I was drawing while I spoke.”

Orren’s small sketch is labeled “guess.” Hara advises that it not be filed with the source packet because later readers may miss the label. The player may leave it in Orren’s notebook, where it belongs. This detail lets the story show imagination without allowing it to masquerade as evidence.

## 66. Branching responses: what did the player find?

When the player returns, the summary dialogue can take one of three paths.

### If both map fragments were inspected

Nera asks whether the chart and manual fit together. The player can say:
- “They cover related coast work, but do not establish one complete route.”
- “The chart names an intake trench; the manual names Gate Four.”
- “They were stored in one sleeve, and that is all I can confirm about their association.”

Each answer opens the same route-note scene. The first is interpretive but bounded. The second is a direct summary. The third is the most conservative.

### If only the chart was inspected

Nera asks about the lock timing. The player answers:

> “I did not read that page.”

The story does not award a false composite summary. Hara files the chart alone and leaves the note “manual not reviewed.”

### If only the manual was inspected

Nera asks whether the trench marks the way to the site. The player answers:

> “The chart is still unread.”

Sima adds that a manual cannot locate a trench by itself. The player can return to the packet later if the current expedition flow supports revisit. If it does not, the story can mark the archive summary as incomplete and still close the chain’s existing objective.

### If the player inspected neither fragment

The cipher route can still resolve under the existing chain. Nera says:

> “The location name came through. The source papers are still waiting.”

The narrative plan does not secretly require both fragments to complete the cipher.

## 67. Branching responses: how much to write

The player selects one of three note lengths:
- **Card:** one route label with one source distinction.
- **Cover:** a short paragraph with both catalog descriptions and the warning about historical tide language.
- **Journal:** a longer account that includes witness limits and the missing legend.

All versions contain the same base facts. The difference is how much context the player carries forward. No outcome grants a bonus or removes a future location. The choice can be represented by a single dialogue answer, not a persistent branching database.

If a later system needs one canonical quest resolution, all three options can call the same existing resolved action. The player-facing text records the chosen version in an existing quest log only if that owner already supports choice summaries. Otherwise, display the selected wording in the completion dialogue and store no extra state.

## 68. Branching responses: which label appears first

The player chooses “shelter first,” “coast relay first,” or “neutral coast installation.” The sequence is designed so that each title remains attributed to a source.

The front-label choice should not change:
- the destination ID;
- the route;
- encounter probabilities;
- item categories;
- map reveal;
- cipher state;
- any faction relationship;
- any time, tide, or hazard variable.

It changes the order of two names in a piece of authored text. The player’s choice matters to readability, but it is not a disguised systems decision.

### Shelter first

Nera uses the current location display name as the heading and cites the coast-zone description beneath it.

### Coast description first

Nera uses the Deep Coast Saline Shelf as the heading and lists the location display name below it.

### Neutral heading

The card reads “Coast installation, see attached source records.” Both names appear in the archive cover.

## 69. Branching responses: how to treat the rumor

The player may remove the claim of a stocked room, or preserve it as an unverified rumor. The source annotation remains clear in either case.

- **Remove it:** Rell says the claim has no cited source and agrees to leave it off the route board.
- **Mark it unverified:** Rell writes “unverified report of stored supplies; no source located” on a private archive note, not as an expedition promise.
- **Leave the board unchanged:** the rumor is not repeated by the player, but no new correction is published.

The plan should avoid presenting rumor removal as the only ethical choice. A useful archive can preserve that a claim circulated while making clear that it has not been verified. A field card can omit it because it is not route information.


## 70. Ending A: the source packet remains open

In the most detailed ending, Hara keeps both labels visible on the cover and leaves the relationship line blank. The blank is not a puzzle slot. It is an honest field that current evidence cannot fill.

The player reads the final card once, then turns it over:

> CURRENT LOCATION ENTRY: Dead-Drop Command Shelter; described as an automated contingency shelter.
>
> DEEP COAST SALINE SHELF ENTRY: same mapped location ID; described as a submerged maritime relay station past tidal locks.
>
> SURVIVING FRAGMENTS: Bathymetric Sounding Chart; Tidal Lock Manual for Lock Gate Four.
>
> RELATIONSHIP BETWEEN DESCRIPTIONS: not established by these records.
>
> ACCESS NOTE: historical low-slack wording is not current route guidance.

Nera says it is too long for the public board but right for the sleeve. Hara closes the packet only after the player checks that the pages remain in the same order. The scene ends with Jori copying the shorter front label, not with a discovery or reward animation.

This ending is best for an archive or journal presentation. It does not imply that the question will be answered in a sequel. The text remains open to future evidence without promising it.

## 71. Ending B: the compact field card

In the compact ending, the player chooses a readable front line and keeps the longer source note folded into the sleeve.

> DEAD-DROP COMMAND SHELTER
>
> Coast-zone record also describes a submerged maritime relay station. See source sleeve. Historical tide notes are not current access guidance.

Nera reads it under the lamp and says:

> “I can fit this beside the square.”

Jori asks if the small copy needs both names. Nera says that it does, but the front line can point to the sleeve for the rest. The route-board symbol remains under the existing map authority. No new route is added.

At the next visit, the card is still there. Its edge is worn where a traveler turned it over to read the reverse. There is no scripted callback if the implementation cannot preserve the card’s visual state; a static completion line can carry the same effect.

## 72. Ending C: the player declines to publish a card

The player leaves the route board untouched. Hara makes an archive cover with the attributed descriptions, and Nera declines to attach the draft to a field route.

**Nera:**

> “I would rather leave a blank space than make the page sound more certain than it is.”

**Player:** “Does that help the next traveler?”

**Nera:** “Not by itself. The map still has the location. The archive now has the words.”

The response acknowledges the tradeoff. A map note can make information easier to use; an archive note can preserve nuance. The player need not pretend one form serves both purposes. The quest resolves through the existing chain owner; the map remains exactly as it was.

## 73. Ending D: the player keeps a preferred label

A player may believe the location name is the right practical heading even though the relation is unresolved. The ending lets them make that choice without recasting it as a factual verdict.

The front reads “Dead-Drop Command Shelter.” On the reverse, the coast-zone wording is attributed in full. Nera says:

> “You picked the name the map already uses. The other wording stays with it.”

The choice is a matter of navigation. It is not an assertion that the location catalog’s architecture is more historically accurate than the zone record’s. If a future writer changes the heading, the source note remains the stable content.

## 74. Ending E: the player leads with the coast record

The player may lead with “Deep Coast Saline Shelf” because it describes the regional route. The location’s display name appears beneath it, introduced as the current location entry’s label.

Sima approves the ordering:

> “That tells a runner which sheet to find. It does not promise what is past the lock.”

Nera worries that travelers who know the destination by the shelter name will not recognize it. Jori writes both on the same card, with the map symbol between them but no equivalence slash. The player can choose to keep a plain separator line.

The ending is not a different map or route. It merely lets the player decide which source name will be easiest to search.

## 75. Completion scene: the next copyist

After the player closes the file, a new copyist named Aman Tey begins preparing a clean sleeve. They ask whether to transcribe the names exactly or “correct the mismatch.” Hara gives them the original cover card.

**Hara:** “Copy what it says. Then add what we know.”

**Aman:** “Should I write that the names are the same place?”

**Hara:** “Write that the current entries point to the same map location. The rest is still a question.”

Aman starts the line, pauses at “same,” and looks to the player. The player can say:
- “Same map location.”
- “Same current entry.”
- “Same source packet.”

Each wording has a different scope. The first references the map node; the second the data key; the third the physical filing. Aman uses the player’s selected phrase and signs their own copy date. The new date is present-day archive metadata, not a fictional pre-war date.

## 76. Completion scene: closing the radio register

Evin returns the two broadcast copies to their sleeve. The shorter copy is placed behind the complete transcription. They leave the record open to the line “No answer heard between copies.”

The player can ask whether the standby carrier should stay on the board. Evin replies:

> “Keep the recording. Take the instruction off the route card.”

The archive distinguishes a transmission from a travel direction. If the player suggests that the phrase still sounds like an order, Evin says:

> “It sounds the way it sounds. The note can tell the reader what was recorded.”

The carrier’s final noise fades without a second voice. The scene does not reveal a hidden speaker or trigger a cliffhanger. The radio register closes because the player has copied the transmission accurately.

## 77. Journal entry: “Two descriptions”

**Proposed journal entry, short:**

> The location catalog calls the destination the Dead-Drop Command Shelter, an automated contingency shelter. The Deep Coast Saline Shelf record describes the same mapped location as a submerged maritime relay station. The map fragments name an artificial trench for intake lines and a Lock Gate Four bypass manual. The records do not explain how the two descriptions relate.

**Proposed journal entry, longer:**

> The Last Rotation chain revealed the existing map entry for Dead-Drop Command Shelter. Two surviving records attach different descriptions to that location: the location catalog calls it an automated contingency shelter, while the Deep Coast Saline Shelf record calls it a submerged maritime relay station past tidal locks. The Bathymetric Sounding Chart fragment describes a pre-war artificial trench for intake lines. The Tidal Lock Manual concerns bypass timing at Lock Gate Four.
>
> Leth Varo recognized a copyist’s mark but could not identify the chart’s author. Sima Dorr remembered low-slack timing as a work phrase for one crew but did not confirm the manual or a current route. Evin Rusk verified that the standby broadcast repeated; they could not identify a current operator. The source packet preserves both names. Their relationship remains unresolved.

The journal version should not claim that all witnesses were met unless the current narrative system supports conditional entries. If it does not, use the short version by default and keep witness memories in dialogue.

## 78. Journal entry: “A phrase on the coast”

An optional journal entry appears if the player speaks to Sima:

> Sima remembers “low slack” as a phrase used for a crew’s work check. She does not know whether the surviving manual is the one her crew used. Her memory does not give a present-day tide reading or authorize an entry.

If the player only reads the manual, the journal must not attribute Sima’s view. A source-only version reads:

> The Tidal Lock Manual fragment includes hand annotations about Lock Gate Four bypass timing. No full timing table survives in the fragment description. Historical wording is not a current access instruction.

These entries can be selected by the existing narrative system, but the plan does not require new persistent flags. If conditional note generation would require a new owner, use the dialogue itself and omit the variant journal entry.

## 79. Journal entry: “The broadcast”

> Evin Rusk’s register records a repeated standby carrier under the phrase “Last Rotation.” The transcription asks for an index before routing. Evin can confirm the wording and the repeated interval, but not whether a present operator heard the response or what the word “last” refers to.

The player may add a personal line:
- “The gap sounded like a pause for an answer.”
- “The repeat sounded scheduled.”
- “I cannot tell from the copy.”

All are subjective journal observations, not changes to the objective report. The archival summary stays neutral. The player’s diary can record impressions without turning them into canon facts.

## 80. Journal entry: “Same shelf code”

> The two current records use the same location ID. That establishes how the current map joins their descriptions. It does not identify the old office that wrote either heading or prove what functions the site held.

The phrase “current records” matters. A later content implementation should avoid an in-world term like “ID” unless that is already diegetic. The character-facing equivalent is “the same shelf code” or “the same map entry.” The technical ID belongs in this planning document and in implementation notes, not necessarily on the player’s screen.

## 81. Journal entry: “What I did not verify”

A final optional reflection can list the limits as plain statements:

> I did not verify that the shelter is dry.
>
> I did not test the lock bypass.
>
> I did not identify the radio operator.
>
> I did not establish that the chart and manual were drafted together.
>
> I did verify that the current records assign both descriptions to the same mapped location.

The player does not need to select every line. This can be a single short paragraph. The list should not frame restraint as heroism. It is simply a record of what the visit did and did not establish.

## 82. Readable environmental text

The following signs and notes can appear in the location scene if supported by existing readable-text presentation. Their placement must be validated against current scene geometry before implementation.

**Sleeve tag:**
> COAST PAPERS — KEEP TOGETHER

**Work-board corner:**
> Low mark entered by the crew on duty. See full register.

**Manual cover:**
> Gate Four bypass notes. Copy only; verify against current reading.

**Unlabeled hook beside the sleeve:**
> Spare cord

**Catalog extract cover:**
> Current location entry. Do not substitute for route data.

The last line is aimed at the archive reader, not an omniscient game system. If the content tone calls for a more diegetic phrasing, use:
> Route card is separate. Ask the board keeper.

No sign says “danger,” “safe,” “do not enter,” or “open at low slack” unless the current gameplay authority already supports that instruction. The story avoids inventing a hazard warning.

## 83. Arrival prose variants

**Dry weather:**

> The paper sleeve sits under a shallow eave. Salt marks the lower boards in a thin white seam. The knot has tightened around the peg, and the two headings remain readable where the sleeve turns inward.

**Wet weather:**

> The route board is damp at the corners. Someone has wrapped the sleeve in waxed paper and tied it twice. A fold covers part of the second heading. The player can lift the fold without tearing it.

**Night or low visibility:**

> The two headings are easier to read under the task lamp. The pencil note is not: the graphite has faded into the paper. Evin reads it aloud from the transcription already in the register.

These variants describe presentation only. They do not make weather change route access, tide, danger, or the player’s ability to reach the location. If the scene cannot vary by weather in the existing runtime, use the dry version as the canonical passage.

## 84. Departure prose variants

**With the concise card:**

> Nera clips the new card beside the existing route mark. It is small enough to read without moving the map. The reverse is protected by the sleeve.

**With the full archive note:**

> Hara places the long note at the front of the source packet. The route board stays as it was. When the sleeve closes, both descriptions remain visible through the two paper windows.

**With no new card:**

> Nera leaves the route mark untouched. The source packet has one more cover sheet and the same two names. She ties the sleeve to the peg with the old double knot.

Each ending shows a concrete result. None grants supplies, changes a faction relation, opens a gate, or alters the location’s map visibility.


## 85. Dialogue order and skip behavior

The narrative can be experienced in a different order without breaking its meaning. A player may hear the transmission before meeting Evin, inspect the chart before the manual, or talk to Nera before finding the forwarding slip. Every scene restates only the minimum needed to understand its own exchange.

If the player meets Nera first, she describes “two names on the route board” without claiming to know the source. If the player reaches the map fragments first, Nera later asks where the destination wording came from. If the player speaks with Sima before reading the manual, Sima describes the general phrase and explicitly says she has not seen the page. If the player reads the manual first, the conversation becomes a response to its wording rather than a clue to its content.

No required conclusion is locked behind an optional witness. The player can finish with only the current catalog, the two fragment descriptions, and the existing cipher chain. Optional dialogue adds perspective and texture, not a hidden correct answer.

## 86. Conversation menu: Nera

The route-board conversation can use the following menu:

- “Why do people use two names?”
- “What do you need from the site?”
- “Can the card fit both descriptions?”
- “Should one name be removed?”
- “Who wrote the old route note?”
- “I have nothing to add.”

**Why do people use two names?**

> “Because the papers came from different desks. I do not know whether the desks saw the same room.”

**What do you need from the site?**

> “A note that will not send someone looking for a dry room because they saw the word shelter.”

**Can the card fit both descriptions?**

> “The front can fit a label. The sleeve can fit the sources.”

**Should one name be removed?**

> “Not while the other is still on the page.”

**Who wrote the old route note?**

> “The handwriting is close to Leth’s copies. Leth says it is not theirs.”

**I have nothing to add.**

> “Then the packet stays as it is. That is still a choice.”

The menu can be displayed in a standard dialogue panel. It does not require new UI. “Nothing to add” is a valid close option at every step.

## 87. Conversation menu: Sima

- “What did low slack mean on your route?”
- “Did you use this exact manual?”
- “Could the word hold refer to the water?”
- “Was the lower board an entrance?”
- “What should a modern note say?”

Sima’s answers stay within memory:

> “It named a work check on one crew’s route. I do not know if this sheet is ours.”

> “I cannot identify it. The handwriting is not mine.”

> “It could. I would need the missing headings to be sure.”

> “We had several lower boards. I cannot tell which one this page means.”

> “Write the source phrase. Write that no current reading is included.”

If the player asks the same question twice, Sima can respond with a short line rather than repeating the full explanation:

> “That is still as far as I can take it.”

This variation keeps the character from sounding like a locked FAQ.

## 88. Conversation menu: Evin

- “Was the voice alive?”
- “Did the broadcast repeat?”
- “What does last rotation mean?”
- “Was someone supposed to answer?”
- “Can I copy your note?”

Answers:

> “The voice was spoken. I cannot tell you whether its speaker was alive when you found the recording.”

> “The register has two copies at the same interval.”

> “I do not know. The phrase is not expanded on the page.”

> “The transcription does not say. A gap is not an answer.”

> “You can copy the words. Keep the date field blank where the page is blank.”

The first answer must not imply any supernatural or digital presence. It describes the ordinary uncertainty of a recorded voice’s age. If existing canon identifies the speaker, replace the line to match that canon.

## 89. Conversation menu: Leth

- “What does the square mean?”
- “Do you recognize the handwriting?”
- “Could the fragments share a route?”
- “Why keep two names?”
- “What should I copy?”

Answers:

> “It is a location mark on this sheet. The legend that would explain it is missing.”

> “I recognize the copy style, not the writer.”

> “They may. The surviving pages do not draw the connecting line.”

> “A name can help find a page. Removing it does not make another page clearer.”

> “Copy the source heading, then add your note beneath it.”

Leth’s dialogue makes them useful without elevating them into the final authority. They are a skilled copyist who knows where their expertise stops.

## 90. A shorter ending exchange

For games where the narrative interface supports only a brief completion conversation, use:

**Nera:** “Which name goes on the card?”

**Player:** “Both. One from the location entry, one from the coast record.”

**Nera:** “And the tide note?”

**Player:** “Historical wording only. No current reading.”

**Nera:** “Then the card can travel.”

Hara puts the source sleeve behind it. The task is complete. The question of the site’s original relationship remains open without becoming the player’s burden.

## 91. A longer ending exchange

For a text-forward journal scene:

**Nera:** “You found the shelter?”

**Player:** “The location entry says shelter. The coast record says relay.”

**Nera:** “Same place?”

**Player:** “Same mapped entry. The papers do not explain more.”

**Hara:** “That is a sentence we can file.”

**Nera:** “It is not a very satisfying sentence.”

**Hara:** “It is the one we have.”

**Jori:** “Can I copy it exactly?”

**Player:** “Yes. Keep the two source names.”

Jori reads the line back. The words sound ordinary when spoken. That ordinariness is important: the story does not turn the choice into a dramatic revelation.

## 92. Optional callback lines

After completion, each character may have one short callback:
- Nera: “The front card is short. The sleeve has the rest.”
- Leth: “The clean copy keeps the old blank.”
- Sima: “No one here has a current tide reading.”
- Evin: “The carrier ended where the register says it ended.”
- Hara: “Both headings are still on the page.”
- Jori: “I left room for a later source.”
- Rell: “I took the stock claim off the board.”
- Pava: “I kept my tracing with my own map.”
- Mara: “The word ‘reserve’ still does more work than I trust it to.”
- Orren: “I labeled my sketch as a guess.”

Callbacks are one line, not daily chatter loops. If the runtime lacks quest-completion dialogue variants, the lines can be omitted without loss.

## 93. Flag and content ownership notes

The plan uses current state ownership. It does not request new mutable data:
- the Last Rotation chain remains owned by CipherQuestChainEngine and its existing save/restore path;
- the map reveal remains the existing target-location reveal;
- the radio event remains under the current broadcast content and host route;
- map-zone fragments remain catalog-authored data;
- journal text remains within the existing journal/content path;
- dialogue remains in the existing dialogue presentation owner;
- route note presentation, if supported, remains within the current map or journal owner;
- the player’s choice of wording is not persisted unless the current narrative owner already stores it.

Before implementation, the content owner must inspect whether the chain’s “resolved” state is currently persisted and what action marks it. This plan does not call that API by name beyond the state and engine found in source. No panel may become a new authority over location reveal or quest completion.

## 94. Promotion audit: evidence to recheck

Before promoting this story from proposal to implementation, a narrative/content owner should recheck:

1. the current location row and exact display name for loc_deaddrop_command_shelter;
2. the current Deep Coast Saline Shelf row, including the hidden-installation description;
3. whether the two descriptions are an intentional dual-use site or an unresolved data conflict;
4. current fragment text and any fragment consumer that presents it to the player;
5. current map-node discoverability and the effect of CipherQuestChainEngine’s Discover call;
6. whether the unique survivor-map collectible independently reveals the same location;
7. the current questline synopsis and its “microfilm index cylinder” wording;
8. the chain definition’s item_archive_index_cylinder requirement and all consumers of that ID;
9. the actual broadcast and cipher-station content for Last Rotation;
10. the current coast waystation quest’s Lock Gate Four wording;
11. current Dead Hand Core canon and the distinction between the two locations;
12. all proposed character names against existing survivor, faction, quest, radio, and item catalogs.

If the shelter/relay descriptions have since been reconciled, the plan should be rewritten to match the current authority before any content is created. If a current source identifies the operator, name, or intended function, use it. If the key item mismatch is already resolved in the data, use the resolved display name and remove the obsolete mismatch note.

## 95. Acceptance criteria for the narrative draft

The future implementation is narratively acceptable when:

- the existing Last Rotation chain is the only cipher progression involved;
- the broadcast is presented as recorded content, not a responsive character;
- the two location descriptions remain correctly attributed;
- the shared map entry is not treated as proof of a shared original function;
- the chart and manual stay distinct sources even when stored together;
- historical tide wording is not presented as current access guidance;
- the player does not operate Lock Gate Four as part of this story;
- no room, inventory, or machine behavior is invented as canon without a source audit;
- witness limits remain visible in dialogue or journal prose;
- the final card or archive note is useful and accurately scoped;
- each optional witness can be skipped;
- no moral score or mechanical reward is attached to the wording choice;
- the story can close without claiming that the unresolved site description has been solved.

The work succeeds if a player can later answer “which record called it a shelter?” and “which record called it a relay?” without mixing the two.

## 96. Content acceptance for a playthrough

During a read-through, a reviewer should be able to follow these facts without consulting this plan:

- The current location entry names a Dead-Drop Command Shelter.
- The coast-zone description calls the same mapped location a submerged maritime relay station.
- The map-zone fragments mention an artificial intake trench and Lock Gate Four.
- A shared map entry is a current data fact.
- The source relationship remains unproven.
- A prior copyist may have noticed the differing headings, but the writer is unknown.
- The standby broadcast repeats; no living responder is confirmed.
- Low-slack language belongs to an old manual and one witness’s work memory, not to current travel instructions.

If a playtester concludes that the shelter definitely contains a working relay station or that the player can safely enter at a calculated tide, the content has overclaimed and must be revised.

## 97. Continuity boundaries

Do not merge this story with the larger Dead Hand Core storyline merely because both use the phrase “dead hand.” The existing Last Rotation chain identifies a target called Dead-Drop Command Shelter. The Dead Hand Core is a different location with its own UXO register and broader narrative. The plan does not establish a direct operational connection between them.

Do not use the Dead Hand Core’s status to suggest that this shelter is awake, watching, or trying to communicate. A repeating carrier is not evidence of awareness. The cipher engine responds to player-owned state: hearing the broadcast, acquiring the required item, decoding, and revealing the map location. Those states belong to the existing engine, not a machine actor in the fiction.

Do not turn the shelter into a fifth faction base, a new military command center, or a hidden civilization. The catalog’s mention of classified directives and technical relics supports recovered records and salvage content under existing owners. It does not imply that a crew survives inside.

## 98. Continuity boundaries for the coast

The fragment description of a trench for intake lines and the manual’s Gate Four bypass timing should not create an implied modern water supply. The plan does not say the trench carried potable water, supplied a settlement, or connected to current utilities. It does not identify the intended source or destination of the intake lines.

The existing coast waystation quest says a brine-scaled bypass at Lock Gate Four is frozen and asks for maintenance materials. This story’s historical manual does not solve that quest, repair the valve, or reveal a new material requirement. If the two content packages are ever promoted at the same time, their route logic and chronology should be reviewed together. Until then, keep this plan’s text archival.

Do not add a navigable tide table. A single phrase like “low slack” cannot support a safe travel schedule. If existing simulation data later supports tide and access, that system’s owner must define the rule. This story can refer to the words on the page without turning them into an operating procedure.

## 99. Continuity boundaries for identity and privacy

The surviving pages do not name a radio operator, lock worker, map drafter, shelter occupant, or copyist. Proposed NPCs in this plan are present-day readers and former workers with limited memories; they are not secretly the authors of unsigned documents unless a later authority establishes that connection.

Do not identify the voice on the carrier with a named character without current evidence. Do not use an archival code or smudged signature to reveal a hidden identity. The story’s emotional content comes from ordinary work left unfinished, not from discovering that an unknown person was secretly the player’s relative or a major faction leader.

If current canon provides a name or witness account, it overrides the invented anonymous space in this proposal. Any supporting character name added in implementation must be collision-checked against the live data authority.

## 100. Sensitivity and portrayal

The plan includes archival uncertainty and obsolete industrial infrastructure. It does not depict a drowning, industrial accident, trapped survivor, or disaster scene. There is no need for graphic injury, panic, or a rescue timer. If environmental art contains dangerous water or damaged machinery, keep the player’s interaction within existing hazard rules and do not invent a scripted near-drowning to intensify the scene.

The radio voice is not a ghost. No spectral sound, impossible reply, or voice recognition effect is appropriate. The story may let a player feel the oddness of a repeated voice, but characters maintain a grounded explanation: a recording can repeat after its speaker has left.

No real-world flag, military logo, nation, conflict, or copied map is used. All fragments should be original game documents consistent with the project’s restrained fictional world.

## 101. Accessibility and reading order

The content should be readable without relying on color alone. The two map fragments use titles and text labels as well as line differences. The “shelter” and “relay” headings are distinguished by written attribution, not red and blue ink.

The full transmission should have a transcript. The source card should have text content available to screen readers if displayed as an image. Dialogue options should be keyboard- and controller-navigable with visible focus. Long notes should wrap at the game’s current text width without hiding the final qualification.

If the player skips audio, the transcript still provides every fact needed for the chain. If the player skips optional dialogue, the journal gives the basic difference between the two source descriptions. No color, sound cue, animation, or timed response is necessary to understand the story.


## 102. The final walk back

The player leaves the coast with no new route token, no key, and no promise that the shelter can be entered. The map entry remains where the existing cipher chain placed it. The player carries a copy of the two descriptions and the words of the manual.

The walk-back passage varies by the player’s selected ending:

- With the full cover note, the packet rests flat inside a protective sleeve.
- With the concise card, the reverse is folded beneath the front line.
- With no public card, Hara’s archive copy remains in the source room.

The player may inspect the paper once more. The note’s final line is visible: “Relationship not established by these records.” Nothing appears beneath it. No new signal interrupts the scene. The chain has led the player to a place name and a file; the story ends at the edge of what those sources can support.

## 103. The last exchange at the board

**Jori:** “If another page turns up, can this change?”

**Nera:** “It should.”

**Jori:** “Then it is not finished.”

**Hara:** “The copy is finished. The record is not.”

Jori turns the card over and checks that the source sleeve is still attached. Nera knots the cord once, then leaves the second loop loose enough to untie without tearing the paper.

If the player asks whether the card is safe in the weather, Nera says she can move it inside when the board is closed. This is a mundane care detail, not a new shelter mechanic. The final image is of a paper label protected from rain, not a secret door waiting to open.

## 104. Title card and completion line

If the quest UI requires a short title, use **The Two Names at Low Slack**. If the existing quest name is authoritative and visible, do not rename the quest; present this as a chapter heading or journal entry. The current quest ID remains quest_cipher_last_rotation.

**Completion line:**

> The Last Rotation papers point to the Dead-Drop Command Shelter. The coast records preserve another name for the mapped place. Both descriptions remain on file; neither explains the other.

**Short completion line, if space is limited:**

> Two descriptions are now filed under the same mapped location. Their relationship remains unconfirmed.

The word “confirmed” may be changed to “established” for plainer language. Do not use “truth revealed,” “secret uncovered,” or “identity solved”; those phrases promise a resolution the evidence does not provide.

## 105. Small note for a future copyist

A final optional note is written by Jori on a blank margin:

> If you add a source, add its name.
>
> If you change a heading, leave the old one readable.
>
> If you do not know, say where you stopped.

The player can leave the note in the sleeve or move it to Jori’s notebook. It is a present-day instruction, not an archival artifact. The note’s author is known because Jori signs it. Unlike the older unsigned slips, this page has a clear owner and date.

Hara says:

> “That is the first sheet in the packet with a name we can ask.”

The line gives the player a small emotional release without assigning authorship to the old pages.

## 106. What the player carries forward

The story’s carried knowledge is intentionally modest:
- the cipher chain reveals the existing target;
- the current location entry and coast-zone record use different descriptive language;
- two named map fragments supply geography and a gate procedure;
- one witness remembers a crew’s phrase, not a current tide;
- one copyist recognizes a marking practice, not a document author;
- one radio clerk confirms a repeated carrier, not a present speaker;
- a useful route note can preserve both source names without deciding between them.

The story gives the player no master key to the installation. It gives them a way to read the available material without converting a shared map entry into an unsupported architectural claim.

## 107. Closing image

Nera pins the short card beside the existing map symbol. Hara sets the long source note behind it. Jori keeps the practice copy with the square and its short western bar. The map surface shows one destination. The sleeve holds two names.

The radio register closes on the words “stand by.” The carrier has no final answer to offer. Sima folds the manual along its old crease. Leth leaves the legend blank. The next reader will have the same papers and a clearer account of what each one says.


