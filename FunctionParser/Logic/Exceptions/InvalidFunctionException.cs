namespace FunctionParser.Logic.Exceptions
{
    /// <summary>
    /// Ошибка поиска функции
    /// </summary>
    public class InvalidFunctionException : ParserException
    {
        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="InvalidFunctionException"/> на основе названия функции
        /// </summary>
        /// <param name="name">Название функции</param>
        public InvalidFunctionException(string name) : base($"Функция \"{name}\" не найдена") { }
    }
}
