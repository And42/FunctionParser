using System;
using System.Collections.Generic;

namespace FunctionParser.Logic
{
    /// <summary>
    /// Контейнер для значений
    /// </summary>
    /// <typeparam name="T">Тип значений</typeparam>
    public class EvaluatableNumContainer<T> : IEvaluatable<T>
    {
        private readonly Func<IDictionary<string, T>, T> _func;
	
        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="EvaluatableNumContainer{T}"/> на основе постоянного значения
        /// </summary>
        /// <param name="elem">Значение</param>
        public EvaluatableNumContainer(T elem)
        {
            _func = dictionary => elem;
        }
	
        // ReSharper disable once UnusedParameter.Local
        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="EvaluatableNumContainer{T}"/> на основе названия параметра
        /// </summary>
        /// <param name="parameterName">Название параметра</param>
        /// <param name="unused">Неиспользуемый параметр (нужен, чтобы избежать конфликта с другим конструктором)</param>
        public EvaluatableNumContainer(string parameterName, bool unused)
        {
            _func = dictionary => dictionary[parameterName];
        }
	
        public T Evaluate(IDictionary<string, T> values)
        {
            return _func(values);
        }
    }
}
