using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

public class Cadete
{
    private const float pagoPorPedido = 500;
    private int id;
    private string nombre;
    private string direccion;
    private string telefono;

    public Cadete()
    {
        id = -1;
        nombre = string.Empty;
        direccion = string.Empty;
        telefono = string.Empty;
    }

    public Cadete(int id, string nombre, string direccion, string telefono)
    {
        this.id = id;
        this.nombre = nombre;
        this.direccion = direccion;
        this.telefono = telefono;
    }
    [JsonPropertyName("id")]
    public int Id { get => id; set => id = value; }
    [JsonPropertyName("nombre")]
    public string Nombre { get => nombre; set => nombre = value; }
    [JsonPropertyName("direccion")]
    public string Direccion { get => direccion; set => direccion = value; }
    [JsonPropertyName("telefono")]
    public string Telefono { get => telefono; set => telefono = value; }

}