using System;
using System.Collections.Generic;
using Code.Cards.Effects;
using Code.Cards.Enums;
using Code.Characters;
using MyBox;
using UnityEngine;

namespace Code.Cards.Collection {
    public enum StepType {
        LookTowardsTarget,
        LookInitialAngle,
        MoveToTarget,
        MoveToInitialPosition,
        SelfAnimation,
        SelfVFX,
        TargetAnimation,
        TargetVFX,
        Wait,
        ShowNumbers
    }

    [Serializable]
    public struct CardStep {
        public StepType StepType;
        [ConditionalField(nameof(StepType), false, StepType.TargetVFX, StepType.SelfVFX)]
        public VFX.VFX VFX;
        [ConditionalField(nameof(StepType), false, StepType.TargetAnimation, StepType.SelfAnimation)]
        public string Animation;
        [ConditionalField(
            nameof(StepType),
            false,
            StepType.Wait,
            StepType.TargetVFX,
            StepType.SelfVFX,
            StepType.TargetAnimation,
            StepType.SelfAnimation,
            StepType.ShowNumbers
        )]
        public float Delay;
    }

    [Serializable]
    public struct CardSteps {
        public List<CardStep> Steps;
        public bool Wait;
    }

    public class Card : MonoBehaviour {
        protected List<CardEffect> CardEffects { get; set; }

        public string Name { get; protected set; }
        public List<Target> AllowedTarget { get; protected set; }
        public Tier Tier { protected get; set; }
        public int Cost { get; protected set; }
        public bool RemoveAfterUsage { get; protected set; }

        [field: SerializeField] public GameObject Icon { get; private set; }
        [field: SerializeField] public List<CardSteps> Steps { get; private set; }

        public List<CardEffect.CardEffectValues> Use(Character from, Character to) {
            from.Cards.Hand.Remove(this);

            List<CardEffect.CardEffectValues> sideEffects = new();
            List<CardEffect.CardEffectValues> effects = new();
            foreach (CardEffect cardEffect in this.CardEffects)
                effects.AddRange(cardEffect.Run(sideEffects, from, to));
            effects.AddRange(sideEffects);

            if (!this.RemoveAfterUsage)
                from.Cards.Discarded.Add(this);

            return effects;
        }

        public void Use(SimulationCharacter from, SimulationCharacter to) {
            from.HandSize--;

            foreach (CardEffect cardEffect in this.CardEffects)
                cardEffect.Run(from, to);
        }

        public bool CanUse(Character from, Character to) {
            if (this.Cost > from.Stats.CurrentActionPoints)
                return false;

            Target target;

            if (from == to)
                target = Target.Self;
            else if (from.GetType() != to.GetType() && to.Stats.Dead)
                target = Target.DeadEnemy;
            else if (from.GetType() != to.GetType() && !to.Stats.Dead)
                target = Target.AliveEnemy;
            else if (from.GetType() == to.GetType() && to.Stats.Dead)
                target = Target.DeadAlly;
            else if (from.GetType() == to.GetType() && !to.Stats.Dead)
                target = Target.AliveAlly;
            else
                throw new Exception("[Card:CanUse] Unexpected exception");

            return this.AllowedTarget.Contains(target);
        }

        public virtual void Initialize() {
            throw new Exception("[Card:Initialize] Must be implemented in the child");
        }

        public IEnumerable<string[]> Description(Player player) {
            foreach (CardEffect cardEffect in this.CardEffects) {
                cardEffect.UpdateDescription(player);
                yield return cardEffect.Description;
            }
        }
    }
}
