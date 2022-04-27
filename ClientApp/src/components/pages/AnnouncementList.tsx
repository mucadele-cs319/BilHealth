import CircularProgress from "@mui/material/CircularProgress";
import Grid from "@mui/material/Grid";
import Fade from "@mui/material/Fade";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useEffect, useState } from "react";
import APIClient from "../../util/API/APIClient";
import { Announcement } from "../../util/API/APITypes";
import { useDocumentTitle } from "../../util/CustomHooks";
import AnnouncementItem from "../AnnouncementItem";
import AnnouncementItemEditable from "../AnnouncementItemEditable";
import { useUserContext } from "../UserContext";

interface Props {
  readonly?: boolean;
}

const Announcements = ({ readonly = false }: Props) => {
  useDocumentTitle("Announcements");

  const { user } = useUserContext();

  const [announcements, setAnnouncements] = useState<Announcement[]>([]);
  const [isLoaded, setIsLoaded] = useState(false);

  const refreshAnnouncements = () => {
    APIClient.announcements.get().then((response) => {
      setAnnouncements(response);
      setIsLoaded(true);
    });
  };

  useEffect(() => {
    refreshAnnouncements();
  }, []);

  return (
    <Grid container justifyContent="center">
      <Fade in={true}>
        <Grid item lg={10} xs={11}>
          {readonly || user?.userType === "Patient" ? null : (
            <AnnouncementItemEditable changeHandler={refreshAnnouncements} />
          )}
          {isLoaded ? (
            announcements.length !== 0 ? (
              announcements.map((announcement, i) => (
                <AnnouncementItem
                  key={i}
                  data={announcement}
                  className="mb-5 mx-auto"
                  readonly={readonly}
                  changeHandler={refreshAnnouncements}
                />
              ))
            ) : (
              <Stack alignItems="center" className="mt-8">
                <Typography color="text.secondary">No announcements at this time.</Typography>
              </Stack>
            )
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

export default Announcements;
