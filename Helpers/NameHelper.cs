namespace ScratchScript.Helpers;

public class NameHelper
{
    private static Dictionary<string, int> _counter = new();

    public static string New(string start)
    {
        if (_counter.TryAdd(start, 0))
            return start;

        _counter[start]++;
        return start + _counter[start];
    }
}