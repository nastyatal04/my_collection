using System;
using System.Drawing;

namespace Graf
{
    class Edge
    {
        /// <summary>
        /// Длина пути.
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Номер вершины, в которую ведёт путь.
        /// </summary>
        public int NextTop { get; set; }

        /// <summary>
        /// Конструктор для инициализации полей класса Edge.
        /// </summary>
        /// <param name="end">Конечная вершина.</param>
        /// <param name="path">Ребро между вершинами.</param>
        public Edge(int end, double path)
        {
            NextTop = end;
            Length = path;
        }
    }

    class MyGraf
    {      
        /// <summary>
        /// Количество вершин в графе.
        /// </summary>
        public int CountTop { get; private set; }
        /// <summary>
        /// Список вершин графа.
        /// </summary>
        public MyList<MyList<int>> Vertices { get; private set; }
        /// <summary>
        /// Список рёбер графа.
        /// </summary>
        public MyList<MyList<Edge>> Edges { get; private set; }
        /// <summary>
        /// Матрица расстояний.
        /// </summary>
        public double[,] Matrix { get; private set; }

        /// <summary>
        /// Константа, показывающая отсутствие пути между вершинами в матрице расстояний.
        /// </summary>
        const double INF = (double.MaxValue-1)/2;
        int[] d;//время входа в вершину
        int[] f;//время выхода из вершины
        int[] pi;
        private int time;//для dfs
        Color[] colorTop;
        bool[] Visited;
        int[,] P;//массив для построения конечных путей для Флойда
        double[,] copy_Matrix;//копия матрицы растояний для Флойда
        MyStack<int> Cv;//стек для хранения вершин для эйлеровых циклов
        MyList<int> res_f_DFS;//хранит вершины в порядке увеличения времени выхода
        MyList<int> component;//хранит сильно связные компоненты
        MyList<int> DFS_result;//хранит порядок обхода графа при поиске в глубину.
        int[] parent;
        MyList<int> GPath;//гамильтонов цикл
        MyList<int> MSTKruskal;

        /// <summary>
        /// Конструктор для инициализации всех полей класса.
        /// </summary>
        /// <param name="matrix">Матрица.</param>
        public MyGraf(double[,] matrix)
        {
            CountTop = matrix.GetLength(0);
            Matrix = matrix;
            Vertices = new MyList<MyList<int>>();
            Edges = new MyList<MyList<Edge>>();

            InitPeremForAlg();

            for (int i = 0; i < CountTop; i++)
            {
                Vertices.Add(new MyList<int>());
                Edges.Add(new MyList<Edge>());
                for (int j = 0; j < CountTop; ++j)
                    if (matrix[i, j] != 0)
                        AddTop(i, j, matrix[i, j]);
            }
        }
        /// <summary>
        /// Конструктор для инициализации всех полей класса.
        /// </summary>
        /// <param name="vertices">Список смежности графа.</param>
        public MyGraf(MyList<MyList<int>> vertices)
        {
            CountTop = vertices.Size;
            Vertices = new MyList<MyList<int>>();
            Matrix = new double[CountTop, CountTop];
            Edges = new MyList<MyList<Edge>>();

            //Заполнили матрицу начальными значениями
            for (int i = 0; i < CountTop; ++i)
                for (int j = 0; j < CountTop; ++j)
                    Matrix[i, j] = 0;

            InitPeremForAlg();

            for (int i = 0; i < CountTop; ++i)
                foreach (int v in vertices[i])
                    Matrix[i, v] = 1;

            for (int i = 0; i < CountTop; i++)
            {
                Vertices.Add(new MyList<int>());
                Edges.Add(new MyList<Edge>());
                for (int j = 0; j < CountTop; ++j)
                    if (Matrix[i, j] != 0)
                        AddTop(i, j, Matrix[i, j]);
            }
        }
        /// <summary>
        /// Создание переменных, необходимых для реализации алгоритмов.
        /// </summary>
        private void InitPeremForAlg()
        {
            pi = new int[CountTop];             //подграф предшествования???
            f = new int[CountTop];
            d = new int[CountTop];            //массив растояний
            colorTop = new Color[CountTop]; //возможные цвета вершин -> чёрный, серый, белый
            Visited = new bool[CountTop]; //массив посещения вершин
            Cv = new MyStack<int>();
            res_f_DFS = new MyList<int>();
            component = new MyList<int>();
            DFS_result = new MyList<int>();
            P = new int[CountTop, CountTop];
            copy_Matrix = new double[CountTop, CountTop];
            parent = new int[CountTop];
            GPath = new MyList<int>();
            MSTKruskal = new MyList<int>();
        }
        /// <summary>
        /// Добавление вершины и ребра в структуру графа.
        /// </summary>
        /// <param name="start">Началная вершина.</param>
        /// <param name="end">Конечная вершина.</param>
        /// <param name="path">Длина пути между начальной и конечной вершиной.</param>
        private void AddTop(int start, int end, double path)
        {
            Vertices[start].Add(end);
            Edges[start].Add(new Edge(end, path));
        }

