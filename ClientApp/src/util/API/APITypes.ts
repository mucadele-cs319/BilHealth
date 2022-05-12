import { Dayjs } from "dayjs";
import { separateCapitalized } from "../StringUtil";

export enum UserType {
  Patient = "Patient",
  Doctor = "Doctor",
  Nurse = "Nurse",
  Admin = "Admin",
  Staff = "Staff",
}

export const getAllUserTypes = () => [
  UserType.Patient,
  UserType.Doctor,
  UserType.Nurse,
  UserType.Admin,
  UserType.Staff,
];

export interface Announcement {
  id: string;
  dateTime: Dayjs;
  title: string;
  message: string;
}

export interface AnnouncementUpdate {
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

export interface AppointmentVisitUpdate {
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

export const stringifyApproval = (approval: ApprovalStatus | undefined) => ApprovalStatus[approval || 0];

export const getAllApprovalTypes = () => [ApprovalStatus.Approved, ApprovalStatus.Rejected, ApprovalStatus.Waiting];

export interface Appointment {
  id: string;
  requestingUser: SimpleUser;
  caseId: string;
  createdAt: Dayjs;
  dateTime: Dayjs;
  description?: string;
  approvalStatus: ApprovalStatus;
  attended: boolean;
  cancelled: boolean;
  visit?: AppointmentVisit;
}

export interface AppointmentUpdate {
  dateTime: Dayjs;
  description?: string;
}

export interface AuditTrail {
  id: string;
  accessTime: Dayjs;
  accessedUser: SimpleUser;
  accessingUser: SimpleUser;
}

export enum BloodType {
  Unspecified,
  APos,
  ANeg,
  BPos,
  BNeg,
  ABPos,
  ABNeg,
  OPos,
  ONeg,
}

export const getAllBloodTypes = () => [
  BloodType.Unspecified,
  BloodType.APos,
  BloodType.ANeg,
  BloodType.BPos,
  BloodType.BNeg,
  BloodType.ABPos,
  BloodType.ABNeg,
  BloodType.OPos,
  BloodType.ONeg,
];

export const stringifyBloodType = (bloodType: BloodType | undefined) => {
  switch (bloodType) {
    case BloodType.APos:
      return "A Rh+";
    case BloodType.ANeg:
      return "A Rh-";
    case BloodType.BPos:
      return "B Rh+";
    case BloodType.BNeg:
      return "B Rh-";
    case BloodType.ABPos:
      return "AB Rh+";
    case BloodType.ABNeg:
      return "AB Rh-";
    case BloodType.OPos:
      return "O Rh+";
    case BloodType.ONeg:
      return "O Rh-";
    default:
      return "Unspecified";
  }
};

export enum Campus {
  Unspecified,
  Main,
  East,
}

export const getAllCampusTypes = () => [Campus.Unspecified, Campus.Main, Campus.East];

export const stringifyCampus = (campus: Campus | undefined) => Campus[campus || 0];

export interface CaseMessage {
  id: string;
  caseId: string;
  user: SimpleUser;
  dateTime: Dayjs;
  content: string;
}

export interface CaseMessageUpdate {
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
  doctorUser: SimpleUser;
  item: string;
}

export interface PrescriptionUpdate {
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

export const stringifyCaseType = (caseType: CaseType | undefined) => separateCapitalized(CaseType[caseType || 0]);

export const getAllCaseTypes = () => [
  CaseType.Dental,
  CaseType.EarNoseThroat,
  CaseType.Dermatology,
  CaseType.Gynecology,
  CaseType.Optometry,
  CaseType.Orthopedics,
  CaseType.Psychiatry,
  CaseType.Radiology,
];

export enum CaseState {
  Ongoing,
  Closed,
  WaitingTriage,
  WaitingTriageApproval,
}

export const stringifyCaseState = (caseState: CaseState | undefined) => separateCapitalized(CaseState[caseState || 0]);

export interface Case {
  id: string;
  dateTime: Dayjs;
  title: string;
  patientUser: SimpleUser;
  doctorUser?: SimpleUser;
  type: CaseType;
  state: CaseState;
  messages: CaseMessage[];
  systemMessages: CaseSystemMessage[];
  prescriptions: Prescription[];
  appointments: Appointment[];
  triageRequests: TriageRequest[];
  diagnosis: string | null;
}

export interface CaseCreate {
  title: string;
  patientUserId: string;
  type: CaseType;
}

export interface CaseDiagnosisUpdate {
  content: string | null;
}

export interface SimpleCase {
  id: string;
  dateTime: Dayjs;
  patientUser: SimpleUser;
  doctorUser?: SimpleUser;
  state: CaseState;
  type: CaseType;
  messageCount: number;
  title: string;
}

export enum Gender {
  Unspecified,
  Male,
  Female,
}

export const getAllGenderTypes = () => [Gender.Unspecified, Gender.Male, Gender.Female];

export const stringifyGender = (gender: Gender | undefined) => Gender[gender || 0];

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

export const getAllMedicalTestTypes = () => [
  MedicalTestType.Blood,
  MedicalTestType.Urine,
  MedicalTestType.Ultrasound,
  MedicalTestType.Xray,
  MedicalTestType.Electrocardiogram,
  MedicalTestType.Tanita,
  MedicalTestType.Covid,
];

export const stringifyMedicalTest = (medicalTestType: MedicalTestType) => MedicalTestType[medicalTestType];

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

export const stringifyNotificationType = (notificationType: NotificationType | undefined) => {
  switch (notificationType) {
    case NotificationType.CaseNewAppointment:
      return "New Appointment";
    case NotificationType.CaseAppointmentTimeChanged:
      return "Appointment Time Change";
    case NotificationType.CaseAppointmentCanceled:
      return "Appointment Canceled";
    case NotificationType.CaseNewMessage:
      return "New Message on a Case";
    case NotificationType.CaseClosed:
      return "Case Closed";
    case NotificationType.CaseTriaged:
      return "Case Triaged";
    case NotificationType.CaseDoctorChanged:
      return "Doctor Change";
    case NotificationType.CaseNewPrescription:
      return "New Prescription";
    case NotificationType.TestResultNew:
      return "New Test Result";
    default:
      return "Unspecified";
  }
};

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

export interface VaccinationUpdate {
  dateTime: Dayjs;
  type: string;
}

export interface TestResult {
  id: string;
  patientUserId: string;
  dateTime: Dayjs;
  type: MedicalTestType;
  file: File;
}

export interface TriageRequest {
  id: string;
  dateTime: Dayjs;
  requestingUser: SimpleUser;
  doctorUser: SimpleUser;
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
  // common
  id: string;
  userType: string;
  email: string;
  firstName: string;
  lastName: string;
  gender?: Gender;
  dateOfBirth?: Dayjs;
  // patient
  bodyWeight?: number;
  bodyHeight?: number;
  bloodType?: BloodType;
  blacklisted: boolean;
  vaccinations?: Vaccination[];
  testResults?: TestResult[];
  timedAccessGrants?: TimedAccessGrant[];
  // patient and doctor
  cases?: SimpleCase[];
  // doctor
  specialization?: string;
  campus?: Campus;
}

export interface UserUpdate {
  // common
  // firstName?: string;
  // lastName?: string;
  gender?: Gender;
  dateOfBirth?: Dayjs;
  // patient
  bodyWeight?: number;
  bodyHeight?: number;
  bloodType?: BloodType;
  // doctor
  specialization?: string;
  campus?: Campus;
}

export interface TimedAccessGrantCreate {
  period: string;
  patientUserId: string;
  userId: string;
}

export interface TimedAccessGrant {
  id: string;
  expiryTime: Dayjs;
  period: string;
  canceled: boolean;
  patientUserId: string;
  grantedUser: SimpleUser;
}
