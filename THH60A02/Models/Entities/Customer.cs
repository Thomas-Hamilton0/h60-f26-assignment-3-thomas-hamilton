using System.ComponentModel.DataAnnotations;

namespace TH_H60_A02.Models.Entities;

public class Customer
{
    public int CustomerId { get; set; }

    [Required]
    [StringLength(50)]
    [RegularExpression(@"^[A-Za-z' -]+$", ErrorMessage = "First name may only contain letters, apostrophes, dashes and spaces.")]
    public string FirstName { get; set; } = "";

    [Required]
    [StringLength(50)]
    [RegularExpression(@"^[A-Za-z' -]+$", ErrorMessage = "Last name may only contain letters, apostrophes, dashes and spaces.")]
    public string LastName { get; set; } = "";

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string EmailAddress { get; set; } = "";

    [StringLength(450)]
    public string? IdentityUserId { get; set; }

    [Required]
    [StringLength(20)]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
    public string PhoneNumber { get; set; } = "";

    [Required]
    [StringLength(20)]
    [RegularExpression(@"^(Ontario|Quebec|New Brunswick|Manitoba)$", ErrorMessage = "Province must be Ontario, Quebec, New Brunswick or Manitoba.")]
    public string Province { get; set; } = "";

    public virtual ShoppingCart? ShoppingCart { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}