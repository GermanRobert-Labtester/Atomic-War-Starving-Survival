using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class VisitorCard
    {
        public string cardId;
        public string title;
        public string factionId; // e.g. military, rebels, terrorists, bandits, civilians, none
        public bool isSkirmish;
        public List<string> encounterIds = new List<string>();

        public VisitorCard() { }

        public VisitorCard(string cardId, string title, string factionId, bool isSkirmish = false)
        {
            this.cardId = cardId;
            this.title = title;
            this.factionId = factionId;
            this.isSkirmish = isSkirmish;
        }
    }

    [Serializable]
    public class VisitorDeck
    {
        public string deckId;
        public List<VisitorCard> cards = new List<VisitorCard>();

        public VisitorCard DrawRandom(System.Random rng)
        {
            if (cards == null || cards.Count == 0) return null;
            int index = rng.Next(0, cards.Count);
            return cards[index];
        }
    }
}
