using Newtonsoft.Json;

namespace currency.watcher {

  public static class JsonExtensions {
    
    private static readonly JsonSerializerSettings settings;
    
    static JsonExtensions() {
      settings = new JsonSerializerSettings {
        DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
        NullValueHandling = NullValueHandling.Ignore,
        // DateFormatHandling = DateFormatHandling.IsoDateFormat,
      };
      // settings.Converters.Add(new StringEnumConverter());
    }

    public static string ToJson(this object obj) {
      return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
    }

    public static T FromJson<T>(this string json) where T : class {
      return JsonConvert.DeserializeObject<T>(json, settings);
    }
  }
}