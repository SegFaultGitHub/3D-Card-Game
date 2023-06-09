using System;
using System.Collections.Generic;
using System.Linq;
using Code.Artifacts;
using Code.Cards.Collection;
using Code.Cards.UI;
using Code.Fight;
using Code.Utils;
using UnityEngine;

namespace Code.Characters {
    public class Player : Character {
        public override IEnumerable<Character> Allies => new List<Character> { this.FightManager.Player };
        public override IEnumerable<Character> Enemies => new List<Character>(this.FightManager.Enemies);
        [field: SerializeField] public _UI UI { get; private set; }
        [Serializable]
        public class _Loot {
            public enum LootType {
                Card, Artifact
            }
            [field: SerializeField] public int Count;
            [field: SerializeField] public List<WeightDistribution<Card>> CardLoot;
            [field: SerializeField] public List<WeightDistribution<Artifact>> ArtifactLoot;
            [field: SerializeField] public List<WeightDistribution<LootType>> LootDistribution;

            public List<Loot> GenerateLoot() {
                LootType type = this.ArtifactLoot.Count == 0 ? LootType.Card : Utils.Utils.Sample(this.LootDistribution);
                return type switch {
                    LootType.Card => Utils.Utils.Sample(this.CardLoot, this.Count)
                        .Select(card => new CardLoot { Card = card })
                        .Cast<Loot>()
                        .ToList(),
                    LootType.Artifact => Utils.Utils.Sample(this.ArtifactLoot, this.Count)
                        .Select(artifact => new ArtifactLoot { Artifact = artifact })
                        .Cast<Loot>()
                        .ToList(),
                    _ => throw new Exception("[Player:_Loot:GenerateLoot] Unexpected LootType")
                };
            }

            public void RemoveArtifact(Artifact artifact) {
                this.ArtifactLoot.Remove(this.ArtifactLoot.Find(w => w.Obj == artifact));
            }
        }
        [field: SerializeField] public _Loot Loot { get; private set; }

        public override void TurnStarts() {
            base.TurnStarts();
            this.UI.HandUI.ShowCards();
        }

        public override void TurnEnds() {
            base.TurnEnds();
            this.UI.HandUI.HideCards();
        }

        public override void FightEnds() {
            base.FightEnds();
            this.UI.HandUI.HideCards();
            this.UI.HandUI.Reset();
        }

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
            card.Initialize();
            this.Cards.Deck.Remove(card);
            this.Cards.Hand.Add(card);
            this.UI.HandUI.AddCard(card);
        }

        public void UpdateCardDescriptions() {
            this.UI.HandUI.UpdateCardDescriptions();
        }

        [Serializable]
        public class _UI {
            [field: SerializeField] public float HandHeight { get; private set; }
            [field: SerializeField] public HandUI HandUI { get; private set; }
        }
    }
}
