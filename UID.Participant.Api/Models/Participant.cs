using System;
using System.Collections.Generic;
using UID.Participant.Api.Validation;

namespace UID.Participant.Api.Models;

public partial class Participant
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool Enabled { get; set; }

    public bool Visible { get; set; }

    [ValidateClientTypes]
    public virtual ICollection<ClientType> ClientTypes { get; set; } = new List<ClientType>();
}
