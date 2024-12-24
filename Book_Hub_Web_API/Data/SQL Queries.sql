use BookHubDB;
go

INSERT INTO Genres (Name, Description)
VALUES
    ('Action', 'High-energy genre that includes intense physical activity, violence, and fast-paced storylines, often featuring heroic characters overcoming great obstacles.'),
    ('Adventure', 'Focuses on exciting, often dangerous quests or journeys, usually with a strong emphasis on exploration, discovery, and overcoming challenges.'),
    ('Comedy', 'Designed to entertain and provoke laughter through humor, exaggeration, and often absurd situations or witty dialogues.'),
    ('Drama', 'Centered on realistic storytelling, focusing on emotional conflicts, character development, and interpersonal relationships with serious or thought-provoking themes.'),
    ('Horror', 'Aimed at eliciting fear, anxiety, or disgust through suspense, supernatural elements, or graphic violence, often involving dark themes and creatures.'),
    ('Fantasy', 'Involves magical or supernatural elements, often set in fictional worlds with mythical creatures, wizards, and heroic quests, blending escapism with imaginative storytelling.'),
    ('Mystery', 'Focuses on solving a puzzle or crime, with a strong emphasis on investigation, suspense, and revealing hidden truths or solving complex problems.'),
    ('Romance', 'Revolves around romantic relationships, focusing on the development of love between characters, often with emotional, dramatic, or heartfelt moments.'),
    ('Sci-Fi', 'Explores futuristic or scientific concepts, including advanced technology, space travel, and speculative theories, often set in future or alternative realities.'),
    ('Thriller', 'Designed to keep the audience on the edge of their seat through suspenseful plots, unexpected twists, and high-stakes scenarios that involve danger or intense tension.');
go

select * from Genres;
go

INSERT INTO Books (Isbn, Title, Author, Publication, PublishedDate, Edition, Language, Description, Cost, AvailableQuantity, TotalQuantity, GenreId)
VALUES
    ('978-3-16-148410-0', 'The Adventure of Tomorrow', 'John Doe', 'Future Publications', '2023-01-15', '1st Edition', 'English', 'A thrilling journey into the future of humanity.', 19.99, 50, 50, 1),
    ('978-1-23-456789-0', 'Mystery of the Lost City', 'Jane Smith', 'Mystery Books Ltd.', '2022-11-22', '2nd Edition', 'English', 'An ancient city’s secrets are uncovered through a gripping mystery.', 14.99, 30, 30, 7),
    ('978-0-12-345678-9', 'Romance in Paris', 'Emily Brown', 'Romantic Reads', '2021-05-10', '1st Edition', 'English', 'A heartwarming romance set against the backdrop of Paris.', 9.99, 100, 100, 8),
    ('978-3-16-222222-3', 'Space Odyssey: A New Beginning', 'Michael Taylor', 'Galactic Press', '2025-07-01', '1st Edition', 'English', 'Explore the unknown in this new chapter of space exploration.', 24.99, 20, 20, 9),
    ('978-1-89-765432-1', 'The Haunted Forest', 'Sarah Johnson', 'Horror Tales', '2024-03-18', '1st Edition', 'English', 'A terrifying encounter in an eerie, haunted forest.', 18.99, 40, 40, 5),
    ('978-0-99-876543-2', 'Fantasy Realms', 'Mark Lee', 'Fantasy World Publishers', '2023-12-05', '1st Edition', 'English', 'Step into a world where magic and mythical creatures reign supreme.', 22.99, 60, 60, 6),
    ('978-1-11-234567-8', 'The Drama of Life', 'Rebecca White', 'Life Stories Press', '2020-08-25', '3rd Edition', 'English', 'A collection of poignant life stories and personal experiences.', 15.99, 70, 70, 4),
    ('978-1-55-555555-0', 'The Ultimate Guide to Adventure', 'James Green', 'Explorer’s Press', '2022-10-15', '1st Edition', 'English', 'A guidebook for thrill-seekers and adventurers alike.', 29.99, 50, 50, 2),
    ('978-3-88-234567-4', 'The Comedy of Errors', 'Anna Roberts', 'Funny Books Inc.', '2021-06-19', '1st Edition', 'English', 'A hilarious tale of misunderstandings and absurd situations.', 12.99, 80, 80, 3),
    ('978-0-34-876543-6', 'The Great Drama of History', 'Peter King', 'Historical Press', '2019-02-12', '2nd Edition', 'English', 'Dive into the complex and dramatic history of humanity.', 20.99, 30, 30, 4);
go

select * from Books;
go

select * from Books;
select * from Genres;
select * from Users;
select * from LogUserActivity where LogId>60;
select * from Borrowed;
select * from Fines;
select * from Notifications;
select * from Reservations;
select * from ContactUs;
go

delete from Books where BookId>10;

update Users set Role='Consumer' where UserId=10;
update Users set Role='Administrator' where UserId=11;
