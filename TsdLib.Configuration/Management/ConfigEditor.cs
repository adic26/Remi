using System.Windows.Forms;

//TODO: combine with TestDetailsEditor to make base class - these classes derive from it and re




namespace TsdLib.Configuration.Management
{
    public class ConfigEditor : IConfigEditor
    {
        public void Edit(IConfigManagerProvider configManagerProdiver)
        {
            IdentityManager.InitializeTestDetails(configManagerProdiver.TestDetails);
            EditConfig(configManagerProdiver);
            IdentityManager.FireIfModified();
        }

        private ITestSystemIdentityManager _identityManager;
        public ITestSystemIdentityManager IdentityManager
        {
            get { return _identityManager ?? (_identityManager = CreateIdentityManager()); }
        }

        protected virtual ITestSystemIdentityManager CreateIdentityManager()
        {
            return new TestSystemIdentityManager();
        }

        protected virtual void EditConfig(IConfigManagerProvider configManagerProdiver)
        {
            using (ConfigEditorForm form = new ConfigEditorForm(configManagerProdiver))
                if (form.ShowDialog() == DialogResult.OK)
                    foreach (IConfigManager modifiedConfig in form.ModifiedConfigs)
                        modifiedConfig.Save();
                else
                    foreach (IConfigManager modifiedConfig in form.ModifiedConfigs)
                        modifiedConfig.Reload();
        }
    }
}
