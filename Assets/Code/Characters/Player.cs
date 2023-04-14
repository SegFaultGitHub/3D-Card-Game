using System;
using System.Collections.Generic;
using System.Linq;
using Code.Artifacts;
using Code.Cards.Collection;
using Code.Cards.UI;
using Code.Fight;
using Code.Utils;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

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
                return Utils.Utils.Sample(this.LootDistribution) switch {
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

        public override Card DrawCard() {
            Card card = base.DrawCard();
            if (card == null)
                return null;
            this.UI.HandUI.AddCard(card);
            return card;
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
