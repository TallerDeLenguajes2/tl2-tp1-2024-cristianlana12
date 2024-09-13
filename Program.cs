using cadeteria;

internal class Program
{
    private static Cadeteria cadeteria;
    private static void Main(string[] args)
    {
        try
        {
            cadeteria = CrearCadeteria();
            cargarCadetes(cadeteria);
        }
        catch (System.Exception e)
        {
            mostrarError(e.Message);
        }

        int opcionSeleccionado = 0;
        int opcionSalida = 5;

        do
        {
            Console.WriteLine("### MENU PRINCIPAL ###\n");
            Console.WriteLine("\t1- Dar de alta un pedido");
            Console.WriteLine("\t2- Asignar un pedido a un cadete");
            Console.WriteLine("\t3- Cambiar el estado de un pedido");
            Console.WriteLine("\t4- Reasignar el cadete en un pedido");
            Console.WriteLine("\t5- Salir del programa");
            Console.Write("\n>Seleccione una opcion: ");

            var strSeleccion = Console.ReadLine() ?? string.Empty;

            try
            {
                if (!int.TryParse(strSeleccion, out opcionSeleccionado))
                {
                    throw new Exception("debe ingresar un numero entero");
                }
                else if (opcionSeleccionado < 1 || opcionSeleccionado > opcionSalida)
                {
                    throw new Exception("Debe ingresar una opcion valida");
                }
                else
                {
                    switch (opcionSeleccionado)
                    {
                        case 1:
                            Console.WriteLine("\n\n ### INGRESAR PEDIDO ###\n");
                            var cliente = SolicitarDatosCliente();
                            break;
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        } while (opcionSeleccionado != opcionSalida);
    }

    private static Cadeteria CrearCadeteria()
    {
        var Csv = LeerCsv("datosCadeteria.csv");

        var datos = Csv[0].Split(",");

        if (datos.Count() < 2) throw new Exception("No hay datos suficientes para instanciar la cadeteria");

        return new Cadeteria(datos[0], datos[1]);
    }

    private static void cargarCadetes(Cadeteria cadeteria)
    {
        var Csv = LeerCsv("DatosCadetes.csv");

        foreach (var linea in Csv)
        {
            var datos = linea.Split(",");
            if (datos.Count() < 4)
            {
                System.Console.WriteLine($"\n[!] No se pudo cargar el cadete: {linea} - {datos}");
                continue;
            }

            cadeteria.AltaCadete(new Cadete(int.Parse(datos[0]), datos[1], datos[2], datos[3]));
        }
    }

    private static List<string> LeerCsv(string archivo, bool cabecera = true)
    {
        var lineas = new List<string>();

        using (FileStream archivoCsv = new FileStream(archivo, FileMode.Open))
        {
            using (StreamReader readerCsv = new StreamReader(archivoCsv))
            {
                if (cabecera) readerCsv.ReadLine();

                while (readerCsv.Peek() != -1)
                {
                    var linea = readerCsv.ReadLine();
                    if (!string.IsNullOrWhiteSpace(linea)) lineas.Add(linea);
                }
            }
        }

        return lineas;
    }

    private static void mostrarError(string error)
    {
        Console.WriteLine($"\n[!] ERROR: {error}\n");
    }

    private static Cliente SolicitarDatosCliente()
    {
        Console.WriteLine("> Ingresar DNI del CLIENTE (SIN PUNTOS NI ESPACIOS) <");
        var DNI = 0;
        var stringDni = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(stringDni)) throw new Exception("El DNI no debe estar vacio");
        if (!int.TryParse(stringDni, out DNI)) throw new Exception("El DNI debe ser un numero");

        Console.WriteLine("> Ingrese el NOMBRE del CLIENTE <");
        var stringNombreCliente = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(stringNombreCliente)) throw new Exception("El NOMBRE no puede estar vacio");

        System.Console.Write("> Ingrese el telefono del cliente: ");
        var telefono = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(telefono)) throw new Exception("El teléfono no puede estar vacío");

        System.Console.Write("> Ingrese la dirección del cliente: ");
        var direccion = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(direccion)) throw new Exception("La dirección no puede estar vacía");

        System.Console.Write("> Ingrese datos o referencias de la dirección del cliente (opcional): ");
        var datosRerencia = Console.ReadLine() ?? string.Empty;

        return new Cliente(DNI, stringNombreCliente,direccion, telefono, datosRerencia);
    }
}