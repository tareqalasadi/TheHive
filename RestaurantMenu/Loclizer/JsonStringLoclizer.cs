
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Globalization;

namespace RestaurantMenu.Loclizer
{
    public class JsonStringLoclizer : IStringLocalizer
    {
        private readonly JsonSerializer _serlizer = new JsonSerializer();
        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var actualValue = this[name];
                return !actualValue.ResourceNotFound ? new LocalizedString(name, string.Format(actualValue.Value, arguments)) : actualValue;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var filepath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
            using FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader reader = new StreamReader(stream);
            using JsonTextReader textReader = new JsonTextReader(reader);
            while (textReader.Read())
            {
                if (textReader.TokenType != JsonToken.PropertyName)
                    continue;
                var key = textReader.Value as string;
                textReader.Read();
                var value = _serlizer.Deserialize<string>(textReader);
                yield return new LocalizedString(key, value);

            }
        }

        private string GetValueFromJson(string value, string filepath)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(filepath))
                return string.Empty;
            using FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader reader = new StreamReader(stream);
            using JsonTextReader textReader = new JsonTextReader(reader);
            while (textReader.Read())
            {
                if (textReader.TokenType == JsonToken.PropertyName && textReader.Value as string == value)
                {
                    textReader.Read();
                    return _serlizer.Deserialize<string>(textReader);
                }
            }
            return string.Empty;

        }

        private string GetString(string key)
        {
            var culture = CultureInfo.CurrentCulture.Name; // Ensure we get the current culture
            var fullFilePath = Path.GetFullPath($"Resources/{culture}.json");

            if (File.Exists(fullFilePath))
            {
                return GetValueFromJson(key, fullFilePath);
            }
            return key; // Return the key if translation not found (for debugging)
        }

   
    }

}
