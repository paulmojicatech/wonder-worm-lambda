using Microsoft.EntityFrameworkCore;
using pmt_auth.Models;

namespace pmt_auth.Context
{
  public class PmtAuthContext : DbContext
  {
    public PmtAuthContext(DbContextOptions<PmtAuthContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

  }  
}