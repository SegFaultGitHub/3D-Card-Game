using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Code.Characters {
    public class PlayerController : CharacterController {
        private Transform Camera;
        private CinemachineFreeLook Cinemachine;
        [field: SerializeField] public bool InFight { private get; set; }

        protected override void Awake() {
            base.Awake();
            this.Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
            this.Cinemachine = GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<CinemachineFreeLook>();
        }

        private void Update() {
            if (this.InFight) {
                if (this.TargetPosition != null) {
                    this.MovementDirection = this.TargetPosition.Value - this.transform.position;
                    if (this.MovementDirection.magnitude <= this.Speed) {
                        this.MovementDirection *= 0;
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
            } else {
                this.GatherInput();
                this.HandleInput();
                if (this.MovementDirection.sqrMagnitude != 0) {
                    float targetAngle = Mathf.Atan2(this.MovementDirection.x, this.MovementDirection.z) * Mathf.Rad2Deg
                                        + this.Camera.eulerAngles.y;
                    this.MovementDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                }
            }
            this.Animator.SetBool(this.GetAnimationAlias("Run"), this.MovementDirection.sqrMagnitude != 0);
        }

        public void StartFight() {
            this.GetComponent<Player>().UI.HandUI.gameObject.SetActive(true);
            this.Cinemachine.gameObject.SetActive(false);
            this.InFight = true;
        }

        public void EndFight() {
            this.GetComponent<Player>().UI.HandUI.gameObject.SetActive(false);
            this.Cinemachine.gameObject.SetActive(true);
            this.InFight = false;
        }

        #region Input
        private List<Action> Interactions;

        private PlayerInputs PlayerInputs;
        private class _Input {
            // --
            public bool InteractPressed;
            public bool MoveEnded;
            public bool MoveInProgress;
            public bool MoveStarted;
        }
        private _Input Input;

        private void OnEnable() {
            this.PlayerInputs = new PlayerInputs();
            this.PlayerInputs.Controls.Enable();
        }

        private void OnDisable() {
            this.PlayerInputs.Controls.Disable();
        }

        private void GatherInput() {
            this.Input = new _Input {
                MoveStarted = this.PlayerInputs.Controls.Move.WasPressedThisFrame(),
                MoveInProgress = this.PlayerInputs.Controls.Move.IsPressed(),
                MoveEnded = this.PlayerInputs.Controls.Move.WasReleasedThisFrame(),
                // --
                InteractPressed = this.PlayerInputs.Controls.Interact.WasPressedThisFrame()
            };
        }

        private void HandleInput() {
            if (this.Input.MoveStarted || this.Input.MoveInProgress || this.Input.MoveEnded) {
                this.Move(this.PlayerInputs.Controls.Move.ReadValue<Vector2>());
            }
            // --
            if (this.Input.InteractPressed) {
                this.Interact();
            }
        }

        private void Move(Vector2 direction) {
            this.MovementDirection = new Vector3(direction.x, 0, direction.y).normalized;
        }

        private void Interact() {
            if (this.Interactions.Count == 0)
                return;

            foreach (Action action in this.Interactions) {
                action.Invoke();
            }
        }
        #endregion
    }
}
