
using SimplyCleanArchitecture.Domain.Shared;

namespace SimplyCleanArchitecture.Application.Common.Models.Cache;

public class CacheModel
{
    List<KeyValueGeneric<string, string>> CacheData { get; set; }
}
