using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Quests
{
    /// <summary>
    /// quest_garrison_last_order — The Last Order (Faction: Garrison).
    /// Stage 1: Intercept a military frequency on a 6-hour loop; cipher key
    ///          split across 3 checkpoints.
    /// Stage 2: Send expeditions to each checkpoint; defended by
    ///          npc_burned_patrol.
    /// Stage 3: Decode the message: destroy the Tessarat water treatment
    ///          plant to deny the enemy. The command structure no longer
    ///          exists.
    /// Stage 4: Choose. Destroy (denies water to all, +Garrison trust),
    ///          refuse (Garrison hostile, water stays), or broadcast and
    ///          let the wasteland decide.
    /// </summary>
    public class Quest_GarrisonLastOrder : QuestRuntime
    {
        public const string Id = "quest_garrison_last_order";
        public const string CheckpointA = "checkpoint_alpha";
        public const string CheckpointB = "checkpoint_bravo";
        public const string CheckpointC = "checkpoint_charlie";
        public const string TessaratPlant = "loc_tessarat_water_plant";
        public const string BurnedPatrolNpc = "npc_burned_patrol";

        public Quest_GarrisonLastOrder()
        {
            Def = new QuestDef
            {
                Id = Id,
                DisplayName = "The Last Order",
                Description = "A military frequency still loops, six hours on, six hours off. The order it carries is from a chain of command that no longer exists.",
                FactionId = "faction_garrison",
                MaxStages = 4
            };
        }

        protected override void OnBegin()
        {
            SetProgress(CheckpointA, 0);
            SetProgress(CheckpointB, 0);
            SetProgress(CheckpointC, 0);
            RecordMoralEntry?.Invoke("intercepted military frequency on a 6-hour loop");
        }

        protected override void OnStageEnter(int stage)
        {
            switch (stage)
            {
                case 2:
                    RecordMoralEntry?.Invoke("dispatched expeditions to three checkpoints");
                    break;
                case 3:
                    RecordMoralEntry?.Invoke("decoded: 'Destroy the Tessarat water treatment plant to deny the enemy.'");
                    break;
                case 4:
                    OfferChoice("destroy");
                    OfferChoice("refuse");
                    OfferChoice("broadcast");
                    break;
            }
        }

        protected override void OnSuccess() { }

        protected override void OnFailure() { }

        // ── Choice resolvers (host-invoked) ──────────────────────────────
        public void ResolveDestroy()
        {
            MarkLocationDestroyed?.Invoke(TessaratPlant, "water_plant");
            AddFactionTrust?.Invoke("faction_garrison", +30f);
            SubtractFactionTrust?.Invoke("faction_militia", -25f);
            SubtractFactionTrust?.Invoke("faction_survivors", -15f);
            RecordMoralEntry?.Invoke("obeyed the order. the water is gone.");
            Complete();
        }

        public void ResolveRefuse()
        {
            SubtractFactionTrust?.Invoke("faction_garrison", -40f);
            AddFactionTrust?.Invoke("faction_survivors", +15f);
            RecordMoralEntry?.Invoke("refused the order. the water stayed.");
            Complete();
        }

        public void ResolveBroadcast()
        {
            BroadcastRadioMessage?.Invoke("faction_survivors", "last_order_disclosed", 0.7f);
            RecordMoralEntry?.Invoke("broadcast the order on the open frequency. the wasteland knows.");
            Complete();
        }
    }
}
