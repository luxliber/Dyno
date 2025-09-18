using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Win32;


namespace DynoInstallCA
{
    public class CustomActions
    {
        private const string RegKey64 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";

        static RegistryKey OpenKey(string key)
        {
            var regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

            return regKey.OpenSubKey(key);
        }

        static string GetDynamoInstallVersion(RegistryKey key)
        {
            if (key != null)
                return key.GetValue("Version") as string;

            return string.Empty;
        }

        [CustomAction]
        public static ActionResult TuneConfig(Session session)
        {

          session.Log("Begin config file tuning");

            try
            {
           /*       var dynamoCoreKey = OpenKey(RegKey64);
                var dynamoCoreKeys = dynamoCoreKey.GetSubKeyNames().Where(s => s.StartsWith("Dynamo Core"));
                if (dynamoCoreKeys.Any())
                {
                    var dynamoVer = GetDynamoInstallVersion(OpenKey(RegKey64 + dynamoCoreKeys.Last()));

                    Version version;
                    Version.TryParse(dynamoVer, out version);

                    if (version == null)
                        throw new Exception($"Dyno has not found Dynamo Core in the system!");

                    var prorubimNodes = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                        $"Prorubim Nodes");

                    var dynamoCfg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        $"Dynamo/Dynamo Revit\\{version.Major}.{version.Minor}\\DynamoSettings.xml");

                    if (!File.Exists(dynamoCfg))
                        throw new Exception($"Dyno has not found Dynamo Revit in the system!");

                    session.Log($"Dynamo {version.Major}.{version.Minor} config file is found: {dynamoCfg}");
                    session.Log($"Processing to change...");


                    var doc = new XmlDocument();
                    doc.Load(dynamoCfg);
                    XmlNode root = doc.DocumentElement;

                    var pkgListNode = root.SelectSingleNode("CustomPackageFolders");
                    var foundNode = pkgListNode.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.InnerText == prorubimNodes);

                    if (foundNode != null)
                        return ActionResult.Success;

                    var elem = doc.CreateElement("string");
                    elem.InnerText = prorubimNodes;
                    pkgListNode.AppendChild(elem);

                    doc.Save(dynamoCfg);
                }
                else
                    throw new Exception($"Dyno has not found Dynamo Core in the system!");*/

                /* var dynoSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Dyno\\Dyno.cfg");
                 if (File.Exists(dynoSettingsPath))
                 {
                     var text = File.ReadAllText(dynoSettingsPath);

                     var splits = text.Split('<', '>');

                     if(splits.Length==3)
                     {
                         var newText = splits[0] + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Dyno\\SampleWorkspaces").Replace(@"\", @"\\")+splits[2];
                        File.WriteAllText(dynoSettingsPath, newText);
                     }
                 }
                 else
                 {
                     session.Log($"Dyno config file: {dynoSettingsPath}, could not be located.");
                     return ActionResult.NotExecuted;
                 }*/



                return ActionResult.Success;

            }
            catch (Exception ex)
            {
                session.Log("There was an error with config file tuning: ");
                session.Log(ex.Message);
                return ActionResult.Failure;
            }
        }



    }
}
