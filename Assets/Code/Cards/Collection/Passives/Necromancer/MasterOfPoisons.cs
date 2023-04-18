using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Passive;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Passives.Necromancer {
    public class MasterOfPoisons : Card {
        public override void Initialize() {
            this.Name = $"Master of Poisons {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.Self };
            this.RemoveAfterUsage = true;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new DrawOnApplyPoison(1) };
                    this.Cost = 3;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new DrawOnApplyPoison(1) };
                    this.Cost = 2;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new DrawOnApplyPoison(1) };
                    this.Cost = 1;
                    break;
                default: throw new Exception($"[MasterOfPoisons:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
