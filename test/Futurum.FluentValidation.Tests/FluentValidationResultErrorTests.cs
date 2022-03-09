using FluentAssertions;

using FluentValidation;

using Xunit;

namespace Futurum.FluentValidation.Tests;

public class FluentValidationResultErrorTests
{
    public class GetErrorStructure
    {
        [Fact]
        public void success()
        {
            var value = new ObjectToValidate(Guid.NewGuid().ToString(), 33);

            var validationResult = new Validator().Validate(value);

            var resultError = validationResult.ToResultError();

            var resultErrorStructure = resultError.GetErrorStructure();
            resultErrorStructure.Message.Should().Be("Validation failure");
            resultErrorStructure.Children.Should().BeEmpty();
        }

        [Fact]
        public void failure_name()
        {
            var value = new ObjectToValidate(null, 33);

            var validationResult = new Validator().Validate(value);

            var resultError = validationResult.ToResultError();

            var resultErrorStructure = resultError.GetErrorStructure();
            resultErrorStructure.Message.Should().Be("Validation failure");
            resultErrorStructure.Children.Count().Should().Be(1);
            resultErrorStructure.Children.Single().Message.Should().Be("Validation failure for 'Name' with error : 'Name must not be null'");
        }

        [Fact]
        public void failure_age()
        {
            var value = new ObjectToValidate(Guid.NewGuid().ToString(), 2);

            var validationResult = new Validator().Validate(value);

            var resultError = validationResult.ToResultError();

            var resultErrorStructure = resultError.GetErrorStructure();
            resultErrorStructure.Message.Should().Be("Validation failure");
            resultErrorStructure.Children.Count().Should().Be(1);
            resultErrorStructure.Children.Single().Message.Should().Be("Validation failure for 'Age' with error : 'Age must be at least 18'");
        }

        [Fact]
        public void failure_name_and_age()
        {
            var value = new ObjectToValidate(null, 2);

            var validationResult = new Validator().Validate(value);

            var resultError = validationResult.ToResultError();

            var resultErrorStructure = resultError.GetErrorStructure();
            resultErrorStructure.Message.Should().Be("Validation failure");
            resultErrorStructure.Children.Count().Should().Be(2);
            resultErrorStructure.Children.Take(1).Single().Message.Should().Be("Validation failure for 'Name' with error : 'Name must not be null'");
            resultErrorStructure.Children.Skip(1).Single().Message.Should().Be("Validation failure for 'Age' with error : 'Age must be at least 18'");
        }
    }

    public class GetErrorString
    {
        [Fact]
        public void success()
        {
            var value = new ObjectToValidate(Guid.NewGuid().ToString(), 33);

            var validationResult = new Validator().Validate(value);

            var resultError = new FluentValidationResultError(validationResult);

            resultError.GetErrorString().Should().BeEmpty();
        }

        [Fact]
        public void failure_name()
        {
            var value = new ObjectToValidate(null, 33);

            var validationResult = new Validator().Validate(value);

            var resultError = new FluentValidationResultError(validationResult);

            var errorString = resultError.GetErrorString();
            errorString.Should().Be("Validation failure for 'Name' with error : 'Name must not be null'");
        }

        [Fact]
        public void failure_age()
        {
            var value = new ObjectToValidate(Guid.NewGuid().ToString(), 2);

            var validationResult = new Validator().Validate(value);

            var resultError = new FluentValidationResultError(validationResult);

            var errorString = resultError.GetErrorString();
            errorString.Should().Be("Validation failure for 'Age' with error : 'Age must be at least 18'");
        }

        [Fact]
        public void failure_name_and_age()
        {
            var value = new ObjectToValidate(null, 2);

            var validationResult = new Validator().Validate(value);

            var resultError = new FluentValidationResultError(validationResult);

            var errorString = resultError.GetErrorString();
            errorString.Should().Be("Validation failure for 'Name' with error : 'Name must not be null'," +
                                    "Validation failure for 'Age' with error : 'Age must be at least 18'");
        }
    }

    public class Validator : AbstractValidator<ObjectToValidate>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage($"{nameof(ObjectToValidate.Name)} must not be null");
            RuleFor(x => x.Age).GreaterThan(18).WithMessage($"{nameof(ObjectToValidate.Age)} must be at least 18");
        }
    }

    public record ObjectToValidate(string Name, int Age);
}