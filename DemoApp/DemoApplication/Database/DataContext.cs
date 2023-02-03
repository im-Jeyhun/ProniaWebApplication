using DemoApplication.Database.Configurations;
using DemoApplication.Database.Models;
using DemoApplication.Database.Models.Common;
using DemoApplication.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DemoApplication.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<PlantTag> PlantTags { get; set; }
        public DbSet<BlogBTag> BlogTags { get; set; }
        public DbSet<BTag> BTags { get; set; }
        public DbSet<PlantColor> PlantColors { get; set; }
        public DbSet<PlantSize> PlantSizes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Navbar> Navbars { get; set; }
        public DbSet<SubNavbar> SubNavbars { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketProduct> BasketProducts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<BlogImage>? BlogImages { get; set; }
        public DbSet<BlogVideo>? BlogVideos { get; set; }
        public DbSet<UserActivation> UserActivations { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<PaymentBenefit> PaymentBenefits { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly<Program>();
        }

        public override int SaveChanges()
        {
            AutoAudit();


            return base.SaveChanges(); //bu hemise olmalidirki base save changes islerini gorsun..
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AutoAudit();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AutoAudit();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AutoAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AutoAudit()
        {
            foreach (var entity in ChangeTracker.Entries())
            {
                if (entity.Entity is not IAuditable auditable) // burada is not un diger bir ozelliyinden istifade edirik
                                                        //yeni entity IAuditabli implement eleyibse ve casting ede bilirse onu IAuditabledan casting edir
                {
                    continue;
                }
                /* var auditable = (IAuditable)entity;*/ // casting for acces IAuditable properties on entity

                DateTime currentTime = DateTime.Now; //for same time 

                if (entity.State == EntityState.Added) // for checking entity's state added
                {
                    auditable.CreatedAt = currentTime;
                    auditable.UpdatedAt = currentTime;
                }
                else if (entity.State == EntityState.Modified) // for checking entity's state modified
                {
                    auditable.UpdatedAt = currentTime;

                }
            }
        }
    }
}
