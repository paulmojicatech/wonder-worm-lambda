using Microsoft.EntityFrameworkCore;

namespace pmt_auth.Context
{
  public class PmtAuthContext : DbContext
  {
    public PmtAuthContext(DbContextOptions<PmtAuthContext> options) : base(options)
    {
    }

  }  
}