using System.Collections.Generic;
using Code.Characters;
using UnityEditor;
using UnityEngine;

namespace Code.Map {
    public class Room : MonoBehaviour {
        [field: SerializeField] private float DistanceFromCenter;
        [field: SerializeField] private float OffsetAngle;
        [field: SerializeField] public float Rotation;
        [field: SerializeField] public int MaxEnemies;
        [field: SerializeField] public Transform Center { get; private set; }
        [field: SerializeField] public Transform CameraFocus { get; private set; }
        [field: SerializeField] public Transform CameraPosition { get; private set; }

        private void OnDrawGizmos() {
            Vector3 playerPosition = this.GetPlayerPosition() + new Vector3(0, 0, 0);
            Handles.DrawWireArc(
                playerPosition,
                Vector3.up,
                Quaternion.Euler(0, this.Rotation, 0) * Vector3.forward,
                this.OffsetAngle * (this.MaxEnemies - 1) / 2,
                this.DistanceFromCenter * 2
            );
            Handles.DrawWireArc(
                playerPosition,
                Vector3.up,
                Quaternion.Euler(0, this.Rotation, 0) * Vector3.forward,
                -this.OffsetAngle * (this.MaxEnemies - 1) / 2,
                this.DistanceFromCenter * 2
            );
            Handles.DrawLine(
                playerPosition,
                playerPosition
                + Quaternion.Euler(0, this.OffsetAngle * (this.MaxEnemies - 1) / 2 + this.Rotation, 0) * Vector3.forward * this.DistanceFromCenter * 2
            );
            Handles.DrawLine(
                playerPosition,
                playerPosition
                + Quaternion.Euler(0, -this.OffsetAngle * (this.MaxEnemies - 1) / 2 + this.Rotation, 0) * Vector3.forward * this.DistanceFromCenter * 2
            );

            Handles.DrawLine(this.CameraPosition.position, this.CameraFocus.position);
        }

        public void PlacePlayer(Character player) {
            player.CharacterController.SetPosition(this.GetPlayerPosition());
            player.transform.LookAt(this.Center, Vector3.up);
        }

        public List<Enemy> SpawnEnemies(Component player, List<Enemy> enemies) {
            List<Enemy> instances = new();
            float angle = (enemies.Count - 1f) * this.OffsetAngle / 2f;
            Vector3 playerPosition = this.GetPlayerPosition();
            foreach (Enemy enemy in enemies) {
                Vector3 position = playerPosition
                                   + Quaternion.Euler(0, angle + this.Rotation, 0) * Vector3.forward * (this.DistanceFromCenter * 2);

                Enemy instance = Instantiate(enemy);
                instance.CharacterController.SetPosition(position);
                instance.transform.LookAt(player.transform, Vector3.up);
                angle -= this.OffsetAngle;
                instances.Add(instance);
            }
            return instances;
        }

        private Vector3 GetPlayerPosition() {
            return this.Center.position + Quaternion.Euler(0, 180 + this.Rotation, 0) * Vector3.forward * this.DistanceFromCenter;
        }
    }
}
