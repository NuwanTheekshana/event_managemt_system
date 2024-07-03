-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jul 03, 2024 at 03:31 PM
-- Server version: 10.4.24-MariaDB
-- PHP Version: 7.4.29

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `event_management_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `event_tbl`
--

CREATE TABLE `event_tbl` (
  `Event_id` int(20) NOT NULL,
  `User_Id` int(20) NOT NULL,
  `Event_Name` varchar(255) NOT NULL,
  `Event_Description` varchar(255) DEFAULT NULL,
  `Event_Location` varchar(255) DEFAULT NULL,
  `Event_DateTime` datetime NOT NULL,
  `Ticket_Price` decimal(8,2) NOT NULL,
  `Ticket_Count` int(20) NOT NULL,
  `Commission_Rate` decimal(8,2) NOT NULL,
  `Status` int(20) NOT NULL DEFAULT 1,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `event_tbl`
--

INSERT INTO `event_tbl` (`Event_id`, `User_Id`, `Event_Name`, `Event_Description`, `Event_Location`, `Event_DateTime`, `Ticket_Price`, `Ticket_Count`, `Commission_Rate`, `Status`, `created_at`, `updated_at`) VALUES
(1, 1, 'Test', 'Test', 'Colombo', '2024-07-10 19:00:00', '2000.00', 20, '10.00', 1, '2024-07-03 08:07:56', '2024-07-03 08:21:40');

-- --------------------------------------------------------

--
-- Table structure for table `ticket_tbl`
--

CREATE TABLE `ticket_tbl` (
  `Ticket_Id` int(20) NOT NULL,
  `User_Id` int(20) NOT NULL,
  `Event_Id` int(20) NOT NULL,
  `Customer_FName` varchar(100) NOT NULL,
  `Customer_LName` varchar(100) NOT NULL,
  `Customer_NIC` varchar(20) NOT NULL,
  `Customer_Email` varchar(255) NOT NULL,
  `Customer_Tel_No` int(10) NOT NULL,
  `Status` int(20) NOT NULL DEFAULT 1,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `ticket_tbl`
--

INSERT INTO `ticket_tbl` (`Ticket_Id`, `User_Id`, `Event_Id`, `Customer_FName`, `Customer_LName`, `Customer_NIC`, `Customer_Email`, `Customer_Tel_No`, `Status`, `created_at`, `updated_at`) VALUES
(2, 1, 1, 'Customer', '1', '852147369V', 'customer@gmail.com', 712365472, 1, '2024-07-03 10:58:05', NULL),
(3, 1, 1, 'Customer', '2', '147852369V', 'customer2@gmail.com', 712586932, 1, '2024-07-03 10:58:55', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `User_Id` int(20) NOT NULL,
  `FName` varchar(100) NOT NULL,
  `LName` varchar(100) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Tel_No` int(10) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `Status` int(20) NOT NULL DEFAULT 1,
  `Permission` int(20) NOT NULL DEFAULT 1 COMMENT '1-Defult, 2-Admin',
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`User_Id`, `FName`, `LName`, `Email`, `Tel_No`, `Password`, `Status`, `Permission`, `created_at`, `updated_at`) VALUES
(1, 'Admin', 'User', 'admin@gmail.com', 771236549, 'YWRtaW5AMTIzNDVhZGVmQEBAa2ZnZ2RmamtkQA==', 1, 2, '2024-07-02 18:54:49', '2024-07-03 12:43:18'),
(2, 'Partner', 'User 1', 'partner1@gmail.com', 773256692, 'cGFydG5lckAxMjM0NWFkZWZAQEBrZmdnZGZqa2RA', 1, 1, '2024-07-03 02:46:05', '2024-07-03 12:44:34');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `event_tbl`
--
ALTER TABLE `event_tbl`
  ADD PRIMARY KEY (`Event_id`),
  ADD KEY `User_Id` (`User_Id`);

--
-- Indexes for table `ticket_tbl`
--
ALTER TABLE `ticket_tbl`
  ADD PRIMARY KEY (`Ticket_Id`),
  ADD KEY `User_Id` (`User_Id`),
  ADD KEY `Event_Id` (`Event_Id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`User_Id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `event_tbl`
--
ALTER TABLE `event_tbl`
  MODIFY `Event_id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `ticket_tbl`
--
ALTER TABLE `ticket_tbl`
  MODIFY `Ticket_Id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `User_Id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `event_tbl`
--
ALTER TABLE `event_tbl`
  ADD CONSTRAINT `event_tbl_ibfk_1` FOREIGN KEY (`User_Id`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `ticket_tbl`
--
ALTER TABLE `ticket_tbl`
  ADD CONSTRAINT `ticket_tbl_ibfk_1` FOREIGN KEY (`User_Id`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `ticket_tbl_ibfk_2` FOREIGN KEY (`Event_Id`) REFERENCES `event_tbl` (`Event_id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
