import dayjs from "dayjs";
import {
  Announcement,
  AnnouncementUpdate,
  Appointment,
  AppointmentUpdate,
  AppointmentVisit,
  AppointmentVisitUpdate,
  ApprovalStatus,
  AuditTrail,
  Case,
  CaseCreate,
  CaseDiagnosisUpdate,
  CaseMessage,
  CaseMessageUpdate,
  CaseState,
  Login,
  MedicalTestType,
  Notification,
  Prescription,
  PrescriptionUpdate,
  Registration,
  SimpleCase,
  SimpleUser,
  TimedAccessGrantCreate,
  TriageRequest,
  User,
  UserUpdate,
  VaccinationUpdate,
} from "./APITypes";

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

const sortTimedGrants = (user: User) => {
  user.timedAccessGrants?.forEach((grant) => {
    grant.expiryTime = dayjs(grant.expiryTime);
  });
  user.timedAccessGrants?.sort((a, b) => (a.expiryTime?.isAfter(b.expiryTime) ? -1 : 1));
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
    sortTimedGrants(user);

    return user;
  },
  get: async (userId: string): Promise<User> => {
    const response = await fetch(`/api/profiles/${userId}`);

    const user: User = await response.json();
    sortVaccinations(user);
    sortTestResults(user);
    processDateOfBirth(user);
    sortTimedGrants(user);

    return user;
  },
  getSimple: async (userId: string): Promise<SimpleUser> => {
    const response = await fetch(`/api/profiles/${userId}/simple`);
    const user: SimpleUser = await response.json();
    return user;
  },
  update: async (userId: string, newProfile: UserUpdate): Promise<void> => {
    await fetch(`/api/profiles/${userId}`, {
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
    add: async (patientUserId: string, vaccination: VaccinationUpdate): Promise<void> => {
      await fetch(`/api/profiles/${patientUserId}/vaccinations`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(vaccination),
      });
    },
    update: async (vaccinationId: string, vaccination: VaccinationUpdate): Promise<void> => {
      await fetch(`/api/profiles/vaccinations/${vaccinationId}`, {
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
  accessControl: {
    grantTimedAccess: async (patientUserId: string, grant: TimedAccessGrantCreate) => {
      await fetch(`/api/profiles/${patientUserId}/accessgrants`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(grant),
      });
    },
    cancelTimedAccess: async (patientUserId: string, accessGrantId: string) => {
      await fetch(`/api/profiles/${patientUserId}/accessgrants/${accessGrantId}`, {
        method: "PATCH",
      });
    },
    getAuditTrails: async (): Promise<AuditTrail[]> => {
      const response = await fetch(`/api/profiles/audittrails`);
      const trails: AuditTrail[] = await response.json();
      trails.forEach((testResult) => {
        testResult.accessTime = dayjs(testResult.accessTime);
      });
      trails.sort((a, b) => (a.accessTime.isAfter(b.accessTime) ? -1 : 1));
      return trails;
    },
  },
};

const testResults = {
  add: async (patientUserId: string, testType: MedicalTestType, file?: File) => {
    const formData = new FormData();
    if (file === undefined) throw Error("No file given for test result");
    formData.append("file", file);

    await fetch(`/api/profiles/${patientUserId}/testresults?testType=${testType.toString()}`, {
      method: "POST",
      body: formData,
    });
  },
  update: async (testResultId: string, testType: MedicalTestType, file?: File) => {
    const formData = new FormData();
    if (file) formData.append("file", file);

    await fetch(`/api/testresults/${testResultId}?testType=${testType.toString()}`, {
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

const appointments = {
  create: async (caseId: string, details: AppointmentUpdate): Promise<Appointment> => {
    const response = await fetch(`/api/cases/${caseId}/appointment`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(details),
    });
    const appointment: Appointment = await response.json();
    appointment.dateTime = dayjs(appointment.dateTime);
    appointment.createdAt = dayjs(appointment.createdAt);
    return appointment;
  },
  update: async (appointmentId: string, details: AppointmentUpdate): Promise<Appointment> => {
    const response = await fetch(`/api/appointments/${appointmentId}`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(details),
    });
    const appointment: Appointment = await response.json();
    appointment.dateTime = dayjs(appointment.dateTime);
    appointment.createdAt = dayjs(appointment.createdAt);
    return appointment;
  },
  cancel: async (appointmentId: string) => {
    await fetch(`/api/appointments/${appointmentId}/cancel`, {
      method: "PUT",
    });
  },
  setApproval: async (appointmentId: string, approval: ApprovalStatus) => {
    await fetch(`/api/appointments/${appointmentId}/approval?approval=${approval}`, {
      method: "PUT",
    });
  },
  visits: {
    create: async (appointmentId: string, details: AppointmentVisitUpdate): Promise<AppointmentVisit> => {
      const response = await fetch(`/api/appointments/${appointmentId}/visit`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(details),
      });
      const visit: AppointmentVisit = await response.json();
      return visit;
    },
    update: async (appointmentId: string, details: AppointmentVisitUpdate) => {
      await fetch(`/api/appointments/${appointmentId}/visit`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(details),
      });
    },
  },
};

const processCaseTimes = (_case: Case) => {
  _case.dateTime = dayjs(_case.dateTime);
  _case.appointments.forEach((appointment) => {
    appointment.createdAt = dayjs(appointment.createdAt);
    appointment.dateTime = dayjs(appointment.dateTime);
  });
  _case.messages.forEach((msg) => (msg.dateTime = dayjs(msg.dateTime)));
  _case.prescriptions.forEach((prescription) => (prescription.dateTime = dayjs(prescription.dateTime)));
  _case.systemMessages.forEach((msg) => (msg.dateTime = dayjs(msg.dateTime)));
  _case.triageRequests.forEach((req) => (req.dateTime = dayjs(req.dateTime)));

  _case.messages.sort((a, b) => (a.dateTime?.isAfter(b.dateTime) ? 1 : -1));
  _case.systemMessages.sort((a, b) => (a.dateTime?.isAfter(b.dateTime) ? 1 : -1));
  _case.triageRequests.sort((a, b) => (a.dateTime?.isAfter(b.dateTime) ? -1 : 1));
};

const cases = {
  getList: async (): Promise<SimpleCase[]> => {
    const response = await fetch(`/api/cases`);
    const cases: SimpleCase[] = await response.json();

    cases.forEach((_case) => (_case.dateTime = dayjs(_case.dateTime)));
    cases.sort((a, b) => (a.dateTime?.isAfter(b.dateTime) ? -1 : 1));
    return cases;
  },
  get: async (caseId: string): Promise<Case> => {
    const response = await fetch(`/api/cases/${caseId}`);
    const _case: Case = await response.json();
    processCaseTimes(_case);
    return _case;
  },
  create: async (details: CaseCreate) => {
    const response = await fetch(`/api/cases`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(details),
    });
    const _case: Case = await response.json();
    processCaseTimes(_case);
    return _case;
  },
  changeState: async (caseId: string, state: CaseState) => {
    await fetch(`/api/cases/${caseId}?state=${state.toString()}`, {
      method: "PATCH",
    });
  },
  messages: {
    add: async (caseId: string, details: CaseMessageUpdate): Promise<CaseMessage> => {
      const response = await fetch(`/api/cases/${caseId}/messages`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(details),
      });
      const message: CaseMessage = await response.json();
      message.dateTime = dayjs(message.dateTime);
      return message;
    },
    update: async (messageId: string, details: CaseMessageUpdate): Promise<CaseMessage> => {
      const response = await fetch(`/api/cases/messages/${messageId}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(details),
      });
      const message: CaseMessage = await response.json();
      message.dateTime = dayjs(message.dateTime);
      return message;
    },
    delete: async (messageId: string) => {
      await fetch(`/api/cases/messages/${messageId}`, {
        method: "DELETE",
      });
    },
  },
  prescriptions: {
    add: async (caseId: string, details: PrescriptionUpdate): Promise<Prescription> => {
      const response = await fetch(`/api/cases/${caseId}/prescriptions`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(details),
      });
      const prescription: Prescription = await response.json();
      prescription.dateTime = dayjs(prescription.dateTime);
      return prescription;
    },
    update: async (prescriptionId: string, details: PrescriptionUpdate): Promise<Prescription> => {
      const response = await fetch(`/api/cases/prescriptions/${prescriptionId}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(details),
      });
      const prescription: Prescription = await response.json();
      prescription.dateTime = dayjs(prescription.dateTime);
      return prescription;
    },
    delete: async (prescriptionId: string) => {
      await fetch(`/api/cases/prescriptions/${prescriptionId}`, {
        method: "DELETE",
      });
    },
  },
  triageRequests: {
    create: async (caseId: string, doctorUserId: string): Promise<TriageRequest> => {
      const response = await fetch(`/api/cases/${caseId}/triagerequest?doctorUserId=${doctorUserId}`, {
        method: "POST",
      });
      const triageRequest: TriageRequest = await response.json();
      triageRequest.dateTime = dayjs(triageRequest.dateTime);
      return triageRequest;
    },
    setApproval: async (caseId: string, approval: ApprovalStatus) => {
      await fetch(`/api/cases/${caseId}/triagerequest?approval=${approval}`, {
        method: "PATCH",
      });
    },
  },
  unassign: async (caseId: string) => {
    await fetch(`/api/cases/${caseId}/unassign`, {
      method: "PATCH",
    });
  },
  diagnosis: async (caseId: string, details: CaseDiagnosisUpdate) => {
    await fetch(`/api/cases/${caseId}/diagnosis`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(details),
    });
  },
  // getReport: async (caseId: string) => {
  //   const response =
  // },
};

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
  create: async (announcementInput: AnnouncementUpdate): Promise<Announcement> => {
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
  update: async (announcementId: string, announcement: AnnouncementUpdate): Promise<void> => {
    await fetch(`/api/announcements/${announcementId}`, {
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
