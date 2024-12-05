namespace UID.Participant.Api.Models
{
    public partial class ClientType
    {
        public override bool Equals(object? obj)
        {
            return obj is ClientType type &&
                   this.Id == type.Id &&
                   this.Name == type.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.Name);
        }

        public override string ToString()
        {
            return $"Id: {this.Id}, Name: {this.Name}";
        }
    }
}
