using Microsoft.EntityFrameworkCore;
using ProAgil.api.Models;

namespace ProAgil.api.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> Options): base(Options) {}

        public DbSet<Evento> Eventos{get; set;}
    
    }


    
}