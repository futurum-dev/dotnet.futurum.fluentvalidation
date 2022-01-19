using FluentValidation.Results;

using Futurum.Core.Linq;
using Futurum.Core.Result;

namespace Futurum.FluentValidation;

/// <summary>
/// ResultError that for validation errors from <see cref="FluentValidation"/>
/// </summary>
public class FluentValidationResultError : IResultErrorNonComposite
{
    private readonly ValidationResult _validationResult;

    internal FluentValidationResultError(ValidationResult validationResult)
    {
        _validationResult = validationResult;
    }

    /// <inheritdoc />
    public string GetErrorString() =>
        _validationResult.Errors
                         .Select(TransformValidationFailureToErrorMessage)
                         .StringJoin(",");

    /// <inheritdoc />
    public ResultErrorStructure GetErrorStructure()
    {
        var children = _validationResult.Errors
                                        .Select(TransformValidationFailureToErrorMessage)
                                        .Select(ResultErrorStructureExtensions.ToResultErrorStructure);

        return new ResultErrorStructure("Validation failure", children);
    }

    private static string TransformValidationFailureToErrorMessage(ValidationFailure validationFailure) =>
        $"Validation failure for '{validationFailure.PropertyName}' with error : '{validationFailure.ErrorMessage}'";
}