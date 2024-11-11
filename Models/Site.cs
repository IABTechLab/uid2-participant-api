using System;
using System.Collections.Generic;

namespace uid2_participant_api;

public partial class Site
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool Enabled { get; set; }

    public bool Visible { get; set; }

    public virtual ICollection<ClientType> ClientTypes { get; set; } = new List<ClientType>();
}
