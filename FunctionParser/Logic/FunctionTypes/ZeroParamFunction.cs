using System;
using System.Collections.Generic;
using FunctionParser.Logic.Exceptions;

namespace FunctionParser.Logic.FunctionTypes
{
    /// <summary>
    /// Функция без переменных
    /// </summary>
    /// <typeparam name="T">Тип вычисляемых значений</typeparam>
    public class ZeroParamFunction<T> : IFunction<T>
    {
        public string Name { get; }

        public bool Initialized { get; } = true;

        public int ParametersCount => 0;

        private readonly Func<T> _func;
	
        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="ZeroParamFunction{T}"/> на основе названия и функции
        /// </summary>
        /// <param name="name">Название функции</param>
        /// <param name="function">Функция без параметров</param>
        public ZeroParamFunction(string name, Func<T> function)
        {
            Name = name;
            _func = function;
        }

        public void Initialize(IList<IEvaluatable<T>> items)
        {
            if (items.Count != ParametersCount)
                throw new ArgumentsCountMismatchException(Name, items.Count, ParametersCount);
        }

        public T Evaluate(IDictionary<string, T> values) => _func();
	
        public IFunction<T> CreateNew() => new ZeroParamFunction<T>(Name, _func);

        /// <summary>
        /// Возвращает текстовое представление текущего экземпляра функции
        /// </summary>
        public override string ToString()
        {
            return $"Name: \"{Name}\"  Parameters count: {ParametersCount}";
        }
    }
}
