namespace Ashfall.Core.Journal
{
    /// <summary>
    /// Trait-driven diegetic copy for journal discoveries. Cold, exhausted,
    /// human — no fourth-wall tutorial language. Tone shifts with RiskBiasTrait.
    /// </summary>
    public static class JournalVoice
    {
        /// <summary>
        /// Compose body text (without leading "Day N.") for a knowledge key.
        /// </summary>
        public static string ComposeBody(string knowledgeKey, RiskBiasTrait bias)
        {
            switch (knowledgeKey)
            {
                case KnowledgeKeys.HighCo2:
                    return HighCo2(bias);
                case KnowledgeKeys.HasSeenRadiation:
                    return SeenRadiation(bias);
                case KnowledgeKeys.HasExperiencedStorm:
                    return ExperiencedStorm(bias);
                case KnowledgeKeys.FilterFailing:
                    return FilterFailing(bias);
                case KnowledgeKeys.FreezingShelter:
                    return FreezingShelter(bias);
                case KnowledgeKeys.ContinuityReclamationDecree:
                    return ContinuityReclamationDecree(bias);
                case KnowledgeKeys.HydroBaronRateCardOrigin:
                    return HydroBaronRateCardOrigin(bias);
                case KnowledgeKeys.DeserterCoalitionFounding:
                    return DeserterCoalitionFounding(bias);
                case KnowledgeKeys.ColdCountBeforeTheLab:
                    return ColdCountBeforeTheLab(bias);
                case KnowledgeKeys.ProvisionedAdvanceKnowledge:
                    return ProvisionedAdvanceKnowledge(bias);
                case KnowledgeKeys.CheckpointConscriptsConfession:
                    return CheckpointConscriptsConfession(bias);
                case KnowledgeKeys.QuartermastersPaperwork:
                    return QuartermastersPaperwork(bias);
                case KnowledgeKeys.InterceptedCipher:
                    return InterceptedCipher(bias);
                case KnowledgeKeys.LedgerNobodySigned:
                    return LedgerNobodySigned(bias);
                default:
                    return "Something changed. I wrote it down so I would not forget.";
            }
        }

        /// <summary>
        /// Full entry text: "Day N. …" matching the acceptance example shape.
        /// </summary>
        public static string ComposeFullText(string knowledgeKey, RiskBiasTrait bias, int day)
        {
            string body = ComposeBody(knowledgeKey, bias);
            if (string.IsNullOrEmpty(body)) body = "I marked the day. That is all.";
            int d = day > 0 ? day : 1;
            // Body may already start with a sentence; prefix day stamp once.
            if (body.StartsWith("Day "))
                return body;
            return $"Day {d}. {body}";
        }

        public static string FormatTimestamp(int day, float hour = -1f)
        {
            int d = day > 0 ? day : 1;
            if (hour < 0f) return $"Day {d}";
            int h = (int)hour;
            if (h < 0) h = 0;
            if (h > 23) h = h % 24;
            return $"Day {d}, {h:00}h";
        }

        private static string HighCo2(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "The air is poison. Thick. My skull is a vice. We crack the vents or we choke — ash or no ash.";
                case RiskBiasTrait.Cautious:
                    return "My head is pounding. The air feels thick. We need to open the vents, even if the ash gets in.";
                case RiskBiasTrait.Realist:
                    return "CO₂ is climbing — headache, heavy air. Crack a vent or the filter is finished. Ash comes with it.";
                case RiskBiasTrait.Reckless:
                    return "Head's fuzzy. Air's garbage. Crack the vents. We'll deal with the ash when it lands.";
                case RiskBiasTrait.Denialist:
                    return "Just a stuffy room. Open a vent if you want. I am fine. The filter will sort it.";
                case RiskBiasTrait.Fatalist:
                    return "Air goes bad. Head pounds. Vents or no vents, the ash finds us either way.";
                default:
                    return "My head is pounding. The air feels thick. We need to open the vents, even if the ash gets in.";
            }
        }

