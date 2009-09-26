using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using UtnEmall.Server.EntityModel;

namespace UtnEmall.Proxies
{
    /// <summary>
    /// Resultado de llamada asíncrona de GetDataModel.
    /// </summary>
    public class GetDataModelResult
    {
        public DataModelEntity DataModel
        {
            get;
            set;
        }
    }
    public class SaveDataModelResult
    {
        public bool Success
        {
            get;
            set;
        }
    }
    public class GetCustomerServiceResult
    {
        public CustomerServiceDataEntity CustomerService
        {
            get;
            set;
        }
    }
    public class SaveCustomerServiceResult
    {
        public bool Success
        {
            get;
            set;
        }
    }
    /// <summary>
    /// Callback para el método BeginGetDataModel.
    /// </summary>
    /// <param name="result">Resultado del método BeginGetDataModel.</param>
    public delegate void GetDataModelCallback(GetDataModelResult result);

    /// <summary>
    /// Callback del método BeginSaveDataModel.
    /// </summary>
    /// <param name="result">Resultado del método BeginSaveDataModel.</param>
    public delegate void SaveDataModelCallback(SaveDataModelResult result);

    /// <summary>
    /// Callback del método BeginGetCustomerService.
    /// </summary>
    /// <param name="result">Resultado del método BeginGetCustomerService.</param>
    public delegate void GetCustomerServiceCallback(GetCustomerServiceResult result);

    /// <summary>
    /// Callback del método BeginSaveCustomerService.
    /// </summary>
    /// <param name="result">Resultado del método BeginSaveCustomerService.</param>
    public delegate void SaveCustomerServiceCallback(SaveCustomerServiceResult result);

    public class SilverlightServicesClient
    {
        Uri serverBaseUri;

        Uri relativeGetCustomerServiceDataURI;
        Uri relativeGetDataModelURI;
        Uri relativeSaveCustomerServiceDataURI;
        Uri relativeSaveDataModelURI;

        GetDataModelCallback getDataModelCallback;
        SaveDataModelCallback saveDataModelCallback;
        GetCustomerServiceCallback getCustomerServiceCallback;
        SaveCustomerServiceCallback saveCustomerServiceCallback;

        public SilverlightServicesClient(Uri serverBaseUri)
        {
            this.serverBaseUri = serverBaseUri;

            // Crear un conjunto de URI relativos.
            relativeGetCustomerServiceDataURI = new Uri("/SilverlightServices.svc/GetCustomerServiceData", UriKind.Relative);
            relativeGetDataModelURI = new Uri("/SilverlightServices.svc/GetDataModel", UriKind.Relative);
            relativeSaveCustomerServiceDataURI = new Uri("/SilverlightServices.svc/SaveCustomerServiceData", UriKind.Relative);
            relativeSaveDataModelURI = new Uri("/SilverlightServices.svc/SaveDataModel", UriKind.Relative);
        }

        public void BeginGetCustomerServiceData(int id, GetCustomerServiceCallback callback)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(
                new Uri(serverBaseUri, relativeGetCustomerServiceDataURI));

            request.Method = "POST";

            getCustomerServiceCallback = callback;

            request.BeginGetRequestStream(delegate(IAsyncResult result)
            {
                HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
                webRequest.ContentType = "text/xml";
                Stream requestStream = webRequest.EndGetRequestStream(result);
                StreamWriter streamWriter = new StreamWriter(requestStream);

                // Serializa los datos a enviar.
                DataContractSerializer serializer = new DataContractSerializer(typeof(int));

                serializer.WriteObject(streamWriter.BaseStream, id);
                streamWriter.Close();

                // Realizar una llamada asíncrona para obtener la respuesta.
                // Un Callback será invocado sobre un hilo ejecutándose en segundo plano.
                request.BeginGetResponse(new AsyncCallback(EndGetCustomerServiceData), request);
            }
            , request);
        }

        public void EndGetCustomerServiceData(System.IAsyncResult result)
        {
            GetCustomerServiceResult callResult = new GetCustomerServiceResult();

            try
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);

                // Crear la instancia del deserializador.
                DataContractSerializer serializer = new DataContractSerializer(typeof(CustomerServiceDataEntity));

                // Deserializar el modelo de datos.
                CustomerServiceDataEntity deserializedCustomerService = (CustomerServiceDataEntity)serializer.ReadObject(response.GetResponseStream());

