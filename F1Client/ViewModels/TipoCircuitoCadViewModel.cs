using F1Client.Models;
using System;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class TipoCircuitoCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly TipoCircuito TipoTipoCircuito = new TipoCircuito();
        private string _nome;

        #endregion Private Fields

        #region Public Constructors

        public TipoCircuitoCadViewModel(TipoCircuito tipoCircuitoSelecionado)
        {
            TipoCircuitoSelecionado = tipoCircuitoSelecionado;
            if (tipoCircuitoSelecionado.Nome == null)
                TipoCircuitoSelecionado = null;
            else
                Nome = tipoCircuitoSelecionado.Nome;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool CanSalvar
        {
            get { return !string.IsNullOrWhiteSpace(Nome); }
        }

        public string Nome
        {
            get { return _nome; }

            set
            {
                _nome = value;
                NotifyOfPropertyChange(() => Nome);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public TipoCircuito TipoCircuitoSelecionado { get; }

        #endregion Public Properties

        #region Public Methods

        public void Salvar()
        {
            try
            {
                var dados = new string[1];
                dados[0] = Nome.Trim();

                if (TipoCircuitoSelecionado == null)
                {
                    if (!VerificaExistencia())
                    {
                        IdadosF1.SalvarDados(TipoTipoCircuito.GetType(), dados);
                        TryClose();
                    }
                    else
                    {
                        MessageBox.Show("O registro já existe", "Erro ao salvar", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (!VerificaRepetido(TipoCircuitoSelecionado.Nome, dados))
                    {
                        IdadosF1.EditarDados(TipoTipoCircuito.GetType(), TipoCircuitoSelecionado.Nome, dados);
                        TryClose();
                    }
                    else
                    {
                        MessageBox.Show("O registro já existe", "Erro ao salvar", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na inclusão do registro!", "Erro na inclusão");
                TryClose();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private bool VerificaExistencia()
        {
            var listaTipoCircuito = TipoCircuitoVm.ListaTiposCircuitos();
            if (!listaTipoCircuito.Any()) return false;
            var itemTipoCircuito = listaTipoCircuito.FirstOrDefault(x => x.Nome == Nome);
            return itemTipoCircuito != null;
        }

        private bool VerificaRepetido(string nomeSelecionado, string[] dados)
        {
            var listaTipoCircuito = TipoCircuitoVm.ListaTiposCircuitos();
            if (!listaTipoCircuito.Any()) return false;
            var itemTipoCircuito =
                listaTipoCircuito.FirstOrDefault(x => x.Nome == dados[0] && x.Nome != nomeSelecionado);
            return itemTipoCircuito != null;
        }

        #endregion Private Methods
    }
}