using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Effects.Active;
using Code.Cards.Enums;

namespace Code.Cards.Collection.Actives.Necromancer {
    public class Torpor : Card {
        public override void Initialize() {
            this.Name = $"Torpor {this.Tier}";
            this.AllowedTarget = new List<Target> { Target.AliveEnemy };
            this.RemoveAfterUsage = false;
            switch (this.Tier) {
                case Tier.I:
                    this.CardEffects = new List<CardEffect> { new ActionPoint(-2) };
                    this.Cost = 3;
                    break;
                case Tier.II:
                    this.CardEffects = new List<CardEffect> { new ActionPoint(-3) };
                    this.Cost = 3;
                    break;
                case Tier.III:
                    this.CardEffects = new List<CardEffect> { new ActionPoint(-3) };
                    this.Cost = 2;
                    break;
                default: throw new Exception($"[Torpor:Initialize] Tier {this.Tier} not allowed");
            }
        }
    }
}
