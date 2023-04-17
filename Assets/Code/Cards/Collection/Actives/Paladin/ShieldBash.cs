using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Paladin {
    public class Smite : Card {
        public override void Initialize() {
            this.Name = $"Smite {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.AliveEnemy };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> {
                        new Damage(3),
                        new Shield(2, self: true)
                    };
                    this.Cost = 4;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> {
                        new Damage(4),
                        new Shield(3, self: true)
                    };
                    this.Cost = 4;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> {
                        new Damage(4),
                        new Shield(4, self: true)
                    };
                    this.Cost = 3;
                    break;
                default: throw new Exception($"[Smite:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
