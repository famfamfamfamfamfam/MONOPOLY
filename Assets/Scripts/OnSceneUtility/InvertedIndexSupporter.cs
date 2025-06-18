using System;

public static class InvertedIndexSupporter
{
    public static void SplitString(string s, Action<string, int, int> makeTokenAndAddToCollection)
    {
        int c = 0;
        bool hasJustCheckedSpecialChar = false;
        for (int i = 0; i < s.Length; i++)
        {
            if (Char.IsUpper(s[i]) && (s[i] < 97 || s[i] > 122) && (s[i] < 48 || s[i] > 57))
            {
                HandleSpecialChar(hasJustCheckedSpecialChar, s, ref c, i, makeTokenAndAddToCollection);
                hasJustCheckedSpecialChar = true;
            }
            else
            {
                if (!hasJustCheckedSpecialChar && i != 0 && Char.IsUpper(s[i]))
                {
                    HandleUppercase(s, ref c, i, makeTokenAndAddToCollection);
                }
                if (i == s.Length - 1)
                {
                    HandleOnTheLastChar(s, c, makeTokenAndAddToCollection);
                    return;
                }
                hasJustCheckedSpecialChar = false;
            }
        }
    }

    static void HandleSpecialChar(bool hasJustCheckedSpecialChar, string s,
        ref int startOfCut, int cutPoint,
        Action<string, int, int> makeTokenAndAddToCollection)
    {
        if (hasJustCheckedSpecialChar)
        {
            startOfCut++;
            return;
        }
        makeTokenAndAddToCollection(s, startOfCut, cutPoint);
        startOfCut = cutPoint + 1;
    }

    static void HandleUppercase(string s, ref int startOfCut, int cutPoint,
        Action<string, int, int> makeTokenAndAddToCollection)
    {
        makeTokenAndAddToCollection(s, startOfCut, cutPoint);
        startOfCut = cutPoint;
    }

    static void HandleOnTheLastChar(string s, int startOfCut, Action<string, int, int> makeTokenAndAddToCollection)
    {
        makeTokenAndAddToCollection(s, startOfCut, s.Length);
    }

}
