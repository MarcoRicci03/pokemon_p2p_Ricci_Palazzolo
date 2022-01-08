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
        Gioco WPFGioco;
        int pippo = 0;
        public Lotta(DatiCondivisi datiConnessione, DatiCondivisiGioco datiGioco, Gioco WPFGioco)
        {
            InitializeComponent();
            this.datiConnessione = datiConnessione;
            this.datiGioco = datiGioco;
            this.WPFGioco = WPFGioco; //finestra precedente
            datiGioco.setWpfLotta(this);
            datiGioco.aggPokemonMio(-1);
            //Thread aggiornamentoPagina = new Thread(aggPagina);
            //aggiornamentoPagina.Start();
            //aggPagina();
            if (pippo == 0)
            {
                Thread turno = new Thread(prova);
                turno.Start();
                //datiGioco.scegliTurno();
            }
            
        }
        public void prova()
        {
            datiGioco.scegliTurno();
            pippo++;
            if (datiGioco.mioTurno == false)
                attesaTurno();
        }
        private void aggPagina()
        {
            BitmapImage bitimg = new BitmapImage();
            datiGioco.aggGrafica();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(@"Properties/" + datiGioco.pokemonAvv.hires, UriKind.RelativeOrAbsolute);
            bitimg.EndInit();
            imgPokemonA.Source = bitimg;
        }

        private void btnMossa1_Click(object sender, RoutedEventArgs e)
        {
            if(datiGioco.mioTurno == true && datiGioco.togliPP(1))
            {
                datiConnessione.manda("m;" + datiConnessione.peerQuesto.port_peer + ";" + datiGioco.getPokemonAttualeNostro().move1.id);
                datiGioco.setTurno(false);
                //ricevo eventuale nuovo pokemon avversario
                attesaTurno();
            }
        }

        private void btnMossa3_Click(object sender, RoutedEventArgs e)
        {
            if (datiGioco.mioTurno == true && datiGioco.togliPP(3))
            {
                datiConnessione.manda("m;" + datiConnessione.peerQuesto.port_peer + ";" + datiGioco.getPokemonAttualeNostro().move3.id);
                datiGioco.setTurno(false);
                attesaTurno();
            }
        }

        private void btnMossa2_Click(object sender, RoutedEventArgs e)
        {
            if (datiGioco.mioTurno == true && datiGioco.togliPP(2))
            {
                datiConnessione.manda("m;" + datiConnessione.peerQuesto.port_peer + ";" + datiGioco.getPokemonAttualeNostro().move2.id);
                datiGioco.setTurno(false);
                attesaTurno();
            }
        }

        private void btnMossa4_Click(object sender, RoutedEventArgs e)
        {
            if (datiGioco.mioTurno == true && datiGioco.togliPP(4))
            {
                datiConnessione.manda("m;" + datiConnessione.peerQuesto.port_peer + ";" + datiGioco.getPokemonAttualeNostro().move4.id);
                datiGioco.setTurno(false);
                attesaTurno();
            }
        }

        public void attesaTurno()
        {

            changeAll(false);
            //Thread attesaa = new Thread(attesa);
            // attesaa.Start();
            attesa();
            
        }

        public void changeAll(bool b)
        {
            btnMossa1.IsEnabled = b;
            btnMossa2.IsEnabled = b;
            btnMossa3.IsEnabled = b;
            btnMossa4.IsEnabled = b;
        }

        public void attesa()
        {
            do
            {
                datiConnessione.ricevi();
                if (datiConnessione.risAscolto[0] != null)
                {
                    datiGioco.setTurno(true);
                }
            } while (!datiGioco.mioTurno);
            changeAll(true);
            datiGioco.aggPokemonMio(datiGioco.searchListMoves(Int32.Parse(datiConnessione.risAscolto[2])).power);
            datiConnessione.risAscolto = null;
        }
    }
}
