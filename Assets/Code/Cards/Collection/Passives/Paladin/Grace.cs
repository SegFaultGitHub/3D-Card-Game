using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Passives.Paladin {
    public class Grace : Card {
        public override void Initialize() {
            this.Name = $"Grace {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.Self };
            this.RemoveAfterUsage = true;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new ShieldUp(1) };
                    this.Cost = 3;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new ShieldUp(2) };
                    this.Cost = 3;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new ShieldUp(3) };
                    this.Cost = 3;
                    break;
                default: throw new Exception($"[Grace:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
