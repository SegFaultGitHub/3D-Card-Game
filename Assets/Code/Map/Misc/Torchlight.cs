using UnityEngine;

namespace Code.Map.Misc {
    public class Torchlight : MonoBehaviour {

        // percentage
        [field: SerializeField] private float MaxIntensityDiff = 0.15f;
        // percentage
        [field: SerializeField] private float MaxRangeDiff = 0.15f;
        [field: SerializeField] private Vector3 MaxPositionDiff;
        private float InitialIntensity;
        private Vector3 InitialPosition;
        private float InitialRange;
        private Light Light;

        private float XOffsetIntensity, YOffsetIntensity;
        private float XOffsetRange, YOffsetRange;
        private Vector3 XOffsetsPosition, YOffsetsPosition;

        private void Start() {
            this.Light = this.GetComponentInChildren<Light>();
            this.InitialIntensity = this.Light.intensity;
            this.InitialRange = this.Light.range;
            this.InitialPosition = this.Light.transform.localPosition;

            this.XOffsetIntensity = Random.Range(-1f, 1f);
            this.YOffsetIntensity = 0;

            this.XOffsetRange = Random.Range(-1f, 1f);
            this.YOffsetRange = 0;

            this.XOffsetsPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            this.YOffsetsPosition = new Vector3(0, 0, 0);
        }

        private void Update() {
            float intensityDiff = 2 * Mathf.PerlinNoise(this.XOffsetIntensity, this.YOffsetIntensity) - 1;
            float rangeDiff = 2 * Mathf.PerlinNoise(this.XOffsetRange, this.YOffsetRange) - 1;
            Vector3 positionDiff = new(
                2 * Mathf.PerlinNoise(this.XOffsetsPosition[0], this.YOffsetsPosition[0]) - 1,
                2 * Mathf.PerlinNoise(this.XOffsetsPosition[1], this.YOffsetsPosition[1]) - 1,
                2 * Mathf.PerlinNoise(this.XOffsetsPosition[2], this.YOffsetsPosition[2]) - 1
            );

            this.Light.intensity = this.InitialIntensity + intensityDiff * (this.InitialIntensity * this.MaxIntensityDiff);
            this.Light.range = this.InitialRange + rangeDiff * (this.InitialRange * this.MaxRangeDiff);
            this.Light.transform.localPosition = this.InitialPosition + Vector3.Scale(positionDiff, this.MaxPositionDiff);

            this.YOffsetIntensity += 0.01f;
            this.YOffsetRange += 0.01f;
            this.YOffsetsPosition += new Vector3(0.005f, 0.005f, 0.005f);
        }
    }
}
