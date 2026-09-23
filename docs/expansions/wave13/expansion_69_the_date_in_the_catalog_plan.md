# EXPANSION 69 — THE DATE IN THE CATALOG
## A Municipal Seed Vault Story Plan
### Wave 13: The Wording in the Margin

**Primary map target:** Municipal Seed Vault, Suburban Heights  
**Location ID:** loc_municipal_seed_vault  
**Related existing content:** damaged-map fragments; tmpl_explore_seed_vault  
**Document type:** prose-first game-content expansion proposal  
**Target:** at least 120,000 characters of authored story and game content  
**Story question:** When a catalog date is remembered by a person, whose memory becomes part of the record?  
**Status:** proposed narrative content; not an implementation contract

---

## 1. Player-facing premise

A folded library blueprint shows a basement that does not appear on public records. A librarian’s note says the vault code is the date of the first seed catalog and adds: “Ask Margaret. She’ll remember.” The note gives no surname, date, or address. It does not say whether Margaret is alive now, whether the code still works, or whether the hidden basement belongs to the library described on the sheet.

The current location catalog calls the destination a climate-controlled agricultural reserve beneath the municipal courthouse basement. The Suburban Heights damaged-map record calls it a community seed bank in the basement of the old library. Both records point to loc_municipal_seed_vault. Neither explains why the building names differ.

A present-day municipal archive worker, Edda Sarn, asks the player to bring back a clear transcription of the catalog clue and any intact accession sheet. Edda has heard three versions of the “first catalog” date. One came from a faded title page. One came from a handwritten market calendar. One came from an old story about a librarian called Margaret. The player’s job is to say which date is actually printed, which date is remembered, and what the records do not connect.

The story is about the work of naming and preserving seed records, not about a new farming system. The player does not decide which community owns seed, add cultivars, run a planting simulation, or replace the existing survey content. The outcome is a careful source note that keeps the date’s wording and the building discrepancy visible.

## 2. Short pitch

The note on the Librarian’s Note fragment sounds simple: the vault code is the date of the first seed catalog; ask Margaret. The instruction assumes a reader knows which catalog, which Margaret, and which calendar. The surviving pages have lost those assumptions.

At the route table, Edda lays a copy of the Library Blueprint beside a clean transcript of the location entry. The blueprint shows a basement under the old library. The location entry places the vault beneath the municipal courthouse. The fragment name says “Librarian’s Note.” A catalog cover reads “First Municipal Seed List” and carries a date with a missing year.

Edda’s archive has heard the first name Margaret in more than one context. A separate narrative record names Margaret Webb in a ration-fraud case. No current source connects that person to the map fragment. The story refuses to make the connection by guesswork. It does not turn a database namesake into a secret librarian.

The player can preserve the date from the catalog, record the note’s instruction as written, or mark the identity and exact date format as unresolved. The ending does not leave the player empty-handed. It leaves the next reader a useful copy and a clear line between what the papers say and what they only suggest.

## 3. Evidence from current data

### 3.1 Location authority

The location record for loc_municipal_seed_vault calls it a climate-controlled agricultural reserve beneath the municipal courthouse basement. It describes dry-pack seed canisters and cold-storage lockers behind insulated airlock hatches, with pre-ashfall crop strains preserved. The row supplies current danger, travel, and radiation values. This proposal does not change those values or claim that the description has been physically reverified.

### 3.2 Damaged-map zone

The Suburban Heights zone names the Municipal Seed Vault as its hidden installation and describes a community seed bank in the basement of the old library. It says the vault is intact and that seeds may still be viable. Its two fragments are:

- **Library Blueprint:** architectural plans for the old public library, showing a basement level not present on public records.
- **Librarian’s Note:** a handwritten line stating that the vault code is the date of the first seed catalog and that Margaret will remember.

The disagreement between “municipal courthouse basement” and “old library basement” is current source evidence. The plan does not merge the buildings, decide that one was renamed, or make a secret passage connect them.

### 3.3 Expedition and dynamic quest content

The expedition catalog gives loc_municipal_seed_vault an existing exploration profile and seed-related loot categories. The dynamic quest template tmpl_explore_seed_vault is a ScoutExploration template whose description asks for a survey of the sub-basement to catalog surviving agricultural cultivars. Its target is the Municipal Seed Vault.

The template’s presence does not, by itself, prove current reachability or runtime activation. The plan proposes narrative wording that could complement that existing authored request after the route is verified. It adds no crop type, seed item, reward, seed viability rule, or new agricultural owner.

### 3.4 Other seed-vault content is separate until proven otherwise

The workspace contains other seed-related locations and story data, including loc_seed_vault and loc_seed_library_annex. The existing standing-record quest quest_record_seed_bank_purge_trace targets loc_seed_library_annex and describes a specific recovered seed case. Narrative data also records a different seed-vault recovery with a finite yield and a later sealed state. Those references use different location IDs.

No source presently cited here equates those locations with loc_municipal_seed_vault. This plan does not import their characters, casualty events, recovered quantities, sealed-vault outcome, or crop history into the Municipal Seed Vault story. A promotion audit must preserve that separation unless the current authority establishes a link.

## 4. Scope and non-goals

This proposal adds authored documents, dialogue, journal text, a return scene, and a source-conscious interpretation of the catalog date. It does not add:
- a seed inventory, seed viability simulation, cultivar catalog engine, germination timer, or farming rule;
- a vault keypad, code-entry mechanic, access-control system, or alternative map reveal;
- a new location, map node, route, seed item, crop type, or harvest reward;
- a second archive or agricultural authority;
- a new identity for Margaret Webb or a claim that the map’s Margaret is the same person;
- a new seed ownership dispute, faction relationship, or community standing calculation;
- an assumption that the vault’s contents remain viable or recoverable;
- an explanation for the library/courthouse discrepancy that current sources do not support;
- a rewrite of tmpl_explore_seed_vault’s target or reward.

Any code, access, crop, location, and save behavior remains with current owners. The story can mention the clue’s phrase “vault code” exactly as written, but it must not invent a working keypad interaction.

## 5. Theme: a date is a human object

A date can be a file key, a public milestone, a memory, or a mark used by one office to find a box. The same digits may appear on a catalog, calendar, seed envelope, and receipt without proving that all four refer to the same event.

Edda wants the date to be useful. Her work depends on being able to find the correct page again. She also knows that a date copied without a title can outlive its meaning. The first seed catalog was a public act of naming: people agreed that the varieties could be listed, compared, and requested by name. The date matters because it marks when a shared record began, not because it is a magical key.

The note’s reference to Margaret brings in memory. The plan leaves Margaret’s identity open because the fragment supplies only a first name. The player can preserve the instruction without declaring who she was. The story treats that restraint as ordinary archival work, not as a mystery solved by guessing.

## 6. Continuity boundary: courthouse and library

The location catalog and map-zone description name different buildings. Until a source audit resolves whether the two descriptions are intentional or inconsistent, the story should use source attribution every time it references a building:
- “the current location entry says courthouse”;
- “the Suburban Heights record says old library”;
- “the Library Blueprint shows a basement on its copy”;
- “the relationship between these building names is not established.”

Do not write that the library became the courthouse, that the courthouse annex was once the library, that one building has two basements, or that a tunnel connects them. Those are possible fictional explanations, not current evidence.

The player may still experience both descriptions as part of one mapped content entry. The narrative explains that the data associates them now, while leaving their historical relationship to a future continuity review.

## 7. Continuity boundary: the name Margaret

The Librarian’s Note uses the first name Margaret. A separate current narrative data file contains a Margaret Webb in an unrelated ration-fraud record. No current evidence links Margaret Webb to the municipal seed clue.

The proposed story must not name the map’s Margaret as Margaret Webb, make Webb the librarian, or rewrite the ration-fraud record to fit this site. The map fragment can remain attributed to an unknown librarian who addressed an unnamed Margaret. A future content owner may introduce a new character only after checking the whole narrative and survivor authority.

For this draft, Margaret remains a name on a note and a remembered point of reference. The story can be moving without granting her an unsupported biography.

## 8. Proposed cast

### 8.1 Edda Sarn — present-day archive worker

Edda maintains the municipal records shelf where the map copies were brought. She is skilled at matching titles and dates but has not visited the vault. Her job is to keep the new copy usable, not to decide which civic building the site occupies.

Edda’s mistake is treating a cleanly transcribed date as a settled date. She initially wants one number on the cover because a file needs a search key. Her arc is recognizing that a date can be copied exactly while its referent remains unknown.

### 8.2 Iven Cale — former catalog assistant

Iven remembers helping prepare a seed list at a public library before the Exchange. They do not remember a municipal courthouse in connection with that work. Their memory concerns printing a catalog cover and carrying boxes to a public table. They cannot confirm that their catalog was the “first seed catalog” named in the map note.

Iven is precise about the limits of memory. They can describe the feel of the paper, the ink smell, and the order of the tables. They will not claim a date from recollection alone.

### 8.3 Sella Orin — community grower

Sella uses seed names in a shared garden and is frustrated when a label disappears. She wants to know whether the Municipal Seed Vault has records that might identify the strains people used to trade. She does not assume the vault has viable seed or that a catalog entry means a packet remains.

Sella’s question is practical: if a name survives, can it help people understand a plant already growing? The plan does not add a new cultivar or growing system. Her arc is learning that a historical name can be preserved even when no living plant has been identified.

### 8.4 Teren Vaul — map copyist

Teren copied the Library Blueprint for the present archive. They can identify the basement line and the “not on public records” annotation, but they did not make the original plan. They are wary of drawing the courthouse outline over the library plan because the pages have no shared scale.

Teren offers the player a clean tracing with a blank building label. The blank is deliberate.

### 8.5 Ora Pell — assistant librarian in an old oral account

Ora appears in one proposed transcript as a voice remembered by Iven. Ora once told a junior worker that a catalog needed a date and a title before anyone could order from it. The transcript does not establish that Ora wrote the Librarian’s Note or knew Margaret personally.

Ora’s role is to provide the story’s strongest line about naming:
> “If the packet has a name but no date, we can admire it. We cannot ask for it.”

The sentence refers to catalog retrieval, not to a new seed distribution rule.

## 9. Supporting voices

Optional voices may include:
- **Nerin Doss, archive visitor:** remembers hearing that the first catalog was read aloud at a market table.
- **Luma Ren, former library aide:** recognizes the blueprint’s stamp style but cannot identify the building.
- **Orel Venn, seed-envelope keeper:** has an envelope with a date but no catalog title.
- **Davi Morn, bookbinder:** remembers repairing a catalog spine and cannot recall the year.
- **Kesa Harl, route-card reader:** wants the building name printed clearly before travel.
- **Pell Anor, current garden volunteer:** remembers a story told by someone called Margaret, but not whether it was this Margaret.
- **Iven Cale:** remembers the catalog work, not the exact date.
- **Edda Sarn:** knows the current archive copy, not the old building.
- **Sella Orin:** knows present garden labels, not the vault’s contents.

Each account is optional, limited, and non-authoritative. No witness owns the one answer that completes the story. If a later authority supplies a living Margaret with a verified biography, replace the anonymous references after continuity review.

## 10. Story spine

1. The existing Suburban Heights fragments point to loc_municipal_seed_vault through the current map path.
2. The player reads the Library Blueprint and Librarian’s Note separately.
3. Edda requests an accurate transcription, not a seed-recovery promise.
4. The player compares the “old library” map-zone wording with the “municipal courthouse” location entry.
5. Iven shares a memory of catalog preparation but refuses to invent the date.
6. An accession card supplies a proposed catalog date as newly authored content, clearly labeled as a source requiring verification.
7. The player preserves the exact date format and its source, without claiming a date is a working access code.
8. Sella asks what the name on a packet can tell a present grower; the dialogue separates a name from viable stock.
9. The player prepares a short index note, a fuller source record, or leaves the building identity unresolved.
10. The existing exploration template remains the only proposed exploration request; current discovery and access owners must be verified before implementation.


## 11. The proposed catalog date

