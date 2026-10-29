using System.Reflection;
using System.Text.Json.Serialization.Metadata;

namespace iDevWorks.Paystack;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
internal sealed class JsonAliasAttribute(params string[] names) : Attribute
{
    public string[] Names { get; } = names;
}

internal static class JsonAliasModifier
{
    public static void Apply(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object) return;

        foreach (var prop in typeInfo.Properties.ToList())
        {
            if (prop.AttributeProvider is not MemberInfo member || prop.Set is null) continue;

            var aliases = member.GetCustomAttributes<JsonAliasAttribute>()
                                .SelectMany(a => a.Names);

            foreach (var alias in aliases)
            {
                var aliasProp = typeInfo.CreateJsonPropertyInfo(prop.PropertyType, alias);
                aliasProp.Set = prop.Set;   // read-only alias: no Get, so it's never serialized
                typeInfo.Properties.Add(aliasProp);
            }
        }
    }
}