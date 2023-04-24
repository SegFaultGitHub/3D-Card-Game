using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Passives.Paladin {
    public class Evangelist : Card {
        public override void Initialize() {
            this.Name = $"Evangelist {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.Self };
            this.RemoveAfterUsage = true;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new ShieldOverTime(1, 3) };
                    this.Cost = 3;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new ShieldOverTime(2, 4) };
                    this.Cost = 3;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new ShieldOverTime(3, 5) };
                    this.Cost = 3;
                    break;
                default: throw new Exception($"[Evangelist:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
