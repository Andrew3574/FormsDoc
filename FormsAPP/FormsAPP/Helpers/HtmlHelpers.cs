using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormsAPP.Helpers
{
    public static class HtmlHelpers
    {
        private static Dictionary<int, string> _timeUnits = new Dictionary<int, string>()
        {
            { 31536000, "year" },
            { 2592000, "month" },
            { 604800, "week" },
            { 86400, "day" },
            { 3600, "hour" },
            { 60, "minute" }
        };

        public static HtmlString LastLoginTime(this IHtmlHelper html, DateTime lastLoginDate)
        {
            var totalSeconds = (DateTime.UtcNow - lastLoginDate).TotalSeconds;
            var timeUnit = _timeUnits.FirstOrDefault(t => totalSeconds >= t.Key);
            if (timeUnit.Value != null)
            {
                var count = (int)(totalSeconds / timeUnit.Key);
                return new HtmlString($"{count} {(count > 1 ? timeUnit.Value + "s" : timeUnit.Value)} ago");
            }
            return new HtmlString("less than a minute ago");
        }
    }
}
