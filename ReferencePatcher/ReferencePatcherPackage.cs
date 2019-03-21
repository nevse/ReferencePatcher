using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;
using ReferencePatcher;
//using System.Threading;
//using System.Threading.Tasks;

namespace ReferencePatcher {
    static class GuidList {
        public const string guidVSReferencePatcherPkgString = "39bf4245-64d8-4180-9c00-b8c809c29c4a";
    };

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
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExistsAndFullyLoaded_string, PackageAutoLoadFlags.BackgroundLoad)]
    [Guid(GuidList.guidVSReferencePatcherPkgString)]
    public sealed class VSPackage1Package : AsyncPackage {

        public VSPackage1Package() {
        }

        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress) {
            await Task.Delay(5000);

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            // Synchronously requesting a service on the UI thread
            var dte = await GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            await SwitchProjectReferenceCommand.InitializeAsync(this, dte);
        }       

    }
}
