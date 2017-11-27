using System;
using System.Collections.Generic;
using System.Linq;
using ReferencePatcher.Settings;
using ReferencePatcher.Settings.Internal;

namespace ReferencePatcher.Services.Settings {
    public class SaveOptionsService : ISaveOptionsService {
        void ISaveOptionsService.Save(List<Variable> variables, List<Reference> references) {
            SettingsSerializer serializer = new SettingsSerializer();
            ReferencePatcherSettings.Instance.References.Clear();
            ReferencePatcherSettings.Instance.References.AddRange(references);
            ReferencePatcherSettings.Instance.Variables.Clear();
            ReferencePatcherSettings.Instance.Variables.AddRange(variables);
            serializer.Save(ReferencePatcherSettings.Instance);
        }
    }
}
