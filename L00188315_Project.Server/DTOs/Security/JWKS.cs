namespace L00188315_Project.Server.DTOs.Security;

/// <summary>
/// JSON Web Key Set (JWKS) for OpenID Connect
/// </summary>
public class JWKS
{
    /// <summary>
    /// List of keys in the JWKS.
    /// </summary>
    public required List<Key>? keys { get; set; }
}

/// <summary>
/// A key yhat is part of the JWKS
/// </summary>
public class Key
{
#pragma warning disable IDE1006
    /// <summary>
    /// Gets or sets the key type (kty) of the cryptographic key.
    /// </summary>
    public required string kty { get; set; }

    /// <summary>
    /// Defines the algorithm intended for use with the key.
    /// </summary>
    public required string use { get; set; }

    /// <summary>
    /// // The key identifier (kid) of the key.
    /// </summary>
    public required string kid { get; set; }

    /// <summary>
    /// The public key exponent (e) of the key.
    /// </summary>
    public required string e { get; set; }

    /// <summary>
    /// The modulus (n) of the key.
    /// </summary>
    public required string n { get; set; }

    /// <summary>
    /// X509 certificate chain (x5c) of the key.
    /// </summary>
    public required List<string> x5c { get; set; }
#pragma warning restore IDE1006
}
