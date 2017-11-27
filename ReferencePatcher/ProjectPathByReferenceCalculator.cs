using System;
using System.Collections.Generic;
using System.Linq;

namespace ReferencePatcher.Settings.Internal {
    
    public class ProjectPathByReferenceCalculator {
        List<Reference> referenceTemplates;
        List<Variable> variables;
        public ProjectPathByReferenceCalculator(List<Reference> referenceTemplates, List<Variable> variables) {
            this.referenceTemplates = referenceTemplates;
            this.variables = variables;
        }

        public List<string> CalculatePaths(string referenceName) {
            List<string> result = new List<string>();
            string version = VersionHelper.VersionFromReferenceName(referenceName);
            bool hasVersion = !String.IsNullOrEmpty(version);
            string referencePattern = hasVersion ? referenceName.Replace(version, "{version}") : referenceName;
            IEnumerable<Reference> pathInfos = this.referenceTemplates.Where(item => item.Name == referencePattern || item.Name == referenceName);
            if (pathInfos == null || !hasVersion)
                return result;
            foreach (Reference refTemplate in pathInfos)
                result.Add(ApplyTemplates(refTemplate.Path, version));
            return result;
        }

        string ApplyTemplates(string path, string version) {
            string result = path;
            foreach (var template in variables) {
                result = result.Replace($"{{{template.Name}}}", template.Value);
            }
            return result.Replace("{version}", version);
        }
    }
    
}
