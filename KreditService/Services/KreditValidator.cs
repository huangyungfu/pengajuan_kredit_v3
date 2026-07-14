namespace KreditService.Data; // This matching namespace allows KreditController to see it without errors

public static class KreditValidator
{
    public static string? ValidateCoreMetrics(decimal plafon, decimal bunga, int tenor)
    {
        if (plafon <= 0) return "Plafon must be greater than 0.";
        if (bunga <= 0 || bunga > 100) return "Bunga must be greater than 0 and maximum 100.";
        if (tenor <= 0) return "Tenor must be a positive non-decimal whole number greater than 0.";
        return null;
    }
}