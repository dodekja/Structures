namespace SemestralThesisOne.Core
{
    internal static class Helpers
    {
        internal static string PadOrTrimString(string str, int desiredLength)
        {
            int difference = desiredLength - str.Length;

            if (difference == 0)
            {
                return str;
            }

            if (difference > 0)
            {
                return str.PadRight(desiredLength);
            }

            return str.Substring(0, desiredLength);
        }
    }
}
