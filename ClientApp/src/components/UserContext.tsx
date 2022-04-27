import React, { FC, useContext, useEffect, useState } from "react";
import APIClient from "../util/API/APIClient";
import { Login, User } from "../util/API/APITypes";

export interface IUserContext {
  user: User | null;
  loadingUser: boolean;
  logout: () => Promise<void>;
  login: (login: Login) => Promise<boolean>;
}

const UserContext = React.createContext<IUserContext | null>(null);

const UserContextProvider: FC = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loadingUser, setLoadingUser] = useState(true);

  useEffect(() => {
    APIClient.profiles
      .me()
      .then((self) => {
        setUser(self);
        setLoadingUser(false);
      })
      .catch((err) => {
        console.warn("UserContext: ", err);
        setLoadingUser(false);
      });
  }, []);

  const userContextValue = {
    user,
    loadingUser,
    async logout() {
      await APIClient.authentication.logout();
      setUser(null);
    },
    async login(login: Login) {
      const success = await APIClient.authentication.login(login);

      if (success) {
        const self = await APIClient.profiles.me();
        setUser(self);
      }

      return success;
    },
  };

  return <UserContext.Provider value={userContextValue}>{children}</UserContext.Provider>;
};

const useUserContext = () => {
  const ctx = useContext(UserContext);
  if (ctx === null) throw Error("UserContext is null");
  return ctx;
};

export { UserContext, UserContextProvider, useUserContext };
