﻿using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Active {
    public class Poison : CardEffect {
        private readonly int Value;

        public Poison(int value) {
            this.Value = value;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Poison, player, null, this.Value, short.MaxValue);
            this.Description = new List<string> {
                $"Applies {value}{SpriteEffectMapping.Get(Effect.Poison)}"
            };
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> sideEffects, Character from, Character to) {
            return new List<CardEffectValues> { RunEffect(sideEffects, CallbackType.Poison, from, to, this.Value, short.MaxValue) };
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            RunEffect(CallbackType.Poison, from, to, this.Value, short.MaxValue);
        }
    }
}
