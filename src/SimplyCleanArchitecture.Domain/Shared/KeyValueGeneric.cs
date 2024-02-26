namespace SimplyCleanArchitecture.Domain.Shared;

public class KeyValueGeneric<TKey, TValue>
{
    public TKey Id { get; set; }

    public TValue Value { get; set; }
}
