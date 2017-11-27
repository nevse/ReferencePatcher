using NUnit.Framework;
using ReferencePatcher;
using ReferencePatcher.Settings.Internal;
using System.Collections.Generic;
using ReferencePatcher.Settings;

namespace ReferencePatcherTests {

    [TestFixture]
    public class ProjectPathByReferenceCalculatorTests {
        List<Variable> variables;
        List<Reference> referenceTemplates;
        ProjectPathByReferenceCalculator sut;
        TestSettingsSerializer settingsBuilder;
        [SetUp]
        public void SetUp() {
            this.settingsBuilder = new TestSettingsSerializer();
            this.referenceTemplates = this.settingsBuilder.CreateDefaultReferenceTemplates();
            this.variables = this.settingsBuilder.CreateDefaultVariables();            
            this.sut = new ProjectPathByReferenceCalculator(this.referenceTemplates, this.variables);
            this.referenceTemplates.Add(new Reference("reference1.v{version}", "path1"));
            this.referenceTemplates.Add(new Reference("reference1.v{version}", "path2"));
        }

        [TearDown]
        public void TearDown() {
            this.referenceTemplates = null;
            this.variables = null;
            this.sut = null;
            this.settingsBuilder.Dispose();
            this.settingsBuilder = null;
        }

        [Test]
        public void CalculatePath() {
            string result = this.sut.CalculatePaths("DevExpress.XtraScheduler.v15.2.Extensions")[0];
            Assert.AreEqual(@"d:\work\source2\2015.2\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Extensions\DevExpress.XtraScheduler.Extensions.csproj", result);

            result = this.sut.CalculatePaths("DevExpress.Xpf.Core.v15.2")[0];
            Assert.AreEqual(@"d:\work\source\2015.2\XPF\DevExpress.Xpf.Core\DevExpress.Xpf.Core\DevExpress.Xpf.Core.csproj", result);
        }

        [Test]
        public void CalculatePathForUnknownReference() {
            List<string> result = this.sut.CalculatePaths("Some.Reference");
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void CalculatePathForSeveralPathForOneReference() {
            List<string> result = this.sut.CalculatePaths("reference1.v15.2");
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("path1", result[0]);
            Assert.AreEqual("path2", result[1]);
        }
    }
}
