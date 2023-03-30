using Eidolon.Data;
using Eidolon.EventTracker;
using Eidolon.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Eidolon
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private EventTrackerData _eventTrackerData;
        [SerializeField] private Button _buttonEvent;
        [SerializeField] private Toggle _toggleEvent;

        private IEventTracker _eventTracker;
        private ISaveSystem _saveSystem;

        private void Awake()
        {
            _saveSystem = new PlayerPrefsSaveSystem();
            _eventTracker = new UnityWebRequestEventTracker(_eventTrackerData, _saveSystem);

            _buttonEvent.onClick.AddListener(ButtonEventHandler);
            _toggleEvent.onValueChanged.AddListener(ToggleEventHandler);
        }

        private void ButtonEventHandler()
        {
            _eventTracker.TrackEvent("button_event", "click");
        }

        private void ToggleEventHandler(bool value)
        {
            _eventTracker.TrackEvent("toggle_event", value.ToString());
        }
    }    
}