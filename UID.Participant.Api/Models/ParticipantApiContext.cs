using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UID.Participant.Api.Models;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientType>(entity =>
        {
            entity.ToTable("ClientTypes", "ParticipantApi");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Site>(entity =>
        {
            entity.ToTable("Sites", "ParticipantApi");

            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasMany(d => d.ClientTypes).WithMany()
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
