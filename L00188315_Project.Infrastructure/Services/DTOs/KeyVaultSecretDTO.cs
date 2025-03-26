using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Infrastructure.Services.DTOs;

public class Attributes
{
    public bool? enabled { get; set; }
    public int? created { get; set; }
    public int? updated { get; set; }
    public string recoveryLevel { get; set; }
    public int? recoverableDays { get; set; }
}

public class KeyVaultSecretDTO
{
    public string value { get; set; }
    public string id { get; set; }
    public Attributes attributes { get; set; }
    public Tags tags { get; set; }
}

public class Tags { }
