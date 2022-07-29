using MQTTnet;
using MQTTnet.Server;
using System.Text;

namespace MQTTBrokerWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        private static int MessageCounter = 0;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            MqttServerOptionsBuilder options = new MqttServerOptionsBuilder()
               .WithDefaultEndpoint()
               .WithDefaultEndpointPort(1883)
               .WithConnectionValidator(OnNewConnection)
               .WithApplicationMessageInterceptor(OnNewMessage);

            Console.WriteLine("options");

            IMqttServer mqttServer = new MqttFactory().CreateMqttServer();

            Console.WriteLine("server");

            mqttServer.StartAsync(options.Build()).GetAwaiter().GetResult();

            Console.WriteLine("start");

            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        protected void OnNewConnection(MqttConnectionValidatorContext context)
        {
            _logger.LogInformation(
                    "New connection: ClientId = {clientId}, Endpoint = {endpoint}, CleanSession = {cleanSession}",
                    context.ClientId,
                    context.Endpoint,
                    context.CleanSession);
        }

        protected  void OnNewMessage(MqttApplicationMessageInterceptorContext context)
        {
            var payload = context.ApplicationMessage?.Payload == null ? null : Encoding.UTF8.GetString(context.ApplicationMessage.Payload);

            MessageCounter++;

            _logger.LogInformation(
                "MessageId: {MessageCounter} - TimeStamp: {TimeStamp} -- Message: ClientId = {clientId}, Topic = {topic}, Payload = {payload}, QoS = {qos}, Retain-Flag = {retainFlag}",
                MessageCounter,
                DateTime.Now,
                context.ClientId,
                context.ApplicationMessage?.Topic,
                payload,
                context.ApplicationMessage?.QualityOfServiceLevel,
                context.ApplicationMessage?.Retain);
        }
    }
}