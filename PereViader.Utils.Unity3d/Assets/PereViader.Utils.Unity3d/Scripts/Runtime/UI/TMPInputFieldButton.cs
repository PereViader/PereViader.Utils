using PereViader.Utils.Unity3d.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PereViader.Utils.Unity3d.UI
{
    public class TMPInputFieldButton : MonoBehaviour
    {
        [Tooltip("If null, will create the button component on Awake")]
        public Button Button;
        public TMP_InputField InputField;

        public void Awake()
        {
            if (Button == null)
            {
                Button = gameObject.GetOrAddComponent<Button>();
            }
            Button.onClick ??= new Button.ButtonClickedEvent();
            Button.onClick.AddListener(StartEditing);

            InputField.onEndEdit ??= new TMP_InputField.SubmitEvent();
            InputField.onEndEdit.AddListener(EndEditing);
            
            Button.transition = InputField.transition;
            Button.targetGraphic = InputField.targetGraphic;
            Button.colors = InputField.colors;
            Button.spriteState = InputField.spriteState;
            Button.animationTriggers = InputField.animationTriggers;
        }

        private void StartEditing()
        {
            InputField.enabled = true;
            InputField.Select();
            Button.enabled = false;
        }
        
        private void EndEditing(string text)
        {
            Button.enabled = true;
            InputField.enabled = false;

            StartCoroutine(CoroutineExtensions.WaitFrame(x =>
            {
                if (EventSystem.current.currentSelectedGameObject == x.InputField.gameObject)
                {
                    x.Button.Select();
                }
            }, (Button, InputField)));
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (InputField == null)
            {
                InputField = GetComponentInChildren<TMP_InputField>();
            }
        }
#endif
    }
}