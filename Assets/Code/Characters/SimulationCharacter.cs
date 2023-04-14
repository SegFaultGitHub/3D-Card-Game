using System;
using System.Collections.Generic;
using System.Linq;
using Code.Callbacks;
using Code.Callbacks.Enums;

namespace Code.Characters {
    public class SimulationCharacter {

        public int HandSize;
        public SimulationCharacter(Character character) {
            this.Stats = new _Stats(character.Stats);

            this.OnComputeCallbacks = new List<OnCompute>(character.OnComputeCallbacks);
            this.OnApplyCallbacks = new List<OnApply>(character.OnApplyCallbacks);
            this.OnTakeCallbacks = new List<OnTake>(character.OnTakeCallbacks);
            this.OnAppliedCallbacks = new List<OnApplied>(character.OnAppliedCallbacks);

            this.HandSize = character.Cards.Hand.Count;
        }

        public void DrawCard() {
            this.HandSize++;
        }

        #region Callbacks
        public int CallbacksTotalTurns;

        private readonly List<OnCompute> OnComputeCallbacks;
        private readonly List<OnApply> OnApplyCallbacks;
        private readonly List<OnTake> OnTakeCallbacks;
        private readonly List<OnApplied> OnAppliedCallbacks;

        public void AddCallback(Callback callback, int? duration) {
            if (duration != null)
                this.CallbacksTotalTurns += duration.Value;
            switch (callback) {
                case OnCompute onCompute:
                    this.OnComputeCallbacks.Add(onCompute);
                    this.OnComputeCallbacks.Sort((c1, c2) => c1.Priority - c2.Priority);
                    break;
                case OnApply onApply:
                    this.OnApplyCallbacks.Add(onApply);
                    this.OnApplyCallbacks.Sort((c1, c2) => c1.Priority - c2.Priority);
                    break;
                case OnTake onTake:
                    this.OnTakeCallbacks.Add(onTake);
                    this.OnTakeCallbacks.Sort((c1, c2) => c1.Priority - c2.Priority);
                    break;
                case OnApplied onApplied:
                    this.OnAppliedCallbacks.Add(onApplied);
                    this.OnAppliedCallbacks.Sort((c1, c2) => c1.Priority - c2.Priority);
                    break;
            }
        }

        public int Compute(CallbackType type, SimulationCharacter from, SimulationCharacter to, int value, int priority) {
            return this.OnComputeCallbacks.Cast<Callback>()
                .Where(callback => callback.Type == type && callback.Priority < priority)
                .Aggregate(value, (current, callback) => callback.Run(from, to, current));
        }

        public int Apply(CallbackType type, SimulationCharacter from, SimulationCharacter to, int value, int priority) {
            return this.OnApplyCallbacks.Cast<Callback>()
                .Where(callback => callback.Type == type && callback.Priority < priority)
                .Aggregate(value, (current, callback) => callback.Run(from, to, current));
        }

        public int Take(CallbackType type, SimulationCharacter from, SimulationCharacter to, int value, int priority) {
            value = this.OnTakeCallbacks.Cast<Callback>()
                .Where(callback => callback.Type == type && callback.Priority < priority)
                .Aggregate(value, (current, callback) => callback.Run(from, to, current));

            switch (type) {
                case CallbackType.Damage:
                    to.RemoveHealth(value);
                    break;
                case CallbackType.Poison:
                    to.AddPoison(value);
                    break;
                case CallbackType.Heal:
                    to.AddHealth(value);
                    break;
                case CallbackType.ActionPoint:
                    to.AddActionPoint(value);
                    break;
                case CallbackType.Shield:
                    to.AddShield(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return value;
        }

        public int Applied(CallbackType type, SimulationCharacter from, SimulationCharacter to, int value, int priority) {
            return this.OnAppliedCallbacks.Cast<Callback>()
                .Where(callback => callback.Type == type && callback.Priority < priority)
                .Aggregate(value, (current, callback) => callback.Run(from, to, current));
        }
        #endregion

        #region Stats
        public class _Stats {
            public int CurrentActionPoints;
            public int Health;
            public int MaxHealth;
            public int Poison;
            public int Shield;

            public _Stats(Character._Stats stats) {
                this.MaxHealth = stats.MaxHealth;
                this.Health = stats.Health;
                this.Shield = stats.Shield;
                this.Poison = stats.Poison;
                this.CurrentActionPoints = stats.CurrentActionPoints;
            }
        }
        public _Stats Stats { get; }

        private void AddHealth(int value) {
            if (value <= 0)
                return;
            this.Stats.Health += value;
            if (this.Stats.Health > this.Stats.MaxHealth)
                this.Stats.Health = this.Stats.MaxHealth;
        }

        private void RemoveHealth(int value) {
            if (value <= 0)
                return;
            int shieldDamage = Math.Min(this.Stats.Shield, value);
            this.Stats.Shield -= shieldDamage;
            value -= shieldDamage;
            this.Stats.Health = Math.Max(this.Stats.Health - value, 0);
        }

        private void AddPoison(int value) {
            if (value <= 0)
                return;
            this.Stats.Poison += value;
        }

        private void RemovePoison(int value) {
            if (value <= 0)
                return;
            this.Stats.Poison -= value;
        }

        private void AddActionPoint(int value) {
            this.Stats.CurrentActionPoints += value;
        }

        private void AddShield(int value) {
            this.Stats.Shield += value;
        }
        #endregion
    }
}
