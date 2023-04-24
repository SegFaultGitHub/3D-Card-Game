using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Common {
    public class Slash : Card {
        public override void Initialize() {
            this.Name = $"Slash {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.AliveEnemy };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new Damage(3) };
                    this.Cost = 4;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new Damage(4) };
                    this.Cost = 4;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new Damage(4) };
                    this.Cost = 3;
                    break;
                default: throw new Exception($"[Slash:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
