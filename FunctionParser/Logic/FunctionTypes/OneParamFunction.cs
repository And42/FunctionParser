using System;
using System.Collections.Generic;
using FunctionParser.Logic.Exceptions;

namespace FunctionParser.Logic.FunctionTypes
{
    /// <summary>
    /// ������� �� ����� ����������
    /// </summary>
    /// <typeparam name="T">��� ����������� ��������</typeparam>
    public class OneParamFunction<T> : IFunction<T>
    {
        public string Name { get; }

        public bool Initialized { get; private set; }

        public int ParametersCount => 1;

        private IEvaluatable<T> _item;

        private readonly Func<T, T> _func;
	
        /// <summary>
        /// ������ ����� ��������� ������ <see cref="OneParamFunction{T}"/> �� ������ �������� � ������� �� ����� ����������
        /// </summary>
        /// <param name="name">�������� �������</param>
        /// <param name="function">������� �� ������ ���������</param>
        public OneParamFunction(string name, Func<T, T> function) {
            Name = name;
            _func = function;
        }

        public T Evaluate(IDictionary<string, T> values) => Process(_item.Evaluate(values));

        /// <summary>
        /// ��������� �������� ������� ������� �� �������� ����������
        /// </summary>
        /// <param name="item">�������� �������</param>
        public T Process(T item) => _func(item);

        /// <summary>
        /// �������������� ������� �� ������ ������������ ��������
        /// </summary>
        /// <param name="item">����������� ��������</param>
        /// <exception cref="ParserException">��������� ��� �������������� ���������� ���������� �����������</exception>
        public void Initialize(IEvaluatable<T> item)
        {
            // ReSharper disable once JoinNullCheckWithUsage
            if (item == null)
                throw new ParserException($"������������ ���������� ��� ������� \"{this}\"");

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
        /// ���������� ��������� ������������� �������� ���������� �������
        /// </summary>
        public override string ToString()
        {
            return $"Name: \"{Name}\"  Parameters count: {ParametersCount}";
        }
    }
}
