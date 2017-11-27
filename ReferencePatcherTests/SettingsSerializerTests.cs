using NUnit.Framework;
using ReferencePatcher;
using ReferencePatcher.Settings.Internal;
using System.IO;
using ReferencePatcher.Settings;
using System.Collections.Generic;
using System;

namespace ReferencePatcherTests {

    [TestFixture]
    public class SettingsSerializerTests {
        List<Variable> variables;
        List<Reference> referenceTemplates;
        TestSettingsSerializer sut;
        [SetUp]
        public void SetUp() {
            this.sut = new TestSettingsSerializer();
            this.variables = this.sut.CreateDefaultVariables();
            this.referenceTemplates = this.sut.CreateDefaultReferenceTemplates();
        }

        [TearDown]
        public void TearDown() {
            this.sut = null;
        }

        //[Test]
        //public void Install() {
        //    SettingsBuilder builder = new SettingsBuilder();
        //    builder.SaveReferencePatcherSettings(builder.CreateDefaultProjectReferenceSettings());
        //    builder.SaveRootPathSettings(builder.CreateDefaultRootPathSettings());
        //}

        [Test]
        public void ToFromXmlDefaultRootPathSettings() {
            this.sut.SaveList(this.variables, "someName");

            List<Variable> result = this.sut.LoadList<Variable>("someName");
            int resultCount = result.Count;
            Assert.AreEqual(this.variables.Count, resultCount);
            for (int i = 0; i < resultCount; i++) {
                Assert.AreEqual(this.variables[i].Value, result[i].Value);
                Assert.AreEqual(this.variables[i].Name, result[i].Name);
            }
        }

        [Test]
        public void ToFromXmlDefaultReferencePatcherSettings() {
            this.sut.SaveList(this.referenceTemplates, "someName");

            List<Reference> result = this.sut.LoadList<Reference>("someName");
            Assert.AreEqual(this.referenceTemplates.Count, result.Count);
            for (int i = 0; i < result.Count; i++) {
                Assert.AreEqual(this.referenceTemplates[i].Path, result[i].Path);
                Assert.AreEqual(this.referenceTemplates[i].Name, result[i].Name);
            }
        }

        [Test]
        public void DuplicatePathTemplates() {
            this.referenceTemplates.Add(new Reference("someRef1", "somePath1"));
            this.referenceTemplates.Add(new Reference("someRef1", "somePath2"));
        }

        [Test]
        public void SerializeReferenceTemplates() {
            List<Reference> references = new List<Reference>();
            references.Add(new Reference() { Name = "DevExpress.Data.v{version}", Path = @"{dxvcsroot}\20{version}\Win\DevExpress.Data\DevExpress.Data.csproj" });
            references.Add(new Reference() { Name = "DevExpress.Web.ASPxScheduler.v{version}", Path = @"{dxhgroot}\20{version}\ASP\DevExpress.Web.ASPxScheduler\DevExpress.Web.ASPxScheduler\DevExpress.Web.ASPxScheduler.csproj" });

            this.sut.SaveList(references, "someFile");

            string result = String.Empty;
            FileInfo file = new FileInfo(Path.Combine(this.sut.TempPath, "someFile"));
            using (var reader = file.OpenText())
                result = reader.ReadToEnd();

            string expectedResult =
    @"<?xml version=""1.0""?>
<ArrayOfReference xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Reference Name=""DevExpress.Data.v{version}"">{dxvcsroot}\20{version}\Win\DevExpress.Data\DevExpress.Data.csproj</Reference>
  <Reference Name=""DevExpress.Web.ASPxScheduler.v{version}"">{dxhgroot}\20{version}\ASP\DevExpress.Web.ASPxScheduler\DevExpress.Web.ASPxScheduler\DevExpress.Web.ASPxScheduler.csproj</Reference>
</ArrayOfReference>";
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void SerializeVariables() {
            List<Variable> variables = new List<Variable>();
            variables.Add(new Variable() { Name = "dxvcsroot", Value = @"d:\work\source" });

            this.sut.SaveList(variables, "someFile2");

            string result = String.Empty;
            FileInfo file = new FileInfo(Path.Combine(this.sut.TempPath, "someFile2"));
            using (var reader = file.OpenText())
                result = reader.ReadToEnd();

            string expectedResult =
@"<?xml version=""1.0""?>
<ArrayOfVariable xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Variable Name=""dxvcsroot"">d:\work\source</Variable>
</ArrayOfVariable>";
            Assert.AreEqual(expectedResult, result);
        }
    }
}
