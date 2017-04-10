using System;
using System.Collections.Generic;
using System.Linq;
using FunctionParser.Logic.Exceptions;

namespace FunctionParser.Logic.FunctionTypes
{
    /// <summary>
    /// Функции с фиксированным количеством параметров
    /// </summary>
    /// <typeparam name="T">Тип обрабатываемых значений</typeparam>
    public class FixedParamFunction<T> : IFunction<T>
    {
        public string Name { get; }

        public bool Initialized { get; private set; }

        public int ParametersCount { get; }

        private IList<IEvaluatable<T>> _items;

        private readonly Func<IList<T>, T> _func;

        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="FixedParamFunction{T}"/> на основе названия, функции от списка параметров и количества параметров
        /// </summary>
        /// <param name="name">Название функции</param>
        /// <param name="function">Функции от списка параметров</param>
        /// <param name="parametersCount">Количество аргументов функции</param>
        public FixedParamFunction(string name, Func<IList<T>, T> function, int parametersCount)
        {
            Name = name;
            _func = function;
            ParametersCount = parametersCount;
        }

        public T Evaluate(IDictionary<string, T> values)
        {
            return Process(_items.Select(it => it.Evaluate(values)).ToArray());
        }

        /// <summary>
        /// Вычисляет значение текущей функции от заданных переменных
        /// </summary>
        /// <param name="item">Список аргументов функции</param>
        public T Process(IList<T> item)
        {
            return _func(item);
        }

        public void Initialize(IList<IEvaluatable<T>> itms)
        {
            if (itms.Count != ParametersCount)
                throw new ArgumentsCountMismatchException(Name, itms.Count, ParametersCount);

            _items = itms;

            Initialized = true;
        }

        public IFunction<T> CreateNew() => new FixedParamFunction<T>(Name, _func, ParametersCount);

        /// <summary>
        /// Возвращает текстовое представление текущего экземпляра функции
        /// </summary>
        public override string ToString()
        {
            return $"Name: \"{Name}\"  Parameters count: {ParametersCount}";
        }
    }
}