The current map fragment does not provide a date. To make the story playable as authored content, this proposal introduces a **draft date** on a recovered first-catalog cover:

> MUNICIPAL SEED LIST — FIRST ISSUE  
> Public reading copy  
> Date: 18 April  
> Year field: not completed

“18 April” is proposed narrative text, not a fact found in current data. It must be compared with the project’s calendar, the actual access clue, and any current quest implementation before promotion. If a canonical first-catalog date already exists elsewhere, that date supersedes this proposal. If no date is authorized, leave the field blank and keep the story focused on the source note.

The date is written long-form so the prose does not assume whether a code expects month-day, day-month, or a year. No code string is derived here. The player may read the date and report it as a date; any access behavior remains with the current map and exploration owner.

## 12. The two buildings

The location record calls the vault a municipal courthouse basement. The damaged-map installation description calls it the basement of the old library. The Library Blueprint shows a basement level not present on public records. The map node uses one location ID.

The player can set the two source descriptions side by side:
> **Location entry:** “beneath the municipal courthouse basement.”
>
> **Suburban Heights record:** “basement of the old library.”
>
> **Blueprint:** “basement level shown; not present on public records.”

Edda says:
> “The map entry joins these descriptions. The pages do not explain whether the buildings joined.”

Teren adds:
> “I can trace the basement line. I cannot place the courthouse on this sheet.”

No architectural passage, secret tunnel, annex, or shared foundation is written as fact. If future source evidence proves the buildings are the same structure, the plan can be revised before any game text is produced.

## 13. What the note asks

The Librarian’s Note is transcribed exactly:

> Vault code is the date of the first seed catalog. Ask Margaret. She’ll remember.

The note does not name the vault’s location, catalog title, author, or recipient. The word “Margaret” is not accompanied by an initial or surname. The handwriting is not linked to an existing character.

Edda notices that the sentence combines an instruction and an assumption. It assumes a reader knows what “first” means and can still reach Margaret. She says:
> “It was written for someone who already knew the room.”

The note’s voice should feel familiar, not cryptic. It is the kind of message a colleague leaves when they expect the next shift to arrive.

## 14. Arrival at the archive table

> The blueprint lies under a stone paperweight. Someone has traced the lower floor in green pencil, then written “not on public records” beside the stair. The line stops at the edge of the page.
>
> The note is folded into the crease of the tracing. Its first line is darker where the fold flattened the ink. “Margaret” sits near the crease and has not faded.
>
> On the table, a clean catalog cover has been propped open to show the date field. The words “18 April” are written in a hand different from the note’s.

The arrival is text-first and does not require a scene inside the vault. If the current exploration flow brings the player to the location, the same passage can be adapted to its existing readable-document presentation. If the site is not currently reachable, the text remains a proposal for the future route; this plan does not add a reveal path.

## 15. Inspection: the blueprint

**Top margin:**
> MUNICIPAL LIBRARY — SERVICE FLOOR

**Basement annotation:**
> Local copy shows lower rooms not on public visitor sheet.

**Stair mark:**
> Down from staff corridor. Landing width not measured.

**Pencil note:**
> Ask before moving the index cabinet.

The blueprint does not use the words “courthouse,” “seed vault,” or “climate control.” Those details come from other records. The player may inspect the basement outline but cannot infer that a specific insulated hatch sits behind any wall unless a current scene or data asset establishes it.

Teren explains:
> “The staff floor is mapped. The basement is an annotation. I cannot tell you how many copies of this page were made.”

If asked whether the plan proves the vault is beneath the old library, Teren says:
> “It proves this copy shows a basement beneath a building labeled library. The vault name is on a different page.”

## 16. Inspection: the Librarian’s Note

The front of the note is short:

> Vault code is the date of the first seed catalog. Ask Margaret. She’ll remember.

The reverse is blank except for a line impression where another page rested. The player can inspect the pressure mark but cannot recover missing writing.

Edda says:
> “The blank side may have been blank when the note was made. It may also have lost whatever was written there. We should not write a sentence into it.”

If asked whether the note’s author was a librarian, Edda says:
> “The map calls it a librarian’s note. That is the catalog label. We do not have a signature.”

This distinction keeps the fragment’s source label intact without inventing a named author.

## 17. Inspection: the catalog cover

**Proposed cover text:**

> MUNICIPAL SEED LIST  
> First public issue  
> Prepared for the library table  
> Date: 18 April  
> Year: [blank]

Inside the cover, three faint pencil strokes remain where a title may have been written. They are not legible. A paper tab labeled “Index” is still attached to the spine.

The player can compare the date with the proposed catalog date in Section 11. If this draft cover is used, it supplies a possible source for the date. A future content owner must confirm that it belongs to the same catalog named by the Librarian’s Note; the current data does not establish that relationship.

If the player asks Iven whether the cover is from the first issue, Iven says:
> “It says first public issue. I remember a catalog table. I cannot authenticate this cover from memory.”

## 18. The date in speech

A player may ask Iven to say the date aloud.

**Player:** “What date do you remember?”

**Iven:** “I remember 18 April from the cover. I remember carrying copies after the first public reading. I do not remember whether those were the first copies.”

**Player:** “Then is it the date of the first seed catalog?”

**Iven:** “The cover says first public issue. The note says first seed catalog. Those might mean the same event.”

**Player:** “Might?”

**Iven:** “I was an assistant. I can tell you what I carried.”

The dialogue offers a source-supported candidate while retaining the difference between title and memory. The exact date remains a proposal until verified.

## 19. A present-day question about the code

Sella asks whether the date should be typed into the vault. Edda does not know how the existing access clue is implemented.

> “The note calls it a code,” Edda says. “The note does not show a keypad.”
>
> “Could the date still open it?” Sella asks.
>
> “Could be. We need the current route and access owner to tell us what the game does.”

The player can answer:
- “The date is a clue in the record.”
- “We need a verified access path.”
- “I will not assume the code format.”

The story never presents a keypad prompt, tests a code, or changes the map node. This keeps authored clue text separate from the game’s access implementation.

## 20. The old library name

Sella calls the site the “library vault” because that is what the map-zone record says. The current location catalog uses “Municipal Seed Vault.” She asks whether the shorter name should go on the garden’s request board.

Edda replies:
> “The location name is Municipal Seed Vault. We can write that the map record places it below the old library. We do not need to rename the location to preserve the note.”

The scene avoids choosing between the two building descriptions. It also gives the player a practical way to speak about the site without taking a side.

## 21. The courthouse name

A route reader, Kesa Harl, says the location entry places the vault below the courthouse and asks whether the blueprint shows a courthouse basement. Teren says it does not; the blueprint’s heading is Municipal Library.

Kesa wants to know whether the route board is wrong. Edda answers:
> “It may be incomplete. It is not our job to make the two pages agree by changing one.”

The player can choose to preserve the courthouse wording, the library wording, or both with attribution. No choice changes the location name or the world map.

## 22. First public reading

Iven remembers a table set near a public entrance. People arrived with slips of paper and asked whether the list had a bean that grew in a shaded yard, a grain that could be stored, or a plant that tolerated damp soil. Iven carried duplicate copies from a print room.

He does not remember the varieties. The story should not invent names for crops from that memory. The focus is the work of making an index accessible:
> “They did not have to agree on what to plant before they could ask what was in the book.”

The player can ask who spoke at the table. Iven says he remembers an older librarian reading the headings aloud, but not whether she was Margaret. The oral account keeps its human texture and its boundary.


## 23. Document: accession leaf

**Proposed new authored document, pending date review:**

> MUNICIPAL SEED LIST — ACCESSION
>
> Title copy received: first public issue
>
> Date entered: 18 April
>
> Year: not supplied
>
> Filing shelf: local index
>
> Public copy: one

This leaf gives the cover date an independent source within the proposed story. It still does not prove the catalog was the first seed list of any kind. “First public issue” may refer to the first copy available at the table, not the first catalog ever made.

Edda reads the title field twice:
> “It says first public issue. The note says first seed catalog. The accession leaf helps, but it does not close the gap.”

The player may include the leaf in the archive packet only if the date itself is approved. If not, retain the structure with a blank date field and do not invent a substitute.

## 24. Document: a table notice

> CATALOG TABLE — OPEN AFTER THE MORNING BELL
>
> Ask for a title before asking for a packet.
>
> The index lists names, not seed quantity.
>
> If a packet is missing, leave the entry in place and mark the copy.

Iven says the notice resembles the public table where they carried catalogs. They cannot confirm that it hung in the old library. The note’s first sentence is a proposed addition. It should be read as a general reading-room instruction, not a specific access schedule.

The third line is important: a catalog is an index, not an inventory. The Municipal Seed Vault expedition template asks for a survey to catalog surviving cultivars, while the location’s data describes sealed canisters. Neither source says that a printed name is proof a viable packet remains.

## 25. Document: the catalog title page

> MUNICIPAL SEED LIST
>
> First public issue
>
> Prepared for reference at the library table
>
> Date: 18 April
>
> This book names entries held in the municipal collection. It does not record the quantity in each canister.

The final sentence is proposed story content. It must not be treated as a current vault inventory or as proof the location record’s “courthouse basement” description is wrong. It communicates the distinction between a catalog and a stock list.

If a future author finds that the catalog’s actual schema includes an existing quantity field, revise the sentence to match the current source. Do not turn it into a universal statement about every agricultural catalog.

## 26. Document: a borrower’s paper

A small folded sheet is filled with questions rather than answers:

> For the table:
>
> Is there a grain that will take shade?
>
> Is there a bean that dries on the vine?
>
> Which packets have a written origin?
>
> Is the garden list the same as the store list?

Iven says these look like public questions from the catalog table. The sheet has no returned answer or name. Sella reads the questions and says that the person wanted categories, not a promise of supply.

The player can preserve it as an example of how people used the catalog:
> “This paper records questions a reader brought to the table. It does not identify any packet or current seed stock.”

The document adds community texture without naming crop types or creating gameplay items.

## 27. Document: a damaged index tab

The index tab has the letters A through M, then a torn edge. The lower part may once have carried later letters. Edda warns that the missing section should not be represented as proof the catalog ended at M.

**Proposed player text:**
> Index tab: A–M legible. Remaining tab is torn.

If the player asks whether the catalog contains more entries, Edda says:
> “The tab is incomplete. The catalog might be complete in another copy.”

The answer preserves possibility without inventing a full catalog or a missing-packet quest.

## 28. Document: the librarian’s penciled reminder

A separate note is found inside the proposed catalog cover:

> If the table is busy, let the reader take the index copy.
>
> Margaret has the date in her book.
>
> Return the book before the afternoon shift.

The paper’s handwriting differs from the main Librarian’s Note. It does not name the book, date, Margaret’s surname, or which afternoon shift. It may belong to the same office, but no matching mark confirms this.

Edda points out that “Margaret has the date in her book” sounds like a colleague’s memory aid, not necessarily a password instruction. The original fragment explicitly calls the date a vault code; the added note is proposed context, not a replacement for that clue.

## 29. Document: a calendar corner

The calendar corner shows a month title and several dates, but it does not say “seed catalog.” A faint circle appears around 18 April. The circle could be a reading day, a delivery, or a personal appointment.

Iven cannot confirm that this calendar belonged to the library:
> “I remember a circle. I do not remember whose calendar.”

If used alongside the proposed title page, the calendar may support the 18 April date. It does not establish the year or prove the meaning of the circle. A future content pass can omit this sheet if it makes the date look more certain than the source permits.

## 30. Document: a folded request slip

> Please save one copy of the first list for the lower desk.
>
> We can return the loan copy after the weather turns.
>
> Ask M. which date belongs on the cover.

The player may wonder if “M.” is Margaret. Edda says:
> “It could be. The paper does not expand the initial.”

The proposed note uses “M.” rather than a surname to preserve the uncertainty. The player can record that an initial appears, but no character may identify it as Margaret without another source.

## 31. Document: two cover copies

Two catalog covers are found in separate sleeves. Both carry “Municipal Seed List.” One says “first public issue”; the other says “reading copy.” One carries 18 April, the other is undated. Their paper stock differs.

