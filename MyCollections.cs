using System;
using System.Collections;

namespace Graf
{
    /// <summary>
    /// Класс, реализующий структуру данных стек одинакового заданного типа, обслуживаемую по принципу "последним пришел - первым вышел" (LIFO).
    /// </summary>
    /// <typeparam name="T">Задает тип элементов в стеке.</typeparam>
    class MyStack<T>
    {
        //Поля класса MyStack
        /// <summary>
        /// Ссылка на текущий элемент стека ("верхний").
        /// </summary>
        private Node top;
        /// <summary>
        /// Размер стека.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Класс, описывающий значение стека.
        /// </summary>
        public class Node
        {
            //Поля класса Node
            /// <summary>
            /// Значение, которое хранит элемент стека.
            /// </summary>
            public T data;
            /// <summary>
            /// Ссылка на "нижний" элемент стека.
            /// </summary>
            public Node lower;

            //Конструкторы класса Node
            /// <summary>
            /// Конструктор, не принимающий никаких параметров.
            /// Инициализирует новый пустой элемент стека.
            /// </summary>
            public Node()
            {
                data = default;
                lower = null;
            }
            /// <summary>
            /// Конструктор, принимающий только значение элемента стека (ключ).
            /// </summary>
            /// <param name="k">Значение стека.</param>
            public Node(T k)
            {
                data = k;
                lower = null;
            }
            /// <summary>
            /// Конструктор, принимающий значение элемента стека (ключ) и ссылку на "нижний" элемент стека.
            /// </summary>
            /// <param name="k">Значение стека.</param>
            /// <param name="reference">Ссылка на "нижний" элемент стека.</param>
            public Node(T k, Node reference)
            {
                data = k;
                lower = reference;
            }
        }

        /// <summary>
        /// Конструктор без параметров.
        /// Задаёт начальное значение размера стека как нулевое и зануляет ссылку, указывающую на текущий элемент стека.
        /// </summary>
        public MyStack()
        {
            top = null;
            Size = 0;
        }

        /// <summary>
        /// Добавление элемента в верхушку стека.
        /// </summary>
        /// <param name="key">Значение, которое необходимо добавить.</param>
        /// <returns>Ссылка на текущий стек.</returns>
        public MyStack<T> Push(T key)
        {
            ++Size;
            if (IsEmpty())//Если стек пуст
            {
                top = new Node(key);//Создаём новую ссылку на начало
                return this;
            }
            Node temp = new Node(key, top);//Если в стеке есть элементы, то добавляем ещё один элемент с ссылкой на нижний
            top = temp;//Верх стека переходит на созданный элемент
            return this;
        }

        /// <summary>
        /// Просто возвращает первый элемент из стека без его удаления.
        /// </summary>
        /// <returns>Значение верхнего элемента стека.</returns>
        public T Peek()
        {
            return top.data;
        }

        /// <summary>
        /// Извлекает и возвращает первый элемент из стека.
        /// </summary>
        /// <returns>Значение верхнего элемента стека.</returns>
        public T Pop()
        {
            Node temp = top;//Сохраняем значение первого элемента в стеке, чтобы его потом вернуть
            top = top.lower;//Перекидываем ссылку на верх стека на нижний элемент
            --Size;
            return temp.data;
        }

        /// <summary>
        /// Проверка стека на пустоту.
        /// Если ссылка, указывающая на верхний элемент стека, указывает на null, то стек пуст.
        /// </summary>
        /// <returns>Булевское значение, соответствующее пустоте стека.</returns>
        public bool IsEmpty()
        {
            return top == null;
        }

