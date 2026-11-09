using System.Text.Json;

namespace TH_H60_A02.Services
{
    internal static class ApiResponse
    {
        public static async Task EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var message = await ReadErrorAsync(response);

            throw new InvalidOperationException(message);
        }

        private static async Task<string> ReadErrorAsync(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(body))
            {
                return $"The service returned status {(int)response.StatusCode}.";
            }

            try
            {
                using var document = JsonDocument.Parse(body);

                var messages = new List<string>();

                foreach (var property in document.RootElement.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.Array)
                    {
                        messages.AddRange(
                            property.Value
                                .EnumerateArray()
                                .Select(value => value.GetString())
                                .Where(value => !string.IsNullOrWhiteSpace(value))!);
                    }
                    else if (property.Value.ValueKind == JsonValueKind.String)
                    {
                        messages.Add(property.Value.GetString()!);
                    }
                }

                if (messages.Count > 0)
                {
                    return string.Join(" ", messages);
                }
            }
            catch (JsonException)
            {
                // Not a JSON error payload; fall back to the raw body.
            }

            return body;
        }
    }
}
