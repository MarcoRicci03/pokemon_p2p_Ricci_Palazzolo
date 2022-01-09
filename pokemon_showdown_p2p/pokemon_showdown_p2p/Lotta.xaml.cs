using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;


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
        BackgroundWorker bg = new BackgroundWorker();
        public Lotta(DatiCondivisi datiConnessione, DatiCondivisiGioco datiGioco, Gioco WPFGioco)
        {
            InitializeComponent();
            this.datiConnessione = datiConnessione;
            this.datiGioco = datiGioco;
            this.WPFGioco = WPFGioco; //finestra precedente
            datiGioco.setWpfLotta(this);
            datiGioco.aggPokemonMio(-1);
            datiConnessione.manda("p;" + datiConnessione.peerQuesto.port_peer.ToString() + ";" + datiGioco.getPokemonAttualeNostro().pokemonScelto.id);

            //avvio i due thread
            //ricevi
            Thread ascolta = new Thread(new ThreadStart(datiConnessione.ricevi));
            ascolta.Start();

            //elabora
            Thread elabora = new Thread(controlla);
            elabora.Start();

            if (datiGioco.mioTurno == false)
                attesaTurno();

        }

        private void controlla()
        {
            Random r = new Random();

            string[] temp;
            int dannoRicevuto = 0, nRandom, nRandomUltimo = 0, valTempRand = -1, sizePrec = -1;
            do
            {
                do
                {
                    nRandom = r.Next();
                }
                while (nRandom == 0 || datiConnessione.getLista().Count == 0);

                do
                {

                } while (datiConnessione.getLista().Count == sizePrec);

                temp = datiConnessione.getLista()[datiConnessione.getLista().Count - 1].Split(';');
                if (valTempRand != -1)
                {
                    if (temp[0] == "p" && valTempRand != nRandomUltimo)
                    {
                        datiGioco.pokemonAvv = datiGioco.searchListPokemon(Int32.Parse(temp[2]));
                        Dispatcher.Invoke(() =>
                        {
                            datiGioco.aggPokemonAvv(-1, Int32.Parse(temp[2]));
                        });
                        nRandomUltimo = nRandom;
                        sizePrec = datiConnessione.getLista().Count;
                    }
                    if (temp[0] == "m" && valTempRand != nRandomUltimo)
                    {
                        //pokemonAlleatoAttuale = listPokemonSelezionatiConMosse[index];
                        dannoRicevuto = datiGioco.searchListMoves(Int32.Parse(temp[2])).power;
                        Dispatcher.Invoke(() =>
                        {
                            datiGioco.aggPokemonMio(dannoRicevuto);
                        });
                        nRandomUltimo = nRandom;
                        datiGioco.setTurno(true);
                        sizePrec = datiConnessione.getLista().Count;
                    }
                    if (temp[0] == "f" && valTempRand != nRandomUltimo)
                    {
                        //chiudo la partita
                        MessageBox.Show("Prova1", "Prova2", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else if (temp.Length == 3)
                {
                    if (temp[0] == "p")
                    {
                        datiGioco.pokemonAvv = datiGioco.searchListPokemon(Int32.Parse(temp[2]));
                        Dispatcher.Invoke(() =>
                        {
                            datiGioco.aggPokemonAvv(-1, Int32.Parse(temp[2]));
                        });
                        nRandomUltimo = nRandom;
                        sizePrec = datiConnessione.getLista().Count;
                    }
                    if (temp[0] == "m")
                    {
                        //pokemonAlleatoAttuale = listPokemonSelezionatiConMosse[index];
                        dannoRicevuto = datiGioco.searchListMoves(Int32.Parse(temp[2])).power;
                        Dispatcher.Invoke(() =>
                        {
                            datiGioco.aggPokemonMio(dannoRicevuto);
                        });
                        nRandomUltimo = nRandom;
                        datiGioco.setTurno(true);
                        sizePrec = datiConnessione.getLista().Count;
                    }
                }
                valTempRand = nRandom;
            } while (true);
        }

        //private void aggPagina()
        //{
        //    BitmapImage bitimg = new BitmapImage();
        //    //datiGioco.aggGrafica();
        //    bitimg.BeginInit();
        //    try
        //    {
        //        datiGioco.pokemonAvv.hires = null;
        //    }
        //    catch (NullReferenceException e)
        //    {
        //        Console.WriteLine("ciao");
        //    }

        //    do
        //    {
        //    } while (datiGioco.pokemonAvv.hires == null);
        //    bitimg.UriSource = new Uri(@"Properties/" + datiGioco.pokemonAvv.hires, UriKind.RelativeOrAbsolute);
        //    bitimg.EndInit();
        //    imgPokemonA.Source = bitimg;
        //}

        private void btnMossa1_Click(object sender, RoutedEventArgs e)
        {
            if (datiGioco.mioTurno == true && datiGioco.togliPP(1))
            {
                datiConnessione.manda("m;" + datiConnessione.peerQuesto.port_peer + ";" + datiGioco.getPokemonAttualeNostro().move1.id);
                datiGioco.setTurno(false);
                datiGioco.aggPokemonAvv(datiGioco.getPokemonAttualeNostro().move1.id, -1);
                //ricevo eventuale nuovo pokemon avversario
               // attesaTurno();
            }
        }

        private void btnMossa3_Click(object sender, RoutedEventArgs e)
        {
            if (datiGioco.mioTurno == true && datiGioco.togliPP(3))
            {
                datiConnessione.manda("m;" + datiConnessione.peerQuesto.port_peer + ";" + datiGioco.getPokemonAttualeNostro().move3.id);
                datiGioco.setTurno(false);
                datiGioco.aggPokemonAvv(datiGioco.getPokemonAttualeNostro().move1.id, -1);
                // attesaTurno();
            }
        }

        private void btnMossa2_Click(object sender, RoutedEventArgs e)
        {
            if (datiGioco.mioTurno == true && datiGioco.togliPP(2))
            {
                datiConnessione.manda("m;" + datiConnessione.peerQuesto.port_peer + ";" + datiGioco.getPokemonAttualeNostro().move2.id);
                datiGioco.setTurno(false);
                datiGioco.aggPokemonAvv(datiGioco.getPokemonAttualeNostro().move1.id, -1);
                //attesaTurno();
            }
        }

        private void btnMossa4_Click(object sender, RoutedEventArgs e)
        {
            if (datiGioco.mioTurno == true && datiGioco.togliPP(4))
            {
                datiConnessione.manda("m;" + datiConnessione.peerQuesto.port_peer + ";" + datiGioco.getPokemonAttualeNostro().move4.id);
                datiGioco.setTurno(false);
                datiGioco.aggPokemonAvv(datiGioco.getPokemonAttualeNostro().move1.id, -1);
                //attesaTurno();
            }
        }

        public void attesaTurno()
        {
            Thread attesaa = new Thread(attesa);
            attesaa.Start();
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
            Dispatcher.Invoke(() =>
            {
                changeAll(false);
            });
            do
            {
                //datiConnessione.ricevi();
                if (datiConnessione.risAscolto[0] != null)
                {
                    datiGioco.setTurno(true);
                }
            } while (!datiGioco.mioTurno);
            Dispatcher.Invoke(() =>
            {
                changeAll(true);
            });
            datiGioco.aggPokemonMio(datiGioco.searchListMoves(Int32.Parse(datiConnessione.risAscolto[2])).power);
            datiConnessione.risAscolto = null;
        }
    }
}