Iven remembers a reading copy but not the specific covers. The player can compare title and date, but cannot prove the undated copy came first. The term “first” remains tied to the dated cover’s wording.

A concise archive line reads:
> “One proposed cover names the first public issue and bears 18 April; a second cover is undated. Their production order is unknown.”

This document makes the date useful while keeping the chronology honest.

## 32. Document: the library’s basement stamp

The blueprint has a stamp:
> MUNICIPAL LIBRARY — SERVICE DRAWING

A separate map-zone transcript calls the hidden installation a community seed bank in the basement of the old library. The location entry calls the vault a reserve beneath the municipal courthouse. The stamp supports the identity of the blueprint’s source, not the current location entry.

Teren says:
> “I can authenticate the label on this sheet. I cannot authenticate the building under the other description.”

The player may include the stamp in the summary or leave it as a close-inspection detail. It does not resolve the courthouse/library difference.

## 33. Document: a public reading transcript

The proposed transcript contains a short exchange at a seed-catalog table:

> **Reader:** “Does the list say what is in the canister?”
>
> **Librarian:** “It says what the entry is called.”
>
> **Reader:** “And the canister?”
>
> **Librarian:** “That needs another page.”

The transcript is anonymous. Iven says it sounds like the table they remember, but cannot confirm the speaker. The words are proposed game writing and should not be attributed to Margaret or any established character.

The exchange crystallizes the story’s distinction: names help people refer to records, but they do not substitute for an inventory. It should be delivered as a small document, not as a tutorial about seed systems.

## 34. Dialogue: Edda and the date field

**Player:** “Is 18 April the first catalog date?”

**Edda:** “The proposed cover calls it the first public issue.”

**Player:** “Does that match the note?”

**Edda:** “It might. The note says ‘first seed catalog.’”

**Player:** “What should the index say?”

**Edda:** “The cover says 18 April. The note refers to a first catalog. Put both sentences in the same sleeve.”

If the player asks whether the year is known:
> “Not from this cover.”

If asked whether “18 April” is enough to enter a code:
> “That is for the current access owner to decide. I can tell you what the paper prints.”

The dialogue never turns the date into a code-entry challenge.

## 35. Dialogue: Sella and a seed’s name

Sella brings a seed envelope from a present-day garden. Its label is partly rubbed away. She asks whether the catalog might help identify it.

**Player:** “The catalog could preserve a name.”

**Sella:** “A name would help me ask the next question.”

**Player:** “It might not identify this envelope.”

**Sella:** “Then I would keep looking. I would not write a name on the seed because it sounds close.”

Sella’s current envelope is not part of the Municipal Seed Vault inventory. The scene does not identify a crop, assign a new item, or grant a seed packet. It shows why names matter to a grower while keeping the two sources separate.

## 36. Dialogue: Iven and the printing day

**Player:** “What do you remember about 18 April?”

**Iven:** “The reading table had a green cloth. The stack was higher than my hand.”

**Player:** “Was it the first catalog?”

**Iven:** “I was told it was the first public issue.”

**Player:** “Who told you?”

**Iven:** “A librarian. I cannot tell you if it was Margaret.”

**Player:** “Do you remember her?”

**Iven:** “I remember a voice at the table. I remember two people asking for the same page. The name is not in my memory.”

The exchange is restrained. It should not retroactively identify the unnamed note-writer.

## 37. Dialogue: the name Margaret

The player asks Edda who Margaret was.

> “The note gives a first name. Our archive has another Margaret in another record. I have no source that joins them.”

**Player:** “Could it be Margaret Webb?”

**Edda:** “It could be the same name. That is not enough to connect the people.”

**Player:** “Should I ask her?”

**Edda:** “There is no current route or contact in this packet.”

The game should use this conversation only if the existing data allows characters to refer to the ration-fraud record. If that would expose unrelated case details or confuse players, keep the distinction in the internal continuity note and leave the player-facing answer at “the note does not say which Margaret.”

No character accuses Webb again or reframes her existing record. No new character is given the surname Webb.

## 38. Dialogue: the building names

**Player:** “Is the vault under the library or the courthouse?”

**Edda:** “One current record says courthouse. The zone record says old library.”

**Player:** “Can they be the same building?”

**Edda:** “The papers do not say.”

**Player:** “Which name should go on the note?”

**Edda:** “The location name first. The source descriptions after it.”

Teren adds:
> “I can trace a basement on the library sheet. I cannot draw the courthouse from it.”

The player has enough wording for a helpful route note without choosing a building history.

## 39. Dialogue: the catalog is not the canister

The player asks whether an entry in the catalog proves that its seed still exists.

**Sella:** “It proves someone wrote the name down.”

**Iven:** “It may prove a packet was listed when the catalog was made.”

**Edda:** “It does not prove the packet is in the vault now.”

The three speakers have distinct knowledge. Sella knows present plant labels, Iven remembers the old catalog process, and Edda knows the archive copy. Their combined answer is more useful than one omniscient lecture.

## 40. Dialogue: how to describe the date

The player chooses a phrase for the index:
- “First public issue: 18 April.”
- “Catalog date: 18 April; year absent.”
- “Date named by the catalog cover; relation to the note unverified.”

The first is simplest. The second preserves the missing year. The third is the most precise but is too long for a field card. Edda recommends using the second in the archive and the first on the short index tab if the title page is verified.

The choice alters prose only. It does not set an access code or unlock the location.

## 41. Dialogue: the date is not a story people own

Sella worries that a shared date could become a claim of ownership. Edda says a catalog date identifies a record, not a person or community.

**Sella:** “If I write that the first list began on 18 April, does that make the seeds theirs?”

**Edda:** “It makes the list dated.”

**Sella:** “And the people who saved it?”

**Edda:** “They need their own record.”

The exchange points toward the player’s final note: separate the catalog’s date, the site description, and any later custody story. It does not create a property or seed-rights system.


## 42. Dialogue: what “first” means

The player can ask four questions about the word “first.”

**First in this building?**
> “The note does not say.”

**First in the town?**
> “The cover says first public issue. It does not define the town.”

**First catalog ever?**
> “We do not have the earlier shelves.”

**First copy of this catalog?**
> “That would be a different claim.”

Edda says the phrase is useful even without an exact scope. It shows how the note’s writer thought the next person could find the date. The story does not make the word “first” a riddle with one hidden answer.

## 43. Dialogue: a remembered catalog table

Iven describes the public reading table:

> “The table was too narrow for the book to lie flat. We put a board across two trestles. People brought their own paper because there were no forms yet.”

The player asks whether Margaret stood there. Iven says:
> “Someone read the headings. I remember the sound of the page turning. I do not remember the name.”

Iven can remember an action without remembering a name. That is the emotional limit of the scene. The player may not press for more; Iven has already given what they can.

## 44. Dialogue: Sella asks for a plant name

Sella wants to know whether the player found a cultivar name that could be used by the current garden. The player can answer:
- “I found a catalog cover, not an entry list.”
- “The index tab survives only through M.”
- “The vault record describes seed canisters, but no current cultivar was verified.”

Sella says she would rather have a clean blank than a familiar name copied onto the wrong plant. She may ask the player to return if an actual list is found. This is ordinary dialogue, not a repeatable survey quest or new catalog collection mechanic.

## 45. Dialogue: Teren and scale

Teren explains that the blueprint’s basement line has no scale bar in the surviving copy. It has room labels and a stair mark, but no overall dimension.

**Player:** “Could you fit the courthouse entry over it?”

**Teren:** “Not without a common scale.”

**Player:** “Could the buildings share a wall?”

**Teren:** “The sheet does not show the courthouse.”

Teren’s job is to keep a copy from becoming an architectural plan. Their contribution is a warning against drawing a connection where none exists.

## 46. Dialogue: the damaged catalog spine

Davi Morn examines the binding and sees that the catalog spine was repaired twice. The first repair used thread; the second used a strip of paper from a different book.

**Davi:** “The book stayed in use. That is all I can say from the spine.”

If asked whether the book was opened on 18 April, Davi says a repair does not give a date. If asked whether the second repair came after the Exchange, Davi says the paper looks older than the cord but cannot be placed to a year.

The scene is about care, not forensic certainty. Davi’s craft adds texture to the catalog as a used object.

## 47. Dialogue: Nerin’s market-table memory

Nerin remembers hearing a list read at an outdoor table where people waited for their produce sacks. They recall someone calling it the “first seed book.” They do not remember a date or building.

**Player:** “Could that be the first public issue?”

**Nerin:** “It could be the same day. It could be a different list.”

**Player:** “Did you see the catalog?”

**Nerin:** “I saw the crowd. I was holding a sack.”

Nerin’s memory is a social perspective on the catalog, not corroboration of the cover. It can be included as optional dialogue, never as the date’s authority.

## 48. Dialogue: Luma and the stamp

Luma recognizes the blueprint’s square stamp as one used by municipal library facilities. She does not remember which branch. The player asks whether that proves the site was the library.

**Luma:** “It proves a library office stamped the copy.”

**Player:** “Could the courthouse have kept it?”

**Luma:** “A courthouse can keep a library drawing. Paper travels.”

This line creates a plausible possibility without resolving the building discrepancy. It also prevents the map-zone copy from becoming the only authority on the location.

## 49. Dialogue: Orel and the seed envelope

Orel’s envelope reads:
> 18 APR — TABLE COPY

No seed name or catalog title appears. Orel says it was used to hold change or small paper slips, not seeds. They found it in a box of old garden records.

**Player:** “Does it confirm the catalog date?”

**Orel:** “It confirms somebody wrote the date on an envelope.”

**Player:** “And ‘table copy’?”

**Orel:** “Could be the same table. Could be another one.”

This additional artifact may be omitted if it makes the proposed date appear too strongly supported. Its role is to show that repeated dates can travel without their title.

## 50. Scene: the request to copy the catalog

Edda asks the player to produce two copies:
- a short cover line for the route packet;
- a longer note for the archive.

The player can choose whether the short line says:
> “Catalog date: 18 April.”

or:
> “Proposed first public issue: 18 April; year absent.”

The longer note can carry the source relationship:
> “The cover calls itself the first public issue. The Librarian’s Note refers to the date of the first seed catalog. A current source audit must establish whether these phrases identify the same catalog.”

Edda is satisfied when both copies retain their different levels of detail. The short line is not a replacement for the full record.

## 51. Scene: the date is read to Sella

The player reads “18 April” to Sella. She does not celebrate. She asks whether that is a planting date.

Edda says:
> “It is the catalog’s date, if the cover is the right cover.”

Sella replies:
> “Then I will write it beside the book, not on a packet.”

This line prevents the date from migrating into a crop calendar. It also shows how a date becomes useful when its purpose remains attached.

## 52. Scene: the blank year field

The year field on the proposed accession cover is blank. The player can ask whether it was erased, never written, or left blank deliberately. Iven says he cannot tell.

**Player:** “Could we infer the year from the paper?”

**Iven:** “Someone with the right record might. I do not have it.”

**Player:** “Should I estimate?”

**Iven:** “Not on the source copy.”

The player can write a new present-day note that the year is absent. No historical year is supplied by this plan. If a current world chronology later authorizes one, the new value must be attributed to that source.

## 53. Scene: the library map in the wrong folder

Teren finds the Library Blueprint in a folder labeled “Municipal Works.” The folder also contains a courthouse elevation, but its page number is missing. The two drawings have different scales and different stamp boxes.

Edda warns:
> “A folder can hold related documents. It can also hold things that fit in the same drawer.”

The player may place the courthouse elevation beside the blueprint for comparison. No matching floor plan appears. The page can be a proposed scene prop, but it should not be used to invent an architectural link.

## 54. Scene: the first list was read aloud

Iven remembers someone reading the catalog headings at a table. The first entry was too faint, so the reader started again. The audience waited without complaint. Iven remembers a child tapping the page with one finger, not the child’s name or what the entry was.

This detail can appear as an oral memory rather than a document. It gives the catalog’s “first public issue” a small human moment: the first reading was not a ceremony but a practical effort to make a faint heading audible.

