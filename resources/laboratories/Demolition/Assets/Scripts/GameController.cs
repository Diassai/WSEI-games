namespace Demolition
{
    using UnityEngine;

    /// <summary>
    /// Handles /Bomb/ spawning on mouse click
    /// </summary>
    public class GameController : MonoBehaviour
    {
        #region Inspector Variables
        [Header("References")]  // Adds a header description visible in Inspector
        [SerializeField]        // [SerializeField] enables private-variables to be preserved and shown in Inspector to modify
        private GameObject _BombPrefab; // Reference to the /Bomb/ prefab instantiated on mouse click
        #endregion Inspector Variables

        #region Unity Messages
        // Update is a method called from Unity engine each frame
        private void Update()
        {
            HandleMouseClick();
        }
        #endregion Unity Messages

        #region Private Methods
        // Method for spawning bombs on mouse click
        private void HandleMouseClick()
        {
            // Check if left mouse button was clicked
            if (Input.GetMouseButtonDown(0))
            {
                // Get current mouse position
                var mousePos = Input.mousePosition;

                // Transform screen-space point to the world-space coordinates using
                // current camera
                var worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                // Remember to set the z value properly,
                // by default it's set to the camera z value
                worldPos.z = -2.0f;

                // Instantiate bomb prefab at calculated position
                // assign Quaternion.identity to rotation which is equivalent to (0, 0, 0) Euler angles
                var go = Instantiate(_BombPrefab, worldPos, Quaternion.identity);
            }
        }
        #endregion Private Methods
    }
}