﻿using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Passive {
    public class DamageOnPoison : CardEffect {
        private const int PRIORITY = 3;
        private readonly int? Duration;
        private readonly int Value;

        public DamageOnPoison(int damage, int? duration = null) {
            this.Value = damage;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Damage, player, null, this.Value, PRIORITY);
            this.Description = $"{SpriteEffectMapping.Get(Effect.Poison)} {SpriteEffectMapping.Arrow} ";

            if (value > this.Value) this.Description += GreenText(value);
            else if (value < this.Value) this.Description += RedText(value);
            else this.Description += BlueText(value);

            this.Description += SpriteEffectMapping.Get(Effect.Damage);

            if (this.Duration != null) this.Description += TurnsString(this.Duration.Value);
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> _1, Character from, Character to) {
            to.AddCallback(new DamageOnPoisonCallback(this.Value), this.Duration);
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            to.AddCallback(new DamageOnPoisonCallback(this.Value), this.Duration);
        }

        private class DamageOnPoisonCallback : OnApply {
            private readonly int Damage;

            public DamageOnPoisonCallback(int damage) : base(PRIORITY, CallbackType.Poison) => this.Damage = damage;

            public override int Run(List<CardEffectValues> sideEffects, Character from, Character to, int value) {
                CardEffectValues values = RunEffect(sideEffects, CallbackType.Damage, from, to, this.Damage, this.Priority);
                sideEffects?.Add(values);
                return value;
            }

            public override int Run(SimulationCharacter from, SimulationCharacter to, int value) {
                RunEffect(CallbackType.Damage, from, to, this.Damage, this.Priority);
                return value;
            }
        }
    }
}
