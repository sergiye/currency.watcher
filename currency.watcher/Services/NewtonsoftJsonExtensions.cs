using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;

namespace currency.watcher {

  public static class NewtonsoftJsonExtensions {
    
    private static readonly JsonSerializerSettings settings;
    
    static NewtonsoftJsonExtensions() {
      settings = new JsonSerializerSettings {
        DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
        NullValueHandling = NullValueHandling.Ignore,
        // DateFormatHandling = DateFormatHandling.IsoDateFormat,
        DateFormatString = "yyyy-MM-dd",
        Converters = {
          new DecimalJsonConverter(),
        }
      };
      // settings.Converters.Add(new StringEnumConverter());
    }

    public static string ToJson(this object obj) {
      return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
    }

    public static T FromJson<T>(this string json) where T : class {
      return JsonConvert.DeserializeObject<T>(json, settings);
    }
    
    public static void ToJsonFile(this object value, string filePath) {
      File.WriteAllText(filePath, ToJson(value));
    }
  }

  class DecimalJsonConverter : JsonConverter {

    public override bool CanConvert(Type objectType) {
      return objectType == typeof(decimal);
    }

    public override bool CanRead => false;

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      writer.WriteRawValue(((decimal)value).ToString("0.####", CultureInfo.InvariantCulture));
    }
  }

  public static class DecimalExtensions {

    public static bool AlmostEquals(this decimal value1, decimal value2, decimal precision = (decimal)0.0001) {
      return (Math.Abs(value1 - value2) < precision);
    }

    public static bool Differs(this decimal value1, decimal value2, decimal precision = (decimal)0.0001) {
      return (Math.Abs(value1 - value2) > precision);
    }
  }
}