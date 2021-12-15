using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace pokemon_showdown_p2p
{
    public class DatiCondivisi
    {
        public UdpClient udpClient;
        public IPEndPoint RemoteIpEndPoint;
        //informazioni di questo peer
        public String ip_peer { get; set; }
        public int port_peer { get; set; }
        public String nome_peer { get; set; }
        public String codUtente { get; set; }
        //info peer connesso
        public String ip_peer_connesso { get; set; }
        public int port_peer_connesso { get; set; }
        public String nome_peer_connesso { get; set; }
        public String codUtente_connesso { get; set; }
        public bool connesso;
        public DatiCondivisi()
        {
            udpClient = new UdpClient(50002); //porta non registrata
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            connesso = false;

            Thread ThreadRiceviConnessione = new Thread(riceviConnessione);
        }

        public String ricevi()
        {
            Byte[] receive_data = new byte[1500];
            String to_split;
            //ricevere il pacchetto, ricomporlo e metterlo in una stringa
            if (connesso) //controllo per sicurezza che sia connesso con qualcuno
            {
                receive_data = udpClient.Receive(ref RemoteIpEndPoint);
                to_split = Encoding.ASCII.GetString(receive_data);
            }
            else
            {
                to_split = "Peer non connesso";
            }
            return to_split;
        }

        public void riceviConnessione()
        {
            Byte[] receive_data = new byte[1500];
            Byte[] send_data;
            String to_split;
            String[] splitted;
            if (!connesso)
            {
                receive_data = udpClient.Receive(ref RemoteIpEndPoint);
                to_split = Encoding.ASCII.GetString(receive_data); //c;indirizzo;porta;nome;codiceUtente
                splitted = to_split.Split(';');
                ip_peer_connesso = splitted[1];
                port_peer_connesso = Int32.Parse(splitted[2]);
                nome_peer_connesso = splitted[3];
                codUtente_connesso = splitted[4];
                connesso = true;

                //invio conferma connessione
                send_data = Encoding.ASCII.GetBytes("c;" + ip_peer + ";" + port_peer.ToString() + ";" + nome_peer + ";" + codUtente); //c;indirizzo;porta;nome;codiceUtente
                Console.WriteLine("Ricevuto");
            }
            else
            {
                send_data = Encoding.ASCII.GetBytes("c;e");
                Console.WriteLine("Non connesso");
            }

            udpClient.Connect(ip_peer_connesso, port_peer_connesso);
            udpClient.Send(send_data, send_data.Length);

        }

        public void inviaConnessione()
        {
            Byte[] receive_data = new byte[1500];
            String to_split;
            String[] splitted;

            if (!connesso)
            {
                Byte[] send_data = Encoding.ASCII.GetBytes("c;" + ip_peer + ";" + port_peer.ToString() + ";" + nome_peer + ";" + codUtente); //c;indirizzo;porta;nome;codiceUtente
                udpClient.Connect(ip_peer_connesso, port_peer_connesso);
                udpClient.Send(send_data, send_data.Length);

                //ascoltare la risposta
                receive_data = udpClient.Receive(ref RemoteIpEndPoint);
                to_split = Encoding.ASCII.GetString(receive_data); //c;indirizzo;porta;nome;codiceUtente
                splitted = to_split.Split(';');
                if (to_split[1].Equals("e"))
                    Console.WriteLine("Errore");
                    
            }
        }

        public List<CMoves> listMosse { get; set; }
        public List<CPokemon> listPokemon { get; set; }
        public List<CPokemon> listPokemonSelezionati { get; set; }
        public void loadDataFromJSON()
        {
            StreamReader r = new StreamReader("moves.json");
            string jsonString = r.ReadToEnd();
            listMosse = JsonConvert.DeserializeObject<List<CMoves>>(jsonString);

            r = new StreamReader("pokemon.json");
            jsonString = r.ReadToEnd();
            listPokemon = JsonConvert.DeserializeObject<List<CPokemon>>(jsonString);

            listPokemonSelezionati = new List<CPokemon>();
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

    }

    public class CTypes
    {
        public string english { get; set; }
        public string ineffective0 { get; set; }
        public string ineffective1 { get; set; }
        public string ineffective2 { get; set; }
        public string ineffective3 { get; set; }
        public string ineffective4 { get; set; }
        public string ineffective5 { get; set; }
        public string ineffective6 { get; set; }
        public string no_effect0 { get; set; }
        public string effective0 { get; set; }
        public string effective1 { get; set; }
        public string effective2 { get; set; }
        public string effective3 { get; set; }
        public string effective4 { get; set; }


    }
}
