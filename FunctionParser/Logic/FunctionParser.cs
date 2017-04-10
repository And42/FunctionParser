using System;
using System.Collections.Generic;
using System.Text;
using FunctionParser.Logic.Exceptions;
using FunctionParser.Logic.FunctionTypes;

namespace FunctionParser.Logic
{
    /// <summary>
    /// Парсер математических выражений
    /// </summary>
    /// <typeparam name="T">Тип обрабатываемых элементов</typeparam>
    public class FunctionParser<T>
    {
        private readonly Func<string, T> _numberParser;

        /// <summary>
        /// Функции от двух аргументов
        /// </summary>
        public CustomHandlerList<TwoParamFunction<T>>   TwoParamFunctions { get; }

        /// <summary>
        /// Функции от двух переменных, в которых знак находится между значениями
        /// </summary>
        public CustomHandlerList<MiddleFunction<T>>     TwoParamMiddleFunctions { get; }

        /// <summary>
        /// Функции от одной переменной
        /// </summary>
        public CustomHandlerList<OneParamFunction<T>>   OneParamFunctions { get; }

        /// <summary>
        /// Функции без переменных
        /// </summary>
        public CustomHandlerList<ZeroParamFunction<T>>  ZeroParamFunctions { get; }

        /// <summary>
        /// Пользовательские функции
        /// </summary>
        public CustomHandlerList<IFunction<T>>          CustomFunctions { get; }

        /// <summary>
        /// Константы
        /// </summary>
        public Dictionary<string, T> Constants { get; } = new Dictionary<string, T>();

        private readonly Dictionary<string, TwoParamFunction<T>>  _twoParamFunctions         = new Dictionary<string, TwoParamFunction<T>>();		
        private readonly Dictionary<char,   MiddleFunction<T>>    _twoParamMiddleFunctions   = new Dictionary<char, MiddleFunction<T>>();	
        private readonly Dictionary<string, OneParamFunction<T>>  _oneParamFunctions         = new Dictionary<string, OneParamFunction<T>>();
        private readonly Dictionary<string, ZeroParamFunction<T>> _zeroParamFunctions        = new Dictionary<string, ZeroParamFunction<T>>();
        private readonly Dictionary<string, IFunction<T>>         _customFunctions           = new Dictionary<string, IFunction<T>>();
		
        private enum ItemTypes
        {
            Function,
            MiddleFunction,
            Element,
            Special,
            Parameter,
            Null
        }

        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="FunctionParser{T}"/> на основе конвертера строки
        /// </summary>
        /// <param name="parser">Функция, возвращающая значение, соответствующее строке</param>
        public FunctionParser(Func<string, T> parser)
        {
            _numberParser = parser;

            TwoParamFunctions = new CustomHandlerList<TwoParamFunction<T>>(() => _twoParamFunctions.Values.GetEnumerator(), it => _twoParamFunctions.Add(it.Name, it), it => _twoParamFunctions.Remove(it.Name));

            TwoParamMiddleFunctions = new CustomHandlerList<MiddleFunction<T>>(() => _twoParamMiddleFunctions.Values.GetEnumerator(), it => _twoParamMiddleFunctions.Add(it.Name[0], it), it => _twoParamMiddleFunctions.Remove(it.Name[0]));

            OneParamFunctions = new CustomHandlerList<OneParamFunction<T>>(() => _oneParamFunctions.Values.GetEnumerator(), it => _oneParamFunctions.Add(it.Name, it), it => _oneParamFunctions.Remove(it.Name));

            ZeroParamFunctions = new CustomHandlerList<ZeroParamFunction<T>>(() => _zeroParamFunctions.Values.GetEnumerator(), it => _zeroParamFunctions.Add(it.Name, it), it => _zeroParamFunctions.Remove(it.Name));

            CustomFunctions = new CustomHandlerList<IFunction<T>>(() => _customFunctions.Values.GetEnumerator(), it => _customFunctions.Add(it.Name, it), it => _customFunctions.Remove(it.Name));
        }

