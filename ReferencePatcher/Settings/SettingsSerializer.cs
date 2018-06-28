using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ReferencePatcher.Settings.Internal {

    public class SettingsSerializer {
#if DEBUG
        public const string OptionsPath = ".dxdevdebug";
#else
        public const string OptionsPath = ".dxdev";
#endif
        public const string VariablePath = "vars.xml";
        public const string ReferenceTemplatesPath = "references.xml";

        public void Load(ReferencePatcherSettings settings) {
            List<Variable> variables = LoadList<Variable>(VariablePath);
            List<Reference> referenceTemplates = LoadList<Reference>(ReferenceTemplatesPath);

            if (variables == null) {
                variables = CreateDefaultVariables();
                SaveList<Variable>(variables, VariablePath);
            }

            if (referenceTemplates == null) {
                referenceTemplates = CreateDefaultReferenceTemplates();
                SaveList<Reference>(referenceTemplates, ReferenceTemplatesPath);
            }

            settings.Variables.Clear();
            settings.References.Clear();

            settings.Variables.AddRange(variables);
            settings.References.AddRange(referenceTemplates);
        }

        public void Save(ReferencePatcherSettings settings) {
            SaveList<Reference>(settings.References, ReferenceTemplatesPath);
            SaveList<Variable>(settings.Variables, VariablePath);
        } 


        public List<T> LoadList<T>(string fileName) where T : class, new() {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            FileInfo fileInfo = new FileInfo(Path.Combine(GetOptionsDirPath(), fileName));
            if (!fileInfo.Exists)
                return null;
            using (Stream stream = fileInfo.OpenRead())
                return serializer.Deserialize(stream) as List<T>;
        }
        public void SaveList<T>(List<T> list, string fileName) where T : class {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            FileInfo fileInfo = new FileInfo(Path.Combine(GetOptionsDirPath(), fileName));
            EnsureFileInfoExist(fileInfo);
            using (Stream stream = fileInfo.Create())
                serializer.Serialize(stream, list);
        }
        
        public List<Variable> CreateDefaultVariables() {
            List<Variable> variables = new List<Variable>();
            variables.Add(new Variable() { Name = "dxvcsroot", Value = @"d:\work\source" });
            variables.Add(new Variable() { Name = "dxhgroot", Value = @"d:\work\source2" });
            return variables;
        }

        public List<Reference> CreateDefaultReferenceTemplates() {
            List<Reference> result = new List<Reference>();
            result.Add(new Reference("DevExpress.XtraScheduler.v{version}", @"{dxhgroot}\20{version}\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler\DevExpress.XtraScheduler.csproj"));
            result.Add(new Reference("DevExpress.XtraScheduler.v{version}.Core", @"{dxhgroot}\20{version}\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Core\DevExpress.XtraScheduler.Core.csproj"));
            result.Add(new Reference("DevExpress.XtraScheduler.v{version}.Design", @"{dxhgroot}\20{version}\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Design\DevExpress.XtraScheduler.Design.csproj"));
            result.Add(new Reference("DevExpress.XtraScheduler.v{version}.Extensions", @"{dxhgroot}\20{version}\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Extensions\DevExpress.XtraScheduler.Extensions.csproj"));
            result.Add(new Reference("DevExpress.XtraScheduler.v{version}.Reporting", @"{dxhgroot}\20{version}\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Reporting\DevExpress.XtraScheduler.Reporting.csproj"));
            result.Add(new Reference("DevExpress.XtraScheduler.v{version}.Reporting.Extensions", @"{dxhgroot}\20{version}\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Reporting.Extensions\DevExpress.XtraScheduler.Reporting.Extensions.csproj"));
            result.Add(new Reference("DevExpress.Xpf.Scheduler.v{version}", @"{dxhgroot}\20{version}\XPF\DevExpress.Xpf.Scheduler\DevExpress.Xpf.Scheduler\DevExpress.Xpf.Scheduler.csproj"));
            result.Add(new Reference("DevExpress.Xpf.Scheduler.v{version}.Design", @"{dxhgroot}\20{version}\XPF\DevExpress.Xpf.Scheduler\DevExpress.Xpf.Scheduler.Design\DevExpress.Xpf.Scheduler.Design.csproj"));
            result.Add(new Reference("DevExpress.Xpf.Core.v{version}", @"{dxvcsroot}\20{version}\XPF\DevExpress.Xpf.Core\DevExpress.Xpf.Core\DevExpress.Xpf.Core.csproj"));
            return result;
        }

        protected virtual string GetOptionsDirPath() {
            DirectoryInfo optionsDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), OptionsPath));
            return optionsDir.FullName;
        }

        void EnsureFileInfoExist(FileInfo fileInfo) {
            DirectoryInfo dirInfo = fileInfo.Directory;
            if (!dirInfo.Exists)
                dirInfo.Create();
            if (!fileInfo.Exists)
                fileInfo.Create().Close();
        }
    }
    
}
