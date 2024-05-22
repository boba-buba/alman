# Overview
- [Alman](#alman)
- [SW engineering aspects and place for ideas](#sw-engineering-aspects-and-place-for-ideas)
- [Specification](#technologies-that-will-be-used-from-subjects)

## ALMAN
Desctop application for managing kindergarten. Cycle in kindergarten is month. The administrator fills the data in the end of the month, including all incomes and exppenses and calculates revenue.
Application will be developed with regard of time periods. Children are diveded to 4-5 groups based on their age. Apart from children the system manages staff, typical for kindergarten expenses like food, communal fees, advertisement, internet fees, ... 

The application is developed for private kidergarten with atmost 70 children. 

## SW engineering aspects and place for ideas

In the kindergarten lessons are taught. Lesons are divided to two categories: 
- ones which subscription doesnt cover. They must be paid separetely
- ones which subscription cover and they dont need to paid.

### Sources of income
- Subscription
- Lesons that must be paid
- Doctor payments
- Contribution fee

### Types of children
- contract
- preliminary contract

### User stories
Administrator should be able to:
- add new lesson
- delete lesson that is not taught
- create new group (to add children)
- delete group where there are no children
- add child to the group (when the child in not in any)
- remove child from the group
- create new child's profile
- delete child's profile
- set type of the payment for the child (subscription/maternal capital/staff's child)
- set price of the subscription for the child (can differ if there are two children from one family, if staff's child)
- fill the dates when the child was present on the particular lesson during particular month.
- fill what was paid for the lesson for particular month for the child.
- get final sum that the parents must pay to the kindergarten or kindegarten must pay to parents (if parents paid in advance and child was not present, then the price is recalculated)
- set if the paiment was charging to account or in cache
- produce final invoice what must be paid by the parents.
- move all children from one group to another (???? in the end of the year)
- set standard price for one lesson (session)

### System requirements
- System must calculate overall sum of the lesson for the child for the month based on the presence pf the child and the price for one session

### Special terminology
- subscription - monthly payment for the child (can differ if there are two children from one family, if staff's child)

## Technical specification for NPRG035, NPRG038, NPRG064, NPRG057 and architecture considerations.
### Technologies that will be used from subjects:
- NPRG064: UI framework Avalonia for multi-platform applications. Application will be developed for Windows and Mac.
- NPRG057: Database (which exactly is still in consideration) Sever, Entity framework.
- NPRG038: Tasks, Networking (For now Refit and JSON format).
### Others (not part of particular subject):
- Resources for localization of the application (https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization/provide-resources?view=aspnetcore-8.0)

### Basic UI design (draft!!!)
![IMG_0506](https://github.com/boba-buba/alman/assets/120932204/70cbacfd-dc18-4d44-88ff-99bd4d78960b)

### Structure
- On client's laptop runs desktop application that sends HTTP requests to server. UI runs in one main thread and tasks are used to send requests, receive replies. 
- On server side there is an application that performs business logic (requests coming through internet), sends via DB API data to the Database client.
- Database client store changes to the database.

![IMG_0505](https://github.com/boba-buba/alman/assets/120932204/30f45edf-8d18-47a9-90a3-aa64650b2035)



