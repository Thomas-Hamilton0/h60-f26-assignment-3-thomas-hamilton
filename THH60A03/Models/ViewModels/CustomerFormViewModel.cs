using System.ComponentModel.DataAnnotations;

namespace TH_H60_A03.Models.ViewModels
{
    public class CustomerFormViewModel
    {
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z' -]+$", ErrorMessage = "First name may only contain letters, apostrophes, dashes and spaces.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = "";

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z' -]+$", ErrorMessage = "Last name may only contain letters, apostrophes, dashes and spaces.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = "";

        [Required]
        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; } = "";

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = "";

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^(Ontario|Quebec|New Brunswick|Manitoba)$", ErrorMessage = "Province must be Ontario, Quebec, New Brunswick or Manitoba.")]
        public string Province { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }

        public string? IdentityUserId { get; set; }

        public bool HasAccount => !string.IsNullOrWhiteSpace(IdentityUserId);

        public static IReadOnlyList<string> Provinces { get; } =
            new[] { "Ontario", "Quebec", "New Brunswick", "Manitoba" };
    }
}
