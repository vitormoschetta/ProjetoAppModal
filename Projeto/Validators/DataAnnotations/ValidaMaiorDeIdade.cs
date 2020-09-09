using System;
using System.ComponentModel.DataAnnotations;

namespace Projeto.Validators.DataAnnotations
{
    public class ValidaMaiorDeIdade : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Preenchimento obrigatório");

            DateTime date = (DateTime)value;

            if ((DateTime.Now.Year - date.Year) < 18)
                return new ValidationResult("Somente maiores de 18 anos");

            return ValidationResult.Success;
        }
    }
}