// Infra/DataContext.cs
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infra
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        // DbSets principais
        public DbSet<User> Users { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Event> Events { get; set; }

        // DbSets de interação
        public DbSet<EventRegistration> EventRegistrations { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        // VVV ADICIONE ESTE MÉTODO AQUI VVV
        // Infra/DataContext.cs

        // Infra/DataContext.cs
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- CHAVES PRIMÁRIAS COMPOSTAS ---
            builder.Entity<Follow>()
                .HasKey(k => new { k.FollowerId, k.FollowingId });

            // --- RELAÇÕES E REGRAS DE EXCLUSÃO (ON DELETE) ---

            // Relação Follow
            builder.Entity<Follow>()
                .HasOne(f => f.Follower).WithMany(u => u.Following).HasForeignKey(f => f.FollowerId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Follow>()
                .HasOne(f => f.Following).WithMany(u => u.Followers).HasForeignKey(f => f.FollowingId).OnDelete(DeleteBehavior.NoAction);

            // Relação Event/Registration
            builder.Entity<Event>()
                .HasOne(e => e.Organizer).WithMany(u => u.OrganizedEvents).HasForeignKey(e => e.OrganizerId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<EventRegistration>()
                .HasOne(er => er.Event).WithMany(e => e.Registrations).HasForeignKey(er => er.EventId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<EventRegistration>()
                .HasOne(er => er.User).WithMany(u => u.EventRegistrations).HasForeignKey(er => er.UserId).OnDelete(DeleteBehavior.NoAction);

            // VVV ADICIONE AS CONFIGURAÇÕES PARA LIKES, BOOKMARKS E COMMENTS VVV

            // Relação Post -> Interações (Cascade é OK)
            builder.Entity<Like>().HasOne(l => l.Post).WithMany(p => p.Likes).HasForeignKey(l => l.PostId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Bookmark>().HasOne(b => b.Post).WithMany(p => p.Bookmarks).HasForeignKey(b => b.PostId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Comment>().HasOne(c => c.Post).WithMany(p => p.Comments).HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.Cascade);

            // Relação User -> Interações (NoAction para quebrar o ciclo)
            builder.Entity<Like>().HasOne(l => l.User).WithMany(u => u.Likes).HasForeignKey(l => l.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Bookmark>().HasOne(b => b.User).WithMany(u => u.Bookmarks).HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Comment>().HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}