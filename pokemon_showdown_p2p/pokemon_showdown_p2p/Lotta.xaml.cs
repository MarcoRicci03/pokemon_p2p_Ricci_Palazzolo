using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;


namespace pokemon_showdown_p2p
{
    /// <summary>
    /// Logica di interazione per Lotta.xaml
    /// </summary>
    public partial class Lotta : Window
    {
        private DatiCondivisi datiConnessione;
        private DatiCondivisiGioco datiGioco;
        private Gioco WPFGioco;
        private BackgroundWorker bg = new BackgroundWorker();
        private Thread elabora, ascolta, waitEnd, waitTurno;
        private Boolean fineAttesaTurno = false;
        private bool flag = true;
        public Lotta(DatiCondivisi datiConnessione, DatiCondivisiGioco datiGioco, Gioco WPFGioco)
        {
            InitializeComponent();
            this.datiConnessione = datiConnessione;
            this.datiGioco = datiGioco;
            this.WPFGioco = WPFGioco; //finestra precedente
            datiGioco.setWpfLotta(this);
            datiGioco.aggPokemonMio(-1);
            datiConnessione.manda("p;" + datiConnessione.peerQuesto.ipport + ";" + datiGioco.getPokemonAttualeNostro().pokemonScelto.id);

            //avvio i due thread
            //ricevi
            ascolta = new Thread(datiConnessione.ricevi);
            ascolta.Start();

            //elabora
            elabora = new Thread(controlla);
            elabora.Start();

            waitEnd = new Thread(waitEndd);
            waitEnd.Start();

            waitTurno = new Thread(attesa);
            waitTurno.Start();
        }

