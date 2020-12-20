using Persistence.MongoDB;
using System;

namespace Subscriber.Models.Domain
{
    public class TextMessage: IIdentifiable<Guid>
    {
        public Guid Id { get; private set; }
        public string Message { get; private set; }
        public string DeviceToken { get; set; }
        public DateTime Timestamp { get; private set; }
        public TextMessage(Guid id, string message, DateTime timestamp,string deviceToken)
        {
            Id = id;
            Message = message;
            Timestamp = timestamp;
            DeviceToken = deviceToken;
        }

    }
}
