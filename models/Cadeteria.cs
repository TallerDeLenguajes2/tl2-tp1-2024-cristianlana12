using System.IO.Compression;
using System.Text.Json.Serialization;

namespace cadeteria
{
    public class Cadeteria

    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        [JsonPropertyName("telefono")]
        public string Telefono { get; set; }

        private List<Cadete> listadoCadetes;
        private List<Pedido> listadoPedidos;
        private List<Pedido> pedidosAsignados;
        public List<Cadete> ListadoCadetes { get => listadoCadetes; set => listadoCadetes = value; }
        public List<Pedido> ListadoPedidos { get => listadoPedidos; set => listadoPedidos = value; }
        public List<Pedido> PedidosAsignados { get => pedidosAsignados; set => pedidosAsignados = value; }

        public Cadeteria()
        {
            Nombre = string.Empty;
            Telefono = string.Empty;
            ListadoCadetes = new List<Cadete>();
            ListadoPedidos = new List<Pedido>();
            PedidosAsignados = new List<Pedido>();
        }

        public Cadeteria(string nombre, string telefono)
        {
            this.Nombre = nombre;
            this.Telefono = telefono;
        }

        public void tomarPedido(Pedido newPedido)
        {
            ListadoPedidos.Add(newPedido);
        }

        public void AsignarCadete(Cadete cadete, Pedido pedido)

        {
            cadete.ListadoPedidos.Add(pedido);
            var cadeteReasignado = ListadoCadetes.Where(c => c.Id != cadete.Id && c.ListadoPedidos.Contains(pedido)).FirstOrDefault();
            if (cadeteReasignado != null) cadeteReasignado.ListadoPedidos.Remove(pedido);

            ListadoPedidos.Remove(pedido);
            pedidosAsignados.Add(pedido);
        }

        public void AltaCadete(Cadete cadete)
        {
            ListadoCadetes.Add(cadete);
        }
        public List<Pedido> ObtenerTodosLosPedidos()
        {
            return ListadoPedidos.Concat(pedidosAsignados).ToList();
        }


        public float JornalACobrar(int idCadete)
        {
            float precioPorPedido = 500;
            return precioPorPedido * pedidosAsignados.Where(p => p.Cadete.Id == idCadete).Count();
        }

        public List<Pedido> BuscarPedidos(int idCadete)
        {
            return pedidosAsignados.Where(p => p.Cadete.Id == idCadete).ToList();
        }
        
        public override string ToString()
        {
            return $"CADETERIA: {Nombre} - {Telefono}";
        }

    }
}