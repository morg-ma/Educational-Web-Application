using System.ComponentModel.DataAnnotations;

namespace EducationalWebApplication.ValidationAttributes
{
    public class IsSelectedAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return null;

            int selectedValue = (int)value;
            if (selectedValue > 0)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"Please, select the {validationContext.DisplayName}");
        }
    }
}
