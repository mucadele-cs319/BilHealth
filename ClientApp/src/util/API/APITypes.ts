import { Dayjs } from "dayjs";

export interface Announcement {
  id: string;
  dateTime: Dayjs;
  title: string;
  message: string;
}

export interface AppointmentVisit {
  id: string;
  appointmentId: string;
  notes?: string;
  bpm?: number;
  bloodPressure?: number;
  bodyTemperature?: number;
}

export enum ApprovalStatus {
  Waiting,
  Approved,
  Rejected,
}

export interface Appointment {
  id: string;
  requestedById: string;
  caseId: string;
  createdAt: Dayjs;
  dateTime: Dayjs;
  description?: string;
  approvalStatus: ApprovalStatus;
  attended: boolean;
  cancelled: boolean;
  visit?: AppointmentVisit;
}

export enum BloodType {
  APos,
  ANeg,
  BPos,
  BNeg,
  ABPos,
  ABNeg,
  OPos,
  ONeg,
}

export enum Campus {
  Main,
  East,
}

export interface CaseMessage {
  id: string;
  caseId: string;
  userId: string;
  dateTime: Dayjs;
  content: string;
}

export enum CaseSystemMessageType {
  CaseClosed,
  CaseStateUpdated,
  PrescriptionRemoved,
  AppointmentUpdated,
  AppointmentApproved,
  AppointmentRejected,
  TriageApproved,
}

export interface CaseSystemMessage {
  id: string;
  caseId: string;
  type: CaseSystemMessageType;
  dateTime: Dayjs;
  content: string;
}

export interface Prescription {
  id: string;
  caseId: string;
  dateTime: Dayjs;
  doctorUserId: string;
  item: string;
}

export enum CaseType {
  Dental,
  EarNoseThroat,
  Dermatology,
  Gynecology,
  Optometry,
  Orthopedics,
  Psychiatry,
  Radiology,
}

export enum CaseState {
  Open,
  Closed,
  WaitingTriage,
  WaitingTriageApproval,
}

export interface Case {
  id: string;
  dateTime: string;
  title: string;
  patientUserId: string;
  doctorUserId: string;
  type: CaseType;
  state: CaseState;
  messages: CaseMessage[];
  systemMessages: CaseSystemMessage[];
  prescriptions: Prescription[];
  appointments: Appointment[];
}

export interface SimpleCase {
  id: string;
  dateTime: Dayjs;
  patientUserId: string;
  doctorUserId: string;
  state: CaseState;
  messageCount: number;
}

export enum Gender {
  Unspecified,
  Male,
  Female,
}

export interface Login {
  userName: string;
  password: string;
  rememberMe: boolean;
}

export interface Registration {
  userType: string;
  userName: string;
  password: string;
  firstName: string;
  lastName: string;
  email: string;
}

export enum MedicalTestType {
  Blood,
  Urine,
  Ultrasound,
  Xray,
  Electrocardiogram,
  Tanita,
  Covid,
}

export enum NotificationType {
  CaseNewAppointment, // CaseId, AppointmentId
  CaseAppointmentTimeChanged, // CaseId, AppointmentId
  CaseAppointmentCanceled, // CaseId, AppointmentId
  CaseNewMessage, // CaseId, CaseMessageId
  CaseClosed, // CaseId
  CaseTriaged, // CaseId
  CaseDoctorChanged, // CaseId
  CaseNewPrescription, // CaseId, PrescriptionId
  TestResultNew, // TestResultId
}

export interface Notification {
  id: string;
  dateTime: Dayjs;
  userId: string;
  read: boolean;
  type: NotificationType;
  referenceId1?: string;
  referenceId2?: string;
}

export interface Vaccination {
  id: string;
  patientUserId: string;
  dateTime: Dayjs;
  type: string;
}

export interface TestResult {
  id: string;
  patientUserId: string;
  dateTime: Dayjs;
  type: MedicalTestType;
}

export interface TriageRequest {
  id: string;
  nurseUserId: string;
  doctorUserId: string;
  caseId: string;
  approvalStatus: ApprovalStatus;
}

export interface SimpleUser {
  id: string;
  userType: string;
  userName: string;
  email: string;
  firstName: string;
  lastName: string;
}

export interface User {
  id: string;
  userType: string;
  email: string;
  firstName: string;
  lastName: string;
  gender: Gender;
  dateOfBirth: Dayjs;
  bodyWeight: number;
  bodyHeight: number;
  bloodType: BloodType;
  vaccinations?: Vaccination[];
  testResults?: TestResult[];
  cases?: Case[];
  blacklisted?: boolean;
  triageRequests: TriageRequest[];
  specialization?: string;
  campus: Campus;
}
