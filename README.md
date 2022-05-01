<!-- markdownlint-disable MD033 -->
# BilHealth - *CS 319 Project*

*BilHealth* is a health center management system built as a web application.

## Team: *Mücadele*

| Member             | ID       |
| ------------------ | -------- |
| Mehmet Alper Çetin | 21902324 |
| Vedat Eren Arıcan  | 22002643 |
| Uygar Onat Erol    | 21901908 |
| Recep Uysal        | 21803637 |
| Efe Erkan          | 21902248 |

## Project Details

### User Types

There are 5 roles of distinct authorization levels, with each user falling into a single one of them.

- **Admin:** Manages the system but is not necessarily part of the health center staff.
  This is a role for developers/maintainers to do housekeeping.
- **Doctor:** Part of the medical staff that can undertake cases opened by patients.
- **Nurse:** Part of the medical staff that can record patient data and forward patients to doctors.
- **Staff:** Part of the health center staff but not for medical tasks and as such cannot accept cases.
  Mostly able to perform a subset of doctor and patient features on their behalf.
- **Patient:** Able to open cases and receive medical attention.

### Features

<details>
  <summary>Click to expand brief list of features</summary>

- Open a case with the ability to specify your concern and proceed to schedule appointments
- Post-appointment follow-up with patients through the opened case
- Send email notifications to patients regarding updates to their cases
- Services include those given by the psychology center, where applicable
- User profiles for patients (past cases, physical measurements, etc.) and doctors (specialization, etc.)
- Get medical test results online (Blood, urine, etc.)
- Prescriptions are added to cases, and [printable documents] are generated for pharmacies
- Announcements for any news such as seminars or first-aid courses
- Convert individual or campus-wide patient data to inclusive report (HTML format, but PDF compatible)
- Do basic calculations such as BMI measurements

</details>

#### Patients

- Open cases to seek medical attention by specifying concerns
- Request appointments through the opened case (Not showing up to an appointment will eventually get you blacklisted)
- Each appointment contains a visit that holds information such as heart rate, body temperature, blood pressure, etc.
- Follow-up with cases by adding messages onto the open case
- Share test results and past cases with a doctor while a case is open with them
- Have a profile with medical history and relevant information such as past cases,
  physical measurements, vaccination history, etc.
- View medical test results online as PDFs
- View announcements by the health center
- Receive on-site and/or email notifications about updates to cases
- Request a report of personal medical history (in HTML format, but PDF/print compatible)
- Get basic calculations such as BMI measurements

#### Doctors

- Receive open cases and provide medical attention online then on premise through appointments
- Open cases on behalf of patients
- Close cases when necessary
- Record notes about a patient's visit, which other doctors can see on request
  (Not the same as sensitive case and patient details, which are specifically visible to certain doctors)
- Follow-up online on patients through the cases they have opened
- Have a profile with relevant information such as area of medical expertise
- Add prescriptions to cases and generate [printable documents] for pharmacies
- Approve appointment requests in cases, or cancel approved appointments
- Make site-wide announcements
- Request inclusive anonymous report of campus-wide patient data (in HTML format, but PDF/print compatible)

#### Nurse

- Forward/triage open cases to specific doctors on the initial visit (Will need to be approved by a staff member)
- Record patient details during the visit, such as BPM/blood pressure/body temperature
- Update patient profiles

#### Staff

- View cases but without the ability to provide medical assistance
- Open cases on behalf of patients, close when necessary
- Approve triaging requests from nurses
- Update patient profiles
- Upload test result PDFs into the system
- Make site-wide announcements
- Request inclusive anonymous report of campus-wide patient data (in HTML format, but PDF/print compatible)

#### Admins

- Register new users
- Monitor system

[printable documents]: https://developer.mozilla.org/en-US/docs/Web/Guide/Printing#print_an_external_page_without_opening_it

---

#### Average Appointment Experience

1. The patient opens a case
2. They schedule their first appointment (no doctor assigned yet)
3. A nurse performs an initial examination, then, if necessary, requests to forward the case to a doctor
4. A staff member approves the nurse's triaging request, and a doctor is assigned to the case
5. The appointments are now made directly to the doctor

