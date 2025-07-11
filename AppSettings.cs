using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System; // Required for Environment, SpecialFolder, Guid

namespace TurnDownTheLights {

    // Class to hold the actual settings data for JSON serialization
    public class SettingsData {
        public Keys TurnOffHotKey { get; set; }
        public Keys ExitHotKey { get; set; }

        public SettingsData() {
            // Initialize with default values
            TurnOffHotKey = Keys.Control | Keys.Alt | Keys.L;
            ExitHotKey = Keys.Control | Keys.Alt | Keys.X;
        }
    }

    public static class AppSettings {
        private static string settingsFilePath;
        private static SettingsData currentSettings;

        // Static constructor to initialize settings path and load data
        static AppSettings() {
            // Define the path for settings.json in User's AppData\Roaming\YourAppName
            // Using a GUID or unique name for "YourAppName" is good practice if the app name is generic
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appNameFolder = Path.Combine(appDataPath, "TurnDownTheLightsApp"); // Or a more unique name

            // Ensure the directory exists
            if (!Directory.Exists(appNameFolder)) {
                Directory.CreateDirectory(appNameFolder);
            }
            settingsFilePath = Path.Combine(appNameFolder, "settings.json");

            Load();
        }

        public static Keys TurnOffHotKey {
            get { return currentSettings.TurnOffHotKey; }
            set { currentSettings.TurnOffHotKey = value; }
        }

        public static Keys ExitHotKey {
            get { return currentSettings.ExitHotKey; }
            set { currentSettings.ExitHotKey = value; }
        }

        private static void Load() {
            if (File.Exists(settingsFilePath)) {
                try {
                    string json = File.ReadAllText(settingsFilePath);
                    currentSettings = JsonConvert.DeserializeObject<SettingsData>(json);
                    // If deserialization results in null (e.g. empty or invalid file), initialize new settings
                    if (currentSettings == null) {
                        currentSettings = new SettingsData(); // Load defaults
                        Console.WriteLine("Settings file was invalid or empty. Loaded default settings.");
                    }
                } catch (Exception ex) {
                    Console.WriteLine($"Error loading settings: {ex.Message}. Loading default settings.");
                    currentSettings = new SettingsData(); // Load defaults in case of error
                }
            } else {
                Console.WriteLine("Settings file not found. Loading default settings.");
                currentSettings = new SettingsData(); // Load defaults if file doesn't exist
            }
        }

        public static void Save() {
            try {
                string json = JsonConvert.SerializeObject(currentSettings, Formatting.Indented);
                File.WriteAllText(settingsFilePath, json);
                Console.WriteLine($"Settings saved to {settingsFilePath}");
            } catch (Exception ex) {
                Console.WriteLine($"Error saving settings: {ex.Message}");
                // Optionally, notify the user through UI if saving fails critically
            }
        }

        // The concept of UpgradeSettingsIfRequired is less direct with JSON.
        // Defaults are handled by the SettingsData constructor and Load method if file is missing/invalid.
        // If you add new properties to SettingsData in future versions,
        // Newtonsoft.Json will typically ignore missing JSON properties during deserialization,
        // and they will retain their default values from the constructor of SettingsData.
        // So, explicit "Upgrade" logic like in Properties.Settings might not be needed
        // unless you have complex data migration needs.

        // SetDefaultsIfEmpty is effectively handled by the Load() logic and SettingsData constructor.
        // This method can be removed or kept if you want an explicit reset-to-defaults option somewhere.
        // For now, I'll remove it as its role is covered.
        /*
        public static void SetDefaultsIfEmpty() {
            // This logic is now part of Load() and SettingsData constructor
        }
        */
    }
}
