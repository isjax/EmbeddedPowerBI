using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmbeddedPowerBI.Areas.Identity.Data;

namespace EmbeddedPowerBI.Areas.Identity.Services
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private SignInManager<ApplicationUser> signInManager;

        static readonly string symbols = "!#$%^*";
        static readonly string digits = "0123456789";
        static readonly string lowercase = "abcdefghjkmnpqrstuvwxyz";
        static readonly string uppercase = "ABCDEFGHJKMNPQRSTUVWXYZ";

        public string GeneratePassword(SignInManager<ApplicationUser> signInManager)
        {
            Random random = new Random();
            PasswordOptions options = signInManager.Options.Password;
            List<char> characters = new List<char>(options.RequiredLength);

            do
            {
                if (characters.Count == options.RequiredLength)
                {
                    break;
                }
                else if (options.RequireDigit)
                {
                    characters.Add(GetRandomChar(random, digits));
                }
                if (characters.Count == options.RequiredLength)
                {
                    break;
                }
                else if (options.RequireLowercase)
                {
                    characters.Add(GetRandomChar(random, lowercase));
                }
                if (characters.Count == options.RequiredLength)
                {
                    break;
                }
                else if (options.RequireNonAlphanumeric)
                {
                    characters.Add(GetRandomChar(random, symbols));
                    if (characters.Count == options.RequiredLength) break;
                }
                if (characters.Count == options.RequiredLength)
                {
                    break;
                }
                else if (options.RequireUppercase)
                {
                    characters.Add(GetRandomChar(random, uppercase));
                }

            } while (true);

            return new string(characters.ToArray());
        }

        private char GetRandomChar(Random random, string charCandidates)
        {
            return charCandidates[random.Next(0, charCandidates.Length - 1)];
        }

    }
}
