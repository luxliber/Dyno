using System;
using System.IO;
using Microsoft.Deployment.WindowsInstaller;


namespace DynoStudioInstallCA
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult TuneConfig(Session session)
        {
            //     Debugger.Launch();
            //    session.Log("Begin config file tuning");
            try
            {
                var dynoStudioSettingsPath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "DynoStudio\\DynoStudio.cfg");
                if (File.Exists(dynoStudioSettingsPath))
                {
                    var text = File.ReadAllText(dynoStudioSettingsPath);
                    var splits = text.Split('<', '>');

                    if (splits.Length == 3)
                    {
                        var newText =
                            splits[0] + Path
                                .Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    "DynoStudio\\SampleWorkspaces").Replace(@"\", @"\\") + splits[2];

                        File.WriteAllText(dynoStudioSettingsPath, newText);
                    }
                }
                else
                {
                    session.Log($"DynoStudio config file: {dynoStudioSettingsPath}, could not be located.");
                    return ActionResult.NotExecuted;
                }

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