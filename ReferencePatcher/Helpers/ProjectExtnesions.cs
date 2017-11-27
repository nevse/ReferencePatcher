using EnvDTE;

namespace ReferencePatcher
{
    public static class ProjectExtnesions
    {
        public static string GetAssemblyName(this Project project)
        {
            return (string)project.Properties.Item("AssemblyName").Value;
        }
    }
}