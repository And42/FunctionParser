using System.Collections.Generic;

namespace FunctionParser.Logic
{
    /// <summary>
    /// Представляет интерфейс функции, обрабатывающей значения типа <see cref="T"/>
    /// </summary>
    /// <typeparam name="T">Тип значений для обработки</typeparam>
    public interface IFunction<T> : IEvaluatable<T>
    {
        /// <summary>
        /// Название функции (как она будет вызываться в выражении)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Возвращает значение, показывающее, инициализирован ли текущий экземпляр функции
        /// </summary>
        bool Initialized { get; }
	
        /// <summary>
        /// Возвращает количество аргументов функции
        /// </summary>
        int ParametersCount { get; }

        /// <summary>
        /// Инициализирует функцию на основе списка вычисляемых значений
        /// </summary>
        /// <param name="items">Список вычисляемых значений</param>
        void Initialize(IList<IEvaluatable<T>> items);
	
        /// <summary>
        /// Возвращает новый экземпляр текущего типа функции
        /// </summary>
        IFunction<T> CreateNew();
    }
}
