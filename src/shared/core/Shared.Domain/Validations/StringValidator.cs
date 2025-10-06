namespace Shared.Domain.Validations
{
    public static class StringValidator
    {
        public static bool ContainsInvalidChars(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            return input.Any(c =>
                !((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') ||
                  (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я') || 
                   c == 'ё' || c == 'Ё'));
        }
    }
}
