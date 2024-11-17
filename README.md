# KTU-Concurrent_Programming-LAB_01_A
L1. Shared memory
Program a: application of consumer — producer pattern
TL;DR
Picture below shows a generalized view of how the program should work: two monitors
are created — one for data and one for results. Each monitor contains an array, for
unprocessed data and computed results respectively. Main thread reads data from a data
file and writes them to the data monitor, while a selected amount of worker threads
concurrently take (remove) items one by one from the data monitor, compute their
result and write it to the result monitor. Program must handle cases when data monitor
is empty and a worker tries to remove an item as well as when data monitor is full and
main thread wants to add one more item – respective thread must wait until the size of
the array changes. Goal of the main thread — copy all data read from the data file to
the data monitor, wait for workers to finish and write all results from the result monitor
to the result file. Goal of the workers — take (remove) item from data monitor, calculate
result, decide if the result should go to the results and write it to result monitor if yes.
Writing to result monitor should be implemented in such a way that it always remains
sorted. Actions are repeated until all data are processed. It is not required to use
the structure shown in the diagram.
Full description
Choose your own data structure that consists of 3 fields — one string, one int and
one double, an operation that calculates a result from the data structure and a criterion
to filter results by. Selected function should not be trivial so that it takes a bit of time
to compute.
Prepare three data files containing at least 25 elements of your selected structure
each. Data file format is free to choose. It is recommended to use a standard data
format (JSON, XML or other) and use existing libraries to read it. Name of the data file
— Group_LastnameF_L1_dat_x.json (Group — your academic group, LastnameF
— your last name and first letter of your first name, x — file number (3 files are required),
file extension depends on your selected format). You have to prepare three data files:
• Group_LastnameF_L1_dat_1.json — all data matches your selected filter
criteria.
• Group_LastnameF_L1_dat_2.json — part of data matches your selected
filter criteria.
• Group_LastnameF_L1_dat_3.json — no data matches your selected filter
criteria.
Main thread works as follows:
1. Reads the data file to a local array, list or other data structure;
2. Spawns a selected amount of worker threads 2 ≤ x ≤
n
4
(n — amount of data in
the file).
3. Writes each read element to the data monitor. If the monitor is full, the thread is
blocked until there is some free space.
4. Waits for all spawned worker thread to complete.
5. Writes all data from result monitor to the text result file as a table.
Worker threads work as follows:
• Take item from data monitor. If data monitor is empty, worker waits until there is
data in the monitor.
• Computes the function selected by the student for the taken item.
• Checks if the result fits the selected criterion. If yes, the result is added to result
monitor. The result monitor must stay sorted after insert.
• Work is repeated until all data from the file is processed.
Requirements for monitors
• It is recommended to implement the monitors as a class or a structure with at least
two operations: to insert an item and to remove an item. If Rust is used, use Rust
monitors.
• Data are stored in a fixed-size array (list or other data structure is not allowed).
• Data monitor array size cannot be larger than half of the elements in data file.
• Result monitor has enough size to contain all results.
• Operations with the monitor are protected using critical section where needed and
thread blocking is implemented using conditional synchronization, using the tools
of your selected language.
• It should be possible to fill and empty the data monitor (otherwise there is no point
to have it :) ).
Result file should be named Group_LastnameF_L1_rez.txt. File is formatted
as a table with a header, filtered data with computed values are shown in the file
