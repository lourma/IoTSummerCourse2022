//namespace BlazorMQTT.Data
//{
//    public class MQTTService
//    {
//        private static readonly string[] SummariesM = new[]
//        {
//        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//    };

//        public Task<MQTTdata[]> GetForecastAsync(DateTime startDate)
//        {
//            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new MQTTdata
//            {
//                Date = startDate.AddDays(index),
//                Roll = Random.Shared.Next(-20, 55),
//                Sensor = SummariesM[Random.Shared.Next(SummariesM.Length)]
//            }).ToArray());
//        }
//    }
//}