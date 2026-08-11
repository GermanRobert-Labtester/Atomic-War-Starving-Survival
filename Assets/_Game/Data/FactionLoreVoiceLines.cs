// FactionLoreVoiceLines.cs — Lore-true radio intercept lines for Expansion II:
// The Weight of Factions. Fed by FactionRadioVoLibrarySO; tone is "nobody is
// a villain, everybody is a system" — no first-person villain speech. Lines
// are terse, radio-intercept style, and the civilian testimony fragments are
// the closest the game comes to a confession.
using System.Collections.Generic;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Static catalog of lore-true voice lines, grouped by faction system and
    /// civilian testimony fragment. Fed into <c>FactionRadioVoLibrarySO</c>
    /// at worldgen time; quoted verbatim by the FactionRadioInterceptSystem
    /// and the diagnostic overlays.
    /// </summary>
    public static class FactionLoreVoiceLines
    {
        // ── Central Garrison: processing the requisition ledger ──────────
        // "Three non-payments. The absence of protection is the bullet."
        public static readonly IReadOnlyList<string> CentralGarrisonProcessing = new[]
        {
            "Requisition ledger open. Slot forty-two. Receipt, please.",
            "Week nine. The shelter has not paid. Note it.",
            "Three non-payments. The absence of protection is the bullet.",
            "Patrol route: dropped to zero weight. Move on.",
            "Four weeks in compliance. The route is restored. Receipt.",
            "You are not punished. You are processed.",
            "Ledger position forty-two. Ledger position forty-three. Ledger open.",
            "The receipt is the only thing that is real out there."
        };

        // ── Upland Militia: the grain war ───────────────────────────────
        // "Ten percent today. Fifteen next week. It is rent, not a fine."
        public static readonly IReadOnlyList<string> UplandMilitiaGrainWar = new[]
        {
            "Ten percent today. Fifteen next week. It is rent, not a fine.",
            "The tithe keeps the road open. The road keeps the grain moving.",
            "The collector is not your enemy. The collector is the road.",
            "Refuse. Three days. The scavenger warlords will find you then.",
            "Two paid weeks and the patrol comes back. That is the bargain.",
            "We did not start the grain war. We are the only ones who can end it.",
            "You will not starve this winter if you pay. You might if you don't.",
            "The militia is not a government. The militia is a price."
        };

        // ── Cultists of the Glow: peace (so they call it) ────────────────
        // "Three visits. After that, the protection is a leash. We do not lie about it."
        public static readonly IReadOnlyList<string> CultistsOfTheGlowPeace = new[]
        {
            "Three visits. After that, the protection is a leash. We do not lie about it.",
            "Communion is on the seventh day. Bring the children. Bring the count.",
            "You missed the week. The deacon will come. The deacon always comes.",
            "Two weeks missed. You cannot leave. The contract was signed in your blood.",
            "The glow is currency. The glow is peace. The glow is yours if you let it.",
            "We are not a cult. We are a family that asks for a headcount.",
            "The bread is warm. The room is warm. The answer is warm.",
            "You can stay. You can always stay. That is the point."
        };

        // ── Scavenger Warlords: the arithmetic ──────────────────────────
        // "Pay. We do not burn. Pay. We do not take the children. Pay."
        public static readonly IReadOnlyList<string> ScavengerWarlordsArithmetic = new[]
        {
            "Pay. We do not burn. Pay. We do not take the children. Pay.",
            "Ninety percent is not full. The arithmetic remembers.",
            "Short week. The next ask is one-point-five. The week after, more.",
            "Eight times the base. That is the ceiling. That is the mercy.",
            "The code is the code. No kill if paying. Kill quickly if not.",
            "We always leave one thing. The code says so. Read it.",
            "You stopped paying. The shelter is no longer a shelter.",
            "Thursday the hatch opens. Thursday the hatch closes. We are on time."
        };

        // ── Civilian testimony fragments ────────────────────────────────
        // "Receipt, please." / "Smile and list." / "Warmth, then questions." / "Thursday."

        /// <summary>Garrison intake desk — receipt culture.</summary>
        public static readonly IReadOnlyList<string> GarrisonReceipt = new[]
        {
            "Receipt, please. I cannot be seen without the receipt.",
            "He stamped the paper twice. I think the second stamp was for me.",
            "Three strikes and the patrol does not come. We learned the third week.",
            "We paid for four weeks after. He did not say welcome back.",
            "The ledger has my name on it now. So I have a name."
        };

        /// <summary>Militia tax collector — the smile and the list.</summary>
        public static readonly IReadOnlyList<string> MilitiaSmileAndList = new[]
        {
            "He smiled when he took the grain. He always smiles.",
            "Fifteen percent this week. Twenty next. The list is on the wall.",
            "We refused once. The patrol did not come for three days. The warlords did.",
            "Two weeks of paying and the road is open again. I do not feel saved. I feel rented.",
            "The collector knows my children's names. That is the most expensive part."
        };

        /// <summary>Cult communion — warmth, then the question.</summary>
        public static readonly IReadOnlyList<string> CultCommunionWarmth = new[]
        {
            "The bread was warm. The deacon asked for the headcount. We gave it.",
            "Three visits and the room was always warm. Then we could not leave.",
            "Missed a week. The deacon came at dawn. He did not raise his voice.",
            "Two weeks missed. They did not say forbidden. They said consequence.",
            "The children were not taken. The children were counted. Same thing, almost."
        };

        /// <summary>Warlord hatch ritual — Thursday.</summary>
        public static readonly IReadOnlyList<string> WarlordThursdayHatch = new[]
        {
            "Thursday the hatch opens. Thursday the hatch closes.",
            "He left one thing on the table. The code says so. We did not read the code.",
            "Short pay. The next ask was one-point-five. I did the math in my head.",
            "Eight times the base. The ceiling. They said it like a kindness.",
            "We stopped paying. The shelter stopped being a shelter. Thursday."
        };

        /// <summary>Returns the right list for a faction id (or null).</summary>
        public static IReadOnlyList<string> LinesForFaction(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return null;
            switch (factionId)
            {
                case "central_garrison":
                case "military_remnants":
                    return CentralGarrisonProcessing;
                case "upland_militia":
                    return UplandMilitiaGrainWar;
                case "cult_of_the_glow":
                    return CultistsOfTheGlowPeace;
                case "scavenger_warlords":
                case "scavenger_camp":
                    return ScavengerWarlordsArithmetic;
                default:
                    return null;
            }
        }

        /// <summary>Returns the right civilian-testimony fragment list.</summary>
        public static IReadOnlyList<string> TestimonyForFaction(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return null;
            switch (factionId)
            {
                case "central_garrison":
                case "military_remnants":
                    return GarrisonReceipt;
                case "upland_militia":
                    return MilitiaSmileAndList;
                case "cult_of_the_glow":
                    return CultCommunionWarmth;
                case "scavenger_warlords":
                case "scavenger_camp":
                    return WarlordThursdayHatch;
                default:
                    return null;
            }
        }
    }
}
