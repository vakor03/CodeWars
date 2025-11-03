using System;
using System.Text;

public class RailFenceCipher {
    public static string Encode(string s, int n) {
        StringBuilder sb = new StringBuilder();
        foreach (int index in GetIndexSeq(s.Length, n))
            sb.Append(s[index]);
        return sb.ToString();
    }

    public static string Decode(string s, int n) {
        StringBuilder sb = new StringBuilder();
        char[] chars = new char[s.Length];
        List<int> list = GetIndexSeq(s.Length, n).ToList();
        for (int i = 0; i < list.Count; i++)
            chars[list[i]] = s[i];

        return chars.Aggregate("", (current, next) => current + next);
    }

    private static IEnumerable<int> GetIndexSeq(int n, int order) {
        for (int i = 0; i < order; i++) {
            int indentation = i;
            int distance = (order - i - 1) * 2;
            int globalDistance = (order - 1) * 2;
            if (i == 0)
                for (int j = 0; j < n; j += globalDistance)
                    yield return j;
            else if (i == order - 1)
                for (int j = order - 1; j < n; j += globalDistance)
                    yield return j;
            else
                for (int j = 0; j < n; j += globalDistance) {
                    int first = j + indentation;
                    int second = first + distance;

                    if (first < n)
                        yield return first;

                    if (second < n)
                        yield return second;
                }
        }
    }
}