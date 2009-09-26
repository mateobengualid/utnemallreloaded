using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UtnEmall.Server.Core;
using System.Diagnostics;

namespace UtnEmall.Server.WpfCore
{
    /// <summary>
    /// Servicio para encontrar dispositivos móviles de clientes
    /// </summary>
    class DiscoveryService
    {
        #region Nested Classes and Structures

        /// <summary>
        /// Enumeración de mensajes de red de bajo nivel
        /// </summary>
        private enum MessagePurpose : byte
        {
            AskingForTime = 1, 
            AskForPassword = 2, 
            AskingForAuthentication = 3, 
            TimeGranted = 4, 
            PasswordAccepted = 5, 
            PasswordRejected = 6
        }

        #endregion

        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        private const int discoveryPort = 20145;

        #endregion

        #region Instance Variables and Properties
        // Socket de servidor
        private Socket serverSocket;
        // End point
        private EndPoint serverEndPoint;
        // Enlace a objeto de administración de sesión.
        private SessionManager sessionManager;

        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Crea una nueva instancia del servicio.
        /// </summary>
        /// <param name="session">Instancia de administrador de sesión</param>
        public DiscoveryService(SessionManager session)
        {
            sessionManager = session;

            // Crea y enlaza el puedto UDP
            serverEndPoint = new IPEndPoint(IPAddress.Any, discoveryPort);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverSocket.Bind(serverEndPoint);
            serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods
        /// <summary>
        /// Comienza escucha para dispositivos móviles
        /// </summary>
        public void ListenForMobileDevices()
        {
            EndPoint clientEndPoint;
            byte[] inputBytes;
            byte[] userIDBytes;
            string mobileID;

            //  Loop while the application is running.
            //
            while (true)
            {
                clientEndPoint = new IPEndPoint(IPAddress.Any, discoveryPort);
                inputBytes = new byte[512];

                try
                {
                    // Bloquea el puerto 20145 hasta que se reciben datos desde alguna dirección IP.
                    serverSocket.ReceiveFrom(inputBytes, inputBytes.Length, SocketFlags.None, ref clientEndPoint);
                    // String de finalización
                    inputBytes[inputBytes.Length - 1] = 0;

                    // Verifica si el mensaje tiene por lo menos una byte.
                    if (inputBytes.Length >= 2)
                    {
                        // Obtiene el cliente IP y el mensaje
                        IPEndPoint clientIPEndPoint = (IPEndPoint)clientEndPoint;
                        MessagePurpose purpose = (MessagePurpose)inputBytes[0];

                        // Decide si el mensaje requiere más tiempo para autenticación
                        switch (purpose)
                        {
                            case MessagePurpose.AskingForTime:
                                Debug.WriteLine("Received message asking for time from " + clientIPEndPoint.Address + ":" + clientIPEndPoint.Port + " at " + DateTime.Now);

                                // Extrae el id de dispositivo móvil
                                userIDBytes = new byte[inputBytes.Length - 1];
                                System.Array.Copy(inputBytes, 1, userIDBytes, 0, userIDBytes.Length);
                                mobileID = Encoding.ASCII.GetString(userIDBytes).Trim('\0');

                                // Verifica que el dispositivo está autenticado.
                                bool existsMobile = sessionManager.MobileHasAuthenticated(mobileID);

                                if (existsMobile)
                                {
                                    bool mobileHasRenewed = sessionManager.RenewTimestamp(mobileID);

                                    // Realiza una renovación y verifica si se realiza correctamente
                                    if (mobileHasRenewed)
                                    {
                                        // Informa al dispositivo sobre la extensión de tiempo.
                                        this.SendRaisedTimeFrameMessage(clientEndPoint, mobileID);
                                    }
                                    else
                                    {
                                        // Informa al dispositivo de la necesidad de autenticación
                                        this.SendPasswordRequiredMessage(clientEndPoint, mobileID);
                                    }
                                }
                                else
                                {
                                    // Informa al dispositivo de la necesidad de autenticación
                                    this.SendPasswordRequiredMessage(clientEndPoint, mobileID);
                                }
                                break;

                            case MessagePurpose.AskingForAuthentication:
                                Debug.WriteLine("Received message asking for authentication from " + clientIPEndPoint.Address + ":" + clientIPEndPoint.Port + " at " + DateTime.Now);

                                // Verifica la longitud mínima
                                if (inputBytes.Length < 22) break;

                                // Extrae la contraseña del mensaje
                                byte[] passwordHashBytes = new byte[20];
                                System.Array.Copy(inputBytes, 1, passwordHashBytes, 0, 20);

                                // Extrae el id de dispositivo
                                userIDBytes = new byte[inputBytes.Length - 20 - 1];
                                System.Array.Copy(inputBytes, 20 + 1, userIDBytes, 0, userIDBytes.Length);
                                mobileID = Encoding.ASCII.GetString(userIDBytes).Trim('\0');

                                // Realiza el proceso de autenticación
                                bool hadLoggedIn = !String.IsNullOrEmpty(sessionManager.ValidateLogin(mobileID, passwordHashBytes, true));

                                if (hadLoggedIn)
                                {
                                    this.SendAcceptedPasswordMessage(clientEndPoint, sessionManager.LoggedMobiles[mobileID]);
                                }
                                else
                                {
                                    // Informa al dispositivo del inicio de sesión fallido
                                    this.SendRejectedPasswordMessage(clientEndPoint, mobileID);
                                }
                                break;

                        }
                    }
                }
                catch (InvalidCastException castError)
                {
                    Debug.WriteLine(castError.Message);
                }
                catch (SocketException socketError)
                {
                    Debug.Write("Error: " + socketError.ErrorCode + " : " + socketError.Message);
                }
            }
        }

        #endregion

        #region Protected and Private Instance Methods

        private void SendAcceptedPasswordMessage(EndPoint clientEndPoint, string SessionString)
        {
            Debug.WriteLine("Sending \"Password Accepted\" to " + SessionString);
            SendVariableStringMessage(clientEndPoint, MessagePurpose.PasswordAccepted, SessionString);
        }

        private void SendRejectedPasswordMessage(EndPoint clientEndPoint, string UserId)
        {
            Debug.WriteLine("Sending \"Password Rejected\" to " + UserId);
            SendVariableStringMessage(clientEndPoint, MessagePurpose.PasswordRejected, UserId);
        }

        private void SendPasswordRequiredMessage(EndPoint clientEndPoint, string UserId)
        {
            Debug.WriteLine("Sending \"Ask for Password\" to " + UserId);
            SendVariableStringMessage(clientEndPoint, MessagePurpose.AskForPassword, UserId);
        }

        private void SendRaisedTimeFrameMessage(EndPoint clientEndPoint, string UserId)
        {
            Debug.WriteLine("Sending \"Time Granted\" to " + UserId);
            SendVariableStringMessage(clientEndPoint, MessagePurpose.TimeGranted, UserId);
        }

        /// <summary>
        /// Este método es invocado por métodos que envían cualquier tipo de mensaje al dispositivo
        /// </summary>
        /// <param name="clientEndPoint"></param>
        /// <param name="Purpose"></param>
        /// <param name="StringToSend"></param>
        private void SendVariableStringMessage(EndPoint clientEndPoint, MessagePurpose Purpose, string StringToSend)
        {
            byte[] stringPieceBytes = Encoding.ASCII.GetBytes(StringToSend);
            byte[] messageBytes = new byte[stringPieceBytes.Length + 1];

            // Ensamblar el mensaje
            messageBytes[0] = (byte)Purpose;
            System.Array.Copy(stringPieceBytes, 0, messageBytes, 1, stringPieceBytes.Length);

            // Enviar el mensaje
            serverSocket.SendTo(messageBytes, messageBytes.Length, SocketFlags.None, clientEndPoint);
        }

        #endregion

        #endregion
    }
}
