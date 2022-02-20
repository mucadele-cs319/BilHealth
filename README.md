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

### Features

- Open a case with the ability to specify your concern and proceed to schedule appointments
- Post-appointment follow-up with patients through the opened case
- Send email notifications to patients regarding updates to their cases
- Services include those given by the psychology center, where applicable
- User profiles for patients (past cases, physical measurements, etc.) and doctors (specialization, etc.)
- Specialized dashboards for patients and health center employees where cases can be opened or browsed.
- Get medical test results online (Blood, urine, etc.)
- Prescriptions are added to cases, and [printable documents] are generated for pharmacies
- An announcements page for any news such as seminars or first-aid courses
- Convert individual or campus-wide patient data to inclusive report (HTML format, but PDF compatible)
- Do basic calculations such as BMI measurements

[printable documents]: https://developer.mozilla.org/en-US/docs/Web/Guide/Printing#print_an_external_page_without_opening_it

### Tech Stack

- **C# (ASP.NET Core)** as the back-end API platform language
- **PostgreSQL** as the database solution
- **TypeScript (ReactJS)** as the front-end SPA client language
- **Tailwind CSS** for front-end styling
- **Docker (docker-compose)** can be used to make development/deployment/testing easier
- Hosting could be a **VPS** or a cloud service such as **Azure**
- If testing is viable, **Moq** or **Playwright** may be used for unit tests and integration tests respectively

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

So far, the information below has only been tested on Arch Linux.
Running on Windows should ideally not be any different though.

### Dependencies

What needs to be installed locally depends on whether you will run the *development* or *production* build.
If you use [Docker][docker-overview], and you should, then you need to install [Docker][docker-install] and [docker-compose][dockercompose-install].
This eliminates the need to install a database server locally,
and Docker by itself should actually be enough to run both builds.

For development though, you should also install locally:

- [.NET Core SDK][netcore-install], version 6.0.x (includes ASP.NET Core runtime)
- [Node.js][nodejs-install], version 16.x
- If using VS Code, install the [C# extension][csharp-ext-vscode]. Some may prefer to use [Rider][csharp-ext-vscode] instead.
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

This development image mounts the project folder into the container, allowing changes to the code
to be seen in real-time.

> The HTTPS support is a little sketchy and your web browser may warn you about it,
> in which case you should add an exception for the certificate
> (ideally your browser should give you the option to do so right on the page).

### Production

docker-compose can also be used to run the project in production mode.

Build and run the production Docker image:

```shell
docker-compose -f docker-compose.prod.yml build
docker-compose -f docker-compose.prod.yml up
```

Once the containers are fully up, the project should be visible at `http://localhost:5000/`.

Note that unlike the development image, this one may need to be rebuilt upon every change to the project.

> The production image doesn't have any way to perform EF Core database migrations at the moment.
> This might make the PostgreSQL server connection fail, and a good solution for migrations needs to be found.
> The best bet is to use an SQL script generated by:
> `dotnet ef migrations script --idempotent -o migrate.sql`

<!-- Separate blockquotes -->
> There is currently no support for HTTPS in the production build.
> It might make sense to use a reverse-proxy for that purpose, some time in the future.

### Tools

This is a general list of commands that may be used throughout development.
These commands are simplified and may need to be run from specific folders in reality.

- `dotnet restore`: Restores (installs) .NET dependencies for the project
- `dotnet run`: Runs the project *locally* in development mode
- `dotnet watch run`: Runs the project *locally* in development mode while watching for changes
- `dotnet build`: Builds the project binaries (this is implicitly run by other `dotnet` commands too)
- `dotnet publish`: Builds an optimized production version of the project
- `dotnet BilHealth.dll`: Runs the project binary *locally* (your CWD must be where the `wwwroot` folder is)
- `dotnet ef migrations add <commitname>`: Commits the current DB model into the `Migrations` folder
- `dotnet ef database update`: Update the database with the latest migration committed

<!-- Separate lists -->
- `docker-compose build`: Build Docker images based on a `docker-compose.yml` file
- `docker-compose up`: Run built Docker images based on a `docker-compose.yml` file
- `docker-compose down`: Stop the running Docker images

<!-- Separate lists -->
- `npm install`: Install Node.js dependencies into the `node_modules` folder
- `npm install <package>`: Install a package and add it to `package.json`
- `npx prettier --check src/`: Check if everything is formatted correctly
- `npx prettier --write src/`: Format everything correctly
- `npx eslint src/*.tsx`: Lint all TypeScript files (does not correct errors)
