using PZ3_NetworkService.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace PZ3_NetworkService.Services
{
    public static class PuteviSvc
    {
        public const string fileName = "Putevi.xml";

        public static ObservableCollection<PutModel> LoadPutevi()
        {
            var collection = new ObservableCollection<PutModel>();
            if (File.Exists(fileName))
            {
                using (var reader = new StreamReader(fileName))
                {
                    var xs = new XmlSerializer(typeof(ObservableCollection<PutModel>));
                    collection = (ObservableCollection<PutModel>)xs.Deserialize(reader);
                }
            }

            return collection;
        }

        public static void SavePutevi(ObservableCollection<PutModel> collection)
        {
            var roadCollection = new ObservableCollection<PutModel>(collection.Where(p => p.ID > 0));
            using (var writer = new StreamWriter(fileName))
            {
                var xs = new XmlSerializer(typeof(ObservableCollection<PutModel>));
                xs.Serialize(writer, roadCollection);
            }
        }

        public static ObservableCollection<TipPuta> LoadTipoviPuta()
        {
            var collection = new ObservableCollection<TipPuta>();
            collection.Add(new TipPuta() { ID = 1, Naziv = "IA" });
            collection.Add(new TipPuta() { ID = 2, Naziv = "IB" });

            return collection;
        }

        public static ObservableCollection<FilterModel> LoadFilterId()
        {
            var collection = new ObservableCollection<FilterModel>();
            collection.Add(new FilterModel() { ID = 0, Naziv = "SVI" });
            collection.Add(new FilterModel() { ID = 1, Naziv = ">" });
            collection.Add(new FilterModel() { ID = 2, Naziv = "<" });

            return collection;
        }

        public static ObservableCollection<FilterModel> LoadFilterVrednost()
        {
            var collection = new ObservableCollection<FilterModel>();
            collection.Add(new FilterModel() { ID = 0, Naziv = "SVE" });
            collection.Add(new FilterModel() { ID = 1, Naziv = "IZVAN GRANICE" });
            collection.Add(new FilterModel() { ID = 2, Naziv = "OČEKIVANE VREDNOSTI" });

            return collection;
        }
    }
}
