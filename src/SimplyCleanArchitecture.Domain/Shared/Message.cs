namespace SimplyCleanArchitecture.Domain.Shared;
public class Message
{
    public Message(string messageStatus, string messageSource, string messageReason, string messageStatusCode)
        : this(messageStatus)
    {
        MessageSource = messageSource;
        MessageReason = messageReason;
        MessageStatusCode = messageStatusCode;
    }
    public Message(string messageStatus)
    {
        MessageStatus = messageStatus;
    }

    public string MessageStatus { get; set; }
    public string MessageSource { get; set; } = string.Empty;
    public string MessageReason { get; set; } = string.Empty;
    public string MessageStatusCode { get; set; } = string.Empty;
}