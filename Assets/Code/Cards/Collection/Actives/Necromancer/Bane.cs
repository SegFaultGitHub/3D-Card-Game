using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Necromancer {
    public class Bane : Card {
        public override void Initialize() {
            this.Name = $"Bane {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.AliveEnemy };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> {
                        new Damage(3),
                        new Poison(2)
                    };
                    this.Cost = 4;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> {
                        new Damage(4),
                        new Poison(3)
                    };
                    this.Cost = 4;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> {
                        new Damage(4),
                        new Poison(4)
                    };
                    this.Cost = 3;
                    break;
                default: throw new Exception($"[Bane:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
