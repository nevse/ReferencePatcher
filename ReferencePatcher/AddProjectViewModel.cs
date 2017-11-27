using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using ReferencePatcher.Settings;

namespace ReferencePatcher {
    public class AddProjectViewModel : ViewModelBase {
        string referenceName;
        string referencePath;
        string referenceTemplate;

        public AddProjectViewModel() {
            Variables = new ObservableCollection<Variable>();
            References = new ObservableCollection<Reference>();
            CalculatedVariables = new ObservableCollection<Variable>();
            SaveCommand = new DelegateCommand(Save);
            UpdateReferenceTemplateCommand = new DelegateCommand(UpdateReferenceTemplate);
            OpenFileCommand = new DelegateCommand(OpenFile);
        }

        public ObservableCollection<Variable> Variables { get; }
        public ObservableCollection<Reference> References { get; }
        public ObservableCollection<Variable> CalculatedVariables { get; }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand UpdateReferenceTemplateCommand { get; set; }
        public DelegateCommand OpenFileCommand { get; set; }

        public IDialogService DialogService { get; set; }
        public ISaveOptionsService SaveOptionsService { get; set; }


        public string ReferenceName {
            get { return referenceName; }
            set { SetProperty(ref referenceName, value, () => ReferenceName); }
        }
        public string ReferencePath {
            get { return referencePath; }
            set { SetProperty(ref referencePath, value, () => ReferencePath); }
        }

        public string ReferenceTemplate {
            get { return referenceTemplate; }
            set { SetProperty(ref referenceTemplate, value, () => ReferenceTemplate); }
        }

        public void UpdateReferenceTemplate() {
            string result = ReferencePath ?? String.Empty;
            result = ReplaceValuesOnVariables(result, Variables);
            result = ReplaceValuesOnVariables(result, CalculatedVariables);
            ReferenceTemplate = result;
        }

        string ReplaceValuesOnVariables(string sourceString, ObservableCollection<Variable> variables) {
            string result = sourceString;
            var sortVariables = variables.OrderByDescending((variable) => variable.Value.Length);

            foreach (var variable in sortVariables) {
                string name = variable.Name.Replace("$", "$$");
                result = Regex.Replace(result, Regex.Escape(variable.Value), $"{{{name}}}", RegexOptions.IgnoreCase);
            }
            return result;
        }

        void OpenFile() {
            string filePath = DialogService.ShowOpenFileDialog(".csproj", "Csproj file (.csproj)|*.csproj");
            if (String.IsNullOrEmpty(filePath))
                return;
            ReferencePath = filePath;
            UpdateReferenceTemplate();
        }

        void Save() {
            List<Reference> newReferences = References.ToList();
            newReferences.Add(new Reference(ReplaceValuesOnVariables(ReferenceName, CalculatedVariables), ReferenceTemplate));
            SaveOptionsService?.Save(this.Variables.ToList(), newReferences);
            DialogService?.Close();
        }
    }
}