        /// <summary>
        /// Создаёт новый экземпляр математического парсера значений типа double со следующими математическими операциями: +,-,*,/,^,cos,sin,sqrt и константой pi
        /// </summary>
        public static FunctionParser<double> CreateStandartDouble()
        {
            FunctionParser<double> parser = new FunctionParser<double>(double.Parse);
		
            parser.Constants.Add("pi", Math.PI);
		
            parser.OneParamFunctions.Add(new OneParamFunction<double>("cos", Math.Cos));
            parser.OneParamFunctions.Add(new OneParamFunction<double>("sin", Math.Sin));
            parser.OneParamFunctions.Add(new OneParamFunction<double>("sqrt", Math.Sqrt));
		
            parser.TwoParamMiddleFunctions.Add(new MiddleFunction<double>('+', 1, (f, s) => f + s));
            parser.TwoParamMiddleFunctions.Add(new MiddleFunction<double>('-', 1, (f, s) => f - s, s => -s));
            parser.TwoParamMiddleFunctions.Add(new MiddleFunction<double>('*', 2, (f, s) => f * s));
            parser.TwoParamMiddleFunctions.Add(new MiddleFunction<double>('/', 2, (f, s) => f / s));
            parser.TwoParamMiddleFunctions.Add(new MiddleFunction<double>('^', 3, Math.Pow));
		
            return parser;
        }

        /// <summary>
        /// Парсит переданную строку и возвращает вычисляемое значение
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <exception cref="ParserException">Возникает при ошибках парсинга выражения</exception>
        public IEvaluatable<T> Parse(string input)
        {
            HashSet<string> parameters;
            return Parse(input, out parameters);
        }

        /// <summary>
        /// Парсит переданную строку и возвращает вычисляемое значение и список используемых переменных
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <param name="parameters">Список используемых переменных</param>
        /// <exception cref="ParserException">Возникает при ошибках парсинга выражения</exception>
        public IEvaluatable<T> Parse(string input, out HashSet<string> parameters)
        {
            parameters = new HashSet<string>();

            Queue<string> items = new Queue<string>();
            var itemTypes = new Queue<ValueContainer<ItemTypes>>();

            StringBuilder temp = new StringBuilder();

            ItemTypes symb = ItemTypes.Null;
            char prev = '\0';

            foreach (char ch in input)
            {
                if (ch == ' ')
                    continue;

                if (ch >= '0' && ch <= '9' || ch == '.')
                {
                    if (symb != ItemTypes.Element && temp.Length > 0)
                        throw new ParserException("Аргументы функций должны быть в скобках");

                    temp.Append(ch);
                    symb = ItemTypes.Element;
                }
                else if (ch == '(' || ch == ')' || ch == ',')
                {
                    ValueContainer<ItemTypes> last = PushTemp(items, itemTypes, temp, symb);

                    if (ch == '(' && last?.Value == ItemTypes.Parameter)
                        last.Value = ItemTypes.Function;

                    items.Enqueue(ch.ToString());
                    itemTypes.Enqueue(new ValueContainer<ItemTypes>(ItemTypes.Special));
                }
                else
                {
                    if (_twoParamMiddleFunctions.ContainsKey(ch))
                    {
                        if (symb == ItemTypes.Function && prev != ')')
                            symb = ItemTypes.Parameter;

                        PushTemp(items, itemTypes, temp, symb);
                        items.Enqueue(ch.ToString());
                        itemTypes.Enqueue(new ValueContainer<ItemTypes>(ItemTypes.MiddleFunction));
                        temp.Clear();
                        symb = ItemTypes.MiddleFunction;
                    }
                    else if (symb == ItemTypes.Element && temp.Length > 0)
                    {
                        throw new ParserException("Формат \"цифрабуква\" недопустим");
                    }
                    else
                    {
                        temp.Append(ch);
                        symb = ItemTypes.Function;
                    }
                }

                prev = ch;
            }

            if (symb == ItemTypes.Function && prev != ')')
                symb = ItemTypes.Parameter;

            PushTemp(items, itemTypes, temp, symb);

            return ParseDeque(items, itemTypes, null, parameters);
        }
	
        private ItemTypes GetFunctionType(string value, ItemTypes currentType)
        {
            if (currentType != ItemTypes.Function)
                return currentType;
			
            return _twoParamMiddleFunctions.ContainsKey(value[0]) 
                ? ItemTypes.MiddleFunction 
                : 
                _zeroParamFunctions.ContainsKey(value) 
                || _oneParamFunctions.ContainsKey(value)
                || _twoParamFunctions.ContainsKey(value)
                || _customFunctions.ContainsKey(value)
                    ? ItemTypes.Function 
                    : ItemTypes.Parameter;
        }
	
        private ValueContainer<ItemTypes> PushTemp(Queue<string> items, Queue<ValueContainer<ItemTypes>> itemTypes, StringBuilder temp, ItemTypes type)
        {
            if (temp.Length == 0)
                return null;

            string value = temp.ToString();
            items.Enqueue(value);
            var res = new ValueContainer<ItemTypes>(GetFunctionType(value, type));
            itemTypes.Enqueue(res);
            temp.Clear();

            return res;
        }
	
