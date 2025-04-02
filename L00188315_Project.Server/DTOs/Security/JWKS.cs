namespace L00188315_Project.Server.DTOs.Security;

public class JWKS
{
    public required List<Key> keys { get; set; }
}

public class Key
{
    public required string kty { get; set; }
    public required string use { get; set; }
    public required string kid { get; set; }
    public required string e { get; set; }
    public required string n { get; set; }
    public required List<string> x5c { get; set; }
}
