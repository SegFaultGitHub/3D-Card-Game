using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Paladin {
    public class RadiantBarrier : Card {
        public override void Initialize() {
            this.Name = $"Radiant Barrier {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.Self, Target.AliveAlly };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new Poison(3) };
                    this.Cost = 3;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new Poison(4) };
                    this.Cost = 3;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new Poison(4) };
                    this.Cost = 2;
                    break;
                default: throw new Exception($"[Infection:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
