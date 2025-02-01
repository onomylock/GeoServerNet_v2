namespace Shared.Common.Models;

public sealed record ClaimEntry
{
    public string Type { get; init; }
    public string Value { get; init; }
    public string ValueType { get; init; }
}