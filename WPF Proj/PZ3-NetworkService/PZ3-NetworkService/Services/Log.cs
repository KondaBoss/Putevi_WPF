using PZ3_NetworkService.Models;
using System.IO;

namespace PZ3_NetworkService.Services
{
    public static class Log
    {
        public const string fileName = "Log.txt";

        public static void Add(StanjeModel stanje)
        {
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine($"Vreme: {stanje.Vreme.ToString("dd.MM.yyyy hh:mm:ss ")}, broj puta: {stanje.BrojPuta}, broj vozila: {stanje.BrojVozila.ToString()}");
            }
        }
    }
}
