using System.ComponentModel.DataAnnotations;

namespace Models;

public class NonEmptyBasketAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return null;
        if (((HashSet<(long, string)>)value).Count == 0)
            return new ValidationResult("Husk å velge minst en kake :)");
        return ValidationResult.Success;
    } 
}

public class FormData
{
    [Required(ErrorMessage = "Navn er påkrevd")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Email er påkrevd")]
    [EmailAddress(ErrorMessage = "Ugyldig e-postadresse")]
    public string? Email { get; set; }

    [NonEmptyBasket]
    public HashSet<(long, string)> Basket { get; set; } = new();

    public string? Comments { get; set; }

}