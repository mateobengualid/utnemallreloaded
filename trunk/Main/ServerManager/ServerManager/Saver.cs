using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using UtnEmall.ServerManager;
using System.Threading;
using System.Reflection;
using System.ServiceModel;
using System.Collections.ObjectModel;
using UtnEmall.ServerManager.Properties;

namespace UtnEmall.ServerManager
{
    public delegate IEntity SaveEntity(IEntity entity, string session);
    public delegate void OnSaveFinished(SaverArgs args);

    public sealed class Saver
    {
        private Saver()
        {

        }

        /// <summary>
        /// Método invocado para iniciar el hilo para guardar entidades
        /// </summary>
        /// <param name="control">El componente que invoca al método</param>
        /// <param name="saver">El delegado que realiza el guardado</param>
        /// <param name="session">La clave de sesión</param>
        /// <param name="entity">La entidad a guardar</param>
        /// <param name="onFinished">Método invocado al finalizar</param>
        static public void Save(UserControl1 control, SaveEntity saver, string session, IEntity entity, OnSaveFinished onFinished)
        {
            SaverParameters saverParams = new SaverParameters(control, saver, session, entity, onFinished);
            Thread thread = new Thread(new ParameterizedThreadStart(DoSave));
            thread.Start(saverParams);
        }

        /// <summary>
        /// Realiza el proceso
        /// </summary>
        /// <param name="sParams">El objeto de parámetro</param>
        static private void DoSave(object sParams)
        {
            SaverParameters saverParams = (SaverParameters)sParams;

            bool succeed;
            Collection<Error> message = new Collection<Error>();

            try
            {
                saverParams.Entity = saverParams.Saver(saverParams.Entity, saverParams.Session);
                if (saverParams.Entity != null)
                {
                    succeed = false;
                    message = saverParams.Entity.Errors;
                }
                else
                {
                    succeed = true;
                }
            }
            catch (TargetInvocationException)
            {
                message.Add(new Error(Resources.ConnectionError, string.Empty, UtnEmall.ServerManager.Properties.Resources.SaveError));
                succeed = false;
            }
            catch (EndpointNotFoundException)
            {
                message.Add(new Error(Resources.ConnectionError, string.Empty, UtnEmall.ServerManager.Properties.Resources.SaveError));
                succeed = false;
            }
            catch (CommunicationException)
            {
                message.Add(new Error(Resources.ConnectionError, string.Empty, UtnEmall.ServerManager.Properties.Resources.UnknownError));
                succeed = false;
            }

            SaverArgs args = new SaverArgs(succeed, message);

            saverParams.Control.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new VoidCallback(delegate()
            {
                saverParams.OnFinished(args);
            }));
        }
    }
}
