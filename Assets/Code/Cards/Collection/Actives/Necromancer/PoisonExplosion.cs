using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Necromancer {
    public class PoisonExplosion : Card {
        public override void Initialize() {
            this.Name = $"Poison Explosion {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.AliveEnemy };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new Effects.Active.PoisonExplosion(2) };
                    this.Cost = 4;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new Effects.Active.PoisonExplosion(2) };
                    this.Cost = 3;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new Effects.Active.PoisonExplosion(3) };
                    this.Cost = 3;
                    break;
                default: throw new Exception($"[PoisonExplosion:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