If asked whether the child returned, Iven says:
> “I do not know. I was carrying the next copy.”

## 55. Scene: Margaret’s empty line

The player asks Edda whether the name Margaret should be written into the archive index.

Edda makes a line on the draft cover:
> Name mentioned in source: Margaret  
> Identity: not established

The player can choose whether to include that line. If included, it says only what the fragment says. If omitted, the source transcription still preserves the name in full.

The archive does not fill in a surname. The existing Margaret Webb case remains unrelated unless a future canonical authority says otherwise. The plan’s new authored characters do not claim to have met Margaret.

## 56. Scene: a title copied without a title

One catalog cover has its title torn away but retains “First public issue.” Davi says the cover may have belonged to the same book as the dated page, but the binding cannot prove it.

The player can place the two covers side by side. If the player says they are the same, Edda asks what mark connects them. The player may revise the note:
> “Two covers with matching issue language are preserved together; shared binding is unverified.”

This is a mild, deliberate test of the player’s source discipline. There is no penalty for changing the wording.

## 57. Scene: a reader returns the catalog

A proposed record of a returned book reads:
> Borrowed for the table. Returned before closing. No name required.

Iven says a public reading copy might not have been borrowed at all. Edda explains that the line could describe any shared book. The player may keep it as an unassigned record or omit it from the seed packet.

This scene adds another example of the difference between custody and authorship. A returned catalog is not necessarily the one named by the fragment.

## 58. Scene: a copyist makes two headings

Teren labels one folder “Municipal Seed Vault” because that is the location name. On the second line they write:
> Location description: courthouse basement.
> Zone description: old library basement.

The player can ask whether the title should be changed to “Library Seed Vault.” Teren says:
> “That would make one source sound like the name of the place.”

The title remains as the current location display name, with both building phrases below. This preserves the current map identity while allowing the disagreement to remain visible.

## 59. Scene: Sella’s label for a living plant

Sella shows the player a seedling in a pot and its label:
> Family red bean — source unknown.

She asks whether the old catalog could identify it. The player can answer:
- “Possibly, if an entry list survives.”
- “The cover alone cannot.”
- “This plant needs its own observation.”

Sella updates her label to add “leaf shape noted today” but does not change the plant’s name. This is a personal garden action, not a new genetics or crop system. It illustrates that a source may help ask a better question without supplying the answer.

## 60. Scene: the hidden basement line

The blueprint’s basement is absent from public records, but the player’s world map already contains the vault as a location entry. Edda explains that “not on public records” describes the blueprint’s public-sheet comparison, not the player’s current map visibility.

This line is meta-adjacent and may be adapted:
> “The old public sheet did not show the lower floor. Our map has a separate location entry. Those are different records.”

The story must not say the basement is secret because of wrongdoing. “Not on public records” can mean an engineering plan or staff-only annotation. No conspiracy or political cover-up is established.

## 61. Scene: who owns the copy?

The player asks whether the catalog copy belongs to the archive, the garden, or the library. Edda says the source packet is held by the archive for reading; Sella says the garden needs a title and a reference, not ownership of the book.

Iven adds:
> “We made copies so a question did not have to own the shelf.”

The player can file the current copy with Edda and provide a reference line to Sella. This creates no trade, inventory, or ownership mechanic. It is a narrative handoff between two characters with different needs.


## 62. The catalog’s unprinted pages

Iven remembers that the public copy had additional pages, but the current packet contains only a cover, an index tab, and one accession leaf. They will not reconstruct the entries from memory.

**Player:** “How many pages were there?”

**Iven:** “Enough that we carried two copies.”

**Player:** “Do you remember what was listed?”

**Iven:** “Not the entries. I remember the page count was written somewhere.”

**Player:** “Can we use your memory to fill the index?”

**Iven:** “No. A count is not a list.”

This exchange gives Iven a practical memory and a clear refusal. No cultivar entries are invented to populate a catalog table.

## 63. The packet’s title card

Edda makes a title card for the packet:

> MUNICIPAL SEED VAULT
>
> Current location name
>
> Sources attached:
> - Library Blueprint
> - Librarian’s Note
> - Proposed catalog cover
>
> Building description discrepancy: retained as written

The card separates the current location title from its source descriptions. The player can choose whether the title card includes the proposed date. If included, it must say “proposed cover date” until the source audit confirms that it belongs to the first catalog named by the note.

This is an archive label, not a new database category. It can appear as a document in the story or be summarized in dialogue.

## 64. The route room’s seed drawer

The route room has a drawer of blank envelopes, not seed stock. Sella uses the envelopes to protect garden labels from damp. She takes one and writes:
> Catalog reference, not packet.

The line is her own modern annotation. It is not discovered at the vault and should not be represented as a pre-war instruction.

Sella says:
> “If I put a name on a packet, someone may plant it. If I put a reference on a page, someone may read it.”

The distinction is a small ethical choice: make clear when the player is handling a record rather than a viable seed item.

## 65. The date format

The proposed date appears in long form as “18 April.” A future implementation may need a numeric form if the current access or clue owner expects one. This plan does not select a keypad format.

If a source audit finds a date convention in the existing map-fragment system, use it. If none exists, keep the date in words and do not create a numeric code. If a new access mechanic is required to use it, that needs a separate authorized plan.

The game-facing story can still treat the date as a human reference even if it is not used mechanically.

## 66. The catalog’s first public issue

The phrase “first public issue” is proposed for the cover because the map note uses “first seed catalog.” The two phrases are close but not identical. The first may mean the first catalog opened to public reading, while the second may mean the first compilation.

Edda wants to write:
> “The cover says first public issue.”

The player can add:
> “The map note calls it the first seed catalog.”

The archive note preserves both phrases rather than silently normalizing them. If source review finds that the actual catalog title says “first seed catalog,” update the transcription exactly.

## 67. Dialogue: Edda and the same date in two places

**Player:** “The date appears on the cover and calendar. Is that enough?”

**Edda:** “It appears on both. We still need to know why.”

**Player:** “Could the calendar be the event date?”

**Edda:** “Could be.”

**Player:** “Should I say it was the first issue?”

**Edda:** “Say the cover calls it that. The calendar agrees on the day, not the title.”

The player can file the calendar as corroborating the day or as an unassigned paper. Edda does not call it definitive proof.

## 68. Dialogue: Iven remembers the person at the table

The player asks Iven to describe the librarian who read the catalog.

> “A sleeve rolled above the wrist. Ink on one finger. They held the page by the corner so the title stayed visible.”

The player asks if the person was Margaret.

> “I do not know.”

Iven remembers an action, not a name. The scene should not offer a portrait or gendered detail that later gets treated as identity evidence. The person at the table can remain “the reader” in the transcript.

## 69. Dialogue: a name may travel farther than a person

Edda remarks that “Margaret” appears in the note but has no context. Sella says names often survive on plant labels after the person is gone. Edda replies:
> “A name can stay readable and still stop identifying someone.”

The player may ask if the archive should delete the name. Edda says no; the source should be transcribed as found, with an annotation that identity is unknown.

The line is an ordinary observation, not a clue to the existing Margaret Webb case.

## 70. Dialogue: the year is missing

The player asks whether the proposed “18 April” can be placed on a world timeline. Edda says the title page has no year and the map fragment does not supply one.

Iven remembers that the catalog was prepared before the Exchange but cannot place which year. If the existing setting chronology allows this relative statement, the story may say “before the Exchange.” Otherwise, use:
> “The surviving copies do not give a year.”

No character calculates the date by the age of paper. No calendar system is introduced.

## 71. Dialogue: the catalog’s public table

Nerin remembers a crowd near an outdoor produce exchange. Iven remembers an indoor table by a library entrance. The two memories may describe separate readings or different views of the same one.

**Player:** “Which table was it?”

**Nerin:** “The table beside the sacks.”

**Iven:** “The table under the lamp.”

**Player:** “Could those be the same room?”

**Nerin:** “I remember the crowd.”

**Iven:** “I remember the book.”

Neither witness is required to reconcile the scene. Their memories add perspectives without overriding the source.

## 72. Dialogue: no one wants a new seed promise

Sella says she hoped the vault might have enough seed to restart a garden. Edda asks whether the player found any measurement. The player can answer:
- “The location description names sealed canisters.”
- “The expedition category lists seed-related items.”
- “No present quantity or viability result is in this packet.”

Sella says:
> “Then I can hope without writing it as fact.”

The story allows aspiration, but no line promises a harvest or turns hope into a reward.

## 73. Document: garden table note

> FOR THE NEXT READER
>
> If you find a name in the catalog, keep the page number with it.
>
> If you find a packet, record its label separately.
>
> Do not copy a catalog title onto an unlabeled packet just because the words fit.

This proposed note is written in a present-day garden hand, not attributed to the old librarian. It may be included only if an existing readable-note surface can show it. Its purpose is to carry the distinction between reference and physical stock into the player’s current world.

## 74. Document: the date written three ways

A draft sheet shows:
- “18 April”
- “18/4”
- “04/18”

The last two are written in different hands, and the year is absent. The player asks whether all three refer to the same date. Edda says:
> “They may. One uses day before month, one month before day. Without the title and convention, I will not merge them.”

The sheet may be excluded if it overcomplicates the plan. If used, it illustrates why the game should not translate a long-form date into a code without a verified convention.

## 75. Document: a catalog table transcript

> **Reader:** “Is this the first list?”
>
> **Librarian:** “It is the first copy on the table.”
>
> **Reader:** “I thought you said first catalog.”
>
> **Librarian:** “I said first one you can borrow.”
>
> **Reader:** “Then what is the other one?”
>
> **Librarian:** “The one we keep.”

Iven says the exchange sounds like the distinction between a working catalog and a public copy. They cannot certify that the transcript belongs to the Municipal Seed Vault or the 18 April title page.

This short piece should be presented as an unattributed transcript fragment, not a definitive explanation of “first.”

## 76. Document: the missing shelf number

The proposed accession leaf has a blank shelf field. Edda says the title and date can be copied without a shelf number, but a future reader may not find the book.

The player can add:
> “Shelf number missing in surviving copy.”

They cannot invent a shelf code. The map note’s “ask Margaret” may have made sense to a colleague who knew the shelf, but the current plan has no evidence for which shelf.

## 77. Document: a returned reading copy

> READING COPY RETURNED
>
> Cover repaired.
>
> Index tab intact through M.
>
> Date transcribed to shelf card.
>
> No packet count taken.

The last line is proposed content, not current data. It distinguishes a catalog return from an inventory. Iven remembers returning a copy but cannot identify this specific record.

If a reviewer thinks “no packet count taken” implies a seed audit should have occurred, remove the line. It should remain only if it supports the distinction between a catalog and a stock count without adding a new inventory authority.

## 78. Document: a note to “Margaret”

> Margaret—
>
> The cover has the date. I put the note inside the front sleeve so you will not have to remember the whole thing.
>
> If the list is needed downstairs, take the reading copy. Leave the other on the table.
>
> I am going to the branch before close.
>
> — no signature

This is a proposed letter, not an existing source. It uses Margaret as an addressee but gives no surname or role. It does not connect the writer or recipient to Margaret Webb. Its wording suggests a colleague expected Margaret to know the context; it does not prove which building “downstairs” meant.

The player may preserve it as a proposed story document or omit it if it introduces too much unsupported identity detail. The core story still works without it.

## 79. Document: the blank catalog request

> TITLE REQUESTED: __________
>
> DATE OF ISSUE: __________
>
> READER: __________
>
> COPY TO: __________

A blank request form appears in the same sleeve. It has no seed names, dates, or signatures. Edda says it may be unused stock.

The player can inspect the form and then leave it blank. The empty fields are not a puzzle; no one should fill them with Margaret’s name or the proposed date.

## 80. Scene: the date belongs to the cover

The player proposes putting 18 April on the vault note. Edda asks what the date belongs to.

**Player:** “The first public issue.”

**Edda:** “According to the proposed cover.”

**Player:** “And the map note?”

