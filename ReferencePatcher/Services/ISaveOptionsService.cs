using System;
using System.Collections.Generic;
using System.Linq;
using ReferencePatcher.Settings;

namespace ReferencePatcher {
    public interface ISaveOptionsService {
        void Save(List<Variable> variables, List<Reference> references);
    }
}
