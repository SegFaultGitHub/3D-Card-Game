using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Code.Callbacks;
using Code.Callbacks.Enums;
using Code.Cards.Collection;
using Code.Cards.Effects;
using Code.Cards.UI;
using Code.Cards.VFX;
using Code.Extensions;
using Code.Fight;
using Code.UI;
using Code.Utils;
using UnityEngine;

namespace Code.Characters {
    public abstract class Character : MonoBehaviour {
        [field: SerializeField] private Transform AttackZone;

        private Transform Camera;

        private List<CardEffect.CardEffectValues> EffectsQueue;
        private Text CurrentEffectText;
        protected FightManager FightManager;
        private LTDescr HighlightTween;
        private Vector3 InitialHighlightScale;
        public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public string Name { get; private set; }

        protected void Awake() {
            this.Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
            this.OnComputeCallbacks = new List<OnCompute>();
            this.OnApplyCallbacks = new List<OnApply>();
            this.OnTakeCallbacks = new List<OnTake>();
            this.OnAppliedCallbacks = new List<OnApplied>();
            this.CharacterController = this.GetComponent<CharacterController>();

            this.InitialHighlightScale = this.Cards.SelectedVFX.transform.localScale;
            this.Cards.SelectedVFX.transform.localScale *= 0;

            this.EffectsQueue = new List<CardEffect.CardEffectValues>();
            this.StartCoroutine(this.ShowCardEffectNumbers());
        }

        private void Update() {
            if (this.CurrentEffectText != null) {
                this.CurrentEffectText.transform.LookAt(this.Camera);
            }
        }

        // ReSharper disable once FunctionRecursiveOnAllPaths
        private IEnumerator ShowCardEffectNumbers() {
            yield return new WaitUntil(() => this.EffectsQueue.Count > 0);
            CardEffect.CardEffectValues cardEffectValues = this.EffectsQueue[0];
            this.EffectsQueue.RemoveAt(0);
            string text;
            if (cardEffectValues.CallbackType == CallbackType.ActionPoint) {
                text = cardEffectValues.Applied switch {
                    > 0 =>
                        $"{{+}}{cardEffectValues.Applied.ToString(CultureInfo.InvariantCulture)} {SpriteEffectMapping.Get(cardEffectValues.CallbackType)}",
                    < 0 =>
                        $"{cardEffectValues.Applied.ToString(CultureInfo.InvariantCulture).Replace("-", "{-}")} {SpriteEffectMapping.Get(cardEffectValues.CallbackType)}",
                    _ => null
                };
            } else {
                text =
                    $"{cardEffectValues.Applied.ToString(CultureInfo.InvariantCulture)} {SpriteEffectMapping.Get(cardEffectValues.CallbackType)}";
            }
            if (text != null) {
                this.CurrentEffectText = Instantiate(this.Cards.TextPrefab, this.transform);
                this.CurrentEffectText.Initialize(text);
                this.CurrentEffectText.transform.localPosition = new Vector3(0, 3, 0);
                this.CurrentEffectText.transform.LookAt(this.Camera);
            }
            yield return new WaitUntil(() => this.CurrentEffectText == null);
            this.StartCoroutine(this.ShowCardEffectNumbers());
        }
        protected void EnqueueEffects(List<CardEffect.CardEffectValues> effects) {
            this.EffectsQueue.AddRange(Aggregate(effects));
        }

        public void DisableHighlight() {
            if (this.HighlightTween != null) LeanTween.cancel(this.HighlightTween.id);

            this.HighlightTween = LeanTween.scale(this.Cards.SelectedVFX, Vector3.zero, 0.2f)
                .setOnStart(() => this.Cards.SelectedVFX.SetActive(true))
                .setEaseInBack()
                .setOnComplete(
                    () => {
                        this.Cards.SelectedVFX.SetActive(false);
                        this.HighlightTween = null;
                    }
                );
        }
        public void EnableHighlight() {
            if (this.HighlightTween != null) LeanTween.cancel(this.HighlightTween.id);

            this.HighlightTween = LeanTween.scale(this.Cards.SelectedVFX, this.InitialHighlightScale, 0.2f)
                .setOnStart(() => this.Cards.SelectedVFX.SetActive(true))
                .setEaseOutBack()
                .setOnComplete(() => this.HighlightTween = null);
        }

