using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Passives.Paladin {
    public class Prayer : Card {
        public override void Initialize() {
            this.Name = $"Prayer {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.Self };
            this.RemoveAfterUsage = true;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new ShieldOnKill(5) };
                    this.Cost = 3;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new ShieldOnKill(10) };
                    this.Cost = 3;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new ShieldOnKill(15) };
                    this.Cost = 3;
                    break;
                default: throw new Exception($"[Prayer:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
