namespace UtnEmall.ServerManager.Properties
{


    // Esta clase permite manejar eventos específicos en la clase de ajuste:
    // El evento SettingChanging es creado antes de modificar el valor de una propiedad.
    // El evento PropertyChanged es creado después de modificar el valor de una propiedad.
    // El evento SettingsLoaded es creado después de cargar los valores de propiedad.
    // El evento SettingsSaving es creado antes de guardar un valor de propiedad.
    internal sealed partial class Settings
    {

        public Settings()
        {
            //  // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
            //
        }

        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            //  Add code to handle the SettingChangingEvent event here.
            //
        }

        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //  Add code to handle the SettingsSaving event here.
            //
        }
    }
}
