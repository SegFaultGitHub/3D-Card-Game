using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Paladin {
    public class HolyAegis : Card {
        public override void Initialize() {
            this.Name = $"Holy Aegis {this.Tier}";
            this.AllowedTarget = new List<Target> {
                Target.Self,
                Target.AliveAlly
            };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> {
                        new Shield(1),
                        new Shield(1),
                        new Shield(1)
                    };
                    this.Cost = 4;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> {
                        new Shield(1),
                        new Shield(1),
                        new Shield(1)
                    };
                    this.Cost = 3;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> {
                        new Shield(1),
                        new Shield(1),
                        new Shield(1),
                        new Shield(1)
                    };
                    this.Cost = 3;
                    break;
                default: throw new Exception($"[HolyAegis:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
