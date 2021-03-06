using System;
using System.Net;
using System.Net.Sockets;

namespace Communication.Sockets.Core.Client
{
    public class ClientTerminal
    {
        Socket m_socClient;
        private SocketListener m_listener;

        public event TCPTerminal_MessageRecivedDel MessageRecived;
        public event TCPTerminal_ConnectDel Connected;
        public event TCPTerminal_DisconnectDel Disconncted;

        public void Connect(IPAddress remoteIPAddress, int alPort)
        {
            //create a new client socket ...
            m_socClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint remoteEndPoint = new IPEndPoint(remoteIPAddress, alPort);
            
            // Connect
            do
            {
                try
                {
                    m_socClient.Connect(remoteEndPoint);
                }
                catch (Exception)
                {
                }
            } while (!m_socClient.Connected);
            

            OnServerConnection();
        }

        public void SendMessage(byte[] byData)
        {
            m_socClient.Send(byData);
        }

        public void StartListen()
        {
            if (m_socClient == null)
            {
                return;
            }

            if (m_listener != null)
            {
                return;
            }

            m_listener = new SocketListener();
            m_listener.Disconnected += OnServerConnectionDroped;
            m_listener.MessageRecived += OnMessageRecvied;
            
            m_listener.StartReciving(m_socClient);
        }

        public void Close()
        {
            if (m_socClient == null)
            {
                return;
            }

            if (m_listener != null)
            {
                m_listener.StopListening();
            }

            m_socClient.Close();
            m_listener = null;
            m_socClient = null;
        }

        private void OnServerConnection()
        {
            if (Connected != null)
            {
                Connected(m_socClient);
            }
        }

        private void OnMessageRecvied(byte[] message)
        {
            if (MessageRecived != null)
            {
                MessageRecived(message);
            }
        }

        private void OnServerConnectionDroped(Socket socket)
        {
            RaiseServerDisconnected(socket);
        }

        private void RaiseServerDisconnected(Socket socket)
        {
            if (Disconncted != null)
            {
                Disconncted(socket);
            }
        }


    }
}
