using System.Text.Json.Serialization;

namespace L00188315_Project.Infrastructure.Services.DTOs
{
    /// <summary>
    /// KeyVaultCertDTO class  for Key Vault Certificate.
    /// </summary>
    public class KeyVaultCertDTO
    {
        /// <summary>
        /// ID of the certificate.
        /// </summary>
        public string? id { get; set; }
        /// <summary>
        /// KID of the certificate.
        /// </summary>
        public string? kid { get; set; }
        /// <summary>
        /// SID of the certificate.
        /// </summary>
        public string? sid { get; set; }
        /// <summary>
        /// x5t of the certificate.
        /// </summary>
        public string? x5t { get; set; }
        /// <summary>
        /// Cert value
        /// </summary>
        public string? cer { get; set; }
        /// <summary>
        /// Attributes of the certificate.
        /// </summary>
        public Attributes? attributes { get; set; }
        /// <summary>
        /// Policy of the certificate.
        /// </summary>
        public Policy? policy { get; set; }

        /// <summary>
        /// Action class for Key Vault Certificate.
        /// </summary>
        public class Action
        {
            /// <summary>
            /// Action type 
            /// </summary>
            public string? action_type { get; set; }
        }
        /// <summary>
        /// Attributes class for Key Vault Certificate.
        /// </summary>
        public class Attributes
        {
            /// <summary>
            /// Is certificate enabled.
            /// </summary>
            public bool enabled { get; set; }
            /// <summary>
            /// Not before date.
            /// </summary>
            public int nbf { get; set; }
            /// <summary>
            /// Expiration date.
            /// </summary>
            public int exp { get; set; }
            /// <summary>
            /// Date Created.
            /// </summary>
            public int created { get; set; }
            /// <summary>
            /// Date Updated.
            /// </summary>
            public int updated { get; set; }
            /// <summary>
            /// Recovery level.
            /// </summary>
            public string? recoveryLevel { get; set; }
            /// <summary>
            /// Days before unecoveable
            /// </summary>
            public int recoverableDays { get; set; }
        }
        /// <summary>
        /// Basic constraints class for Key Vault Certificate.
        /// </summary>
        public class BasicConstraints
        {
            /// <summary>
            /// Certificate authority.
            /// </summary>
            public bool ca { get; set; }
        }
        /// <summary>
        /// Cert Issuer
        /// </summary>
        public class Issuer
        {
            /// <summary>
            /// Certificate issuer name.
            /// </summary>
            public string? name { get; set; }
        }
        /// <summary>
        /// Key properties 
        /// </summary>
        public class KeyProps
        {
            /// <summary>
            /// Is key exportable.
            /// </summary>
            public bool exportable { get; set; }
            /// <summary>
            /// Key type.
            /// </summary>
            public string? kty { get; set; }
            /// <summary>
            /// Key size.
            /// </summary>
            public int key_size { get; set; }
            /// <summary>
            /// Reuse key.
            /// </summary>
            public bool reuse_key { get; set; }
        }
        /// <summary>
        /// Lifetime action 
        /// </summary>
        public class LifetimeAction
        {
            /// <summary>
            /// Trigger for the action.
            /// </summary>
            public Trigger? trigger { get; set; }
            /// <summary>
            /// Action for the trigger.
            /// </summary>
            public Action? action { get; set; }
        }
        /// <summary>
        /// Policy 
        /// </summary>
        public class Policy
        {
            /// <summary>
            /// Id of the policy.
            /// </summary>
            public string? id { get; set; }
            /// <summary>
            /// Key Properties
            /// </summary>
            public KeyProps? key_props { get; set; }
            /// <summary>
            /// Secret properties
            /// </summary>
            public SecretProps? secret_props { get; set; }
            /// <summary>
            /// X509 properties
            /// </summary>
            public X509Props? x509_props { get; set; }
            /// <summary>
            /// Lifetime actions
            /// </summary>
            public List<LifetimeAction>? lifetime_actions { get; set; }
            /// <summary>
            /// Issuer
            /// </summary>
            public Issuer? issuer { get; set; }
            /// <summary>
            /// Cert Atributes
            /// </summary>
            public Attributes? attributes { get; set; }
        }
        /// <summary>
        /// Secret properties
        /// </summary>
        public class SecretProps
        {
            /// <summary>
            /// Content type
            /// </summary>
            public string? contentType { get; set; }
        }
        /// <summary>
        /// Trigger 
        /// </summary>
        public class Trigger
        {
            /// <summary>
            /// Lifetime percentage
            /// </summary>
            public int lifetime_percentage { get; set; }
        }
        /// <summary>
        /// X509 Properties
        /// </summary>
        public class X509Props
        {
            /// <summary>
            /// Subject of the certificate.
            /// </summary>
            public string? subject { get; set; }
            /// <summary>
            /// extended key usage.
            /// </summary>
            public List<object>? ekus { get; set; }
            /// <summary>
            /// Key usage.
            /// </summary>
            public List<object>? key_usage { get; set; }
            /// <summary>
            /// Months Cert is valid.
            /// </summary>
            public int validity_months { get; set; }
            /// <summary>
            /// Basic Constraints.
            /// </summary>
            public BasicConstraints? basic_constraints { get; set; }
        }
    }
}
