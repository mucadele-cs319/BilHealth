import { User, UserType } from "./API/APITypes";

type UserNullable = User | null | undefined;

export const isStaff = (user: UserNullable): user is User =>
  user != null && [UserType.Staff, UserType.Admin].some((type) => type === user.userType);
export const isPatient = (user: UserNullable): user is User => user != null && user.userType === UserType.Patient;
