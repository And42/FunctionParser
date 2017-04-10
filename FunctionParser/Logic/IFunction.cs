using System.Collections.Generic;

namespace FunctionParser.Logic
{
    /// <summary>
    /// ������������ ��������� �������, �������������� �������� ���� <see cref="T"/>
    /// </summary>
    /// <typeparam name="T">��� �������� ��� ���������</typeparam>
    public interface IFunction<T> : IEvaluatable<T>
    {
        /// <summary>
        /// �������� ������� (��� ��� ����� ���������� � ���������)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ���������� ��������, ������������, ��������������� �� ������� ��������� �������
        /// </summary>
        bool Initialized { get; }
	
        /// <summary>
        /// ���������� ���������� ���������� �������
        /// </summary>
        int ParametersCount { get; }

        /// <summary>
        /// �������������� ������� �� ������ ������ ����������� ��������
        /// </summary>
        /// <param name="items">������ ����������� ��������</param>
        void Initialize(IList<IEvaluatable<T>> items);
	
        /// <summary>
        /// ���������� ����� ��������� �������� ���� �������
        /// </summary>
        IFunction<T> CreateNew();
    }
}
