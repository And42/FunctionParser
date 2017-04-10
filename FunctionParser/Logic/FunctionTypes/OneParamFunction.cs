using System;
using System.Collections.Generic;
using FunctionParser.Logic.Exceptions;

namespace FunctionParser.Logic.FunctionTypes
{
    /// <summary>
    /// Функция от одной переменной
    /// </summary>
    /// <typeparam name="T">Тип вычисляемых значений</typeparam>
    public class OneParamFunction<T> : IFunction<T>
    {
        public string Name { get; }

        public bool Initialized { get; private set; }

        public int ParametersCount => 1;

        private IEvaluatable<T> _item;

        private readonly Func<T, T> _func;
	
        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="OneParamFunction{T}"/> на основе названия и функции от одной переменной
        /// </summary>
        /// <param name="name">Название функции</param>
        /// <param name="function">Функция от одного аргумента</param>
        public OneParamFunction(string name, Func<T, T> function) {
            Name = name;
            _func = function;
        }

        public T Evaluate(IDictionary<string, T> values) => Process(_item.Evaluate(values));

        /// <summary>
        /// Вычисляет значение текущей функции от заданной переменной
        /// </summary>
        /// <param name="item">Аргумент функции</param>
        public T Process(T item) => _func(item);

        /// <summary>
        /// Инициализирует функцию на основе вычисляемого значения
        /// </summary>
        /// <param name="item">Вычисляемое значение</param>
        /// <exception cref="ParserException">Возникает при несоответствии переданных параметров требованиям</exception>
        public void Initialize(IEvaluatable<T> item)
        {
            // ReSharper disable once JoinNullCheckWithUsage
            if (item == null)
                throw new ParserException($"Недостаточно параметров для функции \"{this}\"");

            _item = item;

            Initialized = true;
        }
	
        public void Initialize(IList<IEvaluatable<T>> items)
        {
            if (items.Count != ParametersCount)
                throw new ArgumentsCountMismatchException(Name, items.Count, ParametersCount);

            Initialize(items[0]);
        }
	
        public IFunction<T> CreateNew() => new OneParamFunction<T>(Name, _func);

        /// <summary>
        /// Возвращает текстовое представление текущего экземпляра функции
        /// </summary>
        public override string ToString()
        {
            return $"Name: \"{Name}\"  Parameters count: {ParametersCount}";
        }
    }
}
