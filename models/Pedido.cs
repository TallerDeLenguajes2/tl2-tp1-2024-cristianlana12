namespace cadeteria
{
    public class Pedido
    {
        private static int  numGenerado =0;

        public int Nro {get; set;}
        public string obs;
        internal Estado Estado;

        public Cliente Cliente {get; set;}

        public Estado estado {get; set;}

        public Pedido (string observaciones, Cliente cliente){
            Nro = ++numGenerado; //generamos el ID de forma consecutiva
            estado = Estado.PENDIENTE;
            this.obs = observaciones;
            this.Cliente = cliente;
        }
        public  string verDireccionCliente(){
            return $"Pedido Numero: {Nro} - direccion: {Cliente.Direccion}";
        }

        public string VerDatosClientes(){
            return $"Cliente asociado al pedido:{Nro}\nCliente: {Cliente.Nombre} ";
        }

    }
}