using NUnit.Framework;
using ReferencePatcher;

namespace ReferencePatcherTests {
    [TestFixture]
    public class VersionHelperTests {
        [Test]
        public void VersionFromReferenceName() {
            Assert.AreEqual("15.3", VersionHelper.VersionFromReferenceName("DevExpress.SomeProduct.v15.3"));
            Assert.AreEqual("13.10", VersionHelper.VersionFromReferenceName("DevExpress.SomeProduct.Lol.v13.10.Core"));
            Assert.AreEqual("12.2", VersionHelper.VersionFromReferenceName("DevExpress.SomeProduct.v12.2.Core.Extension"));
        }
    }
    
}
