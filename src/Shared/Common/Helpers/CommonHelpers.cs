using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace Shared.Common.Helpers;

public static class CommonHelpers
{
    public static readonly JsonSerializerOptions DefaultJsonSerializerOptions;

    static CommonHelpers()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = null,
            IncludeFields = true,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        //In JS/TS there might be a problem converting string enum into a number, either disable that converter or use https://pastebin.com/raw/uxndBZgZ
        jsonOptions.Converters.Add(new JsonStringEnumConverter());
        //jsonOptions.Converters.Add(new StringTrimmingJsonConverter());

        DefaultJsonSerializerOptions = jsonOptions;
    }

    public static Dictionary<string, (string Description, string AssemblyQualifiedName)>
        GetTypePropertyDescriptionsAndTypes(Type type)
    {
        var result = type
            .GetProperties()
            .Where(_ => _.GetCustomAttribute<DescriptionAttribute>() != null)
            .Select(_ =>
            {
                var typeSelected = _.PropertyType;

                //Incase property type is enumerable, use it's generic type instead
                if (typeSelected.IsGenericType && typeSelected.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    typeSelected = _.PropertyType.GenericTypeArguments[0];

                //Incase property type is dictionary, use it's Value generic type instead
                if (typeSelected.IsGenericType && typeSelected.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    typeSelected = _.PropertyType.GenericTypeArguments[1];

                return new
                {
                    Name = _.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? _.Name,
                    _.GetCustomAttribute<DescriptionAttribute>()?.Description,
                    //Do not set AssemblyQualifiedName for types that not applied with [CustomDescription] attribute
                    AssemblyQualifiedName =
                        typeSelected.GetCustomAttribute<DescriptionAttribute>() == null
                            ? null
                            : typeSelected.AssemblyQualifiedName
                };
            })
            .Where(_ => _.Description != null)
            .ToDictionary(
                _ => _.Name,
                _ => (_.Description, _.AssemblyQualifiedName)
            );

        return result;
    }

    public static string GetTypeDescription(Type type)
    {
        var modelDescription = type.GetCustomAttribute<DescriptionAttribute>()?.Description;

        return modelDescription;
    }

    public static byte[] GenerateRandomBytes(int byteCount)
    {
        var csp = RandomNumberGenerator.Create();
        var buffer = new byte[byteCount];
        csp.GetBytes(buffer);
        return buffer;
    }

    public static string GenerateRandomString(string set, int length)
    {
        var rand = RandomNumberGenerator.Create();
        var sb = new StringBuilder();

        var buffer = new byte[sizeof(int)];

        var range = length + 1;

        for (var i = 0; i < length; i++)
        {
            rand.GetBytes(buffer);

            var value = (int)(BitConverter.ToUInt32(buffer) % range);

            sb.Append(set[value]);
        }

        return sb.ToString();
    }

    public static string ToHttpQueryString(object obj, JsonSerializerOptions jsonSerializerOptions = null)
    {
        return QueryHelpers.AddQueryString(
            "",
            JsonSerializer.Deserialize<Dictionary<string, object>>(JsonSerializer.Serialize(obj, jsonSerializerOptions),
                    jsonSerializerOptions)
                .Where(_ => _.Value is not null)
                .Select(_ => new KeyValuePair<string, StringValues>(_.Key, _.Value.ToString()))
        );
    }

    public static IEnumerable<KeyValuePair<string, string>> ToKeyValues(object obj,
        JsonSerializerOptions jsonSerializerOptions = null)
    {
        return JsonSerializer
            .Deserialize<Dictionary<string, object>>(JsonSerializer.Serialize(obj, jsonSerializerOptions),
                jsonSerializerOptions)
            .Where(_ => _.Value is not null)
            .Select(_ => new KeyValuePair<string, string>(_.Key, _.Value.ToString()));
    }

    public static string ComputeSha256(this byte[] data)
    {
        var sha256 = SHA256.Create();
        sha256.Initialize();
        var hash = sha256.ComputeHash(data);

        var sb = new StringBuilder();

        foreach (var @byte in hash) sb.Append($"{@byte:x2}");

        return sb.ToString();
    }

    public static string ToHexString(this byte[] data)
    {
        var sb = new StringBuilder();

        foreach (var @byte in data) sb.Append($"{@byte:x2}");

        return sb.ToString();
    }

    /// <summary>
    ///     Use to launch tasks, ignoring execution result, suppressing warnings, alternative to _ = SomeAwaitableTask();
    /// </summary>
    /// <param name="task"></param>
    public static void FireAndForget(this Task task)
    {
        // Intentionally empty
    }
#nullable enable
    public static Task NullableTaskWaitAsync(Task? task, CancellationToken cancellationToken = default)
    {
        return task is not null ? task.WaitAsync(cancellationToken) : Task.CompletedTask;
    }

    public static Task NullableCtsCancelAsync(CancellationTokenSource? cts)
    {
        return cts is not null ? cts.CancelAsync() : Task.CompletedTask;
    }
}