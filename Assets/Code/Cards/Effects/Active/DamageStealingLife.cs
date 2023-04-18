using System.Collections.Generic;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Active {
    public class DamageStealingLife : CardEffect {
        private readonly float Ratio;
        private readonly int Value;

        public DamageStealingLife(int damage, float ratio) {
            this.Value = damage;
            this.Ratio = ratio;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Damage, player, null, this.Value, short.MaxValue);
            this.Description = new[] {
                $"Deals {value}{SpriteEffectMapping.Get(Effect.Damage)}",
                $"Steals {(int)(this.Ratio * 100)}{{%}}{SpriteEffectMapping.Heart}"
            };
            // this.Description = $"{value}{SpriteEffectMapping.Get(Effect.Damage)} "
            //                    + $"{(int)(this.Ratio * 100)}{{%}}{SpriteEffectMapping.Get(Effect.Heal)}";
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> sideEffects, Character from, Character to) {
            CardEffectValues damageValues = RunEffect(sideEffects, CallbackType.Damage, from, to, this.Value, short.MaxValue);
            CardEffectValues healValues = RunEffect(
                sideEffects,
                CallbackType.Heal,
                from,
                from,
                (int)(damageValues.Applied * this.Ratio),
                0
            );

            return new List<CardEffectValues> {
                damageValues,
                healValues
            };
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            CardEffectValues values = RunEffect(CallbackType.Damage, from, to, this.Value, short.MaxValue);
            RunEffect(CallbackType.Heal, from, from, (int)(values.Applied * this.Ratio), 0);
        }
    }
}
