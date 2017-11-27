using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using ReferencePatcher.Services.Settings;
using ReferencePatcher.Settings.Internal;

namespace ReferencePatcher {
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class SwitchProjectReferenceCommand {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("da8f9a8e-7113-4ae2-afde-1fa4c9851c33");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchProjectReferenceCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private SwitchProjectReferenceCommand(Package package) {
            if (package == null) {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null) {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static SwitchProjectReferenceCommand Instance {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider {
            get {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package) {
            Instance = new SwitchProjectReferenceCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e) {
            DTE2 envDTE = ServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            SelectedItems selectedItems = envDTE.SelectedItems;

            System.Array solutionProjects = envDTE.ActiveSolutionProjects as System.Array;

            List<VSLangProj.Reference> selectedReferences = new List<VSLangProj.Reference>();

            foreach (Project project in solutionProjects) {
                Debug.WriteLine($"{project.Name}");
                VSLangProj.VSProject vsProject = project.Object as VSLangProj.VSProject;

                foreach (SelectedItem item in selectedItems) {
                    foreach (VSLangProj.Reference reference in vsProject.References) {
                        if (reference.Name == item.Name) {
                            selectedReferences.Add(reference);
                        }
                    }
                }
            }

            ReferencePatcherSettings.Instance.Reload();
            ReferencePatcherSettings settings = ReferencePatcherSettings.Instance;
            var calculator = new ProjectPathByReferenceCalculator(settings.References, settings.Variables);
            foreach (VSLangProj.Reference reference in selectedReferences) {
                VSLangProj.VSProject project = reference.ContainingProject.Object as VSLangProj.VSProject;
                List<string> paths = calculator.CalculatePaths(reference.Name);
                if (paths == null || paths.Count < 1) {
                    var newPath = ShowAddReferenceDialog(reference.Name);
                    if (String.IsNullOrEmpty(newPath))
                        continue;
                    paths.Add(newPath);
                }
                string path = paths.FirstOrDefault(somePath => File.Exists(somePath));
                if (String.IsNullOrEmpty(path)) {
                    path = ShowAddReferenceDialog(reference.Name);
                    if (String.IsNullOrEmpty(path))
                        continue;
                }

                Project newProject = null;
                try {
                    newProject = envDTE.Solution.AddFromFile(path);
                } catch {
                    newProject = FindProject(envDTE, path, reference);
                }
                if (newProject == null)
                    return;
                reference.Remove();
                project.References.AddProject(newProject);
            }
        }

        Project FindProject(DTE2 envDte, string path, VSLangProj.Reference reference) {
            foreach (Project item in envDte.Solution.Projects) {
                string assemblyName = item.GetAssemblyName();
                if (Path.GetFileNameWithoutExtension(reference.Path).Equals(assemblyName))
                    return item;
            }
            return null;
        }

        string ShowAddReferenceDialog(string name) {
            AddProjectPathWindow window = new AddProjectPathWindow();
            AddProjectViewModel viewModel = new AddProjectViewModel();

            viewModel.DialogService = new DialogService(window);
            viewModel.SaveOptionsService = new SaveOptionsService();

            viewModel.ReferenceName = name;
            //viewModel.ReferencePath = @"D:\work\source2\2016.2\win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Core\DevExpress.XtraScheduler.Core.proj";
            viewModel.CalculatedVariables.Add(new ReferencePatcher.Settings.Variable() { Name = "version", Value = VersionHelper.VersionFromReferenceName(viewModel.ReferenceName) });
            ReferencePatcherSettings.Instance.References.ForEach(item => viewModel.References.Add(item));
            ReferencePatcherSettings.Instance.Variables.ForEach(item => viewModel.Variables.Add(item));

            viewModel.ReferenceTemplate = viewModel.ReferencePath;
            viewModel.UpdateReferenceTemplate();
            window.DataContext = viewModel;            
            window.ShowDialog();
            return viewModel.ReferencePath;
        }

    }
}