        /// <summary>
        /// Вывод стека в консоль, без извлечения значений из стека.
        /// </summary>
        public void Print()
        {
            Node temp = top;
            while (temp != null)
            {
                Console.Write(temp.data + " ");
                temp = temp.lower;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Вывод значений из стека с извлечением значений.
        /// </summary>
        public void PrintWithException()
        {
            while (!IsEmpty())
                Console.Write(Pop() + " ");
            Console.WriteLine();
        }

        /// <summary>
        /// Отчищает стек.
        /// </summary>
        public void Clear()
        {
            top = null;
            Size = 0;
        }
    }

    /// <summary>
    ///  Класс, реализующий структуру данных очередь одинакового заданного типа, обслуживаемую по принципу "первым пришёл — первым ушёл" (FIFO).
    /// </summary>
    /// <typeparam name="T">Тип элементов в очереди.</typeparam>
    class MyQueue<T>
    {
        //Поля класса MyQueue
        /// <summary>
        /// Ссылка на начало очереди.
        /// </summary>
        private Node top;

        /// <summary>
        /// Размер очереди.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Класс, описывающий значение очереди.
        /// </summary>
        public class Node
        {
            //Поля класса Node
            /// <summary>
            /// Значение, которое хранит элемент очереди.
            /// </summary>
            public T data { get; set; }
            /// <summary>
            /// Ссылка на следующий элемент очереди.
            /// </summary>
            public Node next;

            //Конструкторы класса Node
            /// <summary>
            /// Конструктор, не принимающий никаких параметров.
            /// Инициализирует новый пустой элемент очереди.
            /// </summary>
            public Node()
            {
                data = default;
                next = null;
            }
            /// <summary>
            /// Конструктор, принимающий только значение элемента очереди (ключ).
            /// </summary>
            /// <param name="k">Значение элемента.</param>
            public Node(T k)
            {
                data = k;
                next = null;
            }
            /// <summary>
            /// Конструктор, принимающий значение элемента очереди (ключ) и ссылку на следующий элемент очереди.
            /// </summary>
            /// <param name="k">Значение элемента.</param>
            /// <param name="reference">Ссылку на следующий элемент очереди.</param>
            public Node(T k, Node reference)
            {
                data = k;
                next = reference;
            }
        }

        /// <summary>
        /// Конструктор без параметров.
        /// Задаёт начальное значение размера очереди как нулевое и зануляет ссылки, указывающую на начало очереди.
        /// </summary>
        public MyQueue()
        {
            top = null;
            Size = 0;
        }

        /// <summary>
        /// Добавление элемента в конец очереди.
        /// </summary>
        /// <param name="key">Значение, которое необходимо добавить.</param>
        /// <returns>Ссылка на текущую очередь.</returns>
        public MyQueue<T> Enqueue(T key)
        {
            ++Size;
            if (IsEmpty())//Если очередь пуста
            {
                top = new Node(key);//Создаём новую ссылку на начало очереди
                return this;
            }
            //Если в очереди есть элементы
            Node temp = top;//Переменная для прохождения по очереди
            while (temp.next != null)//Пока не дойдём до последнего элемента очереди
                temp = temp.next;//Перемещаем ссылку на следующий элемент очереди
            temp.next = new Node(key);//Последний элемент очереди ссылается на новый элемент
            return this;
        }

        /// <summary>
        /// Извлекает и возвращает первый элемент очереди.
        /// </summary>
        /// <returns>Значение первого элемента в очереди.</returns>
        public T Dequeue()
        {
            T data = default;//Переменная для значения первого элемента в очереди
            if (IsEmpty())//Если очередь пуста, возвращаем значение по умолчанию
                return data;
            data = top.data;//Сохраняем значение первого элемента в очереди
            --Size;
            if (top.next == null)//Если в очереди один элемент
            {
                top = null;//Очередь пуста
                return data;
            }
            //Если в очереди больше одного элемента
            top = top.next;//Перемещаем ссылку на начало на следующий элемент очереди
            return data;
        }

        /// <summary>
        /// Просто возвращает первый элемент из начала очереди без его удаления.
        /// </summary>
        /// <returns>Значение первого элемента в очереди.</returns>
        public T Peek()
        {
            if (IsEmpty())//Если очередь пуста
                return default;//Возвращает значение по умолчанию
            return top.data;
        }

        /// <summary>
        /// Проверка очереди на пустоту.
        /// Если ссылка, указывающая на начало очереди, указывает на null, то очередь пуста.
        /// </summary>
        /// <returns>Булевское значение, соответствующее пустоте очереди.</returns>
        public bool IsEmpty()
        {
            return top == null;
        }

        /// <summary>
        /// Вывод очереди в консоль без изъятия элементов.
        /// </summary>
        public void Print()
        {
            Node temp = top;
            while (temp != null)
            {
                Console.Write(temp.data + " ");
                temp = temp.next;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Отчищает очередь.
        /// </summary>
        public void Clear()
        {
            top = null;
            Size = 0;
        }
    }

    /// <summary>
    /// Класс, реализующий структуру данных список одинакового заданного типа
    /// </summary>
    /// <typeparam name="T">Тип элементов в списке</typeparam>
    class MyList<T>
    {
        //Поля класса MyList
        /// <summary>
        /// Ссылка на голову списка
        /// </summary>
        private Node head;
        
        /// <summary>
        /// Размер списка
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Класс, описывающий значение списка
        /// </summary>
        public class Node
        {
            //Поля класса Node
            /// <summary>
            /// Значение, которое хранит элемент списка
            /// </summary>
            public T data;
            /// <summary>
            /// Ссылка на следующий элемент списка
            /// </summary>
            public Node next;

            //Конструкторы класса Node
            /// <summary>
            /// Контруктор, не принимающий никаких параметров
            /// Инициализирует новый пустой элемент списка
            /// </summary>
            public Node()
            {
                data = default;
                next = null;
            }
            /// <summary>
            /// Конструктор, принимающий только значение элемента списка (ключ)
            /// </summary>
            /// <param name="k">Ключ</param>
            public Node(T k)
            {
                data = k;
                next = null;
            }
            /// <summary>
            /// Конструктор, принимающий значение элемента списка (ключ) и ссылку на элемент списка
            /// </summary>
            /// <param name="k">Ключ</param>
            /// <param name="reference">Ссылка на элемент списка</param>
            public Node(T k, Node reference)
            {
                data = k;
                next = reference;
            }
        }

        /// <summary>
        /// Конструктор без параметров
        /// Задаёт начальное значение размера списка как нулевое и зануляет ссылку, указывавющую на начало списка
        /// </summary>
        public MyList()
        {
            head = null;
            Size = 0;
        }

        /// <summary>
        /// Перегрузка оператора квадратных скобок для доступа к элеметам списка через их позицию
        /// Индексация начинается с 1
        /// </summary>
        /// <param name="index">Индекс, по которому необходимо получит элемент</param>
        /// <returns>Элемент по запрашиваемому индексу</returns>
        public T this[int index]
        {
            get
            {
                Node temp = GetElment(index);
                return temp.data;
            }
        }
        /// <summary>
        /// Возвращает элемент списка по указанному индексу.
        /// </summary>
        /// <param name="index">Индекс элемента списка.</param>
        /// <returns>Элемент списка.</returns>
        public Node GetElment(int index)
        {
            Node temp = head;
            int count = 0;
            while (count < index)
            {
                temp = temp.next;
                ++count;
            }
            return temp;
        }

        /// <summary>
        /// Добавление элемента в конец списка
        /// </summary>
        /// <param name="key">Добавляемое значение</param>
        /// <returns>Ссылка на текущий список</returns>
        public MyList<T> Add(T key)
        {
            ++Size;
            if (IsEmpty())
            {
                head = new Node(key);
                return this;
            }
            Node temp = head;
            while (temp.next != null)
                temp = temp.next;
            temp.next = new Node(key);
            return this;
        }
        /// <summary>
        /// Добавление элемента в начало списка
        /// </summary>
        /// <param name="key">Добавляемое значение</param>
        /// <returns>Ссылка на текущий список</returns>
        public MyList<T> AddToStart(T key)
        {
            Node temp = new Node(key, head);
            head = temp;
            ++Size;
            return this;
        }
        /// <summary>
        /// Добавление элемента в список по указанной позиции
        /// Нумерация позиций начиная с 1
        /// </summary>
        /// <param name="position">Позиция, по которой надо вставить элемент</param>
        /// <param name="key">Добавляемое значение</param>
        /// <returns>Ссылка на текущий список</returns>
        public MyList<T> InsertElement(int position, T key)
        {
            if (position == 0)
                return AddToStart(key);
            if (Size < position)
                return Add(key);
            Node temp = head;
            int count = 0;
            while (count < position - 1)
            {
                temp = temp.next;
                ++count;
            }
            Node newNode = new Node(key, temp.next);
            temp.next = newNode;
            ++Size;
            return this;
        }

        /// <summary>
        /// Удаление первого элемента в списке
        /// </summary>
        /// <returns>Ссылка на текущий список</returns>
        public MyList<T> DeleteFirst()
        {
            if (IsEmpty())
                return this;
            if (head.next == null)
            {
                Size = 0;
                head = null;
                return this;
            }
            head = head.next;
            --Size;
            return this;
        }
        /// <summary>
        /// Удаление последнего элемента в списке
        /// </summary>
        /// <returns>Ссылка на текущий список</returns>
        public MyList<T> Delete()
        {
            if (head.next != null)
            {
                Node temp1 = head.next;
                Node temp2 = head;
                while (temp1.next != null)
                {
                    temp2 = temp1;
                    temp1 = temp1.next;
                }
                temp2.next = null;
                --Size;
                return this;
            }
            head = null;
            Size = 0;
            return this;
        }
        /// <summary>
        /// Удаление всего списка
        /// </summary>
        /// <returns>Ссылка на текущий список</returns>
        public MyList<T> DeleteAll()
        {
            head = null;
            Size = 0;
            return this;
        }
        /// <summary>
        /// Удаление элемента списка по указанной позиции
        /// </summary>
        /// <param name="position">Позиция, по которой необходимо удалить</param>
        /// <returns>Ссылка на текущий список</returns>
        public MyList<T> DeletePosition(int position)
        {
            if (position < 0 || position >= Size)
                return this;
            if (position == 0)
                return DeleteFirst();
            if (position == Size - 1)
                return Delete();
            Node temp = head;
            int count = 0;
            while (count < position - 1)
            {
                temp = temp.next;
                ++count;
            }
            Node newNode = temp.next;
            temp.next = newNode.next;
            --Size;
            return this;
        }
        //Сделать не int, а T 
        public MyList<T> DeleteElement(int elenent)
        {
            int position = IndexOf(elenent);
            if (position == -1)
                return this;
            return DeletePosition(position);
        }

        /// <summary>
        /// Копирование части списка.
        /// </summary>
        /// <param name="position">Позиция, начиная с которой необходимо начать копировать.</param>
        /// <param name="count">Количество копируемых элементов.</param>
        /// <returns>Копия части списка.</returns>
        public MyList<T> CopyPartOfList(int position, int count)
        {
            MyList<T> copy = new MyList<T>();
            Node temp = head;
            int counter = 0;
            while (counter < position )
            {
                if (counter == position)
                    break;
                temp = temp.next;
                ++counter;
            }
            counter = 0;
            while (counter < count)
            {
                copy.Add(temp.data);
                temp = temp.next;
                ++counter;
            }
            copy.Size = count;
            return copy;
        }

        /// <summary>
        /// Слияние двух списков с созданием нового списка
        /// </summary>
        /// <param name="list">Список, с которым необходимо слить текущий</param>
        /// <returns>Новый список, полученный в результате слияния исходного и переданного списков</returns>
        public MyList<T> MergeListsNew(MyList<T> list)
        {
            MyList<T> mergeList = CopyPartOfList(1, Size);
            Node temp = list.head;
            while (temp != null)
            {
                mergeList.Add(temp.data);
                temp = temp.next;
            }
            return mergeList;
        }
        /// <summary>
        /// Слияние двух списков без создания нового списка
        /// </summary>
        /// <param name="list">Список, с которым необходимо слить текущий</param>
        /// <returns>Исходный список, с добавленными в конец элементами переданного списка</returns>
        public MyList<T> MergeLists(MyList<T> list)
        {
            Node temp = list.head;//Создаём ссылку для прохождения по второму списку
            while (temp != null)//Пока не дойдём до конца второго списка
            {
                Add(temp.data);//Добавляем в текущий список значения второго списка
                temp = temp.next;//Переходим к следующему элементу второго списка
            }
            return this;
        }

        /// <summary>
        /// Печать списка в консоль
        /// </summary>
        public void Print()
        {
            Node temp = head;
            while (temp != null)
            {
                Console.Write(temp.data + " ");
                temp = temp.next;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Проверка списка на пустоту.
        /// Если ссылка, указывающая на голову списка ссылается на null, то список пуст.
        /// </summary>
        /// <returns>Булевское значение, соотвестсвующее пустоте списка.</returns>
        public bool IsEmpty()
        {
            return head == null;
        }

        public void Sort()
        {
            for (int i = 0; i < Size; ++i)
            {
                for (int j = i; j < Size; ++j)
                    if (string.Compare(this[i].ToString(), this[j].ToString()) > 0)
                    {
                        Node temp1 = new Node(this[i]);
                        DeletePosition(i);
                        InsertElement(j - 1, temp1.data);
                        Node temp2 = new Node(this[j]);
                        DeletePosition(j);
                        InsertElement(i, temp2.data);
                    }
            }
        }

        /// <summary>
        /// Возвращает индекс первого вхождения элемента в списке
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(int item)
        {
            Node temp = head;
            int counter = 0;
            while (temp != null)
            {
                if (Convert.ToInt32(temp.data) == item)
                    return counter;
                ++counter;
                temp = temp.next;
            }
            return -1;
        }

        public IEnumerator GetEnumerator() => new Enumerator(this);

        public struct Enumerator : System.Collections.Generic.IEnumerator<T>, IEnumerator
        {
            MyList<T> list;

            public Enumerator(MyList<T> l)
            {
                list = l;
                Cursor = -1;
            }
            /// <summary>
            /// Указатель для передвижения по списку, необходимый для реализации IEnumerator интерфейса.
            /// </summary>
            private int Cursor { get; set; } 

            public T Current { get { return list[Cursor]; } }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (Cursor == list.Size - 1)
                    return false;
                ++Cursor;
                return true;
            }

            public void Reset()
            {
                Cursor = -1;
            }

            public void Dispose() { }
        }
    }
}
