using System;
using System.Collections.Generic;
using ReferencePatcher.Settings;
using ReferencePatcher.Settings.Internal;

namespace ReferencePatcher {
    
    public class ReferencePatcherSettings {
        
        static ReferencePatcherSettings instance; 
        public static ReferencePatcherSettings Instance {
            get {
                if (instance == null) {
                    instance = new ReferencePatcherSettings();
                    new SettingsSerializer().Load(instance);
                }
                return instance;
            }
        }

        protected ReferencePatcherSettings() {
            Variables = new List<Variable>();
            References = new List<Reference>();
        }

        public List<Variable> Variables { get; private set; }
        public List<Reference> References { get; private set; }
        
        public void Reload() {
            SettingsSerializer settingsBuilder = new SettingsSerializer();            
        }
    }
}
