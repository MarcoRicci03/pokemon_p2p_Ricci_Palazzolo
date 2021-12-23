using System;
using System.Threading;
using System.Windows;

namespace pokemon_showdown_p2p
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DatiCondivisi dati;
        public DatiCondivisiGioco datiGioco;
        bool isRicevendo, avanti = false;
        Window Gioco;
        public MainWindow()
        {
            InitializeComponent();
            dati = new DatiCondivisi();
            datiGioco = new DatiCondivisiGioco();
            isRicevendo = false;
            /*Thread TCheckAvanti = new Thread(checkAvanti);
            TCheckAvanti.Start();
            TCheckAvanti.IsBackground = true;*/
            if(avanti == false)
            {
                Gioco = new Gioco(dati, datiGioco);
                datiGioco.loadDataFromJSON();
                Gioco.ShowDialog();
                this.Close();
            }
        }
        private void btnMandaConnessione_Click(object sender, RoutedEventArgs e)
        {
            dati.peerConnesso.ip_peer_connesso = txtIpDest.Text;
            dati.peerConnesso.port_peer_connesso = Int32.Parse(txtPortDest.Text);
            Thread inviaConnessione = new Thread(dati.inviaConnessione);
            // Name of the thread is Mythread
            inviaConnessione.Name = "Thread per ricezione";
            inviaConnessione.Start();
            // IsBackground is the property of Thread
            // which allows thread to run in the background
            inviaConnessione.IsBackground = true;
        }

        private void btnRiceviConnessione_Click(object sender, RoutedEventArgs e)
        {
            if(!isRicevendo)
            {
                Thread riceviConnessione = new Thread(dati.riceviConnessione);
                // Name of the thread is Mythread
                riceviConnessione.Name = "Thread per ricezione";
                riceviConnessione.Start();
                // IsBackground is the property of Thread
                // which allows thread to run in the background
                riceviConnessione.IsBackground = true;
                isRicevendo = true;
            } else
            {
                Console.WriteLine("Il peer è già in ascolto");
            }
        }

        public void checkAvanti()
        {
            //do
            //{

            //} while (!dati.connesso);
            avanti = true;
        }
    }
}
