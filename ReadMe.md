# Smurf Village Statistical Office Task

## About the Organization

The Smurf Village Statistical Office maintains statistical records about the smurfs, mushroom houses, workplaces, and leisure venues in Smurf Village. The office is soon to receive a new website, but development has been progressing slowly. As an external contractor, you have been asked to help speed up this development process.

## Data Structure

### Smurfs 
Each smurf has the following data:

- Id: Unique identifier
- Name: The smurf’s name
- Age: The smurf’s age
- Job: The smurf’s job
- Food: The smurf’s favorite food
- Brand: The smurf’s favorite brand
- Color: The smurf’s favorite color

### Mushroom Houses

Some smurfs live in mushroom houses, which contain the following data:

- Id: Unique identifier
- Residents: List of smurfs living in the house
- Capacity: The capacity of the mushroom house
- Color: The main color of the house
- Motto: A motto that the residents of the house feel especially connected to

### Working places

Some smurfs have jobs and work at workplaces, which include the following data:

- Id: Unique identifier
- Name: The name of the workplace
- Employees: List of smurfs working there
- AcceptedJobs: List of jobs accepted by the workplace


### Leisure venues

Some smurfs like to relax at leisure venues, which include the following data:

- Id: Unique identifier
- Name: The name of the leisure venue
- Members: List of club member smurfs at the venue
- AcceptedBrand: The brand accepted by the leisure venue

### Jobs

- An enum that contains the possible jobs

### Brands

- An enum that contains the possible brands

### Food

- An enum that contains the possible foods

## About Smurf Village

The residents of Smurf Village are very conscientious and self-respecting smurfs. Their favorite color is extremely important to them; to avoid diminishing their enthusiasm for it, they will not move into a mushroom house whose main color matches their own favorite color. Since the leadership values order, smurfs can only work at workplaces where their qualification is listed among the accepted jobs.

When it comes to leisure, smurfs tend to show a bit of snobbish behavior: they will only party at bars and with other smurfs who share the same brand preferences.

While the smurf leadership appreciates variety, it does so just enough to provide a broad selection of favorite foods, colors, jobs, and brands for the smurfs to choose from.


## Tasks

For both the frontend and the backend, work should be done in a fork of the original repository. The work should also be documented in some way — either through commit messages or separate documentation.

Implement your work in a way that allows for easy future expansion.

### Frontend

According to the original developer, the backend is considered complete since all data in the database can be queried. However, there is one notable flaw: individual entities cannot be queried separately — only all entities together in a single batch. (In a world burdened with AI, we should perhaps be grateful for even this much.)
On the positive side, it has been successfully implemented so that there is no eager loading — that is, if an object references other objects, it only does so by their IDs.

#### Task 1

When the UI loads, fetch all available data from the backend, and based on the IDs, link the data together so that the frontend-created objects reference each other accordingly.

#### Task 2

Design a user-friendly and visually appealing dashboard where the loaded data can be viewed from various perspectives. You are free to define the different views, but aim for maximum functionality. It is strongly recommended to create a summary view, as well as separate tabs for each entity type and their respective statistics.


### Backend

According to the original developer, the backend is considered complete since all data in the database can be queried. However, there is one notable flaw: individual entities cannot be queried separately — only all entities together in a single batch. (In a world burdened with AI, we should perhaps be grateful for even this much.)
On the positive side, it has been successfully implemented so that there is no eager loading — that is, if an object references other objects, it only does so by their IDs.

#### Task 1

Of course, this cannot remain as-is. Refactor the backend code to allow for more flexible data retrieval. You may also implement additional features, such as search functionality. The original endpoints should be preserved, but any additional ones are welcomed.

#### Task 2

Create a service responsible for exporting the dwarves' data into a neatly formatted .txt file.