using System.Collections.Generic;
using Code.Cards.Enums;
using Code.Cards.UI;
using Code.Characters;

namespace Code.Cards.Effects.Active {
    public class DrawCards : CardEffect {
        private readonly int Value;

        public DrawCards(int value) => this.Value = value;

        public override void UpdateDescription(Player _ = null) {
            this.Description = new[] {
                $"Draws {this.Value}{SpriteEffectMapping.Get(Effect.Draw)}"
            };
            // this.Description = $"{this.Value}{SpriteEffectMapping.Get(Effect.Draw, Modifier.Plus)}";
        }

        public override IEnumerable<CardEffectValues> Run(List<CardEffectValues> sideEffects, Character from, Character to) {
            for (int i = 0; i < this.Value; i++)
                to.DrawCard();
            return new List<CardEffectValues>();
        }

        public override void Run(SimulationCharacter from, SimulationCharacter to) {
            for (int i = 0; i < this.Value; i++)
                to.DrawCard();
        }
    }
}
