using System;
using StackExchange.Redis;

namespace SubRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            var channelName = "reservation-request-1";
            var stocksCount = 100;
            var requestId = 1;
            Console.WriteLine("Subscriber is active");

            // get a database client
            var database = ConnectionMultiplexerSingleton.Connection.GetDatabase();


            // get a subscriber client, which is a different client from database.
            var subscriber = ConnectionMultiplexerSingleton.Connection.GetSubscriber();

            // subscribe a channel
            subscriber.Subscribe(channelName, (channel, message) =>
            {
                var result = database.StringIncrement($"request-{requestId}");

                var entity = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestEntity>(message);

                if (stocksCount == result)
                {
                    System.Console.WriteLine("******bitti*******");
                }
                System.Console.WriteLine($"Code:{entity.Code} + increment:{result}");
            });

            Console.ReadLine();
        }
    }
}
