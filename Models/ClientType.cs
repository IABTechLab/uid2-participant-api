using System;
using System.Collections.Generic;

namespace uid2_participant_api;

public partial class ClientType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Site> Sites { get; set; } = new List<Site>();
}
