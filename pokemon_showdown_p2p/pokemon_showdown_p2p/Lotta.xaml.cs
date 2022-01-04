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
using System.Windows.Shapes;

namespace pokemon_showdown_p2p
{
    /// <summary>
    /// Logica di interazione per Lotta.xaml
    /// </summary>
    public partial class Lotta : Window
    {
        DatiCondivisi datiConnessione;
        DatiCondivisiGioco datiGioco;
        CPokemon pokemonNemico;
        Gioco WPFGioco;
        int index = 0;
        public Lotta(DatiCondivisi datiConnessione, DatiCondivisiGioco datiGioco, Gioco WPFGioco)
        {
            InitializeComponent();
            this.datiConnessione = datiConnessione;
            this.datiGioco = datiGioco;
            this.WPFGioco = WPFGioco; //finestra precedente
            datiGioco.setWpfLotta(this);
            aggPagina();

            datiGioco.scegliTurno();
            //immagine pokemon avversario

        }

        private void aggPagina()
        {
            BitmapImage bitimg = new BitmapImage();
            datiGioco.aggGrafica();

            bitimg.BeginInit();
            bitimg.UriSource = new Uri(@"Properties/" + datiGioco.pokemonNemicoAttuale.hires, UriKind.RelativeOrAbsolute);
            bitimg.EndInit();
            imgPokemonA.Source = bitimg;
            //barra della vita avversario
            pBAvversario.Maximum = datiGioco.pokemonNemicoAttuale.HP;
            pBAvversario.Value = datiGioco.pokemonNemicoAttuale.HP;
            lblHPAvversario.Content = datiGioco.pokemonNemicoAttuale.HP;
            //pokemon alleato
            //pBNostra.Maximum = datiGioco.pokemonAlleatoAttuale.pokemonScelto.HP;
            //pBNostra.Value = datiGioco.pokemonAlleatoAttuale.pokemonScelto.HP;
        }

        private void btnMossa1_Click(object sender, RoutedEventArgs e)
        {
            if(datiGioco.mioTurno == true)
            {
                datiConnessione.manda("m;" + datiGioco.pokemonAlleatoAttuale.move1.id);
                datiGioco.setTurno(false);
                attesaTurno();
            }
        }

        private void btnMossa3_Click(object sender, RoutedEventArgs e)
        {
            if (datiGioco.mioTurno == true)
            {
                datiConnessione.manda("m;" + datiGioco.pokemonAlleatoAttuale.move3.id);
                datiGioco.setTurno(false);
                attesaTurno();
            }
        }

        private void btnMossa2_Click(object sender, RoutedEventArgs e)
        {
            if (datiGioco.mioTurno == true)
            {
                datiConnessione.manda("m;" + datiGioco.pokemonAlleatoAttuale.move2.id);
                datiGioco.setTurno(false);
                attesaTurno();
            }
        }

        private void btnMossa4_Click(object sender, RoutedEventArgs e)
        {
            if (datiGioco.mioTurno == true)
            {
                datiConnessione.manda("m;" + datiGioco.pokemonAlleatoAttuale.move4.id);
                datiGioco.setTurno(false);
                attesaTurno();
            }
        }

        public void attesaTurno()
        {
            do
            {
                //attendo il mio turno
            } while (!datiGioco.mioTurno);
            aggPagina();
        }
    }
}
