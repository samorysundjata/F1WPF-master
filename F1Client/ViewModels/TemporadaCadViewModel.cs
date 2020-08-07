using F1Client.Models;
using System;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class TemporadaCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Temporada TemporadaTipo = new Temporada();
        private DateTime _ano;
        private int _numero;

        #endregion Private Fields

        #region Public Constructors

        public TemporadaCadViewModel(Temporada temporadaSelecionada)
        {
            TemporadaSelecionada = temporadaSelecionada;
            if (temporadaSelecionada.Numero == 0 && temporadaSelecionada.Ano == DateTime.MinValue)
            {
                TemporadaSelecionada = null;
                Numero = UltimoNumero();
                Ano = UltimaData();
            }
            else
            {
                Numero = temporadaSelecionada.Numero;
                Ano = temporadaSelecionada.Ano;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public DateTime Ano
        {
            get { return _ano; }

            set
            {
                _ano = value;
                NotifyOfPropertyChange(() => Ano.Date);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public bool CanSalvar
        {
            get
            {
                return !Numero.Equals(0) &&
                       !DateTime.Today.Equals(DataInicio);
            }
        }

        public int Numero
        {
            get { return _numero; }

            set
            {
                _numero = value;
                NotifyOfPropertyChange(() => Numero);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public Temporada TemporadaSelecionada { get; }

        #endregion Public Properties

        #region Public Methods

        public void Salvar()
        {
            try
            {
                var dados = new string[2];
                dados[0] = Numero.ToString();
                dados[1] = Ano.Date.Year.ToString();

                if (TemporadaSelecionada == null)
                {
                    if (!VerificaExistencia())
                    {
                        IdadosF1.SalvarDados(TemporadaTipo.GetType(), dados);
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
                    if (!VerificaRepetido(TemporadaSelecionada.Numero, dados))
                    {
                        IdadosF1.EditarDados(TemporadaTipo.GetType(), TemporadaSelecionada.Numero.ToString(), dados);
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

        private DateTime UltimaData()
        {
            var maxData = DataInicio;

            var listaTemporada = TemporadaVm.ListaTemporadas();
            if (listaTemporada.Any())
                maxData = listaTemporada.Max(x => x.Ano).AddYears(1);

            if (maxData.Year > DateTime.Today.Year) return DataInicio; //Verificar se funciona na última temporada.

            return maxData;
        }

        private int UltimoNumero()
        {
            var maxNumero = 0;

            var listaTemporada = TemporadaVm.ListaTemporadas();
            if (listaTemporada.Any())
                maxNumero = listaTemporada.Max(x => x.Numero);

            maxNumero++;

            return maxNumero;
        }

        private bool VerificaExistencia()
        {
            var listaTemp = TemporadaVm.ListaTemporadas();
            if (!listaTemp.Any()) return false;
            var itemTemporada = listaTemp.FirstOrDefault(x => x.Numero == Numero || x.Ano == Ano);
            return itemTemporada != null;
        }

        private bool VerificaRepetido(int temporadaSelecionadaNumero, string[] dados)
        {
            var listaTemp = TemporadaVm.ListaTemporadas();
            if (!listaTemp.Any()) return false;
            var itemTemporada = listaTemp.FirstOrDefault(x =>
                x.Numero == Convert.ToInt32(dados[0]) && x.Numero != temporadaSelecionadaNumero ||
                x.Ano.ToString("yyyy") == dados[1]);
            return itemTemporada != null;
        }

        #endregion Private Methods
    }
}