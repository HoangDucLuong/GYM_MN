USE master
GO

CREATE DATABASE GYM_MN
GO

USE GYM_MN
GO

CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY,
    RoleName NVARCHAR(50) NOT NULL
);
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY,
    Username VARCHAR(50) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    RoleID INT,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);
-- Tạo bảng Loại thành viên
CREATE TABLE MembershipTypes (
    MembershipTypeID INT PRIMARY KEY IDENTITY,
    TypeName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    Price DECIMAL(10,2) NOT NULL
);

-- Tạo bảng Hội viên
CREATE TABLE Members (
    MemberID INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Gender NVARCHAR(10),
    DOB DATETIME,
    [Address] NVARCHAR(255),
    MembershipTypeID INT,
    JoinDate DATETIME,
    FOREIGN KEY (MembershipTypeID) REFERENCES MembershipTypes(MembershipTypeID)
);
ALTER TABLE Members
ADD UserID INT;

-- Tạo bảng Huấn luyện viên
CREATE TABLE Trainers (
    TrainerID INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100) NOT NULL,
	DOB DATETIME,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Gender NVARCHAR(10),
    Specialization NVARCHAR(100)
);

-- Tạo bảng Đặt chỗ
CREATE TABLE Bookings (
    BookingID INT PRIMARY KEY IDENTITY,
    MemberID INT,
    TrainerID INT,
    BookingDate DATETIME,
    StartDate DATETIME,
    EndDate DATETIME,
    Status NVARCHAR(20),
    FOREIGN KEY (MemberID) REFERENCES Members(MemberID),
    FOREIGN KEY (TrainerID) REFERENCES Trainers(TrainerID)
);

	ALTER TABLE Bookings
ADD CONSTRAINT FK_Trainers_Bookings FOREIGN KEY (TrainerID) REFERENCES Trainers(TrainerID);

-- Tạo bảng Thanh toán
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY,
    MemberID INT,
    PaymentDate DATETIME,
    Amount DECIMAL(10,2),
    PaymentMethod NVARCHAR(50),
    Status NVARCHAR(20),
    FOREIGN KEY (MemberID) REFERENCES Members(MemberID)
);

ALTER TABLE Members
ADD CONSTRAINT FK_Users_Members FOREIGN KEY (UserID) REFERENCES Users(UserID);

ALTER TABLE Trainers
ADD UserID INT;
ALTER TABLE Trainers
ADD CONSTRAINT FK_Users_Trainers FOREIGN KEY (UserID) REFERENCES Users(UserID);

-- Tạo bảng Feedback
CREATE TABLE Feedback (
    FeedbackID INT PRIMARY KEY IDENTITY,
    MemberID INT,
    TrainerID INT,
    FeedbackDate DATETIME,
    Rating INT, -- Điểm đánh giá
    Comment NVARCHAR(MAX), -- Bình luận
    FOREIGN KEY (MemberID) REFERENCES Members(MemberID),
    FOREIGN KEY (TrainerID) REFERENCES Trainers(TrainerID)
);

ALTER TABLE Users
ALTER COLUMN Password VARCHAR(255) NOT NULL;
ALTER TABLE Trainers
ADD WorkStartTime TIME,
    WorkEndTime TIME;

	ALTER TABLE Bookings
ADD MembershipTypeID INT;

ALTER TABLE Bookings
ADD CONSTRAINT FK_MembershipTypes_Bookings FOREIGN KEY (MembershipTypeID) REFERENCES MembershipTypes(MembershipTypeID);

ALTER TABLE Payments
ADD MembershipTypeID INT;

ALTER TABLE Payments
ADD CONSTRAINT FK_MembershipTypes_Payments FOREIGN KEY (MembershipTypeID) REFERENCES MembershipTypes(MembershipTypeID);
