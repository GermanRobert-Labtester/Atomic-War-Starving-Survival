using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// Expansion XI — "The Glass Orchard": authored narrative + mechanic events.
    ///
    /// These <see cref="GameEvent"/>s are registered into the event pool by
    /// <c>EventPoolBuilder.Build</c>. Some are fired directly by the host in
    /// response to <c>GreenhouseSystem</c> state changes (first sprout, blight
    /// outbreak, tainted harvest, glass breaking); others are pooled and
    /// weight-selected (the Rot Farmers' offering, the dead gardener).
    ///
    /// System-mutating resolutions (TreatBlight / Clear / UnlockPreWarWheat /
    /// SurgeContamination) are applied by the host's <c>OnChoiceApplied</c>
    /// handler in <c>GameBootstrap.Greenhouse</c>; this factory only authors the
    /// narrative, the morale/item/trust/flag deltas, and the gates — the same
    /// split the Holdfast events use between authored text and host-side calls.
    ///
    /// The id string constants below are authored content that mirrors
    /// <c>GreenhouseExpansionCatalog</c> (which lives in the Core assembly; the
    /// Events assembly does not reference Core, so the factory stays
    /// self-contained to avoid a circular assembly dependency).
    /// </summary>
    public static class GreenhouseEventFactory
    {
        // ── Event ids ──────────────────────────────────────────────────
        private const string EFirstSprout = "greenhouse_first_sprout";
        private const string EBlightOutbreak = "greenhouse_blight_outbreak";
        private const string ETaintedHarvest = "greenhouse_tainted_harvest";
        private const string ETheOffering = "greenhouse_the_offering";
        private const string EDeadGardener = "greenhouse_dead_gardener";
        private const string EGlassBreaks = "greenhouse_glass_breaks";

        // ── Flag ids ───────────────────────────────────────────────────
        private const string FFirstSproutSeen = "flag_greenhouse_first_sprout_seen";

        // ── Item ids ───────────────────────────────────────────────────
        private const string ISeedGrain = "item_seed_grain";
        private const string ISeedWheat = "item_seed_wheat";
        private const string IBlightTreatment = "item_blight_treatment";
        private const string ILeadGlassPane = "item_lead_glass_pane";
        private const string ITaintedFood = "tainted_food";

        // ── Faction / other ids ────────────────────────────────────────
        private const string FRotFarmers = "rot_farmers";
        private const string ICleanWater = "clean_water";

        public static List<GameEvent> CreateAll()
        {
            var list = new List<GameEvent>(6)
            {
                CreateFirstSprout(),
                CreateBlightOutbreak(),
                CreateTaintedHarvest(),
                CreateTheOffering(),
                CreateDeadGardener(),
                CreateGlassBreaks()
            };
            return list;
        }

        // ───────────────────────────────────────────────────────────────
        // 1. The first sprout — fired once by the host on the first plant.
        // ───────────────────────────────────────────────────────────────
        private static GameEvent CreateFirstSprout()
        {
            var ev = NewEvent(
                EFirstSprout,
                "The First Sprout",
                "It is the colour of nothing else in the bunker — a thin, pale curl of green pushing up through the sifted earth of the planter box. Nobody speaks for a long moment. Then someone says, very quietly, that they had forgotten what it looked like.",
                weight: 0.4f, minDay: 1);
            // Fire-once: only while the seen-flag is not set; each choice sets it.
            ev.conditions.BlockedFlagId = FFirstSproutSeen;

            ev.choices.Add(C("share_it", "Share the first taste around, a sliver each.",
                moraleDelta: 5f,
                setFlags: FlagArr(FFirstSproutSeen)));
            ev.choices.Add(C("save_the_seed", "Save it. Tomorrow is longer than today.",
                moraleDelta: -2f,
                setFlags: FlagArr(FFirstSproutSeen)));
            ev.choices.Add(C("gift_rot_farmers", "Carry it down to the Rot Farmers, as an offering.",
                moraleDelta: 2f,
                factionId: FRotFarmers, trustDelta: 8f,
                setFlags: FlagArr(FFirstSproutSeen)));
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // 2. Blight outbreak — fired by the host when blight first appears.
        // ───────────────────────────────────────────────────────────────
        private static GameEvent CreateBlightOutbreak()
        {
            var ev = NewEvent(
                EBlightOutbreak,
                "The Grey Fuzz",
                "A soft grey mould is spreading across one of the plots, dulling the green to the colour of old ash. It moves faster than anything should in a place this cold. Left alone, it will take the whole box by morning.",
                weight: 0f, minDay: 1);

            // The treatment is consumed by the host only if TreatBlight succeeds
            // (a Failed crop is dead), so no consume effect here — just the gate.
            ev.choices.Add(C("treat_it", "Drench it in the copper wash.",
                requiredItemId: IBlightTreatment));
            ev.choices.Add(C("burn_crop", "Burn it out before it spreads.",
                moraleDelta: -4f));
            ev.choices.Add(C("leave_it", "Leave it. Maybe it stops on its own.",
                moraleDelta: -1f));
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // 3. Tainted harvest — pooled; the soil remembered the ash.
        // ───────────────────────────────────────────────────────────────
        private static GameEvent CreateTaintedHarvest()
        {
            var ev = NewEvent(
                ETaintedHarvest,
                "The Harvest Came Up Wrong",
                "The crop pulled clean from the box, but the grain is dark at the root, and the water it drank was not the water you would have chosen. It is food. It is also, quietly, something else.",
                weight: 0.4f, minDay: 20);

            ev.choices.Add(C("feed_it", "It is still food. Feed it to them and say nothing.",
                moraleDelta: -3f,
                effects: EffArr(Eff(need: "radiation", delta: 4f))));
            ev.choices.Add(C("compost_it", "Till it back under. Let the box try again.",
                moraleDelta: 1f,
                effects: EffArr(Consume(ITaintedFood, 1))));
            ev.choices.Add(C("discard", "Throw it out. Better hungry than slow.",
                moraleDelta: -1f,
                effects: EffArr(Consume(ITaintedFood, 1))));
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // 4. The Rot Farmers' offering — pooled, trust-flavoured trade.
        // ───────────────────────────────────────────────────────────────
        private static GameEvent CreateTheOffering()
        {
            var ev = NewEvent(
                ETheOffering,
                "The Offering",
                "A Rot Farmer waits at the edge of the ash, holding out a twist of cloth. Inside: seed stock, dark and viable, grown in soil no one else would touch. 'It grows,' she says. 'That is all that matters now.'",
                weight: 0.5f, minDay: 25);

            ev.choices.Add(C("accept", "Take the seed. Do not ask what it grew in.",
                moraleDelta: -1f,
                factionId: FRotFarmers, trustDelta: -4f,
                effects: EffArr(Eff(item: ISeedGrain, amount: 2))));
            ev.choices.Add(C("refuse", "Refuse. Some soil remembers too much.",
                moraleDelta: 2f,
                factionId: FRotFarmers, trustDelta: 6f));
            ev.choices.Add(C("trade_water", "Trade clean water for the cleanest of it.",
                requiredItemId: ICleanWater,
                factionId: FRotFarmers, trustDelta: 2f,
                effects: EffArr(
                    Consume(ICleanWater, 1),
                    Eff(item: ISeedGrain, amount: 2))));
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // 5. The dead gardener — pooled discovery; the in-game wheat-unlock path.
        // ───────────────────────────────────────────────────────────────
        private static GameEvent CreateDeadGardener()
        {
            var ev = NewEvent(
                EDeadGardener,
                "The Dead Gardener",
                "In the back of the glasshouse ruins, slumped against a planter long since cracked, is the gardener. In one stiff hand: a note. In the other, clutched tight as a secret, a sealed tin of seed. The note says only: these are from before. Grow them. Remember what for.",
                weight: 0.6f, minDay: 18);

            ev.choices.Add(C("take_seed_tin", "Take the tin. Keep the note.",
                moraleDelta: 3f,
                effects: EffArr(
                    Eff(item: ISeedWheat, amount: 1),
                    Eff(setFlag: "flag_greenhouse_wheat_unlocked", value: true))));
            ev.choices.Add(C("take_common_seed", "Take the common seed. Leave the sealed tin with her.",
                moraleDelta: 1f,
                effects: EffArr(Eff(item: ISeedGrain, amount: 2))));
            ev.choices.Add(C("leave_it", "Leave it. A grave is a grave.",
                moraleDelta: 2f));
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // 6. The glass breaks — pooled; ozone/ash tie-in.
        // ───────────────────────────────────────────────────────────────
        private static GameEvent CreateGlassBreaks()
        {
            var ev = NewEvent(
                EGlassBreaks,
                "The Glass Breaks",
                "A pane of the lead-glass gives way with a sound like a held breath finally let go. Through the crack, the ash drifts in, and behind the ash, the thin killing light of a sky that has forgotten clouds. The crops below stir, then wilt at the edges.",
                weight: 0.5f, minDay: 30);
            ev.conditions.RequireExtremeWeather = true;

            ev.choices.Add(C("patch_it", "Cut a new pane to size and seal the breach.",
                moraleDelta: 1f,
                requiredItemId: ILeadGlassPane,
                effects: EffArr(Consume(ILeadGlassPane, 1))));
            ev.choices.Add(C("sacrifice_crop", "Pull a plot's worth of glass to cover the gap.",
                moraleDelta: -5f));
            ev.choices.Add(C("do_nothing", "Hold. The glass has held this long.",
                moraleDelta: -2f));
            return ev;
        }

        // ═══════════════════════════════════════════════════════════════
        // Builders — keep authored events compact and reviewable.
        // GameEvent is a ScriptableObject; EventChoice / EventEffect are
        // [Serializable] POCOs (new'd directly).
        // ═══════════════════════════════════════════════════════════════

        private static GameEvent NewEvent(string id, string title, string body, float weight, int minDay)
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = id;
            ev.title = title;
            ev.bodyText = body;
            ev.weight = weight;
            ev.conditions = new EventConditions { MinDay = minDay };
            ev.choices = new List<EventChoice>();
            return ev;
        }

        private static EventChoice C(
            string choiceId, string text,
            float moraleDelta = 0f,
            string factionId = null, float trustDelta = 0f,
            string requiredItemId = null,
            List<EventEffect> effects = null,
            List<string> setFlags = null)
        {
            return new EventChoice
            {
                ChoiceId = choiceId,
                Text = text,
                MoraleDelta = moraleDelta,
                FactionId = factionId,
                TrustDelta = trustDelta,
                RequiredItemId = requiredItemId,
                Effects = effects ?? new List<EventEffect>(),
                SetEventFlags = setFlags ?? new List<string>()
            };
        }

        private static EventEffect Eff(string need = null, float delta = 0f,
            string item = null, int amount = 0,
            string setFlag = null, bool value = true)
        {
            var e = new EventEffect();
            if (!string.IsNullOrEmpty(need)) { e.TargetNeed = need; e.NeedDelta = delta; }
            if (!string.IsNullOrEmpty(item)) { e.ItemId = item; e.ItemAmount = amount; }
            if (!string.IsNullOrEmpty(setFlag)) { e.SetWorldFlag = setFlag; e.WorldFlagValue = value; }
            return e;
        }

        /// <summary>Consume (-amount) of an item.</summary>
        private static EventEffect Consume(string itemId, int amount) =>
            Eff(item: itemId, amount: -amount);

        private static List<EventEffect> EffArr(params EventEffect[] effects) =>
            new List<EventEffect>(effects);

        private static List<string> FlagArr(params string[] flags) =>
            new List<string>(flags);
    }
}
