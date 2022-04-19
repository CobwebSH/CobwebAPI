using System.Reflection;
using CobwebAPI.Bootstrap;

namespace CobwebAPI.Utilities;

public static class PluginUtilities
{
    public static Assembly[] GetPluginAssemblies()
    {
        return ChainloaderHooks.Plugins.Select(plugin => plugin.Info.Instance.GetType().Assembly).ToArray();
    }
}