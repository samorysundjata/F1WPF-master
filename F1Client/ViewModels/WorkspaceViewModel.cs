using Caliburn.Micro;

namespace F1Client.ViewModels
{
    internal class WorkspaceViewModel : Conductor<object>
    {
        #region Public Methods

        public void Circuito()
        {
            var viewModel = IoC.Get<CircuitoViewModel>();
            ActivateItem(viewModel);
        }

        public void Equipe()
        {
            var viewModel = IoC.Get<EquipeViewModel>();
            ActivateItem(viewModel);
        }

        public void Evento()
        {
            var viewModel = IoC.Get<EventoViewModel>();
            ActivateItem(viewModel);
        }

        public void GP()
        {
            var viewModel = IoC.Get<GPViewModel>();
            ActivateItem(viewModel);
        }

        public void Grid()
        {
            var viewModel = IoC.Get<GridViewModel>();
            ActivateItem(viewModel);
        }

        public void Motor()
        {
            var viewModel = IoC.Get<MotorViewModel>();
            ActivateItem(viewModel);
        }

        public void Pais()
        {
            var viewModel = IoC.Get<PaisViewModel>();
            ActivateItem(viewModel);
        }

        public void Piloto()
        {
            var viewModel = IoC.Get<PilotoViewModel>();
            ActivateItem(viewModel);
        }

        public void Pneu()
        {
            var viewModel = IoC.Get<PneuViewModel>();
            ActivateItem(viewModel);
        }

        public void Pontuacao()
        {
            var viewModel = IoC.Get<PontuacaoViewModel>();
            ActivateItem(viewModel);
        }

        public void RelatorioPilotos()
        {
            var viewModel = IoC.Get<ReportPilotosViewModel>();
            ActivateItem(viewModel);
        }

        public void Sair()
        {
            TryClose();
        }

        public void Temporada()
        {
            var viewModel = IoC.Get<TemporadaViewModel>();
            ActivateItem(viewModel);
        }

        public void TipoCircuito()
        {
            var viewModel = IoC.Get<TipoCircuitoViewModel>();
            ActivateItem(viewModel);
        }

        public void TipoPneu()
        {
            var viewModel = IoC.Get<TipoPneuViewModel>();
            ActivateItem(viewModel);
        }

        #endregion Public Methods
    }
}