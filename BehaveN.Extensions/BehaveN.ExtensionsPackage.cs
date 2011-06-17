using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace BehaveN.BehaveN_Extensions
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidBehaveN_ExtensionsPkgString)]
    public sealed class BehaveN_ExtensionsPackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public BehaveN_ExtensionsPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));

            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                AddCommandHandler(mcs, PkgCmdIDList.cmdidFindStepDefinitionCommand, FindStepDefinitionCommand);
                AddCommandHandler(mcs, PkgCmdIDList.cmdidFindStepsCommand, FindStepsCommand);

                AddCommandHandler(mcs, PkgCmdIDList.cmdidReplaceHumpsWithSpacesCommand, ReplaceHumpsWithSpacesCommand);
                AddCommandHandler(mcs, PkgCmdIDList.cmdidReplaceHumpsWithUnderscoresCommand, ReplaceHumpsWithUnderscoresCommand);
                AddCommandHandler(mcs, PkgCmdIDList.cmdidReplaceSpacesWithHumpsCommand, ReplaceSpacesWithHumpsCommand);
                AddCommandHandler(mcs, PkgCmdIDList.cmdidReplaceSpacesWithUnderscoresCommand, ReplaceSpacesWithUnderscoresCommand);
                AddCommandHandler(mcs, PkgCmdIDList.cmdidReplaceUnderscoresWithHumpsCommand, ReplaceUnderscoresWithHumpsCommand);
                AddCommandHandler(mcs, PkgCmdIDList.cmdidReplaceUnderscoresWithSpacesCommand, ReplaceUnderscoresWithSpacesCommand);
            }
        }

        #endregion

        private static void AddCommandHandler(OleMenuCommandService mcs, uint cmdid, EventHandler handler)
        {
            var cmdId = new CommandID(GuidList.guidBehaveN_ExtensionsCmdSet, (int)cmdid);
            var menuCmd = new MenuCommand(handler, cmdId);
            mcs.AddCommand(menuCmd);
        }

        private static void FindStepDefinitionCommand(object sender, EventArgs e)
        {
            var dte = (EnvDTE80.DTE2)GetGlobalService(typeof(DTE));
            var doc = dte.ActiveDocument;

            if (doc != null)
            {
                var sel = (TextSelection)doc.Selection;
                var text = GetCurrentLine(sel);

                if (!string.IsNullOrEmpty(text))
                {
                    text = Regex.Replace(text, @"((\$\s*)?-?\d+(\.\d+)?)|("".*"")", "arg*");
                    var words = text.Split().ToList();
                    var searchText = string.Join("_", words.GetRange(1, words.Count - 1));
                    FindInFiles(dte, searchText, "*.cs");
                }
            }
        }

        private static void FindStepsCommand(object sender, EventArgs e)
        {
            var dte = (EnvDTE80.DTE2)GetGlobalService(typeof(DTE));
            var doc = dte.ActiveDocument;

            if (doc != null)
            {
                var sel = (TextSelection)doc.Selection;
                var text = GetCurrentLine(sel);

                if (!string.IsNullOrEmpty(text))
                {
                    text = Regex.Replace(text, @"\(.+$", "");
                    text = Regex.Replace(text, @"arg\d+", "*");
                    var words = new List<string>(SplitMethodName(text));
                    var searchText = string.Join(" ", words.GetRange(1, words.Count - 1));
                    FindInFiles(dte, searchText, "*.txt;*.cs");
                }
            }
        }

        private static string GetCurrentLine(TextSelection sel)
        {
            if (sel == null)
            {
                return "";
            }

            if (sel.Text.Length == 0)
            {
                sel.SelectLine();
            }

            return sel.Text.Trim();
        }

        private static IEnumerable<string> SplitMethodName(string text)
        {
            return Regex.Split(text, "_+");
        }

        private static void FindInFiles(EnvDTE80.DTE2 dte, string findWhat, string filesOfType)
        {
            dte.ExecuteCommand("Edit.FindinFiles");
            dte.Find.FindWhat = findWhat;
            dte.Find.Target = vsFindTarget.vsFindTargetFiles;
            dte.Find.MatchCase = false;
            dte.Find.MatchWholeWord = false;
            dte.Find.MatchInHiddenText = true;
            dte.Find.PatternSyntax = vsFindPatternSyntax.vsFindPatternSyntaxWildcards;
            dte.Find.SearchPath = "Entire Solution";
            dte.Find.SearchSubfolders = true;
            dte.Find.FilesOfType = filesOfType;
            dte.Find.ResultsLocation = vsFindResultsLocation.vsFindResults1;
            dte.Find.Action = vsFindAction.vsFindActionFindAll;
            dte.Find.Execute();
            dte.Windows.Item("{CF2DDC32-8CAD-11D2-9302-005345000000}").Close(); // Find and Replace
        }

        private static void ReplaceHumpsWithSpacesCommand(object sender, EventArgs e)
        {
            ReplaceSelectedTextAndReSelect("(?<!^|_)(?=[A-Z])", " ");
        }

        private static void ReplaceHumpsWithUnderscoresCommand(object sender, EventArgs e)
        {
            ReplaceSelectedTextAndReSelect("(?<!^|_)(?=[A-Z])", "_");
        }

        private static void ReplaceSpacesWithHumpsCommand(object sender, EventArgs e)
        {
            ReplaceSelectedTextAndReSelect(" +.", ToHump);
        }

        private static void ReplaceSpacesWithUnderscoresCommand(object sender, EventArgs e)
        {
            ReplaceSelectedTextAndReSelect(" ", "_");
        }

        private static void ReplaceUnderscoresWithHumpsCommand(object sender, EventArgs e)
        {
            ReplaceSelectedTextAndReSelect("_+.", ToHump);
        }

        private static void ReplaceUnderscoresWithSpacesCommand(object sender, EventArgs e)
        {
            ReplaceSelectedTextAndReSelect("_", " ");
        }

        private static void ReplaceSelectedTextAndReSelect(string pattern, string replacement)
        {
            var dte = (EnvDTE80.DTE2)GetGlobalService(typeof(DTE));
            var doc = dte.ActiveDocument;

            if (doc != null)
            {
                var sel = (TextSelection)doc.Selection;

                if (!string.IsNullOrEmpty(sel.Text))
                {
                    var newText = Regex.Replace(sel.Text, pattern, replacement);
                    sel.Text = newText;
                    sel.CharLeft(true, newText.Length);
                }
            }
        }

        private static void ReplaceSelectedTextAndReSelect(string pattern, MatchEvaluator evaluator)
        {
            var dte = (EnvDTE80.DTE2)GetGlobalService(typeof(DTE));
            var doc = dte.ActiveDocument;

            if (doc != null)
            {
                var sel = (TextSelection)doc.Selection;

                if (!string.IsNullOrEmpty(sel.Text))
                {
                    var newText = Regex.Replace(sel.Text, pattern, evaluator);
                    sel.Text = newText;
                    sel.CharLeft(true, newText.Length);
                }
            }
        }

        private static string ToHump(Match m)
        {
            return m.Value[m.Value.Length - 1].ToString().ToUpper();
        }

        private void ShowMessage(string message)
        {
            var uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            var clsid = Guid.Empty;
            int result;
            ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
                0,
                ref clsid,
                "BehaveN.Extensions",
                message,
                string.Empty,
                0,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                OLEMSGICON.OLEMSGICON_INFO,
                0, // false
                out result));
        }
    }
}