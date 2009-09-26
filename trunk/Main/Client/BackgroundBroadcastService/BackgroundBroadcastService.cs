using System;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using UtnEmall.Client.EntityModel;
using System.Diagnostics;

namespace UtnEmall.Client.BackgroundBroadcastService
{
    /// <summary>
    /// Enumeración de los estados posibles para el protocolo de comunicación.
    /// </summary>
    enum MessagePurpose : byte
    {
        AskingForTime = 1,
        AskForPassword = 2,
        AskingForAuthentication = 3,
        TimeGranted = 4,
        PasswordAccepted = 5,
        PasswordRejected = 6
    }

    /// <summary>
    /// Implementa la comunicación de bajo nivel con el UTN Mobile Mall Server usando sockets UDP/TCP 
    /// </summary>
    public class BackgroundBroadcast : IDisposable
    {
        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        private static int portServer;
        private static int pingTime;
        private static string ipServer;

        private static BackgroundBroadcast singleton;
        /// <summary>
        /// Obtiene la instancia para el BackgroundBroadcast.
        /// </summary>
        /// <returns>La instancia singleton del BackgroundBroadcast.</returns>
        public static BackgroundBroadcast Instance
        {
            get
            {
                return singleton;
            }
        }

        public const string DefaultServerPort = "20145";
        public const string DefaultPingTime = "15000";
        #endregion

        #region Instance Variables and Properties

        private int tryCount;
        private IPEndPoint ipEndPointServer;
        private UdpClient udpClient;
        private Thread serverListenerThread;
        private Thread serverSenderThread;
        private bool disposed;
        private CustomerEntity customer;