So in summary, the appointments are first handled by **nurse/staff**, and later by **doctor/staff**.

### Tech Stack

- **C# (ASP.NET Core)** as the back-end API platform language
- **PostgreSQL** as the database solution
- **TypeScript (ReactJS)** as the front-end SPA client language
- **Tailwind CSS** and **Material UI** for front-end styling
- **Docker (docker-compose)** can be used to make development/deployment/testing easier
- Hosting could be a **VPS** or a cloud container service from **Azure**, etc.
- **Nginx** can be used as a reverse proxy to configure HTTPS
- If feasible, simple integration tests should be done (not in-memory DB)

### Architecture

The project follows a typical SPA web app design. These are some notes on it.

- Front-end code is kept strictly in the React app
- The back-end serves React static files, which make requests to the API endpoints
- Business logic is kept in the service layer where most code should typically go (namespace `Services`)
- In alignment with separation of concerns, API controllers should be very concise (a few lines)
  and make calls to the service layer for actions
- Models should avoid having methods within, and leave such actions to the service layer
- DTOs are used for HTTP requests/responses, and for some service layer methods.
  Passing around full models is not always ideal for data protection and such
- The repository pattern is not used since (arguably) EF Core is a sufficiently similar implementation of repositories. The benefit of implementing repositories on top is not worth the effort in this case.
- Mundane utility types and methods should go under the `Utility` namespace

## Useful Links

