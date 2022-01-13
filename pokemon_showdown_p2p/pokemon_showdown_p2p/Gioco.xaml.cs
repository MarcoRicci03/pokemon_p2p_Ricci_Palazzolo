using System.Windows;

namespace pokemon_showdown_p2p
{
    /// <summary>
    /// Logica di interazione per Gioco.xaml
    /// </summary>
    public partial class Gioco : Window
    {
        public static Window selectPokemon;
        public Window paginaConnessione;
        DatiCondivisi datiConnessione;
        DatiCondivisiGioco datiGioco;
        public Gioco(DatiCondivisi dati, DatiCondivisiGioco datiGioco, Window paginaConnessione)
        {
            InitializeComponent();
            this.datiConnessione = dati;
            this.datiGioco = datiGioco;
            lblTesto.Content = datiConnessione.peerQuesto.nome_peer + " stai giocando contro " + datiConnessione.peerConnesso.nome_peer;
            this.paginaConnessione = paginaConnessione;
        }

        private void btnSelectPokemon_Click(object sender, RoutedEventArgs e)
        {
            selectPokemon = new selectPokemon(datiConnessione, datiGioco, this);
            selectPokemon.Show();
            this.Hide();
        }

        private void btnLotta_Click(object sender, RoutedEventArgs e)
        {
            if (datiGioco.listPokemonSelezionati.Count == 6) //possiamo iniziare a giocare solo se abbiamo 6 pokemon selezionati
            {
                datiGioco.assegnaMosse();
                Lotta fLotta = new Lotta(datiConnessione, datiGioco, this);

                //Thread riceviPacchetto = new Thread(datiConnessione.ricevi);
                //riceviPacchetto.Start();

                fLotta.Show();
                this.Hide();
            }
            else
            {
                lblErrore.Content = "Selezionare 6 pokemon";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            paginaConnessione.Show();
            this.Close();
        }
    }
}
