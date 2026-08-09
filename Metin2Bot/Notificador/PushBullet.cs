using System.Text;
using System.Text.RegularExpressions;

namespace Metin2Bot.Notificador
{
    public static class PushBullet
    {
        static async Task GetDevices()
        {
            try
            {
                string apiKey = "o.LdXwhAktIZpJaRNxVb12C84tUNoYUw6Z";  // Reemplaza con tu API Key

                string url = "https://api.pushbullet.com/v2/devices";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Access-Token", apiKey);

                    var response = await client.GetAsync(url);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Dispositivos disponibles:");
                        Console.WriteLine(responseContent);
                    }
                    else
                    {
                        Console.WriteLine("Error al obtener dispositivos: " + responseContent);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR al consultar devices: {ex.Message}");
            }
        }

        public static async Task EnviarNotificacion(string texto)
        {
            try
            {
                // API Key de Pushbullet (obténla de la configuración en tu cuenta de Pushbullet)
                string apiKey = "o.LdXwhAktIZpJaRNxVb12C84tUNoYUw6Z";  // Reemplaza con tu API Key

                // ID del dispositivo al que quieres enviar la notificación
                string deviceId = "ujEQbfXKzy8sjuTXIMLhL2";  // Reemplaza con tu Device ID

                // URL de la API de Pushbullet para enviar mensajes
                string url = "https://api.pushbullet.com/v2/pushes";

                // Crea el cliente HTTP
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Access-Token", apiKey);  // Agrega tu API Key a los headers

                    // Cuerpo del mensaje
                    var content = new StringContent(
                        $"{{\"type\": \"note\", \"title\": \"🚨 ALERTA 🚨 Metin2 Rubinum\", \"body\": \"{Regex.Replace(new string(texto), @"\s+", "")}\", \"device_iden\": \"{deviceId}\", \"priority\": \"high\"}}",
                        Encoding.UTF8,
                        "application/json"
                    );

                    // Enviar la solicitud POST
                    var response = await client.PostAsync(url, content);

                    // Leer la respuesta
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Notificación enviada exitosamente");
                    }
                    else
                    {
                        Console.WriteLine("Error al enviar la notificación: " + responseContent);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR al enviar Notificación: {ex.Message}");
            }
        }
    }
}
