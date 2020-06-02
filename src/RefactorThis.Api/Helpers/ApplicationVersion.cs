using System.Reflection;

namespace RefactorThis.Api.Helpers
{
    public static class ApplicationVersion
    {
        public static string Value { get; } =
            typeof(ApplicationVersion)
                .GetTypeInfo()
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
    }
}