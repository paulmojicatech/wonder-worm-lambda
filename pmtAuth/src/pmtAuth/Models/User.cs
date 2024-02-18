using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace pmt_auth.Models
{
  [Index(nameof(Email), IsUnique = true)]
  public class User
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }    
    public required string Name { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public required string DateOfBirth { get; set; } = string.Empty; 
    public required string PasswordHash { get; set; } = string.Empty;
    public ICollection<Children> Children { get; set; }
  }
}