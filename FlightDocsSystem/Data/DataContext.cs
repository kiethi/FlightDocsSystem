using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System;

namespace FlightDocsSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentHistory> DocumentHistories { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<TypeGroup> TypeGroups { get; set; }


        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<User>()
             .HasIndex(e => e.Email)
             .IsUnique();

            modelbuilder.Entity<Group>()
             .HasIndex(e => e.Name)
             .IsUnique();

            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelbuilder.Entity<Document>()
                .HasMany(u => u.DocumentHistories)
                .WithOne(d => d.Document)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelbuilder.Entity<Group>()
                .HasMany(u => u.GroupUsers)
                .WithOne(d => d.Group)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelbuilder.Entity<DocumentType>()
                .HasMany(u => u.TypeGroups)
                .WithOne(d => d.DocumentType)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.Cascade);


            //tao composite key cho GroupUser
            modelbuilder.Entity<GroupUser>(entity =>
            {
                entity.HasKey(groupUser => new { groupUser.GroupId, groupUser.UserId });
                //entity.HasNoKey();
            });

            //tao composite key cho TypeGroup
            modelbuilder.Entity<TypeGroup>(entity =>
            {
                entity.HasKey(typeGroup => new { typeGroup.GroupId, typeGroup.DocumentTypeId });
                //entity.HasNoKey();
            });

            base.OnModelCreating(modelbuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
