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
    }
}
