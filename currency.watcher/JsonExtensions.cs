using Newtonsoft.Json;

namespace currency.watcher {
  public static class JsonExtensions {

    private static readonly JsonSerializerSettings settings;

    static JsonExtensions() {
      settings = new JsonSerializerSettings {
        TypeNameHandling = TypeNameHandling.None,
        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        Formatting = Formatting.Indented
      };
    }

    public static string ToJson(this object obj) {
      return JsonConvert.SerializeObject(obj, settings);
    }

    public static T FromJson<T>(this string value) where T : class {
      return JsonConvert.DeserializeObject<T>(value, settings);
    }
  }
}