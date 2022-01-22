Part List Report Exercise

Fusion Worldwide has a lead with a customer who is building custom PCs and is requesting a part list. 

You are tasked with writing code to generate an optimized part list report for the customer. 

•	Customer’s budget is $1,800.00
•	The customer wants Fusion Worldwide to include the 2 most expensive CPUs.
•	As many non-CPU parts as possible up to the max budget. (Fusion Worldwide wants the total price to be as high/close to customers budget as possible)
•	No part should be listed more than once.

Write an app or script in C# that will download the Fusion Worldwide part list directly from:

https://6f5c9791-a282-482e-bbe9-2c1d1d3f4c9f.mock.pstmn.io/interview/part-list

and generate the expected output:
1.	An integer indicating the total number of parts in the final report.
2.	The total cost in USD of all items in the final report, e.g. “$854.15”.
3.	The items and their respective prices.
o	Output the CPU items first, sorted by price in increasing order.
o	Output the other (non-CPU) items next, sorted by price in increasing order.
