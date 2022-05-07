import dayjs from "dayjs";
import { Announcement, Login, Notification, Registration, SimpleUser, TestResult, User, Vaccination } from "./APITypes";

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

const notifications = {
  get: async (unread = false): Promise<Notification[]> => {
    const response = await fetch(`/api/notifications?unread=${unread}`);
    const notifications: Notification[] = await response.json();
    notifications.forEach((notification) => {
      notification.dateTime = dayjs(notification.dateTime);
    });
    return notifications;
  },
  markRead: async (notificationId: string) => {
    await fetch(`/api/notifications/${notificationId}`, {
      method: "PATCH",
    });
  },
  markAllRead: async () => {
    await fetch("/api/notifications", {
      method: "POST",
    });
  },
};

const sortVaccinations = (user: User) => {
  user.vaccinations?.forEach((vaccination) => {
    vaccination.dateTime = dayjs(vaccination.dateTime);
  });
  user.vaccinations?.sort((a, b) => (a.dateTime?.isAfter(b.dateTime) ? -1 : 1));
};

const processDateOfBirth = (user: User) => {
  if (user.dateOfBirth) user.dateOfBirth = dayjs(user.dateOfBirth);
};

const sortTestResults = (user: User) => {
  user.testResults?.forEach((testResult) => {
    testResult.dateTime = dayjs(testResult.dateTime);
  });
  user.testResults?.sort((a, b) => (a.dateTime?.isAfter(b.dateTime) ? -1 : 1));
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
    sortTestResults(user);
    processDateOfBirth(user);

    return user;
  },
  get: async (userId: string): Promise<User> => {
    const response = await fetch(`/api/profiles/${userId}`);

    const user: User = await response.json();
    sortVaccinations(user);
    sortTestResults(user);
    processDateOfBirth(user);

    return user;
  },
  update: async (newProfile: User): Promise<void> => {
    await fetch(`/api/profiles/${newProfile.id}`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ ...newProfile, dateOfBirth: newProfile.dateOfBirth?.format("YYYY-MM-DD") }),
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

const testResults = {
  add: async (testResult: TestResult) => {
    const formData = new FormData();
    if (testResult.type === undefined) throw Error("No type given for test result");
    if (testResult.file === undefined) throw Error("No file given for test result");
    formData.append("file", testResult.file);

    await fetch(`/api/profiles/${testResult.patientUserId}/testresults?type=${testResult.type.toString()}`, {
      method: "POST",
      body: formData,
    });
  },
  update: async (testResult: TestResult) => {
    const formData = new FormData();
    if (testResult.type) formData.append("type", testResult.type.toString());
    if (testResult.file) formData.append("file", testResult.file);

    await fetch(`/api/testresults/${testResult.id}`, {
      method: "PATCH",
      body: formData,
    });
  },
  delete: async (testResultId: string) => {
    await fetch(`/api/testresults/${testResultId}`, {
      method: "DELETE",
    });
  },
  getFile: (testResultId: string) => `/api/testresults/${testResultId}/file`,
};

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
