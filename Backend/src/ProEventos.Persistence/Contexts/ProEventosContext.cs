using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Domain.Identity;

namespace ProEventos.Persistence.Contexts
{
    public class ProEventosContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ProEventosContext(DbContextOptions<ProEventosContext> options)
            : base(options) { }
        public DbSet<Event> Events { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<SpeakerEvent> SpeakersEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.User)
                    .WithMany(us => us.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                userRole.HasOne(ur => ur.Role)
                    .WithMany(rl => rl.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            modelBuilder.Entity<SpeakerEvent>()
                .HasKey(pe => new { pe.EventId, pe.SpeakerId });

            modelBuilder.Entity<Event>()
                .HasMany(ev => ev.SocialNetworks)
                .WithOne(sn => sn.Event)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Speaker>()
                .HasMany(ev => ev.SocialNetworks)
                .WithOne(sn => sn.Speaker)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
