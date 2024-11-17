# KTU-Concurrent_Programming-LAB_01_A
L1. Shared Memory - Consumer-Producer Pattern Implementation

TL;DR
This program implements the consumer-producer pattern using shared memory. It involves two monitors: one for storing unprocessed data and another for storing computed results. The main thread reads data from a file and writes it to the data monitor. Worker threads concurrently remove data from the monitor, compute results, and add them to the results monitor. The program handles cases when the data monitor is full or empty, ensuring synchronization between threads. Finally, the main thread writes the computed results to a result file.

Full Description
Problem Overview
The program uses shared memory to implement a consumer-producer pattern. The main thread is responsible for reading data from a file and writing it to a data monitor. Worker threads take items from the data monitor, compute results, and write them to a results monitor, ensuring the results monitor remains sorted. The program ensures synchronization between threads when the data monitor is full or empty.

Data Structure
You need to define a custom data structure with the following fields:
- A `string` (e.g., a description or name)
- An `int` (e.g., a quantity or identifier)
- A `double` (e.g., a value or measurement)

Additionally, the structure should include a function that computes a result from the data and a filter criterion for filtering the computed results.

The computation function should not be trivial to ensure that it takes enough time to process. You also need to prepare three data files, each containing at least 25 elements of your selected structure.

Data File Format
You should choose a format for the data files, such as JSON or XML, and use existing libraries to read it. The filenames should follow this format:
- `Group_LastnameF_L1_dat_1.json` – All data matches your selected filter criteria.
- `Group_LastnameF_L1_dat_2.json` – Part of the data matches your selected filter criteria.
- `Group_LastnameF_L1_dat_3.json` – No data matches your selected filter criteria.

Main Thread Workflow
1. Reads the data file into a local array, list, or other data structure.
2. Spawns a specified number of worker threads (2 ≤ x ≤ n/4, where n is the number of data items in the file).
3. Writes each data item to the data monitor. If the monitor is full, the main thread waits until there is space.
4. Waits for all worker threads to finish processing.
5. Writes the results from the result monitor to a result file in a tabular format.

Worker Threads Workflow
1. Each worker thread removes an item from the data monitor. If the data monitor is empty, the worker waits for data.
2. Computes the result based on the selected function.
3. Checks if the result meets the selected filter criterion. If it does, the result is added to the result monitor. The results monitor must remain sorted after each insertion.
4. Repeats the process until all data from the file are processed.

Monitor Requirements
- Implement the data and result monitors as classes or structures with at least two operations: insert an item and remove an item.
- The data monitor array should have a fixed size, not exceeding half of the elements in the data file.
- The result monitor should be large enough to store all results.
- Use synchronization mechanisms like critical sections and conditional synchronization to protect operations and manage thread blocking.
- Ensure that it is possible to fill and empty the data monitor.

Result File Format
The result file should be named `Group_LastnameF_L1_rez.txt`. It should be formatted as a table with a header, showing the filtered data along with the computed values.

---

