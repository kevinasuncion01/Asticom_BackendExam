using Asticom_BackendExam.Models.DbModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Asticom_BackendExam.Context
{
    public class AsticomContext : IdentityDbContext<AdminInfo>
    {
        public AsticomContext()
        {
            
        }
       public AsticomContext(DbContextOptions<AsticomContext> options) : base(options)
       {
       }

        public virtual DbSet<UserInfo> UserInfo { get; set; } = null!;
    }
}
