using System;
using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;
using UnityEngine;

namespace Code.Cards.Effects.Active {
    public class ActionPoint : CardEffect {
        private readonly int Value;

        public ActionPoint(int value) => this.Value = value;

        public override void UpdateDescription(Player player = null) {
            int value = player == null
                ? this.Value
                : player.Compute(null, CallbackType.ActionPoint, player, null, this.Value, short.MaxValue);

            string text = value switch {
                >= 0 => $"Gives {value}",
                < 0 => $"Removes {Mathf.Abs(value)}"
            };
            this.Description = new List<string> {
                $"{text}{SpriteEffectMapping.Get(Effect.ActionPoint)}"
            };
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> sideEffects, Character from, Character to) {
            return new List<CardEffectValues> { RunEffect(sideEffects, CallbackType.ActionPoint, from, to, this.Value, short.MaxValue) };
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            RunEffect(CallbackType.ActionPoint, from, to, this.Value, short.MaxValue);
        }
    }
}
