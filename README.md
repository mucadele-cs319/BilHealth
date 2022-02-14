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
- An announcements page for any news such as seminars or first-aid courses
- Convert individual or campus-wide patient data to inclusive report (HTML format, but PDF compatible)
- Do basic calculations such as BMI measurements

### Tech Stack

- **C# (ASP.NET Core)** as the back-end API platform language
- **PostgreSQL** as the database solution
- **TypeScript (ReactJS)** as the front-end SPA client language
- **Tailwind CSS** for front-end styling
- **Docker (docker-compose)** may be used to make development/deployment/testing easier
- Hosting could be a **VPS** or a cloud service such as **Azure**
- If testing is viable, **Moq** or **Playwright** may be used for unit tests and integration tests respectively

## Useful Links

- [Bilkent Health Center Homepage](https://w3.bilkent.edu.tr/bilkent/health-center/) (more links at the bottom)
- [Bilkent International Center - Health Care](https://w3.bilkent.edu.tr/bilkent/international-center/health-care/)
- [CS 319 Homepage](http://www.cs.bilkent.edu.tr/~eraytuzun/teaching/cs319/)
- [CS 319 Project Description](https://docs.google.com/document/d/1puvB-hY725Av7boHbbAH3WhnFuxw43weXf--gsyHZLE)

## Git Workflow

We use a simple and robust workflow where the `master` branch **only receives commits through GitHub pull requests**.
Any new commits must first be made to a new branch based on `master`, such as `readme-gitflow`, and once the feature is ready, they can be merged through pull requests on GitHub.
Preferably, the pull requests should be **reviewed and approved** through the GitHub UI before being merged into `master`. Each distinct feature should get its own branch and have its own pull request.

See the relevant page on [Microsoft Docs](https://docs.microsoft.com/en-us/azure/devops/repos/git/git-branching-guidance?view=azure-devops#keep-your-branch-strategy-simple) for more information about this workflow.
