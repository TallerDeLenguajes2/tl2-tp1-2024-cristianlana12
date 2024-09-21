namespace cadeteria

{
    public class Cadeteria

    {
        public string Nombre { get; set; } 
        public string Telefono { get; set; }

        private List<Cadete> listadoCadetes;
        private List<Pedido> listadoPedidos;

        public Cadeteria(string nombre, string telefono)

        {
            this.Nombre = nombre;
            this.Telefono = telefono;

            ListadoCadetes = new List<Cadete>();
            ListadoPedidos = new List<Pedido>();
        }
        public List<Cadete> ListadoCadetes { get => listadoCadetes; set => listadoCadetes = value; }
        public List<Pedido> ListadoPedidos { get => listadoPedidos; set => listadoPedidos = value; }

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
        }

        public void AltaCadete(Cadete cadete)
        {
            ListadoCadetes.Add(cadete);
        }

        public List<Pedido> ObtenerPedidosAsignados()

        {
            return ListadoCadetes.SelectMany(cadete => cadete.ListadoPedidos).Where(pedido => pedido.estado == Estado.PENDIENTE).ToList();
        }

        public List<Pedido> ObtenerTodosLosPedidos()

        {
            return ListadoPedidos.Concat(ObtenerPedidosAsignados()).ToList();
        }

        public override string ToString()
        {
            return $"CADETERIA: {Nombre} - {Telefono}";
        }

    }
}