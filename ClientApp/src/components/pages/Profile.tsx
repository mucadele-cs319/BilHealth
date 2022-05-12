import React, { useEffect, useState } from "react";
import { titleify, useDocumentTitle } from "../../util/CustomHooks";
import Stack from "@mui/material/Stack";
import Fade from "@mui/material/Fade";
import Grid from "@mui/material/Grid";
import CircularProgress from "@mui/material/CircularProgress";
import { TimedAccessGrant, User } from "../../util/API/APITypes";
import APIClient from "../../util/API/APIClient";
import { useParams } from "react-router-dom";
import ProfileDetails from "../profile/ProfileDetails";
import VaccinationDetails from "../profile/VaccinationDetails";
import { useUserContext } from "../UserContext";
import BlacklistCard from "../profile/BlacklistCard";
import PasswordCard from "../profile/PasswordCard";
import TestResultLinkerCard from "../profile/TestResultLinkerCard";
import { isPatient, isStaff } from "../../util/UserTypeUtil";
import TimedGrantCard from "../profile/TimedGrantCard";
import MinimalCaseListCard from "../profile/MinimalCaseListCard";

const Profile = () => {
  useDocumentTitle("Profile");

  const { user } = useUserContext();

  const params = useParams();
  const [queryUser, setQueryUser] = useState<User>();
  const [isLoaded, setIsLoaded] = useState(false);

  const refreshUser = () => {
    if (params.userid === undefined) throw Error("Couldn't get URL param for user ID");
    APIClient.profiles.get(params.userid).then((fetchedUser) => {
      setQueryUser(fetchedUser);
      document.title = titleify(`${fetchedUser.firstName}'s Profile`);
      setIsLoaded(true);
    });
  };

  useEffect(() => {
    refreshUser();
  }, [params]);

  return (
    <Grid container justifyContent="center">
      <Fade in={true}>
        <Grid item lg={10} xs={11}>
          {isLoaded && queryUser ? (
            <>
              <ProfileDetails data={queryUser as User} editable={user?.id === queryUser.id || isStaff(user)} />
              {!isPatient(queryUser) ? null : (
                <VaccinationDetails
                  readonly={!isStaff(user)}
                  refreshHandler={refreshUser}
                  patientId={queryUser.id as string}
                  vaccinations={queryUser.vaccinations}
                />
              )}
              {isPatient(queryUser) ? <TestResultLinkerCard /> : null}
              {
                isPatient(queryUser) && !isPatient(user) && queryUser.cases ? <MinimalCaseListCard cases={queryUser.cases} /> : null
              }
              {!isStaff(user) || !isPatient(queryUser) ? null : (
                <BlacklistCard
                  patientId={queryUser.id as string}
                  blacklisted={queryUser.blacklisted as boolean}
                  refreshHandler={refreshUser}
                />
              )}
              {isPatient(queryUser) && (user?.id === queryUser.id || isStaff(user)) ? (
                <TimedGrantCard
                  patientId={queryUser.id}
                  refreshHandler={refreshUser}
                  grants={queryUser.timedAccessGrants as TimedAccessGrant[]}
                />
              ) : null}
              {user?.id === queryUser.id ? <PasswordCard /> : null}
            </>
          ) : (
            <Stack alignItems="center" className="mt-8">
              <CircularProgress />
            </Stack>
          )}
        </Grid>
      </Fade>
    </Grid>
  );
};

export default Profile;