- [Bilkent Health Center Homepage](https://w3.bilkent.edu.tr/bilkent/health-center/) (more links at the bottom)
- [Bilkent International Center - Health Care](https://w3.bilkent.edu.tr/bilkent/international-center/health-care/)
- [CS 319 Homepage](http://www.cs.bilkent.edu.tr/~eraytuzun/teaching/cs319/)
- [CS 319 Project Description](https://docs.google.com/document/d/1puvB-hY725Av7boHbbAH3WhnFuxw43weXf--gsyHZLE)

## Git Workflow

We use a simple and robust workflow where the `master` branch **only receives commits through GitHub pull requests**.
Any new commits must first be made to a new branch based on `master`, such as `readme-gitflow`,
and once the feature is ready, they can be merged through pull requests on GitHub.
Preferably, the pull requests should be **reviewed and approved** through the GitHub UI before being merged into `master`.
Each distinct feature should get its own branch and have its own pull request.

See the relevant page on [Microsoft Docs][msdn-gitflow] for more information about this workflow.

[msdn-gitflow]: https://docs.microsoft.com/en-us/azure/devops/repos/git/git-branching-guidance?view=azure-devops#keep-your-branch-strategy-simple

### Continuous Integration

The GitHub Actions CI is configured to run a build and lint modified code on each commit of a PR.
Unless the CI itself is somehow broken, try to make sure all CI jobs pass with success before merging.

In the future, if code for testing the project is added, the CI may include automated tests too.

## How to Run

The project runs best on a Linux-based environment, despite using Docker.
Through [WSL][wsl-about] and its integration with Docker, Windows is also able to run the project reasonably well.

> **Note for Windows:** You **must** place the project within the WSL filesystem.
> If it isn't already, make the entrypoint script executable with the command
> `chmod +x ./scripts/entrypoint-migrate.sh`.
> Also, WSL may eat up unnecessarily high amounts of memory when using Docker,
> so you might want to [limit it](https://github.com/microsoft/WSL/issues/4166#issuecomment-526725261).

[wsl-about]: https://docs.microsoft.com/en-us/windows/wsl/about

### Dependencies

What needs to be installed locally depends on whether you will run the *development* or *production* build.
If you use [Docker][docker-overview], and you should, then you need to install [Docker][docker-install] and [docker-compose][dockercompose-install].
This eliminates the need to install a database server locally,
and Docker by itself should actually be enough to run both builds.

For development though, you should also install locally:

- [.NET Core SDK][netcore-install], version 6.0.x (includes ASP.NET Core runtime)
- [Node.js][nodejs-install], version 16.x
- If using VS Code, install the [C# extension][csharp-ext-vscode]. Some may prefer to use [Rider][rider-ide] instead.
- For your IDE of choice, install extensions for:
  - [editorconfig]
  - [ESLint]
  - [Prettier]

The purpose of these is briefly shown in the *Tools* section below.

[docker-overview]: https://docs.docker.com/get-started/overview/
[docker-install]: https://docs.docker.com/get-docker/
[dockercompose-install]: https://docs.docker.com/compose/install/
[netcore-install]: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
[nodejs-install]: https://nodejs.org/en/download/
[csharp-ext-vscode]: https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp
[rider-ide]: https://www.jetbrains.com/rider/
[editorconfig]: https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig
[eslint]: https://marketplace.visualstudio.com/items?itemName=dbaeumer.vscode-eslint
[prettier]: https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode

### Development

The suggested workflow for development is to use docker-compose.

To run the project in development mode, first build the Docker image:

```shell
docker-compose -f docker-compose.devel.yml build
```

After it's been built for the first time, you should be able to run it thereafter with simply:

```shell
docker-compose -f docker-compose.devel.yml up
```

Once the containers are fully up, the project should be visible at `https://localhost:7257/`.
At `https://localhost:7257/swagger`, you can find the SwaggerUI documentation and test the API.

This development image mounts the project folder into the container, allowing changes to the code
to be seen in real-time.

> The HTTPS support is a little sketchy and your web browser may warn you about it,
> in which case you should add an exception for the certificate
> (ideally your browser should give you the option to do so right on the page).

### Production

docker-compose can also be used to run the project in production mode.

For the first ever time, build and run the production Docker image:

```shell
# Build image
docker-compose -f docker-compose.prod.yml build

# Perform database migration (do this only on first run or new migration)
## Bring up only the database container
docker-compose -d -f docker-compose.prod.yml up dbpostgres
## Generate idempotent SQL script and copy into container
dotnet ef migrations --idempotent -o migrate.sql
docker cp migrate.sql bilhealth_db_postgres_1:/migrate.sql
## Enter the database environment and execute migration
docker exec -it bilhealth_dbpostgres_1 bash
cd / && psql -U postgres -d bilhealthprod -f migrate.sql && exit
## Bring down database
docker-compose -f docker-compose.prod.yml down

# Bring up all containers
docker-compose -d -f docker-compose.prod.yml up
```

Once the containers are fully up, the project should be visible at `http://localhost:5000/`.

Note that unlike the development image, this one may need to be rebuilt upon every change to the project.
The database migration process needs to be done only if the database is not up to date with the latest migration.

> There is currently no support for HTTPS in the production build.
> Ideally, a reverse proxy (Nginx) should be used to support HTTPS.

### Tools

This is a general list of commands that may or may not be used throughout development.
These commands are simplified and may need to be run from specific folders or with specific arguments in reality.

- `dotnet restore`: Restores (installs) .NET dependencies for the project
- `dotnet run`: Runs the project *locally* in development mode
- `dotnet watch run`: Runs the project *locally* in development mode while watching for changes
- `dotnet build`: Builds the project binaries (this is implicitly run by other `dotnet` commands too)
- `dotnet publish`: Builds an optimized production version of the project
- `dotnet BilHealth.dll`: Runs the project binary *locally* (your CWD must be where the `wwwroot` folder is)
- `dotnet ef migrations add <commitname>`: Commits the current DB model into the `Migrations` folder
- `dotnet ef database update`: Update the database with the latest migration committed
- `dotnet format --verify-no-changes`: Check if C# is formatted correctly
- `dotnet format`: Format C# correctly

<!-- Separate lists -->
- `docker-compose build`: Build Docker images based on a `docker-compose.yml` file
- `docker-compose up`: Run built Docker images based on a `docker-compose.yml` file
- `docker-compose down`: Stop the running Docker images

<!-- Separate lists -->
- `npm install`: Install Node.js dependencies into the `node_modules` folder
- `npm install <package>`: Install a package and add it to `package.json`
- `npx prettier --check src/`: Check if client code is formatted correctly
- `npx prettier --write src/`: Format client code correctly
- `npx eslint src/*.tsx`: Lint all TypeScript files (does not correct errors)
