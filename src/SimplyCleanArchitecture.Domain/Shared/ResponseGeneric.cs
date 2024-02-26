using MediatR;

namespace SimplyCleanArchitecture.Domain.Shared;

public class Response<T> : IRequest<T>
{
    public Message Message { get; set; }
    public T Body { get; set; }
}

public class ServiceResponse<T> : Response<T>
{
    public string Service { get; set; }
    public int Status { get; set; }
}
