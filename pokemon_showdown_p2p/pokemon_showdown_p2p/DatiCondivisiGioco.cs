using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace pokemon_showdown_p2p
{
    public class DatiCondivisiGioco
    {
        public List<string> tipiPokemon { get; set; }
        public List<CMoves> listMosse { get; set; } //lista delle mosse
        public List<CPokemon> listPokemon { get; set; } //lista di tutti i pokemon basica
        public List<CPokemon> listPokemonSelezionati { get; set; } //lista pokemon selezionati basica
        public List<CPokemonConMosse> listPokemonSelezionatiConMosse { get; set; } //lista dei pokemon selezionati con mosse assegnate
        //finestra per modificar i dati
        Lotta wpfLotta;
        //boolean
        public Boolean perso { get; set; }
        public bool mioTurno { get; set; }
        //dati condivisi per la connessione
        private DatiCondivisi datiConnessione;
        //interi
        public int indexMio { get; set; }//index dei miei pokemon
        private int contPokAvv;//contatore dei pokemon avversari

        public DatiCondivisiGioco(DatiCondivisi datiConnessione)
        {
            tipiPokemon = new List<string>();
            loadDataFromJSON();//carico le liste dei pokemon
            this.datiConnessione = datiConnessione;
            contPokAvv = -1;
            indexMio = 0;
        }
        public CPokemon searchListPokemon(int idEsterno)
        {
            CPokemon pokemonReturn = null;
            foreach (CPokemon pokemon in listPokemon)
            {
                if (pokemon.id == idEsterno)
                {
                    pokemonReturn = pokemon;
                }
            }
            return pokemonReturn;
        }

        public CMoves searchListMoves(int idEsterno)
        {
            CMoves moveReturn = null;
            foreach (CMoves moves in listMosse)
            {
                if (moves.id == idEsterno)
                {
                    moveReturn = moves;
                }
            }
            return moveReturn;
        }

        private BitmapImage bitimg = null;
        private readonly object bitimgLock = new object();

        public BitmapImage getBitimg()
        {
            lock (bitimgLock)
            {
                return bitimg;
            }
        }
        public int pippo = 0;
        public CPokemon pokemonAvv { get; set; }

        public CPokemonConMosse getPokemonAttualeNostro()
        {
            return listPokemonSelezionatiConMosse[indexMio];
        }

        public void aggPokemonMio(int danno)
        {
            wpfLotta.Dispatcher.Invoke(() =>
            {
                if (danno != -1)
                {
                    listPokemonSelezionatiConMosse[indexMio].pokemonScelto.HP -= danno;
                }
                if (listPokemonSelezionatiConMosse[indexMio].pokemonScelto.HP > 0 && danno != -1)
                {

                    wpfLotta.lblHPAlleato.Content = listPokemonSelezionatiConMosse[indexMio].pokemonScelto.HP + "/" + wpfLotta.pBNostra.Maximum;
                    wpfLotta.pBNostra.Value = listPokemonSelezionatiConMosse[indexMio].pokemonScelto.HP;
                    aggColorHPBar(1);

                }
                else if (danno == -1)
                {
                    aggButton(0);
                    wpfLotta.lblHPAlleato.Content = listPokemonSelezionatiConMosse[indexMio].pokemonScelto.HP + "/" + listPokemonSelezionatiConMosse[indexMio].pokemonScelto.HP;
                    wpfLotta.pBNostra.Foreground = Brushes.LightGreen;
                    BitmapImage bitimg = new BitmapImage();
                    bitimg.BeginInit();
                    bitimg.UriSource = new Uri(@"Properties/" + listPokemonSelezionati[indexMio].hires, UriKind.RelativeOrAbsolute);
                    bitimg.EndInit();
                    wpfLotta.imgPokemonN.Source = bitimg;
                    wpfLotta.pBNostra.Maximum = listPokemonSelezionatiConMosse[indexMio].pokemonScelto.HP;
                    wpfLotta.pBNostra.Value = listPokemonSelezionatiConMosse[indexMio].pokemonScelto.HP;
                    wpfLotta.lblPokemon1Nostro.Background = Brushes.Green;
                }
                else if (listPokemonSelezionatiConMosse[indexMio].pokemonScelto.HP <= 0)
                {
                    if (indexMio + 1 >= 6)
                    {
                        perso = true;
                    }
                    else
                    {
                        aggButton(1);
                        wpfLotta.lblHPAlleato.Content = listPokemonSelezionatiConMosse[indexMio + 1].pokemonScelto.HP + "/" + listPokemonSelezionatiConMosse[indexMio + 1].pokemonScelto.HP;
                        wpfLotta.pBNostra.Foreground = Brushes.LightGreen;
                        BitmapImage bitimg = new BitmapImage();
                        bitimg.BeginInit();
                        bitimg.UriSource = new Uri(@"Properties/" + listPokemonSelezionati[indexMio + 1].hires, UriKind.RelativeOrAbsolute);
                        bitimg.EndInit();
                        wpfLotta.imgPokemonN.Source = bitimg;
                        wpfLotta.pBNostra.Maximum = listPokemonSelezionatiConMosse[indexMio + 1].pokemonScelto.HP;
                        wpfLotta.pBNostra.Value = listPokemonSelezionatiConMosse[indexMio + 1].pokemonScelto.HP;
                        datiConnessione.manda("p;" + datiConnessione.peerQuesto.ipport + ";" + listPokemonSelezionati[indexMio + 1].id);
                        switch (indexMio)
                        {
                            case 0:
                                wpfLotta.lblPokemon1Nostro.Background = Brushes.Red;
                                wpfLotta.lblPokemon2Nostro.Background = Brushes.Green;
                                break;
                            case 1:
                                wpfLotta.lblPokemon2Nostro.Background = Brushes.Red;
                                wpfLotta.lblPokemon3Nostro.Background = Brushes.Green;
                                break;
                            case 2:
                                wpfLotta.lblPokemon3Nostro.Background = Brushes.Red;
                                wpfLotta.lblPokemon4Nostro.Background = Brushes.Green;
                                break;
                            case 3:
                                wpfLotta.lblPokemon4Nostro.Background = Brushes.Red;
                                wpfLotta.lblPokemon5Nostro.Background = Brushes.Green;
                                break;
                            case 4:
                                wpfLotta.lblPokemon5Nostro.Background = Brushes.Red;
                                wpfLotta.lblPokemon6Nostro.Background = Brushes.Green;
                                break;
                            case 5:
                                wpfLotta.lblPokemon6Nostro.Background = Brushes.Red;
                                break;
                        }
                        indexMio++;
                    }
                }
            });
        }
        private readonly object buttons = new object();

        private void aggButton(int add)
        {
            lock (buttons)
            {
                wpfLotta.btnMossa1.Content = listPokemonSelezionatiConMosse[indexMio + add].move1.ename + "\nDanno:" + listPokemonSelezionatiConMosse[indexMio + add].move1.power.ToString() + "\nPP:" + listPokemonSelezionatiConMosse[indexMio + add].move1.PP.ToString();
                wpfLotta.btnMossa2.Content = listPokemonSelezionatiConMosse[indexMio + add].move2.ename + "\nDanno:" + listPokemonSelezionatiConMosse[indexMio + add].move2.power.ToString() + "\nPP:" + listPokemonSelezionatiConMosse[indexMio + add].move2.PP.ToString();
                wpfLotta.btnMossa3.Content = listPokemonSelezionatiConMosse[indexMio + add].move3.ename + "\nDanno:" + listPokemonSelezionatiConMosse[indexMio + add].move3.power.ToString() + "\nPP:" + listPokemonSelezionatiConMosse[indexMio + add].move3.PP.ToString();
                wpfLotta.btnMossa4.Content = listPokemonSelezionatiConMosse[indexMio + add].move4.ename + "\nDanno:" + listPokemonSelezionatiConMosse[indexMio + add].move4.power.ToString() + "\nPP:" + listPokemonSelezionatiConMosse[indexMio + add].move4.PP.ToString();
            }
        }

        private void aggColorHPBar(int player)//1 siamo noi 2 l'avversario
        {
            double valTemp;
            switch (player)
            {
                default:
                    break;
                case 1:
                    valTemp = (100 * wpfLotta.pBNostra.Value) / wpfLotta.pBNostra.Maximum;
                    if (valTemp >= 50)
                    {
                        wpfLotta.pBNostra.Foreground = Brushes.LightGreen;
                    }
                    else if (valTemp < 50 && valTemp >= 20)
                    {
                        wpfLotta.pBNostra.Foreground = Brushes.Orange;
                    }
                    else if (valTemp < 20)
                    {
                        wpfLotta.pBNostra.Foreground = Brushes.Red;
                    }
                    break;
                case 2:
                    valTemp = (100 * wpfLotta.pBAvversario.Value) / wpfLotta.pBAvversario.Maximum;
                    if (valTemp >= 50)
                    {
                        wpfLotta.pBAvversario.Foreground = Brushes.LightGreen;
                    }
                    else if (valTemp < 50 && valTemp >= 20)
                    {
                        wpfLotta.pBAvversario.Foreground = Brushes.Orange;
                    }
                    else if (valTemp < 20)
                    {
                        wpfLotta.pBAvversario.Foreground = Brushes.Red;
                    }
                    break;
            }
        }
        public void aggPokemonAvv(int danno, int id)
        {
            wpfLotta.Dispatcher.Invoke(() =>
            {
                CPokemon pokemonn = new CPokemon();
                if (id != -1)
                {
                    pokemonn = searchListPokemon(id);
                }
                int valTemp = -1;
                if (danno != 1)
                {
                    valTemp = (int)wpfLotta.pBAvversario.Value - danno;
                }

                if (valTemp > 0 && danno != -1)
                {
                    int temp = (int)wpfLotta.pBAvversario.Value;
                    wpfLotta.pBAvversario.Value = temp - danno;
                    wpfLotta.lblHPAvversario.Content = (temp - danno).ToString() + "/" + wpfLotta.pBAvversario.Maximum;
                    aggColorHPBar(2);
                }
                else if ((id != -1 || valTemp <= 0) && contPokAvv < 6)
                {
                    wpfLotta.lblHPAvversario.Content = pokemonn.HP + "/" + pokemonn.HP;
                    wpfLotta.pBAvversario.Foreground = Brushes.LightGreen;
                    BitmapImage bitimg = new BitmapImage();
                    bitimg.BeginInit();
                    bitimg.UriSource = new Uri(@"Properties/" + pokemonn.hires, UriKind.RelativeOrAbsolute);
                    bitimg.EndInit();
                    wpfLotta.imgPokemonA.Source = bitimg;
                    wpfLotta.pBAvversario.Maximum = pokemonn.HP;
                    wpfLotta.pBAvversario.Value = pokemonn.HP;
                    if (id != -1)
                    {
                        switch (contPokAvv)
                        {
                            case -1:
                                wpfLotta.lblPokemon1Avv.Background = Brushes.Green;
                                break;
                            case 0:
                                wpfLotta.lblPokemon1Avv.Background = Brushes.Red;
                                wpfLotta.lblPokemon2Avv.Background = Brushes.Green;
                                break;
                            case 1:
                                wpfLotta.lblPokemon2Avv.Background = Brushes.Red;
                                wpfLotta.lblPokemon3Avv.Background = Brushes.Green;
                                break;
                            case 2:
                                wpfLotta.lblPokemon3Avv.Background = Brushes.Red;
                                wpfLotta.lblPokemon4Avv.Background = Brushes.Green;
                                break;
                            case 3:
                                wpfLotta.lblPokemon4Avv.Background = Brushes.Red;
                                wpfLotta.lblPokemon5Avv.Background = Brushes.Green;
                                break;
                            case 4:
                                wpfLotta.lblPokemon5Avv.Background = Brushes.Red;
                                wpfLotta.lblPokemon6Avv.Background = Brushes.Green;
                                break;
                            case 5:
                                wpfLotta.lblPokemon6Avv.Background = Brushes.Red;
                                break;
                        }
                        contPokAvv++;
                    }
                }
            });
        }
        public void loadDataFromJSON()
        {
            //leggiamo dati da file JSON
            StreamReader r = new StreamReader("moves.json");
            string jsonString = r.ReadToEnd();
            listMosse = JsonConvert.DeserializeObject<List<CMoves>>(jsonString);
            for (int i = 0; i < listMosse.Count; i++)
            {
                if (listMosse[i].power == 0)
                {
                    listMosse.RemoveAt(i);
                    i--;
                }
                if (!tipiPokemon.Contains(listMosse[i].type))
                {
                    tipiPokemon.Add(listMosse[i].type);
                }
            }
            foreach(CMoves move in listMosse)
            {
                if(move.power == 0)
                {
                    Console.WriteLine(move.id + " " + move.ename);
                }
            }
            r = new StreamReader("pokemon.json");
            jsonString = r.ReadToEnd();
            listPokemon = JsonConvert.DeserializeObject<List<CPokemon>>(jsonString);
            //inizializzo liste
            listPokemonSelezionati = new List<CPokemon>();
            foreach (CPokemon pokemon in listPokemon)
            {
                pokemon.HP = pokemon.HP * 3;
            }
        }
        /*Prendiamo le mosse in base al tipo del pokemon*/
        public void assegnaMosse()
        {
            listPokemonSelezionatiConMosse = new List<CPokemonConMosse>();
            List<CMoves> listMosseTipo = new List<CMoves>();
            Random r = new Random();
            List<CMoves> t;
            indexMio = 0;
            for (int i = 0; i < listPokemonSelezionati.Count; i++)
            {
                t = new List<CMoves>();
                listMosseTipo.Clear();
                for (int j = 0; j < listMosse.Count; j++)
                {
                    if (listPokemonSelezionati[i].type1 == listMosse[j].type || listPokemonSelezionati[i].type2 == listMosse[j].type)
                    {
                        listMosseTipo.Add(listMosse[j]);
                    }
                }
                for (int h = 0; h < 4; h++)
                {
                    CMoves m = listMosseTipo[r.Next(0, listMosseTipo.Count)];
                    if (!t.Contains(m))
                    {
                        t.Add(m);
                    }
                    else
                    {
                        h--;
                    }
                }
                CPokemonConMosse tempPokemon = new CPokemonConMosse(listPokemonSelezionati[i], t[0], t[1], t[2], t[3]);
                listPokemonSelezionatiConMosse.Add(tempPokemon);
            }
        }
        public int togliPP(int nMossa)
        {
            int ok = 2; //0=va bene    1=pp finiti ma ultima mossa     2=pp finiti
            if (indexMio < 6)
            {
                switch (nMossa)
                {
                    case 1:
                        if (listPokemonSelezionatiConMosse[indexMio].move1.PP - 1 >= 0)
                        {
                            if (listPokemonSelezionatiConMosse[indexMio].move1.PP - 1 == 0)
                                ok = 1;
                            else
                                ok = 0;
                            listPokemonSelezionatiConMosse[indexMio].move1.PP -= 1;
                        }
                        break;

                    case 2:
                        if (listPokemonSelezionatiConMosse[indexMio].move2.PP - 1 >= 0)
                        {
                            if (listPokemonSelezionatiConMosse[indexMio].move2.PP - 1 == 0)
                                ok = 1;
                            else
                                ok = 0;
                            listPokemonSelezionatiConMosse[indexMio].move2.PP -= 1;
                        }
                        break;

                    case 3:
                        if (listPokemonSelezionatiConMosse[indexMio].move3.PP - 1 >= 0)
                        {
                            if (listPokemonSelezionatiConMosse[indexMio].move3.PP - 1 == 0)
                                ok = 1;
                            else
                                ok = 0;
                            listPokemonSelezionatiConMosse[indexMio].move3.PP -= 1;
                        }
                        break;

                    case 4:
                        if (listPokemonSelezionatiConMosse[indexMio].move4.PP - 1 >= 0)
                        {
                            if (listPokemonSelezionatiConMosse[indexMio].move4.PP - 1 == 0)
                                ok = 1;
                            else
                                ok = 0;
                            listPokemonSelezionatiConMosse[indexMio].move4.PP -= 1;
                        }
                        break;
                }
                aggButton(0);
            }
            return ok;
        }
        public int trovaPokemonPerNome(string nome)
        {
            int pos = -1;
            foreach (CPokemon pokemon in listPokemon)
            {
                if (pokemon.name.Equals(nome))
                {
                    pos = listPokemon.IndexOf(pokemon);
                }
            }
            return pos;
        }
        public void setWpfLotta(Lotta wpfLotta)
        {
            this.wpfLotta = wpfLotta;
        }

        public void setDatiConnessione(DatiCondivisi datiConnessione)
        {
            this.datiConnessione = datiConnessione;
        }
        //get e set di turno con lock
        private readonly object turnoLock = new object();
        public void setTurno(bool b)
        {
            lock (turnoLock)
            {
                mioTurno = b;
            }
        }
        public Boolean getTurno()
        {
            return mioTurno;
        }
    }
    public class CPokemon
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type1 { get; set; }
        public string type2 { get; set; }
        public int HP { get; set; }
        public string description { get; set; }
        public string sprite { get; set; }
        public string hires { get; set; }

        public override string ToString()
        {
            return name + " " + HP + "hp";
        }
    }
    public class CMoves
    {
        public string accuracy { get; set; }
        public string ename { get; set; }
        public int id { get; set; }
        public int power { get; set; }
        public string type { get; set; }
        public int PP { get; set; }

        public CMoves()
        {

        }

    }
    public class CPokemonConMosse
    {
        public CPokemon pokemonScelto { get; set; }
        public CMoves move1 { get; set; }
        public CMoves move2 { get; set; }
        public CMoves move3 { get; set; }
        public CMoves move4 { get; set; }

        public CPokemonConMosse(CPokemon pokemonScelto, CMoves move1, CMoves move2, CMoves move3, CMoves move4)
        {
            this.pokemonScelto = pokemonScelto;
            this.move1 = move1;
            this.move2 = move2;
            this.move3 = move3;
            this.move4 = move4;
        }
    }

}
