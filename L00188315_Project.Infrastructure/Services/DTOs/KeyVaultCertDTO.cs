using System.Text.Json.Serialization;

namespace L00188315_Project.Infrastructure.Services.DTOs
{
    public class KeyVaultCertDTO
    {
        public string? id { get; set; }
        public string? kid { get; set; }
        public string? sid { get; set; }
        public string? x5t { get; set; }
        public string? cer { get; set; }
        public Attributes? attributes { get; set; }
        public Policy? policy { get; set; }

        public class Action
        {
            public string? action_type { get; set; }
        }

        public class Attributes
        {
            public bool enabled { get; set; }
            public int nbf { get; set; }
            public int exp { get; set; }
            public int created { get; set; }
            public int updated { get; set; }
            public string? recoveryLevel { get; set; }
            public int recoverableDays { get; set; }
        }

        public class BasicConstraints
        {
            public bool ca { get; set; }
        }

        public class Issuer
        {
            public string? name { get; set; }
        }

        public class KeyProps
        {
            public bool exportable { get; set; }
            public string? kty { get; set; }
            public int key_size { get; set; }
            public bool reuse_key { get; set; }
        }

        public class LifetimeAction
        {
            public Trigger? trigger { get; set; }
            public Action? action { get; set; }
        }

        public class Policy
        {
            public string? id { get; set; }
            public KeyProps? key_props { get; set; }
            public SecretProps? secret_props { get; set; }
            public X509Props? x509_props { get; set; }
            public List<LifetimeAction>? lifetime_actions { get; set; }
            public Issuer? issuer { get; set; }
            public Attributes? attributes { get; set; }
        }

        public class SecretProps
        {
            public string? contentType { get; set; }
        }

        public class Trigger
        {
            public int lifetime_percentage { get; set; }
        }

        public class X509Props
        {
            public string? subject { get; set; }
            public List<object>? ekus { get; set; }
            public List<object>? key_usage { get; set; }
            public int validity_months { get; set; }
            public BasicConstraints? basic_constraints { get; set; }
        }
    }
}
