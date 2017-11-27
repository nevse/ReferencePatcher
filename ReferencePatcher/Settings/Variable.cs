using System;
using System.Linq;
using System.Xml.Serialization;

namespace ReferencePatcher.Settings {
    [Serializable]
    public class Variable {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}
