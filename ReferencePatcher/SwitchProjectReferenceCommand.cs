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
using Task = System.Threading.Tasks.Task;

namespace ReferencePatcher {
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class SwitchProjectReferenceCommand {        
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("da8f9a8e-7113-4ae2-afde-1fa4c9851c33");

        public static async Task InitializeAsync(AsyncPackage package, EnvDTE.DTE dte) {
            var commandService = (IMenuCommandService)await package.GetServiceAsync(typeof(IMenuCommandService));
            var cmdId = new CommandID(CommandSet, CommandId);
            var cmd = new MenuCommand((s, e) => Execute(package, dte), cmdId);
            commandService.AddCommand(cmd);
        }

        static void Execute(Package package, EnvDTE.DTE dte) {
            DTE2 envDTE = dte as EnvDTE80.DTE2;
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
        
        static Project FindProject(DTE2 envDte, string path, VSLangProj.Reference reference) {
            foreach (Project item in envDte.Solution.Projects) {
                string assemblyName = item.GetAssemblyName();
                if (Path.GetFileNameWithoutExtension(reference.Path).Equals(assemblyName))
                    return item;
            }
            return null;
        }

        static string ShowAddReferenceDialog(string name) {
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
