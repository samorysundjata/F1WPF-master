using F1Client.Models;
using System;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class PaisCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Pais PaisTipo = new Pais();
        private string _nome;
        private string _sigla;

        #endregion Private Fields

        #region Public Constructors

        public PaisCadViewModel(Pais paisSelecionado)
        {
            PaisSelecionado = paisSelecionado;
            if (paisSelecionado.Nome == null && paisSelecionado.Sigla == null)
            {
                PaisSelecionado = null;
            }
            else
            {
                Sigla = paisSelecionado.Sigla;
                Nome = paisSelecionado.Nome;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public bool CanSalvar
        {
            get { return !string.IsNullOrWhiteSpace(Nome) && !string.IsNullOrWhiteSpace(Sigla); }
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

        public Pais PaisSelecionado { get; }

        public string Sigla
        {
            get { return _sigla; }

            set
            {
                _sigla = value;
                NotifyOfPropertyChange(() => Sigla);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Salvar()
        {
            try
            {
                var dados = new string[2];
                dados[0] = Sigla.ToUpper().Trim();
                dados[1] = Nome.Trim();

                if (PaisSelecionado == null)
                {
                    if (!VerificaExistencia())
                    {
                        IdadosF1.SalvarDados(PaisTipo.GetType(), dados);
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
                    if (!VerificaRepetido(PaisSelecionado.Sigla.ToUpper(), dados))
                    {
                        IdadosF1.EditarDados(PaisTipo.GetType(), PaisSelecionado.Sigla.ToUpper(), dados);
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
            var listaPais = PaisVm.ListaPaises();
            if (!listaPais.Any()) return false;
            var itemPais = listaPais.FirstOrDefault(x => x.Sigla == Sigla.ToUpper() || x.Nome == Nome);
            return itemPais != null;
        }

        private bool VerificaRepetido(string chavePaisSelecionado, string[] dados)
        {
            var listaPais = PaisVm.ListaPaises();
            if (!listaPais.Any()) return false;
            var itemPais = listaPais.FirstOrDefault(x =>
                x.Nome == dados[1] && x.Sigla != chavePaisSelecionado ||
                x.Sigla == dados[0].ToUpper() && x.Sigla != chavePaisSelecionado);
            return itemPais != null;
        }

        #endregion Private Methods
    }
}