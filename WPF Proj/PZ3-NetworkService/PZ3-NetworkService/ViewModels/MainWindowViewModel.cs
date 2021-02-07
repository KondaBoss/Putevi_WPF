using PZ3_NetworkService.Models;
using PZ3_NetworkService.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Net;
using System.Net.Sockets;
using System.Windows.Shapes;
using PZ3_NetworkService.Extras;
using System.Windows.Media;
using System.Windows;

namespace PZ3_NetworkService.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Constructors

        public MainWindowViewModel()
        {
            KreirajKolekcije();

            UcitajPodatke();

            OsveziListe();
            
            CreateListener(); //Povezivanje sa serverskom aplikacijom

            iscrtajGrafikon(null);
            
        }

        #endregion

        #region Fields

        private ObservableCollection<PutModel> putevi;
        private PutModel puteviSelected;
        private PutModel nadgledaniPuteviSelected;
        private PutModel nenadgledaniPuteviSelected;
        private ObservableCollection<StanjeModel> promeneStanja;
        private ObservableCollection<PutModel> grafikPutevi;
        private PutModel grafikPuteviSelected;
        private PutModel puteviSourceSelected;
        private ObservableCollection<FilterModel> filterId;
        private FilterModel filterIdSelected;
        private ObservableCollection<FilterModel> filterTip;
        private FilterModel filterTipSelected;
        private ObservableCollection<FilterModel> filterVrednost;
        private FilterModel filterVrednostSelected;
        private string filterIdText;
        private int id;
        private string idText;
        private string broj;
        private bool idTextNijeValidan;
        private bool brojNijeValidan;
        private bool filterIDNijeValidan;
        private ObservableCollection<TipPuta> tipoviPuta;
        private TipPuta tipoviPutaSelected;
        private Visibility confirmingMessageVisibility;
        private Visibility validationIDTextBlockVisibility;
        private Visibility validationBrojTextBlockVisibility;
        private Visibility validationFilterIDTextBlockVisibility;
        private GridLength procIAWidth;
        private GridLength procIBWidth;
        

        #endregion

        #region Properties

        public ObservableCollection<PutModel> Putevi
        {
            get { return putevi; }
            set
            {
                if (putevi != value)
                {
                    putevi = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public PutModel PuteviSelected
        {
            get { return puteviSelected; }
            set
            {
                if (puteviSelected != value)
                {
                    puteviSelected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICollectionView NadgledaniPutevi
        {
            get
            {
                var source = new CollectionViewSource { Source = Putevi }.View;
                source.SortDescriptions.Add(new SortDescription("Index", ListSortDirection.Ascending));
                source.Filter = FilterNadgledani;
                return source;
            }
        }

        public PutModel NadgledaniPuteviSelected
        {
            get { return nadgledaniPuteviSelected; }
            set
            {
                if (nadgledaniPuteviSelected != value)
                {
                    nadgledaniPuteviSelected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICollectionView NenadgledaniPutevi
        {
            get
            {
                var source = new CollectionViewSource { Source = Putevi }.View;
                source.SortDescriptions.Add(new SortDescription("Broj", ListSortDirection.Ascending));
                source.Filter = FilterNenadgledani;
                return source;
            }
        }

        public PutModel NenadgledaniPuteviSelected
        {
            get { return nenadgledaniPuteviSelected; }
            set
            {
                if (nenadgledaniPuteviSelected != value)
                {
                    nenadgledaniPuteviSelected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<StanjeModel> PromeneStanja
        {
            get { return promeneStanja; }
            set
            {
                if(promeneStanja != value)
                {
                    promeneStanja = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<PutModel> GrafikPutevi
        {
            get { return grafikPutevi; }
            set
            {
                if (grafikPutevi != value)
                {
                    grafikPutevi = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public PutModel GrafikPuteviSelected
        {
            get { return grafikPuteviSelected; }
            set
            {
                if (grafikPuteviSelected != value)
                {
                    grafikPuteviSelected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICollectionView PuteviSource
        {
            get
            {
                var source = new CollectionViewSource { Source = Putevi }.View;
                source.SortDescriptions.Add(new SortDescription("ID", ListSortDirection.Ascending));
                source.Filter = FilterPuteviSource;
                return source;
            }
        }

        public PutModel PuteviSourceSelected
        {
            get { return puteviSourceSelected; }
            set
            {
                if (puteviSourceSelected != value)
                {
                    puteviSourceSelected = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("IDIsEnabled");
                    PodesiVrednostiKontrola(value);
                }
            }
        }

        public ObservableCollection<FilterModel> FilterId
        {
            get { return filterId; }
            set
            {
                if (filterId != value)
                {
                    filterId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public FilterModel FilterIdSelected
        {
            get { return filterIdSelected; }
            set
            {
                if (filterIdSelected != value)
                {
                    filterIdSelected = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("FilterIdEnabled");
                    NotifyPropertyChanged("PuteviSource");
                }
            }
        }

        public ObservableCollection<FilterModel> FilterTip
        {
            get { return filterTip; }
            set
            {
                if (filterTip != value)
                {
                    filterTip = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public FilterModel FilterTipSelected
        {
            get { return filterTipSelected; }
            set
            {
                if (filterTipSelected != value)
                {
                    filterTipSelected = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("PuteviSource");
                }
            }
        }

        public ObservableCollection<FilterModel> FilterVrednost
        {
            get { return filterVrednost; }
            set
            {
                if (filterVrednost != value)
                {
                    filterVrednost = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public FilterModel FilterVrednostSelected
        {
            get { return filterVrednostSelected; }
            set
            {
                if (filterVrednostSelected != value)
                {
                    filterVrednostSelected = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("PuteviSource");
                }
            }
        }

        public string FilterIdText
        {
            get { return filterIdText; }
            set
            {
                if (filterIdText != value)
                {
                    filterIdText = value;
                  
                    FilterIDNijeValidan = !NumberValidationRule.Validate(filterIdText);
                    ValidationFilterIDTextBlockVisibility = FilterIDNijeValidan ? Visibility.Visible : Visibility.Collapsed;
                    
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("PuteviSource");
                }
            }
        }

        public bool FilterIDNijeValidan
        {
            get { return filterIDNijeValidan; }
            set
            {
                if (filterIDNijeValidan != value)
                {
                    filterIDNijeValidan = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool FilterIdEnabled
        {
            get { return FilterIdSelected != null && FilterIdSelected.ID > 0; }
        }


        public int ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string IDText
        {
            get { return idText; }
            set
            {
                if (idText != value)
                {
                    idText = value;

                    if (!(IDTextNijeValidan = !NumberValidationRule.Validate(idText)))
                    {
                        ID = int.Parse(idText);
                        ValidationIDTextBlockVisibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ID = 0;
                        ValidationIDTextBlockVisibility = Visibility.Visible;
                    }

                    NotifyPropertyChanged("IDText");
                    NotifyPropertyChanged("ValidationIDTextBlockVisibility");
                    NotifyPropertyChanged("ID");
                }
            }
        }

        public bool IDTextNijeValidan
        {
            get { return idTextNijeValidan; }
            set
            {
                if (idTextNijeValidan != value)
                {
                    idTextNijeValidan = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public string Broj
        {
            get { return broj; }
            set
            {
                if (broj != value)
                {
                    broj = value;

                    BrojNijeValidan = !NumberValidationRule.Validate(broj);
                    ValidationBrojTextBlockVisibility = BrojNijeValidan ? Visibility.Visible : Visibility.Collapsed;

                    NotifyPropertyChanged();
                }
            }
        }

        public bool BrojNijeValidan
        {
            get { return brojNijeValidan; }
            set
            {
                if(brojNijeValidan != value)
                {
                    brojNijeValidan = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<TipPuta> TipoviPuta
        {
            get { return tipoviPuta; }
            set
            {
                if (tipoviPuta != value)
                {
                    tipoviPuta = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public TipPuta TipoviPutaSelected
        {
            get { return tipoviPutaSelected; }
            set
            {
                if (tipoviPutaSelected != value)
                {
                    tipoviPutaSelected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Visibility ConfirmingMessageVisibility
        {
            get { return confirmingMessageVisibility; }
            set
            {
                if (confirmingMessageVisibility != value)
                {
                    confirmingMessageVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public Visibility ValidationIDTextBlockVisibility
        {
            get { return validationIDTextBlockVisibility; }
            set
            {
                if (validationIDTextBlockVisibility != value)
                {
                    validationIDTextBlockVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        public Visibility ValidationBrojTextBlockVisibility
        {
            get { return validationBrojTextBlockVisibility; }
            set
            {
                if (validationBrojTextBlockVisibility != value)
                {
                    validationBrojTextBlockVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Visibility ValidationFilterIDTextBlockVisibility
        {
            get { return validationFilterIDTextBlockVisibility; }
            set
            {
                if (validationFilterIDTextBlockVisibility != value)
                {
                    validationFilterIDTextBlockVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IDIsEnabled
        {
            get { return PuteviSourceSelected == null; }
        }

        public ObservableCollection<GraphObjectModel> Items { get; set; }

        public GridLength ProcIAWidth
        {
            get { return procIAWidth; }
            set
            {
                if (procIAWidth != value)
                {
                    procIAWidth = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public GridLength ProcIBWidth
        {
            get { return procIBWidth; }
            set
            {
                if (procIBWidth != value)
                {
                    procIBWidth = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Methods

        private void KreirajKolekcije()
        {
            PromeneStanja = new ObservableCollection<StanjeModel>();
            Items = new ObservableCollection<GraphObjectModel>();
        }

        private void UcitajPodatke()
        {
            Putevi = PuteviSvc.LoadPutevi();
            TipoviPuta = PuteviSvc.LoadTipoviPuta();
            TipoviPutaSelected = TipoviPuta.FirstOrDefault();
            GrafikPutevi = new ObservableCollection<PutModel>(PuteviSvc.LoadPutevi().OrderBy(p => p.Broj));
            GrafikPuteviSelected = GrafikPutevi.First();
            ConfirmingMessageVisibility = Visibility.Collapsed;
            ValidationIDTextBlockVisibility = Visibility.Collapsed;
            ValidationBrojTextBlockVisibility = Visibility.Collapsed;
            ValidationFilterIDTextBlockVisibility = Visibility.Collapsed;
            DodajKontroleZaDrop();
            PodesiOdnosTipova();
            BrojNijeValidan = false;
            FilterIDNijeValidan = false;
            FilterId = PuteviSvc.LoadFilterId();
            FilterIdSelected = FilterId[0];
            FilterTip = LoadFilterTip();
            FilterTipSelected = FilterTip[0];
            FilterVrednost = PuteviSvc.LoadFilterVrednost();
            FilterVrednostSelected = FilterVrednost[0];
        }

        private void CreateListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */

                            //Šaljemo broj nadgledanih objekata bez placeholder-a
                            Byte[] data = Encoding.ASCII.GetBytes(Putevi.Count(p => p.NadgledaSe && p.ID > 0).ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            //Console.WriteLine(incomming); //Na primer: "Objekat_1:272"

                            //################ IMPLEMENTACIJA ####################

                            var stanje = ParseStanje(incomming);
                            if (stanje != null)
                                OnPromenaStanja(stanje);
                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }

        private void OsveziListe()
        {
            NotifyPropertyChanged("NadgledaniPutevi");
            NotifyPropertyChanged("NenadgledaniPutevi");
        }

        private bool FilterNadgledani(object item)
        {
            PutModel put = item as PutModel;
            return put.NadgledaSe;
        }

        private bool FilterNenadgledani(object item)
        {
            PutModel put = item as PutModel;
            return !put.NadgledaSe;
        }

        private bool FilterPuteviSource(object item)
        {
            PutModel put = item as PutModel;
            bool uslov = put.ID > 0;
            
            if(uslov && FilterIdSelected != null && FilterIdSelected.ID > 0)
            {
                int filterId;
                if(int.TryParse(FilterIdText, out filterId))
                if(FilterIdSelected.ID == 1)
                {
                    uslov = put.ID > filterId;
                }
                else
                {
                    uslov = put.ID < filterId;
                }
            }

            if (uslov && FilterTipSelected != null && FilterTipSelected.ID > 0)
            {
                uslov = put.Tip.ID == FilterTipSelected.ID;
            }

            if (uslov && FilterVrednostSelected != null && FilterVrednostSelected.ID > 0)
            {
                if (FilterVrednostSelected.ID == 1)
                {
                    uslov = put.BrojVozila > put.GranicnoStanjeVozila;
                }
                else
                {
                    uslov = put.BrojVozila <= put.GranicnoStanjeVozila;
                }
            }

            return uslov;
        }

        private StanjeModel ParseStanje(string incomming)
        {
            try
            {
                string[] delovi = incomming.Split('_', ':');

                var indeks = int.Parse(delovi[1]);
                var brojVozila = int.Parse(delovi[2]);

                var put = Putevi.FirstOrDefault(np => np.NadgledaSe && np.Index == indeks);

                if (put != null && put.ID != 0)
                {
                    var id = PromeneStanja.Count == 0 ? 1 : PromeneStanja.Max(ps => ps.ID) + 1;
                    return new StanjeModel() { ID = id, Vreme = DateTime.Now, IDPuta = put.ID, BrojPuta = put.Broj, BrojVozila = brojVozila };
                }
                else
                {
                    return null;
                }

            }
            catch
            {
                return null;
            }
        }

        public void OnPromenaStanja(StanjeModel stanje)
        {
            PromeneStanja.Add(stanje);
            AzurirajStanjaNadgledanihPuteva(stanje);
            Log.Add(stanje);
        }

        private void AzurirajStanjaNadgledanihPuteva(StanjeModel stanje)
        {
            AzurirajBrojVozilaNaPutu(Putevi.FirstOrDefault(p => p.NadgledaSe && p.ID == stanje.IDPuta), stanje.BrojVozila);
        }

        private void AzurirajBrojVozilaNaPutu(PutModel nadgledaniPut, int brojVozila)
        {
            if (nadgledaniPut != null)
            {
                nadgledaniPut.BrojVozila = brojVozila;

                OsveziListe();
            }
        }

        private void PodesiVrednostiKontrola(PutModel put)
        {
            if (put == null)
            {
                ID = 0;
                Broj = string.Empty;
                TipoviPutaSelected = TipoviPuta.FirstOrDefault();
            }
            else
            {
                ID = put.ID;
                Broj = put.Broj;
                TipoviPutaSelected = TipoviPuta.FirstOrDefault(tp => tp.ID == put.Tip.ID);
            }
        }

        private void Snimi()
        {
            PuteviSvc.SavePutevi(Putevi);
            UcitajPodatke();
        }

        private void Osvezi()
        {
            PuteviSourceSelected = null;
            PodesiVrednostiKontrola(null);
        }

        private void DodajKontroleZaDrop()
        {
            for (int i = 0; i < 16; i++)
            {
                if(!Putevi.Any(p => p.NadgledaSe && p.Index == i))
                {
                    Putevi.Add(NewPutPlaceholderModel(i));
                }
            }
        }

        private void PodesiOdnosTipova()
        {
            ProcIAWidth = new GridLength(Putevi.Count(p => p.ID > 0 && string.Equals(p.Tip.Naziv, "IA")), GridUnitType.Star);
            ProcIBWidth = new GridLength(Putevi.Count(p => p.ID > 0 && string.Equals(p.Tip.Naziv, "IB")), GridUnitType.Star);
        }

        private PutPlaceholderModel NewPutPlaceholderModel(int index)
        {
            return new PutPlaceholderModel { ID = 0, Broj = string.Empty, Tip = TipoviPuta[0], NadgledaSe = true, Index = index, BrojVozila = 0 };
        }

        private ObservableCollection<FilterModel> LoadFilterTip()
        {
            var kolekcija = new ObservableCollection<FilterModel>();

            kolekcija.Add(new FilterModel() { ID = 0, Naziv = "SVI TIPOVI" });

            foreach (var tip in TipoviPuta)
            {
                kolekcija.Add(new FilterModel() { ID = tip.ID, Naziv = tip.Naziv });
            }
            return kolekcija;
        }

        private void PonistiSelekcije()
        {
            NenadgledaniPuteviSelected = null;
            NadgledaniPuteviSelected = null;
        }

        #endregion

        #region Commands

        #region DodajUNadgledaneCommand

        public ICommand DodajUNadgledaneCommand { get { return new RelayCommand(dodajUNadgledane, canDodajUNadgledane); } }

        private void dodajUNadgledane(object param)
        {
            var placeholder = (PutPlaceholderModel)Putevi.Where(p => p.NadgledaSe && p.ID == 0).OrderBy(p => p.Index).FirstOrDefault();
            
            premestiPutUNadgledane(NenadgledaniPuteviSelected, placeholder);
        }

        private bool canDodajUNadgledane(object param)
        {
            return NenadgledaniPuteviSelected != null;
        }

        #endregion

        #region VratiUNenadgledaneCommand

        public ICommand VratiUNenadgledaneCommand { get { return new RelayCommand(vratiUNenadgledaneCommand, canVratiUNenadgledaneCommand); } }

        private void vratiUNenadgledaneCommand(object param)
        {
            premestiPutUNenadgledane(NadgledaniPuteviSelected);
        }

        private bool canVratiUNenadgledaneCommand(object param)
        {
            return NadgledaniPuteviSelected != null && NadgledaniPuteviSelected.ID > 0;
        }

        #endregion

        #region Dodaj

        public ICommand Dodaj { get { return new RelayCommand(dodaj, canDodaj); } }

        private void dodaj(object param)
        {
            Putevi.Add(new PutModel { ID = ID, Broj = Broj, Tip = TipoviPutaSelected });

            Snimi();
            Osvezi();
        }

        private bool canDodaj(object param)
        {
            return PuteviSourceSelected == null
                && ID > 0
                && !BrojNijeValidan
                && !string.IsNullOrEmpty(Broj)
                && !Putevi.Any(p => string.Equals(p.ID, ID))
                && !Putevi.Any(p => string.Equals(p.Broj, Broj))
                && TipoviPutaSelected != null;
        }

        #endregion

        #region Izmeni

        public ICommand Izmeni { get { return new RelayCommand(izmeni, canIzmeni); } }

        private void izmeni(object param)
        {
            PuteviSourceSelected.Broj = Broj;
            PuteviSourceSelected.Tip = TipoviPutaSelected;

            Snimi();
        }

        private bool canIzmeni(object param)
        {
            return PuteviSourceSelected != null
                && !string.IsNullOrEmpty(Broj)
                && !Putevi.Any(p => p.ID != PuteviSourceSelected.ID && string.Equals(p.Broj, Broj))
                && TipoviPutaSelected != null;
        }

        #endregion

        #region Izbrisi

        public ICommand Izbrisi { get { return new RelayCommand(izbrisi, canIzbrisi); } }

        private void izbrisi(object param)
        {
            ConfirmingMessageVisibility = Visibility.Visible;
        }

        private bool canIzbrisi(object param)
        {
            return PuteviSourceSelected != null;
        }

        #endregion

        #region Odustani

        public ICommand Odustani { get { return new RelayCommand(odustani, canOdustani); } }

        private void odustani(object param)
        {
            Osvezi();
        }

        private bool canOdustani(object param)
        {
            return true;
        }

        #endregion

        #region BrisanjeOdustani

        public ICommand BrisanjeOdustani { get { return new RelayCommand(brisanjeOdustani, canBrisanjeOdustani); } }

        private void brisanjeOdustani(object param)
        {
            ConfirmingMessageVisibility = Visibility.Collapsed;
        }

        private bool canBrisanjeOdustani(object param)
        {
            return true;
        }

        #endregion

        #region BrisanjePotvrdi

        public ICommand BrisanjePotvrdi { get { return new RelayCommand(brisanjePotvrdi, canBrisanjePotvrdi); } }

        private void brisanjePotvrdi(object param)
        {
            Putevi.Remove(PuteviSourceSelected);

            Snimi();
            Osvezi();

            ConfirmingMessageVisibility = Visibility.Collapsed;
        }

        private bool canBrisanjePotvrdi(object param)
        {
            return true;
        }

        #endregion

        #region Iscrtaj grafikon

        public ICommand IscrtajGrafikon { get { return new RelayCommand(iscrtajGrafikon, canIscrtajGrafikon); } }

        private void iscrtajGrafikon(object param)
        {
            var poslednjihPet = PromeneStanja.Where(ps => ps.IDPuta == GrafikPuteviSelected.ID).OrderByDescending(ps => ps.ID).Take(5);
            if (poslednjihPet != null)
            {
                var kolekcija = new ObservableCollection<StanjeModel>(poslednjihPet);
                Crtac.IscrtajGrafik(Items, kolekcija);
            }
        }

        private bool canIscrtajGrafikon(object param)
        {
            return GrafikPuteviSelected != null;
        }
        #endregion

        #region PremestiPutUNenadgledane Command

        /// <summary>
        /// Private member backing variable for <see cref="PremestiPutUNenadgledane" />
        /// </summary>
        private RelayCommand _PremestiPutUNenadgledane = null;

        /// <summary>
        /// Gets the command which adds a user to the collection of users
        /// </summary>
        public RelayCommand PremestiPutUNenadgledane
        {
            get
            {
                if (_PremestiPutUNenadgledane == null)
                { _PremestiPutUNenadgledane = new RelayCommand(premestiPutUNenadgledane, canPremestiPutUNenadgledane); }

                return _PremestiPutUNenadgledane;
            }
        }

        /// <summary>
        /// Implements the execution of <see cref="PremestiPutUNenadgledane" />
        /// </summary>
        private void premestiPutUNenadgledane(object param)
        {
            if(param != null && param is PutModel)
            {
                var nPut = (PutModel)param;
                var tempIndex = nPut.Index;

                nPut.Index = 0;
                nPut.NadgledaSe = false;

                Putevi.Add(NewPutPlaceholderModel(tempIndex));

                PuteviSvc.SavePutevi(Putevi);

                PonistiSelekcije();
                OsveziListe();
            }
        }

        /// <summary>
        /// Determines if <see cref="PremestiPutUNenadgledane" /> is allowed to execute
        /// </summary>
        private bool canPremestiPutUNenadgledane(object param)
        {
            return true;
        }

        #endregion

        #region PremestiPutUNadgledane Command

        /// <summary>
        /// Private member backing variable for <see cref="PremestiPutUNadgledane" />
        /// </summary>
        private RelayCommand _PremestiPutUNadgledane = null;

        /// <summary>
        /// Gets the command which adds a user to the collection of users
        /// </summary>
        public RelayCommand PremestiPutUNadgledane
        {
            get
            {
                if (_PremestiPutUNadgledane == null)
                { _PremestiPutUNadgledane = new RelayCommand(premestiPutUNadgledane, canPremestiPutUNadgledane); }

                return _PremestiPutUNadgledane;
            }
        }

        /// <summary>
        /// Implements the execution of <see cref="PremestiPutUNadgledane" />
        /// </summary>
        private void premestiPutUNadgledane(object param)
        {
            if (param != null 
                && param is Tuple<object, object>
                && ((Tuple<object, object>)param).Item1 is PutModel
                && ((Tuple<object, object>)param).Item2 is PutPlaceholderModel)
            {
                var nPut = (PutModel)((Tuple<object, object>)param).Item1;
                var placeholder = (PutPlaceholderModel)((Tuple<object, object>)param).Item2;
                premestiPutUNadgledane(nPut, placeholder);
            }
        }

        private void premestiPutUNadgledane(PutModel nPut, PutPlaceholderModel placeholder)
        {
            if (placeholder != null)
            {
                var newIndex = placeholder.Index;

                Putevi.Remove(placeholder);

                nPut.Index = newIndex;
                nPut.NadgledaSe = true;

                PuteviSvc.SavePutevi(Putevi);

                PonistiSelekcije();
                OsveziListe();
            }
        }

        /// <summary>
        /// Determines if <see cref="PremestiPutUNadgledane" /> is allowed to execute
        /// </summary>
        private bool canPremestiPutUNadgledane(object param)
        {
            return true;
        }

        #endregion

        #endregion

        #region INotifyPropertyChanged implementacija

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