        /// <summary>
        /// Проверка переданной вершины
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private bool VertexCheck(int v)
        {
            return (v < CountTop && v >= 0);
        }

        //--- Поиск в ширину ---
        /// <summary>
        /// Поиск в ширину (волновой алгоритм). Использует представление графа в виде списка смежности.
        /// </summary>
        /// <param name="v">Номер вершины, с которой начинаем обход.</param>
        public void WidthSearch(int v)
        {
            if (!VertexCheck(v))//проверяем прпереданную вершину
                return;
            MyList<int> traversal = new MyList<int>();  //хранит порядок обхода (надо ли вообще???)
            MyQueue<int> Delayed = new MyQueue<int>();  //
            for (int u = 0; u < CountTop; ++u)          //пока ещё все вершины считаем непосещёнными
                Visited[u] = false;
            Delayed.Enqueue(v);
            Visited[v] = true;
            do
            {
                int current = Delayed.Dequeue();
                traversal.Add(current);
                foreach (int u in Vertices[current])
                {
                    if (!Visited[u])
                    {
                        Delayed.Enqueue(u);
                        Visited[u] = true;
                    }
                }
            } while (!Delayed.IsEmpty());//пока стек не пуст
            traversal.Print();
        }
        /// <summary>
        /// Поиск в ширину. Использует представление графа в виде матрицы смежности.
        /// </summary>
        /// <param name="v">Номер вершины, с которой начинаем обход.</param>
        public void SW(int v)
        {
            if (!VertexCheck(v))//проверяем прпереданную вершину
                return;
            MyList<int> traversal = new MyList<int>();
            MyQueue<int> Delayed = new MyQueue<int>();
            for (int u = 0; u < CountTop; ++u)          //пока ещё все вершины считаем непосещёнными
                Visited[u] = false;
            Delayed.Enqueue(v);
            Visited[v] = true;
            while (!Delayed.IsEmpty())//Пока очередь не пуста???
            {
                v = Delayed.Dequeue();
                traversal.Add(v);
                for (int j = 0; j < CountTop; ++j)
                    if (Matrix[v, j] != 0 && Visited[j] == false)
                    {
                        Delayed.Enqueue(j);
                        Visited[j] = true;
                    }
            }
            traversal.Print();
        }
        /// <summary>
        /// Поиск в ширину
        /// </summary>
        /// <param name="v">Номер вершины, с которой начинаем обход.</param>
        public void BFS(int v)
        {
            if (!VertexCheck(v))//проверяем прпереданную вершину
                return;
            MyList<int> traversal = new MyList<int>();
            for (int i = 0; i < CountTop; ++i)
            {
                colorTop[i] = Color.White;          //все вершины ещё непосещены
                d[i] = int.MaxValue;                //пока что все расстояния принимаем за максимально возможные
                pi[i] = -1;                          //предшественников ещё нет
            }
            //Отмечаем как находящуюся в рассмотрении входную вершину
            colorTop[v] = Color.Gray;
            d[v] = 0;
            pi[v] = -1;
            MyQueue<int> Delayed = new MyQueue<int>();//очередь для хранения множества серых вершин (т.е. находящихся в рассмотрении)
            Delayed.Enqueue(v);
            while (!Delayed.IsEmpty())
            {
                int current = Delayed.Peek();
                foreach (int u in Vertices[current])
                    if (colorTop[u] == Color.White)
                    {
                        colorTop[u] = Color.Gray;
                        d[u] = d[current] + 1;
                        pi[u] = current;
                        Delayed.Enqueue(u);
                    }
                traversal.Add(current);
                Delayed.Dequeue();
                colorTop[current] = Color.Black;
            }
            traversal.Print();
            foreach (int i in d)
                Console.Write(i + " ");
            Console.WriteLine();
            foreach (int i in pi)
                Console.Write(i + " ");
            Console.WriteLine();
        }
        /// <summary>
        /// Печать кратчайших путей
        /// </summary>
        /// <param name="s">из</param>
        /// <param name="v">в</param>
        public void PrintPath(int s, int v)
        {
            if (v == s)
                Console.Write(s + "-");
            else if (pi[v] == -1)
                Console.WriteLine("Пути из " + s + " в " + v + " нет");
            else
            {
                PrintPath(s, pi[v]);
                Console.WriteLine(v);
            }
        }

