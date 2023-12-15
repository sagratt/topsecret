using Microsoft.EntityFrameworkCore;

namespace HtmlToPdfService.ConversionApi.Data.AppDatabase.Context;

using File = HtmlToPdfService.ConversionApi.Data.AppDatabase.Entities.File;

public class ApplicationDatabaseContext : DbContext
{
    public DbSet<File> Files => Set<File>();
    
    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
    }
    
    public Task<File?> GetFileById(Guid id)
    {
        return Files.Where(e => e.Id == id).FirstOrDefaultAsync();
    }
}