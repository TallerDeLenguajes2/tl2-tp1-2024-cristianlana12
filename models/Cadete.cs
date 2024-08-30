namespace cadeteria
{
    public class Cadete
    {
        private float pagoPorPedido;

        public int Id {get; set ;}
        public string ?Nombre {get; set ;}
        public string ?Direccion {get; set ;}
        public string ?Telefono {get; set ;}

        public List<Pedido> ?ListadoPedidos { get; set; } 

        public Cadete(int id, string nombre, string direccion, string telefono){
            this.Id = id;
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Telefono = telefono;
            this.ListadoPedidos = new List<Pedido>();
        }

        public float JornalACobrar(){
            return pagoPorPedido * ListadoPedidos.Where(p => p.Estado == Estado.COMPLETADO);
        }
    }
}