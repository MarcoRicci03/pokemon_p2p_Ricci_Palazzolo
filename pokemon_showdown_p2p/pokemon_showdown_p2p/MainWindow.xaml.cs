using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pokemon_showdown_p2p
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatiCondivisi dati;
        bool isRicevendo;
        public MainWindow()
        {
            InitializeComponent();
            dati = new DatiCondivisi();
            isRicevendo = false;
        }
        private void btnMandaConnessione_Click(object sender, RoutedEventArgs e)
        {
            dati.ip_peer_connesso = txtIpDest.Text;
            dati.port_peer_connesso = Int32.Parse(txtPortDest.Text);
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
    }
}
