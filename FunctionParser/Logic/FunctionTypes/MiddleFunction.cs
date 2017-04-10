using System;
using System.Collections.Generic;
using FunctionParser.Logic.Exceptions;

// ReSharper disable ParameterHidesMember

namespace FunctionParser.Logic.FunctionTypes
{
    /// <summary>
    /// ������������ �������� �������
    /// </summary>
    /// <typeparam name="T">��� �����</typeparam>
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
        /// ������ ��������� ������ <see cref="MiddleFunction{T}"/> �� ������ �����, ����������, ������� �� ���� ���������� � ������� �� ������������� ������� ���������
        /// </summary>
        /// <param name="name">��� ������� (��������� ������, ������: +,-,*,/)</param>
        /// <param name="priority">��������� ������� (��� ������, ��� ������ ����� �����������)</param>
        /// <param name="function">������� �� ���� ����������, �����</param>
        /// <param name="rightParam">������� �� ������������� ������� ��������� (�����, ����� �����������, � �������, "-1")</param>
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
            if (second == null || first == null && _rightParam == null)
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

        /// <summary>
        /// ��������� �������
        /// </summary>
        public int Priority { get; }

        public IFunction<T> CreateNew() => new MiddleFunction<T>(Name[0], Priority, _func, _rightParam);

        /// <summary>
        /// ���������� ��������� ������������� �������� ���������� �������
        /// </summary>
        public override string ToString()
        {
            return $"Name: \"{Name}\"  Parameters count: {ParametersCount}  Priority: {Priority}";
        }
    }
}
