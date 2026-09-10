# LETTER IDENTITY AND RELATIONSHIP MAP

## 1. Identity Resolution Policy
Under Plan 150, sender and recipient tokens in letters are evaluated with strict provenance discipline:
1. **No Fuzzy Matching**: A sender like `a_woman ("A.")` is NEVER automatically bound to a live survivor whose name starts with "A".
2. **Preserve Archetypes & Anonymity**: Tokens like `a_conscript`, `a_father`, `a_dying_man`, `a_man_who_ran` remain deliberate archetypes of the wasteland experience.
3. **No Automatic NPC Creation**: Mentioning off-screen relatives (e.g. Pavel, Katerina, Loma grandfather, Dima) does not spawn game actors. They exist as memory anchors.
4. **Canonical Entity Cross-Anchors**: Known recurring characters in the Holdfast (Quartermaster Yelena, Engineer Tomas, Cook Boris, Dr. Vel/doctor, Captain) are documented as canonical context anchors without transferring mutable gameplay states.

---

## 2. Sender & Recipient Identity Census

| Letter ID | Sender Token | Sender Classification | Recipient Token | Recipient Classification | Narrative Relationship |
|---|---|---|---|---|---|
| `letter_01` | `a_daughter` ("R.") | Relationship Archetype | `Mother` | Off-screen relative | Mother-daughter; coat left on door hook. Linked to `letter_25`. |
| `letter_02` | `a_father` | Relationship Archetype | `My son, Pavel` | Named off-screen kin | Father-son; conflict over intake key. |
| `letter_03` | `an_elder_sister` ("Y.") | Relationship Archetype | `Katerina` | Named off-screen kin | Sisters; secret low flour reserves. |
| `letter_04` | `a_woman` ("A.") | Anonymous Lover Archetype | `M.` | Anonymous Lover Archetype | Domestic lovers in shelter; morning lamp/tea rituals. Linked to `05`, `24`. |
| `letter_05` | `a_man` ("M.") | Anonymous Lover Archetype | `A.` | Anonymous Lover Archetype | Bereaved partner; cold tea & stopped watch. Linked to `04`, `24`. |
| `letter_06` | `an_old_mother` ("Bubba") | Grandparent Archetype | `My Rima` | Shelter child (age 9) | Grandmother-granddaughter; planting potato in dirt. Linked to `15`. |
| `letter_07` | `a_conscript` | Military Archetype | `Mother and Father` | Off-screen relatives | Conscript at forward post; took Katerina's coat. Linked to list in `18`. |
| `letter_08` | `a_key_holder` | Shelter Worker Archetype | `the_quartermaster` ("Yelena") | Canonical character (Quartermaster) | Theft of 2 flour sacks for freezing night watch. |
| `letter_09` | `a_man_who_ran` | Moral Confession Archetype | `no_one` | Confessional void | Fled from screaming on road; shame and trauma. |
| `letter_10` | `a_doctor` | Shelter Physician Archetype | `no_one` / "the book" | Medical ledger | Triage confession; favored the strong; river woman died. Linked to `18`. |
| `letter_11` | `a_widow` | Bereaved Archetype | `My husband` | Deceased shelter dweller | Husband died in clinic, buried in unmarked south plot. |
| `letter_12` | `a_mother` | Bereaved Mother Archetype | `My Dima` (age 6) | Deceased child | Child died on Day 22; empty bed candle; meal counting. Linked to `18`. |
| `letter_13` | `a_dying_man` | Dying Traveler Archetype | `whoever finds this` | Prospective finder | Man dying in ditch; boots vs coat; road warning. Linked to `18`. |
| `letter_14` | `a_stranger` | Wasteland Scavenger Archetype | `the one who left the screws` | Fellow scavenger | Silent barter at printing works; screws for candle. Linked to `21`. |
| `letter_15` | `rima` | Shelter Child (age 9) | `Papa` | Missing parent | Child's journal; drawn smiling sun; dinner counting. Linked to `06`. |
| `letter_16` | `a_woman` ("A.") | Shelter Dweller Archetype | `Boris` | Canonical character (Kitchen staff) | Taking burned Tuesday bread for hungry Loma girl. |
| `letter_17` | `the_quartermaster` ("Y.") | Canonical character (Yelena) | `Tomas` | Canonical character (Engineer) | Engine bearing knock; demand for honest week margin in logbook. |
| `letter_18` | `the_duty_clerk` | Institutional Shelter Role | `the_captain` | Institutional Authority Role | Census list of deceased and unreturned shelter dwellers. |
| `letter_19` | `a_returning_woman` | Wasteland Traveler Archetype | `the house` | Anthropomorphized Structure | Returned to pre-war house with faded blue door and set table. |
| `letter_20` | `a_man` | Contrite Traveler Archetype | `the woman at the well` | Stranger at well | Stole water jug; spent hour refilling it; apology under jug. |
| `letter_21` | `the_scavenger` | Wasteland Scavenger Archetype | `the teacher` | Shelter Educator Archetype | Gift of two chalk sticks found in printing works desk. Linked to `14`. |
| `letter_22` | `an_old_woman` | Self-Reflective Elder | `myself, before` | Former self | Reflection at Day 100 on autumn planting and tears at the sprout. Linked to `06`. |
| `letter_23` | `a_man_leaving` | Fleeing Survivor Archetype | `the next one` | Future occupant | Cabin survival advice; inside-only latch; clean well; unseen neighbor. |
| `letter_24` | `a_woman` ("A.") | Anonymous Lover Archetype | `M.` | Anonymous Lover Archetype | Persistent devotion; filling lamp, winding watch, double shifts. Linked to `04`, `05`. |
| `letter_25` | `a_mother` | Protective Parent Archetype | `my daughter` | Fleeing daughter ("R.") | Day 3 urgent note: "Take the coat and go now... love is the going, go." Linked to `01`. |

---

## 3. Structural Safeguards
- Letters mentioning deaths (e.g. `letter_18` listing Dima, Loma grandfather, river woman) represent the clerk's historical records, not active event commands to kill live campaign survivors.
- Relationships depicted (e.g. A. and M., Bubba and Rima) are diegetic memories that enrich room exploration without overriding the simulation's dynamic relationship graphs.
