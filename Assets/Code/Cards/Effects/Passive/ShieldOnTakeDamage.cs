using System.Collections.Generic;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Passive {
    public class ShieldOnTakeActionPoint : CardEffect {
        private const int PRIORITY = 4;
        private readonly int? Duration;
        private readonly int Value;

        public ShieldOnTakeActionPoint(int poison, int? duration = null) {
            this.Value = poison;
            this.Duration = duration;
        }

        public override void UpdateDescription(Player player = null) {
            int value = player == null ? this.Value : player.Compute(null, CallbackType.Poison, player, null, this.Value, PRIORITY);
            this.Description = $"{SpriteEffectMapping.Get(Effect.ActionPoint)} "
                               + $"{SpriteEffectMapping.Arrow} "
                               + $"{value}{SpriteEffectMapping.Get(Effect.ActionPoint)}";

            if (this.Duration != null) this.Description += TurnsString(this.Duration.Value);
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> _, Character from, Character to) {
            to.AddCallback(new PoisonOnActionPointCallback(this.Value, from), this.Duration);
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            to.AddCallback(new PoisonOnActionPointCallback(this.Value, null), this.Duration);
        }

        private class PoisonOnActionPointCallback : OnTake {
            private readonly int Poison;
            private readonly Character Caster;

            public PoisonOnActionPointCallback(int poison, Character caster) : base(PRIORITY, CallbackType.ActionPoint) {
                this.Poison = poison;
                this.Caster = caster;
            }

            public override int Run(List<CardEffectValues> list, Character _, Character to, int value) {
                if (value >= 0)
                    return value;
                CardEffectValues values = RunEffect(list, CallbackType.Poison, this.Caster, to, this.Poison, this.Priority);
                list?.Add(values);
                return value;
            }

            public override int Run(SimulationCharacter _, SimulationCharacter to, int value) {
                RunEffect(CallbackType.Poison, this.Caster.GenerateSimulationCharacter(), to, this.Poison, this.Priority);
                return value;
            }
        }
    }
}
