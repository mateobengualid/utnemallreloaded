using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Diagnostics;
using System.IO;
using System.ServiceModel.Channels;
using System.Reflection;
using System.Security;
using System.Runtime.Serialization;
using UtnEmall.Server.BusinessLogic;
using UtnEmall.Server.ServiceCompiler;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace UtnEmall.Server.Core
{
    /// <summary>
    /// InterfaSe para el servicio de transferencia de archivos usando Ms WFC.
    /// </summary>
    [ServiceContract]
    public interface IFileTransferService
    {
        /// <summary>
        /// Transferir un archivo desde la carpeta assemblies en el servidor.
        /// </summary>
        /// <param name="request">Parámetros de petición</param>
        /// <param name="sessionIdentifier">Sesión de usuario</param>
        /// <returns>La información del archivo</returns>
        [OperationContract]
        RemoteFileInfo DownloadFile(DownloadRequest request, string sessionIdentifier);
    }

    /// <summary>
    /// Esta clase es usada como argumento para el método DownloadFile del servicio de 
    /// transferencia de archivos.
    /// </summary>
    [DataContract]
    public class DownloadRequest
    {
        /// <summary>
        /// El nombre del archivo a bajar desde el servidor.
        /// </summary>
        [DataMember(Order = 1)]
        public string FileName { get; set; }
        /// <summary>
        /// Fecha y hora del archivo en el dispositivo cliente.
        /// </summary>
        [DataMember(Order = 2)]
        public string FileDateTime { get; set; }
    }

    /// <summary>
    /// Clase para ser usada como valor de retorno del método DownloadFile en el servicio de
    /// transferencia de archivos.
    /// </summary>
    [DataContract]
    public class RemoteFileInfo
    {
        /// <summary>
        /// El nombre del archivo de retorno.
        /// </summary>
        [DataMember(Order = 1)]
        public string FileName { get; set; }
        /// <summary>
        /// La longitud en bytes del archivo transferido.
        /// </summary>
        [DataMember(Order = 2)]
        public long Length { get; set; }
        /// <summary>
        /// El contenido en bytes del archivo transferido.
        /// </summary>
        [SuppressMessage("Microsoft.Performance",
                         "CA1819:PropertiesShouldNotReturnArrays",
                         Justification = "This property is used to store raw bytes.")]
        [DataMember(Order = 3)]
        public Byte[] FileByteStream { get; set; }
        /// <summary>
        /// Si el archivo no fue modificado desde la fecha provista por el cliente.
        /// </summary>
        [DataMember(Order = 4)]
        public bool NotChanged { get; set; }
        /// <summary>
        /// La última modificación del archivo.
        /// </summary>
        [DataMember(Order = 5)]
        public string FileDateTime { get; set; }
    }

    /// <summary>
    /// Implementa el servicio IFileTransferService para permitir a los dispositivos clientes 
    /// transferir archivos seleccionados desde el servidor.
    /// </summary>
    public class FileTransferService : IFileTransferService
    {
        /// <summary>
        /// Implementa el contrato WCF DownloadFile.
        /// Lee el archivo desde el repositorio público del servidor de UTNEmall y envía el contenido
        /// como un vector de bytes.
        /// </summary>
        /// <param name="request">Información del archivo solicitado.</param>
        /// <param name="sessionIdentifier">Identificador de sesión.</param>
        /// <returns>El archivo de datos como un vector de bytes.</returns>
        public RemoteFileInfo DownloadFile(DownloadRequest request, string sessionIdentifier)
        {
            if (request == null || request.FileName == null)
            {
                throw new ArgumentException("Invalid request argument.", "request");
            }
            if (sessionIdentifier == null)
            {
                throw new ArgumentException("Invalid session identifier argument.", "sessionIdentifier");
            }
            
            // Verificar el identificador de sesión
            UtnEmall.Server.WpfCore.SessionManager.Instance.IsCustomerSession(sessionIdentifier);

            try
            {

                // obtener información desde el archivo de entrada
                string filePath = System.IO.Path.Combine(ServiceBuilder.AssembliesFolder, request.FileName);
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);

                // verificar la fecha del ensamblado del dispositivo con respecto a la fecha local
                DateTime mobileFileDate = new DateTime(Convert.ToInt64(request.FileDateTime, CultureInfo.InvariantCulture));
                DateTime localFileDate = fileInfo.LastWriteTime;

                if (mobileFileDate.Date >= localFileDate.Date
                    && mobileFileDate.Hour == localFileDate.Hour
                    && mobileFileDate.Minute == localFileDate.Minute)
                {
                    return null;
                }

                // reportar inicio
                Debug.WriteLine("Sending file " + request.FileName + " to client.");
                Debug.WriteLine("File size " + fileInfo.Length + " bytes.");

                // verificar si existe
                if (!fileInfo.Exists)
                {
                    return null;
                }

                // abrir flujo de salida
                System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                RemoteFileInfo result = new RemoteFileInfo();

                // leer el archivo y retornar el resultado
                result.FileName = request.FileName;
                result.Length = fileInfo.Length;
                result.FileByteStream = new Byte[fileInfo.Length];
                result.FileDateTime = fileInfo.LastWriteTime.Ticks.ToString(CultureInfo.InvariantCulture);
                stream.Read(result.FileByteStream, 0, (int)fileInfo.Length);
                stream.Close();
                return result;
            }
            catch (ArgumentException)
            {
                Debug.WriteLine("ArgumentException while reading file on DownloadFile.");
            }
            catch(FileNotFoundException)
            {
                Debug.WriteLine("FileNotFoundException while reading file on DownloadFile.");
            }
            catch(UnauthorizedAccessException)
            {
                Debug.WriteLine("UnauthorizedAccessException while reading file on DownloadFile.");
            }
            catch (IOException ioError)
            {
                Debug.WriteLine("IOException while reading file on DownloadFile. ERROR : " + ioError.Message);
            }
            return null;            
        }
    }
}
