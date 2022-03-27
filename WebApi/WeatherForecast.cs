namespace WebApi
{
    /// <summary>
    /// 天气类
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 摄氏温度
        /// </summary>
        public int TemperatureC { get; set; }
        /// <summary>
        /// 华氏温度
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        /// <summary>
        /// 天气总结
        /// </summary>
        public string? Summary { get; set; }
    }
}