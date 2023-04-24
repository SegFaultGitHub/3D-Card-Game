using System;
using System.Collections.Generic;
using Code.Cards.VFX;
using UnityEngine;

namespace Code.Characters {
    public class CharacterController : MonoBehaviour {
        protected Animator Animator;
        private Dictionary<string, AnimationClip> Clips;

        private UnityEngine.CharacterController Controller;

        protected virtual void Awake() {
            this.Controller = this.GetComponent<UnityEngine.CharacterController>();
            this.Animator = this.GetComponentInChildren<Animator>();
            this.Clips = new Dictionary<string, AnimationClip>();
            foreach (AnimationClip clip in this.Animator.runtimeAnimatorController.animationClips)
                this.Clips[clip.name] = clip;
            this.AnimationAliases = new Dictionary<string, List<string>>();
            foreach (_AnimationMapping animationMapping in this.AnimationMapping) {
                this.AnimationAliases[animationMapping.Default] = new List<string>(animationMapping.Aliases);
            }
        }

        private void Update() {
            if (this.TargetPosition != null) {
                this.MovementDirection = this.TargetPosition.Value - this.transform.position;
                if (this.MovementDirection.magnitude <= this.Speed) {
                    this.MovementDirection *= 0;
                    this.SetPosition(this.TargetPosition.Value);
                    this.TargetPosition = null;
                }
            }
            if (this.TargetAngle != null) {
                float diff = (this.transform.eulerAngles.y + 360) % 360 - (this.TargetAngle.Value + 360) % 360;
                if (Math.Abs(diff) < 3) {
                    this.transform.rotation = Quaternion.Euler(0, this.TargetAngle.Value, 0);
                    this.TargetAngle = null;
                }
            }
            this.Animator.SetBool(this.GetAnimationAlias("Run"), this.MovementDirection.sqrMagnitude != 0);
        }

        private void FixedUpdate() {
            if (this.MovementDirection.sqrMagnitude != 0) {
                float yValue = this.MovementDirection.y;
                float targetAngle = Mathf.Atan2(this.MovementDirection.x, this.MovementDirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(
                    this.transform.eulerAngles.y,
                    targetAngle,
                    ref this.TurnSmoothVelocity,
                    TURN_SMOOTH_TIME
                );
                this.transform.rotation = Quaternion.Euler(0, angle, 0);

                Vector3 movementDirection = (Quaternion.Euler(0, targetAngle, 0) * Vector3.forward).normalized;
                movementDirection.y = yValue;
                this.Controller.Move(this.Speed * movementDirection);
            } else if (this.TargetAngle != null) {
                float angle = Mathf.SmoothDampAngle(
                    this.transform.eulerAngles.y,
                    this.TargetAngle.Value,
                    ref this.TurnSmoothVelocity,
                    TURN_SMOOTH_TIME
                );
                this.transform.rotation = Quaternion.Euler(0, angle, 0);
            }

            CollisionFlags flags = this.Controller.Move(this.FallingSpeed);
            if (flags == CollisionFlags.Below)
                this.FallingSpeed = GRAVITY;
            else
                this.FallingSpeed += GRAVITY;
        }

        private void OnParticleCollision(GameObject other) {
            other.GetComponentInParent<VFX>().Completed = true;
        }

        public void SetPosition(Vector3 position) {
            this.Controller.enabled = false;
            this.transform.position = position;
            this.Controller.enabled = true;
        }

        public void LaunchDeathAnimation() {
            string alias = this.GetAnimationAlias("Death");
            this.Animator.SetBool(alias, true);
        }

        public float LaunchAnimation(string triggerName) {
            triggerName = this.GetAnimationAlias(triggerName);
            this.Animator.SetTrigger(triggerName);
            return this.Clips[triggerName].length;
        }

        public VFX LaunchVFX(VFX vfx) {
            VFX instance = Instantiate(vfx);
            Transform t1 = this.transform;
            Transform t2 = instance.transform;
            t2.position = t1.position;
            t2.eulerAngles = t1.eulerAngles;
            return instance;
        }

        public void LookTowards(Transform target) {
            Quaternion q = Quaternion.LookRotation(target.position - this.transform.position, Vector3.up);
            this.TargetAngle = q.eulerAngles.y;
        }

        protected string GetAnimationAlias(string animationName) {
            List<string> aliases = this.AnimationAliases.GetValueOrDefault(animationName, null);
            if (aliases != null) animationName = Utils.Utils.Sample(aliases);
            return animationName;
        }
        [Serializable]
        private class _AnimationMapping {
            [field: SerializeField] public string Default { get; private set; }
            [field: SerializeField] public string[] Aliases { get; private set; }
        }

        #region Movement
        private const float TURN_SMOOTH_TIME = 0.1f;
        private static readonly Vector3 GRAVITY = new(0, -0.02f, 0);
        [field: SerializeField] protected float Speed;
        private Vector3 FallingSpeed;
        [field: SerializeField] protected Vector3 MovementDirection;
        private float TurnSmoothVelocity;
        public Vector3? TargetPosition { get; set; }
        public float? TargetAngle { get; set; }
        #endregion

        #region Animation
        [field: SerializeField] private _AnimationMapping[] AnimationMapping;
        private Dictionary<string, List<string>> AnimationAliases;
        #endregion
    }
}
