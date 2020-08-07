using Caliburn.Micro;
using F1Client.ViewModels;
using System.Windows;

namespace F1Client
{
    internal class HelloBootstrapper : BootstrapperBase
    {
        #region Public Constructors

        public HelloBootstrapper()
        {
            Initialize();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<WorkspaceViewModel>();
        }

        #endregion Protected Methods
    }
}