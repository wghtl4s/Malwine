using System.Collections.Generic;

namespace Malwine.Models.Dto;

public class User : LightUser
{
  public required string About { get; init; }
  public required IEnumerable<LightMovie> Liked { get; init; }
}