import CircularProgress from "@mui/material/CircularProgress";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import React, { useEffect, useState } from "react";
import APIClient from "../../util/API/APIClient";
import { Announcement } from "../../util/API/APITypes";
import { useDocumentTitle } from "../../util/CustomHooks";
import AnnouncementItem from "../AnnouncementItem";

const Announcements = () => {
  useDocumentTitle("Announcements");

  const [announcements, setAnnouncements] = useState<Announcement[]>([]);
  const [isLoaded, setIsLoaded] = useState(false);

  useEffect(() => {
    APIClient.announcements.get().then((response) => {
      setAnnouncements(response);
      setIsLoaded(true);
    });
  }, []);

  return (
    <Grid container justifyContent="center">
      <Grid item lg={10} xs={11}>
        {isLoaded ? (
          announcements.length !== 0 ? (
            announcements.map((announcement, i) => <AnnouncementItem key={i} data={announcement} className="mb-5 mx-auto" />)
          ) : (
            <Typography>No announcements at this time.</Typography>
          )
        ) : (
          <CircularProgress className="mt-8" />
        )}
      </Grid>
    </Grid>
  );
};

export default Announcements;
