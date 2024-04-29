# ALMAN
Desctop application for managing kindergarten. Cycle in kindergarten is month. The administrator fills the data in the end of the month, including all incomes and exppenses and calculates revenue.
Application will be developed with regard of time. Children are diveded to 4-5 groups based on their age.

The application is developed for private kidergarten with atmost 80 children. In the kindergarten lessons are taught. 
Lesons are divided to two categories: 
- ones which subscription doesnt cover. They must be paid separetely
- ones which subscription cover and they dont need to paid.

## Sources of income
- Subscription
- Lesons that must be paid
- Doctor payments
- Contribution fee

## Types of children
- contract
- preliminary contract

## User stories
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

## System requirements
- System must calculate overall sum of the lesson for the child for the month based on the presence pf the child and the price for one session

## Special terminology
- subscription - monthly payment for the child (can differ if there are two children from one family, if staff's child)
