using AutoFixture;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UID.Participant.Api.Models;

namespace UID.Participant.Api.Test.ControllerTests
{
    public abstract class ControllerTestsBase : IDisposable
    {
        protected readonly Fixture Fixture;
        protected readonly ParticipantApiContext WriteParticipantContext;
        protected readonly ParticipantApiContext ReadParticipantContext;
        protected readonly SqliteConnection SqLiteConnection;
        protected readonly List<ClientType> KnownClientTypes;

        protected ControllerTestsBase()
        {
            this.Fixture = new Fixture();
            this.SqLiteConnection = new SqliteConnection("Filename=:memory:;Pooling=false");
            this.SqLiteConnection.Open();
            this.WriteParticipantContext = new ParticipantApiContext(new DbContextOptionsBuilder<ParticipantApiContext>().UseSqlite(this.SqLiteConnection).Options);
            this.WriteParticipantContext.Database.EnsureCreated();
            this.KnownClientTypes = new List<ClientType>(
                [
                new ClientType { Id = 1, Name = "DSP" },
                new ClientType { Id = 2, Name = "PUBLISHER" },
                new ClientType { Id = 3, Name = "DATA_PROVIDER" },
                new ClientType { Id = 4, Name = "ADVERTISER" }
                ]
            );

            this.WriteParticipantContext.ClientTypes.AddRange(this.KnownClientTypes);
            this.WriteParticipantContext.SaveChanges();

            // separate the read and write contexts, so only changes that happen through .SaveChanges are returned in the queries
            this.ReadParticipantContext = new ParticipantApiContext(new DbContextOptionsBuilder<ParticipantApiContext>().UseSqlite(this.SqLiteConnection).Options);
        }

        public void Dispose()
        {
            if (this.SqLiteConnection != null)
            {
                this.SqLiteConnection.Close();
                this.SqLiteConnection.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}