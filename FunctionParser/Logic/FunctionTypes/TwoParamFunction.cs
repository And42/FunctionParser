using System;
using System.Collections.Generic;
using FunctionParser.Logic.Exceptions;

// ReSharper disable ParameterHidesMember

namespace FunctionParser.Logic.FunctionTypes
{
    /// <summary>
    /// ������� �� ���� ����������
    /// </summary>
    /// <typeparam name="T">��� ����������� ��������</typeparam>
    public class TwoParamFunction<T> : IFunction<T>
    {
        public string Name { get; }

        public bool Initialized { get; private set; }

        public int ParametersCount => 2;

        private IEvaluatable<T> _first;
        private IEvaluatable<T> _second;

        private readonly Func<T, T, T> _func;

        /// <summary>
        /// ������ ����� ��������� ������ <see cref="TwoParamFunction{T}"/> �� ������ �������� � ������� �� ���� ����������
        /// </summary>
        /// <param name="name">�������� �������</param>
        /// <param name="function">������� �� ���� ����������</param>
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
        /// ��������� �������� ������� ������� �� �������� ����������
        /// </summary>
        /// <param name="first">������ �������� �������</param>
        /// <param name="second">������ �������� �������</param>
        public T Process(T first, T second) => _func(first, second);

        /// <summary>
        /// �������������� ������� �� ��������� ���� ����������� ��������
        /// </summary>
        /// <param name="first">������ ����������� ��������</param>
        /// <param name="second">������ ����������� ��������</param>
        /// <exception cref="ParserException">��������� ��� �������������� ���������� ���������� �����������</exception>
        public void Initialize(IEvaluatable<T> first, IEvaluatable<T> second)
        {
            if (first == null || second == null)
                throw new ParserException($"������������ ���������� ��� ������� \"{this}\"");

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
        /// ���������� ��������� ������������� �������� ���������� �������
        /// </summary>
        public override string ToString()
        {
            return $"Name: \"{Name}\"  Parameters count: {ParametersCount}";
        }
    }
}
