using UnityEngine;

namespace Code.Camera {
    public class CameraRotator : MonoBehaviour {
        [field: SerializeField] private float MaxSpeed;
        [field: SerializeField] private float Velocity;
        [field: SerializeField] private float Amplitude;
        private float CurrentAngle;
        private int Direction = 1;
        private float InitialAngle;
        private float Speed;

        private void Awake() {
            this.InitialAngle = this.transform.eulerAngles.y;
            this.CurrentAngle = this.InitialAngle;
        }

        private void FixedUpdate() {
            if (this.CurrentAngle < this.InitialAngle - this.Amplitude) {
                this.Direction = 1;
            } else if (this.CurrentAngle > this.InitialAngle + this.Amplitude) {
                this.Direction = -1;
            }

            this.CurrentAngle += this.Speed;
            this.Speed = Mathf.Clamp(this.Speed + this.Velocity * this.Direction, -this.MaxSpeed, this.MaxSpeed);

            Transform t = this.transform;
            Vector3 eulerAngles = t.eulerAngles;
            eulerAngles = new Vector3(eulerAngles.x, this.CurrentAngle, eulerAngles.z);
            t.eulerAngles = eulerAngles;
        }
    }
}
