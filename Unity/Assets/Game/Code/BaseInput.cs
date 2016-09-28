using UnityEngine;

namespace Teario.Util
{
    public abstract class BaseInput : MonoBehaviour
    {
        public enum eInputButton
        {
            BUTTON_MAIN_ACTION
        };

        public enum eButtonState
        {
            STATE_NONE,
            STATE_PUSHED,
            STATE_HELD,
            STATE_RELEASED
        };

        [SerializeField]
        protected bool m_ForceDisabled;

        public abstract bool IsSelectable();
        public abstract void Select( System.Action<bool> lCallback );
        public abstract Vector2 GetCursorPosition();

        public virtual eButtonState GetButtonState( eInputButton lButton )
        {
            return eButtonState.STATE_NONE;
        }

        public virtual void Disable()
        {
            gameObject.SetActive( false );
        }
    }
}