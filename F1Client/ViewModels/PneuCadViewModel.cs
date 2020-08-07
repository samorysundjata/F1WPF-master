using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class PneuCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Pneu PneuTipo = new Pneu();
        private string _nome;
        private Pais _pais;
        private ObservableCollection<Pais> _paises;

        #endregion Private Fields

        #region Public Constructors

        public PneuCadViewModel(Pneu pneuSelecionado)
        {
            PneuSelecionado = pneuSelecionado;
            if (pneuSelecionado.Nome == null && pneuSelecionado.Pais == null)
            {
                PneuSelecionado = null;
            }
            else
            {
                Nome = pneuSelecionado.Nome;
                Pais = pneuSelecionado.Pais;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public bool CanSalvar
        {
            get { return !string.IsNullOrWhiteSpace(Nome) && !string.IsNullOrWhiteSpace(Pais.Sigla); }
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

        public Pais Pais
        {
            get { return _pais; }

            set
            {
                _pais = value;
                NotifyOfPropertyChange(() => Pais);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public ObservableCollection<Pais> Paises
        {
            get { return _paises; }

            set
            {
                _paises = value;
                NotifyOfPropertyChange(() => Paises);
            }
        }

        public Pneu PneuSelecionado { get; }

        #endregion Public Properties

        #region Public Methods

        public void Salvar()
        {
            try
            {
                var dados = new string[2];
                dados[0] = Nome.Trim();
                dados[1] = Pais.Sigla;

                if (PneuSelecionado == null)
                {
                    if (!VerificaExistencia())
                    {
                        IdadosF1.SalvarDados(PneuTipo.GetType(), dados);
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
                    if (!VerificaRepetido(PneuSelecionado.Nome, dados))
                    {
                        IdadosF1.EditarDados(PneuTipo.GetType(), PneuSelecionado.Nome, dados);
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

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            Paises = PaisVm.ListaPaises();
        }

        #endregion Protected Methods

        #region Private Methods

        private bool VerificaExistencia()
        {
            var listaPneus = PneuVm.ListaPneus();
            if (!listaPneus.Any()) return false;
            var itemPneu = listaPneus.FirstOrDefault(x => x.Nome == Nome);
            return itemPneu != null;
        }

        private bool VerificaRepetido(string pneuSelecionadoNome, string[] dados)
        {
            var listaPneus = PneuVm.ListaPneus();
            if (!listaPneus.Any()) return false;
            var itemPneu = listaPneus.FirstOrDefault(x => x.Nome == dados[0] && x.Nome != pneuSelecionadoNome);
            return itemPneu != null;
        }

        #endregion Private Methods
    }
}