using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class TemporadaViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Temporada TemporadaTipo = new Temporada();
        private DateTime _ano;
        private int _numero;
        private ObservableCollection<Temporada> _temporadas;
        private Temporada _temporadaSelecionada;

        #endregion Private Fields

        #region Public Properties

        public DateTime Ano
        {
            get { return _ano; }

            set
            {
                _ano = value;
                NotifyOfPropertyChange(() => Ano);
            }
        }

        public int Numero
        {
            get { return _numero; }

            set
            {
                _numero = value;
                NotifyOfPropertyChange(() => Numero);
            }
        }

        public ObservableCollection<Temporada> Temporadas
        {
            get { return _temporadas; }

            set
            {
                _temporadas = value;
                NotifyOfPropertyChange(() => Temporadas);
            }
        }

        public Temporada TemporadaSelecionada
        {
            get { return _temporadaSelecionada; }

            set
            {
                _temporadaSelecionada = value;
                NotifyOfPropertyChange(() => TemporadaSelecionada);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Editar()
        {
            if (TemporadaSelecionada != null)
            {
                var viewModel = new TemporadaCadViewModel(TemporadaSelecionada);
                AbreJanela("Editar Temporada", viewModel);
                ListaTemporadas();
            }
            else
            {
                MessageBox.Show("Selecione uma temporada para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (TemporadaSelecionada != null)
            {
                var messageBoxResult = MessageBox.Show(
                    "Deseja mesmo excluir a temporada do ano de " + TemporadaSelecionada.Ano.ToString("yyyy") + "?",
                    "Confirme a exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult != MessageBoxResult.Yes) return;
                IdadosF1.ExcluirDados(TemporadaTipo.GetType(), TemporadaSelecionada.Numero.ToString());
                ListaTemporadas();
            }
            else
            {
                MessageBox.Show("Selecione uma temporada para excluir!", "Erro ao excluir", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Temporada> ListaTemporadas()
        {
            try
            {
                Temporadas = new ObservableCollection<Temporada>();

                var nos = IdadosF1.ListaDados(TemporadaTipo.GetType());

                if (nos != null)
                    foreach (XmlNode node in nos)
                        Temporadas.Add(new Temporada
                        {
                            Numero = Convert.ToInt32(node.SelectSingleNode("Numero")?.InnerText),
                            Ano = DateTime.ParseExact(node.SelectSingleNode("Ano")?.InnerText, "yyyy", null)
                        });

                return Temporadas = new ObservableCollection<Temporada>(Temporadas.OrderBy(x => x.Numero));
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem dos registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new TemporadaCadViewModel(TemporadaTipo);
            AbreJanela("Nova Temporada", viewModel);
            ListaTemporadas();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ListaTemporadas();
        }

        #endregion Protected Methods
    }
}