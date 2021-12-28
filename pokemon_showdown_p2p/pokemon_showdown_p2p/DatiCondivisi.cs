using System;
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
        public Peer_questo peerQuesto { get; set; }
        //info peer connesso
        public Peer_connesso peerConnesso { get; set; }
        
        public bool connesso;
        //info ricezione
        public string[] risAscolto { get; set; }
        public DatiCondivisi()
        {
            peerQuesto = new Peer_questo();
            peerConnesso = new Peer_connesso();
            //risAscolto[1] = "nulla";
            udpClient = new UdpClient(peerQuesto.port_peer); //porta non registrata
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            connesso = false;

            Thread ThreadRiceviConnessione = new Thread(riceviConnessione);
        }

        public void ricevi()
        {
            Byte[] receive_data;
            String to_split;
            string[] v;
            do
            {
                receive_data = new byte[1500];
                //ricevere il pacchetto, ricomporlo e metterlo in una stringa
                receive_data = udpClient.Receive(ref RemoteIpEndPoint);
                to_split = Encoding.ASCII.GetString(receive_data);
                v = to_split.Split(';');
                if (Int32.Parse(v[0]) == peerConnesso.port_peer_connesso)
                {
                    risAscolto = v;
                }
            } while (true); //da cambiare la condizione
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
                to_split = Encoding.ASCII.GetString(receive_data); //c;indirizzo;porta;nome
                splitted = to_split.Split(';');
                peerConnesso.ip_peer_connesso = splitted[1];
                peerConnesso.port_peer_connesso = Int32.Parse(splitted[2]);
                peerConnesso.nome_peer_connesso = splitted[3];
                connesso = true;

                //invio conferma connessione
                send_data = Encoding.ASCII.GetBytes("c;" + peerQuesto.ip_peer + ";" + peerQuesto.port_peer.ToString() + ";" + peerQuesto.nome_peer); //c;indirizzo;porta;nome
                Console.WriteLine("Ricevuto");
            }
            else
            {
                send_data = Encoding.ASCII.GetBytes("c;e");
                Console.WriteLine("Non connesso");
            }

            udpClient.Connect(peerConnesso.ip_peer_connesso, peerConnesso.port_peer_connesso);
            udpClient.Send(send_data, send_data.Length);

        }

        public void inviaConnessione()
        {
            Byte[] receive_data = new byte[1500];
            String to_split;

            if (!connesso)
            {
                Byte[] send_data = Encoding.ASCII.GetBytes("c;" + peerQuesto.ip_peer + ";" + peerQuesto.port_peer.ToString() + ";" + peerQuesto.nome_peer); //c;indirizzo;porta;nome
                udpClient.Connect(peerConnesso.ip_peer_connesso, peerConnesso.port_peer_connesso);
                udpClient.Send(send_data, send_data.Length);

                //ascoltare la risposta
                receive_data = udpClient.Receive(ref RemoteIpEndPoint);
                to_split = Encoding.ASCII.GetString(receive_data); //c;indirizzo;porta;nome;codiceUtente
                risAscolto = to_split.Split(';');
                connesso = true;
            }
        }

        public void manda(string s)
        {
            Byte[] send_data = Encoding.ASCII.GetBytes(peerConnesso.port_peer_connesso + ";" + s);
            udpClient.Connect(peerConnesso.ip_peer_connesso, peerConnesso.port_peer_connesso);
            udpClient.Send(send_data, send_data.Length);
        }
    }

    public class Peer_connesso
    {
        public String ip_peer_connesso { get; set; }
        public int port_peer_connesso { get; set; }
        public String nome_peer_connesso { get; set; }

        public Peer_connesso()
        {
            ip_peer_connesso = "";
            port_peer_connesso = 0;
            nome_peer_connesso = "";
        }
    }

    public class Peer_questo
    {
        public String ip_peer { get; set; }
        public int port_peer { get; set; }
        public String nome_peer { get; set; }

        public Peer_questo()
        {
            ip_peer = "localhost";
            port_peer = 50002;
            nome_peer = "";
        }
    }
}