        #region Cards
        [Serializable]
        public class _Cards {
            [field: SerializeField] public List<Card> BaseDeck;
            [field: SerializeField] public List<Card> Deck;
            [field: SerializeField] public int InitialHandSize;
            [field: SerializeField] public int CardsDrawnPerTurn;
            [field: SerializeField] public int MaxHandSize;
            [field: SerializeField] public List<Card> Hand;
            public List<Card> Discarded;

            public Text TextPrefab;
            public GameObject SelectedVFX;
        }
        [field: SerializeField] public _Cards Cards { get; private set; }

        public abstract IEnumerable<Character> Allies { get; }
        public abstract IEnumerable<Character> Enemies { get; }

        protected bool IsAlly(Character character) {
            return this.Allies.ToList().IndexOf(character) != -1;
        }
        protected bool IsEnemy(Character character) {
            return !this.IsAlly(character);
        }

        public virtual Card DrawCard() {
            if (this.Cards.Hand.Count >= this.Cards.MaxHandSize)
                return null;
            if (this.Cards.Deck.Count == 0) {
                if (this.Cards.Discarded.Count == 0)
                    return null;
                this.Cards.Deck = new List<Card>(this.Cards.Discarded);
                this.Cards.Discarded = new List<Card>();
            }

            Card card = Utils.Utils.Sample(this.Cards.Deck);
            card.Initialize();
            this.Cards.Deck.Remove(card);
            this.Cards.Hand.Add(card);
            return card;
        }

        public void AddToDeck(Card card) {
            this.Cards.BaseDeck.Add(card);
        }

        public void Foo(Character target) {
            this.UseCard(this.Cards.Hand[0], target);
        }

        public bool UseCard(Card card, Character target) {
            if (!card.CanUse(this, target) || this.FightLocked || !this.FightManager.InProgress)
                return false;

            Debug.Log($"{this.name} played {card.Name}");
            this.StartCoroutine(this.AsynchronousUseCard(card, target));
            return true;
        }

        // public bool UseCardNow(Card card, Character target) {
        //     if (!card.CanUse(this, target))
        //         return false;
        //     Debug.Log($"{this.name} played played {card.Name}");
        //     card.Use(this, target);
        //     CardEffect.RunEffect(CallbackType.ActionPoint, this, this, -card.Cost, 0);
        //     return true;
        // }

        public SimulationTeams? SimulateUseCard(Card card, Character target) {
            if (!card.CanUse(this, target)) return null;

            List<SimulationCharacter> allies = (
                from ally in this.Allies
                where ally != this && ally != target
                select ally.GenerateSimulationCharacter()).ToList();
            List<SimulationCharacter> enemies = (
                from enemy in this.Enemies
                where enemy != this && enemy != target
                select target.GenerateSimulationCharacter()).ToList();

            SimulationCharacter from = this.GenerateSimulationCharacter();
            SimulationCharacter to = this == target ? from : target.GenerateSimulationCharacter();

            card.Use(from, to);
            CardEffect.RunEffect(CallbackType.ActionPoint, from, from, -card.Cost, 0);

            allies.Add(from);
            // ReSharper disable once InvertIf
            if (this != target) {
                if (target.IsAlly(this)) allies.Add(to);
                else enemies.Add(to);
            }

            return new SimulationTeams {
                Allies = allies,
                Enemies = enemies
            };
        }

        public SimulationCharacter GenerateSimulationCharacter() {
            return new SimulationCharacter(this);
        }

