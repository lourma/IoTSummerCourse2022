// See https://aka.ms/new-console-template for more information



using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;

Console.WriteLine("Hello, World!");



// Setup and start a managed MQTT client.
var options = new ManagedMqttClientOptionsBuilder()
    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
    .WithClientOptions(new MqttClientOptionsBuilder()
        .WithClientId("Malo")
        .WithTcpServer("´localhost", 1883)
        .WithTls().Build())
    .Build();

var mqttClient = new MqttFactory().CreateManagedMqttClient();

Console.WriteLine("mqttClient");
MqttTopicFilter? c =  new MqttTopicFilterBuilder().WithTopic("topic_data").Build();
var cc = new List<MqttTopicFilter>();
cc.Add(c);
await mqttClient.SubscribeAsync(cc);
await mqttClient.StartAsync(options);

// StartAsync returns immediately, as it starts a new thread using Task.Run, 
// and so the calling thread needs to wait.
Console.ReadLine();


// Create TCP based options using the builder.
//var options = new MqttClientOptionsBuilder()
//    .WithClientId("WebManager")
//    .WithTcpServer("localhost", 1883)
//    .WithCredentials("bud", "%spencer%")
//    .WithTls()
//    .WithCleanSession()
//    .Build();

//var  _mqttClient = new MqttFactory().CreateMqttClient();

//await _mqttClient.ConnectAsync(options, CancellationToken.None);

//_mqttClient.UseConnectedHandler(async e =>
//{
//    Console.WriteLine("### CONNECTED WITH SERVER ###");

//    // Subscribe to a topic
//    await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("my/topic").Build());

//    Console.WriteLine("### SUBSCRIBED ###");
//});


//_mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
//_mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
//_mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);

//_mqttClient = new MqttApplicationMessageReceivedEventArgs(a =>
//{
//    if (a.ApplicationMessage.Topic == "topic_data")
//    {
//        var payload = Encoding.UTF8.GetString(a.ApplicationMessage.Payload);


//        if (int.TryParse(payload.Split(".")[0], out int vinkel))
//        {
//            Console.WriteLine("Vinkel " + vinkel);
//            if (vinkel < 0)
//                _mqttClient.PublishAsync("topic_command", "green");
//            else
//                _mqttClient.PublishAsync("topic_command", "red");
//        }
//        else
//            _mqttClient.PublishAsync("topic_command", "off");
//    }
//});

//string[] strTopics = { "topic_data" };

//MqttClientSubscribeOptions objSubOptions = new MqttClientSubscribeOptions();
//List<MqttTopicFilter> objTopics = new List<MqttTopicFilter>();

//foreach (string strTopic in strTopics)
//{
//    MqttTopicFilter objAdd = new MqttTopicFilter();
//    objAdd.Topic = strTopic;
//    objTopics.Add(objAdd);
//}

//_mqttClient.StartAsync(options).GetAwaiter().GetResult();
//_mqttClient.SubscribeAsync(objTopics);


//_mqttClient.PublishAsync("topic_command", "yellow");
//Console.WriteLine($"+ Sent topic_command: yellow");

//while (true)
//{
//    //string json = JsonConvert.SerializeObject(new { message = "Heyo :)", sent= DateTimeOffset.UtcNow });
//    //_mqttClient.PublishAsync("from_py", json);
//    ////dev.to / topic / json
//    //Console.WriteLine($"+ Sent message: {json}");
//    Task.Delay(1000).GetAwaiter().GetResult();
//}
//
//      }

//public static void OnConnected(MqttClientConnectedEventArgs obj)
//{
//    Log.Logger.Information("Successfully connected.");
//}

//public static void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
//{
//    Log.Logger.Warning("Couldn't connect to broker.");
//}

//public static void OnDisconnected(MqttClientDisconnectedEventArgs obj)
//{
//    Log.Logger.Information("Successfully disconnected.");
//}

//private void HandleMessageReceived(MqttApplicationMessage applicationMessage)
//{
//    Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
//    Console.WriteLine($"+ Topic = {applicationMessage.Topic}");

//    Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(applicationMessage.Payload)}");
//    Console.WriteLine($"+ QoS = {applicationMessage.QualityOfServiceLevel}");
//    Console.WriteLine($"+ Retain = {applicationMessage.Retain}");
//    Console.WriteLine();
//}