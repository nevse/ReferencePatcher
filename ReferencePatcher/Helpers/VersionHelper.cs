using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReferencePatcher {

    public static class VersionHelper {
        public static string VersionFromReferenceName(string referenceName) {
            Regex re = new Regex(@".*\.v(?<version>\d*\.\d*)");
            Match match = re.Match(referenceName);
            if (!match.Success)
                return String.Empty;
            var matchGroup = match.Groups["version"];
            var success = matchGroup.Success;
            if (!success)
                return String.Empty;
            return matchGroup.Value;
        }
    }

}