        private IEvaluatable<T> ParseDeque(Queue<string> items, Queue<ValueContainer<ItemTypes>> itemTypes, IFunction<T> function, ISet<string> parameters)
        {
            var tempArr = new List<List<IEvaluatable<T>>>();
            var temp = new List<IEvaluatable<T>>();
		
            tempArr.Add(temp);

            bool brk = false;

            while (items.Count > 0 && !brk)
            {
                string item = items.Dequeue();
                ItemTypes itemType = itemTypes.Dequeue().Value;

                switch (itemType)
                {
                    case ItemTypes.Function:
                        items.Dequeue();
                        itemTypes.Dequeue();

                        if (_oneParamFunctions.ContainsKey(item))
                            temp.Add(ParseDeque(items, itemTypes, _oneParamFunctions[item].CreateNew(), parameters));
                        else if (_twoParamFunctions.ContainsKey(item))
                            temp.Add(ParseDeque(items, itemTypes, _twoParamFunctions[item].CreateNew(), parameters));
                        else if (_zeroParamFunctions.ContainsKey(item))
                            temp.Add(ParseDeque(items, itemTypes, _zeroParamFunctions[item].CreateNew(), parameters));
                        else if (_customFunctions.ContainsKey(item))
                            temp.Add(ParseDeque(items, itemTypes, _customFunctions[item].CreateNew(), parameters));
                        else
                            throw new InvalidFunctionException(item);

                        break;
                    case ItemTypes.Element: 
                        temp.Add(new EvaluatableNumContainer<T>(_numberParser(item)));
                        break;
                    case ItemTypes.MiddleFunction:
                        if (!_twoParamMiddleFunctions.ContainsKey(item[0]))
                            throw new InvalidFunctionException(item[0].ToString());

                        temp.Add(_twoParamMiddleFunctions[item[0]].CreateNew());
                        break;
                    case ItemTypes.Parameter:
                        if (Constants.ContainsKey(item))
                        {
                            temp.Add(new EvaluatableNumContainer<T>(Constants[item]));
                            break;
                        }

                        if (!parameters.Contains(item))
                            parameters.Add(item);

                        temp.Add(new EvaluatableNumContainer<T>(item, true));
                        break;
                    case ItemTypes.Special:
                        if (item == ",")
                        {
                            temp = new List<IEvaluatable<T>>();
                            tempArr.Add(temp);
                        }
                        else if (item == "(")
                        {
                            temp.Add(ParseDeque(items, itemTypes, null, parameters));
                        }
                        else if (item == ")")
                            brk = true;
                        break;
                }
            }
		
            List<IEvaluatable<T>> simplified = new List<IEvaluatable<T>>();
		
            if (tempArr.Count > 1 || tempArr.Count == 1 && tempArr[0].Count > 0)
                foreach (List<IEvaluatable<T>> item in tempArr)
                    simplified.Add(Simplify(item));
			
            if (function != null)
            {
                function.Initialize(simplified);
                return function;
            }
			
            return simplified.Count > 0 ? simplified[0] : null;
        }
	
        private IEvaluatable<T> Simplify(List<IEvaluatable<T>> temp)
        {
            if (temp.Count == 0)
                return null;
			
            List<MiddleFunction<T>> functions = new List<MiddleFunction<T>>();

            var mTemp = temp[0] as MiddleFunction<T>;

            if (mTemp?.Initialized == false)
            {
                mTemp.Initialize(null, temp[1]);
                temp.RemoveAt(1);
            }

            for (int i = 1; i < temp.Count; i+= 2)
            {
                MiddleFunction<T> tmp = temp[i] as MiddleFunction<T>;

                if (tmp == null)
                    throw new ParserException("Неверное представление функции");

                functions.Add(tmp);
            }

            while (temp.Count > 1)
            {
                int max = -1;

                foreach (MiddleFunction<T> func in functions)
                {
                    int priority = func.Priority;

                    if (priority > max)
                        max = priority;
                }

                for (int i = 0; i < functions.Count;)
                {
                    MiddleFunction<T> func = functions[i];

                    if (func.Priority == max)
                    {
                        int findex = i * 2;
                        int fcindex = findex + 1;
                        int sindex = fcindex + 1;

                        func.Initialize(temp[findex], temp[sindex]);

                        temp.RemoveAt(sindex);
                        temp.RemoveAt(findex);

                        functions.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return temp[0];
        }
    }
}
