using System.Web.Script.Serialization;

namespace currency.watcher {

  public static class JsonExtensions {

    public static string ToJson(this object obj) {
      return new JavaScriptSerializer().Serialize(obj);
    }

    public static T FromJson<T>(this string json) where T : class {
      return new JavaScriptSerializer().Deserialize<T>(json);
    }
  }
}