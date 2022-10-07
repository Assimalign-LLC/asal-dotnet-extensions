# Overview
Cron is a the Linux Operating System Task scheduling program which is similar to Windows Task Scheduler. However unlike Windows Task Scheduler which has a different scheduling format Cron uses Crontab which stands for **cron table**. It allows to use job scheduler, which is known as cron to execute tasks.


## Crontab Expression Format:

```
* * * * *
- - - - -
| | | | |
| | | | +----- day of week (0 - 6) (Sunday=0)
| | | +------- month (1 - 12)
| | +--------- day of month (1 - 31)
| +----------- hour (0 - 23)
+------------- min (0 - 59)

Star (*) in the value field above means all legal values as in
braces for that column. The value column can have a * or a list
of elements separated by commas. An element is either a number in
the ranges shown above or two numbers in the range separated by a
hyphen (meaning an inclusive range).

Source: http://www.adminschoice.com/docs/crontab.htm


Six-part expression format:

* * * * * *
- - - - - -
| | | | | |
| | | | | +--- day of week (0 - 6) (Sunday=0)
| | | | +----- month (1 - 12)
| | | +------- day of month (1 - 31)
| | +--------- hour (0 - 23)
| +----------- min (0 - 59)
+------------- sec (0 - 59)

The six-part expression behaves similarly to the traditional
crontab format except that it can denotate more precise schedules
that use a seconds component.


```

