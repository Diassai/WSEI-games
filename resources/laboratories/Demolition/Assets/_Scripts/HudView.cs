namespace Demolition
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Stores the references to the UI elements,
    /// decouples UI from game logic
    ///
    /// Note that in this case, none of the UI elements
    /// are part of public API, so we can completely change UI without
    /// affecting any other part of the game
    /// </summary>
    public class HudView : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private Text _ScoreText;
        [SerializeField] private Button _RestartButton;
        #endregion Inspector Variables

        #region Public Variables
        // Uses int value to update UI
        public int Score
        {
            set
            {
                _ScoreText.text = "Score: " + value;
            }
        }

        // Allows for binding custom methods that
        // will be called on button click
        public UnityEngine.Events.UnityAction Restart
        {
            set
            {
                _RestartButton.onClick.AddListener(value);
            }
        }
        #endregion Public Variables
    }
}