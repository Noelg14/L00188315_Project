namespace L00188315_Project.Server.DTOs.Security;

public class JWKS
{
    public List<Key> keys { get; set; }
}

public class Key
{
    public string kty { get; set; }
    public string use { get; set; }
    public string kid { get; set; }
    public string e { get; set; }
    public string n { get; set; }
    public List<string> x5c { get; set; }
}
