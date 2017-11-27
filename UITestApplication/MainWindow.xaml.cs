using System;
using System.Linq;
using System.Windows;
using ReferencePatcher;
using ReferencePatcher.Services.Settings;

namespace UITestApplication {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        void btnShowAddReferenceDialog_Click(object sender, RoutedEventArgs e) {
            AddProjectPathWindow window = new AddProjectPathWindow();
            AddProjectViewModel viewModel = new AddProjectViewModel();

            viewModel.DialogService = new DialogService(window);
            viewModel.SaveOptionsService = new SaveOptionsService();

            viewModel.ReferenceName = "DevExpress.XtraScheduler.v16.2.Core";
            viewModel.ReferencePath = @"D:\work\source2\2016.2\win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Core\DevExpress.XtraScheduler.Core.proj";
            viewModel.CalculatedVariables.Add(new ReferencePatcher.Settings.Variable() { Name = "version", Value = VersionHelper.VersionFromReferenceName(viewModel.ReferenceName) });
            ReferencePatcherSettings.Instance.References.ForEach(item => viewModel.References.Add(item));
            ReferencePatcherSettings.Instance.Variables.ForEach(item => viewModel.Variables.Add(item));

            viewModel.ReferenceTemplate = viewModel.ReferencePath;
            viewModel.UpdateReferenceTemplate();
            window.DataContext = viewModel;
            window.Show();
        }

    }


    

}
