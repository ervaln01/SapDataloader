# SapDataloader
Приложение для выгрузки данных из системы SAP.
Алгоритм заточен под специфичную функцию и специфичную базу данных.
Алгоритм построен следующим образом:
1. Из БД забирается List<string>.
2. Разбивает список на отдельные списки по 25 строк (было выполнено множество тестов для определения этого числа).
3. Для каждого из этих списков выполняется подключение к SAP и выполнение специфичной функции SAP.
4. По итогу выполнения данной функции из её результатов формируется и обрабатывается необходимая таблица, а далее записывается в таблицу БД.
