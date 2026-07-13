using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.StaticAssets;

namespace TraineeManagement.Api.Helpers;

public class Hasher
{
    public string ComputeSha256Hash(string input)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input), "Input cannot be null.");

        using (SHA256 sha256 = SHA256.Create())
        {
            
            byte[] bytes = Encoding.UTF8.GetBytes(input);

            byte[] hashBytes = sha256.ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                builder.Append(b.ToString("x2")); 
            }

            return builder.ToString();
        }
    }
}