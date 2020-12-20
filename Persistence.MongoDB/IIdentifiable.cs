using System;

namespace Persistence.MongoDB
{
    public interface IIdentifiable<out T>
    {
        T Id { get; }
        DateTime Timestamp { get; }
    }
}