namespace FlightDocsSystem.Helpers
{
    public static class Utils
    {
        private static string GetFirstChar(string text)
        {
            var letters = text.Split(" ");
            string result = string.Concat(letters.Select(x => x[0])).ToUpper();
            return result;
        }

        public static string TwoPointToRoute(string loading, string unloading) => GetFirstChar(loading.Trim()) + " - " + GetFirstChar(unloading.Trim());

        public static string HashPassword(this string input) => BCrypt.Net.BCrypt.HashPassword(input);

        public static bool CheckPassword(this string password, string hashPassword) => BCrypt.Net.BCrypt.Verify(password, hashPassword);

        public static string GetNewVersion(string version)
        {
            float verFloat = float.Parse(version);
            verFloat += 0.1f;
            if (verFloat == (int)verFloat)
                return verFloat.ToString() + ".0";
            else
                return verFloat.ToString();
        }
    }
}