        //--- Поиск в глубину ---
        /// <summary>
        /// Рекурсивный алгоритм поиска в глубину.
        /// </summary>
        /// <param name="v">Номер вершины, с которой начинаем обход.</param>
        public void DertSearch(int v)
        {
            for (int u = 0; u < CountTop; ++u)          //пока ещё все вершины считаем непосещёнными
                Visited[u] = false;
            DertSearchRealization(v);
            //Надо ли то, что ниже??????????????????????????????????
            for(int i = 0; i<CountTop; ++i)
                if (!Visited[i])
                    DertSearchRealization(i);
        }
        /// <summary>
        /// Реализация рекурсивного алгоритма поска в глубину.
        /// </summary>
        /// <param name="v">Номер вершины, с которой начинаем обход.</param>
        private void DertSearchRealization(int v)
        {
            Visited[v] = true;
            Console.Write(v + " ");
            foreach (int u in Vertices[v])
                if (!Visited[u])
                    DertSearchRealization(u);
        }
        /// <summary>
        /// Нерекурсивный алгоритм поиска в глубину.
        /// </summary>
        /// <param name="v">Номер вершины, с которой начинаем обход.</param>
        public void Pgn(int v)
        {
            MyStack<int> St = new MyStack<int>();       //стек для хранения проверенных вершин
            for (int u = 0; u < CountTop; ++u)          //пока ещё все вершины считаем непосещёнными
                Visited[u] = false;
            St.Push(v);
            Console.Write(v + " ");
            Visited[v] = true;
            while (!St.IsEmpty())
            {
                int j = 0, pp = 0;
                do
                {
                    if (Matrix[v, j] != 0 && Visited[j] == false)
                        pp = 1;
                    else
                        j++;
                } while (pp == 0 && j < CountTop);
                if (pp == 1)
                {
                    Console.Write(j + " ");
                    St.Push(j);
                    Visited[j] = true;
                }
                v = St.Pop();
            }
        }
        /// <summary>
        /// Поиск в глубину, проходящий по всем вершинам графа.
        /// </summary>
        public void DFS()
        {
            time = 0;
            for (int i = 0; i < CountTop; ++i)
            {
                colorTop[i] = Color.White;
                pi[i] = -1;
                d[i] = 0;
                f[0] = 0;
            }
            for (int i = 0; i < CountTop; ++i)
                if (colorTop[i] == Color.White)
                    DFS_Visit(i);            
            //DFS_result.Print();
        }
        /// <summary>
        /// Рассмортение вершины при поиске в глубину.
        /// </summary>
        /// <param name="i">Номер текущей вершины.</param>
        private void DFS_Visit(int i)
        {
            colorTop[i] = Color.Gray;
            DFS_result.Add(i);
            d[i] = ++time;
            component.Add(i);
            foreach (int j in Vertices[i])
                if (colorTop[j] == Color.White)
                {
                    pi[j] = i;
                    DFS_Visit(j);
                }
            res_f_DFS.Add(i);
            colorTop[i] = Color.Black;
            DFS_result.Add(i);
            f[i] = ++time;
        }

