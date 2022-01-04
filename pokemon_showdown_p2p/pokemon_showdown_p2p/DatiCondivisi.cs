﻿using System;
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
        public CPeer peerQuesto { get; set; }
        //info peer connesso
        public CPeer peerConnesso { get; set; }
        
        public bool connesso;
        //info ricezione
        public string[] risAscolto { get; set; }
        public DatiCondivisi()
        {
            peerQuesto = new CPeer("localhost", 666, "Marco");
            peerConnesso = new CPeer("", 0, "");
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
                if (Int32.Parse(v[1]) == peerConnesso.port_peer)
                {
                    risAscolto = v;
                }
            } while (false); //da cambiare la condizione
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
                peerConnesso.ip_peer = splitted[1];
                peerConnesso.port_peer = Int32.Parse(splitted[2]);
                peerConnesso.nome_peer = splitted[3];
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

            udpClient.Connect(peerConnesso.ip_peer, peerConnesso.port_peer);
            udpClient.Send(send_data, send_data.Length);

        }

        public void inviaConnessione()
        {
            Byte[] receive_data = new byte[1500];
            String to_split;

            if (!connesso)
            {
                Byte[] send_data = Encoding.ASCII.GetBytes("c;" + peerQuesto.ip_peer + ";" + peerQuesto.port_peer.ToString() + ";" + peerQuesto.nome_peer); //c;indirizzo;porta;nome
                udpClient.Connect(peerConnesso.ip_peer, peerConnesso.port_peer);
                udpClient.Send(send_data, send_data.Length);

                //ascoltare la risposta
                receive_data = udpClient.Receive(ref RemoteIpEndPoint);
                to_split = Encoding.ASCII.GetString(receive_data); //c;indirizzo;porta;nome;codiceUtente
                risAscolto = to_split.Split(';');
                peerConnesso.ip_peer = risAscolto[1];
                peerConnesso.port_peer = Int32.Parse(risAscolto[2]);
                peerConnesso.nome_peer = risAscolto[3];
                connesso = true;
            }
        }

        public void manda(string s)
        {
            Byte[] send_data = Encoding.ASCII.GetBytes(s);
            udpClient.Connect(peerConnesso.ip_peer, peerConnesso.port_peer);
            udpClient.Send(send_data, send_data.Length);
        }


    }

    public class CPeer
    {
        public String ip_peer { get; set; }
        public int port_peer { get; set; }
        public String nome_peer { get; set; }

        public CPeer(string ip_peer, int port_peer, string nome_peer)
        {
            this.ip_peer = ip_peer; //da cambiare ogni volta
            this.port_peer = port_peer;
            this.nome_peer = nome_peer;
        }
    }
}
