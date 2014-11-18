using Kozol.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Kozol.Utilities {
    public class KozolUtilities {
        public static string HashMD5(string input) {
            // Calculate MD5 hash from input.
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // Convert byte array to hex string.
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static byte[] CreateSalt(int size = 16) {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return buff;
        }

        public static byte[] HashSHA256(string value, byte[] salt) {
            return HashSHA256(Encoding.UTF8.GetBytes(value), salt);
        }

        public static byte[] HashSHA256(byte[] value, byte[] salt) {
            byte[] saltedValue = value.Concat(salt).ToArray();
            return new SHA256Managed().ComputeHash(saltedValue);
        }

        public static string HashStringSHA256(string value, byte[] salt) {
            return Convert.ToBase64String(HashSHA256(Encoding.UTF8.GetBytes(value), salt));
        }

        public static string HashStringSHA256(byte[] value, byte[] salt) {
            byte[] saltedValue = value.Concat(salt).ToArray();
            return Convert.ToBase64String(new SHA256Managed().ComputeHash(saltedValue));
        }
    }

    public class DatabaseSeeder : DropCreateDatabaseIfModelChanges<KozolContainer> {
        protected override void Seed(KozolContainer context) {
            // Insert required base user, admin, and speaker roles.
            var defaultRole = context.UserRoles
                .Where(r => r.ID == 1)
                .FirstOrDefault();
            if (defaultRole == null) {
                context.UserRoles.Add(new UserRole {
                    ID = 1,
                    Name = "User"
                });
            } else if (defaultRole.Name != "User") {
                defaultRole.Name = "User";
            }

            var adminRole = context.UserRoles
                .Where(r => r.ID == 2)
                .FirstOrDefault();
            if (adminRole == null) {
                context.UserRoles.Add(new UserRole {
                    ID = 2,
                    Name = "Global Admin"
                });
            } else if (adminRole.Name != "Global Admin") {
                adminRole.Name = "Global Admin";
            }

            var speakerRole = context.UserRoles
                .Where(r => r.ID == 3)
                .FirstOrDefault();
            if (speakerRole == null) {
                context.UserRoles.Add(new UserRole {
                    ID = 3,
                    Name = "Global Speaker"
                });
            } else if (speakerRole.Name != "Global Speaker") {
                speakerRole.Name = "Global Speaker";
            }

            context.SaveChanges();

            UserManager.CreateUser("testUser@test.com", "testPassword", "TestUser");

            UserManager.CreateUser("esteinkerchner@gmail.com", "testPassword", "Roundaround");
            UserManager.AssignUserToRoles("esteinkerchner@gmail.com", new List<string>() { "Global Admin", "Global Speaker" });
        }
    }
}