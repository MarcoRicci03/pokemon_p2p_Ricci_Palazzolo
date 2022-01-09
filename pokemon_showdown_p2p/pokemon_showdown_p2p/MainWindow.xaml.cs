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
            datiGioco = new DatiCondivisiGioco(dati);
            isRicevendo = false;
        }
        private void btnMandaConnessione_Click(object sender, RoutedEventArgs e)
        {
            dati.peerConnesso.ip_peer = txtIpDest.Text;
            dati.peerConnesso.port_peer = Int32.Parse(txtPortDest.Text);
            Thread inviaConnessione = new Thread(dati.inviaConnessione);
            inviaConnessione.Start();
            datiGioco.setTurno(true);
        }

        private void btnRiceviConnessione_Click(object sender, RoutedEventArgs e)
        {
            if (!isRicevendo)
            {
                Thread riceviConnessione = new Thread(dati.riceviConnessione);
                riceviConnessione.Start();
                isRicevendo = true;
                datiGioco.setTurno(false);
            }
            else
            {
                Console.WriteLine("Il peer è già in ascolto");
            }
        }

        private void btnAvanti_Click(object sender, RoutedEventArgs e)
        {
            if(dati.connesso)
            {
                Gioco = new Gioco(dati, datiGioco);
                //principale
                datiGioco.loadDataFromJSON();
                Gioco.Show();
                this.Close();
            }
        }
    }
}