**Edda:** “It says the date of the first seed catalog.”

**Player:** “Then I will cite both.”

Edda writes:
> “Proposed cover dated 18 April and labeled first public issue; map note refers to date of first seed catalog. Relationship to be verified.”

This is the preferred archive wording until a source review resolves the title match.


## 81. Scene: the date becomes a rumor

A present-day grower tells Sella that the vault opens on 18 April. The grower heard the date from someone who saw a route card. Sella asks where the route card came from. The answer is a copy of the proposed cover.

**Sella:** “The cover has a date.”

**Grower:** “It said the vault code.”

**Sella:** “The note says that. The cover says first public issue.”

**Grower:** “So it is not the code?”

**Sella:** “I did not say that. I said the pages need to be read together.”

The player can correct the rumor gently:
> “The map note calls the catalog date a vault code. The proposed cover carries 18 April. The current access path has not been verified.”

The rumor is not treated as foolish. It grew from two pieces of paper that look related. The story’s task is to preserve their relation without calling it proven.

## 82. Scene: the route card has room for one line

Kesa Harl asks for a short line that can fit beside the location symbol. Edda offers:
> “Seed vault — catalog date noted.”

Kesa says the phrase could be read as a current code. Sella suggests:
> “Seed vault — source note mentions catalog date.”

The player may choose either line or defer the card. The full archive note keeps the longer distinction. No one expects a traveler to carry the entire history on a route card.

The short line does not identify the courthouse or library. Those descriptions remain in the attached source note.

## 83. Scene: the date is not a harvest day

A gardener asks whether 18 April should become the first planting day. Sella says the source does not say that. The catalog date marks a document, not a season.

**Gardener:** “Could still be a good planting day.”

**Sella:** “Maybe. We need a growing record for that.”

The player can write:
> “Catalog date is archival; no planting advice is attached.”

This is an optional note, not a new season or crop mechanic. If the existing game already has authored planting calendars, refer to that owner instead.

## 84. Scene: the first names in the index

The catalog’s partial index includes headings but no legible cultivars. The player can see the letter tabs and the worn paper. Sella asks whether the absence of visible entries means the book is empty.

Iven says:
> “It means this copy is missing its pages.”

Edda adds:
> “The index tab is not the catalog.”

The player may record the missing pages. They may not fill them with seed names from another vault or another narrative dataset. Existing seed items remain authoritative in their current catalogs.

## 85. Scene: the absent Margaret

Edda reads the note once more:
> “Ask Margaret. She’ll remember.”

She says the line sounds as if the writer expected Margaret to be nearby. No current source gives an address, a surname, or a route to her. The player can ask whether to add a search for Margaret as an objective.

Edda replies:
> “Not from this note alone. We can preserve the name. We cannot send someone after a person we have not identified.”

The story does not add an NPC locator, a new survivor, or a search quest. It allows the name to remain unresolved.

## 86. Scene: Iven hears the name

When Iven hears “Margaret,” they pause, then ask whether the player has a surname. The player says no. Iven says:
> “There was a Margaret at the table. There were many people at the table. I do not know which one the note means.”

If asked whether this was Margaret Webb, Iven says they never knew a Webb at the library. They do not identify or condemn the person in the ration-fraud record. A future implementation should include this line only if it does not pull unrelated criminal history into the seed story.

The safer default is to leave the namesake issue in internal continuity notes and keep Iven’s player-facing answer at “I cannot identify her.”

## 87. Scene: Sella reads the label on a seedling

Sella reads the label on her garden plant:
> “Old red bean, source unknown.”

She asks whether a catalog can make that label better. Edda says it can offer a name to compare, if the correct index is found. It cannot prove that the plant descended from a catalog entry.

Sella adds:
> “Then the label stays honest until I have another page.”

The player can choose to inspect the seedling but gains no item or skill. Its purpose is to echo the document’s theme through a living object without claiming that the plant came from the vault.

## 88. Scene: the librarian’s table in memory

Iven describes setting out the catalog:
- the table rocked on one short leg;
- a folded paper kept it level;
- the cover was placed where sunlight would not reach it;
- readers asked for the index before they asked for a packet.

Iven cannot remember what was used to steady the table. They do not remember whether the table was inside the library or just outside the door. The story presents these details as one person’s memory, not verified room description.

The player may ask if the memory helps locate the vault. Iven says:
> “No. It helps me remember the table.”

## 89. Scene: two readers want the same copy

Iven remembers two readers asking for the same catalog page. One waited while the other copied the title. The page was returned with a damp thumbprint. No name or variety survives in the recollection.

**Player:** “Did they get the seed they wanted?”

**Iven:** “They got the page.”

**Player:** “Was that enough?”

**Iven:** “For that afternoon, it was.”

The line carries a small kindness without promising that a packet existed. A catalog could help a person make a request even when the seed itself was elsewhere.

## 90. Scene: the archive worker chooses a shelf

Edda has three possible archive labels:
- Municipal Seed Vault;
- Old Library Basement;
- Courthouse Basement.

She chooses the current location name for the folder and lists the other two source descriptions beneath it. If the player asks why not use a neutral name, Edda says:
> “The location name is what the current map calls it. The source notes show how the records differ.”

The label does not settle which building is correct. It preserves the present key for finding the record.

## 91. Scene: the first catalog gets a cover

The proposed title page arrives without a binding. Edda creates a paper cover and asks whether the date should appear on the front.

The player can choose:
- put “18 April” on the front with “year not stated”;
- put the date inside, with its source;
- leave the date out of the new cover and preserve it only on the original copy.

Iven prefers keeping the original visible. Edda prefers protecting it from handling. Both are reasonable. The player’s choice changes how the proposed archive presents the date, not the date itself.

## 92. Scene: a copy for the garden

Sella asks for a reference she can keep beside the garden labels. Edda writes:
> “Municipal Seed Vault — see archive index; catalog date reference under review.”

Sella says that is enough for her notebook. She does not ask to take seeds or claim the vault’s contents. The reference allows a present community to ask about the source without treating an old record as a delivery.

## 93. Scene: the old public record

The Library Blueprint says the basement is missing from public records. Kesa asks whether that proves it was deliberately hidden. Teren says:
> “It proves the public sheet did not show it.”

Edda adds:
> “A staff drawing and a visitor map can differ for ordinary reasons.”

The player may record “basement absent from public visitor sheet” rather than “secret basement.” This avoids turning an architectural omission into a conspiracy. If later canon establishes secrecy, that source can be added separately.

## 94. Scene: a copied title crosses the page

A faint title on the catalog cover may read “Municipal Seed List” or “Municipal Seed Library.” The last letters are rubbed away. Edda can read only “Municipal Seed L—”.

The player may choose:
- preserve the visible letters and mark the word incomplete;
- use the location’s current display name in a modern cover;
- leave the title transcription out.

No choice completes the damaged word. The location name remains known from the current catalog; the source title does not.

## 95. Scene: the two catalog dates

A separate calendar sheet has 18 April circled. A second note has “18/4” written beside a library table. Edda says they may point to the same day. Iven remembers a table, but not the calendar. The player can preserve these as two separate observations.

If they are presented together:
> “The proposed first-public-issue cover and an unassigned calendar both carry 18 April. Their relationship is not established.”

No document says that a code was entered on the date itself. The note says the date is the code; the story does not add a date-based door mechanic.

## 96. Scene: the question of viability

Sella asks whether the location entry’s “preserving crop strains” means the seeds are still viable. Edda replies:
> “It describes the reserve. The map-zone text says they may still be viable. Neither is a test result from today.”

Sella says she can preserve both claims:
> “The records say sealed. They say may be viable. I can leave the next step open.”

The player’s note should use “may be viable” with attribution, not “seeds are viable.” No test or seed germination minigame is proposed.

## 97. Scene: not every catalog is an inventory

Iven finds a margin note in the proposed catalog:
> “Names revised after table reading.”

They explain that a catalog could have been a reference list, an order list, or an index to a storage ledger. The current cover does not say which. The player may ask whether the catalog entries correspond one-to-one with seed canisters.

Iven answers:
> “The book has entries. The vault has canisters. A matching label would be needed to connect them.”

This line keeps the story within source authority and avoids a hidden inventory reveal.

## 98. Scene: Edda declines a surname

A player suggests writing “Margaret Webb” beside the note because it is the only Margaret in a known record. Edda refuses:

> “A name search gives us a person with that name. It does not give us the person this note means.”

The player can choose:
- “Keep the name Margaret only.”
- “Mark identity unverified.”
- “Do not index the name separately.”

Edda recommends the second. The existing ration-fraud record remains intact and is not retconned. This scene can be omitted from player-facing dialogue if it would expose unrelated content; the internal continuity rule still applies.

## 99. Scene: the note remains folded

The player may fold the Librarian’s Note along its original crease. Edda suggests leaving it open for storage so the ink does not abrade, but she does not make the choice mandatory.

The player can leave it:
- open in a sleeve;
- folded inside the blueprint;
- copied onto a new transcription card.

The original remains unchanged. If an implementation does not model document state, a single narration describes Edda placing the paper in a sleeve. The choice carries no preservation meter or archive durability system.

## 100. The answer to the request

Edda asks what the player can now tell a reader who asks for the seed vault.

The player may answer:
- “The current location entry calls it a Municipal Seed Vault beneath the courthouse.”
- “The Suburban Heights record calls it a community seed bank beneath the old library.”
- “Both descriptions share one mapped location ID; their relationship is not explained.”
- “The map note names a first catalog date and an unidentified Margaret.”

Edda says:
> “That is a complete answer to what we have, even though it is not a complete history.”

This closes the central interaction. The story does not require the player to decide which building name is true.


## 101. Ending A: file the date with both titles

The player chooses a long archive note:

> **Municipal Seed Vault** is the current location name. The current location description places the agricultural reserve beneath the municipal courthouse basement. The Suburban Heights map-zone description places a community seed bank in the basement of the old library. The surviving source text does not reconcile these building descriptions.
>
> The Librarian’s Note says that the vault code is the date of the first seed catalog and names “Margaret” without a surname. A proposed catalog cover marked “first public issue” carries the date 18 April, with no year. The match between that cover and the note is not established by current data. The identity of Margaret is unknown.

Edda says the summary will be long enough to preserve the questions. Teren staples it to the blueprint copy, not the original. Sella takes a reference card for the garden. The date is kept as a source detail, not used as a present access code.

## 102. Ending B: a short route card

The player chooses a short front card:

> **Municipal Seed Vault**
>
> Current records differ on the building name. The map note mentions a first-catalog date; see archive copy.

The reverse carries:
> “Librarian’s Note” names Margaret without a surname. A proposed catalog cover carries 18 April; title match and code format unverified.

Kesa says the card is clear enough for a traveler to find the right folder. Edda agrees that the full building descriptions belong in the archive. The route card does not rename the location or alter the map.

## 103. Ending C: leave the date unconfirmed

The player may choose not to use the proposed 18 April date because the accession source has not been verified. Edda writes:

> First-catalog date: mentioned by the Librarian’s Note; no verified date in the current packet.

Iven accepts this. They would rather wait for an authoritative cover than see a remembered date copied as fact. The story resolves the task by preserving the note and describing the missing evidence.

## 104. Ending D: keep the proposed date as a proposal

The player may preserve 18 April in the story packet but label it “proposed source.” The archive note says:
> “A proposed first-public-issue cover carries 18 April. Review against the current catalog authority before using the date as a code.”

This branch acknowledges the player found a candidate, while preventing it from masquerading as current data. No existing quest state or access flag depends on this selection.

## 105. Ending E: keep Margaret’s name in the transcript only

The player may decline to add Margaret as a separate index heading. The name remains in the verbatim transcription of the note. Edda says:
> “The source keeps the name. The index does not need to pretend we know the person.”

This option protects the text while avoiding a false identity record. If a later content owner discovers a source that identifies Margaret, an indexed name can be added with that evidence.

## 106. Ending F: list the two building descriptions

