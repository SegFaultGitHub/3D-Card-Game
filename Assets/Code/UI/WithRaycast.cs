using UnityEngine;

namespace Code.UI {
    public abstract class WithRaycast : MonoBehaviour {

        private static readonly RaycastHit[] RAYCAST_HITS = new RaycastHit[10];

        protected UnityEngine.Camera Camera;
        protected Vector2 MousePosition, MousePositionDelta;

        protected virtual void Start() {
            this.Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.Camera>();
        }

        protected virtual void Update() {
            this.HandleInput();
        }

        protected Hit<T> Raycast<T>(LayerMask layer) {
            for (int i = 0; i < RAYCAST_HITS.Length; i++) RAYCAST_HITS[i] = default;
            Physics.RaycastNonAlloc(this.Camera.ScreenPointToRay(this.MousePosition), RAYCAST_HITS, 1000, layer);

            float minDistance = float.MaxValue;
            T result = default;
            foreach (RaycastHit hit in RAYCAST_HITS) {
                if (hit.collider is null)
                    return new Hit<T> {
                        Distance = minDistance,
                        Obj = result
                    };
                if (minDistance < hit.distance)
                    continue;
                minDistance = hit.distance;
                T current = hit.collider.GetComponentInParent<T>();
                if (current is not null) result = current;
            }
            return new Hit<T> {
                Distance = minDistance,
                Obj = result
            };
        }
        protected struct Hit<T> {
            public float Distance;
            public T Obj;
        }

        #region Input
        protected InputActions InputActions;

        protected virtual void OnEnable() {
            this.InputActions = new InputActions();
            this.InputActions.Globals.Enable();
        }

        protected virtual void OnDisable() {
            this.InputActions.Globals.Disable();
        }

        private void HandleInput() {
            this.MousePosition = this.InputActions.Globals.MousePosition.ReadValue<Vector2>();
            this.MousePositionDelta = this.MousePosition - this.MousePositionDelta;
        }
        #endregion
    }
}
