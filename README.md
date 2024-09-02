# Alman
Desktop multiplatform application for managing kindergarten (imformation system). Cycle in kindergarten is month. The administrator fills the data in the end of the month, including all incomes and exppenses and calculates revenue. Application will be developed with regard of time periods. Children are diveded to 4-5 groups based on their age. Apart from children the system manages staff, typical for kindergarten expenses like food, communal fees, advertisement, internet fees, ...

The application is developed for private kidergarten with atmost 70 children.

## Documentation
High-level documentation can be found in file [Docs](./FinishedDocumentation.md).

Code documentation can be generated. For that the user need `doxygen` to be installed. [Link](https://doxygen.nl/manual/install.html) to the installation manual.

After installing go to the root directory of the project (Doxyfile must be there) and run:

```zsh
doxygen Doxyfile
```

The folder `docs` will appear in the root directory and through file [index.html](./docs/html/index.html) in html folder the user can see code documentation.