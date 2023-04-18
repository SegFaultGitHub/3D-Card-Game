using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Paladin {
    public class DivineBulwark : Card {
        public override void Initialize() {
            this.Name = $"Divine Bulwark {this.Tier}";
            this.AllowedTarget = new List<Target> {
                Target.Self,
                Target.AliveAlly
            };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new IncreaseShield(.5f) };
                    this.Cost = 2;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new IncreaseShield(.7f) };
                    this.Cost = 2;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new IncreaseShield(1) };
                    this.Cost = 2;
                    break;
                default: throw new Exception($"[DivineBulwark:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