                callResult.CustomerService = deserializedCustomerService;
            }
            catch (SerializationException serializationError)
            {
                Debug.WriteLine(serializationError.Message);
            }
            catch (WebException webError)
            {
                Debug.WriteLine(webError.Message);
            }

            // Si hay callback, invocarlo.
            if (getCustomerServiceCallback != null)
            {
                getCustomerServiceCallback(callResult);
                getCustomerServiceCallback = null;
            }
        }

        public void BeginGetDataModel(GetDataModelCallback callback)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(
                new Uri(serverBaseUri, relativeGetDataModelURI));

            request.Method = "POST";
            request.BeginGetResponse(new AsyncCallback(EndGetDataModel), request);
            getDataModelCallback = callback;
        }

        public void EndGetDataModel(System.IAsyncResult result)
        {
            GetDataModelResult callResult = new GetDataModelResult();

            try
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);

                // Crear la instancia del deserializador.
                DataContractSerializer serializer = new DataContractSerializer(typeof(DataModelEntity));

                // Deserializar el modelo de datos.
                DataModelEntity deserializedDataModel = (DataModelEntity)serializer.ReadObject(response.GetResponseStream());

                callResult.DataModel = deserializedDataModel;
            }
            catch (SerializationException serializationError)
            {
                Debug.WriteLine(serializationError.Message);
            }
            catch (WebException webError)
            {
                Debug.WriteLine(webError.Message);
            }

            // Si existe un callback, invocarlo.
            if (getDataModelCallback != null)
            {
                getDataModelCallback(callResult);
                getDataModelCallback = null;
            }
        }

        public void BeginSaveCustomerServiceData(CustomerServiceDataEntity customServiceDataEntity, SaveCustomerServiceCallback callback)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(
                new Uri(serverBaseUri, relativeSaveCustomerServiceDataURI));

            request.Method = "POST";

            saveCustomerServiceCallback = callback;

            request.BeginGetRequestStream(delegate(IAsyncResult result)
            {
                HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
                webRequest.ContentType = "text/xml";
                Stream requestStream = webRequest.EndGetRequestStream(result);
                StreamWriter streamWriter = new StreamWriter(requestStream);

                // Serializar los datos a enviar.
                DataContractSerializer serializer = new DataContractSerializer(typeof(CustomerServiceDataEntity));
                serializer.WriteObject(streamWriter.BaseStream, customServiceDataEntity);
                streamWriter.Close();

                // Realizar una llamada asíncrona. Se invocará un Callback sobre un hilo ejecutándose en segundo plano.
                request.BeginGetResponse(new AsyncCallback(EndSaveCustomerServiceData), request);
            }
            , request);
        }

        public void EndSaveCustomerServiceData(System.IAsyncResult result)
        {
            SaveCustomerServiceResult callResult = new SaveCustomerServiceResult();

            try
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);

                // Crear una instancia del deserializador.
                DataContractSerializer serializer = new DataContractSerializer(typeof(bool));

                // Deserializar el resultado.
                bool deserializedResult = (bool)serializer.ReadObject(response.GetResponseStream());

                callResult.Success = deserializedResult;
            }
            catch (SerializationException serializationError)
            {
                Debug.WriteLine(serializationError.Message);
            }
            catch (WebException webError)
            {
                Debug.WriteLine(webError.Message);
            }

            // Si existe un callback, invocarlo.
            if (saveCustomerServiceCallback != null)
            {
                saveCustomerServiceCallback(callResult);
                saveCustomerServiceCallback = null;
            }
        }

        public void BeginSaveDataModel(DataModelEntity dataModelEntity, SaveDataModelCallback callback)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(
                new Uri(serverBaseUri, relativeSaveDataModelURI));

            request.Method = "POST";

            saveDataModelCallback = callback;

            request.BeginGetRequestStream(delegate(IAsyncResult result)
            {
                HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
                webRequest.ContentType = "text/xml";
                Stream requestStream = webRequest.EndGetRequestStream(result);
                StreamWriter streamWriter = new StreamWriter(requestStream);

                // Serializar los datos a enviar.
                DataContractSerializer serializer = new DataContractSerializer(typeof(DataModelEntity));
                serializer.WriteObject(streamWriter.BaseStream, dataModelEntity);
                streamWriter.Close();

                // Invocar una llamada asíncrona. Un Callback será invocado en un hilo ejecutándose en segundo plano.
                request.BeginGetResponse(new AsyncCallback(EndSaveDataModel), request);
            }
            , request);

        }

        public void EndSaveDataModel(System.IAsyncResult result)
        {
            SaveDataModelResult callResult = new SaveDataModelResult();

            try
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);

                // Crear la instancia del deserializador.
                DataContractSerializer serializer = new DataContractSerializer(typeof(bool));

                // Deserializar el resultado.
                bool deserializedResult = (bool)serializer.ReadObject(response.GetResponseStream());

                callResult.Success = deserializedResult;
            }
            catch (SerializationException serializationError)
            {
                Debug.WriteLine(serializationError.Message);
            }
            catch (WebException webError)
            {
                Debug.WriteLine(webError.Message);
            }

            // Si existe callback, invocarlo.
            if (saveDataModelCallback != null)
            {
                saveDataModelCallback(callResult);
                saveDataModelCallback = null;
            }
        }
    }
}