The player chooses a balanced building note:
> “The location entry names the municipal courthouse basement; the Suburban Heights entry names the old library basement. Both use the current Municipal Seed Vault map entry. The relationship is unresolved.”

The wording is not presented as a riddle. It is a clear account of a source difference. Teren says it will help the next copyist avoid tracing the courthouse onto the library plan.

## 107. Journal entry: short

> The Municipal Seed Vault is named in the current location catalog and the Suburban Heights damaged-map zone. The location description places it beneath the municipal courthouse; the map-zone description places a community seed bank beneath the old library. The fragments show a basement on a library blueprint and a note that ties a vault code to the date of the first seed catalog.
>
> A proposed catalog cover carries 18 April but lacks a year. Its connection to the note is not established. “Margaret” appears as a first name only.

## 108. Journal entry: long

> The Library Blueprint shows a lower floor absent from public records. The Librarian’s Note says the vault code is the date of the first seed catalog and tells a future reader to ask Margaret. The current location catalog describes a climate-controlled reserve beneath the municipal courthouse basement. The Suburban Heights map-zone record calls the installation a community seed bank beneath the old library.
>
> A proposed catalog cover titled “Municipal Seed List — First Public Issue” carries 18 April, with no year. Iven Cale remembers carrying copies to a public table but cannot authenticate the cover or identify the librarian who read the list. The proposed date should not be treated as the code until its source and format are verified.
>
> The note’s Margaret is not identified. Current content contains another person named Margaret Webb in a separate record, but no source connects them. Other seed-vault narratives use separate location IDs and retain their own histories.

If journal voice should not reference another content record, use the shorter version and keep the namesake separation in the design note only.

## 109. Journal entry: the catalog date

> The proposed cover carries 18 April and calls itself the first public issue. The map note calls its referent the date of the first seed catalog. The year is absent, and the current data does not establish that these are the same event.

This entry can be added only if the proposed cover is included. Without that new content, the player journal should simply say the date is not found in the current fragment text.

## 110. Journal entry: the two locations

> The current location description says courthouse basement. The Suburban Heights installation description says old library basement. The Library Blueprint has a library service-drawing stamp. No surviving page in this packet connects the two building names.

The final sentence should be rechecked against current lore before implementation. If another authoritative source establishes a connection, replace it with that source’s wording.

## 111. Journal entry: the name in the note

> The Librarian’s Note says “Ask Margaret. She’ll remember.” It gives no surname. The archive has not identified the person.

This avoids overexplaining the unrelated Margaret Webb record to a player unless an existing narrative scene makes it relevant.

## 112. Optional scene: the catalog returned to its sleeve

At the end of the visit, Edda puts the proposed catalog cover back in a sleeve and checks that the date field is visible. Iven slides the note behind it but does not fold it over the name. Teren places the blueprint in a separate transparent cover because its lines are beginning to flake.

Sella asks whether the pages should all be kept together. Edda says:
> “Together in the packet. Separate in the labels.”

The physical arrangement mirrors the story’s content discipline. The papers can remain associated by the current location entry without being described as one original document set.

## 113. Optional scene: reading the title aloud

Iven reads the catalog title aloud once:
> “Municipal Seed List. First public issue. Eighteen April.”

They stop before saying “the first seed catalog.” The player asks why.

> “That is what the note calls it. This is what the cover calls itself.”

Iven’s restraint should feel personal rather than procedural. They have carried the book and remember the table, but they will not upgrade a title into a claim.

## 114. Optional scene: Sella writes a garden note

Sella’s garden notebook receives:
> “Municipal Seed Vault catalog reference under review. No current seed packet identified.”

She leaves a blank line beneath it for a future source. The plan does not supply a harvest date or seed packet. The notebook belongs to Sella and does not become a new persistent gameplay resource.

## 115. Optional scene: Edda handles the namesake record

If the player asks to compare the note with the Margaret Webb record, Edda says:
> “I can show you that another record uses the name Margaret Webb. I cannot show you that it is the same Margaret.”

The player can ask to keep the records separate. Edda adds a private cross-check note:
> “Do not merge by first name alone.”

This scene should be hidden or omitted if the namesake record is not appropriate to show in this quest. The internal rule remains: no identity merge without evidence.

## 116. Optional scene: the public catalog table is rebuilt

Jori, a junior archive volunteer, sets up a small display table with the title page, the index tab, and the date card. They place the Librarian’s Note at the center. Edda moves it to one side because it is not part of the catalog.

Jori asks:
> “If the note tells us how to find the book, should it be in the book?”

Edda answers:
> “It can be beside the book. That is why we keep a sleeve.”

The player may help arrange the papers. No world-state changes. The scene is an environmental staging of source distinction.

## 117. Optional scene: someone asks for seeds

A garden visitor arrives and asks if the vault has seed packets. Sella says the location record describes sealed canisters and the map-zone description says seeds may still be viable. The packet contains no current inventory or test.

The visitor responds:
> “Then I will not plan a bed around it yet.”

Sella offers the catalog reference so the visitor can read the records. The story does not tell them to wait, deny them a resource, or promise a future delivery.

## 118. Optional scene: the courthouse clerk

A present-day clerk recognizes the phrase “municipal courthouse basement” from the location entry and says the building’s lower floor was once used for records. The clerk does not know whether that was the same structure shown on the Library Blueprint.

If the player asks whether the seed bank moved, the clerk says:
> “I can remember where we filed municipal forms. I cannot tell you where the old library stood from this page.”

This optional witness does not resolve the mismatch. If adding the courthouse clerk would imply too much new history, omit the scene.

## 119. Optional scene: a library branch story

Luma remembers that several library branches used the same basement drawing template. A service-drawing could show a standard lower floor without describing one specific building. She says:
> “We reused the form. We did not reuse the basement.”

This memory is proposed and needs review. It offers one possible reason the blueprint looks generic, but does not assert that the courthouse and library were the same building. The player may record it as Luma’s recollection rather than official explanation.

## 120. Optional scene: the proposed date is challenged

A player asks whether the 18 April date should be removed because its year is blank. Iven says a date without a year can still identify a local event if the people using the catalog shared a calendar. Edda says the archive needs the source attached.

**Iven:** “It meant a day to the people who were there.”
**Edda:** “And now it needs a page beside it.”
**Iven:** “Both can be true.”

The dialogue allows the date to matter emotionally without becoming a complete archival identifier.


## 121. Distinguishing the other seed-vault files

Edda checks the location IDs on the catalog table:
- loc_municipal_seed_vault;
- loc_seed_vault;
- loc_seed_library_annex.

They are different current identifiers. The player can read summaries of the other records only if the existing archive interface exposes them. The narrative must not present them as different names for one site.

If asked whether the older recovery records prove this vault is already empty, Edda answers:
> “Those records name other locations. They do not measure this one.”

If asked whether the standing-record seed-bank quest describes the Municipal Seed Vault, she says:
> “Its target is the Seed Library Annex. The map here uses a different location entry.”

This is a continuity boundary for writers and, if shown to players, a straightforward explanation of why similar words do not automatically identify the same place.

## 122. The finite yield belongs to another site

A separate seed-vault narrative records a recovered total and says that its own cryo-system failed and its site was sealed. This plan does not import that yield or outcome into the Municipal Seed Vault. The current Municipal description instead describes sealed canisters and cold-storage lockers. The two sets of records may share a broad category, but no source joins their locations.

A future implementation may not use the other site’s total as the Municipal vault’s inventory. It may not tell the player that “all seeds were already recovered” unless a current authority adds that fact for loc_municipal_seed_vault.

## 123. The other Margaret remains separate

Margaret Webb appears in a narrative fraud record. This plan does not make her the librarian, a survivor, a relative of the note-writer, or the person who knew the catalog date. First-name matching is insufficient.

If a future continuity pass discovers a deliberate link, it must state the source and consider the existing character portrayal. Until then, keep the references separate. Player-facing content can omit the namesake entirely.

## 124. The town’s memory of a table

Nerin remembers a public reading at a produce table. Iven remembers an indoor library table. A later storyteller may have combined both memories into one scene. The player can preserve both recollections without calling either one a false account.

Edda writes:
> “Two oral memories describe a catalog reading in different settings. Neither identifies the building on the map fragment.”

This keeps the social history alive while respecting the site mismatch. People may have read the catalog in multiple places; the sources do not tell us.

## 125. The list that traveled

A proposed note from Iven reads:
> “The public copy went where readers were. The reference copy stayed on the shelf.”

Iven says this explains why the catalog could be remembered at a market table while the blueprint points to a library basement. It is a plausible explanation, not proof. The player may file it as Iven’s memory.

The story should not make this line the resolution to the courthouse/library discrepancy. It tells us a catalog copy traveled; it does not identify where the vault was.

## 126. The date is retained, not activated

The completion scene may display the proposed 18 April date in the archive card. It must not animate a lock opening, set a map flag, grant entry, or mark a vault as explored. The current clue’s use in the game requires a separate route-owner check.

If the existing content already treats the date as an access clue, use the exact current behavior. The prose plan does not replace that behavior or define a new one. If no current access rule exists, keep the date as readable lore until a system owner authorizes its use.

## 127. The meaning of “code”

Sella asks whether “code” could mean an index code rather than a door code. Edda says the note uses “vault code,” which strongly suggests access, but the note does not define the mechanism.

The player may say:
- “It probably refers to access.”
- “The wording suggests access, but the method is not described.”
- “The current system must decide how this clue works.”

Edda recommends the second phrasing for the archive. The story does not erase the clue’s implication; it distinguishes likely meaning from verified implementation.

## 128. Scene: the title card and the key

The player places the catalog cover beside the access clue. There is no keypad in this scene. The date is written on the cover in ordinary ink. The note is folded beside it.

> The papers fit in one sleeve. Their edges do not align. The cover gives a date. The note gives a reason to ask for one. The map gives a place name. The current data gives two building descriptions.

The player can choose which sheet goes on top. Edda recommends the note so its “Ask Margaret” instruction remains visible. Iven recommends the catalog cover so the date is not lost. They agree to use a contents card that references both.

## 129. Scene: the date has a reader

The date 18 April is spoken again when Sella asks why it matters.

**Edda:** “It lets someone find the first-public-issue cover.”
**Iven:** “It reminds me of the table.”
**Sella:** “It tells me when people could ask for the list.”
**Teren:** “It gives me a line to copy onto the index.”

The same date serves different practical purposes. None of these lines says that 18 April is a planting date or a current access instruction.

## 130. Scene: the name has no reader

The player asks whether anyone currently remembers Margaret. Edda says no one in this packet has identified her. Iven remembers a reader at the table but not a name. Nerin remembers a crowd but not its members. The current record gives only a first name.

The final journal wording is:
> “The note names Margaret. No witness in this account identifies her.”

This does not say Margaret is dead or missing. It simply records that the story has no verified identification.

## 131. Scene: a catalog’s social life

A library volunteer remembers that a public catalog could be borrowed by families for a night. They carried it home to compare the names against a garden plot. The volunteer cannot identify the current cover or the vault.

**Volunteer:** “The list went home in someone’s coat pocket.”
**Player:** “Did it come back?”
**Volunteer:** “The copy did. I do not know what the family wrote beside it.”

The recollection suggests that readers used the catalog privately. It does not establish a new list of cultivars or a family’s seed stock. This optional voice can be omitted if the proposed cast becomes too large.

## 132. Scene: preserve the phrase exactly

Edda notices that an early transcription changed “She’ll remember” to “She remembers.” The player can correct the copy back to the exact fragment wording.

The difference matters:
- “She’ll remember” is a prediction from the note’s writer.
- “She remembers” sounds like the current narrator’s claim.

Edda says:
> “One small verb can make a promise out of a message.”

The final transcription uses “She’ll remember.” No voiceover should imply that the current team has verified the writer’s expectation.

## 133. Scene: preserve the title exactly

The current location display name is Municipal Seed Vault. The catalog cover may say Municipal Seed List. The map-zone name is Municipal Seed Vault. The player must not substitute “Seed List” for the location title.

Edda explains:
> “A book can have one title and the place that holds it another.”

