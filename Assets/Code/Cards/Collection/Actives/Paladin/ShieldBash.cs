using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Paladin {
    public class ShieldBash : Card {
        public override void Initialize() {
            this.Name = $"Shield Bash {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.AliveEnemy };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new ShieldDamage(1) };
                    this.Cost = 3;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new ShieldDamage(1.3f) };
                    this.Cost = 3;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new ShieldDamage(2) };
                    this.Cost = 3;
                    break;
                default: throw new Exception($"[ShieldBash:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
