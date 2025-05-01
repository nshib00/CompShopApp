using System.Security.Cryptography;


namespace DAL.Utils
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16); // 128 бит

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                byte[] hashBytes = new byte[salt.Length + hash.Length];
                Buffer.BlockCopy(salt, 0, hashBytes, 0, salt.Length);
                Buffer.BlockCopy(hash, 0, hashBytes, salt.Length, hash.Length);

                return Convert.ToBase64String(hashBytes);
            }
        }

        // Метод для проверки пароля
        public static bool VerifyPassword(string storedHash, string passwordToVerify)
        {
            // Декодируем строку из Base64, чтобы получить соль и хеш
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Извлекаем соль из первых 16 байт
            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, salt.Length);

            // Извлекаем хеш из оставшихся байт
            byte[] storedHashValue = new byte[hashBytes.Length - salt.Length];
            Buffer.BlockCopy(hashBytes, salt.Length, storedHashValue, 0, storedHashValue.Length);

            // Хешируем введённый пароль с теми же параметрами
            using (var pbkdf2 = new Rfc2898DeriveBytes(passwordToVerify, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); // Длина хеша 256 бит (32 байта)

                // Сравниваем хеши
                for (int i = 0; i < storedHashValue.Length; i++)
                {
                    if (hash[i] != storedHashValue[i])
                    {
                        return false; // Пароль неверный
                    }
                }
            }

            return true; // Пароль верный
        }
    }
}
