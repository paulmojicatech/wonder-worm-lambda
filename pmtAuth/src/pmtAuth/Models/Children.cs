using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pmt_auth.Models
{  
  public class Children
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    
    public Guid Id { get; set; }    
    [Required]
    public string Name { get; set; } = string.Empty;    
    [Required]
    public string DateOfBirth { get; set; } = string.Empty;
  }
}