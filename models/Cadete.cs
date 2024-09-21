namespace cadeteria
{
    public class Cadete
    {
        private const float pagoPorPedido = 500f;

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        public List<Pedido> ListadoPedidos { get; set; }

        public Cadete(int id, string nombre, string direccion, string telefono)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Telefono = telefono;
            this.ListadoPedidos = new List<Pedido>();
        }

        public float JornalACobrar()
        {
            return pagoPorPedido * ListadoPedidos.Where(p => p.Estado == Estado.COMPLETADO).Count();
        }

        public List<Pedido> buscarPedidos(Estado e)
        {
            return ListadoPedidos.Where(p => p.Estado == e).ToList();
            /*
            p => p.Estado == e: Es una expresión lambda. Es una forma corta de escribir una función anónima.
            Esta expresión significa: "Para cada pedido p en ListadoPedidos, revisa si el Estado de ese pedido (p.Estado)
            es igual al estado e que se pasó como parámetro". Si es verdadero, el pedido p se incluye en el resultado; si es falso, no se incluye.
            */
        }

        public override string ToString()
        {
            return $"CADETE: {Id} - {Nombre} - {Direccion} - {Telefono}";
        }
    }
}