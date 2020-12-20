namespace TestApi.MessageBrokers
{
    public interface IMessageBroker
    {
      public  void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey)
       where T : class;
    }
}
