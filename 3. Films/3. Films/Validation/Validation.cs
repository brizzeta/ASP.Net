using System.ComponentModel.DataAnnotations;

namespace _3._Films.Validation
{
    public class DirectorValidationAttribute : ValidationAttribute
    {
        private readonly string[] _directors;

        public DirectorValidationAttribute(params string[] directors)
        {
            _directors = directors;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !_directors.Contains(value.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Этот режиссер запрещен!");
        }
    }
}
