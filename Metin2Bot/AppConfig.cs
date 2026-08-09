using System.Text.Json;

namespace Metin2Bot
{
    public static class AppConfig
    {
        private static readonly string configFilePath = "config/appsettings.json";
        private static readonly JsonDocument jsonConfig;

        static AppConfig()
        {
            if (File.Exists(configFilePath))
            {
                string jsonString = File.ReadAllText(configFilePath);
                jsonConfig = JsonDocument.Parse(jsonString);
            }
            else
            {
                throw new FileNotFoundException($"No se encontró el archivo de configuración: {configFilePath}");
            }
        }

        public static string GetRouteValue(string key)
        {
            return jsonConfig.RootElement.GetProperty("ROUTES").GetProperty(key).GetString();
        }
    }
}