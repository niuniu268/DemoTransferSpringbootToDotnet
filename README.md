# DEMO backend application by dotnet core 6

## Third party library

- csvhelper
- nlog
- mssql

## Purpose

This assignment consists of two tasks:

1. Preparation: I needed to import CSV files into an MS SQL database (Docker) using C# and CsvHelper. The files students.csv and shifts.csv were successfully imported.
To address the following task, I added a new table called tasks to the MS SQL database. I split the values in the tasks column of the students table and transformed these values into rows in the tasks table, adding a weight column. For example, if there are three tasks in the tasks column of the students table, each task has a weight of 33. If there are only two tasks, each has a weight of 50. For a single task, the weight is 100. This weight distribution simplifies the allocation of work hours.
As a result, I created three tables: students, shifts, and tasks. Following Boyce-Codd Normal Form (BCNF) principles, I set the id column as the primary key in the students table. Additionally, id is the primary key in the shifts table, with appointed_by as a foreign key. In the tasks table, students_id serves as a foreign key referencing the students table.

![Image1](https://github.com/niuniu268/AssignmentHogwarts/raw/master/images/Screenshot%202024-11-02%20at%2011.07.49.png?raw=true)

- Input raw into database

![Image2]()
2. Endpoint Exposure: I have exposed an endpoint that shows the total time each house spent on different tasks. Using the tasks table, we can filter for a task such as "Polishing pots." After joining the shifts, students, and tasks tables, the program returns the aggregated number of hours spent on this task by each house.