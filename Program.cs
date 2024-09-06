using cadeteria;

internal class Program
{
    private static Cadeteria  cadeteria;
    private static void Main(string[] args)
    {
        try
        {
            cadeteria = CrearCadeteria();
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    private static Cadeteria CrearCadeteria()
    {
        var Csv = LeerCsv("datosCadeteria.csv");

        var datos = Csv[0].Split(",");

        if(datos.Count() < 2)throw new Exception("No hay datos suficientes para instanciar la cadeteria");

        return new Cadeteria(datos[0], datos[1]);
    }

    private static void cargarCadetes(Cadeteria cadeteria){
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
    }q

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
}