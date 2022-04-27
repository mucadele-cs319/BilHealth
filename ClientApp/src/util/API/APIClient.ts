import dayjs from "dayjs";
import { Announcement, Login, Registration, User } from "./APITypes";

const authentication = {
  register: async (registration: Registration) => {
    await fetch("/api/auth/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(registration),
    });
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

const profiles = {
  me: async (): Promise<User> => {
    const response = await fetch("/api/profiles/me");
    if (!response.ok) throw Error("Couldn't get own profile. Am I authenticated?");
    return await response.json();
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
  update: async (announcement: Announcement): Promise<Announcement> => {
    const response = await fetch(`/api/announcements/${announcement.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(announcement),
    });
    const newAnnouncement: Announcement = await response.json();
    newAnnouncement.dateTime = dayjs(newAnnouncement.dateTime);
    return newAnnouncement;
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
