using System.ComponentModel.DataAnnotations;

namespace ProcessadorTarefasWebAPI.ConfigValidations
{
    public class MinLowerThanMaxAttribute : ValidationAttribute
    {
        private readonly string _minPropertyName;
        private readonly string _maxPropertyName;

        public MinLowerThanMaxAttribute(string minPropertyName, string maxPropertyName)
        {
            _minPropertyName = minPropertyName;
            _maxPropertyName = maxPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var minProperty = validationContext.ObjectType.GetProperty(_minPropertyName);
            var maxProperty = validationContext.ObjectType.GetProperty(_maxPropertyName);

            if (minProperty == null)
                return new ValidationResult($"Propriedade {_minPropertyName} não encontrada.");

            if (maxProperty == null)
                return new ValidationResult($"Propriedade {_maxPropertyName} não encontrada.");

            var minValue = (int?)minProperty.GetValue(validationContext.ObjectInstance);
            var maxValue = (int?)maxProperty.GetValue(validationContext.ObjectInstance);

            if (minValue >= maxValue && minValue.HasValue && maxValue.HasValue)
                return new ValidationResult(ErrorMessage ?? $"O campo {_minPropertyName} deve ser menor que {_maxPropertyName}.");

            return ValidationResult.Success;
        }
    }
}
