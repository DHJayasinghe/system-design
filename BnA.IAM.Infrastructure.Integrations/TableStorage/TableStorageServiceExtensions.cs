using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BnA.IAM.Infrastructure.Integrations.TableStorage;

internal static class TableStorageServiceExtensions
{
    public static T ToObject<T>(this IDictionary<string, object> source)
        where T : class, new()
    {
        var json = JsonSerializer.Serialize(source);
        return JsonSerializer.Deserialize<T>(json);
    }

    public static Dictionary<string, object> ToDictionary<T>(this T source)
        where T : class, new()
    {
        var json = JsonSerializer.Serialize(source);
        return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
    }

    public static string ClearKey(this string str) => new Regex("[^a-zA-Z0-9 -]").Replace(str, "");
}
