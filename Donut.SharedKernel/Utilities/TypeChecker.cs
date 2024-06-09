using System.Reflection;

namespace Donut.SharedKernel.Utilities;

public static class TypeChecker
{
    public static bool IsAssignableToType<T>(TypeInfo info)
    {
        return typeof(T).IsAssignableFrom(info) && !info.IsAbstract && !info.IsInterface;
    }
}
