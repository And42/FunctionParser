using System.Collections.Generic;

namespace FunctionParser.Logic
{
    /// <summary>
    /// Представляет интерфейс вычисляемого значения
    /// </summary>
    /// <typeparam name="T">Тип вычисляемого значения</typeparam>
    public interface IEvaluatable<T>
    {
        /// <summary>
        /// Вычисляет текущее значение, основываясь на переменных (если необходимы)
        /// </summary>
        /// <param name="parameters">Словарь переменных</param>
        T Evaluate(IDictionary<string, T> parameters = null);
    }
}
