using Microsoft.EntityFrameworkCore;

namespace Connection
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) 
        { 

        }

    }
}
