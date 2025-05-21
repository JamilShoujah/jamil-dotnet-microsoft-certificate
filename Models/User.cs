using System;
using System.ComponentModel.DataAnnotations;

namespace JamilDotnetMicrosoftCertificate.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string Name { get; set; }
        public required string Email { get; set; }

        [Range(18, 100)]
        public int Age { get; set; }

        public required string Role { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