        #region Asynchronous card utilization
        private Card Card;
        private Character Target;
        public bool FightLocked { get; private set; }
        public bool Moving { get; private set; }
        private int Lock;

        private IEnumerator LockFor(float seconds) {
            this.Lock++;
            yield return new WaitForSeconds(seconds);
            this.Lock--;
        }

        private IEnumerator LockUntil(Func<bool> predicate) {
            this.Lock++;
            yield return new WaitUntil(predicate);
            this.Lock--;
        }

        private IEnumerator LockUntilWithDelay(float delay, Func<bool> predicate) {
            this.Lock++;
            yield return new WaitForSeconds(delay);
            yield return new WaitUntil(predicate);
            this.Lock--;
        }

        private IEnumerator LockWithDelay(float delay, Func<float> action) {
            this.Lock++;
            yield return new WaitForSeconds(delay);
            yield return new WaitForSeconds(action());
            this.Lock--;
        }

        private IEnumerator AsynchronousUseCard(Card card, Character target) {
            this.FightLocked = true;

            List<CardEffect.CardEffectValues> effects = card.Use(this, target);
            List<CardEffect.CardEffectValues> actionPointsSideEffects = new();
            effects.Add(CardEffect.RunEffect(actionPointsSideEffects, CallbackType.ActionPoint, this, this, -card.Cost, short.MaxValue));
            effects.AddRange(actionPointsSideEffects);

            Transform t = this.transform;
            Vector3 initialPosition = t.position;
            float initialAngle = t.eulerAngles.y;

            foreach (CardSteps cardSteps in card.Steps) {
                foreach (CardStep cardStep in cardSteps.Steps) {
                    switch (cardStep.StepType) {
                        case StepType.LookTowardsTarget:
                            this.CharacterController.LookTowards(target.transform);
                            if (cardSteps.Wait) this.StartCoroutine(this.LockUntil(() => this.CharacterController.TargetAngle == null));
                            break;
                        case StepType.LookInitialAngle:
                            this.CharacterController.TargetAngle = initialAngle;
                            if (cardSteps.Wait) this.StartCoroutine(this.LockUntil(() => this.CharacterController.TargetAngle == null));
                            break;
                        case StepType.MoveToTarget:
                            this.CharacterController.TargetPosition = target.AttackZone.position;
                            if (cardSteps.Wait) this.StartCoroutine(this.LockUntil(() => this.CharacterController.TargetPosition == null));
                            break;
                        case StepType.MoveToInitialPosition:
                            this.CharacterController.TargetPosition = initialPosition;
                            if (cardSteps.Wait) this.StartCoroutine(this.LockUntil(() => this.CharacterController.TargetPosition == null));
                            break;
                        case StepType.SelfAnimation:
                            if (cardSteps.Wait) {
                                this.StartCoroutine(
                                    this.LockWithDelay(cardStep.Delay, () => this.CharacterController.LaunchAnimation(cardStep.Animation))
                                );
                            } else {
                                this.InSeconds(cardStep.Delay, () => this.CharacterController.LaunchAnimation(cardStep.Animation));
                            }
                            break;
                        case StepType.TargetAnimation:
                            if (target.Stats.Dead) break;
                            if (cardSteps.Wait) {
                                this.StartCoroutine(
                                    this.LockWithDelay(cardStep.Delay, () => target.CharacterController.LaunchAnimation(cardStep.Animation))
                                );
                            } else {
                                this.InSeconds(cardStep.Delay, () => target.CharacterController.LaunchAnimation(cardStep.Animation));
                            }
                            break;
                        case StepType.SelfVFX:
                            if (cardSteps.Wait) {
                                if (cardStep.VFX.Projectile) {
                                    this.Lock++;
                                    this.InSeconds(
                                        cardStep.Delay,
                                        () => {
                                            VFX vfx = this.CharacterController.LaunchVFX(cardStep.VFX);
                                            this.While(() => vfx != null && !vfx.Completed, () => this.Lock--);
                                        }
                                    );
                                } else {
                                    this.StartCoroutine(
                                        this.LockWithDelay(cardStep.Delay, () => this.CharacterController.LaunchVFX(cardStep.VFX).Duration)
                                    );
                                }
                            } else {
                                this.InSeconds(cardStep.Delay, () => this.CharacterController.LaunchVFX(cardStep.VFX));
                            }
                            break;
                        case StepType.TargetVFX:
                            if (cardSteps.Wait) {
                                if (cardStep.VFX.Projectile) {
                                    this.Lock++;
                                    this.InSeconds(
                                        cardStep.Delay,
                                        () => {
                                            VFX vfx = target.CharacterController.LaunchVFX(cardStep.VFX);
                                            this.While(() => vfx != null && !vfx.Completed, () => this.Lock--);
                                        }
                                    );
                                } else {
                                    this.StartCoroutine(
                                        this.LockWithDelay(
                                            cardStep.Delay,
                                            () => target.CharacterController.LaunchVFX(cardStep.VFX).Duration
                                        )
                                    );
                                }
                            } else {
                                this.InSeconds(cardStep.Delay, () => target.CharacterController.LaunchVFX(cardStep.VFX));
                            }
                            break;
                        case StepType.Wait:
                            this.StartCoroutine(this.LockFor(cardStep.Delay));
                            break;
                        case StepType.ShowNumbers:
                            this.InSeconds(
                                cardStep.Delay,
                                () => {
                                    effects = Aggregate(effects);
                                    foreach (CardEffect.CardEffectValues cardEffectValues in effects) {
                                        if (cardEffectValues.Target.Stats.Dead)
                                            cardEffectValues.Target.CharacterController.LaunchDeathAnimation();
                                        cardEffectValues.Target.EffectsQueue.Add(cardEffectValues);
                                    }
                                }
                            );
                            break;
                        default: throw new ArgumentOutOfRangeException();
                    }
                }
                yield return new WaitUntil(() => this.Lock == 0);
            }

            this.FightLocked = false;
        }

