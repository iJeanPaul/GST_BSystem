CREATE TABLE Users (
User_Id int identity(1, 1),
User_Name varchar(50),
User_Email varchar(50),
User_Type varchar(4),
PRIMARY KEY (User_Id)
);

CREATE TABLE BadgeType(
BT_Name varchar(50),
BT_Descript varchar(300), /* may need to use a bigger # than 300*/
BT_Id int identity(1, 1),
PRIMARY KEY(BT_Id)
);

CREATE TABLE BadgeGiveType(
BGT_Name varchar(50),
BGT_Descript varchar(300),/* may need to use a bigger # than 300*/
BGT_Id int identity(1, 1),
PRIMARY KEY (BGT_Id)
);

CREATE TABLE BadgeStatus(
BS_Name varchar(20),
BS_Descript varchar(300),
BS_Id int identity(1, 1),
PRIMARY KEY (BS_Id)
);

CREATE TABLE Badge( 
Badge_Image varchar(300), /* better to store image locations instead of actual images*/
Badge_RetireDate varchar(9),
Badge_ActivateDate varchar(9),
Badge_Id int identity(1, 1),
Badge_Name varchar(50),
Badge_Descript varchar(300),/* may need to use a bigger # than 300*/
Badge_Notes varchar(300),
BadgeType int,
BadgeGiveType int,
BadgeStatus int,
PRIMARY KEY (Badge_Id),
FOREIGN KEY (BadgeType) REFERENCES BadgeType(BT_Id),
FOREIGN KEY (BadgeGiveType) REFERENCES BadgeGiveType(BGT_Id),
FOREIGN KEY (BadgeStatus) REFERENCES BadgeStatus(BS_Id)
);

CREATE TABLE BadgeTransaction(
Sender int,
Reciever int,
Badge_Id int,
BTrans_Date varchar(30),
Badge_Comment varchar(300),/* may need to use a bigger # than 300*/
FOREIGN KEY (Sender) REFERENCES Users(User_Id),
FOREIGN KEY (Reciever) REFERENCES Users(User_Id),
FOREIGN KEY (Badge_Id) REFERENCES Badge(Badge_Id)
);

/* Add badge types*/
INSERT INTO BadgeType (BT_Name, BT_Descript) VALUES ( 'Core' , 'all competencies achieved');
INSERT INTO BadgeType (BT_Name, BT_Descript) VALUES ( 'Competencies' , 'skills acquired');
INSERT INTO BadgeType (BT_Name, BT_Descript) VALUES ( 'Commendations' , 'activities that enrich');

/* Add badge give types*/
INSERT INTO BadgeGiveType (BGT_Name, BGT_Descript) VALUES ( 'Student to peer' , 'badge given from the student to student');
INSERT INTO BadgeGiveType (BGT_Name, BGT_Descript) VALUES ( 'Student to self' , 'badge a student gives him/her self');
INSERT INTO BadgeGiveType (BGT_Name, BGT_Descript) VALUES ( 'Faculty to student' , 'badge given by faculty to student');
INSERT INTO BadgeGiveType (BGT_Name, BGT_Descript) VALUES ( 'Staff to student' , 'badge given by staff to student');

/* Add badge statuses*/
INSERT INTO BadgeStatus (BS_Name, BS_Descript) VALUES ('Active','This badge is currently active and can be used');
INSERT INTO BadgeStatus (BS_Name, BS_Descript) VALUES ('DeActivated','This badge is currently deactivated and can not be used');
