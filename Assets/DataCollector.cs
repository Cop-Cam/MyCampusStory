using System;
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

        [Tooltip("If true, create a separate file for each run.")]
        public bool useRunSpecificFileNames = true;

        [Tooltip("Use an incremental run number for the file name (run1, run2, ...)")]
        public bool useIncrementalRunNumber = true;

        [Tooltip("Use a timestamp in the file name instead of a run number.")]
        public bool useTimestampInFileName = false;

        [Tooltip("Optional description stored at the top of each saved file.")]
        [TextArea]
        public string runDescription = "";

        [Tooltip("Prefix for run-specific files.")]
        public string runFilePrefix = "run";

        private int currentRunNumber;
        private string currentRunId;
        private float elapsedTime;
        private List<float> timeSamples;
        private List<string> eventLogs;

        void Start()
        {
            currentRunNumber = GetNextRunNumber();
            currentRunId = GetRunIdentifier();
            Debug.Log("Persistent data path: " + Application.persistentDataPath);
            Debug.Log("DataCollector run id: " + currentRunId);
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

        void OnApplicationPause(bool pauseStatus)
        {
            // Save data when app is paused (backgrounded) on mobile devices
            // This ensures data is saved before OS kills the app
            if (pauseStatus)
            {
                Debug.Log("App paused. Saving data...");
                if (saveToFile)
                {
                    SaveTimeData();
                }

                if (saveEventsToFile)
                {
                    SaveEventData();
                }
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
            string outputFileNameWithRun = GetRunSpecificFileName(outputFileName);
            string path = Path.Combine(Application.persistentDataPath, outputFileNameWithRun);
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                WriteRunHeader(writer, "Time Data");
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
            string outputFileNameWithRun = GetRunSpecificFileName(eventOutputFileName);
            string path = Path.Combine(Application.persistentDataPath, outputFileNameWithRun);
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                WriteRunHeader(writer, "Event Data");
                writer.WriteLine("Timestamp\tEventName\tDetails");
                foreach (string logEntry in eventLogs)
                {
                    writer.WriteLine(logEntry);
                }
            }

            Debug.Log($"Saved event data to: {path}");
        }

        private string GetRunSpecificFileName(string baseFileName)
        {
            if (!useRunSpecificFileNames)
            {
                return baseFileName;
            }

            string nameWithoutExtension = Path.GetFileNameWithoutExtension(baseFileName);
            string extension = Path.GetExtension(baseFileName);
            return $"{nameWithoutExtension}_{currentRunId}{extension}";
        }

        private void WriteRunHeader(StreamWriter writer, string fileType)
        {
            writer.WriteLine($"Run: {currentRunId}");
            writer.WriteLine($"File Type: {fileType}");
            writer.WriteLine($"Saved At: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            if (!string.IsNullOrEmpty(runDescription))
            {
                writer.WriteLine($"Description: {runDescription}");
            }
            writer.WriteLine();
        }

        private int GetNextRunNumber()
        {
            int nextRun = PlayerPrefs.GetInt("DataCollectorRunNumber", 1);
            PlayerPrefs.SetInt("DataCollectorRunNumber", nextRun + 1);
            PlayerPrefs.Save();
            return nextRun;
        }

        private string GetRunIdentifier()
        {
            if (!useRunSpecificFileNames)
            {
                return string.Empty;
            }

            if (useIncrementalRunNumber)
            {
                return $"{runFilePrefix}{currentRunNumber}";
            }

            if (useTimestampInFileName)
            {
                return $"{runFilePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}";
            }

            return $"{runFilePrefix}{currentRunNumber}";
        }
    }
}
