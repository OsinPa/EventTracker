using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Eidolon.Data;
using Eidolon.SaveSystem;
using UnityEngine;
using UnityEngine.Networking;

namespace Eidolon.EventTracker
{
    public class UnityWebRequestEventTracker : IEventTracker
    {
        private const string SaveSystemKey = "UnityWebRequestEventTracker";
        private const string RequestPostMethod = "POST";
        private const string RequestHeaderName = "Content-Type";
        private const string RequestHeaderValue = "application/json";
        private const long ResponseCodeOk = 200;
        private const int EventListCapacity = 30;

        private readonly string _url;
        private readonly int _cooldownSeconds;
        private readonly ISaveSystem _saveSystem;
        private readonly Events _events;
        private DateTime _lastDateTime;

        public UnityWebRequestEventTracker(EventTrackerData data, ISaveSystem saveSystem)
        {
            _url = data.ServerUrl;
            _cooldownSeconds = data.CooldownSeconds;
            _saveSystem = saveSystem;
            _lastDateTime = DateTime.Now;

            var saveState = _saveSystem.Get(SaveSystemKey);
            _events = JsonUtility.FromJson<Events>(saveState) ?? new Events();

            TryPostEvents();
        }

        public void TrackEvent(string type, string data)
        {
            _events.Add(type, data);
            SaveEvents();

            var currentDateTime = DateTime.Now;
            var spentTime = currentDateTime - _lastDateTime;
            if (spentTime.TotalSeconds >= _cooldownSeconds)
            {
                _lastDateTime = currentDateTime;
                TryPostEvents();
            }
        }

        private void SaveEvents()
        {
            var saveState = JsonUtility.ToJson(_events);
            _saveSystem.Add(SaveSystemKey, saveState);
            _saveSystem.Save();
        }

        private async void TryPostEvents()
        {
            var eventsCount = _events.Count;
            if (eventsCount == 0)
            {
                return;
            }

            using var request = new UnityWebRequest(_url, RequestPostMethod);
            var json = JsonUtility.ToJson(_events);
            var data = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(data);

            request.SetRequestHeader(RequestHeaderName, RequestHeaderValue);
            request.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }

            if (request.responseCode == ResponseCodeOk)
            {
                _events.RemoveRange(0, eventsCount);
                SaveEvents();
            }

            Debug.Log(request.responseCode);
            Debug.Log(request.result);
        }

        [Serializable]
        public class Events
        {
            public int Count => events.Count;
            [SerializeField] private List<EventData> events = new List<EventData>(EventListCapacity);

            public void Add(string type, string data)
            {
                events.Add(new EventData(type, data));
            }

            public void RemoveRange(int index, int count)
            {
                events.RemoveRange(index, count);
            }
        }

        [Serializable]
        public class EventData
        {
            [SerializeField] private string type;
            [SerializeField] private string data;

            public EventData(string type, string data)
            {
                this.type = type;
                this.data = data;
            }
        }
    }
}