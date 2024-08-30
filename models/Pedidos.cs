namespace cadeteria
{
    public class Pedido
    {
        private static int  numGenerado =0;

        public int Nro {get; set;}
        public string? obs;
        public Cliente? Cliente {get; set;}

        public Estado estado {get; set;}

        public  string verDireccionCliente(){
            return $"Pedido Numero: {Nro} - direccion: {Cliente.Direccion}";
        }

        public string VerDatosClientes(){
            return $"Cliente asociado al pedido:{Nro}\nCliente: {Cliente.Nombre} ";
        }

        public Pedido (string observaciones, Cliente cliente){
            Nro = ++numGenerado; //generamos el ID de forma consecutiva
            estado = Estado.PENDIENTE;

            this.obs = obs;
            this.Cliente = cliente;
        }
    }
}