        private static string SeenRadiation(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "The dosimeter twitched. Or I imagined it. Either way I will not take my coat off indoors.";
                case RiskBiasTrait.Cautious:
                    return "I felt the dose climb. Not much — enough. We log it, we scrub, we do not pretend it is nothing.";
                case RiskBiasTrait.Realist:
                    return "Radiation is on us now. Small number, real number. Keep the suits sealed when we go out.";
                case RiskBiasTrait.Reckless:
                    return "Got a tick on the counter. Still standing. Wash the boots and keep moving.";
                case RiskBiasTrait.Denialist:
                    return "The needle moved. Instruments lie. I feel fine.";
                case RiskBiasTrait.Fatalist:
                    return "The dose goes up. It always does. Write it down so the next one knows.";
                default:
                    return "I felt the dose climb. We log it and we scrub.";
            }
        }

        private static string ExperiencedStorm(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "The sky is eating the world. Fallout on the roof. Do not open anything. Not the hatch. Not a crack.";
                case RiskBiasTrait.Cautious:
                    return "Storm hit. Ash and worse. Seal the intake if we can. No trips until it breaks.";
                case RiskBiasTrait.Realist:
                    return "Fallout storm. Outdoor exposure spikes. Stay under concrete until the wind dies.";
                case RiskBiasTrait.Reckless:
                    return "Ugly sky. Storm. If someone has to go out, make it short and make them count.";
                case RiskBiasTrait.Denialist:
                    return "Weather's loud. It will pass. Always does.";
                case RiskBiasTrait.Fatalist:
                    return "Storm again. The ash settles on everything. We wait. That is the work.";
                default:
                    return "Storm hit. Seal what we can and wait it out.";
            }
        }

        private static string FilterFailing(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "The filter is dying. I can taste dust. If it fails we breathe the outside — and the outside wants us dead.";
                case RiskBiasTrait.Cautious:
                    return "Filter health is low. Spare cartridges, or we start sharing headaches with the ash.";
                case RiskBiasTrait.Realist:
                    return "Air filter is wearing out. Scrap and a swap, or indoor air turns against us.";
                case RiskBiasTrait.Reckless:
                    return "Filter's shot soon. Swap it when we can. Or don't. Throat's already rough.";
                case RiskBiasTrait.Denialist:
                    return "Filter light is yellow. Yellow is fine. Yellow is not red.";
                case RiskBiasTrait.Fatalist:
                    return "Filter fails eventually. Everything does. Still — write it before the air does.";
                default:
                    return "Filter health is low. We need a spare before the air turns.";
            }
        }

        private static string FreezingShelter(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "Cold is in the walls. Heater dead or starving. We freeze careful or we freeze stupid.";
                case RiskBiasTrait.Cautious:
                    return "Indoor cold is biting. Fuel the heater. Stack bodies if we must. Do not sleep wet.";
                case RiskBiasTrait.Realist:
                    return "Bunker temperature is below safe. Heater fuel or we lose fingers and morale with them.";
                case RiskBiasTrait.Reckless:
                    return "Cold as a grave. Fire up the heater or burn something. I'm not sitting still for this.";
                case RiskBiasTrait.Denialist:
                    return "A bit chilly. Put on another layer. The heater can wait.";
                case RiskBiasTrait.Fatalist:
                    return "Cold comes in. We write it. We feed the stove if there is fuel. If not, we endure.";
                default:
                    return "Indoor cold is biting. Fuel the heater before sleep.";
            }
        }

        // ── Expansion 06 — The Muster (Section III / XI) ───────────────
        // Three witnesses, no verdict. Same world state, different felt
        // danger: the trait system does the weighting, never new branching
        // logic. An Empath author does not write the dark accounts down at
        // all; a Sociopath records them as transactions.

        private static string ContinuityReclamationDecree(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "Voss's bulletin is gone. Harven's is up, and it says nothing about why. That is a coup that signs its own name.";
                case RiskBiasTrait.Cautious:
                    return "Command changed hands overnight. All orders void, permits re-issued. We get new papers before we move anywhere.";
                case RiskBiasTrait.Realist:
                    return "Garrison command changed. Old permits void; re-issue at Checkpoint Gamma. Standard succession, fast execution.";
                case RiskBiasTrait.Reckless:
                    return "New colonel, same checkpoints. Papers, no papers — we go where we need to go.";
                case RiskBiasTrait.Denialist:
                    return "New name on the bulletin. Command changes hands. Nothing about our shelter changes.";
                case RiskBiasTrait.Fatalist:
                    return "Harven signs, Voss does not. Whoever signs, the checkpoints stay. We queue either way.";
                case RiskBiasTrait.Empath:
                    return "Someone lost command of something they built. The board is repapered and nobody says goodbye.";
                default:
                    return "Garrison command changed. Old permits void; re-issue at Checkpoint Gamma.";
            }
        }

        private static string HydroBaronRateCardOrigin(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "The rate card is older than the Exchange — a drought surcharge from a company called Halloway. The apocalypse changed nothing. They were always deciding who paid more.";
                case RiskBiasTrait.Cautious:
                    return "The card predates the war. A pre-Exchange contractor's sheet, unrevised. Worth remembering when we negotiate.";
                case RiskBiasTrait.Realist:
                    return "Six-year-old surcharge sheet, still enforced. The price of water was decided before the water was poisoned.";
                case RiskBiasTrait.Reckless:
                    return "An old company's old sheet still sets our water price. Somebody should revise it with a crowbar.";
                case RiskBiasTrait.Denialist:
                    return "A piece of paper from before. Rates are rates. We pay what everyone pays.";
                case RiskBiasTrait.Fatalist:
                    return "The card was old before the bombs. It will be old after. We drink on their terms or not at all.";
                case RiskBiasTrait.Empath:
                    return "Odalen explains the queue the way someone explains a family arrangement — tired, patient, four thousand times.";
                default:
                    return "The rate card predates the Exchange by six years. Nobody has revised a single line of it.";
            }
        }

        private static string DeserterCoalitionFounding(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "A tally scratched into the transformer housing — one mark per person who stayed. The first mark is dated to an embargo that 'never happened.' It happened.";
                case RiskBiasTrait.Cautious:
                    return "The substation holds a marked count of people who refuse to return to garrison. We count them carefully — they are not the garrison's friends.";
                case RiskBiasTrait.Realist:
                    return "Deserters have been filtering to this substation since the fuel embargo. The tally is honest about who stays.";
                case RiskBiasTrait.Reckless:
                    return "A tally of people who walked off. Good for them. Someone should keep count of the ones who were right.";
                case RiskBiasTrait.Denialist:
                    return "Scratches on a wall. Could be anyone. People exaggerate numbers in the dark.";
                case RiskBiasTrait.Fatalist:
                    return "Marks on a wall, one per deserter. The wall fills or the wall doesn't. We all end up counted somewhere.";
                case RiskBiasTrait.Empath:
                    return "Each mark is someone who made it here and stayed. The first one dates to a thing that never officially happened.";
                default:
                    return "Someone has scratched a tally into the transformer housing — one mark per person who made it here and stayed.";
            }
        }

        private static string ColdCountBeforeTheLab(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "Four researchers signed up for an isotope survey and never left. Their instruments were bought, not salvaged. The lab outlived the agency that ordered it — ask why nobody came back.";
                case RiskBiasTrait.Cautious:
                    return "The lab predates the Exchange. Four staff, bought instruments, a survey mandate. We log what they tell us and verify it ourselves.";
                case RiskBiasTrait.Realist:
                    return "The Cold Count predates the war: four surveyors who stayed at their post. Their instruments outlasted their agency.";
                case RiskBiasTrait.Reckless:
                    return "Four scientists, one lab, bought gear, no exit plan. Either the smartest or the most stubborn people in the sector.";
                case RiskBiasTrait.Denialist:
                    return "A survey team that kept working. People do that. It does not mean the sky is falling — it already fell, and we are fine.";
                case RiskBiasTrait.Fatalist:
                    return "Four people measured what was coming and stayed to watch it arrive. That is either purpose or refusal. Either way, they wrote it down.";
                case RiskBiasTrait.Empath:
                    return "They must have known. They bought instruments instead of tickets out.";
                default:
                    return "Four researchers signed up for a civil-defence isotope survey and never left. The lab outlived the agency that commissioned it.";
            }
        }

        private static string ProvisionedAdvanceKnowledge(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "The homestead was resupplied every winter for a decade before the Exchange — by nobody the sector can identify. Someone knew. Someone is still supplying it. Find out who.";
                case RiskBiasTrait.Cautious:
                    return "The log shows years of stocked runs before the Allocation Schedule existed. The Provisioned knew before anyone. We watch what they do with that.";
                case RiskBiasTrait.Realist:
                    return "The homestead's stock runs predate the war. The Provisioned spent a decade preparing alone and never told anyone.";
                case RiskBiasTrait.Reckless:
                    return "They saw it coming and stocked for it, and never warned a soul. Either brilliant or as selfish as they come — probably both.";
                case RiskBiasTrait.Denialist:
                    return "A well-stocked shelter. Lucky people. It does not mean they knew anything special.";
                case RiskBiasTrait.Fatalist:
                    return "Somebody prepared for ten years and it still was not enough to stop the Exchange. Prepare or not, the ash does not read logs.";
                case RiskBiasTrait.Empath:
                    return "Someone believed this was coming with enough certainty to spend a decade on it — and never once sold that certainty to anyone who could have used it.";
                default:
                    return "The homestead's log predates the Allocation Schedule by three winters of stocked supply runs.";
            }
        }

        private static string CheckpointConscriptsConfession(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "There it is. Voss refused to fire on a grain convoy and his own staff shot him for it. The boy confessed it over drinks. They killed their commander for a grain convoy — what will they do to us.";
                case RiskBiasTrait.Cautious:
                    return "A conscript says Voss was killed by his own men for refusing an order. Unverifiable, unretractable now that it is written. We treat it as an open question.";
                case RiskBiasTrait.Realist:
                    return "Account one of three: the conscript's. Voss refused a firing order and was shot by his own staff. One witness, at a checkpoint, drunk.";
                case RiskBiasTrait.Reckless:
                    return "They shot their own colonel for not shooting civilians. Whatever else that is, it is a story worth repeating.";
                case RiskBiasTrait.Denialist:
                    return "A drunk boy's story about his officers. Soldiers talk. Most of it is barracks noise.";
                case RiskBiasTrait.Fatalist:
                    return "However Voss died, he is dead, and the conscript will not sleep better for telling us. Neither will we.";
                case RiskBiasTrait.Empath:
                    return "He said it like a confession, then asked me not to repeat it. I cannot promise him that — it is already in this journal.";
                case RiskBiasTrait.Sociopath:
                    return "Source at the checkpoint, low value, volunteered after alcohol. Claims a commander was shot by his own staff over a convoy order. Unverified. Keep the source warm; he will talk again.";
                default:
                    return "A conscript at Checkpoint Gamma: Voss was shot by his own staff for refusing a direct order to fire on a grain convoy.";
            }
        }

        private static string QuartermastersPaperwork(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "The quartermaster is too calm. 'Reassignment, coastal liaison.' Nobody has heard from Voss since. Paperwork that tidy is paperwork built to bury something.";
                case RiskBiasTrait.Cautious:
                    return "Account two: Voss requested reassignment to the coast at the start of Phase V. Paperwork is real; whereabouts unknown. We keep both accounts.";
                case RiskBiasTrait.Realist:
                    return "The quartermaster's record: Voss rotated to a coastal liaison post, Day 240. Plausible, ordinary, and impossible to check from here.";
                case RiskBiasTrait.Reckless:
                    return "Officers leave the worst postings all the time. Maybe he did. Maybe the paperwork says so because the paperwork was told to.";
                case RiskBiasTrait.Denialist:
                    return "There — a reassignment order. He left for the coast like any officer. The other story is a drunk kid's imagination.";
                case RiskBiasTrait.Fatalist:
                    return "Rotate out, rotate in; the garrison grinds on either way. Wherever Voss is, the paper will not say more than it says.";
                case RiskBiasTrait.Empath:
                    return "The quartermaster finds it entirely unremarkable. That is the most remarkable thing about it.";
                case RiskBiasTrait.Sociopath:
                    return "Contact at the motor pool: reassignment order, coastal liaison, Day 240. Paperwork in order. Useful only as evidence of how well the garrison files things it does not want read.";
                default:
                    return "The quartermaster: Voss requested reassignment to the coastal evacuation liaison post at the outset of Phase V. Nobody has heard from him since.";
            }
        }

        private static string InterceptedCipher(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "A cipher retired with Voss's command, transmitted three weeks after Day 240, logged and never forwarded. It is him, or someone with his keys. Either way someone is alive out there using a dead man's code.";
                case RiskBiasTrait.Cautious:
                    return "Account three: an unregistered burst using a retired cipher, logged by signals, never forwarded. Ruhl's assessment is the most honest thing in the file.";
                case RiskBiasTrait.Realist:
                    return "The intercept: low-power, unregistered, cipher retired with Voss's command. Could be him; could be a code never properly decommissioned.";
                case RiskBiasTrait.Reckless:
                    return "A ghost is broadcasting on a dead cipher. Ruhl says she logs ghosts, not chases them. Somebody ought to chase this one.";
                case RiskBiasTrait.Denialist:
                    return "A radio signal on an old code. Equipment quirks. Static in, static out. It does not mean anything.";
                case RiskBiasTrait.Fatalist:
                    return "Whoever it is, on a dead cipher, they will answer when they want to be answered. Logged or not, it changes nothing we can do.";
                case RiskBiasTrait.Empath:
                    return "Whoever is out there still signs with a dead commander's code. That is either respect or a very old habit.";
                case RiskBiasTrait.Sociopath:
                    return "Intercept, low power, cipher retired with the old command. Ruhl logs it; I log that Ruhl logs it. If it is him, he owes us. If it is not him, someone useful is alive.";
                default:
                    return "An unregistered, low-power signal using an authentication cipher retired with Voss's old command. Ruhl: 'I don't chase ghosts. I log them.'";
            }
        }

        private static string LedgerNobodySigned(RiskBiasTrait bias)
        {
            switch (bias)
            {
                case RiskBiasTrait.Paranoid:
                    return "One code, three ledgers, marked PAID in three hands — and nobody can say who opened the account. The oldest page is signed with one initial. They are hiding something and they have been for years.";
                case RiskBiasTrait.Cautious:
                    return "A debt code appears in three independent ledgers, always PAID, never identifiable. We note it and do not touch it.";
                case RiskBiasTrait.Realist:
                    return "The ledger nobody signed: three matching pages, one code, three different hands, all PAID, account untraceable. The ink on the oldest is the best-preserved thing in the cache.";
                case RiskBiasTrait.Reckless:
                    return "Somebody paid something for years and nobody claims it. The Tally does not know, Iversen does not know, the cache does not know. I want to know.";
                case RiskBiasTrait.Denialist:
                    return "Old paperwork, old debts, old ink. It was settled before any of this mattered. Close the file.";
                case RiskBiasTrait.Fatalist:
                    return "A debt without a debtor is still a debt. It will surface when it wants to surface. These pages have outlived everyone who could explain them.";
                case RiskBiasTrait.Empath:
                    return "Someone kept records this long — through a war, through a change of command — of a debt nobody claims. That is a life's worth of care about something.";
                default:
                    return "Three pages, one code, marked PAID in three different hands. Nobody currently employed by any of the three ledgers can say who opened the account.";
            }
        }
    }
}
