using System.Collections;
using System.Linq;
using MyBox;
using UnityEngine;

namespace Code.Cards.VFX {
    public class VFX : MonoBehaviour {
        [field: SerializeField] public bool Projectile;
        private Rigidbody Collider;
        [field: SerializeField, ConditionalField(nameof(Projectile), true)]
        public float Duration { get; private set; }
        [field: SerializeField, ConditionalField(nameof(Projectile))]
        public ParticleSystem MainProjectile { get; private set; }
        public bool Completed { get; set; }

        private void Start() {
            this.Completed = false;
            this.StartCoroutine(this.DestroyWhenFinished());
        }

        private IEnumerator DestroyWhenFinished() {
            ParticleSystem[] particles = this.GetComponentsInChildren<ParticleSystem>();
            yield return new WaitUntil(() => particles.All(particle => particle.isStopped));
            Destroy(this.gameObject);
        }
    }
}
