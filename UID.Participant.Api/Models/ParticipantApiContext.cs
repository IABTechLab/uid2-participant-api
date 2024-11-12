using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace uid2_participant_api;

public partial class ParticipantApiContext : DbContext
{
    public ParticipantApiContext()
    {
    }

    public ParticipantApiContext(DbContextOptions<ParticipantApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClientType> ClientTypes { get; set; }

    public virtual DbSet<Site> Sites { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer(Configuration.GetConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientType>(entity =>
        {
            entity.ToTable("ClientTypes", "ParticipantApi");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Site>(entity =>
        {
            entity.ToTable("Sites", "ParticipantApi");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasMany(d => d.ClientTypes).WithMany(p => p.Sites)
                .UsingEntity<Dictionary<string, object>>(
                    "SiteClientType",
                    r => r.HasOne<ClientType>().WithMany()
                        .HasForeignKey("ClientTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_SiteClientType_ClientTypes"),
                    l => l.HasOne<Site>().WithMany()
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_SiteClientType_Sites"),
                    j =>
                    {
                        j.HasKey("SiteId", "ClientTypeId");
                        j.ToTable("SiteClientType", "ParticipantApi");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
