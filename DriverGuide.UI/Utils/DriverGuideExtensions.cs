namespace DriverGuide.UI
{
    public static class DriverGuideExtensions
    {
        public static bool EndWith(this string? text, params string[] texts)
        {
            if (string.IsNullOrWhiteSpace(text) || texts is null || !texts.Any())
                return false;

            foreach (var v in texts)
                if (text.EndsWith(v, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }
    }
}
