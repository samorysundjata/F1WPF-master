using Caliburn.Micro;
using F1Data.DAL;
using F1Data.Util;
using System;
using System.Configuration;
using System.Dynamic;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class ViewModelBase : Screen
    {
        #region Protected Fields

        protected static readonly CircuitoViewModel CircuitoVm = new CircuitoViewModel();

        protected static readonly DateTime DataInicio =
            Convert.ToDateTime(ConfigurationManager.AppSettings["date:inicial"]);

        protected static readonly DateTime DateBirth =
            Convert.ToDateTime(ConfigurationManager.AppSettings["date:birth"]);

        protected static readonly EquipeViewModel EquipeVm = new EquipeViewModel();
        protected static readonly EventoViewModel EventoVm = new EventoViewModel();
        protected static readonly GPViewModel GPVm = new GPViewModel();
        protected static readonly GridViewModel GridVm = new GridViewModel();
        protected static readonly MotorViewModel MotorVm = new MotorViewModel();
        protected static readonly PaisViewModel PaisVm = new PaisViewModel();
        protected static readonly PilotoViewModel PilotoVm = new PilotoViewModel();
        protected static readonly PneuViewModel PneuVm = new PneuViewModel();
        protected static readonly PontuacaoViewModel PontuacaoVm = new PontuacaoViewModel();
        protected static readonly TemporadaViewModel TemporadaVm = new TemporadaViewModel();
        protected static readonly TipoCircuitoViewModel TipoCircuitoVm = new TipoCircuitoViewModel();
        protected static readonly TipoPneuViewModel TipoPneuVm = new TipoPneuViewModel();
        protected static readonly Utils Utils = new Utils();
        protected readonly IDadosF1 IdadosF1 = new GerenciaDados();

        #endregion Protected Fields

        #region Protected Methods

        protected static void AbreJanela(string titulo, object viewModel)
        {
            try
            {
                var windowManager = IoC.Get<IWindowManager>();
                dynamic settings = new ExpandoObject();

                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = titulo;

                windowManager.ShowDialog(viewModel, null, settings);
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, "", "Erro ao abrir janela");
            }
        }

        protected static void TrataExcecao(Exception ex, string compl, string textojanela)
        {
            switch (ex.GetType().ToString())
            {
                case "System.ArgumentNullException":
                    MessageBox.Show("O erro " + ex.Message + compl, textojanela, MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    break;

                case "System.NullReferenceException":
                    MessageBox.Show("Erro ao inserir dados no " + ex.Source + " " + ex.Message + compl, textojanela,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    break;

                default:
                    MessageBox.Show("Este é um erro do tipo " + ex.GetType() + "  que não foi tratado!",
                        "Erro não tratado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    break;
            }
        }

        #endregion Protected Methods
    }
}