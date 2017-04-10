using System;
using System.Collections.Generic;
using FunctionParser.Logic.Exceptions;

// ReSharper disable ParameterHidesMember

namespace FunctionParser.Logic.FunctionTypes
{
    /// <summary>
    /// Представляет бинарную функцию
    /// </summary>
    /// <typeparam name="T">Тип чисел</typeparam>
    public class MiddleFunction<T> : IFunction<T>
    {
        public string Name { get; }

        public bool Initialized { get; private set; }

        public int ParametersCount => 2;

        private IEvaluatable<T> _first;
        private IEvaluatable<T> _second;

        private readonly Func<T, T, T> _func;
        private readonly Func<T, T> _rightParam;

        /// <summary>
        /// Создаёт экземпляр класса <see cref="MiddleFunction{T}"/> на основе имени, приоритета, функции от двух аргументов и функции от единственного правого аргумента
        /// </summary>
        /// <param name="name">Имя функции (единичный символ, пример: +,-,*,/)</param>
        /// <param name="priority">Приоритет функции (чем бельше, тем раньше будет вычисляться)</param>
        /// <param name="function">Функция от двух аргументов, чисел</param>
        /// <param name="rightParam">Функция от единственного правого аргумента (нужна, чтобы реализовать, к примеру, "-1")</param>
        public MiddleFunction(char name, int priority, Func<T, T, T> function, Func<T, T> rightParam = null)
        {
            Name = name.ToString();
            Priority = priority;
            _func = function;
            _rightParam = rightParam;
        }

        public T Evaluate(IDictionary<string, T> values)
        {
            return _first == null && _rightParam != null 
                ? _rightParam(_second.Evaluate()) 
                : Process(_first.Evaluate(values), _second.Evaluate(values));
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
            if (second == null || first == null && _rightParam == null)
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

        /// <summary>
        /// Приоритет функции
        /// </summary>
        public int Priority { get; }

        public IFunction<T> CreateNew() => new MiddleFunction<T>(Name[0], Priority, _func, _rightParam);

        /// <summary>
        /// Возвращает текстовое представление текущего экземпляра функции
        /// </summary>
        public override string ToString()
        {
            return $"Name: \"{Name}\"  Parameters count: {ParametersCount}  Priority: {Priority}";
        }
    }
}