        //--- Эйлеровы циклы ---
        /// <summary>
        /// Поиск эйлерова цикла в графе.
        /// </summary>
        /// <param name="v">Номер вершины, с которой начинаем обход.</param>
        public void E_Circle(int v)
        {
            //нужна ли проверка, что граф являетя эйлеровым?
            //сохраняем матрицу смежности, т.к. в процессе работы алгоритма она будет изменена
            double[,] temp = new double[CountTop, CountTop];
            for (int i = 0; i < CountTop; ++i)
                for (int j = 0; j < CountTop; ++j)
                    temp[i, j] = Matrix[i, j];
            E_Circle_Realis(v);
            Matrix = temp;                       //восстанавливаем матрицу смежности
            Print_E_Circle();               //печатаем полученный эйлеров цикл
        }
        private void E_Circle_Realis(int v)
        {
            for (int j = 0; j < CountTop; ++j)
                if (Matrix[v, j] != 0)
                {
                    Matrix[v, j] = 0;
                    Matrix[j, v] = 0;
                    E_Circle_Realis(j);
                }
            Cv.Push(v);
        }
        public void Print_E_Circle()
        {
            Cv.Print();
        }
        /// <summary>
        /// Нерекурсивная реализация поиска эйлеровых циклов.
        /// </summary>
        /// <param name="v"></param>
        public void E_Circle_NR(int v)
        {
            Cv.Clear();
            //создаём копию списка смежности
            MyList<MyList<int>> SpSm = new MyList<MyList<int>>();
            for (int i = 0; i < CountTop; ++i)
            {
                SpSm.Add(new MyList<int>());
                for (int j = 0; j < Vertices[i].Size; ++j)
                    SpSm[i].Add(Vertices[i][j]);                
            }
            Cv.Push(v);
            while(!Cv.IsEmpty())
            {
                v = Cv.Peek();
                if(SpSm[v].IsEmpty())
                    Console.Write(Cv.Pop() + " ");
                else
                {
                    foreach (int u in SpSm[v])
                    {
                        Cv.Push(u);
                        SpSm[v].DeleteElement(u);
                        SpSm[u].DeleteElement(v);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Основная функция, печатающая эйлеров путь. Сначала он находит вершину нечетной степени (если есть любое), а затем вызывает printEulerUtil() для напечатать путь
        /// </summary>
        public void Fleury()
        {
            //Найдите вершину с нечетной степенью
            int u = 0;
            for (int i = 0; i < CountTop; i++)
                if (Vertices[i].Size % 2 == 1)
                {
                    u = i;
                    break;
                }
            // Распечатать тур, начиная с одв
            printEulerUtil(u);
            Console.WriteLine();
        }
        // Выведите эйлеровский тур, начиная с вершины u
        private void printEulerUtil(int u)
        {
            // Повторять для всех вершин, смежных с этой вершиной
            for (int i = 0; i < Vertices[u].Size; i++)
            {
                int v = Vertices[u][i];

                // Если ребро u-v является допустимым следующим ребром
                if (isValidNextEdge(u, v))
                {
                    Console.Write(u + "-" + v + " ");

                    // Это ребро уже используется, поэтому удалите его сейчас.
                    removeEdge(u, v);
                    printEulerUtil(v);
                }
            }
        }
        private void addEdge(int u, int v)
        {
            Vertices[u].Add(v);
            Vertices[v].Add(u);
        }
        private void removeEdge(int u, int v)
        {
            Vertices[u].DeleteElement(v);
            Vertices[v].DeleteElement(u);
        }
        /// <summary>
        /// Функция для проверки того, можно ли рассматривать ребро u-v как следующее ребро в Euler Tour
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private bool isValidNextEdge(int u, int v)
        {
            // Ребро u-v допустимо в одном из следующих двух случаев:

            // 1) Если v является единственной смежной вершиной u, т.е. размер списка смежных вершин равен 1
            if (Vertices[u].Size == 1)
            {
                return true;
            }

            //2) Если есть несколько соседей, то u - v не является мостом.Выполните следующие шаги, чтобы проверить, является ли u-v мостом.
            // 2.a) количество вершин, достижимых из u
            bool[] isVisited = new bool[CountTop];
            int count1 = dfsCount(u, isVisited);

            // 2.b) Удалить ребро (u, v) и после удаления ребра подсчитать вершины, достижимые из u
            removeEdge(u, v);
            isVisited = new bool[CountTop];
            int count2 = dfsCount(u, isVisited);

            // 2.c) Добавьте ребро обратно в граф
            addEdge(u, v);
            return (count1 > count2) ? false : true;
        }
        // Функция на основе DFS для подсчета достижимых вершин из v
        private int dfsCount(int v, bool[] isVisited)
        {
            // Отметить текущий узел как посещенный
            isVisited[v] = true;
            int count = 1;

            // Повторять для всех вершин, смежных с этой вершиной
            foreach (int i in Vertices[v])
            {
                if (!isVisited[i])
                {
                    count = count + dfsCount(i, isVisited);
                }
            }
            return count;
        }

        // --- Гамильтоновы циклы ---
        public bool G_Circle(int curr = 0)
        {
            GPath.Add(curr);
            if (GPath.Size == CountTop)//Если в список занесены все вершины
                if (Matrix[GPath[0], GPath[GPath.Size - 1]] == 1)
                    return true;
                else
                {
                    GPath.Delete();
                    return false;
                }
            Visited[curr] = true;
            for (int nxt = 0; nxt < CountTop; ++nxt)
                if (Matrix[curr, nxt] == 1 && !Visited[nxt])
                    if (G_Circle(nxt))
                        return true;
            Visited[curr] = false;
            GPath.Delete();
            return false;

        }
        public void G_Circle_Print()
        {
            if (GPath.Size == 0)
                return;
            foreach (var el in GPath)
                Console.Write(el + " ");
            Console.WriteLine();
        }

        //--- Сильно связные компоненты ---
        /// <summary>
        /// Алгортим поиска сильно связных компонент.
        /// </summary>
        public void SCС()
        {
            DFS();//заполнили res_f_DFS для нашего графа
            MyGraf graf =  Transposition();//транспонировали матрицу исходного графа
            for (int i = 0; i < CountTop; ++i)
            {
                graf.colorTop[i] = Color.White;
                graf.pi[i] = -1;
                graf.d[i] = 0;
                graf.f[0] = 0;
            }
            for (int i = CountTop - 1; i >= 0; --i)
                if (graf.colorTop[res_f_DFS[i]] == Color.White)
                {
                    graf.DFS_Visit(res_f_DFS[i]);
                    graf.component.Print();
                    graf.component.DeleteAll();
                }
        }
        /// <summary>
        /// Транспонирование матрицы смежности. необходимо в алгоритме поиска сильно связных компонент.
        /// </summary>
        private MyGraf Transposition()
        {
            double[,] matrix = new double[CountTop, CountTop];
            for (int i = 0; i < CountTop; ++i)
                for (int j = 0; j < CountTop; ++j)
                {
                    matrix[j, i] = Matrix[i, j];
                    matrix[i, j] = Matrix[j, i];
                }
            return new MyGraf(matrix);
        }

        //--- Вывод матрицы ---
        /// <summary>
        /// Вывод списка смежности графа.
        /// </summary>
        public void Print()
        {
            for(int i =0; i<CountTop;++i)
            {
                Console.Write(i + "-->");
                for(int j =0; j<Vertices[i].Size;++j)
                {
                    Console.Write(Vertices[i][j] + "(" + Edges[i][j].Length + ")");
                    if (j != Vertices[i].Size - 1)
                        Console.Write("->");
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Вывод матрицы расстояний.
        /// </summary>
        public void Print_Matrix()
        {
            for (int i = 0; i < CountTop; ++i)
            {
                for (int j = 0; j < CountTop; ++j)
                    Console.Write(Matrix[i, j] + " ");
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Вывод матрицы инцедентности.
        /// </summary>
        public void MatrixInc()
        {
            int countEdge = CountEdges();
            int[,] matrixInc = new int[CountTop, countEdge];
            for (int i = 0; i < CountTop; ++i)
                for (int j = 0; j < countEdge; ++j)
                    matrixInc[i, j] = 0;
            int edge = 0;
            for (int i = 0; i < CountTop; ++i)
                for (int j = i; j < CountTop; ++j)
                    if (Matrix[i, j] != 0)
                    {
                        matrixInc[i, edge] = 1;
                        matrixInc[j, edge++] = -1;
                    }
            for (int i = 0; i < CountTop; ++i)
            {
                for (int j = 0; j < countEdge; ++j)
                    Console.Write(matrixInc[i, j] + "\t");
                Console.WriteLine();
            }

        }

        //--- Свойства графа ---
        /// <summary>
        /// Метод определения взвешенности графа.
        /// </summary>
        /// <returns></returns>
        public bool IsWeightedGraph()
        {
            for (int i = 0; i < CountTop; ++i)
                for (int j = 0; j < CountTop; ++j)
                    if (Matrix[i, j] > 1 && Matrix[i, j] != 0)
                        return true;
            return false;
        }
        /// <summary>
        /// Определение ориетнированности графа
        /// </summary>
        /// <returns></returns>
        public bool IsDirectedGraph()
        {
            for (int i = 0; i < CountTop; ++i)
                for (int j = 0; j < CountTop; ++j)
                    if (Matrix[i, j] != Matrix[j, i])
                        return true;
            return false;
        }
        /// <summary>
        /// Количество рёбер в графе.
        /// </summary>
        /// <returns>Количество рёбер в графе.</returns>
        public int CountEdges()
        {
            int countEdge = 0;
            for (int i = 0; i < CountTop; ++i)
                foreach (int v in Vertices[i])
                    ++countEdge;
            return (IsDirectedGraph()) ? countEdge : countEdge / 2;
        }
        /// <summary>
        /// Степени вершин графа.
        /// </summary>
        public void StV()
        {
            MyList<int> st = new MyList<int>();
            for (int i = 1; i < CountTop + 1; ++i)
                Console.Write(i + " ");
            Console.WriteLine();
            for (int i = 0; i < CountTop; ++i)
                st.Add(Vertices[i].Size);
            st.Print();
        }

        //--- Нахождение кратчайших путей на графах ---
        /// <summary>
        /// Алгоритм Дейкстры.
        /// </summary>
        /// <param name="S">Начальная вершина.</param>
        public void Dijkstra(int S)
        {
            double[] Distance = new double[CountTop];//кратчайшие расстояния
            for (int i = 0; i < CountTop; ++i)
            {
                Distance[i] = INF;
                Visited[i] = false;
            }
            Distance[S] = 0;
            double MinD;
            do
            {
                MinD = INF;
                int MinV = -1;
                for (int i = 0; i < CountTop; ++i)
                    if (Distance[i] < MinD && !Visited[i])
                    {
                        MinD = Distance[i];
                        MinV = i;
                    }
                if (MinV == -1)
                    break;
                for (int i = 0; i < CountTop; ++i)
                    if (Matrix[MinV, i] != 0 && !Visited[i])
                        Distance[i] = Math.Min(Distance[i], Distance[MinV] + Matrix[MinV, i]);
                Visited[MinV] = true;
            }
            while (MinD < INF);
            Console.WriteLine("Кратчайшие расстояния до вершин:");
            PrintByVertices(Distance);
        }
        /// <summary>
        /// Функция для вывода значений кратчайшего расстояния для каждой вершины
        /// </summary>
        /// <param name="D"></param>
        void PrintByVertices(double[] D)
        {
            for (int i = 0; i < CountTop; ++i)
                Console.Write("{0,6}", i);
            Console.WriteLine();
            for (int i = 0; i < CountTop; ++i)
                if (D[i] == INF)
                    Console.Write("{0,6}", '-');
                else
                    Console.Write("{0,6}", D[i]);
            Console.WriteLine();
        }

        public void Floyd()
        {
            if (!(IsWeightedGraph() || IsDirectedGraph()))
            {
                Console.WriteLine("Алгоритм не работает на неориентированном или невзвешенном графе.");
                return;
            }
            for (int i = 0; i < CountTop; ++i)
                for (int j = 0; j < CountTop; ++j)
                    if (Matrix[i, j] == 0 && i != j)
                        copy_Matrix[i, j] = INF;
                    else
                        copy_Matrix[i, j] = Matrix[i, j];
            for (int k = 0; k < CountTop; ++k)
                for (int i = 0; i < CountTop; ++i)
                    for (int j = 0; j < CountTop; ++j)
                        if (copy_Matrix[i, k] + copy_Matrix[k, j] < copy_Matrix[i, j])
                        {
                            copy_Matrix[i, j] = copy_Matrix[i, k] + copy_Matrix[k, j];
                            P[i, j] = k;
                        }
            ShortDistBetweenVertices();
        }
        /// <summary>
        /// Вывод матрицы кратчайших расстояний между вершинами, полученной в результате работы алгоритма Флойда.
        /// </summary>
        private void ShortDistBetweenVertices()
        {
            for (int i = 0; i < CountTop; ++i)
                for (int j = 0; j < CountTop; ++j)
                    if(copy_Matrix[i,j]!=0 && copy_Matrix[i,j]!=INF)
                    {
                        Console.Write($"{i+1}-{j+1} ({copy_Matrix[i,j]}) ");
                        Path(i, j);
                        Console.WriteLine();
                    }    
        }
        private void Path(int i, int j)
        {
            int k = P[i, j];
            if (k == 0)
                return;
            Path(i, k);
            Console.Write(k + " ");//печать к
            Path(k, j);
        }

        //--- Построение минимального остовного дерева ---
        //--- Алгоритм Прима ---
        /// <summary>
        /// Вспомогательная функция для поиска вершины с минимальным значением ключа из набора вершин, еще не включенных в мин.ост.дерево.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mstSet"></param>
        /// <returns></returns>
        private int minKey(double[] key, bool[] mstSet)
        {
            double min = double.MaxValue;
            int min_index = -1;
            for (int v = 0; v < CountTop; v++)
                if (mstSet[v] == false && key[v] < min)
                {
                    min = key[v];
                    min_index = v;
                }
            return min_index;
        }
        /// <summary>
        /// Вспомогательная функция для печати построенного MST, сохраненного в parent[]
        /// </summary>
        /// <param name="parent"></param>
        private void printMST(int[] parent)
        {
            Console.WriteLine("Edge \tWeight");
            for (int i = 1; i < CountTop; i++)
                Console.WriteLine(parent[i] + " - " + i + "\t" + Matrix[i, parent[i]]);
        }
        /// <summary>
        /// Алгоритм Прима. Функция для построения и печати мин ост дерева для графа, представленного с использованием представления матрицы смежности.
        /// </summary>
        public void PrimMST()
        {
            // Массив для хранения построенного MST
            int[] parent = new int[CountTop];
            // Ключевые значения, используемые для выбора ребра минимального веса в разрезе
            double[] key = new double[CountTop];
            // Для представления набора вершин, включенных в MST
            bool[] mstSet = new bool[CountTop];
            // Инициализировать все ключи как INF
            for (int i = 0; i < CountTop; i++)
            {
                key[i] = int.MaxValue;
                mstSet[i] = false;
            }
            //Всегда включайте первую 1-ю вершину в MST. Сделайте ключ 0, чтобы эта вершина выбиралась как первая вершина. Первый узел всегда является корнем MST.
            key[0] = 0;
            parent[0] = -1;
            // MST будет иметь V вершин
            for (int count = 0; count < CountTop - 1; count++)
            {
                // Выберите минимальную ключевую вершину из набора вершин, еще не включенных в MST.
                int u = minKey(key, mstSet);
                // Добавьте выбранную вершину в набор MST.
                mstSet[u] = true;
                // Обновите значение ключа и родительский индекс смежных вершин выбранной вершины. Учитывать только те вершины, которые еще не включены в MST
                for (int v = 0; v < CountTop; v++)
                    // Matrix[u][v] отличен от нуля только для смежных вершин m mstSet[v] ложен для вершин, еще не включенных в MST Обновлять ключ, только если Matrix[u][v] меньше ключа[v]
                    if (Matrix[u, v] != 0 && mstSet[v] == false && Matrix[u, v] < key[v])
                    {
                        parent[v] = u;
                        key[v] = Matrix[u, v];
                    }
            }
            // Распечатать построенный MST
            printMST(parent);
        }
        //--- Алгоритм Крускала ---
        private const int MAX = 100;
        private int _edgesCount;
        private int[,] tree;
        private int[] sets;

        public double Cost { get; private set; }

        private void ArrangeEdges(int k)
        {
            Edge temp;
            for (int i = 1; i < k; i++)
            {
                for (int j = 1; j <= k - i; j++)
                {
                    if (Edges[i][j].Length > Edges[i][j+1].Length)
                    {
                        temp = Edges[i][j];
                        Edges[i][j] = Edges[i][j + 1];
                        Edges[i][j + 1] = temp;
                    }
                }
            }
        }

        private int Find(int vertex)
        {
            return (sets[vertex]);
        }

        private void Join(int v1, int v2)
        {
            if (v1 < v2)
                sets[v2] = v1;
            else
                sets[v1] = v2;
        }

        public void BuildSpanningTree()
        {
            for (int i = 1; i <= CountTop; i++) sets[i] = i;
            int k = _verticlesCount;
            int i, t = 1;
            this.ArrangeEdges(k);
            this.Cost = 0;
            for (i = 1; i <= k; i++)
            {
                for (i = 1; i < k; i++)
                    if (Find(_edges[i].U) != this.Find(_edges[i].V))
                    {
                        tree[t, 1] = _edges[i].U;
                        tree[t, 2] = _edges[i].V;
                        this.Cost += _edges[i].Weight;
                        this.Join(_edges[t].U, _edges[t].V);
                        t++;
                    }
            }
            DisplayInfo();
        }

        private void DisplayInfo()
        {
            Console.WriteLine("The Edges of the Minimum Spanning Tree are:");
            for (int i = 1; i < CountTop; i++)
                Console.WriteLine(tree[i, 1] + " - " + tree[i, 2]);
        }





        //int[] St;//массив для хранения номеров вершин
        //int yk, num;//указатель записи в массиве st, хз
        //int[] Gnum;
        ///// <summary>
        ///// Вызов FS_Circle()
        ///// </summary>
        //public void FS_Circle(int v)
        //{
        //    St = new int[CountTop];
        //    Gnum = new int[CountTop];
        //    num = 0;
        //    yk = 0;
        //    for (int i = 0; i < CountTop; ++i)
        //        Gnum[i] = 0;
        //    FS_Circle_Realis(v);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="v">Вершина с которой начинаем обход</param>
        //public void FS_Circle_Realis(int v)
        //{
        //    yk++;
        //    St[yk] = v;
        //    num++;
        //    Gnum[v] = num;
        //    for (int j = 0; j < CountTop; ++j)
        //        if (A[v, j] != 0)
        //            if (Gnum[j] == 0)
        //                FS_Circle_Realis(j);
        //            else if (j != St[yk - 1] && Gnum[j] < Gnum[v])
        //                foreach (int el in St)
        //                    Console.WriteLine(el);
        //    yk--;
        //}

        ////Алгоритм Беллмана-Форда
        //public int[] ABF(int s)
        //{
        //    int[] d = new int[CountTop];
        //    for (int i = 0; i < CountTop; ++i)
        //        d[i] = int.MaxValue;
        //    d[s] = 0;
        //    for(int i =0; i<CountTop -1; ++i)
        //        for(int u = 0; u<CountTop; ++u)
        //            for(int v= 0; v<CountTop; ++v)
        //                if(A[v,i]>A[u,i-1]+Edges(u,v))
        //                {
        //                    A[v, i] = A[u, i - 1] + Eges[u, v];
        //                    P[v, i] = u;
        //                }
        //    return d;
        //}
        ////восстановление кратчайшего пути из алг.Б-Ф.
        //public int[] vost_kr_path(int i, int j)
        //{
        //    while(j>0)
        //    {
        //        d[j] = i;
        //        i = P[i, j];
        //        j = j - 1;
        //    }
        //    return d;
        //}
    }
}
