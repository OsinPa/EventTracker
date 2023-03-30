using UnityEngine;

namespace Eidolon.Data
{
    [CreateAssetMenu(fileName = "EventTrackerData", menuName = "Eidolon/EventTrackerData")]
    public class EventTrackerData : ScriptableObject
    {
        public string ServerUrl => _serverUrl;
        public int CooldownSeconds => cooldownSeconds;

        [SerializeField] private string _serverUrl;
        [SerializeField] private int cooldownSeconds;
    }
}