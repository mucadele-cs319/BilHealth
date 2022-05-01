import React, { useEffect, useState } from "react";
import { titleify, useDocumentTitle } from "../../util/CustomHooks";
import Stack from "@mui/material/Stack";
import Fade from "@mui/material/Fade";
import Grid from "@mui/material/Grid";
import CircularProgress from "@mui/material/CircularProgress";
import { User, UserType } from "../../util/API/APITypes";
import APIClient from "../../util/API/APIClient";
import { useParams } from "react-router-dom";
import ProfileDetails from "../profile/ProfileDetails";
import VaccinationDetails from "../profile/VaccinationDetails";
import { useUserContext } from "../UserContext";
import BlacklistCard from "../profile/BlacklistCard";
import PasswordCard from "../profile/PasswordCard";

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
          {isLoaded ? (
            <>
              <ProfileDetails data={queryUser as User} />
              {queryUser?.userType !== UserType.Patient ? null : (
                <VaccinationDetails
                  readonly={![UserType.Staff, UserType.Admin].some((type) => type === user?.userType)}
                  refreshHandler={refreshUser}
                  patientId={queryUser.id as string}
                  vaccinations={queryUser.vaccinations}
                />
              )}
              {![UserType.Staff, UserType.Admin].some((type) => type === user?.userType) ||
              queryUser?.userType !== UserType.Patient ? null : (
                <BlacklistCard
                  patientId={queryUser.id as string}
                  blacklisted={queryUser.blacklisted as boolean}
                  refreshHandler={refreshUser}
                />
              )}
              {user?.id === queryUser?.id ? <PasswordCard /> : null}
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
