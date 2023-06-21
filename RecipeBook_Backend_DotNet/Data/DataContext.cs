namespace RecipeBook_Backend_DotNet.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=recipebook_backend_dotnet;Trusted_Connection=true;TrustServerCertificate=true;");
        }

        public DbSet<Recipe> Recipes { get; set; }
    }
}
