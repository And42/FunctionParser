using System;
using System.Collections.Generic;
using FunctionParser.Logic.Exceptions;

// ReSharper disable ParameterHidesMember

namespace FunctionParser.Logic.FunctionTypes
{
    /// <summary>
    /// функция от двух переменных
    /// </summary>
    /// <typeparam name="T">Тип вычисляемых значений</typeparam>
    public class TwoParamFunction<T> : IFunction<T>
    {
        public string Name { get; }

        public bool Initialized { get; private set; }

        public int ParametersCount => 2;

        private IEvaluatable<T> _first;
        private IEvaluatable<T> _second;

        private readonly Func<T, T, T> _func;

        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="TwoParamFunction{T}"/> на основе названия и функции от двух переменных
        /// </summary>
        /// <param name="name">Название функции</param>
        /// <param name="function">Функция от двух аргументов</param>
        public TwoParamFunction(string name, Func<T, T, T> function)
        {
            Name = name;
            _func = function;
        }

        public T Evaluate(IDictionary<string, T> values)
        {
            return Process(_first.Evaluate(values), _second.Evaluate(values));
        }

        /// <summary>
        /// Вычисляет значение текущей функции от заданных переменных
        /// </summary>
        /// <param name="first">Первый аргумент функции</param>
        /// <param name="second">Второй аргумент функции</param>
        public T Process(T first, T second) => _func(first, second);

        /// <summary>
        /// Инициализирует функцию на основании двух вычисляемых значений
        /// </summary>
        /// <param name="first">Первое вычисляемое значение</param>
        /// <param name="second">Второе вычисляемое значение</param>
        /// <exception cref="ParserException">Возникает при несоответствии переданных параметров требованиям</exception>
        public void Initialize(IEvaluatable<T> first, IEvaluatable<T> second)
        {
            if (first == null || second == null)
                throw new ParserException($"Недостаточно параметров для функции \"{this}\"");

            _first = first;
            _second = second;

            Initialized = true;
        }
	
        public void Initialize(IList<IEvaluatable<T>> items)
        {
            if (items.Count != ParametersCount)
                throw new ArgumentsCountMismatchException(Name, items.Count, ParametersCount);

            Initialize(items[0], items[1]);
        }
	
        public IFunction<T> CreateNew()
        {
            return new TwoParamFunction<T>(Name, _func);
        }

        /// <summary>
        /// Возвращает текстовое представление текущего экземпляра функции
        /// </summary>
        public override string ToString()
        {
            return $"Name: \"{Name}\"  Parameters count: {ParametersCount}";
        }
    }
}
