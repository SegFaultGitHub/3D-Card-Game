using System.Collections;
using System.Collections.Generic;
using Code.Cards.Collection;
using Code.Cards.Enums;
using Code.Extensions;
using UnityEngine;

namespace Code.Characters {
    public class Enemy : Character {
        public override IEnumerable<Character> Allies => new List<Character>(this.FightManager.Enemies);
        public override IEnumerable<Character> Enemies => new List<Character> { this.FightManager.Player };

        [field: SerializeField] private Tier Tier;

        public override void DrawCard() {
            if (this.Cards.Hand.Count >= this.Cards.MaxHandSize)
                return;
            if (this.Cards.Deck.Count == 0) {
                if (this.Cards.Discarded.Count == 0)
                    return;
                this.Cards.Deck = new List<Card>(this.Cards.Discarded);
                this.Cards.Discarded = new List<Card>();
            }

            Card card = Utils.Utils.Sample(this.Cards.Deck);
            card.Tier = this.Tier;
            card.Initialize();
            this.Cards.Deck.Remove(card);
            this.Cards.Hand.Add(card);
        }

        public override void TurnStarts() {
            base.TurnStarts();
            if (this.Stats.Dead) this.InSeconds(0.5f, () => this.FightManager.NextTurn());
            else this.InSeconds(0.5f, () => this.StartCoroutine(this.PlayAsynchronousTurn()));
        }

        private IEnumerator PlayAsynchronousTurn() {
            if (Simulation.MakeChoice(this)) {
                yield return new WaitUntil(() => !this.FightLocked);
                this.StartCoroutine(this.PlayAsynchronousTurn());
            } else {
                this.FightManager.NextTurn();
            }
        }
    }
}
