import dayjs from "dayjs";
import { Announcement, Login, Registration, SimpleUser, User, Vaccination } from "./APITypes";

const authentication = {
  register: async (registration: Registration) => {
    const response = await fetch("/api/auth/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(registration),
    });
    if (!response.ok) throw Error(`Failed to register user!: ${response.status}`);
  },
  login: async (login: Login): Promise<boolean> => {
    const response = await fetch("/api/auth/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(login),
    });
    return (await response.json()).succeeded;
  },
  logout: async () => {
    await fetch("/api/auth/logout", {
      method: "POST",
    });
  },
  changePassword: async (currentPassword: string, newPassword: string) => {
    await fetch(`/api/auth/changepassword?currentPassword=${currentPassword}&newPassword=${newPassword}`, {
      method: "POST",
    });
  },
};

const notifications = {};

const sortVaccinations = (user: User) => {
  user.vaccinations?.forEach((vaccination) => {
    vaccination.dateTime = dayjs(vaccination.dateTime);
  });
  user.vaccinations?.sort((a, b) => (a.dateTime?.isAfter(b.dateTime) ? -1 : 1));
};

const profiles = {
  all: async (): Promise<SimpleUser[]> => {
    const response = await fetch("/api/profiles");
    return await response.json();
  },
  me: async (): Promise<User> => {
    const response = await fetch("/api/profiles/me");
    if (!response.ok) throw Error("Couldn't get own profile. Am I authenticated?");

    const user: User = await response.json();
    sortVaccinations(user);

    return user;
  },
  get: async (userId: string): Promise<User> => {
    const response = await fetch(`/api/profiles/${userId}`);

    const user: User = await response.json();
    sortVaccinations(user);

    return user;
  },
  update: async (newProfile: User): Promise<void> => {
    await fetch(`/api/profiles/${newProfile.id}`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(newProfile),
    });
  },
  blacklist: async (userId: string, state: boolean) => {
    await fetch(`/api/profiles/${userId}/blacklist?newState=${state}`, {
      method: "PUT",
    });
  },
  vaccination: {
    add: async (vaccination: Vaccination): Promise<void> => {
      await fetch(`/api/profiles/${vaccination.patientUserId}/vaccinations`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(vaccination),
      });
    },
    update: async (vaccination: Vaccination): Promise<void> => {
      await fetch(`/api/profiles/vaccinations/${vaccination.id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(vaccination),
      });
    },
    delete: async (vaccinationId: string): Promise<void> => {
      await fetch(`/api/profiles/vaccinations/${vaccinationId}`, {
        method: "DELETE",
      });
    },
  },
};

const testResults = {};

const appointments = {};

const cases = {};

const announcements = {
  get: async (): Promise<Announcement[]> => {
    const response = await fetch("/api/announcements");
    const announcements: Announcement[] = await response.json();
    announcements.forEach((announcement) => {
      announcement.dateTime = dayjs(announcement.dateTime);
    });
    announcements.sort((a, b) => (a.dateTime?.isAfter(b.dateTime) ? -1 : 1));
    return announcements;
  },
  create: async (announcementInput: Announcement): Promise<Announcement> => {
    const response = await fetch("/api/announcements", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(announcementInput),
    });
    const announcement: Announcement = await response.json();
    announcement.dateTime = dayjs(announcement.dateTime);
    return announcement;
  },
  update: async (announcement: Announcement): Promise<void> => {
    await fetch(`/api/announcements/${announcement.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(announcement),
    });
  },
  delete: async (announcementId: string): Promise<boolean> => {
    const response = await fetch(`/api/announcements/${announcementId}`, {
      method: "DELETE",
    });
    return response.ok;
  },
};

const APIClient = {
  authentication,
  notifications,
  profiles,
  testResults,
  appointments,
  cases,
  announcements,
};

export default APIClient;
