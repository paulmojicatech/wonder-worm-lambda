using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pmt_auth.Models
{
  [Index(nameof(Email), IsUnique = true)]
  public class User
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }    
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string DateOfBirth { get; set; } = string.Empty; 
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    [Required]
    public UserStatus Status { get; set; } = UserStatus.PENDING;
    public ICollection<Children> Children { get; set; }
    public string RegistrationCode {get;set;}
  }
}

public enum UserStatus {
  PENDING = 0,
  ACTIVE
}