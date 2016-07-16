using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Validator.Extensions;

namespace Validator
{
    public class ValidatorValidation<T>
    {
        private string _propertyName;
        private Expression<Func<T, object>> _property;

        public T ParentModelInstance { get; set; }

        public Type NestedPropertyType { get; set; }

        public object NestedPropertyValidatorInstance { get; set; }

        public ValidatorValidation<T> SetNestedPropertyAccessor(Expression<Func<T, object>> property)
        {
            _property = property;
            return this;
        }

        public Expression<Func<T, object>> GetNestedPropertyAccessor()
        {
            return _property;
        }

        public string GetNestedPropertyName()
        {
            var memberExpression = GetNestedPropertyAccessor().Body as MemberExpression ?? ((UnaryExpression)GetNestedPropertyAccessor().Body).Operand as MemberExpression;

            if (memberExpression == null)
            {
                _propertyName = default(string);
                return _propertyName;
            }

            _propertyName = memberExpression.Member.Name;

            return _propertyName;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            var res = new List<ValidationResult>();

            if (ParentModelInstance == null)
            {
                return new List<ValidationResult>
                {
                   new ValidationResult
                   {
                       PropertyName = "ParentModelInstance",
                       ErrorMessage = string.Format("ParentModelInstance is null")
                   }
                };
            }

            var modelObj = GetNestedPropertyAccessor().Compile().Invoke(ParentModelInstance);

            if(modelObj == null)
            {
                return new List<ValidationResult>
                {
                   new ValidationResult
                   {
                       PropertyName = "Model",
                       ErrorMessage = string.Format("Model is null")
                   }
                };
            }

            var modelObjType = modelObj.GetType();

            if(!NestedPropertyType.IsAssignableFrom(modelObjType))
            {
                return new List<ValidationResult>
                {
                   new ValidationResult
                   {
                       PropertyName = "Model",
                       ErrorMessage = string.Format("Model is not of type {0}", NestedPropertyType.ToString())
                   }
                };
            }            

            if(NestedPropertyValidatorInstance == null)
            {
                return new List<ValidationResult>
                {
                    new ValidationResult
                    {
                        PropertyName = "ValidatorInstance",
                        ErrorMessage = string.Format("ValidatorInstance is null")
                    }
                };
            }

            var validatorType = NestedPropertyValidatorInstance.GetType();

            Type genValidatorType;
            Type actualNestedModelType;
            var openValidatorType = typeof(IValidator<>);
            
            if (typeof(IEnumerable).IsAssignableFrom(NestedPropertyType))
            {
                var collectionElementType = NestedPropertyType.GetCollectionTypeElementType();
                if(collectionElementType == null)
                {
                   return new List<ValidationResult>
                   {
                     new ValidationResult
                     {
                        PropertyName = "NestedPropertyType",
                        ErrorMessage = string.Format("Unable to get element type of collection type {0}", NestedPropertyType.ToString())
                     }
                   };
                }

                actualNestedModelType = collectionElementType;
                Type[] genTypeArgs = { collectionElementType };
                genValidatorType = openValidatorType.MakeGenericType(genTypeArgs);
            }
            else
            {
                Type[] genTypeArgs = { NestedPropertyType };
                genValidatorType = openValidatorType.MakeGenericType(genTypeArgs);
                actualNestedModelType = NestedPropertyType;
            }

            if(!genValidatorType.IsAssignableFrom(validatorType))
            {
                return new List<ValidationResult>
                {
                    new ValidationResult
                    {
                        PropertyName = "ValidatorInstance",
                        ErrorMessage = string.Format("ValidatorInstance is not of type {0}", genValidatorType.ToString())
                    }
                };
            }

            var validationMethodInfo = validatorType.GetMethod("Validate", new Type[] { actualNestedModelType });

            if (validationMethodInfo == null)
            {
                return new List<ValidationResult>
                {
                    new ValidationResult
                    {
                        PropertyName = "ValidatorInstance",
                        ErrorMessage = string.Format("ValidatorInstance does not have a Validate method")
                    }
                };
            }

            var validationMethodInfoRetType = validationMethodInfo.ReturnType;
            var expectedRetType = typeof(IEnumerable<ValidationResult>);

            if(!validationMethodInfoRetType.IsAssignableFrom(expectedRetType))
            {
                return new List<ValidationResult>
                {
                    new ValidationResult
                    {
                        PropertyName = "ValidatorInstance",
                        ErrorMessage = string.Format("ValidatorInstance does not have a Validate method")
                    }
                };
            }

            //is model a collection?

            if(typeof(IEnumerable).IsAssignableFrom(NestedPropertyType))
            {
                var en = (IEnumerable) modelObj;

                var idx = 0;

                foreach(var i in en)
                {
                    var resObj = validationMethodInfo.Invoke(NestedPropertyValidatorInstance, new object[] { i });                    

                    var valRes = resObj as IEnumerable<ValidationResult>;

                    if (valRes != null && valRes.Any())
                    {

                        var parentPropertyName = GetNestedPropertyName();

                        var rr = valRes.Select(r => new ValidationResult
                        {
                            PropertyName = string.Format("{0}[{1}].{2}", parentPropertyName, idx, r.PropertyName),
                            ErrorMessage = r.ErrorMessage
                        });

                        res.AddRange(rr);
                    }

                    ++idx;
                }
            }
            else
            {
                var resObj = validationMethodInfo.Invoke(NestedPropertyValidatorInstance, new object[] { modelObj });

                var valRes = resObj as IEnumerable<ValidationResult>;

                if (valRes != null)
                {

                    var parentPropertyName = GetNestedPropertyName();

                    var rr = valRes.Select(r => new ValidationResult
                    {
                       PropertyName = string.Format("{0}.{1}", parentPropertyName, r.PropertyName),
                       ErrorMessage = r.ErrorMessage
                    });

                    res.AddRange(valRes);
                }
            }

            return res;
        }
    }
}
