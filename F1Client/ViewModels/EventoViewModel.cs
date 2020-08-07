using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;
using MessageBox = System.Windows.MessageBox;

namespace F1Client.ViewModels
{
    internal class EventoViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Evento EventoTipo = new Evento();
        private ObservableCollection<Evento> _eventos;
        private Evento _eventoSelecionado;
        private GP _gp;
        private Grid _grid;
        private int _numero;
        private int _posicao;

        #endregion Private Fields

        #region Public Properties

        public ObservableCollection<Evento> Eventos
        {
            get { return _eventos; }
            set
            {
                _eventos = value;
                NotifyOfPropertyChange(() => Eventos);
            }
        }

        public Evento EventoSelecionado
        {
            get { return _eventoSelecionado; }
            set
            {
                _eventoSelecionado = value;
                NotifyOfPropertyChange(() => EventoSelecionado);
            }
        }

        public GP Gp
        {
            get { return _gp; }
            set
            {
                _gp = value;
                NotifyOfPropertyChange(() => Gp);
            }
        }

        public Grid Grid
        {
            get { return _grid; }
            set
            {
                _grid = value;
                NotifyOfPropertyChange(() => Grid);
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

        public int Posicao
        {
            get { return _posicao; }
            set
            {
                _posicao = value;
                NotifyOfPropertyChange(() => Posicao);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Editar()
        {
            if (EventoSelecionado != null)
            {
                var viewModel = new EventoCadViewModel(EventoSelecionado);
                AbreJanela("Editar Evento", viewModel);
                ListarEventos();
            }
            else
            {
                MessageBox.Show("Selecione um evento para editar!", "Erro ao Editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (EventoSelecionado != null)
            {
                var messageBoxResult =
                    MessageBox.Show(
                        "Deseja mesmo excluir o evento do GP de " +
                        EventoSelecionado.GP.DataCorrida.ToShortDateString() +
                        "?", "Confirme a exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            else
            {
                MessageBox.Show("Escolha um evento para excuir!", "Erro ao excluir", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Evento> ListarEventos()
        {
            try
            {
                Eventos = new ObservableCollection<Evento>();

                var nos = IdadosF1.ListaDados(EventoTipo.GetType());

                if (nos != null)
                    foreach (XmlNode node in nos)
                        Eventos.Add(new Evento
                        {
                            GP = GPVm.ListaGPs().FirstOrDefault(x =>
                                x.DataCorrida == Convert.ToDateTime(node.SelectSingleNode("GP")?.InnerText)),
                            Grid = GridVm.ListaGrids().FirstOrDefault(x =>
                                x.Numero == Convert.ToInt32(node.SelectSingleNode("Grid")?.InnerText)),
                            Posicao = Convert.ToInt32(node.SelectSingleNode("Posicao")?.InnerText)
                        });

                return Eventos;
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem do registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new EventoCadViewModel(EventoTipo);
            AbreJanela("Novo Evento", viewModel);
            ListarEventos();
        }

        #endregion Public Methods
    }
}