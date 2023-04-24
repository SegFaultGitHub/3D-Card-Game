using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Common {
    public class SleightOfHand : Card {
        public override void Initialize() {
            this.Name = $"Sleight of Hand {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.Self };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new DrawCards(2) };
                    this.Cost = 0;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new DrawCards(3) };
                    this.Cost = 0;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new DrawCards(4) };
                    this.Cost = 0;
                    break;
                default: throw new Exception($"[Accelerate:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
