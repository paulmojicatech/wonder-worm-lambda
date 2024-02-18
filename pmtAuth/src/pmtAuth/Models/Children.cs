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
    public required string Name { get; set; } = string.Empty;    
    public required string DateOfBirth { get; set; } = string.Empty;
  }
}