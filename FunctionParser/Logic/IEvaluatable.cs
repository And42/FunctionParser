using System.Collections.Generic;

namespace FunctionParser.Logic
{
    /// <summary>
    /// ������������ ��������� ������������ ��������
    /// </summary>
    /// <typeparam name="T">��� ������������ ��������</typeparam>
    public interface IEvaluatable<T>
    {
        /// <summary>
        /// ��������� ������� ��������, ����������� �� ���������� (���� ����������)
        /// </summary>
        /// <param name="parameters">������� ����������</param>
        T Evaluate(IDictionary<string, T> parameters = null);
    }
}
