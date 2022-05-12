import { SimpleUser, User } from "./API/APITypes";
import React from "react";
import Link from "@mui/material/Link";
import { Link as RLink } from "react-router-dom";

export const separateCapitalized = (input: string): string => input.split(/(?=[A-Z])/g).join(" ");

export const fullNameify = (user?: User | SimpleUser): string =>
  user ? `${user.firstName} ${user.lastName}` : ".....";

export const linkUser = (user?: User | SimpleUser, content?: string) =>
  <Link component={RLink} to={`/profiles/${user?.id}`}>{content || fullNameify(user)}</Link>;

export const fmtDateOnly = "DD/MM/YYYY";
export const fmtConcise = "DD/MM/YYYY, HH:mm";
export const fmtReadableConcise = "D MMM YYYY [at] H:mm";
export const fmtReadableDetailed = "dddd, D MMM YYYY [at] H:mm";
