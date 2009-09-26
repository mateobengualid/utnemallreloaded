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
    public delegate IEntity RemoveEntity(IEntity dataModel, string session);
    public delegate void OnRemoveFinished(RemoverArguments args);

    public sealed class Remover
    {
        private Remover()
        {

        }

        /// <summary>
        /// Método invocado cuando se inicia el hilo para eliminar una entidad
        /// </summary>
        /// <param name="control">El componente que invoca el evento</param>
        /// <param name="saver">El delegado que realiza la eliminación</param>
        /// <param name="session">La clave de sesión</param>
        /// <param name="entity">La entidad a eliminar</param>
        /// <param name="onFinished">Método a ser invocado al finalizar la eliminación</param>
        static public void Delete(UserControl1 control, RemoveEntity remover, string session, IEntity entity, OnRemoveFinished onFinished)
        {
            RemoverParameters removerParams = new RemoverParameters(control, remover, session, entity, onFinished);
            Thread thread = new Thread(new ParameterizedThreadStart(DoRemove));
            thread.Start(removerParams);
        }

        static private void DoRemove(object rParams)
        {
            RemoverParameters removerParams = (RemoverParameters)rParams;

            bool succeed;
            Collection<Error> message = new Collection<Error>();

            try
            {
                removerParams.Entity = removerParams.Remover(removerParams.Entity, removerParams.Session);

                if (removerParams.Entity != null)
                {
                    succeed = false;
                    message = removerParams.Entity.Errors;
                }
                else
                {
                    succeed = true;
                }
            }
            catch (TargetInvocationException)
            {
                message.Add(new Error(Resources.ConnectionError, string.Empty, UtnEmall.ServerManager.Properties.Resources.DeleteFailed));
                succeed = false;
            }
            catch (EndpointNotFoundException)
            {
                message.Add(new Error(Resources.ConnectionError, string.Empty, UtnEmall.ServerManager.Properties.Resources.DeleteFailed));
                succeed = false;
            }
            catch (CommunicationException)
            {
                message.Add(new Error(Resources.ConnectionError, string.Empty, UtnEmall.ServerManager.Properties.Resources.UnknownError));
                succeed = false;
            }

            RemoverArguments args = new RemoverArguments(succeed, message);

            removerParams.Control.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new VoidCallback(delegate()
            {
                removerParams.OnFinished(args);
            }));
        }
    }
}
