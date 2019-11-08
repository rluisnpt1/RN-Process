using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RN_Process.Shared.Commun
{
    public class DefaultValidatorStrategy<T> : IValidatorStrategy<T>
    {
        public bool IsValid(T validateThis)
        {
            var results = Validate(validateThis);

            return results.Count == 0;
        }

        private IList<ValidationResult> Validate(T model)
        {
            var results = new List<ValidationResult>();

            var context = new ValidationContext(model);

            Validator.TryValidateObject(model, context, results, true);

            return results;
        }
    }
}