using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Passives.Necromancer {
    public class Alchemy : Card {
        public override void Initialize() {
            this.Name = $"Alchemy {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.Self };
            this.RemoveAfterUsage = true;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new PoisonUp(1) };
                    this.Cost = 3;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new PoisonUp(2) };
                    this.Cost = 3;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new PoisonUp(3) };
                    this.Cost = 3;
                    break;
                default: throw new Exception($"[Alchemy:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
