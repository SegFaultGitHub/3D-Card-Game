using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Passives.Necromancer {
    public class Curse : Card {
        public override void Initialize() {
            this.Name = $"Curse {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.AliveEnemy };
            this.RemoveAfterUsage = true;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new PoisonOnTakeActionPoint(1) };
                    this.Cost = 3;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new PoisonOnTakeActionPoint(2) };
                    this.Cost = 3;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new PoisonOnTakeActionPoint(3) };
                    this.Cost = 3;
                    break;
                default: throw new Exception($"[Curse:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
