namespace Demolition
{
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class Bomb : MonoBehaviour
    {
        #region Inspector Variables
        [Header("References")]
        // Reference to effects object (explosion audio and animation)
        [SerializeField]
        private GameObject _FX;

        [Header("Settings")]
        [SerializeField]
        private float _Radius = 3.0f;
        [SerializeField]
        private float _Force = 10.0f;
        [SerializeField]
        private float _Time = 3.0f;
        #endregion Inspector Variables

        #region Public Methods
        public void Explode()
        {
            // Call the [Activate()] method after [_Time] seconds
            Invoke("Activate", _Time);
        }
        #endregion Public Methods

        #region Unity Messages
        private void OnDrawGizmos()
        {
            // Set the gizmo color
            Gizmos.color = Color.red;

            // Draw editor gizmo to visualize explosion radius
            Gizmos.DrawWireSphere(
                transform.position, _Radius);
        }
        #endregion Unity Messages

        #region Private Methods
        private void Activate()
        {
            // Get all colliders within specified radius
            var colliders = Physics2D.OverlapCircleAll(transform.position, _Radius);

            // Check if any colliders where found, if not return from method
            if (colliders == null || colliders.Length == 0)
            {
                // But before, activate fx and delayed destroy
                OnExplode();
                return;
            }

            // Write to console, how many colliders where found
            Debug.Log("Found: " + colliders.Length + " colliders on explosion");

            // For each found collider
            foreach (var collider in colliders)
            {
                // Get the rigidbody component
                var rgbody = collider.GetComponent<Rigidbody2D>();

                // Get sprite component
                var renderer = collider.GetComponent<SpriteRenderer>();

                // Check if it exists in collider gameobject, if not - skip it
                if (rgbody == null) continue;

                // Calculate vector from the center of the bomb to the platform
                var vector = collider.transform.position - transform.position;

                // Calculate length and direction
                var length = vector.magnitude;
                var direction = vector.normalized;

                // Based on this, calculate explosion force
                var force = Mathf.Pow(_Force / (length + 1.0f), 2);

                // Add force to the object rigidbody
                rgbody.AddForce(direction * force, ForceMode2D.Impulse);

                // if sprite renderer is attached
                if (renderer != null)
                {
                    // calculate [0, 1] range for interpolation
                    //
                    // force =  Mathf.Pow(_Force / (length + 1.0f), 2);
                    // min(force) = (_Force / +inf) ^ 2 = 0;
                    // max(force) = (_Force / 1.0f) ^ 2 = _Force ^ 2;
                    float t = force / (_Force * _Force);

                    // change from linear to cubic interpolation to offset small values
                    t = Mathf.Pow(t, 1.0f / 3.0f);
                    renderer.color = Color.Lerp(Color.white, Color.black, t);
                }
            }

            OnExplode();
        }

        private void OnExplode()
        {
            // Activate effects object
            _FX.SetActive(true);

            // Destroy this gameobject
            Destroy(gameObject, 0.3f);
        }
        #endregion Private Methods
    }
}