The archive cover keeps both:
> Location: Municipal Seed Vault.
> Catalog title: Municipal Seed List.

This distinction will help future content authors keep item, location, and document names separate.

## 134. Scene: the date in a spoken memory

Iven says:
> “Eighteen April. I remember the number because I carried two copies and one had a wet corner.”

The player asks whether the date came from the cover or from memory.

> “Both now. I read it on the cover after someone showed me the copy.”

This is a subtle memory contamination scene: a person can remember a fact because they later read it, not because they witnessed the original event. Edda’s archive note should label the date as a memory confirmed by the proposed cover, not an independent recollection.

## 135. Scene: a date with no year

Sella asks whether the date could be attached to a current campaign day. Edda says the cover has no year, and the setting’s day count cannot be derived from the month and day alone.

The player can add:
> “Year not present in the surviving copy.”

This line avoids a date conversion mechanic. No chronology is added. If a current source later supplies the year, the note can be amended with attribution.

## 136. Scene: a returned page

A loose catalog page is found with the lower corner folded. The page has a page number but no title. Iven remembers pages being removed for public copies when someone needed to read a single entry.

The player may preserve the page number, but no entry title is legible. They cannot identify a variety, count, or packet. The page is not connected to the proposed first issue unless an existing source says so.

## 137. Scene: the archivist corrects her own note

Edda’s first draft says:
> “The first catalog was published 18 April.”

She realizes that “published” and “first” are stronger than the cover supports. The player can help revise it:
> “The proposed cover labels itself the first public issue and carries 18 April.”

Edda signs the correction. The scene models an archivist fixing overconfident prose in a visible way. It does not make her incompetent; good records include corrections.

## 138. Scene: Sella’s garden remains unnamed

At the end of the story, Sella returns to her current seedling. The label remains “Family red bean — source unknown.” She does not rename it based on the catalog.

> “I can know the plant in front of me without knowing where the old book would have put it.”

The line is the emotional closing for the garden thread. The player has helped preserve a reference, not identify a living crop.

## 139. Scene: the route card is placed

Kesa places the short route card beside the current map entry. It reads:
> Municipal Seed Vault — see archive copy for building-source difference and catalog-date note.

The wording fits on one line. It does not identify an access route or tell travelers to enter. If no route-card surface exists, Kesa can say the line aloud and Edda can keep it in the narrative journal.

## 140. Scene: the archive stays open

The player can leave the archive scene without forcing a final answer. The table remains arranged with:
- the current location entry;
- the map-zone summary;
- the blueprint copy;
- the note;
- the proposed catalog cover;
- the contents card.

No paper is placed on top of all the others as “the truth.” The location name remains current; the sources retain their wording.



## 141. Optional scene: the witness asks what will be written

Iven does not object to the archive. He objects to the way a question can harden into an answer when it is copied three times.

> “If I tell you I carried a book, you will write that I carried a book.”
>
> “If I tell you the date I can see on the cover, you will write that the cover has a date.”
>
> “If you write that I remember the first catalog, I will become the man who remembers a thing I never said.”

Edda asks the player to read the draft aloud. The draft is deliberately ordinary:
> Iven Cale reports carrying a copy of the Municipal Seed List.

The player may leave it, qualify the source, or decline to record his name. If qualified, the final wording is:
> Iven Cale remembers carrying a copy identified to him as the Municipal Seed List. His recollection does not establish the first publication date or the contents of the vault.

Iven listens without correcting the sentence.

> “That is less flattering.”
>
> “It is also closer.”

This is a small but important relationship beat. The player earns trust by keeping the account within what the witness actually said. No trust score is introduced. The changed line is a narrative outcome only.

## 142. Branch: the player insists the note means 18 April

If the player says the date must be the keypad answer, Edda does not treat the conclusion as clever or confirmed. She points out that the note contains no year, that the cover is a proposed authored insert pending source review, and that neither source states that a date is an access code.

> “A date can be a date,” she says. “The word code is a job you have given it.”
>
> “Margaret called it the code.”
>
> “The note says the code is a date. It does not tell us which date, and it does not tell us where to type it.”

The player may still repeat the guess. Nothing changes in the site state, no door is simulated, and no failed-entry penalty appears. The scene exists to stop the narrative from quietly presenting speculation as an interaction contract.

If the player chooses to write the guess in the notebook, the entry is explicitly marked:
> Field inference, unverified: the librarian’s “date” may refer to the catalog cover. No source says the date opens a door.

There is no branch in which the proposed date unlocks a mechanism by implication.

## 143. Branch: the player treats the note as a false lead

Sella is angry when the player calls the note useless.

> “It got us a book.”
>
> “It got us a possible cover.”
>
> “It got us a room we cannot name.”
>
> “It got us one person saying another person remembered. That is not nothing.”

Edda agrees that it is not nothing, but refuses to call it a confirmed location key. The archive keeps the note in a sleeve with its transcription. The player can describe it as a lead, a record of a promise, or an unresolved instruction. They cannot mark it fraudulent unless the current content owner has authored evidence for that conclusion.

Sella’s response varies with the choice:
- Calling it a lead makes her relieved but wary.
- Calling it a promise makes her insist that the note does not say who made the promise.
- Calling it unresolved earns a quiet nod from Edda.
- Calling it false ends the argument, but Sella stops asking the player to read the catalog with her.

The last outcome is social texture, not a hidden relationship penalty.

## 144. Branch: the player declines to name the unknown librarian

The note says “Ask Margaret.” It supplies a first name, a role, and a remembered relationship to the date of a catalog. It does not give a surname, pronouns, age, employment history, or a reason for leaving the note.

If asked to enter a name into the archive index, the player can select:
> Margaret — identity not established by this note.

Edda asks whether this feels incomplete. The player may answer:
> “It is incomplete.”
or
> “A false surname would only make it look finished.”

The second answer does not unlock a reward. It is the scene’s clearest statement of the expansion’s ethic: preserving a gap is sometimes the most useful available record.

## 145. Short written artifact: transcription of the librarian’s note

The note’s transcription appears in the narrative journal as a source object, not as a newly invented quest objective:

> LIBRARIAN’S NOTE — TRANSCRIPTION
>
> The vault code is the date of the first seed catalog.
> Ask Margaret. She’ll remember.
>
> Handwriting and date of note: not established in this copy.
> Attachment to a building or key: not established.
> “Margaret”: first name only in the text.
> Meaning of “first seed catalog”: not established.
> Catalog date: not established by the note alone.

The transcription does not include decorative stains, a signature, a writing instrument, or an author attribution unless those details are separately supported by the data or approved by the content owner. The scene may show a simple paper scrap because the source calls it a note; its physical age and condition remain unasserted.

## 146. Short written artifact: proposed catalog cover, review copy

This is a new diegetic text proposal, not an existing game item. It is included in the expansion package only as an authored candidate that must be checked against current catalog, calendar, and access authority before any implementation:

> MUNICIPAL SEED LIST
>
> FIRST PUBLIC ISSUE
>
> 18 APRIL
>
> Copy designation, year, printer, and municipal office: not present on the reviewed face.

The expanded plan must not add a year or convert the day into a current campaign date. “First public issue” is wording created for the story. It is not cited as established fact about the current setting. A content review may revise or remove it; if removed, every line that depends on the cover becomes a conditional scene, and the unresolved story still functions.

The face is not a map, credential, pass, or keypad instruction. It does not contain a vault inventory.

## 147. Short written artifact: catalog contents card

> CATALOG CONTENTS — COPY INDEX
>
> Cover observed: Municipal Seed List.
> Cover date proposed for this story copy: 18 April, year not shown.
> Page count: unverified.
> Entries legible in this copy: none supplied by this expansion.
> Cultivars represented: not established.
> Seed quantity held in the vault: not established.
> Storage condition: not established by the cover.
> Current access state: outside this record.
> Relation to the Municipal Seed Vault location: proposed narrative association; review required.

This card is an archive’s description of what it has, not an in-world claim that the catalog was recovered at the vault. No pages, labels, named cultivars, batch sizes, or germination percentages should be invented in implementation to make the artifact feel more complete.

## 148. Edda's margin marks

The player can read three marks in Edda’s pencil:
- “Cover says first issue; independent confirmation absent.”
- “Note directs us to a person; identity remains open.”
- “Place named by this record not established.”

Edda explains that the marks are not a verdict. They are a way of making future readers see what remains to be checked.

> “Somebody will open this folder after we are gone. If I leave the gap invisible, they will fill it with whatever makes the page easiest to use.”
>
> “What if they fill it anyway?”
>
> “Then at least they will have to cross my handwriting.”

These lines are designed for restrained delivery, with pauses and no triumphant music cue. Their purpose is to make careful recordkeeping feel like a human action, not a tutorial popup.

## 149. Camp conversation: Sella sorts the surviving labels

Sella lays three plant labels on the table. One is legible, one has only a color stripe, and one is a square of paper with no writing. She asks the player which should be kept.

All three are kept, but for different reasons. The legible label is a useful identification. The color stripe may correspond to a convention not represented in the surviving records. The blank paper has no proven meaning and should not be described as a seed label at all.

> “You keep the blank one too?”
>
> “I keep the paper. I do not pretend I know what the paper did.”
>
> “That sounds like a lot of keeping.”
>
> “It is less than inventing.”

This exchange gives Sella practical warmth without turning her into a lecturer. Her care is rooted in making records usable by whoever comes next.

## 150. Camp conversation: Teren asks for a number

Teren wants to know whether the vault has enough seed to justify a trip. The existing plan deliberately offers no count. The player can tell him:
> “We have a reference to a catalog.”
or
> “We do not have a verified stock count.”

He dislikes both answers.

> “A reference does not feed anyone.”
>
> “No,” Sella says. “But a made-up count does not feed them either.”

Teren eventually asks Edda to keep the distinction on the route note, not because it makes the decision easier but because the next person should know why it remains hard. The scene carries the survival pressure without fabricating a yield, stockpile, or resource reward.

## 151. Camp conversation: Kesa refuses a false map label

Kesa is offered a label reading “Library Basement.” She compares it with the current location name and declines to put it on the map.

> “The clue says library. The location entry says courthouse. One of them may be wrong.”
>
> “Which one?”
>
> “That is why I have not written an arrow between them.”

The player may ask her to preserve both source names in a note. She writes:
> Map-zone fragment: old library basement.
> Location description: beneath municipal courthouse basement.
> Relationship between these descriptions: unresolved.

No path is drawn. No map reveal is triggered. This scene directly translates the data discrepancy into an observable content outcome while preserving the distinction between names and routes.

## 152. Camp conversation: Ora refuses to become the answer

Ora never claimed the note was about her. She hears the others discussing Margaret and asks why they are using her name so carefully.

Edda explains:
> “Because one note gives us a first name and no more.”
>
> “Then use no more.”
>
> “We are.”
>
> “Good. I have already been mistaken for people I never met.”

Ora's own history is not supplied, and this line should be removed if it accidentally implies a specific former identity. It exists to underline the boundary, not create a second mystery that must be solved.

## 153. Night scene: the table is cleared

The story can end a session at the archive table. Edda gathers the papers into separate sleeves, naming each as she moves it:
- map fragment;
- librarian’s note;
- blueprint copy;
- proposed cover;
- current location entry;
- newly written contents card.

The camera remains on hands and paper. No one piles all sources into a single stack. The visual rhythm is slow, almost routine.

Sella asks:
> “Do we know more than we did?”
>
>Edda answers:
> “We know which question belongs to which page.”

Sella accepts this without smiling. The scene avoids the false emotional turn where uncertainty is suddenly celebrated as a victory.

## 154. Camp scene: a label for tomorrow

The following day, Teren finds Edda’s short card attached to the archive sleeve. He reads aloud:
> “Building source differs. Catalog date unverified. No stock count.”

He asks whether that means the trip is canceled. Edda says the card does not make the decision.

> “A note should not pretend to be a vote.”
>
> “Then what does it do?”
>
> “It tells the next person what we knew when we chose.”

