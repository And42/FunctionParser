using System;
using System.Collections.Generic;

namespace FunctionParser.Logic
{
    /// <summary>
    /// ��������� ��� ��������
    /// </summary>
    /// <typeparam name="T">��� ��������</typeparam>
    public class EvaluatableNumContainer<T> : IEvaluatable<T>
    {
        private readonly Func<IDictionary<string, T>, T> _func;
	
        /// <summary>
        /// ������ ����� ��������� ������ <see cref="EvaluatableNumContainer{T}"/> �� ������ ����������� ��������
        /// </summary>
        /// <param name="elem">��������</param>
        public EvaluatableNumContainer(T elem)
        {
            _func = dictionary => elem;
        }
	
        // ReSharper disable once UnusedParameter.Local
        /// <summary>
        /// ������ ����� ��������� ������ <see cref="EvaluatableNumContainer{T}"/> �� ������ �������� ���������
        /// </summary>
        /// <param name="parameterName">�������� ���������</param>
        /// <param name="unused">�������������� �������� (�����, ����� �������� ��������� � ������ �������������)</param>
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
