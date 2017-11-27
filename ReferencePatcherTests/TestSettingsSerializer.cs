using System;
using System.IO;
using ReferencePatcher.Settings.Internal;

namespace ReferencePatcherTests {
    public class TestSettingsSerializer : SettingsSerializer {
        string tempPath;
        public string TempPath {
            get {
                if (!String.IsNullOrEmpty(tempPath))
                    return tempPath;
                tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                DirectoryInfo dirInfo = new DirectoryInfo(tempPath);
                if (!dirInfo.Exists)
                    dirInfo.Create();
                return tempPath;
            }
        }

        protected override string GetOptionsDirPath() {
            return TempPath;
        }
        
        public void Dispose() {
            new DirectoryInfo(TempPath).Delete(true);
        }
    }
}
