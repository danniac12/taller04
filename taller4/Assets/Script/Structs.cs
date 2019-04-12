

/// <summary>
/// En este struct se almacena la informacion que se le manda desde la clase Walker
/// </summary>
public struct InfoZomb
{
    public string gusto;
    public string nombre;
    public float edad;
}

/// <summary>
/// En este struct se almacena la informacion que se le manda desde la clase Aldeano
/// </summary>
public struct InfoAlde
{
    public string name;
    public int edad;
    static public implicit operator InfoZomb(InfoAlde a)
    {
        InfoZomb z = new InfoZomb();
        z.edad = a.edad;
        z.gusto = "Cerebors";
        z.nombre = "Zombie " + a.name;
        return a;
    }
}

