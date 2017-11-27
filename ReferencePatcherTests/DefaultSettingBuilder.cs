using ReferencePatcher.Settings.Internal;
using System.Collections.Generic;
using ReferencePatcher.Settings;

namespace ReferencePatcher.Tests {
    public static class DefaultOptionsBuilder {
        public static void Build() {
            ReferencePatcherSettings.Instance.Variables.Clear();
            ReferencePatcherSettings.Instance.References.Clear();

            ReferencePatcherSettings.Instance.Variables.Add(new Variable() { Name = "dxroot", Value = @"d:\work\source" });
            ReferencePatcherSettings.Instance.Variables.Add(new Variable() { Name = "dxroot2", Value = @"d:\work\source2" });

            ReferencePatcherSettings.Instance.References.Add(new Reference() { Name = "DevExpress.XtraScheduler.v{version}", Path = @"{dxroot2}\20{version}\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler\DevExpress.XtraScheduler.csproj" });
            ReferencePatcherSettings.Instance.References.Add(new Reference() { Name = "DevExpress.XtraScheduler.v{version}.Core", Path = @"{dxroot2}\20{version}\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Core\DevExpress.XtraScheduler.Core.csproj" });
            ReferencePatcherSettings.Instance.References.Add(new Reference() { Name = "DevExpress.XtraScheduler.v{version}.Design", Path = @"{dxroot2}\20{version}\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Design\DevExpress.XtraScheduler.Design.csproj" });
            ReferencePatcherSettings.Instance.References.Add(new Reference() { Name = "DevExpress.XtraScheduler.v{version}.Extensions", Path = @"{dxroot2}\20{version}\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Extensions\DevExpress.XtraScheduler.Extensions.csproj" });
            ReferencePatcherSettings.Instance.References.Add(new Reference() { Name = "DevExpress.XtraScheduler.v{version}.Reporting", Path = @"{dxroot2}\20{version}\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Reporting\DevExpress.XtraScheduler.Reporting.csproj" });
            ReferencePatcherSettings.Instance.References.Add(new Reference() { Name = "DevExpress.XtraScheduler.v{version}.Reporting.Extensions", Path = @"{dxroot2}\20{version}\Win\DevExpress.XtraScheduler\DevExpress.XtraScheduler.Reporting.Extensions\DevExpress.XtraScheduler.Reporting.Extensions.csproj" });
            ReferencePatcherSettings.Instance.References.Add(new Reference() { Name = "DevExpress.Xpf.Scheduler.v{version}", Path = @"{dxroot2}\20{version}\XPF\DevExpress.Xpf.Scheduler\DevExpress.Xpf.Scheduler\DevExpress.Xpf.Scheduler.csproj" });
            ReferencePatcherSettings.Instance.References.Add(new Reference() { Name = "DevExpress.Xpf.Scheduler.v{version}.Design", Path = @"dxroot2}\20{version}\XPF\DevExpress.Xpf.Scheduler\DevExpress.Xpf.Scheduler.Design\DevExpress.Xpf.Scheduler.Design.csproj" });
        }
    }
}
