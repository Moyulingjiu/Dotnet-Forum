using System;

// 只是官方生成的时候就给的一个示例，可以看一下他们怎么做的
namespace CampusForum
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
