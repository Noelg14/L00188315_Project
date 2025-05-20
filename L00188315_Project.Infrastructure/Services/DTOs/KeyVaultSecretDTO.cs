using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Infrastructure.Services.DTOs;
/// <summary>
/// Attributes for the Key Vault Secret.
/// </summary>
public class Attributes
{
    /// <summary>
    /// Enabled flag
    /// </summary>
    public bool? enabled { get; set; }
    /// <summary>
    /// Date the secret was created
    /// </summary>
    public int? created { get; set; }
    /// <summary>
    /// Date the secret was updated
    /// </summary>
    public int? updated { get; set; }
    /// <summary>
    /// Recovery level 
    /// </summary>
    public string? recoveryLevel { get; set; }
    /// <summary>
    /// Days before the secret is unrecoverable
    /// </summary>
    public int? recoverableDays { get; set; }
}
/// <summary>
/// Key Vault Secret Data Transfer Object.
/// </summary>
public class KeyVaultSecretDTO
{
    /// <summary>
    /// Secret value
    /// </summary>
    public required string value { get; set; }
    /// <summary>
    /// Id of the secret
    /// </summary>
    public required string id { get; set; }
    /// <summary>
    /// Secret Attributes
    /// </summary>
    public Attributes? attributes { get; set; }
    /// <summary>
    /// Tags for the seret
    /// </summary>
    public Tags? tags { get; set; }
}
/// <summary>
/// Tags for the Key Vault Secret.
/// </summary>
public class Tags { }
