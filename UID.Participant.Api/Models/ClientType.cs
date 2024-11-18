﻿using System;
using System.Collections.Generic;

namespace UID.Participant.Api.Models;

public partial class ClientType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

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
