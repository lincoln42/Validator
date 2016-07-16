using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator
{
    public class Validator<T> : IValidator<T> where T : class
    {
        private readonly List<ValidationResult> _validationResults;
        private readonly List<Validation<T>> _validations;
        private readonly List<ValidatorValidation<T>> _validatorValidations;

        public Validator()
        {
            _validations = new List<Validation<T>>();
            _validatorValidations = new List<ValidatorValidation<T>>();
            _validationResults = new List<ValidationResult>();
        }

        public IEnumerable<ValidationResult> Validate(object model)
        {
            var x = model as T;
            return Validate(x);
        }

        public Validation<T> AddValidation(Validation<T> validationRule)
        {
            _validations.Add(validationRule);
            return validationRule;
        }

        public ValidatorValidation<T> AddValidatorValidation(ValidatorValidation<T> valValidationRule)
        {
            _validatorValidations.Add(valValidationRule);
            return valValidationRule;
        }

        public virtual IEnumerable<ValidationResult> Validate(T model)
        {

            if(model == null)
            {
                return new List<ValidationResult>
                {
                   new ValidationResult
                   {
                       PropertyName = "Model",
                       ErrorMessage = string.Format("Model of type {0} is null", typeof(T).ToString())
                   }
                };
            }

            foreach (var validation in _validations)
            {
                validation.OnValidating();
                var validater = validation.GetValidater();

                if (!validater(model))
                    _validationResults.Add(validation.GetValidationResult());
            }

            foreach(var valValidation in _validatorValidations)
            {
                valValidation.ParentModelInstance = model;

                var valRes = valValidation.Validate();

                _validationResults.AddRange(valRes);
            }

            return _validationResults;
        }
    }
}
