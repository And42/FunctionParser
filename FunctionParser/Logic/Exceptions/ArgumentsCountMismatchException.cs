namespace FunctionParser.Logic.Exceptions
{
    /// <summary>
    /// Ошибка несоответствия количества передаваемых параметров
    /// </summary>
    public class ArgumentsCountMismatchException : ParserException
    {
        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="ArgumentsCountMismatchException"/> на основе названия функции, полученного и требуемого количества параметров
        /// </summary>
        /// <param name="func">Название функции</param>
        /// <param name="gotParameters">Полученное количество параметров</param>
        /// <param name="neededParameters">Требуемое количество параметров</param>
        public ArgumentsCountMismatchException(string func, int gotParameters, int neededParameters) : base($"Функция \"{func}\" принимает аргументов: {neededParameters}. Передано аргументов: {gotParameters}")
        { }
    }
}
