using System.IO.Compression;
using System.Text.Json.Serialization;

public class Cadeteria
{
    private const float precioPorPedido = 500;

    private string nombre;
    private string telefono;
    private List<Cadete> listadoCadetes;
    private List<Pedido> pedidosAsignados;
    private List<Pedido> pedidoTomado;

    public Cadeteria()
    {
        nombre = string.Empty;
        telefono = string.Empty;
        listadoCadetes = new List<Cadete>();
        pedidosAsignados = new List<Pedido>();
        pedidoTomado = new List<Pedido>();
    }

    public Cadeteria(string nombre, string telefono)
    {
        this.nombre = nombre;
        this.telefono = telefono;
    }
    [JsonPropertyName("nombre")]
    public string Nombre { get => nombre; set => nombre = value; }

    [JsonPropertyName("telefono")]
    public string Telefono { get => telefono; set => telefono = value; }

    public List<Cadete> ListadoCadetes { get => listadoCadetes; set => listadoCadetes = value; }
    public List<Pedido> PedidosAsignados { get => pedidosAsignados; set => pedidosAsignados = value; }
    public List<Pedido> PedidoTomado { get => pedidoTomado; set => pedidoTomado = value; }

    public void asignarCadete(Cadete cadete, Pedido pedido){
        pedido.Cadete = cadete;
        pedidoTomado.Remove(pedido);
        pedidosAsignados.Add(pedido);
    }

    public void altaCadete (Cadete cadete){
        listadoCadetes.Add(cadete);
    }

    public List<Pedido> obtenerTodosLosPedidos(){
        return pedidoTomado.Concat(pedidosAsignados).ToList();
    }

    public float jornalAcobrar(int idCadete){
        return precioPorPedido * pedidosAsignados.Where(p => p.Cadete.Id == idCadete && 
                                                            p.Estado == Estado.COMPLETO).Count();
    }

    public List<Pedido> buscarPedidos(int idCadete){
        return pedidosAsignados.Where(p => p.Cadete.Id == idCadete).ToList();
    }

    public override string ToString()
    {
        return $"CADETERIA: {nombre} - {telefono}";
    }
}