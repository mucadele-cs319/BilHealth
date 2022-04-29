import React, { useEffect, useState } from "react";
import { useDocumentTitle } from "../../util/CustomHooks";
import Stack from "@mui/material/Stack";
import Fade from "@mui/material/Fade";
import Grid from "@mui/material/Grid";
import CircularProgress from "@mui/material/CircularProgress";
import { User } from "../../util/API/APITypes";
import APIClient from "../../util/API/APIClient";
import { useParams } from "react-router-dom";
import ProfileDetails from "../profile/ProfileDetails";

const Profile = () => {
  useDocumentTitle("Profile");

  const params = useParams();
  const [queryUser, setQueryUser] = useState<User>();
  const [isLoaded, setIsLoaded] = useState(false);

  useEffect(() => {
    if (params.userid === undefined) throw Error("Couldn't get URL param for user ID");
    APIClient.profiles.get(params.userid).then(user => {
      setQueryUser(user);
      document.title = `${user.firstName}'s ${document.title}`;
      setIsLoaded(true);
    });
  }, [params]);

  return (
    <Grid container justifyContent="center">
      <Fade in={true}>
        <Grid item lg={10} xs={11}>
          {isLoaded ? (
            <ProfileDetails data={queryUser as User} />
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