        private static List<CardEffect.CardEffectValues> Aggregate(List<CardEffect.CardEffectValues> list) {
            List<CardEffect.CardEffectValues> result = new();
            foreach (CardEffect.CardEffectValues effectValues in list) {
                int index = result.FindIndex(e => e.Target == effectValues.Target && e.CallbackType == effectValues.CallbackType);
                if (index == -1) result.Add(effectValues);
                else result[index].Applied += effectValues.Applied;
            }
            return result;
        }
        #endregion
        #endregion

        #region Stats
        [Serializable]
        public class _Stats {
            [field: SerializeField] public int MaxHealth;
            [field: SerializeField] public int Health;
            [field: SerializeField] public int Shield;
            [field: SerializeField] public int Poison;
            [field: SerializeField] public int ActionPoints;
            [field: SerializeField] public int CurrentActionPoints;
            public float HealthRatio => (float)this.Health / this.MaxHealth;
            public float ActionPointsRatio => (float)this.CurrentActionPoints / this.ActionPoints;
            [field: SerializeField] public bool Dead { get; set; }

            public void IncreaseMaxHealth(float ratio) {
                int value = (int) (this.MaxHealth * ratio);
                this.IncreaseMaxHealth(value);
            }

            public void IncreaseMaxHealth(int value) {
                this.MaxHealth += value;
                this.Health += value;
            }
        }
        [field: SerializeField] public _Stats Stats { get; private set; }

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
            this.Stats.Health -= value;

            if (this.Stats.Health <= 0)
                this.Die();
        }

        private void AddPoison(int value) {
            if (value <= 0)
                return;
            this.Stats.Poison += value;
        }

