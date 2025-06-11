using System;

public class InvertedIndexMachine
{
    static InvertedIndexMachine instance;
    public static InvertedIndexMachine Instance { 
        get {
            if (instance == null)
            {
                instance = new InvertedIndexMachine();
            }
            return instance;
        }
    }

    Func<int, bool> isUppercased;

    private InvertedIndexMachine()
    {
        isUppercased = num => num >= 65 && num <= 90;
    }

    public void SplitString(string s, Action<string, int, int> makeTokenAndAddToCollection)
    {
        int c = 0;
        bool hasJustCheckedSpecialChar = false;
        for (int i = 0; i < s.Length; i++)
        {
            if (!isUppercased(s[i]) && (s[i] < 97 || s[i] > 122) && (s[i] < 48 || s[i] > 57))
            {
                HandleSpecialChar(hasJustCheckedSpecialChar, s, ref c, i, makeTokenAndAddToCollection);
                hasJustCheckedSpecialChar = true;
            }
            else
            {
                if (!hasJustCheckedSpecialChar && i != 0 && isUppercased(s[i]))
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

    void HandleSpecialChar(bool hasJustCheckedSpecialChar, string s,
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

    void HandleUppercase(string s, ref int startOfCut, int cutPoint,
        Action<string, int, int> makeTokenAndAddToCollection)
    {
        makeTokenAndAddToCollection(s, startOfCut, cutPoint);
        startOfCut = cutPoint;
    }

    void HandleOnTheLastChar(string s, int startOfCut, Action<string, int, int> makeTokenAndAddToCollection)
    {
        makeTokenAndAddToCollection(s, startOfCut, s.Length);
    }

}
