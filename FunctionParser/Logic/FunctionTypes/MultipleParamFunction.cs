using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionParser.Logic.FunctionTypes
{
    /// <summary>
    /// Функция от неограниченного числа элементов (допускается их отсутствие)
    /// </summary>
    /// <typeparam name="T">Тип вычисляемых значений</typeparam>
    public class MultipleParamFunction<T> : IFunction<T>
    {
        public string Name { get; }

        public bool Initialized { get; private set; }

        public int ParametersCount => -1;

        private IList<IEvaluatable<T>> _items;

        private readonly Func<IList<T>, T> _func;

        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="MultipleParamFunction{T}"/> на основе названия и функции от списка переменных
        /// </summary>
        /// <param name="name">Название функции</param>
        /// <param name="function">Список аргументов функции</param>
        public MultipleParamFunction(string name, Func<IList<T>, T> function)
        {
            Name = name;
            _func = function;
        }

        public T Evaluate(IDictionary<string, T> values) => Process(_items.Select(it => it.Evaluate(values)).ToArray());

        /// <summary>
        /// Вычисляет значение текущей функции от заданных переменных
        /// </summary>
        /// <param name="item">Список аргументов</param>
        public T Process(IList<T> item) => _func(item);

        public void Initialize(IList<IEvaluatable<T>> itms)
        {
            _items = itms;

            Initialized = true;
        }

        public IFunction<T> CreateNew() => new MultipleParamFunction<T>(Name, _func);

        /// <summary>
        /// Возвращает текстовое представление текущего экземпляра функции
        /// </summary>
        public override string ToString()
        {
            return $"Name: \"{Name}\"  Parameters count: {ParametersCount}";
        }
    }
}
