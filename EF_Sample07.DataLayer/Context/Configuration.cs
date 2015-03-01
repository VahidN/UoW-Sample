using System.Data.Entity.Migrations;

namespace EF_Sample07.DataLayer.Context
{
    public class Configuration : DbMigrationsConfiguration<Sample07Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Sample07Context context)
        {
            base.Seed(context);
        }
    }
}
