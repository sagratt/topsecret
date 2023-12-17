using Microsoft.EntityFrameworkCore;
using File = HtmlToPdf.ConversionApi.Data.AppDatabase.Entities.File;

namespace HtmlToPdf.ConversionApi.Data.AppDatabase.Context;

using File = File;

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