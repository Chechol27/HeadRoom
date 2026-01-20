using System;
using System.Reflection;

public static class StateMachineRegistryGenerator
{
    public static void GeneratePartialRuntimeRegistry<TRegistry>(string templatePath, string templateData,TRegistry registry)
    {
        Type t = typeof(TRegistry);
        var typeInfo = t.GetTypeInfo();
    }
}
