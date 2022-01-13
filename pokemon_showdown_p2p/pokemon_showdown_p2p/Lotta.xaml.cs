﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
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
            datiConnessione.manda("p;" + datiConnessione.peerQuesto.ipport + ";" + datiGioco.getPokemonAttualeNostro().pokemonScelto.id);

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
                        //datiGioco.pokemonAvv = datiGioco.searchListPokemon(Int32.Parse(temp[2]));
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
                        //Dispatcher.Invoke(() =>
                        //{
                        datiGioco.aggPokemonMio(dannoRicevuto);
                        //});
                        nRandomUltimo = nRandom;
                        datiGioco.setTurno(true);
                        sizePrec = datiConnessione.getLista().Count;
                    }
                    if (temp[0] == "f" && valTempRand != nRandomUltimo)
                    {
                        //chiudo la partita
                        MessageBox.Show("Hai vinto!", "Bella partita.", MessageBoxButton.OK, MessageBoxImage.Information);
                        nRandomUltimo = nRandom;
                        sizePrec = datiConnessione.getLista().Count;
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
                        //Dispatcher.Invoke(() =>
                        //{
                        datiGioco.aggPokemonMio(dannoRicevuto);
                        //});
                        nRandomUltimo = nRandom;
                        datiGioco.setTurno(true);
                        sizePrec = datiConnessione.getLista().Count;
                    }
                }
                valTempRand = nRandom;
            } while (true);
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
                    // attesaTurno();
                }
                else if (ris == 1)
                {
                    if (datiGioco.indexMio < 5)
                    {
                        btnMossa1.Background = Brushes.Red;
                        btnMossa1.Content += "\nPP Finiti.";
                    }
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
                    // attesaTurno();
                }
                else if (ris == 1)
                {
                    if (datiGioco.indexMio < 5)
                    {
                        btnMossa3.Background = Brushes.Red;
                        btnMossa3.Content += "\nPP Finiti.";
                    }
                }
            }
        }

        private void btnMossa2_Click(object sender, RoutedEventArgs e)
        {
            int ris = 0;
            if (datiGioco.mioTurno)
            {
                ris = datiGioco.togliPP(2);
                if (ris == 0)
                {
                    datiConnessione.manda("m;" + datiConnessione.peerQuesto.ipport + ";" + datiGioco.getPokemonAttualeNostro().move2.id);
                    datiGioco.setTurno(false);
                    datiGioco.aggPokemonAvv(datiGioco.getPokemonAttualeNostro().move2.power, -1);
                    // attesaTurno();
                }
                else if (ris == 1)
                {
                    if (datiGioco.indexMio < 5)
                    {
                        btnMossa2.Background = Brushes.Red;
                        btnMossa2.Content += "\nPP Finiti.";
                    }
                }
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
                    // attesaTurno();
                }
                else if (ris == 1)
                {
                    if (datiGioco.indexMio < 5)
                    {
                        btnMossa4.Background = Brushes.Red;
                        btnMossa4.Content += "\nPP Finiti.";
                    }
                }
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
                //if (datiConnessione.risAscolto[0] != null)
                //{
                //    datiGioco.setTurno(true);
                //}
            } while (!datiGioco.mioTurno);
            Dispatcher.Invoke(() =>
            {
                changeAll(true);
            });
            //datiGioco.aggPokemonMio(datiGioco.searchListMoves(Int32.Parse(datiConnessione.risAscolto[2])).power);
            //datiConnessione.risAscolto = null;
        }
    }
}
