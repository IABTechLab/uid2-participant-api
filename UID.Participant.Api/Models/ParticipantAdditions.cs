using Microsoft.AspNetCore.Mvc;
using UID.Participant.Api.Validation;

namespace UID.Participant.Api.Models;

/// <summary>
/// This class provides additional metadata to the generated <see cref="Participant"/> class. Specifically it adds a validation attribute
/// </summary>
public class ParticipantAdditions
{
    [ValidateClientTypes]
    public virtual ICollection<ClientType> ClientTypes { get; set; } = [];
}

[ModelMetadataType(typeof(ParticipantAdditions))]
public partial class Participant
{
}
