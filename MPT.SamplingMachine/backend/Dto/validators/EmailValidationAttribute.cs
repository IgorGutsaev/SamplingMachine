using Filuet.Infrastructure.Abstractions.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MPT.Vending.API.Dto
{
    public class Email : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            => value?.ToString().IsEmail() ?? false ? ValidationResult.Success : new ValidationResult("Invalid email");
    }
}
