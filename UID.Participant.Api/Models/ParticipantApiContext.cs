﻿using System;
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

    // The = null!; is to tell the compiler that although it looks like ClientTypes is null, don't worry about it.
    // It is set by EF Core via reflection
    public virtual DbSet<ClientType> ClientTypes { get; set; } = null!;

    public virtual DbSet<Participant> Participants { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientType>(entity =>
        {
            entity.ToTable("ClientTypes", "ParticipantApi");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Participant>(entity =>
        {
            entity.ToTable("Participants", "ParticipantApi");

            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasMany(d => d.ClientTypes).WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ParticipantClientType",
                    r => r.HasOne<ClientType>().WithMany()
                        .HasForeignKey("ClientTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ParticipantClientType_ClientTypes"),
                    l => l.HasOne<Participant>().WithMany()
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ParticipantClientType_Participants"),
                    j =>
                    {
                        j.HasKey("ParticipantId", "ClientTypeId");
                        j.ToTable("ParticipantClientType", "ParticipantApi");
                    });

            entity.Navigation(e => e.ClientTypes).AutoInclude();
        });

        this.OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
