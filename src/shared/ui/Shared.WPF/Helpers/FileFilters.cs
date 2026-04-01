namespace Shared.WPF.Helpers
{
    public static class FileFilters
    {
        public static string ImagesAndPdf => Build("Изображения и PDF документы", ".jpg", ".jpeg", ".png", ".webp", ".pdf");
        public static string Images => Build("Изображения", ".jpg", ".jpeg", ".png", ".webp");
        public static string Pdf => Build("PDF документы", ".pdf");
        public static string Word => Build("Word документы", ".doc", ".docx");
        public static string Excel => Build("Excel", ".xlsx", ".xls");
        public static string All => Build("Все файлы", ".*");

        public static string Build(string description, params string[] extensions)
        {
            if (extensions.Length == 0) 
                return string.Empty;

            var pattern = string.Join("; ", extensions.Select(x => x.StartsWith("*") ? x : $"*{x}"));

            return $"{description} ({pattern})|{pattern}";
        }
    }
}
