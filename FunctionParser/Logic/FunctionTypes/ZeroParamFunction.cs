using System;
using System.Collections.Generic;
using FunctionParser.Logic.Exceptions;

namespace FunctionParser.Logic.FunctionTypes
{
    /// <summary>
    /// ������� ��� ����������
    /// </summary>
    /// <typeparam name="T">��� ����������� ��������</typeparam>
    public class ZeroParamFunction<T> : IFunction<T>
    {
        public string Name { get; }

        public bool Initialized { get; } = true;

        public int ParametersCount => 0;

        private readonly Func<T> _func;
	
        /// <summary>
        /// ������ ����� ��������� ������ <see cref="ZeroParamFunction{T}"/> �� ������ �������� � �������
        /// </summary>
        /// <param name="name">�������� �������</param>
        /// <param name="function">������� ��� ����������</param>
        public ZeroParamFunction(string name, Func<T> function)
        {
            Name = name;
            _func = function;
        }

        public void Initialize(IList<IEvaluatable<T>> items)
        {
            if (items.Count != ParametersCount)
                throw new ArgumentsCountMismatchException(Name, items.Count, ParametersCount);
        }

        public T Evaluate(IDictionary<string, T> values) => _func();
	
        public IFunction<T> CreateNew() => new ZeroParamFunction<T>(Name, _func);

        /// <summary>
        /// ���������� ��������� ������������� �������� ���������� �������
        /// </summary>
        public override string ToString()
        {
            return $"Name: \"{Name}\"  Parameters count: {ParametersCount}";
        }
    }
}
