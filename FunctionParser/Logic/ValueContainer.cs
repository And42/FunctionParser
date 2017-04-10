namespace FunctionParser.Logic
{
    internal class ValueContainer<T>
    {
        public T Value { get; set; }

        public ValueContainer(T value)
        {
            Value = value;
        }
    }
}
