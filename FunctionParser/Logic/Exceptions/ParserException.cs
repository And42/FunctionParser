using System;

namespace FunctionParser.Logic.Exceptions
{
    /// <summary>
    /// Ошибка парсера
    /// </summary>
    public class ParserException : Exception
    {
        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="ParserException"/> на основе сообщения
        /// </summary>
        /// <param name="message">Сообщение ошибки</param>
        public ParserException(string message) : base(message) { }
    }
}