        public void RemovePoison(int value) {
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

        private void Die() {
            this.Stats.Dead = true;
            this.ExpireAllCallbacks();
            this.FightManager.CheckFightEnded();
        }
        #endregion

        #region Callbacks
        public List<OnCompute> OnComputeCallbacks { get; private set; }
        public List<OnApply> OnApplyCallbacks { get; private set; }
        public List<OnTake> OnTakeCallbacks { get; private set; }
        public List<OnApplied> OnAppliedCallbacks { get; private set; }

        private readonly Dictionary<int, List<Callback>> TemporaryCallbacks = new();

        public void AddCallback(Callback callback) {
            switch (callback) {
                case OnCompute onCompute:
                    this.OnComputeCallbacks.Add(onCompute);
                    this.OnComputeCallbacks.Sort((c1, c2) => c1.Priority - c2.Priority);
                    if (this is Player player) player.UpdateCardDescriptions();
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
                case OnFightStarts onFightStarts:
                    this.OnFightStartsCallbacks.Add(onFightStarts);
                    this.OnFightStartsCallbacks.Sort((c1, c2) => c1.Priority - c2.Priority);
                    break;
                case OnFightEnds onFightEnds:
                    this.OnFightEndsCallbacks.Add(onFightEnds);
                    this.OnFightEndsCallbacks.Sort((c1, c2) => c1.Priority - c2.Priority);
                    break;
                case OnTurnStarts onTurnStarts:
                    this.OnTurnStartsCallbacks.Add(onTurnStarts);
                    this.OnTurnStartsCallbacks.Sort((c1, c2) => c1.Priority - c2.Priority);
                    break;
                case OnTurnEnds onTurnEnds:
                    this.OnTurnEndsCallbacks.Add(onTurnEnds);
                    this.OnTurnEndsCallbacks.Sort((c1, c2) => c1.Priority - c2.Priority);
                    break;
            }
        }

        public void AddCallback(Callback callback, int? duration) {
            int key = -1;
            if (duration != null)
                key = this.FightManager.Turn + (int)duration;
            if (!this.TemporaryCallbacks.ContainsKey(key))
                this.TemporaryCallbacks[key] = new List<Callback>();

            this.TemporaryCallbacks[key].Add(callback);
            this.AddCallback(callback);
        }

        private void RemoveCallback(Callback callback) {
            switch (callback) {
                case OnCompute onCompute:
                    this.OnComputeCallbacks.Remove(onCompute);
                    if (this is Player player) player.UpdateCardDescriptions();
                    break;
                case OnApply onApply:
                    this.OnApplyCallbacks.Remove(onApply);
                    break;
                case OnTake onTake:
                    this.OnTakeCallbacks.Remove(onTake);
                    break;
                case OnApplied onApplied:
                    this.OnAppliedCallbacks.Remove(onApplied);
                    break;
                case OnFightStarts onFightStarts:
                    this.OnFightStartsCallbacks.Remove(onFightStarts);
                    break;
                case OnFightEnds onFightEnds:
                    this.OnFightEndsCallbacks.Remove(onFightEnds);
                    break;
                case OnTurnStarts onTurnStarts:
                    this.OnTurnStartsCallbacks.Remove(onTurnStarts);
                    break;
                case OnTurnEnds onTurnEnds:
                    this.OnTurnEndsCallbacks.Remove(onTurnEnds);
                    break;
            }
        }

        private void ExpireAllCallbacks() {
            foreach (Callback callback in this.TemporaryCallbacks.Values.SelectMany(callbacks => callbacks))
                this.RemoveCallback(callback);
        }

        private void ExpireCallbacks() {
            if (!this.TemporaryCallbacks.ContainsKey(this.FightManager.Turn))
                return;
            foreach (Callback callback in this.TemporaryCallbacks[this.FightManager.Turn])
                this.RemoveCallback(callback);
        }

        public int Compute(
            List<CardEffect.CardEffectValues> list, CallbackType type, Character from, Character to, int value, int priority
        ) {
            return this.OnComputeCallbacks.Cast<Callback>()
                .Where(callback => callback.Type == type && callback.Priority < priority)
                .Aggregate(value, (current, callback) => callback.Run(list, from, to, current));
        }

        public int Apply(List<CardEffect.CardEffectValues> list, CallbackType type, Character from, Character to, int value, int priority) {
            return this.OnApplyCallbacks.Cast<Callback>()
                .Where(callback => callback.Type == type && callback.Priority < priority)
                .Aggregate(value, (current, callback) => callback.Run(list, from, to, current));
        }

        public int Take(List<CardEffect.CardEffectValues> list, CallbackType type, Character from, Character to, int value, int priority) {
            value = this.OnTakeCallbacks.Cast<Callback>()
                .Where(callback => callback.Type == type && callback.Priority < priority)
                .Aggregate(value, (current, callback) => callback.Run(list, from, to, current));

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
            Debug.Log($"{to} took {value} {type}");
            return value;
        }

        public int Applied(
            List<CardEffect.CardEffectValues> list, CallbackType type, Character from, Character to, int value, int priority
        ) {
            return this.OnAppliedCallbacks.Cast<Callback>()
                .Where(callback => callback.Type == type && callback.Priority < priority)
                .Aggregate(value, (current, callback) => callback.Run(list, from, to, current));
        }

        private readonly List<OnFightStarts> OnFightStartsCallbacks = new();
        private readonly List<OnFightEnds> OnFightEndsCallbacks = new();
        private readonly List<OnTurnStarts> OnTurnStartsCallbacks = new();
        private readonly List<OnTurnEnds> OnTurnEndsCallbacks = new();

        public virtual void FightStarts(FightManager fightManager) {
            this.FightManager = fightManager;
            this.Cards.Deck = new List<Card>(this.Cards.BaseDeck);
            this.Cards.Hand = new List<Card>();
            this.Cards.Discarded = new List<Card>();
            for (int i = 0; i < this.Cards.InitialHandSize; i++)
                this.DrawCard();
            this.Stats.CurrentActionPoints = this.Stats.ActionPoints;

            List<CardEffect.CardEffectValues> effects = new();
            foreach (OnFightStarts callback in this.OnFightStartsCallbacks)
                effects.AddRange(callback.Run(this));
            this.EnqueueEffects(effects);
        }

        public virtual void FightEnds() {
            List<CardEffect.CardEffectValues> effects = new();
            foreach (OnFightEnds callback in this.OnFightEndsCallbacks)
                effects.AddRange(callback.Run(this));
            this.EnqueueEffects(effects);

            this.Stats.Poison = 0;
            this.Stats.Shield = 0;
            this.Cards.Hand = new List<Card>();
            this.Cards.Discarded = new List<Card>();
            this.ExpireAllCallbacks();
        }

        public virtual void TurnStarts() {
            List<CardEffect.CardEffectValues> effects = new();
            foreach (OnTurnStarts callback in this.OnTurnStartsCallbacks)
                effects.AddRange(callback.Run(this));
            this.EnqueueEffects(effects);
            this.ExpireCallbacks();
            for (int i = 0; i < this.Cards.CardsDrawnPerTurn; i++) {
                this.DrawCard();
            }
            if (this.Stats.Poison != 0) {
                this.EffectsQueue.Add(
                    new CardEffect.CardEffectValues {
                        Applied = this.Stats.Poison,
                        Target = this,
                        CallbackType = CallbackType.Damage
                    }
                );
                this.Take(null, CallbackType.Damage, this, this, this.Stats.Poison, 0);
                if (this.Stats.Dead)
                    this.CharacterController.LaunchDeathAnimation();
            }
            this.FightLocked = false;
        }

        public virtual void TurnEnds() {
            this.Stats.CurrentActionPoints = this.Stats.ActionPoints;
            List<CardEffect.CardEffectValues> effects = new();
            foreach (OnTurnEnds callback in this.OnTurnEndsCallbacks)
                effects.AddRange(callback.Run(this));
            this.EnqueueEffects(effects);
            this.FightLocked = true;

        }
        #endregion
    }
}