        private void waitEndd()
        {
            elabora.Join();
            fineAttesaTurno = true;
            datiConnessione.fineAscolto = true;
            Dispatcher.Invoke(() =>
            {
                WPFGioco.Show();
                flag = false;
                this.Close();
            });
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
                        datiGioco.aggPokemonAvv(-1, Int32.Parse(temp[2]));
                        nRandomUltimo = nRandom;
                        sizePrec = datiConnessione.getLista().Count;
                    }
                    if (temp[0] == "m" && valTempRand != nRandomUltimo)
                    {
                        dannoRicevuto = datiGioco.searchListMoves(Int32.Parse(temp[2])).power;
                        datiGioco.aggPokemonMio(dannoRicevuto);
                        if (datiGioco.perso)
                        {
                            datiConnessione.manda("f;" + datiConnessione.peerQuesto.ipport + ";Hai vinto");
                            Dispatcher.Invoke(() =>
                            {
                                lblHPAlleato.Content = "0/" + pBNostra.Maximum;
                                pBNostra.Value = 0;
                            });
                            Thread.Sleep(2000);
                            MessageBox.Show("Hai perso :(", "Bella partita.", MessageBoxButton.OK, MessageBoxImage.Information);
                            sizePrec = -1;

                        }
                        nRandomUltimo = nRandom;
                        datiGioco.setTurno(true);
                        if (sizePrec != -1)
                            sizePrec = datiConnessione.getLista().Count;
                    }
                    if (temp[0] == "f" && valTempRand != nRandomUltimo)
                    {
                        Thread.Sleep(2000);
                        MessageBox.Show("Hai vinto!", "Bella partita.", MessageBoxButton.OK, MessageBoxImage.Information);
                        sizePrec = -1;
                    }
                }
                else
                {
                    if (temp[0] == "p")
                    {
                        datiGioco.pokemonAvv = datiGioco.searchListPokemon(Int32.Parse(temp[2]));
                        datiGioco.aggPokemonAvv(-1, Int32.Parse(temp[2]));
                        nRandomUltimo = nRandom;
                        sizePrec = datiConnessione.getLista().Count;
                    }
                    //if (temp[0] == "m")
                    //{
                    //    dannoRicevuto = datiGioco.searchListMoves(Int32.Parse(temp[2])).power;
                    //    datiGioco.aggPokemonMio(dannoRicevuto);
                    //    nRandomUltimo = nRandom;
                    //    datiGioco.setTurno(true);
                    //    sizePrec = datiConnessione.getLista().Count;
                    //}
                }
                valTempRand = nRandom;
            } while (sizePrec != -1);
        }

        private void btnMossa1_Click(object sender, RoutedEventArgs e)
        {
            int ris = 0;
            if (datiGioco.mioTurno)
            {
                ris = datiGioco.togliPP(1);
                if (ris == 0)
                {
                    datiConnessione.manda("m;" + datiConnessione.peerQuesto.ipport + ";" + datiGioco.getPokemonAttualeNostro().move1.id);
                    datiGioco.setTurno(false);
                    datiGioco.aggPokemonAvv(datiGioco.getPokemonAttualeNostro().move1.power, -1);
                }
                if (ris == 1)
                {
                    btnMossa1.Background = Brushes.Red;
                    btnMossa1.Content += "\nPP Finiti.";
                }
            }
            else if (ris == 2)
            {
                if (datiGioco.indexMio < 5)
                {
                    btnMossa1.Background = Brushes.Red;
                    btnMossa1.Content += "\nPP Finiti.";
                }
            }
        }

        private void btnMossa3_Click(object sender, RoutedEventArgs e)
        {
            int ris = 0;
            if (datiGioco.mioTurno)
            {
                ris = datiGioco.togliPP(3);
                if (ris == 0)
                {
                    datiConnessione.manda("m;" + datiConnessione.peerQuesto.ipport + ";" + datiGioco.getPokemonAttualeNostro().move3.id);
                    datiGioco.setTurno(false);
                    datiGioco.aggPokemonAvv(datiGioco.getPokemonAttualeNostro().move3.power, -1);
                }
                if (ris == 1)
                {
                    btnMossa3.Background = Brushes.Red;
                    btnMossa3.Content += "\nPP Finiti.";
                }
            }
            else if (ris == 2)
            {
                if (datiGioco.indexMio < 5)
                {
                    btnMossa3.Background = Brushes.Red;
                    btnMossa3.Content += "\nPP Finiti.";
                }
            }
        }


        private void btnMossa2_Click(object sender, RoutedEventArgs e)
        {
            int ris = 0;
            if (datiGioco.mioTurno)
            {
                ris = datiGioco.togliPP(2);
                if (ris == 0 || ris == 1)
                {
                    datiConnessione.manda("m;" + datiConnessione.peerQuesto.ipport + ";" + datiGioco.getPokemonAttualeNostro().move2.id);
                    datiGioco.setTurno(false);
                    datiGioco.aggPokemonAvv(datiGioco.getPokemonAttualeNostro().move2.power, -1);
                    if (ris == 1)
                    {
                        btnMossa2.Background = Brushes.Red;
                        btnMossa2.Content += "\nPP Finiti.";
                    }
                }
                else if (ris == 2)
                {
                    if (datiGioco.indexMio < 5)
                    {
                        btnMossa2.Background = Brushes.Red;
                        btnMossa2.Content += "\nPP Finiti.";
                    }
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (flag)
            {
                datiConnessione.manda("f;" + datiConnessione.peerQuesto.ipport + ";Hai vinto");
            }
        }

        private void btnMossa4_Click(object sender, RoutedEventArgs e)
        {
            int ris = 0;
            if (datiGioco.mioTurno)
            {
                ris = datiGioco.togliPP(4);
                if (ris == 0)
                {
                    datiConnessione.manda("m;" + datiConnessione.peerQuesto.ipport + ";" + datiGioco.getPokemonAttualeNostro().move4.id);
                    datiGioco.setTurno(false);
                    datiGioco.aggPokemonAvv(datiGioco.getPokemonAttualeNostro().move4.power, -1);
                }
                if (ris == 1)
                {
                    btnMossa4.Background = Brushes.Red;
                    btnMossa4.Content += "\nPP Finiti.";
                }
            }
            else if (ris == 2)
            {
                if (datiGioco.indexMio < 5)
                {
                    btnMossa4.Background = Brushes.Red;
                    btnMossa4.Content += "\nPP Finiti.";
                }
            }
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
                do
                {

                } while (datiGioco.getTurno());
                Dispatcher.Invoke(() =>
                {
                    changeAll(false);
                });
                do
                {

                } while (!datiGioco.mioTurno);
                Dispatcher.Invoke(() =>
                {
                    changeAll(true);
                });
            } while (!fineAttesaTurno);
        }
    }
}
