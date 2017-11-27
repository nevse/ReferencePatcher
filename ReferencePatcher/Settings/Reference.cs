using System;
using System.Linq;
using System.Xml.Serialization;

namespace ReferencePatcher.Settings {
    [Serializable]
    public class Reference {

        public Reference() {
        }
        public Reference(string name, string path) {
            Name = name;
            Path = path;
        }

        [XmlAttribute]
        public string Name { get; set; }
        [XmlText]
        public string Path { get; set; }
    }
}
