import dayjs from "dayjs";
import { Announcement, Login, Registration } from "./APITypes";

const authentication = {
  register: async (registration: Registration) => {
    await fetch("/api/auth/register", {
      method: "POST",
      body: JSON.stringify(registration),
    });
  },
  login: async (login: Login) => {
    await fetch("/api/auth/login", {
      method: "POST",
      body: JSON.stringify(login),
    });
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

const profiles = {};

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
    announcements.sort((a, b) => (a.dateTime.isAfter(b.dateTime) ? 1 : -1));
    return announcements;
  },
  create: async (announcementInput: Announcement): Promise<Announcement> => {
    const response = await fetch("/api/announcements", {
      method: "POST",
      body: JSON.stringify(announcementInput),
    });
    const announcement: Announcement = await response.json();
    announcement.dateTime = dayjs(announcement.dateTime);
    return announcement;
  },
  update: async (announcementId: string, announcementInput: Announcement): Promise<Announcement> => {
    const response = await fetch(`/api/announcements/${announcementId}`, {
      method: "PUT",
      body: JSON.stringify(announcementInput),
    });
    const announcement: Announcement = await response.json();
    announcement.dateTime = dayjs(announcement.dateTime);
    return announcement;
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