This line connects the content to expedition planning without adding an expedition-decision feature. The story can use the existing journal or dialogue delivery. It does not create a mission board, resource commitment, or travel unlock.

## 155. The player takes the archive copy

If the player chooses to carry the transcription, it is a narrative inventory representation only where an existing journal or note-item path already supports it. The expansion plan does not request a new inventory class or item effect.

The card reads:
> The librarian’s note links a vault code to a first seed catalog date and names Margaret as someone who may remember. The surviving line does not identify Margaret, establish a year, give an access method, or confirm the location description. The municipal seed list cover date proposed in this story remains unverified.

If no portable-note owner exists, this text remains in the existing quest journal or dialogue record. The prose is not grounds for creating a separate mutable archive inventory.

## 156. The player leaves the card behind

If the player does not carry a copy, Sella asks whether the evidence will still be there when they return. Edda says she will retain the archive copy under the current journal’s normal record rules, if that route is already supported.

> “You do not have to carry every uncertain thing.”
>
> “Does leaving it make it less important?”
>
> “No. It makes it somewhere else.”

This branch does not claim persistence behavior that is absent from the existing journal. The implementation must use the current narrative record owner and its save path; it may not invent a new save section to preserve a paper prop.

## 157. Conditional scene if the proposed cover is rejected

If content review rejects the new 18 April cover text, the story remains complete. Edda removes the cover from the source table and replaces it with a blank review slip:

> Proposed catalog copy not admitted to the archive.
> Date and contents remain unverified.
> The librarian’s note still records an instruction to ask Margaret.

Iven can still discuss the difference between memory and later reading. Sella can still preserve an unknown label. Kesa can still record the library/courthouse mismatch. Teren can still refuse a fabricated stock count. The player’s meaningful outcome is the accurate separation of what the sources say, not possession of an invented date.

## 158. Conditional scene if the source mismatch is resolved by new authority

If a current source owner supplies evidence that the old library and courthouse descriptions refer to the same structure, an implementation may update the scene’s archive note with the new attribution. It must identify that authority and preserve the previous descriptions as historical text if they were already published.

Until that evidence exists, the story does not call the discrepancy a continuity error, a secret passage, or proof of multiple vaults. The expansion may ship without resolving it. Edda’s line remains:
> “Two records give us two descriptions. We have not yet earned the sentence that joins them.”

## 159. Conditional scene if a verified catalog date becomes available

A later content owner may supply a catalog date. The authored scene must quote the authority and retain any missing year or issue details as unknown. A date may become confirmed as a date without becoming an access code.

If the authority says only “first catalog issued 18 April,” the journal can say:
> A later verified record identifies the first catalog issue as 18 April. It does not identify an access procedure.

If the authority also establishes an access procedure, that work belongs to the location and interaction owner. This story’s dialogue and proposed cover do not implement it.

## 160. Final scene: the question goes in the margin

At the last table scene, the player may ask Edda to write one question at the bottom of the archive card. The choices are:
- Which building does the map-zone fragment describe?
- Which catalog did the note call the first?
- Who was Margaret?
- What did “vault code” mean in this context?
- What did the vault hold at the time of the record?

Edda writes the selected question without adding an answer. The other four are retained on the reverse, unranked.

> “Why the margin?”
>
> “The middle of the page is for what we can support.”
>
> “And the margin?”
>
> “For what we must remember to ask.”

The scene gives the player agency over what uncertainty to carry forward. It does not promise that any question is answered in a sequel.

## 161. Closing journal entry: preserve the source boundary

> The Municipal Seed Vault is named in the current location catalog. A map-zone fragment describes a community bank in an old library basement, while the location description places the reserve beneath a municipal courthouse basement. The relationship between those descriptions remains unresolved.
>
> A librarian’s note says the vault code is the date of the first seed catalog and asks that Margaret be consulted. The note gives no surname, year, access method, or explanation of “code.”
>
> A catalog-cover text dated 18 April has been proposed for this story but is not verified source content. No seed quantity, cultivar list, or present stock condition has been established here.
>
> Records preserved; location and access questions remain open.

This is the canonical closing text for the expansion. If a later implementation omits the proposed cover, its second paragraph changes to say the catalog date remains absent from the reviewed materials.

## 162. Closing journal entry: the emotional version

A shorter, more personal entry may be used if the full archive note would feel too administrative at the end:

> Sella labeled the seedling “source unknown” and left the label where the next person could read it. Edda kept the note with its missing surname. Kesa refused to draw a route from two descriptions that do not yet meet.
>
> We did not find a number that could settle the argument. We found the places where the argument began.

This text contains no newly established fact about the vault. It closes on the people’s choices to preserve what they actually know.

## 163. Optional final exchange with Iven

As the player leaves, Iven may ask:
> “Did you write me down?”

The player can say:
- “I wrote what you said.”
- “I left your name out.”
- “I wrote that you remember carrying a copy.”

If the player says the first, he asks what exactly the sentence says. The answer repeats the final attributed wording. If the player cannot recall, he answers:
> “Then the paper has to be better than my memory.”

If the player left his name out, he says:
> “Good. The book was not mine to certify.”

If the player used the attributed version, he answers:
> “That is fair.”

No response is framed as a moral score. Iven’s agreement is not a provenance certification; it is his reaction to how he was represented.

## 164. Optional final exchange with Sella

Sella asks whether the archive has helped her garden.

> “It gave me no seed.”
>
> “No.”
>
> “It gave me no name for this one.”
>
> “No.”
>
> “It stopped me from borrowing a name just because the page was empty.”
>
> “That sounds like something.”
>
> “It is something I can use.”

This exchange keeps the difference between information and material supply visible. The story can matter to the player without awarding seed packets or altering crop behavior.

## 165. Optional final exchange with Edda

Edda asks the player to check the archive card one last time. The player sees one deliberate blank line:

> Verified relation between the named place and the municipal vault: __________

Edda asks whether the blank should be removed to make the card cleaner. The player may answer:
> “Leave it.”
or
> “Mark it as unresolved.”

Edda prefers the second because it tells a future reader the blank is intentional.

> “A blank can look like an omission.”
>
> “Then write that we meant to leave it blank.”
>
> “That is a kind of answer.”
>
> “It is an answer about us.”

The exchange underlines that the archive documents both the subject and the limits of the group’s knowledge.

## 166. Optional artifact: route-side caution card

The route-side card is a short, player-readable copy suitable for existing journal presentation:

> MUNICIPAL SEED VAULT — SOURCE NOTE
>
> Do not use the catalog cover date as an access code on this evidence.
> The current map-zone and location descriptions name different municipal buildings.
> The note’s “Margaret” is not identified beyond a first name.
> No current stock count is established.

The first line describes the evidence state; it does not prohibit player experimentation through a nonexistent interaction. It prevents future prose from turning a proposed date into a valid code.

## 167. Optional artifact: Sella’s field label

> FAMILY RED BEAN
>
> Current plant label.
> Source: unknown.
> Catalog match: not established.
> Seed-vault origin: not established.

This label is not a new item or plant variety. It is an example of careful wording applied to a living object that matters to Sella. It should be used only if an existing scene can display or narrate a label.

## 168. Optional artifact: Kesa’s map annotation

> MUNICIPAL SEED VAULT
>
> Map-zone fragment: old library basement.
> Location description: municipal courthouse basement.
> Do not join descriptions pending source review.
> No route drawn from this note.

The annotation preserves the contradiction in one compact place. It does not change any world-map node, reveal state, danger lock, route graph, or starting-unlocked value.

## 169. Optional artifact: the deferred question list

> QUESTIONS RETAINED
>
> Is the old library the same structure as the courthouse basement?
> What does “first seed catalog” refer to?
> Who is the Margaret named in the note?
> Does “vault code” refer to a date, a person’s memory, or another record?
> What stock, if any, was present at the time represented by these records?

The list is a diegetic record of uncertainty. It is not a checklist with required completion flags, and its order must not imply priority or availability.

## 170. Delivery note: scene order and pacing

The story should begin with the location/data discrepancy, then introduce the librarian’s note, then let the proposed cover enter only after the player has heard what the note cannot prove. If the cover is presented first, its date risks being treated as a solve. The dialogue should stay quiet, with interruptions and ordinary tasks around the table.

A suggested sequence is:
1. Kesa refuses to connect the library and courthouse labels.
2. Sella introduces the current plant label and asks for a source.
3. Edda presents the note and explains the first-name boundary.
4. Iven discusses carrying a catalog copy.
5. The proposed cover is reviewed conditionally.
6. Teren asks for a stock count and receives an honest unknown.
7. The player chooses which unresolved question to preserve.

Any implementation may reorder scenes for an existing quest structure, provided the cover remains explicitly provisional and the three central distinctions remain intact.

## 171. Content boundaries for localization and UI delivery

All new player-facing text is plain, short-form narrative prose. It should be added through the existing narrative content authority after review, with the existing localization conventions. Do not place story paragraphs in Godot panel code or embed them in a location panel as a new authority.

Where a current interface cannot display the long archive note, the full wording may live in an existing journal record while a panel offers the established summary. The panel must not introduce a new “catalog certainty” meter, progress bar, date-input field, or readiness badge.

The authored names in this plan are proposals for this story’s supporting cast and require the usual name/canon review. They do not identify existing characters with similar names or establish any off-page relation to the separate Margaret reference in ration-fraud records.

## 172. Continuity review: the same word can carry different work

“Code” may mean a date remembered by a person, a catalog reference, an instruction in the note, or an access credential in some other authority. The note itself does not resolve the meaning. The player’s notebook may quote the word but must not silently replace it with “keypad code.”

“Vault” may mean the named location or the community seed bank described by the fragment. The plan does not create a second vault to resolve the wording.

“First” may describe the note’s intended catalog, the proposed cover’s issue claim, or an unverified recollection. Each appearance retains its speaker or source.

“Margaret” remains a first name in the note. No surname is supplied. No connection is made to another file’s namesake.

These word-level distinctions are small enough to disappear in a summary. Content review must inspect quest descriptions, journal text, dialogue summaries, map labels, and any promotional copy together.

## 173. Content acceptance: authored material

Before the expansion is considered ready for implementation, reviewers should confirm that:
- the story gives the player sustained scenes with the supporting cast;
- the note is shown as an artifact and not paraphrased into a confirmed access code;
- the proposed catalog cover is visibly marked as a new, reviewable content insertion;
- at least one branch allows the player to preserve uncertainty in their own words;
- Teren’s demand for a stock count creates survival pressure without a fabricated count;
- Kesa’s cartographic restraint results in an observable map annotation or spoken equivalent;
- Sella’s garden thread receives an emotional conclusion without a seed reward;
- the final archive entries retain the building discrepancy and first-name boundary.

These are content acceptance conditions. They do not authorize new gameplay mechanics or claim that the current quest engine supports every presentation detail.

## 174. Content acceptance: evidence and implementation

The data owner must verify the current location and map-zone wording before any release. The narrative owner must confirm that the proposed cover is acceptable within canon or remove it and use the conditional ending. The journal and dialogue owner must identify existing delivery surfaces before content is integrated.

The work is not accepted if:
- it alters location descriptions to make them agree without an authorized content decision;
- it adds an assumed year to 18 April;
- it associates Margaret with a surname or with an unrelated record;
- it invents catalog contents, seed quantities, or present stock;
- it changes map discovery, route reachability, or access state;
- it introduces a code-entry interaction to make the note’s wording actionable;
- it uses a journal phrase that turns the proposed cover into confirmed source authority.

## 175. Handoff note

This is a content plan, not a production data patch. The implementing writer should create only the approved narrative records through current data conventions, with existing journal/quest presentation. Data owners should inspect the location, map-zone, and expedition records in place before writing cross-references. If the active quest authority has no supported route for the story, stop at the content package and report the route gap rather than building a parallel quest system.

No save schema, simulation, crop behavior, inventory, map reveal, travel edge, quest target, or reward is requested by this plan. The story is successful when the player leaves with a useful record of what is known, an honest account of what is unknown, and a clearer relationship with the people who keep those records.

