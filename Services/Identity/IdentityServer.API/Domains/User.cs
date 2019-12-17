using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace IdentityServer.API.Domains
{
    public sealed class User : IEquatable<User>
    {
        [Required]
        [JsonProperty("username")]
        [Key]
        public string Username { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !ReferenceEquals(obj, this))
            {
                return false;
            }

            return Equals(obj as User);
        }

        public bool Equals([AllowNull] User other)
        {
            if (other != null)
            {
                bool result = true;
                result &= string.Compare(Username, other.Username, StringComparison.Ordinal) == 0;
                result &= string.Compare(Password, other.Password, StringComparison.Ordinal) == 0;

                return result;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return new { Username, Password }.GetHashCode();
        }
    }
}
