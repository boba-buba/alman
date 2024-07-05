CREATE schema [Alman_db]
	Authorization [dbo];

create table [alman_db].[Child] (
	ChildID int IDENTITY(1, 1) PRIMARY KEY,
	ChildLastName nvarchar(255) NOT NULL,
	ChildFirstName nvarchar(255) NOT NULL
);

alter table [Alman_db].[Child]
add Contract nvarchar(255);

alter table [Alman_db].[Child]
add ChildGroup int;

create table [Alman_db].[Activity] (
    ActivityID int IDENTITY(1, 1) PRIMARY KEY,
    ActivityName varchar(255),
    ActivityPrice money
);

create table [Alman_db].[Precontract] (
    PChildID int Foreign key references Alman_db.Child(ChildID),
    PSum money,
    Constraint PK_Precontract PRIMARY KEY (PChildID, PSum)
);

create table Precontracts (
    PChildID INTEGER,
    PSum INTEGER,
	FOREIGN KEY(PChildID) REFERENCES Children(ChildId),
    Constraint PK_Precontract PRIMARY KEY (PChildID, PSum)
);



create table [Alman_db].[YearFee] (
    YFChildID int Foreign key references Alman_db.Child(ChildID),
    YFDate date NOT NULL,
    YFSum money,
    Constraint PK_YearFee PRIMARY KEY (YFChildID, YFDate, YFSum)
);

-- Example of monthly table
create table [Alman_db].[Year_Month_Activity] (
    YMChildID int Foreign KEY references Alman_db.Child(ChildID),
    YMActivityID int Foreign KEY references Alman_db.Activity(ActivityID),
    YMActivitySum money NOT NULL,
    Constraint PK_Year_Month_Act PRIMARY KEY (YMChildID, YMActivityID, YMActivitySum)
);
---

create table [Alman_db].[Year_Subs] (
    YChildID int Foreign KEY references Alman_db.Child(ChildID),
    YDate date NOT NULL,
    YSum money,
    Constraint PK_Year_Subs PRIMARY KEY (YChildID, YDate, YSubscription)
);




