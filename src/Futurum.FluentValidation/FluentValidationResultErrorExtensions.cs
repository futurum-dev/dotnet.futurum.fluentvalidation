using FluentValidation.Results;

using Futurum.Core.Result;

namespace Futurum.FluentValidation;

/// <summary>
/// Extension methods to transform a <see cref="FluentValidation"/> <see cref="ValidationResult"/> into a <see cref="IResultError"/> 
/// </summary>
public static class FluentValidationResultErrorExtensions
{
    /// <summary>
    /// Transform a FluentValidation <see cref="ValidationResult"/> into a <see cref="IResultError"/>
    /// </summary>
    public static IResultError ToResultError(this ValidationResult validationResult) =>
        new FluentValidationResultError(validationResult);

    /// <summary>
    /// Transform a FluentValidation <see cref="ValidationResult"/> into a <see cref="IResultError"/>
    /// </summary>
    public static IResultError ToResultError<T>(this ValidationResult validationResult, T validationObject) =>
        ResultErrorCompositeExtensions.ToResultError($"Failed to Validate object, type : '{validationObject.GetType().FullName}'".ToResultError(),
                                                     validationResult.ToResultError());
}