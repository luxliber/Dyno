using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using Dyno;
using Dyno.Models.Workspaces;
using LitJson;
using Prorubim.DynoRevitCore.ViewModels;
using Color = System.Drawing.Color;
using RibbonButton = Autodesk.Revit.UI.RibbonButton;
using Size = System.Drawing.Size;

namespace Prorubim.DynoRevitCore
{
    [Transaction(TransactionMode.Manual),
     Regeneration(RegenerationOption.Manual)]
    public class WorkspaceCommandBase : IExternalCommand
    {
        public virtual void Start()
        {
        }

        public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Start();
            return Result.Succeeded;
        }
    }

    public static class WorkspaceCommandsManager
    {
        public static Color GetColorFromString(string s)
        {
            var r = new Random(s.GetHashCode());
            var b = r.Next(0, 11);
            var ca = new[]
            {
                Color.SteelBlue,
                Color.Tomato,
                Color.CadetBlue,
                Color.Firebrick,
                Color.SlateBlue,
                Color.SlateGray,
                Color.DarkKhaki,
                Color.LightCoral,
                Color.LightSalmon,
                Color.Teal,
                Color.DarkOliveGreen,
                Color.SlateGray
            };
            return ca[b];
        }

        internal static string GetTinyText(string s)
        {
            var res = "";
            var words = s.Split(new[] {' ', '\t', '_', '.'}, StringSplitOptions.RemoveEmptyEntries);
            var cwords = new List<string>();

            foreach (var word in words)
                cwords.Add(int.TryParse(word, out var number) ? number.ToString() : word);

            if (cwords.Count == 1)
            {
                res = cwords[0][0].ToString(CultureInfo.InvariantCulture).ToUpper();
                if (cwords[0].Length > 1) res += cwords[0][1].ToString(CultureInfo.InvariantCulture).ToLower();
                if (cwords[0].Length > 2) res += cwords[0][2].ToString(CultureInfo.InvariantCulture).ToLower();
            }

            if (cwords.Count == 2)
            {
                res = cwords[0][0].ToString(CultureInfo.InvariantCulture).ToUpper();
                res += cwords[1][0].ToString(CultureInfo.InvariantCulture).ToLower();
                if (cwords[1].Length > 1) res += cwords[1][1].ToString(CultureInfo.InvariantCulture).ToLower();
            }

            if (cwords.Count > 2)
            {
                res = cwords[0][0].ToString(CultureInfo.InvariantCulture).ToUpper();
                res += cwords[1][0].ToString(CultureInfo.InvariantCulture).ToLower();
                res += cwords[2][0].ToString(CultureInfo.InvariantCulture).ToLower();
            }

            return res;
        }

        public static void CreateRibbonWorkspaceButtons(WorkspaceGroup root, string packageName, bool createButtons,
            List<string> storageFolders)
        {
            var aName = new AssemblyName("WorkspaceCommand");

            var modulePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Dyno\\");
            var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave,
                modulePath);

            var mb = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            foreach (var storageFolder in storageFolders)
            {
                var buttonsFile = Path.Combine(storageFolder, "buttons.txt");
                CreateRibbonWorkspaceButtons(root, packageName, buttonsFile, mb, createButtons);
            }

            try
            {
                if (createButtons)
                    ab.Save(aName.Name + ".dll");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        internal static void CreateRibbonWorkspaceButtons(WorkspaceGroup root, string packageName, string buttonsFile,
            ModuleBuilder mb,
            bool createButtons)
        {
            if (!File.Exists(buttonsFile))
                return;

            var r = new StreamReader(buttonsFile);

            var json = r.ReadToEnd();
            r.Close();

            if (json == string.Empty)
                return;

            try
            {
                var jreader = new JsonReader(json) {AllowComments = true};
                var jmdata = JsonMapper.ToObject(jreader);

                //lets iterate tabs in buttons file
                foreach (KeyValuePair<string, JsonData> it in jmdata)
                {
                    //iterate names in each tab
                    var tab = it.Key;
                    foreach (var name in it.Value)
                    {
                        //try to find workspace by name
                        var w = root.GetWorkspaceByInnerName(name.ToString());

                        //if wp does not have presets create single button
                        if (w != null && w.IsNoPresets)
                        {
                            CreatePresetButton(packageName, tab, w.WorkspacePresets.First(), w.WorkspaceGroup.Name, mb,
                                createButtons);
                            continue;
                        }

                        //else if wp have any presets create list button
                        if (w != null && !w.IsNoPresets)
                        {
                            CreateWorkspaceListButton(packageName, tab, w, mb, createButtons);
                            continue;
                        }

                        //try to find wp preset by name and create single button 
                        var wp = root.GetWorkspacePresetByInnerName(name.ToString());
                        if (wp != null)
                        {
                            CreatePresetButton(packageName, tab, wp, wp.Workspace.WorkspaceGroup.Name, mb,
                                createButtons);
                            continue;
                        }

                        //try to find wp preset by wp name pr name and create single button 
                        var splits = name.ToString().Split(new[] {':'}, 2);

                        if (splits.Length > 1)
                        {
                            wp = root.GetWorkspacePresetByInnerNameAndWorkspaceInnerName(
                                splits[0].Trim(), splits[1].Trim());
                            if (wp != null)
                            {
                                CreatePresetButton(packageName, tab, wp, wp.Workspace.WorkspaceGroup.Name, mb,
                                    createButtons);
                                continue;
                            }
                        }

                        //try to find group by name and create list button with presets
                        var wg = root.GetWorkspaceGroupByName(name.ToString());
                        if (wg != null)
                        {
                            CreateGroupListButton(packageName, tab, wg, mb, createButtons);
                            continue;
                        }

                        //try to find playlist by name and create list button with presets
                        var wpl = root.GetPlaylistByName(name.ToString());
                        if (wpl != null)
                        {
                            CreatePlaylistListButton(packageName, tab, wpl, mb, createButtons);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var err = $"Can`t create ribbon buttons (with file: {buttonsFile})";
                err += "\n\n";
                err += "Error text: \n";
                err += exception.Message;
                MessageBox.Show(err);
            }
        }

        private static void CreateGroupListButton(string packageName, string tab, WorkspaceGroup wg, ModuleBuilder mb,
            bool createButtons)
        {
            if (mb == null) throw new ArgumentNullException(nameof(mb));

            var isRibbonTabCreated = false;

            var wgButtonName = wg.Name;
            var circleColor = GetColorFromString(wg.Name);
            wg.RibbonButtonColor = circleColor;

            foreach (var workspacePreset in wg.WorkspacePresets)
                workspacePreset.RibbonButtonColor = circleColor;

            if (!createButtons)
                return;

            if (ComponentManager.Ribbon.Tabs.FirstOrDefault(t => t.Name == tab) != null)
                isRibbonTabCreated = true;

            if (!isRibbonTabCreated)
                DynoAppBase.UiControlledApp.CreateRibbonTab(tab);

            var ribbonPanel =
                DynoAppBase.UiControlledApp.GetRibbonPanels(tab).Find(x => x.Name == wg.ParentGroup.Name) ??
                DynoAppBase.UiControlledApp.CreateRibbonPanel(tab, wg.ParentGroup.Name);

            foreach (var workspacePreset in wg.WorkspacePresets)
                workspacePreset.RibbonButtonColor = circleColor;

            if (ribbonPanel.GetItems().FirstOrDefault(x => x.Name == wgButtonName) != null)
                return;

            var dynoButton = (PulldownButton) ribbonPanel.AddItem(
                new PulldownButtonData(
                    wgButtonName,
                    wgButtonName));

            var groupImagePath = wg.ImagePath;
            if (groupImagePath != null)
                LoadBitmapForButton(groupImagePath, dynoButton);
            else
                CreateBitmapForListButton(circleColor, wg.Name, dynoButton);

            foreach (var workspacePreset in wg.WorkspacePresets)
            {
                var wpNameForDef = Regex.Replace(workspacePreset.InnerName, @"[^\w]", "_");
                CreateWorkspaceCommandClass(packageName, workspacePreset.InnerName, mb);
                var wpButtonName = workspacePreset.InnerName;
                var b = new PushButtonData(
                    wpButtonName,
                    wpButtonName,
                    mb.FullyQualifiedName,
                    "WorkspaceCommand_" + wpNameForDef)
                {
                    ToolTip = string.IsNullOrEmpty(workspacePreset.Desc) ? wpButtonName : workspacePreset.Desc,
                };

                var pushButton = dynoButton.AddPushButton(b);
                var workspaceImagePath = workspacePreset.Workspace.ImagePath;
                if (workspaceImagePath != null)
                    LoadBitmapForButton(workspaceImagePath, pushButton);
                else
                    CreateBitmapForSingleButton(circleColor, workspacePreset.Workspace.Name, pushButton);
            }
        }

        private static void CreatePlaylistListButton(string packageName, string tab, WorkspacePlaylist wpl,
            ModuleBuilder mb,
            bool createButtons)
        {
            if (mb == null) throw new ArgumentNullException(nameof(mb));

            var isRibbonTabCreated = false;

            var wgButtonName = wpl.Name;
            var circleColor = GetColorFromString(wpl.Name);
            wpl.RibbonButtonColor = circleColor;

            foreach (var workspacePreset in wpl.Presets)
                workspacePreset.RibbonButtonColor = circleColor;

            if (!createButtons)
                return;

            if (ComponentManager.Ribbon.Tabs.FirstOrDefault(t => t.Name == tab) != null)
                isRibbonTabCreated = true;

            if (!isRibbonTabCreated)
                DynoAppBase.UiControlledApp.CreateRibbonTab(tab);

            var ribbonPanel = DynoAppBase.UiControlledApp.GetRibbonPanels(tab)
                                  .Find(x => x.Name == wpl.WorkspaceGroup.Name) ??
                              DynoAppBase.UiControlledApp.CreateRibbonPanel(tab, wpl.WorkspaceGroup.Name);

            foreach (var workspacePreset in wpl.Presets)
                workspacePreset.RibbonButtonColor = circleColor;

            if (ribbonPanel.GetItems().FirstOrDefault(x => x.Name == wgButtonName) != null)
                return;

            var dynoButton = (PulldownButton) ribbonPanel.AddItem(
                new PulldownButtonData(
                    wgButtonName,
                    wgButtonName));

            var imagePath = wpl.ImagePath;
            if (imagePath != null)
                LoadBitmapForButton(imagePath, dynoButton);
            else
                CreateBitmapForListButton(circleColor, wpl.Name, dynoButton);

            foreach (var workspacePreset in wpl.Presets)
            {
                var wpNameForDef = Regex.Replace(workspacePreset.InnerName, @"[^\w]", "_");
                CreateWorkspaceCommandClass(packageName, workspacePreset.InnerName, mb);
                var wpButtonName = workspacePreset.InnerName;
                var b = new PushButtonData(
                    wpButtonName,
                    wpButtonName,
                    mb.FullyQualifiedName,
                    "WorkspaceCommand_" + wpNameForDef)
                {
                    ToolTip = string.IsNullOrEmpty(workspacePreset.Desc) ? wpButtonName : workspacePreset.Desc,
                };

                var pushButton = dynoButton.AddPushButton(b);
                var workspaceImagePath = workspacePreset.Workspace.ImagePath;
                if (workspaceImagePath != null)
                    LoadBitmapForButton(workspaceImagePath, pushButton);
                else
                    CreateBitmapForSingleButton(circleColor, workspacePreset.Workspace.Name, pushButton);
            }
        }

        private static void CreateWorkspaceListButton(string packageName, string tab, Workspace w, ModuleBuilder mb,
            bool createButtons)
        {
            var isRibbonTabCreated = false;

            var wgButtonName = w.Name;
            var circleColor = GetColorFromString(w.Name);

            foreach (var workspacePreset in w.WorkspacePresets)
                workspacePreset.RibbonButtonColor = circleColor;

            if (!createButtons)
                return;

            if (ComponentManager.Ribbon.Tabs.FirstOrDefault(t => t.Name == tab) != null)
                isRibbonTabCreated = true;

            if (!isRibbonTabCreated)
                DynoAppBase.UiControlledApp.CreateRibbonTab(tab);

            var ribbonPanel =
                DynoAppBase.UiControlledApp.GetRibbonPanels(tab).Find(x => x.Name == w.WorkspaceGroup.Name) ??
                DynoAppBase.UiControlledApp.CreateRibbonPanel(tab, w.WorkspaceGroup.Name);

            if (ribbonPanel.GetItems().FirstOrDefault(x => x.Name == wgButtonName) != null)
                return;

            var dynoButton = (PulldownButton) ribbonPanel.AddItem(
                new PulldownButtonData(
                    wgButtonName,
                    wgButtonName));

            var imagePath = w.ImagePath;
            if (imagePath != null)
                LoadBitmapForButton(imagePath, dynoButton);
            else
                CreateBitmapForListButton(circleColor, w.Name, dynoButton);

            foreach (var workspacePreset in w.WorkspacePresets)
            {
                var wpNameForDef = Regex.Replace(workspacePreset.InnerName, @"[^\w]", "_");

                CreateWorkspaceCommandClass(packageName, workspacePreset.InnerName, mb);

                var wpButtonName = workspacePreset.InnerName;
                var b = new PushButtonData(
                        wpButtonName,
                        wpButtonName,
                        mb.FullyQualifiedName,
                        "WorkspaceCommand_" + wpNameForDef)
                    {ToolTip = string.IsNullOrEmpty(workspacePreset.Desc) ? wpButtonName : workspacePreset.Desc};
                dynoButton.AddPushButton(b);
            }
        }

        private static void CreateBitmapForListButton(Color circleColor, string name, PulldownButton dynoButton)
        {
            var imageText = GetTinyText(name);

            var bigBmp = new Bitmap(32, 32, PixelFormat.Format32bppArgb);
            var bigGr = Graphics.FromImage(bigBmp);
            bigGr.SmoothingMode = SmoothingMode.HighQuality;

            var circlePen = new Pen(circleColor, 3);
            bigGr.FillEllipse(Brushes.White, 1, 1, 31, 31);
            bigGr.DrawEllipse(circlePen, 1, 1, 29, 29);

            bigGr.TextRenderingHint = TextRenderingHint.AntiAlias;
            var strFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            var lineSize = bigGr.MeasureString(imageText, new Font("Myriad Pro Cond", 17, GraphicsUnit.Pixel));
            if (lineSize.Width > 31) imageText = imageText.Remove(imageText.Length - 1);

            bigGr.DrawString(imageText, new Font("Myriad Pro Cond", 17, GraphicsUnit.Pixel), Brushes.Black,
                new RectangleF(0, 0, 32, 32), strFormat);

            var bitmapSourceLarge =
                Imaging.CreateBitmapSourceFromHBitmap(
                    bigBmp.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            var smallBmp = new Bitmap(bigBmp, new Size(16, 16));

            var smallBitmapSourceLarge =
                Imaging.CreateBitmapSourceFromHBitmap(
                    smallBmp.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            dynoButton.LargeImage = bitmapSourceLarge;
            dynoButton.Image = smallBitmapSourceLarge;

            bigBmp.Dispose();
            smallBmp.Dispose();
            bigGr.Dispose();
            circlePen.Dispose();
        }

        private static void CreateWorkspaceCommandClass(string packageName, string wpName, ModuleBuilder mb)
        {
            var wpNameForDef = Regex.Replace(wpName, @"[^\w]", "_");

            var res = mb.GetTypes().FirstOrDefault(x => x.Name == "WorkspaceCommand_" + wpNameForDef);
            if (res != null) return;

            var tb = mb.DefineType("WorkspaceCommand_" + wpNameForDef,
                TypeAttributes.Public | TypeAttributes.Class, typeof(WorkspaceCommandBase), null);

            var transactionAttributeParams = new[] {typeof(TransactionMode)};
            var transactionAttrInfo = typeof(TransactionAttribute).GetConstructor(transactionAttributeParams);

            if (transactionAttrInfo == null) return;

            var transactionAttributeBuilder = new CustomAttributeBuilder(transactionAttrInfo,
                new object[] {TransactionMode.Manual});
            tb.SetCustomAttribute(transactionAttributeBuilder);


            var mbExecute = tb.DefineMethod("Start",
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig |
                MethodAttributes.ReuseSlot, null, Type.EmptyTypes);

            var ilGen = mbExecute.GetILGenerator();
            ilGen.Emit(OpCodes.Ldstr, wpName);
            ilGen.Emit(OpCodes.Ldstr, packageName);

            Type[] paramListExecuteWorkspaceCommand = {typeof(string), typeof(string)};

            ilGen.EmitCall(OpCodes.Call,
                typeof(DynoRevitManagerBase).GetMethod("ExecuteWorkspaceFromCommand",
                    BindingFlags.Static | BindingFlags.Public),
                paramListExecuteWorkspaceCommand);

            ilGen.Emit(OpCodes.Ret);


            tb.CreateType();


            /*     var compileUnit = new CodeCompileUnit();
                 var codeNamespace = new CodeNamespace("Dyno.WorkspaceCommands");
                 codeNamespace.Imports.Add(new CodeNamespaceImport("Dyno.Models.Workspaces"));
                 codeNamespace.Imports.Add(new CodeNamespaceImport("Prorubim.DynoRevitCore"));
                 codeNamespace.Imports.Add(new CodeNamespaceImport("rorubim.DynoRevitCore.ViewModels"));
     
                 var codeType =
                     new CodeTypeDeclaration("WorkspaceCommand_" + wpNameForDef)
                     {
                         Attributes = MemberAttributes.Public,
                         BaseTypes = {typeof(WorkspaceCommandBase)}
                     };
     
                 var startMethod = new CodeMemberMethod
                 {
                     Attributes = MemberAttributes.Public
                 };
                 var mainexp1 = new CodeMethodInvokeExpression(
                     new CodeTypeReferenceExpression("System.Console"),
                     "WriteLine", new CodePrimitiveExpression("Inside Main ..."));
                 startMethod.Statements.Add(mainexp1);
     
                 //     CodeStatement cs = new CodeVariableDeclarationStatement(typeof(CodeDomSample), "cs", new CodeObjectCreateExpression(new CodeTypeReference(typeof(CodeDomSample))));
                 //   mainmethod.Statements.Add(cs);
                 //newType.Members.Add(constructor);
                 codeType.Members.Add(startMethod);
                 codeNamespace.Types.Add(codeType);
                 compileUnit.Namespaces.Add(codeNamespace);
     
     
                 var codeProvider = CodeDomProvider.CreateProvider("CSharp");
                 var tw = new IndentedTextWriter(new StringWriter(), "    ");
     
                 codeProvider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());
                 tw.Close();n*/

            //  codeProvider.CompileAssemblyFromDom()
        }

        private static void CreatePresetButton(string packageName, string tab, WorkspacePreset wp,
            string workspaceGroup, ModuleBuilder mb,
            bool createButtons)
        {
            var isRibbonTabCreated = false;


            var wpNameForDef = Regex.Replace(wp.InnerName, @"[^\w]", "_");
            var wpButtonName = wp.ButtonName;
            var circleColor = GetColorFromString(workspaceGroup);
            wp.RibbonButtonColor = circleColor;

            if (!createButtons)
                return;

            CreateWorkspaceCommandClass(packageName, wp.InnerName, mb);

            if (ComponentManager.Ribbon.Tabs.FirstOrDefault(t => t.Name == tab) != null)
                isRibbonTabCreated = true;

            if (!isRibbonTabCreated)
                DynoAppBase.UiControlledApp.CreateRibbonTab(tab);

            var ribbonPanel = DynoAppBase.UiControlledApp.GetRibbonPanels(tab).Find(x => x.Name == workspaceGroup) ??
                              DynoAppBase.UiControlledApp.CreateRibbonPanel(tab, workspaceGroup);

            if (ribbonPanel.GetItems().FirstOrDefault(x => x.Name == wpButtonName) != null)
                return;


            var dynoButton = (PushButton) ribbonPanel.AddItem(
                new PushButtonData(
                        wpButtonName,
                        wpButtonName,
                        mb.FullyQualifiedName,
                        "WorkspaceCommand_" + wpNameForDef)
                    {ToolTip = string.IsNullOrEmpty(wp.Desc) ? wpButtonName : wp.Desc});

            var imagePath = wp.Workspace.ImagePath;
            if (imagePath != null)
                LoadBitmapForButton(imagePath, dynoButton);
            else
                CreateBitmapForSingleButton(circleColor, wp.InnerName, dynoButton);
        }

        private static void LoadBitmapForButton(string imagePath, RibbonButton dynoButton)
        {
            var bigBmp = new Bitmap(32, 32, PixelFormat.Format32bppArgb);
            var imgBmp = new Bitmap(imagePath);

            using (var g = Graphics.FromImage(bigBmp))
            {
                var r = new RectangleF(0, 0, 32, 32);
                var path = new GraphicsPath();
                path.AddEllipse(r);

                //    g.Clip = new Region(path);

                var units = GraphicsUnit.Pixel;

                g.DrawImage(imgBmp, r, imgBmp.GetBounds(ref units), units);
            }

            var bitmapSourceLarge =
                Imaging.CreateBitmapSourceFromHBitmap(
                    bigBmp.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            var smallBmp = new Bitmap(bigBmp, new Size(16, 16));

            var smallBitmapSourceLarge =
                Imaging.CreateBitmapSourceFromHBitmap(
                    smallBmp.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            dynoButton.LargeImage = bitmapSourceLarge;
            dynoButton.Image = smallBitmapSourceLarge;

            imgBmp.Dispose();
            bigBmp.Dispose();
            smallBmp.Dispose();
        }

        private static void CreateBitmapForSingleButton(Color circleColor, string name, PushButton dynoButton)
        {
            var imageText = GetTinyText(name);

            var bigBmp = new Bitmap(32, 32, PixelFormat.Format32bppArgb);
            var bigGr = Graphics.FromImage(bigBmp);
            bigGr.SmoothingMode = SmoothingMode.HighQuality;


            Brush circleBrush = new SolidBrush(circleColor);
            bigGr.FillEllipse(circleBrush, 1, 1, 31, 31);

            bigGr.TextRenderingHint = TextRenderingHint.AntiAlias;
            var strFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            var lineSize = bigGr.MeasureString(imageText, new Font("Myriad Pro Cond", 17, GraphicsUnit.Pixel));
            if (lineSize.Width > 31) imageText = imageText.Remove(imageText.Length - 1);

            bigGr.DrawString(imageText, new Font("Myriad Pro Cond", 17, GraphicsUnit.Pixel), Brushes.White,
                new RectangleF(1, 1, 33, 33), strFormat);

            var bitmapSourceLarge =
                Imaging.CreateBitmapSourceFromHBitmap(
                    bigBmp.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            var smallBmp = new Bitmap(bigBmp, new Size(16, 16));

            var smallBitmapSourceLarge =
                Imaging.CreateBitmapSourceFromHBitmap(
                    smallBmp.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            dynoButton.LargeImage = bitmapSourceLarge;
            dynoButton.Image = smallBitmapSourceLarge;

            bigBmp.Dispose();
            smallBmp.Dispose();
            bigGr.Dispose();
            circleBrush.Dispose();
        }
    }
}