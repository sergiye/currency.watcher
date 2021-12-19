using Xamarin.Forms;

namespace currency.mobile.Services {
  
  public static class Utils {

    public static Label GetLabel(string text, int fontSize, Color textColor) {
      return new Label {
        Text = text, FontSize = fontSize, TextColor = textColor,
        VerticalOptions = LayoutOptions.CenterAndExpand,
        HorizontalOptions = LayoutOptions.CenterAndExpand
      };
    }

    public static Color GetDiffColor(decimal delta) {
      return delta > 0 ? Color.LimeGreen
        : delta < 0 ? Color.OrangeRed
        : Color.White;
    }

    public static string GetDirection(decimal delta) {
      return delta < 0 ? "↓" : delta > 0 ? "↑" : "-";
    }
  }
}
