using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using LitJson;
using MessageBox = System.Windows.MessageBox;
using MessageBoxOptions = System.Windows.MessageBoxOptions;

namespace Dyno.Models
{
    [Serializable]
    public class DynoSettingsBase : INotifyPropertyChanged
    {
        public bool IsShowDynamoFileVersion { get; set; } = true;
        public bool IsShowSplashScreen { get; set; } = true;

        public static T ReadSettings<T>(string settingsPath) where T : new()
        {
            var dynoSettingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), settingsPath);

            if (File.Exists(dynoSettingsPath))
            {
                var r = new StreamReader(dynoSettingsPath);
                var json = r.ReadToEnd();
                r.Close();
                try
                {
                    var s = JsonMapper.ToObject<T>(json);
                    if (s == null)
                        throw new Exception("Conversion failed");
                    return s;
                }
                catch (Exception e)
                {
                    MessageBox.Show(
                        $"Can`t parse settings file: {dynoSettingsPath}. Default settings will be used and new file will be created. Please check your settings.\nError message:\n{e.Message}",
                        "Dyno",
                        MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK,
                        MessageBoxOptions.DefaultDesktopOnly
                    );
                    WriteSettings(settingsPath, new DynoSettingsBase());
                }
            }
            else
            {
                MessageBox.Show(
                    $"Can`t find settings file: {dynoSettingsPath}. Default settings will be used and new file will be created. Please check your settings",
                    "Dyno",
                    MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK,
                    MessageBoxOptions.DefaultDesktopOnly);
                WriteSettings(settingsPath, new DynoSettingsBase());
            }

            return new T();
        }

        public static void WriteSettings(string path, object settings)
        {
            var dynoSettingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);

            try
            {
                if (!File.Exists(dynoSettingsPath))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(dynoSettingsPath))) // if it doesn't exist, create
                        Directory.CreateDirectory(Path.GetDirectoryName(dynoSettingsPath));

                    File.Create(dynoSettingsPath).Dispose();
                }


                var sw = new StreamWriter(dynoSettingsPath);
                var jwriter = new JsonWriter {PrettyPrint = true};
                JsonMapper.ToJson(settings, jwriter);
                sw.Write(jwriter.TextWriter.ToString());
                sw.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Can`t Save settings file: " + dynoSettingsPath);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}