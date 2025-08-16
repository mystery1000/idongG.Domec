using System.Net;
using System.Net.Sockets;

namespace idongG.Domec.PlcDA.ToolManage.Modbus.EasyModbus;

#region TCPHandler class

public class TCPHandler
{
    private TcpListener server = null;

    private List<Client> tcpClientLastRequestList = new List<Client>();

    private IPAddress localIPAddress = IPAddress.Any;

    /// <summary>
    /// idongG添加
    /// </summary>
    public IReadOnlyList<Client> ClientList => tcpClientLastRequestList;

    private void AcceptTcpClientCallback(IAsyncResult asyncResult)
    {
        TcpClient tcpClient = new TcpClient();
        try
        {
            tcpClient = server.EndAcceptTcpClient(asyncResult);
            tcpClient.ReceiveTimeout = 4000;
            if (ipAddress != null)
            {
                string ipEndpoint = tcpClient.Client.RemoteEndPoint.ToString();
                ipEndpoint = ipEndpoint.Split(':')[0];
                if (ipEndpoint != ipAddress)
                {
                    tcpClient.Client.Disconnect(false);
                    return;
                }
            }
        }
        catch (Exception) { }
        try
        {
            server.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
            Client client = new Client(tcpClient);
            NetworkStream networkStream = client.NetworkStream;
            networkStream.ReadTimeout = 4000;
            networkStream.BeginRead(client.Buffer, 0, client.Buffer.Length, ReadCallback, client);
            if (numberOfClientsChanged != null)
                numberOfClientsChanged();
        }
        catch (Exception) { }
    }

    private int GetAndCleanNumberOfConnectedClients(Client client)
    {
        lock (this)
        {
            int i = 0;
            bool objetExists = false;
            foreach (Client clientLoop in tcpClientLastRequestList)
            {
                if (client.Equals(clientLoop))
                    objetExists = true;
            }
            try
            {
                tcpClientLastRequestList.RemoveAll(delegate (Client c)
                {
                    return DateTime.Now.Ticks - c.Ticks > 40000000;
                }

                    );
            }
            catch (Exception) { }
            if (!objetExists)
                tcpClientLastRequestList.Add(client);

            return tcpClientLastRequestList.Count;
        }
    }

    private void ReadCallback(IAsyncResult asyncResult)
    {
        NetworkConnectionParameter networkConnectionParameter = new NetworkConnectionParameter();
        Client client = asyncResult.AsyncState as Client;
        client.Ticks = DateTime.Now.Ticks;
        NumberOfConnectedClients = GetAndCleanNumberOfConnectedClients(client);
        if (numberOfClientsChanged != null)
            numberOfClientsChanged();
        if (client != null)
        {
            int read;
            NetworkStream networkStream = null;
            try
            {
                networkStream = client.NetworkStream;

                read = networkStream.EndRead(asyncResult);
            }
            catch (Exception ex)
            {
                return;
            }

            if (read == 0)
            {
                //OnClientDisconnected(client.TcpClient);
                //connectedClients.Remove(client);
                return;
            }
            byte[] data = new byte[read];
            Buffer.BlockCopy(client.Buffer, 0, data, 0, read);
            networkConnectionParameter.bytes = data;
            networkConnectionParameter.stream = networkStream;
            if (dataChanged != null)
                dataChanged(networkConnectionParameter);
            try
            {
                networkStream.BeginRead(client.Buffer, 0, client.Buffer.Length, ReadCallback, client);
            }
            catch (Exception)
            {
            }
        }
    }

    public class Client
    {
        private readonly TcpClient tcpClient;
        private readonly byte[] buffer;

        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            int bufferSize = tcpClient.ReceiveBufferSize;
            buffer = new byte[bufferSize];
        }

        public long Ticks { get; set; }

        public TcpClient TcpClient
        {
            get { return tcpClient; }
        }

        public byte[] Buffer
        {
            get { return buffer; }
        }

        public NetworkStream NetworkStream
        {
            get
            {
                return tcpClient.GetStream();
            }
        }
    }

    public string ipAddress = null;

    /// <summary>
    /// Listen to all network interfaces.
    /// </summary>
    /// <param name="port">TCP port to listen</param>
    public TCPHandler(int port)
    {
        server = new TcpListener(LocalIPAddress, port);
        server.Start();
        server.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
    }

    /// <summary>
    /// Listen to a specific network interface.
    /// </summary>
    /// <param name="localIPAddress">IP address of network interface to listen</param>
    /// <param name="port">TCP port to listen</param>
    public TCPHandler(IPAddress localIPAddress, int port)
    {
        this.localIPAddress = localIPAddress;
        server = new TcpListener(LocalIPAddress, port);
        server.Start();
        server.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
    }

    public delegate void DataChanged(object networkConnectionParameter);

    public delegate void NumberOfClientsChanged();

    public event DataChanged dataChanged;

    public event NumberOfClientsChanged numberOfClientsChanged;

    public int NumberOfConnectedClients { get; set; }

    /// When making a server TCP listen socket, will listen to this IP address.
    public IPAddress LocalIPAddress
    {
        get { return localIPAddress; }
    }

    public void Disconnect()
    {
        try
        {
            foreach (Client clientLoop in tcpClientLastRequestList)
            {
                clientLoop.NetworkStream.Close(00);
            }
        }
        catch (Exception) { }
        server.Stop();
    }
}

#endregion