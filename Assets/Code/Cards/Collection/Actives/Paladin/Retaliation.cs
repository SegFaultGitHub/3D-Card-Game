using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Paladin {
    public class Retaliation : Card {
        public override void Initialize() {
            this.Name = $"Retaliation {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.AliveEnemy };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new Damage(6) };
                    this.Cost = 5;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new Damage(7) };
                    this.Cost = 4;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new Damage(10) };
                    this.Cost = 4;
                    break;
                default: throw new Exception($"[Retaliation:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
