using Newtonsoft.Json.Converters;

namespace currency {

  public class DateFormatConverter : IsoDateTimeConverter {

    public DateFormatConverter(string format) {
      DateTimeFormat = format;
    }
  }
}