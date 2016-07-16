using System.Collections.Generic;

namespace Validator
{
    public interface IValidator
    {
        IEnumerable<ValidationResult> Validate(object model);
    }

    public interface IValidator<T> : IValidator where T : class
    {
        IEnumerable<ValidationResult> Validate(T model);

        Validation<T> AddValidation(Validation<T> validationRule);
    }
}