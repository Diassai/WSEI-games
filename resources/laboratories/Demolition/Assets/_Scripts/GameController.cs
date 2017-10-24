namespace Demolition
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Handles bomb spawning on mouse click
    /// </summary>
    public class GameController : MonoBehaviour
    {
        #region Inspector Variables
        [Header("References")]
        // Reference to the Bomb prefab instantiated on mouse click
        [SerializeField]
        private GameObject _BombPrefab;
        // Reference to the ScoreBar, used to calculate points
        [SerializeField]
        private Transform _ScoreBar;
        // Reference to UI objects
        [SerializeField]
        private HudView _HudView;

        [Header("Settings")]
        [SerializeField]
        private int _MaxBombs = 3;
        [SerializeField]
        private float _ScoreTime = 3.0f;
        #endregion Inspector Variables

        #region Public Variables
        public int BombsLeft
        {
            get
            {
                return _BombsLeft;
            }
        }
        #endregion Public Variables

        #region Private Variables
        private List<Bomb> _Bombs = new List<Bomb>();
        private int _BombsLeft;
        #endregion Private Variables

        #region Unity Messages
        private void Start()
        {
            _BombsLeft = _MaxBombs;

            // Sets the restart action of the UI to Restart method of GameController
            _HudView.Restart = Restart;

            // Disable the score bar
            _ScoreBar.gameObject.SetActive(false);
        }
        private void Update()
        {
            HandleMouseClick();
            UpdateScore();
        }
        #endregion Unity Messages

        #region Private Methods
        private void HandleMouseClick()
        {
            // Check if the mouse click was over any UI element, if yes, do not place new bomb
            var eventSystem = UnityEngine.EventSystems.EventSystem.current;
            if (eventSystem.IsPointerOverGameObject())
            {
                return;
            }

            // Check if left mouse button was clicked and we have any bombs
            if (Input.GetMouseButtonDown(0) && _BombsLeft > 0)
            {
                // Get current mouse position
                var mousePos = Input.mousePosition;

                // Transform screen-space point to the world coordinates using
                // current camera
                var worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                // Remember to set the z value properly,
                // by default it's set to the camera z value
                worldPos.z = -2.0f;

                // Instantiate bomb prefab at calculated position
                // assign Quaternion.identity to rotation which is equivalent to (0, 0, 0) Euler angles
                var go = Instantiate(_BombPrefab, worldPos, Quaternion.identity);
                var bombScript = go.GetComponent<Bomb>();

                // Add our bombs to list for later use
                _Bombs.Add(bombScript);
                _BombsLeft--;

                // If we placed all bombs
                if (_BombsLeft == 0)
                {
                    // Invoke Explode method
                    foreach (var bomb in _Bombs)
                    {
                        bomb.Explode();
                    }

                    // Enable score bar after some, constant time
                    Invoke("StartScoreCount", _ScoreTime);
                }
            }
        }
        private void UpdateScore()
        {
            // Estimate score using heuristically determined,
            // height based, linear function with static offset
            _HudView.Score = (int)((20.0f - _ScoreBar.transform.position.y) * 10.0f);
        }

        private void Restart()
        {
            SceneManager.LoadScene(0);
        }

        private void StartScoreCount()
        {
            _ScoreBar.gameObject.SetActive(true);
        }
        #endregion Private Methods
    }
}