        public CustomerEntity Customer
        {
            get 
            { 
                return customer; 
            }
            set 
            { 
                customer = value; 
            }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Crea un cliente para enviar y recibir datos desde el servidor. Crea dos hilos separados.
        /// </summary>
        /// <param name="ipServerAddress">Dirección IP del server.</param>
        /// <param name="portServerNumber">Puerto.</param>
        /// <param name="pingTimeAlive">Intervalo entre consultas con el servidor para mantener estado.</param>
        /// <param name="customer">El cliente que esta usando la configuración</param>
        public BackgroundBroadcast(string ipServerAddress, int portServerNumber, int pingTimeAlive, CustomerEntity customer)
        {
            ipServer = ipServerAddress;
            portServer = portServerNumber;
            pingTime = pingTimeAlive;
            Customer = customer;
            
            CreateClient();

            // Iniciar un hilo para escuchar al servidor
            serverListenerThread = new Thread(new ThreadStart(this.ListenForServerMessages));
            serverListenerThread.IsBackground = true;
            serverListenerThread.Start();

            // Comenzar un hilo para mandar datos al servidor
            serverSenderThread = new Thread(new ThreadStart(this.SendMessagesToServer));
            serverSenderThread.IsBackground = true;
            serverSenderThread.Start();
        }

        #endregion

        #region Static Methods
        /// <summary>
        /// Crea una instancia de BackgroundBroadcastService si no esta actualmente activo.
        /// </summary>
        /// <param name="ipServer">Dirección IP del servidor.</param>
        /// <param name="portServer">Puerto</param>
        /// <param name="pingTime">Tiempo entre consultas para mantener estado.</param>
        /// <param name="customer">El cliente asociado al dispositivo.</param>
        public static void Run(string ipServer, int portServer, int pingTime, CustomerEntity customer)
        {
            if (singleton == null)
            {
                singleton = new BackgroundBroadcast(ipServer, portServer, pingTime, customer);
            }
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods
        /// <summary>
        /// Aborta los hilos de segundo plano y cierra el cliente UDP
        /// </summary>
        public void Terminate()
        {
            if (singleton != null)
            {
                serverSenderThread.Abort();
                serverListenerThread.Abort();
                udpClient.Close();
                udpClient = null;
                ipEndPointServer = null;
                singleton = null;
            }
        }

        /// <summary>
        /// Metodo para enviar mensajes al servidor.
        /// </summary>
        public void SendMessagesToServer()
        {
            // Enviar mensajes mientras la aplicación esta corriendo
            while (true)
            {
                if (UtnEmall.Client.SmartClientLayer.Connection.IsConnected)
                {
                    // Mantener la conexión activa, una vez que se ha logueado
                    SendTimeRequestMessage();
                }
                else
                {
                    // Intentar loguearse
                    SendAuthenticationRequestMessage();
                }

                // Esperar algunos segundos antes de enviar el siguiente mensaje
                Thread.Sleep(pingTime);
            }
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Crea un cliente UDP
        /// </summary>
        private void CreateClient()
        {
            if (ipEndPointServer == null && udpClient == null)
            {
                ipEndPointServer = new IPEndPoint(IPAddress.Parse(ipServer), portServer);

                // El constructo asigna un número de puerto local arbitrario
                udpClient = new UdpClient(portServer);
                udpClient.Connect(ipEndPointServer);
            }
        }

        /// <summary>
        /// Metodo que recibe los datos desde el servidor
        /// </summary>
        private void ListenForServerMessages()
        {
            // IPEndPoint nos permite leer datagramas de cualquier origen.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse(ipServer), portServer);
            // IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] receiveBytes;

            // repetir mientras la aplicacion esta corriendo
            while(true)
            {
                try
                {
                    // bloquear mientras esperamos respuesta desde el host remoto
                    receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    MessagePurpose purpose = (MessagePurpose)receiveBytes[0];

                    // decidir si el mensaje es para pedir más tiempo o para autenticar
                    switch (purpose)
                    {
                        case MessagePurpose.AskingForTime:
                            // no debería pasar esto
                            Debug.WriteLine("WARNING : Server Ask for time.");
                            break;
                        case MessagePurpose.AskForPassword:
                            Debug.WriteLine("Server Ask for password");
                            break;

                        case MessagePurpose.AskingForAuthentication:
                            Debug.WriteLine("Server asked for authentication");
                            break;

                        case MessagePurpose.TimeGranted:
                            Debug.WriteLine("More time granted");
                            // renovar el contador de intentos fallidos
                            tryCount = 0;
                            break;

                        case MessagePurpose.PasswordAccepted:
                            Debug.WriteLine("Password accepted");
                            byte[] sessionArray = new byte[receiveBytes.Length - 1];
                            System.Array.Copy(receiveBytes, 1, sessionArray, 0, receiveBytes.Length - 1);

                            UtnEmall.Client.SmartClientLayer.Connection.Session = Encoding.ASCII.GetString(sessionArray, 0, sessionArray.Length);
                            UtnEmall.Client.SmartClientLayer.Connection.IsConnected = true;
                            if (ConnectionStateChanged != null)
                            {
                                this.ConnectionStateChanged(this, null);
                            }
                            break;

                        case MessagePurpose.PasswordRejected:
                            Debug.WriteLine("Password rejected");
                            break;
                    }
                }
                catch (InvalidCastException castError)
                {
                    Debug.WriteLine(castError.Message);
                }
                catch (SocketException socketException)
                {
                    udpClient.Close();
                    udpClient = null;
                    ipEndPointServer = null;

                    CreateClient();

                    Debug.WriteLine("SocketException: " + socketException.Message);

                    UtnEmall.Client.SmartClientLayer.Connection.IsConnected = false;
                    if (ConnectionStateChanged != null)
                    {
                        this.ConnectionStateChanged(this, null);
                    }
                }
                catch (FormatException formatException)
                {
                    Debug.WriteLine("FormatException: " + formatException.Message);

                    UtnEmall.Client.SmartClientLayer.Connection.IsConnected = false;
                    if (ConnectionStateChanged != null)
                    {
                        this.ConnectionStateChanged(this, null);
                    }
                }
            }
        }

        /// <summary>
        /// Enviar una solicitud de más tiempo con el nombre de usuario
        /// </summary>
        private void SendTimeRequestMessage()
        {
            SendData(MessagePurpose.AskingForTime, Customer.UserName);
            tryCount++;
            // Intentar tres veces
            if (tryCount > 3)
            {
                tryCount = 0;
                UtnEmall.Client.SmartClientLayer.Connection.IsConnected = false;
                if (ConnectionStateChanged != null)
                {
                    this.ConnectionStateChanged(this, null);
                }
            }
        }

        private void SendAuthenticationRequestMessage()
        {
            SendData(MessagePurpose.AskingForAuthentication, System.Convert.FromBase64String(Customer.Password), Customer.UserName);
        }

        /// <summary>
        /// Envia un mensaje al server para renovar el tiempo
        /// </summary>
        /// <param name="mobile">Identificador del móvil.</param>
        /// <param name="message">Mensaje a transmitir.</param>
        private void SendData(MessagePurpose message, string mobile)
        {
            Send(Convert.ToChar((byte)message) + mobile);
        }

        /// <summary>
        /// Envia un mensaje al servidor para autenticar el inicio de seción.
        /// </summary>
        /// <param name="mobile">Identificador del móvil.</param>
        /// <param name="message">Mensaje a transmitir.</param>
        /// <param name="clientPassword">Clave del cliente móvil.</param>
        private void SendData(MessagePurpose message, byte[] clientPassword, string mobile)
        {
            byte[] messageBytes = new byte[1 + clientPassword.Length + Encoding.ASCII.GetByteCount(mobile)];

            messageBytes[0] = (byte)message;
            Array.Copy(clientPassword, 0, messageBytes, 1, clientPassword.Length);
            Array.Copy(Encoding.ASCII.GetBytes(mobile), 0, messageBytes, clientPassword.Length + 1, Encoding.ASCII.GetByteCount(mobile));
            Send(messageBytes);
        }

        /// <summary>
        /// Envia datos al servidor.
        /// </summary>
        /// <param name="data">Datos a enviar.</param>
        private void Send(string data)
        {
            try
            {
                // Envia un mensaje al server.
                byte[] sendBytes = Encoding.ASCII.GetBytes(data);
                if(udpClient!=null)
                    udpClient.Send(sendBytes, sendBytes.Length);
            }
            catch (SocketException socketException)
            {
                Console.WriteLine("Socket Exception: An error occurred when accessing the socket. " + socketException.Message);
            }
        }

        /// <summary>
        /// Envia datos al servidor.
        /// </summary>
        /// <param name="data">Datos a enviar.</param>
        private void Send(byte[] data)
        {
            try
            {
                // Envia un mensaje al server.
                if (udpClient != null)
                    udpClient.Send(data, data.Length);
            }
            catch (SocketException socketException)
            {
                Console.WriteLine("Socket Exception: An error occurred when accessing the socket. " + socketException.Message);
            }
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler ConnectionStateChanged;

        #endregion

        #region IDisposable Members
        // Implementa IDisposable
        //No marcar como virtual el metodo, las clases derivadas deberían sobreescribir
        public void Dispose()
        {
            Dispose(true);
            // Éste objeto sera elinimado por el metodo Dispose
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Limpia los recursos
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            // Controla si Dispose ya fue llamado
            if (!this.disposed)
            {
                // Si disposing es true eliminar todos los recursos
                if (disposing)
                {
                    // cerrar el cliente udp
                    if (udpClient != null) udpClient.Close();
                    // terminar los hilos
                    if (serverListenerThread != null)
                        serverListenerThread.Abort();
                    if (serverSenderThread != null)
                        serverSenderThread.Abort();
                }
            }
            disposed = true;
        }

       ~BackgroundBroadcast()      
        {
            // Llamar a Dispose
            Dispose(false);
        }

        #endregion
    }
}