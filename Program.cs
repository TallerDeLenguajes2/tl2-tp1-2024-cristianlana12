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
    }

    private static List<string> LeerCsv(string v, bool cabecera = true)
    {
        var lineas = new List<string>();
    }
}