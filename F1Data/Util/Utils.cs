using System;

namespace F1Data.Util
{
    public class Utils
    {
        #region Private Fields

        private static string _plural = string.Empty;

        private static string _singular = string.Empty;

        #endregion Private Fields

        #region Public Methods

        public string EscreveNoPlural(Type entidade)
        {
            _plural = entidade.Name.ToLower();
            var ultimo = _plural[_plural.Length - 1];

            if (ultimo == 's' || ultimo == 'r')
                _plural = _plural + 'e' + 's';
            else
                _plural += 's';

            return _plural;
        }

        public string EscreveNoSingular(Type entidade)
        {
            _singular = entidade.Name.ToLower();
            return _singular;
        }

        public string TratarNumeracao(string numero)
        {
            var primeiro = numero[0];
            if (primeiro.ToString() == "0")
                numero.Remove(0);

            return numero;
        }

        #endregion Public Methods
    }
}