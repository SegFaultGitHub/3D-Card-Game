using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Necromancer {
    public class Contamination : Card {
        public override void Initialize() {
            this.Name = $"Contamination {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.AliveEnemy };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new IncreasePoison(.5f) };
                    this.Cost = 4;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new IncreasePoison(.7f) };
                    this.Cost = 3;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new IncreasePoison(1f) };
                    this.Cost = 3;
                    break;
                default: throw new Exception($"[Contamination:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
