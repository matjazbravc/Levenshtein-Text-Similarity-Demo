using System;

namespace LevenshteinDemo.Services
{
    /// Levenshtein. In 1965 Vladmir Levenshtein created a distance algorithm.
    /// With Levenshtein distance, we measure similarity and match approximate strings with fuzzy logic.
    /// Compute Levenshtein distance which tells us the number of edits needed to turn one string into another.
    public class Levenshtein : ILevenshtein
    {
        private readonly IUnicodeNormalizer _unicodeNormalizer;

        public Levenshtein(IUnicodeNormalizer unicodeNormalizer)
        {
            _unicodeNormalizer = unicodeNormalizer;
        }

        public int Distance(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.IsNullOrEmpty(target) ? 0 : target.Length;
            }
            if (string.IsNullOrEmpty(target))
            {
                return source.Length;
            }
            if (Math.Max(source.Length, target.Length) > Math.Pow(2, 31))
            {
                throw new Exception("\nMaximum string length in Levenshtein.Distance is " + Math.Pow(2, 31) +
                                    ".\nYours is " + Math.Max(source.Length, target.Length) + ".");
            }
            source = _unicodeNormalizer.Normalize(source);
            target = _unicodeNormalizer.Normalize(target);
            var rowLen = source.Length;
            var colLen = target.Length;
            var v0 = new int[rowLen + 1];
            var v1 = new int[rowLen + 1];
            int rowIdx;
            for (rowIdx = 1; rowIdx <= rowLen; rowIdx++)
            {
                v0[rowIdx] = rowIdx;
            }
            int colIdx;
            for (colIdx = 1; colIdx <= colLen; colIdx++)
            {
                v1[0] = colIdx;
                var colJ = target[colIdx - 1];
                for (rowIdx = 1; rowIdx <= rowLen; rowIdx++)
                {
                    var rowI = source[rowIdx - 1];
                    var cost = rowI == colJ ? 0 : 1;
                    var mMin = v0[rowIdx] + 1;
                    var b = v1[rowIdx - 1] + 1;
                    var c = v0[rowIdx - 1] + cost;
                    if (b < mMin)
                    {
                        mMin = b;
                    }
                    if (c < mMin)
                    {
                        mMin = c;
                    }
                    v1[rowIdx] = mMin;
                }
                var vTmp = v0;
                v0 = v1;
                v1 = vTmp;
            }
            // Distance = 0 = Perfect match
            var distance = v0[rowLen];
            // 100% = Perfect match, 0% = Totaly different
            var max = Math.Max(rowLen, colLen);
            return 100 - 100 * distance / max;
        }
    }
}