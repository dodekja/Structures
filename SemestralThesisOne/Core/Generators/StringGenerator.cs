using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SemestralThesisOne.Core.Generators
{
    internal static class StringGenerator
    {
        private static readonly string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateRandomString(int length)
        {
            var characters = new char[length];

            for (int index = 0; index < length; index++)
            {
                characters[index] = _chars[Random.Shared.Next(_chars.Length)];
            }

            return new string(characters);
        }
    }
}
