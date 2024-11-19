# Conference Project

Welcome to the Conference Project!
This project is designed to ilustrate the multithread processing and some of the common problems.

## Present examples

Deadlock
RaceConditions
ThreadSafeQueue

One usage of a Threadsafe collection: this example has a producer and consumer.
The Producer thread will read the file line by line, and the consumers only when find something at the queue will process them.
You have input.txt as a 15 line file example, if you want to see more tou can exchange for `input-bigger.txt` that has 1000 lines.

### Prerequisites

- .NET 8

### Installation

1. **Clone the repository:**
    ```bash
    git clone https://github.com/samuelpampolini/pth-parallel-examples
    cd pth-parallel-examples
    ```
2. **Run the application:**
    ```bash
    dotnet run
    ```
    The application will end when you press ESC key, the only exception is with the Deadlock example that you need to hit `CTRL+C`

    You will be presented with the follow options:
    ```bash
    info: Program[0] Press the number of the example you want to run:
    info: Program[0] 1 - DeadLock
    info: Program[0] 2 - RaceCondition
    info: Program[0] 3 - ThreadContention
    info: Program[0] 4 - ThreadSafeQueue
    ```

