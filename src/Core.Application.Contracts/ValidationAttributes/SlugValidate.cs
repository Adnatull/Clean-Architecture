using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Application.Contracts.ValidationAttributes
{
    public class SlugValidate : ValidationAttribute
    {

        public SlugValidate() : base("{0} has one or many space character.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("This field is required");
            }
            var txt = value.ToString();
            if (txt != null && txt.Contains(' '))
            {
                var errorMessage = FormatErrorMessage((validationContext.DisplayName));
                return new ValidationResult(errorMessage);
            }

            if (txt != null && txt.Count(c => char.IsLetterOrDigit(c) || (c == ',') || (c == '.') || (c == '-') || (c == '_') || (c == '=')) == txt.Length)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid slug. please enter only letter, numbers and hiphen(-)");
        }
    }
}
