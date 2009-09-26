using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtnEmall.ServerManager;
using UtnEmall.Server.EntityModel;
using System.Reflection;
using System.ServiceModel;
using System.Collections.ObjectModel;

namespace UtnEmall.ServerManager
{
    #region Delegates

    public delegate ReadOnlyCollection<IEntity> LoadList(bool loadRelations, string session);
    public delegate void OnFinished(LoaderArguments args);
    public delegate void VoidCallback();

    #endregion

    public sealed class Loader
    {
        #region Constructors

        private Loader()
        {

        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Método invocado para iniciar el hilo que carga una entidad
        /// </summary>
        /// <param name="control">El componente que invocó al evento</param>
        /// <param name="saver">El delegado que realiza la carga</param>
        /// <param name="session">La clave de sesión</param>
        /// <param name="loadRelations">Indica si se deben cargar las relaciones de las entidades</param>
        /// <param name="onFinished">método invocado cuando finaliza el proceso</param>
        static public void Load(UserControl1 control, LoadList loader, string session, bool loadRelations, OnFinished onFinished)
        {
            LoaderParameters loaderParams = new LoaderParameters(control, loader, session, loadRelations, onFinished);
            Thread thread = new Thread(new ParameterizedThreadStart(DoLoad));
            thread.Start(loaderParams);
        }

        static private void DoLoad(object lParams)
        {
            LoaderParameters loaderParams = (LoaderParameters)lParams;

            bool succeed;
            string message;
            List<IEntity> items = new List<IEntity>();

            try
            {
                foreach (IEntity entity in loaderParams.Loader(loaderParams.LoadRelations, loaderParams.Session))
                {
                    items.Add(entity);
                }

                message = "";
                succeed = true;
            }
            catch (TargetInvocationException)
            {
                message = UtnEmall.ServerManager.Properties.Resources.LoadFailed;
                succeed = false;
            }
            catch (EndpointNotFoundException)
            {
                message = UtnEmall.ServerManager.Properties.Resources.LoadFailed;
                succeed = false;
            }
            catch (CommunicationException)
            {
                message = UtnEmall.ServerManager.Properties.Resources.UnknownError;
                succeed = false;
            }

            LoaderArguments args = new LoaderArguments(succeed, message, new ReadOnlyCollection<IEntity>(items));

            loaderParams.Control.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new VoidCallback(delegate()
            {
                loaderParams.OnFinished(args);
            }));
        }

        #endregion
    }
}
