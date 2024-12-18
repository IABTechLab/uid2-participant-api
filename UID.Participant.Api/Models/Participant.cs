﻿using System;
using System.Collections.Generic;

namespace UID.Participant.Api.Models;

public partial class Participant
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool Enabled { get; set; }

    public bool Visible { get; set; }

    public virtual ICollection<ClientType> ClientTypes { get; set; } = new List<ClientType>();
}
