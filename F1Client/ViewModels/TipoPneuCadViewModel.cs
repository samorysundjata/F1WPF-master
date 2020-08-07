using F1Client.Models;
using System;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class TipoPneuCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly TipoPneu TipoPneuTipo = new TipoPneu();

        private string _nome;

        #endregion Private Fields

        #region Public Constructors

        public TipoPneuCadViewModel(TipoPneu tipopneuSelecionado)
        {
            TipopneuSelecionado = tipopneuSelecionado;
            if (tipopneuSelecionado.Nome == null)
                TipopneuSelecionado = null;
            else
                Nome = tipopneuSelecionado.Nome;
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

        public TipoPneu TipopneuSelecionado { get; }

        #endregion Public Properties

        #region Public Methods

        public void Salvar()
        {
            try
            {
                var dados = new string[1];
                dados[0] = Nome.Trim();

                if (TipopneuSelecionado == null)
                {
                    if (!VerificarExistencia())
                    {
                        IdadosF1.SalvarDados(TipoPneuTipo.GetType(), dados);
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
                    if (!VerificaRepetido(TipopneuSelecionado.Nome, dados))
                    {
                        IdadosF1.EditarDados(TipoPneuTipo.GetType(), TipopneuSelecionado.Nome, dados);
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

        private bool VerificaRepetido(string tipoPneuSelecionadoNome, string[] dados)
        {
            var listaTiposPneus = TipoPneuVm.ListaTiposPneus();
            if (!listaTiposPneus.Any()) return false;
            var itemTipoPneu =
                listaTiposPneus.FirstOrDefault(x => x.Nome == dados[0] && x.Nome != tipoPneuSelecionadoNome);
            return itemTipoPneu != null;
        }

        private bool VerificarExistencia()
        {
            var listaTiposPneus = TipoPneuVm.ListaTiposPneus();
            if (!listaTiposPneus.Any()) return false;
            var itemTipoPneu = listaTiposPneus.FirstOrDefault(x => x.Nome == Nome);
            return itemTipoPneu != null;
        }

        #endregion Private Methods
    }
}