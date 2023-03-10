using FluentValidation.Results;

using Futurum.Core.Linq;
using Futurum.Core.Result;

namespace Futurum.FluentValidation;

/// <summary>
/// ResultError that for validation errors from <see cref="FluentValidation"/>
/// </summary>
public class FluentValidationResultError : IResultErrorNonComposite
{
    internal FluentValidationResultError(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }

    public ValidationResult ValidationResult { get; }

    /// <inheritdoc />
    public string GetErrorStringSafe() =>
        ValidationResult.Errors
                        .Select(TransformValidationFailureToErrorMessage)
                        .StringJoin(",");

    /// <inheritdoc />
    public string GetErrorString() =>
        GetErrorStringSafe();

    /// <inheritdoc />
    public ResultErrorStructure GetErrorStructureSafe()
    {
        var children = ValidationResult.Errors
                                       .Select(TransformValidationFailureToErrorMessage)
                                       .Select(ResultErrorStructureExtensions.ToResultErrorStructure);

        return new ResultErrorStructure("Validation failure", children);
    }

    /// <inheritdoc />
    public ResultErrorStructure GetErrorStructure() =>
        GetErrorStructureSafe();

    private static string TransformValidationFailureToErrorMessage(ValidationFailure validationFailure) =>
        $"Validation failure for '{validationFailure.PropertyName}' with error : '{validationFailure.ErrorMessage}'";
}