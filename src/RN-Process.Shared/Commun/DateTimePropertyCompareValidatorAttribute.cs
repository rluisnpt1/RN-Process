using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RN_Process.Shared.Commun
{
    public class DateTimePropertyCompareValidatorAttribute : ValidationAttribute
    {
        private DateTimeDeltaTypeEnum _CompareType;
        private string _OtherPropertyName;

        public DateTimePropertyCompareValidatorAttribute(DateTimeDeltaTypeEnum compareType, string otherPropertyName)
        {
            _CompareType = compareType;
            _OtherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Value cannot be null.");
            }


            DateTime valueAsDateTime;

            if (value is DateTime)
            {
                valueAsDateTime = (DateTime)value;
            }
            else
            {
                var valueAsString = value.ToString();

                if (String.IsNullOrWhiteSpace(valueAsString) == true)
                {
                    return new ValidationResult("Value cannot be blank.");
                }

                if (DateTime.TryParse(valueAsString, out valueAsDateTime) == false)
                {
                    return new ValidationResult("Value is not a DateTime.");
                }
            }

            object otherValue = null;

            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(_OtherPropertyName);

            if (otherPropertyInfo != null)
            {
                otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);
            }
            else
            {
                return new ValidationResult("Invalid property name for other property.");
            }

            if (otherValue == null)
            {
                return new ValidationResult("Other property value not specified.");
            }

            DateTime otherValueAsDateTime;

            if (otherValue is DateTime)
            {
                otherValueAsDateTime = (DateTime)otherValue;
            }
            else
            {
                if (DateTime.TryParse(otherValue.ToString(), out otherValueAsDateTime) == false)
                {
                    return new ValidationResult("Other value is not a DateTime.");
                }
            }

            if (_CompareType == DateTimeDeltaTypeEnum.GreaterThan)
            {
                return valueAsDateTime == default(DateTime) || valueAsDateTime > otherValueAsDateTime
                    ? ValidationResult.Success : new ValidationResult(
                        $"{validationContext.DisplayName} should be greater than {_OtherPropertyName}.");
            }
            else
            {
                if (otherValueAsDateTime == default(DateTime) ||
                    valueAsDateTime < otherValueAsDateTime)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(
                        $"{validationContext.DisplayName} should be less than {_OtherPropertyName}.");
                }
            }
        }

    }
}
