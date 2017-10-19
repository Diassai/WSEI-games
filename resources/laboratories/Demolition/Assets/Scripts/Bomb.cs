namespace Demolition
{
    using UnityEngine;

    /// <summary>
    /// Bomb script used for affecting other objects,
    /// (in future: playing animations and sounds)
    /// </summary>
    public class Bomb : MonoBehaviour
    {
        #region Inspector Variables
        [Header("Settings")]
        [SerializeField]
        private float _Radius = 3.0f;
        [SerializeField]
        private float _Force = 10.0f;
        [SerializeField]
        private float _Time = 3.0f;
        #endregion Inspector Variables

        #region Unity Methods
        // method called by Unity, when object is enabled
        public void Start()
        {
            // call the /Activate()/ method after /_Time/ seconds
            Invoke("Activate", _Time);
        }

        // method called by Unity, enabled drawing simple shapes and lines in Editor for debug purposes
        private void OnDrawGizmos()
        {
            // set the gizmo color
            Gizmos.color = Color.red;

            // draw editor gizmo to visualize explosion radius
            Gizmos.DrawWireSphere(
                transform.position, _Radius);
        }
        #endregion Unity Methods

        #region Private Methods
        private void Activate()
        {
            // get all colliders within specified radius
            var colliders = Physics2D.OverlapCircleAll(transform.position, _Radius);

            // check if any colliders where found, if not return from method
            if (colliders == null || colliders.Length == 0)
            {
                // But before, explode the bomb without affecting any objects (because there aren't any)
                OnExplode();
                return;
            }

            // Write to console, how many colliders where found
            Debug.Log("Found: " + colliders.Length + " colliders on explosion");

            // for each found collider
            foreach (var collider in colliders)
            {
                // get the rigidbody component
                var rgbody = collider.GetComponent<Rigidbody2D>();

                // check if the rigidbody exists in collider gameobject, if not - skip it
                if (rgbody == null) continue;

                // calculate vector from the center of the bomb to the platform
                var vector = collider.transform.position - transform.position;

                // calculate length and direction
                var length = vector.magnitude;
                var direction = vector.normalized;

                // based on this, calculate explosion force
                var force = Mathf.Pow(_Force / (length + 1.0f), 2);

                // add force to the object rigidbody
                rgbody.AddForce(direction * force, ForceMode2D.Impulse);
            }

            // explode the bomb
            OnExplode();
        }

        private void OnExplode()
        {
            // destroy this gameobject
            Destroy(gameObject);
        }
        #endregion Private Methods
    }
}