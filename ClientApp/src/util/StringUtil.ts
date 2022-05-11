import { SimpleUser, User } from "./API/APITypes";

export const separateCapitalized = (input: string): string => input.split(/(?=[A-Z])/g).join(" ");

export const fullNameify = (user?: User | SimpleUser): string =>
  user ? `${user.firstName} ${user.lastName}` : ".....";
