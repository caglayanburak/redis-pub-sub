using System;
using System.Threading.Tasks;

namespace PubToRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            var channelName = "reservation-request-1";
            var stocksCount = 100;
            var requestId = 1;
            Console.WriteLine("Publisher is active");

            var database = ConnectionMultiplexerSingleton.Connection.GetDatabase();
            var publisher = ConnectionMultiplexerSingleton.Connection.GetSubscriber();

            //start when partition splitted
            database.StringSet($"request-{requestId}", 0);

            // publish message to one channel
            Parallel.For(0, stocksCount,
                  index =>
                  {
                      var entity = new RequestEntity()
                      {
                          Id = index,
                          Code = "Code" + index
                      };
                      System.Console.WriteLine($"Code:{entity.Code}");
                      publisher.Publish(channelName, Newtonsoft.Json.JsonConvert.SerializeObject(entity));
                  });

            Console.ReadLine();
        }
    }
}
