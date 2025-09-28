using UnityEngine;

public static class NotateNumber
{
    public static string Transform(long originNumber)
    {
        string[] symbol = new string[7] { "K", "M", "G", "T", "P", "E", "Z" };

        string result = originNumber.ToString();

        if(result.Length > 4)
        {
            return result;
        }

        for(int i = 0; i < symbol.Length; ++i)
        {
            if(4 + 3 * i <= result.Length && result.Length < 4 + 3 * (i + 1))
            {
                int n = result.Length % 3;
                n = n == 0 ? 3 : n;

                result = $"{result.Substring(0, n)}.{result.Substring(n, 1)}";
                result += symbol[i];

                break;
            }
        }

        return result;
    }
}
