using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Common {
    public class Accelerate : Card {
        public override void Initialize() {
            this.Name = $"Accelerate {this.Tier}";
            this.AllowedTarget = new List<Target> {
                Target.Self,
                Target.AliveAlly
            };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new ActionPoint(3) };
                    this.Cost = 0;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new ActionPoint(4) };
                    this.Cost = 0;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new ActionPoint(5) };
                    this.Cost = 0;
                    break;
                default: throw new Exception($"[Accelerate:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
