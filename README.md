# Api Test


## SQL Test Data

```sql
-- Insert test data into the Projects table
INSERT INTO Projects (Name, Description, ProjectManager) VALUES
('Project Alpha', 'Description of Project Alpha', 'Jane Doe'),
('Project Beta', 'Description of Project Beta', 'John Smith');

-- Assuming IDs are auto-generated and the first project has ID 1, and the second has ID 2
-- Insert test data into the Epics table, linking to Projects
INSERT INTO Epics (Name, Description, ProjectId) VALUES
('Epic 1', 'Description of Epic 1 in Project Alpha', 1),
('Epic 2', 'Description of Epic 2 in Project Alpha', 1),
('Epic 3', 'Description of Epic 3 in Project Beta', 2);

-- Assuming IDs are auto-generated and the first epic has ID 1, the second has ID 2, and so on
-- Insert test data into the Tasks table, linking to Epics
INSERT INTO Tasks (Name, Description, Responsible, EpicId) VALUES
('Task 1', 'Description of Task 1 in Epic 1', 'Alice', 1),
('Task 2', 'Description of Task 2 in Epic 1', 'Bob', 1),
('Task 3', 'Description of Task 3 in Epic 2', 'Charlie', 2),
('Task 4', 'Description of Task 4 in Epic 3', 'Dana', 3);
```
