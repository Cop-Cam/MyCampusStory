using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MyCampusStory
{
    public class DataCollector : MonoBehaviour
    {
        [Tooltip("Collect elapsed time data while the game is running.")]
        public bool collectTimeData = true;

        [Tooltip("Save collected time data to a file when the application quits.")]
        public bool saveToFile = false;

        [Tooltip("Collect event data while the game is running.")]
        public bool collectEventData = true;

        [Tooltip("Save collected event data to a file when the application quits.")]
        public bool saveEventsToFile = false;

        [Tooltip("Log elapsed time to the Unity Console while the game is running.")]
        public bool logTimeToConsole = true;

        [Tooltip("Filename written to Application.persistentDataPath when saving time data.")]
        public string outputFileName = "time_data.txt";

        [Tooltip("Filename written to Application.persistentDataPath when saving event data.")]
        public string eventOutputFileName = "event_data.txt";

        private float elapsedTime;
        private List<float> timeSamples;
        private List<string> eventLogs;

        void Start()
        {
            elapsedTime = 0f;
            timeSamples = new List<float>();
            eventLogs = new List<string>();
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;

            if (collectTimeData)
            {
                timeSamples.Add(elapsedTime);
            }

            if (logTimeToConsole)
            {
                Debug.Log($"Elapsed time: {elapsedTime:F2} seconds");
            }
        }

        void OnApplicationQuit()
        {
            Debug.Log($"Total elapsed play time: {elapsedTime:F2} seconds");

            if (saveToFile)
            {
                SaveTimeData();
            }

            if (saveEventsToFile)
            {
                SaveEventData();
            }
        }

        public float GetElapsedTimeSeconds()
        {
            return elapsedTime;
        }

        public IReadOnlyList<float> GetTimeSamples()
        {
            return timeSamples.AsReadOnly();
        }

        public void RecordEvent(string eventName, string details = null)
        {
            if (!collectEventData)
            {
                return;
            }

            string logEntry = $"{Time.time:F2}\t{eventName}\t{details ?? string.Empty}";
            eventLogs.Add(logEntry);
            Debug.Log($"DataCollector event: {logEntry}");
        }

        public IReadOnlyList<string> GetEventLogs()
        {
            return eventLogs.AsReadOnly();
        }

        private void SaveTimeData()
        {
            string path = Path.Combine(Application.persistentDataPath, outputFileName);
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine("ElapsedTimeSeconds");
                foreach (float sample in timeSamples)
                {
                    writer.WriteLine(sample.ToString("F4"));
                }
            }

            Debug.Log($"Saved time data to: {path}");
        }

        private void SaveEventData()
        {
            string path = Path.Combine(Application.persistentDataPath, eventOutputFileName);
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine("Timestamp\tEventName\tDetails");
                foreach (string logEntry in eventLogs)
                {
                    writer.WriteLine(logEntry);
                }
            }

            Debug.Log($"Saved event data to: {path}");
        }
    }
}
