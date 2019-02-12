namespace LevenshteinDemo.Services
{
    public interface ILevenshtein
    {
        int Distance(string source, string target);
    }
}
