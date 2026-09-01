namespace SmartCoreHub.Core.SDK.Common.Attributes;

/// <summary>
/// Marks a type or member as a thin wrapper (shell) that delegates its implementation
/// to a canonical underlying type in a target package (typically <c>SmartCoreHub.Core.SDK</c>).
/// </summary>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct |
    AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property |
    AttributeTargets.Constructor | AttributeTargets.Field,
    AllowMultiple = true,
    Inherited = false)]
public sealed class SdkWrappedSourceAttribute : Attribute
{
    /// <summary>Fully qualified name of the canonical target type or member being wrapped.</summary>
    public string TargetType { get; }

    /// <summary>Name of the destination package containing the canonical implementation (default: <c>SmartCoreHub.Core.SDK</c>).</summary>
    public string TargetPackage { get; }

    /// <summary>Optional description or context about the delegation.</summary>
    public string Description { get; }

    /// <summary>
    /// Creates a thin-wrapper source tracking attribute.
    /// </summary>
    /// <param name="targetType">FQN of the underlying canonical type in the target package.</param>
    /// <param name="targetPackage">Package where the canonical implementation lives (default: <c>SmartCoreHub.Core.SDK</c>).</param>
    /// <param name="description">Optional description or context.</param>
    public SdkWrappedSourceAttribute(
        string targetType,
        string targetPackage = "SmartCoreHub.Core.SDK",
        string description = "")
    {
        TargetType = targetType;
        TargetPackage = targetPackage;
        Description = description;
    }
}
