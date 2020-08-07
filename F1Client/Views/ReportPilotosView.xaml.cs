using F1Client.ViewModels;
using System.Windows.Controls;

namespace F1Client.Views
{
    /// <summary>
    ///     Interaction logic for ReportPilotosView.xaml
    /// </summary>
    public partial class ReportPilotosView : UserControl
    {
        #region Public Constructors

        public ReportPilotosView()
        {
            InitializeComponent();

            var rpViewModel = new ReportPilotosViewModel();
            RpViewer.ViewerCore.ReportSource = rpViewModel.CarregarRelatorio();
        }

        #endregion Public Constructors
    }
}