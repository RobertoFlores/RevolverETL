# RevolverETL
Tiny ETL System 
Extract, Transform & Load

Main Goals
- Extract information from MySQL, SQL Server, SQLite DBMS, CSV file, Excel File, XML File, Web Service, FTP.
- Transform easily the information extracted.
- Load the information to MySQL, SQL Server, SQLite DBMS

Features
- MultiThread, no blocking approach
- NetMQ based PipeLine System
- Nlog based logger
- Quartz Based Scheduler
- Windows Service Daemon
- UI to set up the Runner

| Item	| Grok variable	| Log example data |
| :---: | ------------- | ---------------- |
|	1	| %{NUMBER:linenumber:int} | 9468 |
|	2	| %{TIME:time} | 17:28:45 |
|	3	| %{WORD:loglevel} | INFO   |

