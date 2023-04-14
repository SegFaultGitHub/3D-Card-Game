using System.Collections;
using System.Collections.Generic;
using Code.Extensions;
using UnityEngine;

namespace Code.Characters {
    public class Enemy : Character {
        public override IEnumerable<Character> Allies => new List<Character>(this.FightManager.Enemies);
        public override IEnumerable<Character> Enemies => new List<Character> { this.FightManager.Player };

        public override void TurnStarts() {
            base.TurnStarts();
            if (this.Stats.Dead) this.InSeconds(0.5f, () => this.FightManager.NextTurn());
            else this.InSeconds(0.5f, () => this.StartCoroutine(this.PlayAsynchronousTurn()));
        }

        private IEnumerator PlayAsynchronousTurn() {
            if (Simulation.MakeChoice(this)) {
                yield return new WaitUntil(() => !this.FightLocked);
                this.StartCoroutine(this.PlayAsynchronousTurn());
            } else {
                this.FightManager.NextTurn();
            }
        }
    }
}
