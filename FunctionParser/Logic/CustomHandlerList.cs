using System;
using System.Collections;
using System.Collections.Generic;

namespace FunctionParser.Logic
{
    /// <summary>
    /// Настраиваемый обработчик коллекции
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomHandlerList<T> : IEnumerable<T>
    {
        private readonly Func<IEnumerator<T>> _getEnumerator;
        private readonly Action<T> _addingHandler;
        private readonly Action<T> _removingHandler;

        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="CustomHandlerList{T}"/> на основе функции перечисления, добавления и удаления элемента
        /// </summary>
        /// <param name="getEnumerator">Функция, возвращающая Enumerator текущего объекта</param>
        /// <param name="addingHandler">Функция добавления элемента в коллекцию</param>
        /// <param name="removingHandler">Функция удаления элемента из коллекции</param>
        public CustomHandlerList(Func<IEnumerator<T>> getEnumerator, Action<T> addingHandler, Action<T> removingHandler)
        {
            _getEnumerator = getEnumerator;
            _addingHandler = addingHandler;
            _removingHandler = removingHandler;
        }

        /// <summary>
        /// Добавляет элемент в коллекцию
        /// </summary>
        /// <param name="item">Элемент</param>
        public void Add(T item)
        {
            _addingHandler(item);
        }

        /// <summary>
        /// Удаляет элемент из коллекции
        /// </summary>
        /// <param name="item">Элемент</param>
        public void Remove(T item)
        {
            _removingHandler(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Возвращает Enumerator текущего экземпляра коллекции
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return _getEnumerator();
        }
    }
}
